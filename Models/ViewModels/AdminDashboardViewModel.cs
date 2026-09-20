namespace NovelPlatform.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public decimal TotalRevenue { get; set; }
        public int TotalNovels { get; set; }
        public int TotalChapters { get; set; }
        public int TotalPurchases { get; set; }
        public int TotalUsers { get; set; }

        public List<Order> RecentOrders { get; set; } = new List<Order>();
        public List<Purchase> RecentPurchases { get; set; } = new List<Purchase>();
        public List<Novel> TopNovels { get; set; } = new List<Novel>();
    }
}
