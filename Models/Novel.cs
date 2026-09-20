using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NovelPlatform.Models
{
    public class Novel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [MaxLength(500)]
        public string CoverImageUrl { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        public bool IsPublished { get; set; } = true;

        public bool IsFeatured { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public double Rating { get; set; } = 4.8;

        public int ViewsCount { get; set; } = 0;

        public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
    }
}
