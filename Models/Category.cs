using System.ComponentModel.DataAnnotations;

namespace NovelPlatform.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // e.g. خيال علمي, دراما ورومانسية, غموض وإثارة

        [MaxLength(250)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(50)]
        public string IconClass { get; set; } = "fas fa-book";

        public virtual ICollection<Novel> Novels { get; set; } = new List<Novel>();
    }
}
