using ECommerceAppL8.Server.Entities;
using ECommerceAppL8.Server.Repositories.Abstract;
using ECommerceAppL8.Server.Services.Concrete;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAppHw13.Tests.Services
{
    public class CartServiceTest
    {
        private Mock<ICartRepository> _cartRepositoryMock = null!;
        private CartService _cartService = null!;

        [SetUp]
        public void SetUp()
        {
            _cartRepositoryMock = new Mock<ICartRepository>();
            _cartService = new CartService(_cartRepositoryMock.Object);
        }

        [Test]
        public async Task AddCartAsync_Should_Return_Added_Cart()
        {
            //Arrange
            var cart = new Cart
            {
                Id = 1,
                UserId = 2,
                Items = null!
            };

            _cartRepositoryMock
                .Setup(x => x.AddCartAsync(cart))
                .ReturnsAsync(cart);

            //Act
            var result = await _cartService.AddCartAsync(cart);

            //Assert
            Assert.That(result, Is.EqualTo(cart));
        }

        [Test]
        public async Task AddToCartAsync_Should_Call_SaveChanges()
        {
            //Arrange

            var cartItem = new CartItem
            {
                Id = 1,
                CartId = 1,
                ProductId = 15,
                Quantity = 44
            };
            var cart = new Cart
            {
                Id = 1,
                Items = [cartItem],
                UserId = 1
            };

            _cartRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(true);

            //Act
            var result = _cartService.AddToCartAsync(cartItem, cart);

            //Assert

            _cartRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task ClearCartAsync_Should_Call_Repository()
        {
            //Arrange
            var cartItem = new CartItem
            {
                Id = 1,
                CartId = 1,
                ProductId = 15,
                Quantity = 44
            };
            var cart = new Cart
            {
                Id = 1,
                Items = [cartItem],
                UserId = 1
            };

            _cartRepositoryMock.Setup(x => x.ClearCartAsync(cart)).Returns(Task.CompletedTask);
            //Act
            await _cartService.ClearCartAsync(cart);

            //Assert
            _cartRepositoryMock.Verify(x => x.ClearCartAsync(cart), Times.Once);
        }

        [Test]
        public async Task RemoveFromCart_Should_Throw_When_Repository_Fails()
        {
            //Arrange

            var cartItem = new CartItem
            {
                Id = 1,
                CartId = 1,
                ProductId = 15,
                Quantity = 44
            };

            _cartRepositoryMock.Setup(x => x.RemoveFromCartAsync(cartItem))
                .ThrowsAsync(new Exception("Some error"));

            //Act and assert

            Assert.ThrowsAsync<Exception>(async () => await _cartService.RemoveFromCartAsync(cartItem));

        }

        [Test]
        public async Task UpdatedQuantityAsync_Should_Set_Value()
        {
            //Arrange and act
            var cartItem = new CartItem
            {
                Id = 1,
                CartId = 1,
                ProductId = 15,
                Quantity = 44
            };
            await _cartService.UpdateQuantityAsync(cartItem, 5);

            //Assert
            Assert.That(cartItem.Quantity, Is.EqualTo(5));
        }
    }
}
