using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Category> categories { get; set; }
        public DbSet<ProductImage> productImages { get; set; }
        public DbSet<Adv> advs { get; set; }
        public DbSet<Contact> contacts  { get; set; }
        public DbSet<News> news { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }
        public DbSet<Posts> posts { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<ProductCategory> productCategories { get; set; }
        /*public DbSet<ProductImage> productImages  { get; set; }*/
        public DbSet<Subscribe> subscribes { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }




        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}