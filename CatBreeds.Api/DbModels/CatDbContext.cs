using Microsoft.EntityFrameworkCore;

namespace CatBreeds.Api.DbModels
{
    public class CatDbContext :DbContext
    {
        public CatDbContext(DbContextOptions<CatDbContext> options) : base(options)
        {
        }


        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Cat> Cats { get; set; }
    }
}
