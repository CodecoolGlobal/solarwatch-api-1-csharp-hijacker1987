using Microsoft.EntityFrameworkCore;
using SolarWatch.Model;

namespace SolarWatch.Data
{
    public class CityApiContext : DbContext
    {
        public DbSet<City>? Cities { get; set; }
        public DbSet<SunriseSunsetTimes>? Times { get; set; }

        public CityApiContext(DbContextOptions<CityApiContext> options) : base(options)
        {
        }
    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("SolarWatchApi");
            }
        }
    }
}