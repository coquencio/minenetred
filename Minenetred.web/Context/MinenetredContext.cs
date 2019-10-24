using Microsoft.EntityFrameworkCore;
using Minenetred.Web.Context.ContextModels;

namespace Minenetred.Web.Context
{
    public class MinenetredContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public MinenetredContext(DbContextOptions<MinenetredContext> options)
            : base(options)
        {
        }
    }
}