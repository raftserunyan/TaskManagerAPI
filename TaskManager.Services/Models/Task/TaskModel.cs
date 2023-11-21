using TaskManager.Core.Models.Author;

namespace TaskManager.Core.Models.Task
{
    public class TaskModel : BaseModel
    {
        public string Title { get; set; }
        public string Text { get; set; }

        public int? AuthorId { get; set; }
        public AuthorModel Author { get; set; }
    }
}
