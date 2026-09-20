using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NovelPlatform.Models
{
    public class Purchase
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser User { get; set; } = null!;

        public int ChapterId { get; set; }
        public virtual Chapter Chapter { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePaid { get; set; }

        public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;
    }
}
