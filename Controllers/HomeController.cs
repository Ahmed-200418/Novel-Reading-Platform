using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovelPlatform.Data;
using NovelPlatform.Models.ViewModels;

namespace NovelPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var featured = await _context.Novels
                .Include(n => n.Category)
                .Include(n => n.Chapters)
                .Where(n => n.IsPublished && n.IsFeatured)
                .OrderByDescending(n => n.Rating)
                .Take(4)
                .ToListAsync();

            var latest = await _context.Novels
                .Include(n => n.Category)
                .Include(n => n.Chapters)
                .Where(n => n.IsPublished)
                .OrderByDescending(n => n.CreatedAt)
                .Take(6)
                .ToListAsync();

            var categories = await _context.Categories
                .Include(c => c.Novels)
                .ToListAsync();

            var viewModel = new HomeViewModel
            {
                FeaturedNovels = featured,
                LatestNovels = latest,
                Categories = categories,
                TotalNovelsCount = await _context.Novels.CountAsync(n => n.IsPublished),
                TotalChaptersCount = await _context.Chapters.CountAsync(c => c.IsPublished),
                TotalReadersCount = await _context.Users.CountAsync()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
