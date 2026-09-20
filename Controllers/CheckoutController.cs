using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovelPlatform.Data;
using NovelPlatform.Models;
using NovelPlatform.Services;

namespace NovelPlatform.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaymentService _paymentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutController(
            ApplicationDbContext context,
            IPaymentService paymentService,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _paymentService = paymentService;
            _userManager = userManager;
        }

        // GET: /Checkout/Buy/5
        public async Task<IActionResult> Buy(int chapterId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var chapter = await _context.Chapters
                .Include(c => c.Novel)
                .FirstOrDefaultAsync(c => c.Id == chapterId);

            if (chapter == null) return NotFound();

            // Check if already purchased
            bool alreadyOwned = await _paymentService.HasUserPurchasedChapterAsync(user.Id, chapterId);
            if (alreadyOwned)
            {
                TempData["InfoMessage"] = "أنت تمتلك هذا الفصل بالفعل!";
                return RedirectToAction("Read", "Chapters", new { id = chapterId });
            }

            try
            {
                var order = await _paymentService.CreateOrderAsync(user.Id, chapterId);
                ViewBag.Chapter = chapter;
                return View(order);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Details", "Novels", new { id = chapter.NovelId });
            }
        }

        // POST: /Checkout/ConfirmPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPayment(int orderId, string paymentMethod, string cardHolderName, string cardNumber)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var order = await _context.Orders
                .Include(o => o.Chapter)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == user.Id);

            if (order == null) return NotFound();

            string txnRef = $"TXN-{Guid.NewGuid().ToString("N")[..10].ToUpper()}";
            bool success = await _paymentService.ProcessPaymentAsync(orderId, paymentMethod ?? "بطاقة ائتمانية", txnRef);

            if (success)
            {
                TempData["SuccessMessage"] = "تمت عملية الدفع بنجاح! تم فتح الفصل وقرائته ممتعة.";
                return RedirectToAction("Read", "Chapters", new { id = order.ChapterId });
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء معالجة عملية الدفع. يرجى المحاولة مرة أخرى.";
                return RedirectToAction("Buy", new { chapterId = order.ChapterId });
            }
        }
    }
}
