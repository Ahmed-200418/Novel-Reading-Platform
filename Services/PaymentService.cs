using Microsoft.EntityFrameworkCore;
using NovelPlatform.Data;
using NovelPlatform.Models;

namespace NovelPlatform.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(string userId, int chapterId)
        {
            var chapter = await _context.Chapters.FindAsync(chapterId);
            if (chapter == null)
            {
                throw new InvalidOperationException("الفصل غير موجود");
            }

            // Check if already purchased
            var existingPurchase = await _context.Purchases
                .FirstOrDefaultAsync(p => p.UserId == userId && p.ChapterId == chapterId);
            if (existingPurchase != null)
            {
                throw new InvalidOperationException("لقد قمت بشراء هذا الفصل بالفعل.");
            }

            // Check if there is an existing pending order
            var pendingOrder = await _context.Orders
                .FirstOrDefaultAsync(o => o.UserId == userId && o.ChapterId == chapterId && o.Status == OrderStatus.Pending);

            if (pendingOrder != null)
            {
                return pendingOrder;
            }

            var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";

            var order = new Order
            {
                OrderNumber = orderNumber,
                UserId = userId,
                ChapterId = chapterId,
                Amount = chapter.Price,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<bool> ProcessPaymentAsync(int orderId, string paymentMethod, string transactionRef)
        {
            var order = await _context.Orders
                .Include(o => o.Chapter)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null || order.Status == OrderStatus.Completed)
            {
                return false;
            }

            // 1. Create Payment record
            var payment = new Payment
            {
                OrderId = order.Id,
                Provider = "بوابة سداد الروايات المباشرة",
                PaymentMethod = paymentMethod,
                TransactionReference = string.IsNullOrWhiteSpace(transactionRef) 
                    ? $"TXN-{Guid.NewGuid().ToString("N")[..10].ToUpper()}" 
                    : transactionRef,
                Status = "Success",
                Amount = order.Amount,
                PaidAt = DateTime.UtcNow
            };

            await _context.Payments.AddAsync(payment);

            // 2. Create Purchase ownership record if not already created
            var existingPurchase = await _context.Purchases
                .FirstOrDefaultAsync(p => p.UserId == order.UserId && p.ChapterId == order.ChapterId);

            if (existingPurchase == null)
            {
                var purchase = new Purchase
                {
                    UserId = order.UserId,
                    ChapterId = order.ChapterId,
                    PricePaid = order.Amount,
                    PurchasedAt = DateTime.UtcNow
                };
                await _context.Purchases.AddAsync(purchase);
            }

            // 3. Mark order as completed
            order.Status = OrderStatus.Completed;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasUserPurchasedChapterAsync(string userId, int chapterId)
        {
            var chapter = await _context.Chapters.FindAsync(chapterId);
            if (chapter == null) return false;

            // Free chapters are accessible to everyone
            if (chapter.IsFree) return true;

            if (string.IsNullOrEmpty(userId)) return false;

            return await _context.Purchases
                .AnyAsync(p => p.UserId == userId && p.ChapterId == chapterId);
        }

        public async Task<List<Purchase>> GetUserPurchasesAsync(string userId)
        {
            return await _context.Purchases
                .Include(p => p.Chapter)
                .ThenInclude(c => c.Novel)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.PurchasedAt)
                .ToListAsync();
        }
    }
}
