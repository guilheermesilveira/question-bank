using QuestionBank.Application.DTOs.Pagination;
using QuestionBank.Application.DTOs.Topic;

namespace QuestionBank.Application.Contracts.Services;

public interface ITopicService
{
    Task<TopicDto?> Add(AddTopicDto dto);
    Task<TopicDto?> Update(int id, UpdateTopicDto dto);
    Task Delete(int id);
    Task<PaginationDto<TopicDto>> Search(SearchTopicDto dto);
    Task<TopicDto?> GetById(int id);
    Task<List<TopicDto>> GetAll();
}