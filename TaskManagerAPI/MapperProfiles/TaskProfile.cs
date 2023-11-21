using AutoMapper;
using TaskManager.API.ViewModels.Task;
using TaskManager.Core.Models.Task;

namespace TaskManager.API.MapperProfiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskCreateRequestModel, TaskModel>();
            CreateMap<TaskModel, TaskViewModel>()
                .ForMember(x => x.Author, opts => opts.MapFrom(y => y.Author.Name));
        }
    }
}
