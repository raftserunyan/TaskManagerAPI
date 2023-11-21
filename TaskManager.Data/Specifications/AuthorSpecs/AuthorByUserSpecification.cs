using TaskManager.Data.Entities;
using TaskManager.Data.Specifications.Common;

namespace TaskManager.Data.Specifications.AuthorSpecs
{
    public class AuthorByUserIdSpecification : CommonSpecification<AuthorEntity>
    {
        public AuthorByUserIdSpecification(int userId)
            : base (x => x.UserId == userId)
        {
        }
    }
}
