using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NovelPlatform.Models
{
    public enum OrderStatus
    {
        Pending,
        Completed,
        Failed,
        Cancelled
    }

    public class Order
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string OrderNumber { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser User { get; set; } = null!;

        public int ChapterId { get; set; }
        public virtual Chapter Chapter { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Payment? Payment { get; set; }
    }
}
