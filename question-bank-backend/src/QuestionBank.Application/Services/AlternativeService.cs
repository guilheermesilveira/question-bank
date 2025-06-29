using AutoMapper;
using QuestionBank.Application.Contracts.Services;
using QuestionBank.Application.DTOs.Alternative;
using QuestionBank.Application.DTOs.Pagination;
using QuestionBank.Application.Notifications;
using QuestionBank.Domain.Entities;
using QuestionBank.Domain.Validators;
using QuestionBank.Infra.Contracts.Repositories;

namespace QuestionBank.Application.Services;

public class AlternativeService : BaseService, IAlternativeService
{
    private readonly IAlternativeRepository _alternativeRepository;
    private readonly IQuestionRepository _questionRepository;

    public AlternativeService(
        INotificator notificator,
        IMapper mapper,
        IAlternativeRepository alternativeRepository,
        IQuestionRepository questionRepository
    ) : base(notificator, mapper)
    {
        _alternativeRepository = alternativeRepository;
        _questionRepository = questionRepository;
    }

    public async Task<AlternativeDto?> Add(AddAlternativeDto dto)
    {
        if (!await ValidationsToAdd(dto))
            return null;

        var alternative = Mapper.Map<Alternative>(dto);
        _alternativeRepository.Add(alternative);

        return await CommitChanges() ? Mapper.Map<AlternativeDto>(alternative) : null;
    }

    public async Task<AlternativeDto?> Update(int id, UpdateAlternativeDto dto)
    {
        if (!await ValidationsToUpdate(id, dto))
            return null;

        var alternative = await _alternativeRepository.FirstOrDefault(a => a.Id == id);
        alternative!.Text = dto.Text;
        alternative.IsCorrect = dto.IsCorrect;
        alternative.QuestionId = dto.QuestionId;
        _alternativeRepository.Update(alternative);

        return await CommitChanges() ? Mapper.Map<AlternativeDto>(alternative) : null;
    }

    public async Task Delete(int id)
    {
        var alternative = await _alternativeRepository.GetById(id);
        if (alternative == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        _alternativeRepository.Delete(alternative);
        await CommitChanges();
    }

    public async Task<PaginationDto<AlternativeDto>> Search(SearchAlternativeDto dto)
    {
        var result = await _alternativeRepository.Search(dto.Text, dto.IsCorrect, dto.QuestionId,
            dto.NumberOfItemsPerPage, dto.CurrentPage);

        return new PaginationDto<AlternativeDto>
        {
            TotalItems = result.TotalItems,
            NumberOfItemsPerPage = result.NumberOfItemsPerPage,
            NumberOfPages = result.NumberOfPages,
            CurrentPage = result.CurrentPage,
            Items = Mapper.Map<List<AlternativeDto>>(result.Items)
        };
    }

    public async Task<AlternativeDto?> GetById(int id)
    {
        var alternative = await _alternativeRepository.GetById(id);
        if (alternative != null)
            return Mapper.Map<AlternativeDto>(alternative);

        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<List<AlternativeDto>> GetAll()
    {
        var alternatives = await _alternativeRepository.GetAll();
        return Mapper.Map<List<AlternativeDto>>(alternatives);
    }

    private async Task<bool> ValidationsToAdd(AddAlternativeDto dto)
    {
        var alternative = Mapper.Map<Alternative>(dto);
        var validator = new AlternativeValidator();

        var result = await validator.ValidateAsync(alternative);
        if (!result.IsValid)
        {
            Notificator.Handle(result.Errors);
            return false;
        }

        var questionExist = await _questionRepository.GetById(dto.QuestionId);
        if (questionExist == null)
        {
            Notificator.Handle("Question not found");
            return false;
        }

        var alternatives = await _alternativeRepository.Search(null, null, questionExist.Id);
        if (alternatives.Items.Count == 5)
        {
            Notificator.Handle("This question already has five alternatives");
            return false;
        }

        if (dto.IsCorrect && alternatives.Items.Any(a => a.IsCorrect))
        {
            Notificator.Handle("There is already an alternative marked as correct for this question");
            return false;
        }

        return true;
    }

    private async Task<bool> ValidationsToUpdate(int id, UpdateAlternativeDto dto)
    {
        if (id != dto.Id)
        {
            Notificator.Handle("The ID provided in the URL must be the same as the ID provided in the JSON");
            return false;
        }

        var alternativeExist = await _alternativeRepository.FirstOrDefault(a => a.Id == id);
        if (alternativeExist == null)
        {
            Notificator.HandleNotFoundResource();
            return false;
        }

        var questionExist = await _questionRepository.GetById(dto.QuestionId);
        if (questionExist == null)
        {
            Notificator.Handle("Question not found");
            return false;
        }

        var alternative = Mapper.Map<Alternative>(dto);
        var validator = new AlternativeValidator();

        var result = await validator.ValidateAsync(alternative);
        if (!result.IsValid)
        {
            Notificator.Handle(result.Errors);
            return false;
        }

        if (alternativeExist.QuestionId != dto.QuestionId)
        {
            var alternativesFromAnotherQuestion = await _alternativeRepository.Search(null, null, dto.QuestionId);
            if (alternativesFromAnotherQuestion.Items.Count == 5)
            {
                Notificator.Handle("This question already has five alternatives");
                return false;
            }

            if (dto.IsCorrect && alternativesFromAnotherQuestion.Items.Any(a => a.IsCorrect))
            {
                Notificator.Handle("There is already an alternative marked as correct for this question");
                return false;
            }
        }
        else
        {
            var alternatives = await _alternativeRepository.Search(null, null, alternativeExist.QuestionId);
            if (dto.IsCorrect && alternatives.Items.Any(a => a.IsCorrect && a.Id != id))
            {
                Notificator.Handle("There is already an alternative marked as correct for this question");
                return false;
            }
        }

        return true;
    }

    private async Task<bool> CommitChanges()
    {
        if (await _alternativeRepository.UnitOfWork.Commit())
            return true;

        Notificator.Handle("An error occurred while saving changes");
        return false;
    }
}