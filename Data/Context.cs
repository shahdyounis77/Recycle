

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Recycle.Data
{
    public class Context:IdentityDbContext<Models.User>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
            
        }
    }
}
