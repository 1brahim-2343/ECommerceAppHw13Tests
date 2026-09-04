using ECommerceAppL8.Server.Entities;
using ECommerceAppL8.Server.Repositories.Abstract;
using ECommerceAppL8.Server.Services.Abstract;

namespace ECommerceAppL8.Server.Services.Concrete
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;

        public CartService(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }

        public async Task<Cart> AddCartAsync(Cart cart)
        {
            var result = await _cartRepo.AddCartAsync(cart);
            await _cartRepo.SaveChangesAsync();
            return result;
        }

        public async Task AddToCartAsync(CartItem item, Cart cart)
        {
            cart.Items.Add(item);
            await _cartRepo.SaveChangesAsync();
        }

        public async Task ClearCartAsync(Cart cart)
        {
            await _cartRepo.ClearCartAsync(cart);
        }

        public async Task<Cart?> GetCartAsync(int userId)
        {
            var result = await _cartRepo.GetCartAsync(userId);
            return result;
        }

        public async Task<CartItem?> GetCartItemAsync(int userId, int productId)
        {
            var result = await _cartRepo.GetCartItemAsync(userId, productId);
            return result;
        }

        public async Task RemoveFromCartAsync(CartItem cartItem)
        {
            await _cartRepo.RemoveFromCartAsync(cartItem);
        }

        public async Task UpdateQuantityAsync(CartItem cartItem, int quantity)
        {
            cartItem.Quantity = quantity;
            await _cartRepo.SaveChangesAsync();
        }
    }
}
