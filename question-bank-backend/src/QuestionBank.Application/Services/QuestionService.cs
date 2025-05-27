using AutoMapper;
using QuestionBank.Application.Contracts.Services;
using QuestionBank.Application.DTOs.Pagination;
using QuestionBank.Application.DTOs.Question;
using QuestionBank.Application.Notifications;
using QuestionBank.Domain.Entities;
using QuestionBank.Domain.Validators;
using QuestionBank.Infra.Contracts.Repositories;

namespace QuestionBank.Application.Services;

public class QuestionService : BaseService, IQuestionService
{
    private readonly IQuestionRepository _questionRepository;

    public QuestionService(
        INotificator notificator,
        IMapper mapper,
        IQuestionRepository questionRepository
    ) : base(notificator, mapper)
    {
        _questionRepository = questionRepository;
    }

    public async Task<QuestionDto?> Add(AddQuestionDto dto)
    {
        if (!await ValidationsToAdd(dto))
            return null;

        var question = Mapper.Map<Question>(dto);
        _questionRepository.Add(question);

        return await CommitChanges() ? Mapper.Map<QuestionDto>(question) : null;
    }

    public async Task<QuestionDto?> Update(int id, UpdateQuestionDto dto)
    {
        if (!await ValidationsToUpdate(id, dto))
            return null;

        var question = await _questionRepository.FirstOrDefault(q => q.Id == id);
        question!.Statement = dto.Statement;
        question.Difficulty = dto.Difficulty;
        _questionRepository.Update(question);

        return await CommitChanges() ? Mapper.Map<QuestionDto>(question) : null;
    }

    public async Task<PaginationDto<QuestionDto>> Search(SearchQuestionDto dto)
    {
        var result = await _questionRepository.Search(dto.Id, dto.Statement, dto.Difficulty,
            dto.NumberOfItemsPerPage, dto.CurrentPage);

        return new PaginationDto<QuestionDto>
        {
            TotalItems = result.TotalItems,
            NumberOfItemsPerPage = result.NumberOfItemsPerPage,
            NumberOfPages = result.NumberOfPages,
            CurrentPage = result.CurrentPage,
            Items = Mapper.Map<List<QuestionDto>>(result.Items)
        };
    }

    public async Task<QuestionDto?> GetById(int id)
    {
        var question = await _questionRepository.GetById(id);
        if (question != null)
            return Mapper.Map<QuestionDto>(question);

        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<List<QuestionDto>> GetAll()
    {
        var questions = await _questionRepository.GetAll();
        return Mapper.Map<List<QuestionDto>>(questions);
    }

    private async Task<bool> ValidationsToAdd(AddQuestionDto dto)
    {
        var question = Mapper.Map<Question>(dto);
        var validator = new QuestionValidator();

        var result = await validator.ValidateAsync(question);
        if (!result.IsValid)
        {
            Notificator.Handle(result.Errors);
            return false;
        }

        return true;
    }

    private async Task<bool> ValidationsToUpdate(int id, UpdateQuestionDto dto)
    {
        if (id != dto.Id)
        {
            Notificator.Handle("The ID provided in the URL must be the same as the ID provided in the JSON");
            return false;
        }

        var questionExist = await _questionRepository.FirstOrDefault(q => q.Id == id);
        if (questionExist == null)
        {
            Notificator.HandleNotFoundResource();
            return false;
        }

        var question = Mapper.Map<Question>(dto);
        var validator = new QuestionValidator();

        var result = await validator.ValidateAsync(question);
        if (!result.IsValid)
        {
            Notificator.Handle(result.Errors);
            return false;
        }

        return true;
    }

    private async Task<bool> CommitChanges()
    {
        if (await _questionRepository.UnitOfWork.Commit())
            return true;

        Notificator.Handle("An error occurred while saving changes");
        return false;
    }
}