using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace mocking_ef_datacontext.DB
{
    public interface ITopicsDataContext
    {
        IDbSet<Topic> Topics { get; set; }
        IDbSet<Tutorial> Tutorials { get; set; }
        int SaveChanges();
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}