using ECommerceAppL8.Server.Data;
using ECommerceAppL8.Server.DTOs.Cart;
using ECommerceAppL8.Server.Entities;
using ECommerceAppL8.Server.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAppL8.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICartService _cartService;

        public CartController(AppDbContext context, ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cart = await _cartService.GetCartAsync(userId);

            if (cart is null)
            {
                return Ok(new
                {
                    id = 0,
                    userId,
                    items = Array.Empty<object>(),
                    total = 0
                });
            }

            var items = cart.Items.Select(x => new
            {
                id = x.Id,
                productId = x.ProductId,
                name = x.Product.Name,
                price = x.Product.Price,
                imageUrl = x.Product.ImageUrl,
                quantity = x.Quantity,
                subtotal = x.Product.Price * x.Quantity
            });

            var total = cart.Items.Sum(
                x => x.Product.Price * x.Quantity
            );

            return Ok(new
            {
                cart.Id,
                cart.UserId,
                items,
                total
            });
        }

        [HttpPost("{userId:int}/items")]
        public async Task<IActionResult> AddToCart(
            int userId,
            AddToCartDto dto)
        {
            if (dto.Quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == dto.ProductId);

            if (product is null)
                return NotFound("Product not found.");

            if (product.Stock < dto.Quantity)
                return BadRequest("Not enough stock.");

            var cart = await _cartService.GetCartAsync(userId);

            if (cart is null)
            {
                cart = new Cart
                {
                    UserId = userId
                };

                await _cartService.AddCartAsync(cart);
            }


            var existingItem = cart.Items
                .FirstOrDefault(x => x.ProductId == dto.ProductId);


            if (existingItem is not null)
            {
                if (existingItem.Quantity + dto.Quantity > product.Stock)
                    return BadRequest("Not enough stock.");

                var quantityToSet = existingItem.Quantity + dto.Quantity;

                await _cartService.UpdateQuantityAsync(existingItem, quantityToSet);
            }
            else
            {
                await _cartService.AddToCartAsync(new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                }, cart);
            }


            return Ok();
        }

        [HttpPut("{userId:int}/items/{productId:int}")]
        public async Task<IActionResult> UpdateQuantity(
            int userId,
            int productId,
            [FromQuery] int quantity)
        {
            if (quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

            var cartItem = await _cartService.GetCartItemAsync(userId, productId);

            if (cartItem is null)
                return NotFound("Cart item not found.");

            if (quantity > cartItem.Product.Stock)
                return BadRequest("Not enough stock.");

            await _cartService.UpdateQuantityAsync(cartItem, quantity);

            return Ok();
        }

        [HttpDelete("{userId:int}/items/{productId:int}")]
        public async Task<IActionResult> RemoveFromCart(
            int userId,
            int productId)
        {

            var cartItem = await _cartService.GetCartItemAsync(userId, productId);

            if (cartItem is null)
                return NotFound("Cart item not found.");

            await _cartService.RemoveFromCartAsync(cartItem);

            return NoContent();
        }

        [HttpDelete("{userId:int}")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            var cart = await _cartService.GetCartAsync(userId);

            if (cart is null)
                return NoContent();

            await _cartService.ClearCartAsync(cart);

            return NoContent();
        }
    }
}
