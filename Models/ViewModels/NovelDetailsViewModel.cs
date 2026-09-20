namespace NovelPlatform.Models.ViewModels
{
    public class NovelDetailsViewModel
    {
        public Novel Novel { get; set; } = null!;
        public List<ChapterItemViewModel> Chapters { get; set; } = new List<ChapterItemViewModel>();
    }

    public class ChapterItemViewModel
    {
        public int Id { get; set; }
        public int ChapterNumber { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsFree { get; set; }
        public bool IsPurchased { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
