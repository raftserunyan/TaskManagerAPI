using TaskManager.Data.Entities.Base;

namespace TaskManager.Data.Entities
{
    public class AuthorEntity : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
