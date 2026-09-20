using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovelPlatform.Data;
using NovelPlatform.Models.ViewModels;

namespace NovelPlatform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalRevenue = await _context.Payments
                .Where(p => p.Status == "Success")
                .SumAsync(p => (decimal?)p.Amount) ?? 0m;

            var totalNovels = await _context.Novels.CountAsync();
            var totalChapters = await _context.Chapters.CountAsync();
            var totalPurchases = await _context.Purchases.CountAsync();
            var totalUsers = await _context.Users.CountAsync();

            var recentOrders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Chapter)
                .ThenInclude(c => c.Novel)
                .OrderByDescending(o => o.CreatedAt)
                .Take(10)
                .ToListAsync();

            var recentPurchases = await _context.Purchases
                .Include(p => p.User)
                .Include(p => p.Chapter)
                .ThenInclude(c => c.Novel)
                .OrderByDescending(p => p.PurchasedAt)
                .Take(10)
                .ToListAsync();

            var topNovels = await _context.Novels
                .Include(n => n.Category)
                .Include(n => n.Chapters)
                .OrderByDescending(n => n.ViewsCount)
                .Take(5)
                .ToListAsync();

            var viewModel = new AdminDashboardViewModel
            {
                TotalRevenue = totalRevenue,
                TotalNovels = totalNovels,
                TotalChapters = totalChapters,
                TotalPurchases = totalPurchases,
                TotalUsers = totalUsers,
                RecentOrders = recentOrders,
                RecentPurchases = recentPurchases,
                TopNovels = topNovels
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Users()
        {
            var users = await _context.Users
                .Include(u => u.Purchases)
                .Include(u => u.Orders)
                .OrderByDescending(u => u.RegisteredAt)
                .ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> Transactions()
        {
            var payments = await _context.Payments
                .Include(p => p.Order)
                .ThenInclude(o => o.User)
                .Include(p => p.Order)
                .ThenInclude(o => o.Chapter)
                .ThenInclude(c => c.Novel)
                .OrderByDescending(p => p.PaidAt)
                .ToListAsync();

            return View(payments);
        }
    }
}
