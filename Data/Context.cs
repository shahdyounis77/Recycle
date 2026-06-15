

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Recycle.Data
{
    public class Context:IdentityDbContext<Models.User>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
           
        }

        public DbSet<Models.Machine> Machines { get; set; }
        public DbSet<Models.Transaction> Transactions { get; set; }
        public DbSet<Models.Otp> Otps { get; set; }

       
    }
}
