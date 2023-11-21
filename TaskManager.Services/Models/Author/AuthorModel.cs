using TaskManager.Data.Entities;

namespace TaskManager.Core.Models.Author
{
    public class AuthorModel : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name
        {
            get { return $"{FirstName} {LastName}"; }
        }

        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
