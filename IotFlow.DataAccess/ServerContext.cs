using IotFlow.Models.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IotFlow.DataAccess
{
    public class ServerContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public ServerContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("SQLProviderConnectionString"));
        }
    }
}
