using AutoMapper;
using QuestionBank.Application.DTOs.Auth;
using QuestionBank.Application.DTOs.Question;
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

        #region Question

        CreateMap<AddQuestionDto, Question>();
        CreateMap<UpdateQuestionDto, Question>();
        CreateMap<Question, QuestionDto>();

        #endregion
    }
}