using ECommerceAppL8.Server.Data;
using ECommerceAppL8.Server.Entities;

namespace ECommerceAppL8.Server.Repositories.Abstract
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartAsync(int userId);
        Task<Cart> AddCartAsync(Cart cart);
        Task RemoveFromCartAsync(CartItem cartItem);
        Task ClearCartAsync(Cart cart);
        Task<CartItem?> GetCartItemAsync(int userId, int productId);
        Task<bool> SaveChangesAsync();
    }
}
