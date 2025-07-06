using AutoMapper;
using QuestionBank.Application.DTOs.Test;
using QuestionBank.Application.Notifications;
using QuestionBank.Domain.Entities;
using QuestionBank.Domain.Enums;
using QuestionBank.Infra.Contracts.Repositories;

namespace QuestionBank.Application.Services;

public class TestService : BaseService
{
    private readonly ITestRepository _testRepository;
    private readonly IUserRepository _userRepository;
    private readonly IQuestionRepository _questionRepository;

    public TestService(
        INotificator notificator,
        IMapper mapper,
        ITestRepository testRepository,
        IUserRepository userRepository,
        IQuestionRepository questionRepository
    ) : base(notificator, mapper)
    {
        _testRepository = testRepository;
        _userRepository = userRepository;
        _questionRepository = questionRepository;
    }

    public async Task<TestDto?> Create(AddTestDto dto)
    {
        var user = await _userRepository.GetById(dto.UserId);
        if (user == null)
        {
            Notificator.Handle("User not found");
            return null;
        }

        var questions = await _questionRepository.GetRandom(dto.TopicIds, dto.Difficulty, dto.TotalQuestions);
        if (questions.Count < dto.TotalQuestions)
        {
            Notificator.Handle("There are not enough questions for the given quantity");
            return null;
        }

        var test = new Test
        {
            Title = dto.Title,
            TotalQuestions = dto.TotalQuestions,
            NumberOfCorrectAnswers = 0,
            Status = ETestStatus.InProgress,
            UserId = dto.UserId,
            TestQuestions = questions.Select(q => new TestQuestion
            {
                QuestionId = q.Id
            }).ToList()
        };
        _testRepository.Add(test);

        return await CommitChanges() ? Mapper.Map<TestDto>(test) : null;
    }

    public async Task AnswerQuestion(int testId, int questionId, int alternativeId)
    {
        var test = await _testRepository.GetById(testId);
        if (test == null)
        {
            Notificator.Handle("Test not found");
            return;
        }

        if (test.Status != ETestStatus.InProgress)
        {
            Notificator.Handle("Test already answered");
            return;
        }

        var testQuestion = test.TestQuestions.FirstOrDefault(tq => tq.QuestionId == questionId);
        if (testQuestion == null)
        {
            Notificator.Handle("Question doesn't belong to test");
            return;
        }

        var alternative = testQuestion.Question.Alternatives.FirstOrDefault(a => a.Id == alternativeId);
        if (alternative == null)
        {
            Notificator.Handle("Alternative not found for this question");
            return;
        }

        testQuestion.SelectedAlternativeId = alternative.Id;
        testQuestion.IsCorrect = alternative.IsCorrect;
        _testRepository.Update(test);

        await CommitChanges();
    }

    public async Task<TestDto?> Finish(int testId)
    {
        var test = await _testRepository.GetById(testId);
        if (test == null)
        {
            Notificator.Handle("Test not found");
            return null;
        }

        if (test.Status == ETestStatus.Finished)
        {
            Notificator.Handle("Test already finished");
            return null;
        }

        test.NumberOfCorrectAnswers = test.TestQuestions.Count(tq => tq.IsCorrect == true);
        test.Status = ETestStatus.Finished;
        _testRepository.Update(test);

        return await CommitChanges() ? Mapper.Map<TestDto>(test) : null;
    }

    private async Task<bool> CommitChanges()
    {
        if (await _testRepository.UnitOfWork.Commit())
            return true;

        Notificator.Handle("An error occurred while saving changes");
        return false;
    }
}