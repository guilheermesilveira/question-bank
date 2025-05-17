using AutoMapper;
using QuestionBank.Application.DTOs.Auth;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Application.Configurations;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        #region Auth

        CreateMap<LoginDto, User>();

        #endregion
    }
}