using AutoMapper;
using QuestionBank.Application.Contracts.Services;
using QuestionBank.Application.DTOs.UserAnswer;
using QuestionBank.Application.Notifications;
using QuestionBank.Domain.Entities;
using QuestionBank.Infra.Contracts.Repositories;

namespace QuestionBank.Application.Services;

public class UserAnswerService : BaseService, IUserAnswerService
{
    private readonly IUserAnswerRepository _userAnswerRepository;

    public UserAnswerService(
        INotificator notificator,
        IMapper mapper,
        IUserAnswerRepository userAnswerRepository
    ) : base(notificator, mapper)
    {
        _userAnswerRepository = userAnswerRepository;
    }

    public async Task<UserAnswerDto?> Add(AddUserAnswerDto dto)
    {
        var userAnswer = Mapper.Map<UserAnswer>(dto);
        _userAnswerRepository.Add(userAnswer);

        return await CommitChanges() ? Mapper.Map<UserAnswerDto>(userAnswer) : null;
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

    private async Task<bool> CommitChanges()
    {
        if (await _userAnswerRepository.UnitOfWork.Commit())
            return true;

        Notificator.Handle("An error occurred while saving changes");
        return false;
    }
}