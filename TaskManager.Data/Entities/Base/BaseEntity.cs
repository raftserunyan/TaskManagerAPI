using System.ComponentModel.DataAnnotations;
    
namespace TaskManager.Data.Entities.Base
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
