using System.Data.Entity;

namespace mocking_ef_datacontext.DB
{
    public class TopicsDataContext : DbContext
    {
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Tutorial> Tutorials { get; set; }
    }
}