using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CarsHub.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
       
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<Car> Cars { get; set; }
       
        public DbSet<Brand> Brands { get; set; }

        public DbSet<Customer> Customer { get; set; }

        public DbSet<Country> Countrys { get; set; }
              

        public DbSet<State> States { get; set; }

        public DbSet<Admin> Admins { get; set; }

        //public DbSet<Payment> Payments { get; set; }
        //public DbSet<Motor> Carss { get; set; }

        //public DbSet<CarOrder> CarOrders { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<CarOrder>().HasRequired(c => c.Car).WithMany().WillCascadeOnDelete(false);
        //    ////modelBuilder.Entity<Car>().HasRequired(c => c.).WithMany().WillCascadeOnDelete(false);

        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //    modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        //    modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        //}

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //    modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
           
        //}
       
    }
}