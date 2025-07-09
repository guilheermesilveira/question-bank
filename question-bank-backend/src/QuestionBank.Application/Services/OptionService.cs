using AutoMapper;
using QuestionBank.Application.Contracts.Services;
using QuestionBank.Application.DTOs.Option;
using QuestionBank.Application.DTOs.Pagination;
using QuestionBank.Application.Notifications;
using QuestionBank.Domain.Entities;
using QuestionBank.Domain.Validators;
using QuestionBank.Infra.Contracts.Repositories;

namespace QuestionBank.Application.Services;

public class OptionService : BaseService, IOptionService
{
    private readonly IOptionRepository _optionRepository;
    private readonly IQuestionRepository _questionRepository;

    public OptionService(
        INotificator notificator,
        IMapper mapper,
        IOptionRepository optionRepository,
        IQuestionRepository questionRepository
    ) : base(notificator, mapper)
    {
        _optionRepository = optionRepository;
        _questionRepository = questionRepository;
    }

    public async Task<OptionDto?> Add(AddOptionDto dto)
    {
        if (!await ValidationsToAdd(dto))
            return null;

        var option = Mapper.Map<Option>(dto);
        _optionRepository.Add(option);

        return await CommitChanges() ? Mapper.Map<OptionDto>(option) : null;
    }

    public async Task<OptionDto?> Update(int id, UpdateOptionDto dto)
    {
        if (!await ValidationsToUpdate(id, dto))
            return null;

        var option = await _optionRepository.FirstOrDefault(o => o.Id == id);
        option!.Text = dto.Text;
        option.IsCorrect = dto.IsCorrect;
        option.QuestionId = dto.QuestionId;
        _optionRepository.Update(option);

        return await CommitChanges() ? Mapper.Map<OptionDto>(option) : null;
    }

    public async Task Delete(int id)
    {
        var option = await _optionRepository.GetById(id);
        if (option == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        _optionRepository.Delete(option);
        await CommitChanges();
    }

    public async Task<PaginationDto<OptionDto>> Search(SearchOptionDto dto)
    {
        var result = await _optionRepository.Search(dto.Text, dto.IsCorrect, dto.QuestionId,
            dto.NumberOfItemsPerPage, dto.CurrentPage);

        return new PaginationDto<OptionDto>
        {
            TotalItems = result.TotalItems,
            NumberOfItemsPerPage = result.NumberOfItemsPerPage,
            NumberOfPages = result.NumberOfPages,
            CurrentPage = result.CurrentPage,
            Items = Mapper.Map<List<OptionDto>>(result.Items)
        };
    }

    public async Task<OptionDto?> GetById(int id)
    {
        var option = await _optionRepository.GetById(id);
        if (option != null)
            return Mapper.Map<OptionDto>(option);

        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<List<OptionDto>> GetAll()
    {
        var options = await _optionRepository.GetAll();
        return Mapper.Map<List<OptionDto>>(options);
    }

    private async Task<bool> ValidationsToAdd(AddOptionDto dto)
    {
        var option = Mapper.Map<Option>(dto);
        var validator = new OptionValidator();

        var result = await validator.ValidateAsync(option);
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

        var options = await _optionRepository.Search(null, null, questionExist.Id);
        if (options.Items.Count == 5)
        {
            Notificator.Handle("This question already has five options");
            return false;
        }

        if (dto.IsCorrect && options.Items.Any(o => o.IsCorrect))
        {
            Notificator.Handle("There is already an option marked as correct for this question");
            return false;
        }

        return true;
    }

    private async Task<bool> ValidationsToUpdate(int id, UpdateOptionDto dto)
    {
        if (id != dto.Id)
        {
            Notificator.Handle("The ID provided in the URL must be the same as the ID provided in the JSON");
            return false;
        }

        var optionExist = await _optionRepository.FirstOrDefault(o => o.Id == id);
        if (optionExist == null)
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

        var option = Mapper.Map<Option>(dto);
        var validator = new OptionValidator();

        var result = await validator.ValidateAsync(option);
        if (!result.IsValid)
        {
            Notificator.Handle(result.Errors);
            return false;
        }

        if (optionExist.QuestionId != dto.QuestionId)
        {
            var optionsFromAnotherQuestion = await _optionRepository.Search(null, null, dto.QuestionId);
            if (optionsFromAnotherQuestion.Items.Count == 5)
            {
                Notificator.Handle("This question already has five options");
                return false;
            }

            if (dto.IsCorrect && optionsFromAnotherQuestion.Items.Any(o => o.IsCorrect))
            {
                Notificator.Handle("There is already an option marked as correct for this question");
                return false;
            }
        }
        else
        {
            var options = await _optionRepository.Search(null, null, optionExist.QuestionId);
            if (dto.IsCorrect && options.Items.Any(o => o.IsCorrect && o.Id != id))
            {
                Notificator.Handle("There is already an option marked as correct for this question");
                return false;
            }
        }

        return true;
    }

    private async Task<bool> CommitChanges()
    {
        if (await _optionRepository.UnitOfWork.Commit())
            return true;

        Notificator.Handle("An error occurred while saving changes");
        return false;
    }
}