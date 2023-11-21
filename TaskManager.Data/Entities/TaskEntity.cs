using TaskManager.Data.Entities.Base;

namespace TaskManager.Data.Entities
{
    public class TaskEntity : BaseEntity
    {
        public string Title { get; set; }
        public string Text { get; set; }

        public int? AuthorId { get; set; }
        public AuthorEntity Author { get; set; }
    }
}
