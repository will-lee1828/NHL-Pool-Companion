using NHL_Pool_Companion.Models;
using System.Data.Entity;

namespace NHL_Pool_Companion
{
    public class NHLPoolDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
    }
}