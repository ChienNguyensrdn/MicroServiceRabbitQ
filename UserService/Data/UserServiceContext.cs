using Microsoft.EntityFrameworkCore;
namespace UserService.Data
{
    public class UserServiceContext:DbContext 
    {
        public UserServiceContext(DbContextOptions<UserServiceContext> options)
           : base(options)
        {
        }

        public DbSet<UserService.Entities.User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserService.Entities.User>().ToTable("User");
        }
    }
}
