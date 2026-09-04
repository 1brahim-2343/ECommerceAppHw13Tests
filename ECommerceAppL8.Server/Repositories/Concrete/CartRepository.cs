using ECommerceAppL8.Server.Data;
using ECommerceAppL8.Server.Entities;
using ECommerceAppL8.Server.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAppL8.Server.Repositories.Concrete
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Cart> AddCartAsync(Cart cart)
        {
            var createdCart = (await _context.Carts.AddAsync(cart)).Entity;
            return createdCart;
        }

        public async Task ClearCartAsync(Cart cart)
        {
            _context.CartItems.RemoveRange(cart.Items);

            await _context.SaveChangesAsync();
        }

        public async Task<Cart?> GetCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        public async Task<CartItem?> GetCartItemAsync(int userId, int productId)
        {
            var cartItem = await _context.CartItems
                .Include(x => x.Cart)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x =>
                    x.Cart.UserId == userId &&
                    x.ProductId == productId);
            return cartItem;
        }

        public async Task RemoveFromCartAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);

           await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
           return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task UpdateQuantityAsync(CartItem cartItem, int quantity)
        {
            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();
        }
    }
}
