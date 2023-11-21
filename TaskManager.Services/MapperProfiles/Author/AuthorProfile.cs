using AutoMapper;
using TaskManager.Core.Models.Author;
using TaskManager.Data.Entities;

namespace TaskManager.Core.MapperProfiles.Author
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<AuthorEntity, AuthorModel>()
                .ForMember(x => x.FirstName, opts => opts.MapFrom(y => y.User.FirstName))
                .ForMember(x => x.LastName, opts => opts.MapFrom(y => y.User.LastName));

            CreateMap<AuthorModel, AuthorEntity>();
        }
    }
}
