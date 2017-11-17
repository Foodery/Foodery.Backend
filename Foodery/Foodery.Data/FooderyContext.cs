using Foodery.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodery.Data
{
    public class FooderyContext : DbContext
    {
        public FooderyContext(DbContextOptions<FooderyContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
    }
}
