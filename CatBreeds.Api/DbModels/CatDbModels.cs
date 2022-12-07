using System.ComponentModel.DataAnnotations;

namespace CatBreeds.Api
{
    public class Breed
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
    public class Cat
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        public int BreedId { get; set; }
        public Breed Breed { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; }
    }
}