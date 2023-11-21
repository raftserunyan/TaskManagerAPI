using TaskManager.Data.Entities;
using TaskManager.Data.Specifications.Common;

namespace TaskManager.Data.Specifications.TaskSpecs
{
    public class TasksByAuthorIdSpecification : CommonSpecification<TaskEntity>
    {
        public TasksByAuthorIdSpecification(int authorId) : base(x => x.AuthorId == authorId)  { }
    }
}
