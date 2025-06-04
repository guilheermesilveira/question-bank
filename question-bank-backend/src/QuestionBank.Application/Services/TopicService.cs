using AutoMapper;
using QuestionBank.Application.Contracts.Services;
using QuestionBank.Application.DTOs.Pagination;
using QuestionBank.Application.DTOs.Topic;
using QuestionBank.Application.Notifications;
using QuestionBank.Domain.Entities;
using QuestionBank.Domain.Validators;
using QuestionBank.Infra.Contracts.Repositories;

namespace QuestionBank.Application.Services;

public class TopicService : BaseService, ITopicService
{
    private readonly ITopicRepository _topicRepository;

    public TopicService(
        INotificator notificator,
        IMapper mapper,
        ITopicRepository topicRepository
    ) : base(notificator, mapper)
    {
        _topicRepository = topicRepository;
    }

    public async Task<TopicDto?> Add(AddTopicDto dto)
    {
        if (!await ValidationsToAdd(dto))
            return null;

        var topic = Mapper.Map<Topic>(dto);
        _topicRepository.Add(topic);

        return await CommitChanges() ? Mapper.Map<TopicDto>(topic) : null;
    }

    public async Task<TopicDto?> Update(int id, UpdateTopicDto dto)
    {
        if (!await ValidationsToUpdate(id, dto))
            return null;

        var topic = await _topicRepository.GetById(id);
        topic!.Name = dto.Name;
        _topicRepository.Update(topic);

        return await CommitChanges() ? Mapper.Map<TopicDto>(topic) : null;
    }

    public async Task<PaginationDto<TopicDto>> Search(SearchTopicDto dto)
    {
        var result = await _topicRepository.Search(dto.Name, dto.NumberOfItemsPerPage, dto.CurrentPage);

        return new PaginationDto<TopicDto>
        {
            TotalItems = result.TotalItems,
            NumberOfItemsPerPage = result.NumberOfItemsPerPage,
            NumberOfPages = result.NumberOfPages,
            CurrentPage = result.CurrentPage,
            Items = Mapper.Map<List<TopicDto>>(result.Items)
        };
    }

    public async Task<TopicDto?> GetById(int id)
    {
        var topic = await _topicRepository.GetById(id);
        if (topic != null)
            return Mapper.Map<TopicDto>(topic);

        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<List<TopicDto>> GetAll()
    {
        var topics = await _topicRepository.GetAll();
        return Mapper.Map<List<TopicDto>>(topics);
    }

    private async Task<bool> ValidationsToAdd(AddTopicDto dto)
    {
        var topic = Mapper.Map<Topic>(dto);
        var validator = new TopicValidator();

        var result = await validator.ValidateAsync(topic);
        if (!result.IsValid)
        {
            Notificator.Handle(result.Errors);
            return false;
        }

        var topicExist = await _topicRepository.FirstOrDefault(t => t.Name == dto.Name);
        if (topicExist != null)
        {
            Notificator.Handle("Name already exists");
            return false;
        }

        return true;
    }

    private async Task<bool> ValidationsToUpdate(int id, UpdateTopicDto dto)
    {
        if (id != dto.Id)
        {
            Notificator.Handle("The ID provided in the URL must be the same as the ID provided in the JSON");
            return false;
        }

        var topicExist = await _topicRepository.FirstOrDefault(t => t.Id == id);
        if (topicExist == null)
        {
            Notificator.HandleNotFoundResource();
            return false;
        }

        var topic = Mapper.Map<Topic>(dto);
        var validator = new TopicValidator();

        var result = await validator.ValidateAsync(topic);
        if (!result.IsValid)
        {
            Notificator.Handle(result.Errors);
            return false;
        }

        return true;
    }

    private async Task<bool> CommitChanges()
    {
        if (await _topicRepository.UnitOfWork.Commit())
            return true;

        Notificator.Handle("An error occurred while saving changes");
        return false;
    }
}