using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Explorevia.Models
{
    public class ExploreviaDBContext : IdentityDbContext
    {
        public ExploreviaDBContext(DbContextOptions<ExploreviaDBContext> options) : base(options)
        {
        }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<BookingRequests> BookingRequests { get; set; }
    }
}
