using HostelWebApi.Entities;
using HostelWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HostelWebApi.Data
{
    public class AppDbContext : DbContext
    {   
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {

            }

            public DbSet<User> Users { get; set; }
            public DbSet<SalesData> SalesData { get; set; }
    }
    
}
