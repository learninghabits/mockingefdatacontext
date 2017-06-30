using System.Data.Entity;

namespace mocking_ef_datacontext.DB
{
    public class TopicsDataContext : DbContext, ITopicsDataContext
    {

        //public DbSet<Topic> Topics { get; set; }
        //public DbSet<Tutorial> Tutorials { get; set; }
        //Changed to use interfaces instead.. Also note the interface
        public IDbSet<Topic> Topics { get; set; }
        public IDbSet<Tutorial> Tutorials { get; set; }
    }
}