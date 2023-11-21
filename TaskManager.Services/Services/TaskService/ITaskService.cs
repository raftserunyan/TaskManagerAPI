using TaskManager.Core.Models.Task;
using TaskManager.Core.Services.Common;
using TaskManager.Data.Entities;

namespace TaskManager.Core.Services.TaskService
{
    public interface ITaskService : ICommonService<TaskModel, TaskEntity>
    {
        Task<TaskModel> Add(TaskModel model, int userId);
        Task<IEnumerable<TaskModel>> GetTasksForUser(int userId);
    }
}
