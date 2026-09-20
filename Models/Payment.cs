using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NovelPlatform.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Provider { get; set; } = "NovelsPay Gateway";

        [Required]
        [MaxLength(100)]
        public string TransactionReference { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } = "بطاقة ائتمانية"; // Card, Wallet, Fawry

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Success";

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime PaidAt { get; set; } = DateTime.UtcNow;
    }
}
