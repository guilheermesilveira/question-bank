using AutoMapper;
using QuestionBank.Application.Contracts.Services;
using QuestionBank.Application.DTOs.Pagination;
using QuestionBank.Application.DTOs.UserAnswer;
using QuestionBank.Application.Notifications;
using QuestionBank.Domain.Entities;
using QuestionBank.Domain.Validators;
using QuestionBank.Infra.Contracts.Repositories;

namespace QuestionBank.Application.Services;

public class UserAnswerService : BaseService, IUserAnswerService
{
    private readonly IUserAnswerRepository _userAnswerRepository;
    private readonly IUserRepository _userRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IAlternativeRepository _alternativeRepository;

    public UserAnswerService(
        INotificator notificator,
        IMapper mapper,
        IUserAnswerRepository userAnswerRepository,
        IUserRepository userRepository,
        IQuestionRepository questionRepository,
        IAlternativeRepository alternativeRepository
    ) : base(notificator, mapper)
    {
        _userAnswerRepository = userAnswerRepository;
        _userRepository = userRepository;
        _questionRepository = questionRepository;
        _alternativeRepository = alternativeRepository;
    }

    public async Task<UserAnswerDto?> Add(AddUserAnswerDto dto)
    {
        if (!await ValidationsToAdd(dto))
            return null;

        var userAnswer = Mapper.Map<UserAnswer>(dto);
        _userAnswerRepository.Add(userAnswer);

        return await CommitChanges() ? Mapper.Map<UserAnswerDto>(userAnswer) : null;
    }

    public async Task<PaginationDto<UserAnswerDto>> Search(SearchUserAnswerDto dto)
    {
        var result = await _userAnswerRepository.Search(dto.IsCorrect, dto.UserId, dto.QuestionId,
            dto.AlternativeId, dto.NumberOfItemsPerPage, dto.CurrentPage);

        return new PaginationDto<UserAnswerDto>
        {
            TotalItems = result.TotalItems,
            NumberOfItemsPerPage = result.NumberOfItemsPerPage,
            NumberOfPages = result.NumberOfPages,
            CurrentPage = result.CurrentPage,
            Items = Mapper.Map<List<UserAnswerDto>>(result.Items)
        };
    }

    public async Task<UserAnswerDto?> GetById(int id)
    {
        var userAnswer = await _userAnswerRepository.GetById(id);
        if (userAnswer != null)
            return Mapper.Map<UserAnswerDto>(userAnswer);

        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<List<UserAnswerDto>> GetAll()
    {
        var userAnswers = await _userAnswerRepository.GetAll();
        return Mapper.Map<List<UserAnswerDto>>(userAnswers);
    }

    private async Task<bool> ValidationsToAdd(AddUserAnswerDto dto)
    {
        var userAnswer = Mapper.Map<UserAnswer>(dto);
        var validator = new UserAnswerValidator();

        var result = await validator.ValidateAsync(userAnswer);
        if (!result.IsValid)
        {
            Notificator.Handle(result.Errors);
            return false;
        }

        var userExist = await _userRepository.GetById(dto.UserId);
        if (userExist == null)
        {
            Notificator.Handle("User not found");
            return false;
        }

        var questionExist = await _questionRepository.GetById(dto.QuestionId);
        if (questionExist == null)
        {
            Notificator.Handle("Question not found");
            return false;
        }

        var alternativeExist = await _alternativeRepository.GetById(dto.AlternativeId);
        if (alternativeExist == null)
        {
            Notificator.Handle("Alternative not found");
            return false;
        }

        return true;
    }

    private async Task<bool> CommitChanges()
    {
        if (await _userAnswerRepository.UnitOfWork.Commit())
            return true;

        Notificator.Handle("An error occurred while saving changes");
        return false;
    }
}