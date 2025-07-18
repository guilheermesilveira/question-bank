using AutoMapper;
using QuestionBank.Application.Contracts.Services;
using QuestionBank.Application.DTOs.Test;
using QuestionBank.Application.Notifications;
using QuestionBank.Domain.Entities;
using QuestionBank.Domain.Enums;
using QuestionBank.Domain.Validators;
using QuestionBank.Infra.Contracts.Repositories;

namespace QuestionBank.Application.Services;

public class TestService : BaseService, ITestService
{
    private readonly ITestRepository _testRepository;
    private readonly IUserRepository _userRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly ITopicRepository _topicRepository;

    public TestService(
        INotificator notificator,
        IMapper mapper,
        ITestRepository testRepository,
        IUserRepository userRepository,
        IQuestionRepository questionRepository,
        ITopicRepository topicRepository
    ) : base(notificator, mapper)
    {
        _testRepository = testRepository;
        _userRepository = userRepository;
        _questionRepository = questionRepository;
        _topicRepository = topicRepository;
    }

    public async Task<TestDto?> Create(CreateTestDto dto)
    {
        if (!await ValidationsToCreate(dto))
            return null;

        var questions = await _questionRepository.GetRandom(dto.TopicIds, dto.Difficulty, dto.TotalQuestions);

        var test = new Test
        {
            Title = dto.Title,
            TotalQuestions = dto.TotalQuestions,
            NumberOfCorrectAnswers = 0,
            Status = ETestStatus.InProgress,
            UserId = dto.UserId,
            TestQuestions = questions.Select(q => new TestQuestion
            {
                QuestionId = q.Id,
                IsCorrect = null
            }).ToList()
        };
        _testRepository.Add(test);

        return await CommitChanges() ? Mapper.Map<TestDto>(test) : null;
    }

    public async Task<TestDto?> Finish(FinishTestDto dto)
    {
        if (!await ValidationsToFinish(dto))
            return null;

        var test = await _testRepository.GetById(dto.TestId);

        foreach (var answer in dto.Answers)
        {
            var testQuestion = test!.TestQuestions.FirstOrDefault(tq => tq.QuestionId == answer.QuestionId);
            var option = testQuestion!.Question.Options.FirstOrDefault(o => o.Id == answer.SelectedOptionId);

            testQuestion.SelectedOptionId = option!.Id;
            testQuestion.IsCorrect = option.IsCorrect;
        }

        test!.NumberOfCorrectAnswers = test.TestQuestions.Count(tq => tq.IsCorrect == true);
        test.Status = ETestStatus.Finished;
        _testRepository.Update(test);

        return await CommitChanges() ? Mapper.Map<TestDto>(test) : null;
    }

    public async Task Cancel(int id)
    {
        var test = await _testRepository.GetById(id);
        if (test == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        test.Status = ETestStatus.Canceled;
        _testRepository.Update(test);
        await CommitChanges();
    }

    public async Task<TestDto?> GetById(int id)
    {
        var test = await _testRepository.GetById(id);
        if (test != null)
            return Mapper.Map<TestDto>(test);

        Notificator.HandleNotFoundResource();
        return null;
    }

    private async Task<bool> ValidationsToCreate(CreateTestDto dto)
    {
        var test = Mapper.Map<Test>(dto);
        var validator = new TestValidator();
        var result = await validator.ValidateAsync(test);
        if (!result.IsValid)
        {
            Notificator.Handle(result.Errors);
            return false;
        }

        if (dto.Difficulty != EDifficultyLevel.Easy && dto.Difficulty != EDifficultyLevel.Medium &&
            dto.Difficulty != EDifficultyLevel.Hard)
        {
            Notificator.Handle("Difficulty must be either Easy or Medium or Hard");
            return false;
        }

        if (dto.TopicIds.Count == 0)
        {
            Notificator.Handle("TopicIds List cannot be empty");
            return false;
        }

        foreach (var topicId in dto.TopicIds)
        {
            var topic = await _topicRepository.GetById(topicId);
            if (topic == null)
            {
                Notificator.Handle("In the topic list, there is an invalid topic");
                return false;
            }
        }

        var user = await _userRepository.GetById(dto.UserId);
        if (user == null)
        {
            Notificator.Handle("User not found");
            return false;
        }

        var questions = await _questionRepository.GetRandom(dto.TopicIds, dto.Difficulty, dto.TotalQuestions);
        if (questions.Count < dto.TotalQuestions)
        {
            Notificator.Handle("There are not enough questions for the given quantity");
            return false;
        }

        return true;
    }

    private async Task<bool> ValidationsToFinish(FinishTestDto dto)
    {
        var test = await _testRepository.GetById(dto.TestId);
        if (test == null)
        {
            Notificator.Handle("Test not found");
            return false;
        }

        if (test.Status == ETestStatus.Finished)
        {
            Notificator.Handle("Test already finished");
            return false;
        }

        if (test.Status == ETestStatus.Canceled)
        {
            Notificator.Handle("Test already canceled");
            return false;
        }

        if (dto.Answers.Count == 0)
        {
            Notificator.Handle("Answers List cannot be empty");
            return false;
        }

        foreach (var answer in dto.Answers)
        {
            var testQuestion = test.TestQuestions.FirstOrDefault(tq => tq.QuestionId == answer.QuestionId);
            if (testQuestion == null)
            {
                Notificator.Handle("In the list, there is an invalid question");
                return false;
            }

            var option = testQuestion.Question.Options.FirstOrDefault(o => o.Id == answer.SelectedOptionId);
            if (option == null)
            {
                Notificator.Handle("In the list, there is an invalid option");
                return false;
            }
        }

        return true;
    }

    private async Task<bool> CommitChanges()
    {
        if (await _testRepository.UnitOfWork.Commit())
            return true;

        Notificator.Handle("An error occurred while saving changes");
        return false;
    }
}