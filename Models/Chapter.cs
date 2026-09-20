using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NovelPlatform.Models
{
    public class Chapter
    {
        public int Id { get; set; }

        public int NovelId { get; set; }
        public virtual Novel Novel { get; set; } = null!;

        public int ChapterNumber { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty; // Protected chapter text content

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } = 0.00m;

        public bool IsFree { get; set; } = false;

        public bool IsPublished { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    }
}
