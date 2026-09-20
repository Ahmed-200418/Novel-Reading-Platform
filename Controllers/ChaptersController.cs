using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovelPlatform.Data;
using NovelPlatform.Models;
using NovelPlatform.Models.ViewModels;
using NovelPlatform.Services;

namespace NovelPlatform.Controllers
{
    public class ChaptersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaymentService _paymentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChaptersController(
            ApplicationDbContext context,
            IPaymentService paymentService,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _paymentService = paymentService;
            _userManager = userManager;
        }

        // GET: /Chapters/Read/5
        public async Task<IActionResult> Read(int id)
        {
            var chapter = await _context.Chapters
                .Include(c => c.Novel)
                .ThenInclude(n => n.Chapters)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsPublished);

            if (chapter == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            // Server-Side Access Control Check
            bool isPurchased = chapter.IsFree || isAdmin;

            if (!isPurchased && !string.IsNullOrEmpty(userId))
            {
                isPurchased = await _paymentService.HasUserPurchasedChapterAsync(userId, id);
            }

            // Find Prev / Next chapters in the same novel
            var sortedChapters = chapter.Novel.Chapters
                .Where(c => c.IsPublished)
                .OrderBy(c => c.ChapterNumber)
                .ToList();

            var currentIndex = sortedChapters.FindIndex(c => c.Id == id);
            int? prevId = currentIndex > 0 ? sortedChapters[currentIndex - 1].Id : null;
            int? nextId = currentIndex < sortedChapters.Count - 1 ? sortedChapters[currentIndex + 1].Id : null;

            var viewModel = new ReadChapterViewModel
            {
                Chapter = chapter,
                PreviousChapterId = prevId,
                NextChapterId = nextId,
                IsPurchased = isPurchased
            };

            return View(viewModel);
        }
    }
}
