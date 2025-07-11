using AutoMapper;
using QuestionBank.Application.DTOs.Auth;
using QuestionBank.Application.DTOs.Option;
using QuestionBank.Application.DTOs.Question;
using QuestionBank.Application.DTOs.Test;
using QuestionBank.Application.DTOs.Topic;
using QuestionBank.Application.DTOs.User;
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

        #region Topic

        CreateMap<AddTopicDto, Topic>();
        CreateMap<UpdateTopicDto, Topic>();
        CreateMap<Topic, TopicDto>();

        #endregion

        #region Question

        CreateMap<AddQuestionDto, Question>();
        CreateMap<UpdateQuestionDto, Question>();
        CreateMap<Question, QuestionDto>()
            .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => src.Difficulty.ToString()));

        #endregion

        #region Option

        CreateMap<AddOptionDto, Option>();
        CreateMap<UpdateOptionDto, Option>();
        CreateMap<Option, OptionDto>();

        #endregion

        #region Test

        CreateMap<CreateTestDto, Test>();
        CreateMap<Test, TestDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        CreateMap<TestQuestion, TestQuestionDto>();

        #endregion
    }
}