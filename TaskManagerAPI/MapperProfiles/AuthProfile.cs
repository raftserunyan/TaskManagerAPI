using AutoMapper;
using TaskManager.API.ViewModels.Auth;
using TaskManager.Core.Models.Auth;

namespace TaskManager.API.MapperProfiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RegistrationRequest, RegistrationModel>();
            CreateMap<SignInRequest, SignInRequestModel>();
        }
    }
}
