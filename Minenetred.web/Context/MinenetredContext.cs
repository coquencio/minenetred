using Microsoft.EntityFrameworkCore;
using Minenetred.web.Context.ContextModels;

namespace Minenetred.web.Context
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
