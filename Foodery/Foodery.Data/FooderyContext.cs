using Foodery.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Foodery.Data
{
    public class FooderyContext : IdentityDbContext<User>
    {
        public FooderyContext(DbContextOptions<FooderyContext> options)
            : base(options)
        { }
    }
}
