using System.ComponentModel.DataAnnotations;

namespace CatBreeds.Api
{
    public class BreedVm
    {
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }
    }
    
    public class CatPostVm
    {
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        public int BreedId { get; set; }

    }
    public class CatReturnVm
    {
        public string Name { get; set; }
        public int BreedId { get; set; }
        public string BreedName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; }

    }
}