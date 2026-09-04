using ECommerceAppL8.Server.Entities;

namespace ECommerceAppL8.Server.Services.Abstract
{
    public interface ICartService
    {
        Task<Cart?> GetCartAsync(int userId);
        Task<Cart> AddCartAsync(Cart cart);
        Task AddToCartAsync(CartItem item, Cart cart);
        Task UpdateQuantityAsync(CartItem cartItem, int quantity);
        Task RemoveFromCartAsync(CartItem cartItem);
        Task ClearCartAsync(Cart cart);
        Task<CartItem?> GetCartItemAsync(int userId, int productId);
    }
}
