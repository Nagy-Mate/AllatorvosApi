using Microsoft.EntityFrameworkCore;

namespace VetApi.Core;

public class VetDbContext(DbContextOptions<VetDbContext> options) : DbContext(options)
{
    public DbSet<Treatment> Treatment { get; set; }
}
