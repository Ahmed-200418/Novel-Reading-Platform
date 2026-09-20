using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovelPlatform.Data;
using NovelPlatform.Models;

namespace NovelPlatform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminChaptersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminChaptersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminChapters?novelId=1
        public async Task<IActionResult> Index(int novelId)
        {
            var novel = await _context.Novels
                .Include(n => n.Chapters.OrderBy(c => c.ChapterNumber))
                .FirstOrDefaultAsync(n => n.Id == novelId);

            if (novel == null) return NotFound();

            ViewBag.Novel = novel;
            return View(novel.Chapters.ToList());
        }

        // GET: Admin/AdminChapters/Create?novelId=1
        public async Task<IActionResult> Create(int novelId)
        {
            var novel = await _context.Novels.FindAsync(novelId);
            if (novel == null) return NotFound();

            var maxChapter = await _context.Chapters
                .Where(c => c.NovelId == novelId)
                .Select(c => (int?)c.ChapterNumber)
                .MaxAsync() ?? 0;

            var chapter = new Chapter
            {
                NovelId = novelId,
                ChapterNumber = maxChapter + 1,
                IsFree = false,
                Price = 20.00m
            };

            ViewBag.NovelTitle = novel.Title;
            return View(chapter);
        }

        // POST: Admin/AdminChapters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                chapter.CreatedAt = DateTime.UtcNow;
                if (chapter.IsFree)
                {
                    chapter.Price = 0.00m;
                }

                _context.Chapters.Add(chapter);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم إضافة الفصل جديد بنجاح!";
                return RedirectToAction(nameof(Index), new { novelId = chapter.NovelId });
            }

            var novel = await _context.Novels.FindAsync(chapter.NovelId);
            ViewBag.NovelTitle = novel?.Title ?? "";
            return View(chapter);
        }

        // GET: Admin/AdminChapters/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var chapter = await _context.Chapters
                .Include(c => c.Novel)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chapter == null) return NotFound();

            ViewBag.NovelTitle = chapter.Novel.Title;
            return View(chapter);
        }

        // POST: Admin/AdminChapters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Chapter chapter)
        {
            if (id != chapter.Id) return NotFound();

            if (ModelState.IsValid)
            {
                if (chapter.IsFree)
                {
                    chapter.Price = 0.00m;
                }

                _context.Update(chapter);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم تحديث بيانات الفصل بنجاح!";
                return RedirectToAction(nameof(Index), new { novelId = chapter.NovelId });
            }

            var novel = await _context.Novels.FindAsync(chapter.NovelId);
            ViewBag.NovelTitle = novel?.Title ?? "";
            return View(chapter);
        }

        // POST: Admin/AdminChapters/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter != null)
            {
                int novelId = chapter.NovelId;
                _context.Chapters.Remove(chapter);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم حذف الفصل بنجاح.";
                return RedirectToAction(nameof(Index), new { novelId });
            }
            return RedirectToAction("Index", "AdminNovels");
        }
    }
}
