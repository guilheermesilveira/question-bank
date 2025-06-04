using AutoMapper;
using QuestionBank.Application.DTOs.Alternative;
using QuestionBank.Application.DTOs.Auth;
using QuestionBank.Application.DTOs.Question;
using QuestionBank.Application.DTOs.Topic;
using QuestionBank.Application.DTOs.User;
using QuestionBank.Application.DTOs.UserAnswer;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Application.Configurations;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        #region Auth & User

        CreateMap<LoginDto, User>();
        CreateMap<AddUserDto, User>();
        CreateMap<UpdateUserDto, User>();
        CreateMap<User, UserDto>();

        #endregion

        #region UserAnswer

        CreateMap<AddUserAnswerDto, UserAnswer>();
        CreateMap<UserAnswer, UserAnswerDto>();

        #endregion

        #region Topic

        CreateMap<AddTopicDto, Topic>();
        CreateMap<UpdateTopicDto, Topic>();
        CreateMap<Topic, TopicDto>();

        #endregion

        #region Question

        CreateMap<AddQuestionDto, Question>();
        CreateMap<UpdateQuestionDto, Question>();
        CreateMap<Question, QuestionDto>();

        #endregion

        #region Alternative

        CreateMap<AddAlternativeDto, Alternative>();
        CreateMap<UpdateAlternativeDto, Alternative>();
        CreateMap<Alternative, AlternativeDto>();

        #endregion
    }
}