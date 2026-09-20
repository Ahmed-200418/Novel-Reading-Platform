using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace NovelPlatform.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    }
}
