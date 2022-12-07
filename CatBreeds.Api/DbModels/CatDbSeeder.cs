using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CatBreeds.Api.DbModels
{
    public static class CatDbSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var ctx = new CatDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<CatDbContext>>()))

            {
                SeedCatBreeds(ctx);
            }
        }

        public static void SeedCatBreeds(CatDbContext ctx)
        {
            if (!ctx.Breeds.Any())
            {
                var _breeds = new[] {
                    "Siamese cat",
                    "British Shorthair",
                    "Maine Coon",
                    "Persian cat",
                    "Ragdoll",
                    "Sphynx cat",
                    "American Shorthair",
                    "Abyssinian",
                    "Exotic Shorthair",
                    "Scottish Fold",
                    "Burmese cat",
                    "Birman"
                };
                foreach (var breed in _breeds)
                {
                    ctx.Breeds.Add(new Breed() { Name = breed });
                }
                ctx.SaveChanges();
            }
        }
    }
}
