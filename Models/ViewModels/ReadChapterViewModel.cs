namespace NovelPlatform.Models.ViewModels
{
    public class ReadChapterViewModel
    {
        public Chapter Chapter { get; set; } = null!;
        public int? PreviousChapterId { get; set; }
        public int? NextChapterId { get; set; }
        public bool IsPurchased { get; set; }
    }
}
