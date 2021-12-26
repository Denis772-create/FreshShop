using AuthServer.Infrastructure;

namespace AuthServer.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> GetByTokenAsync(string token);
        Task CreateAsync(RefreshToken refreshToken);
        Task DeleteAsync(Guid id);
        Task DeleteAll(Guid userId);
    }
}
