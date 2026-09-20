using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovelPlatform.Data;
using NovelPlatform.Models;
using NovelPlatform.Models.ViewModels;
using NovelPlatform.Services;

namespace NovelPlatform.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPaymentService _paymentService;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IPaymentService paymentService,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _paymentService = paymentService;
            _context = context;
        }

        // GET: /Account/Login
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "البريد الإلكتروني أو كلمة المرور غير صحيحة.");
            return View(model);
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                EmailConfirmed = true,
                RegisteredAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Reader");
                await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["SuccessMessage"] = $"مرحباً بك {user.FullName} في منصة كتاب!";
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["InfoMessage"] = "تم تسجيل الخروج بنجاح.";
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Library
        [Authorize]
        public async Task<IActionResult> Library()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var purchases = await _paymentService.GetUserPurchasesAsync(user.Id);
            var orders = await _context.Orders
                .Include(o => o.Chapter)
                .ThenInclude(c => c.Novel)
                .Include(o => o.Payment)
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            ViewBag.Orders = orders;
            ViewBag.UserFullName = user.FullName;
            ViewBag.UserEmail = user.Email;

            return View(purchases);
        }
    }
}
