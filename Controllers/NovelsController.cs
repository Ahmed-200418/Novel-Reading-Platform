using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovelPlatform.Data;
using NovelPlatform.Models;
using NovelPlatform.Models.ViewModels;

namespace NovelPlatform.Controllers
{
    public class NovelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NovelsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Novels
        public async Task<IActionResult> Index(int? categoryId, string? search)
        {
            var query = _context.Novels
                .Include(n => n.Category)
                .Include(n => n.Chapters)
                .Where(n => n.IsPublished)
                .AsQueryable();

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(n => n.CategoryId == categoryId.Value);
                ViewBag.SelectedCategoryId = categoryId.Value;
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(n => n.Title.Contains(search) || n.Author.Contains(search) || n.Description.Contains(search));
                ViewBag.SearchQuery = search;
            }

            var novels = await query.OrderByDescending(n => n.CreatedAt).ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View(novels);
        }

        // GET: /Novels/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var novel = await _context.Novels
                .Include(n => n.Category)
                .Include(n => n.Chapters.OrderBy(c => c.ChapterNumber))
                .FirstOrDefaultAsync(n => n.Id == id && n.IsPublished);

            if (novel == null)
            {
                return NotFound();
            }

            // Increment views count
            novel.ViewsCount++;
            await _context.SaveChangesAsync();

            var currentUserId = _userManager.GetUserId(User);
            var purchasedChapterIds = new HashSet<int>();

            if (!string.IsNullOrEmpty(currentUserId))
            {
                purchasedChapterIds = (await _context.Purchases
                    .Where(p => p.UserId == currentUserId)
                    .Select(p => p.ChapterId)
                    .ToListAsync()).ToHashSet();
            }

            var chapterItems = novel.Chapters.Select(c => new ChapterItemViewModel
            {
                Id = c.Id,
                ChapterNumber = c.ChapterNumber,
                Title = c.Title,
                Price = c.Price,
                IsFree = c.IsFree,
                IsPurchased = c.IsFree || purchasedChapterIds.Contains(c.Id),
                CreatedAt = c.CreatedAt
            }).ToList();

            var viewModel = new NovelDetailsViewModel
            {
                Novel = novel,
                Chapters = chapterItems
            };

            return View(viewModel);
        }
    }
}
