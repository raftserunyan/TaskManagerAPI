using AutoMapper;
using TaskManager.Core.Models.Task;
using TaskManager.Data.Entities;

namespace TaskManager.Core.MapperProfiles.Task
{
    internal class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskModel, TaskEntity>().ReverseMap();
        }
    }
}
