namespace NovelPlatform.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Novel> FeaturedNovels { get; set; } = new List<Novel>();
        public List<Novel> LatestNovels { get; set; } = new List<Novel>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public int TotalNovelsCount { get; set; }
        public int TotalChaptersCount { get; set; }
        public int TotalReadersCount { get; set; }
    }
}
