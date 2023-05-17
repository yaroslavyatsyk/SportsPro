using Microsoft.EntityFrameworkCore;

namespace SportsPro.Models
{
    public class SportContext : DbContext
    {
        public SportContext(DbContextOptions<SportContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<Customer> Customers { get; set; }


        public DbSet<Technician> Technicianes { get; set; }

        public DbSet<Incident> Incidents { get; set; }

        public DbSet<Registration> Registrations { get; set; }

       
    }
}
