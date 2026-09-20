using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovelPlatform.Data;
using NovelPlatform.Models;

namespace NovelPlatform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminNovelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminNovelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminNovels
        public async Task<IActionResult> Index()
        {
            var novels = await _context.Novels
                .Include(n => n.Category)
                .Include(n => n.Chapters)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return View(novels);
        }

        // GET: Admin/AdminNovels/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(new Novel());
        }

        // POST: Admin/AdminNovels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Novel novel)
        {
            if (ModelState.IsValid)
            {
                novel.CreatedAt = DateTime.UtcNow;
                if (string.IsNullOrWhiteSpace(novel.CoverImageUrl))
                {
                    novel.CoverImageUrl = "https://images.unsplash.com/photo-1544716278-ca5e3f4abd8c?auto=format&fit=crop&w=600&q=80";
                }
                _context.Novels.Add(novel);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "تم إضافة الرواية بنجاح!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(novel);
        }

        // GET: Admin/AdminNovels/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var novel = await _context.Novels.FindAsync(id);
            if (novel == null) return NotFound();

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(novel);
        }

        // POST: Admin/AdminNovels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Novel novel)
        {
            if (id != novel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(novel);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "تم تحديث بيانات الرواية بنجاح!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Novels.AnyAsync(e => e.Id == novel.Id))
                        return NotFound();
                    else
                        throw;
                }
            }
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(novel);
        }

        // POST: Admin/AdminNovels/TogglePublish/5
        [HttpPost]
        public async Task<IActionResult> TogglePublish(int id)
        {
            var novel = await _context.Novels.FindAsync(id);
            if (novel == null) return NotFound();

            novel.IsPublished = !novel.IsPublished;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = novel.IsPublished ? "تم نشر الرواية." : "تم إلغاء نشر الرواية.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/AdminNovels/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var novel = await _context.Novels.FindAsync(id);
            if (novel != null)
            {
                _context.Novels.Remove(novel);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "تم حذف الرواية بنجاح.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
