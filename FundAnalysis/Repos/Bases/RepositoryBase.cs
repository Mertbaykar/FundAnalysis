using FundAnalysis.API.DbContexts;
using FundAnalysis.Models;

namespace FundAnalysis.API.Repos.Bases
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        protected readonly FundContext _fundContext;
        public RepositoryBase(FundContext fundContext)
        {
            _fundContext = fundContext;
        }

        // Define Type Based Methods when necessary...

    }
}
