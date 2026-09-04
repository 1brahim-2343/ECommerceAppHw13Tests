using ECommerceAppL8.Server.Entities;

namespace ECommerceAppL8.Server.Services.Abstract
{
    public interface IJwtService
    {
        Task<string> GenerateAccessTokenAsync(ApplicationUser user);
        DateTime GetAccessTokenExpiration();
    }
}
