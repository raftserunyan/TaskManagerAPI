using AutoMapper;
using TaskManager.Core.Models.Author;
using TaskManager.Core.Services.Common;
using TaskManager.Data.Entities;
using TaskManager.Data.Specifications.AuthorSpecs;
using TaskManager.Data.UnitOfWork;

namespace TaskManager.Core.Services.AuthorService
{
    internal class AuthorService : CommonService<AuthorModel, AuthorEntity>, IAuthorService
    {
        public AuthorService(IMapper mapper, IUnitOfWork uow) : base(uow, mapper)
        {
        }

        public async Task<AuthorModel> GetAuthorByUser(User user)
        {
            var authorEntity = await _uow.Repository<AuthorEntity>()
                                        .GetSingleBySpecification(new AuthorByUserIdSpecification(user.Id));

            if (authorEntity == null)
                authorEntity = new AuthorEntity { User = user };

            var authorModel = _mapper.Map<AuthorModel>(authorEntity);

            return authorModel;
        }
    }
}
