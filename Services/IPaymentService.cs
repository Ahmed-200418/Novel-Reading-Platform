using NovelPlatform.Models;

namespace NovelPlatform.Services
{
    public interface IPaymentService
    {
        Task<Order> CreateOrderAsync(string userId, int chapterId);
        Task<bool> ProcessPaymentAsync(int orderId, string paymentMethod, string transactionRef);
        Task<bool> HasUserPurchasedChapterAsync(string userId, int chapterId);
        Task<List<Purchase>> GetUserPurchasesAsync(string userId);
    }
}
