using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ParaPharmacie.Models;

namespace ParaPharmacie.Data
{
    public class EcommerceContext : IdentityDbContext<ApplicationUser>
    {

        public EcommerceContext(DbContextOptions<EcommerceContext> options)
            : base(options)
        {
        }
                public  DbSet<Category> Categories { get; set; }
                public  DbSet<Product> Products { get; set; }

                public DbSet<Contact> Contacts { get; set; }

                public DbSet<ShoppingCart> ShoppingCarts { get; set; }

                public DbSet<Order> Orders { get; set; }

                public DbSet<OrderDetail> OrdersDetails { get; set; }


    }    
}
