using TaskManager.Core.Models.Author;
using TaskManager.Core.Services.Common;
using TaskManager.Data.Entities;

namespace TaskManager.Core.Services.AuthorService
{
    public interface IAuthorService : ICommonService<AuthorModel, AuthorEntity>
    {
        Task<AuthorModel> GetAuthorByUser(User user);
    }
}
