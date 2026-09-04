using ECommerceAppL8.Server.Entities;

namespace ECommerceAppL8.Server.Services.Abstract
{
    public interface IRefreshTokenService
    {
        RefreshToken Generate(int userId);
    }
}
