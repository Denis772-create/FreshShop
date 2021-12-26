using AuthServer.Infrastructure;
using AuthServer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        AuthDbContext _context;

        public RefreshTokenService(AuthDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(RefreshToken refreshToken)
        {
            try
            {
                await _context.RefreshTokens.AddAsync(refreshToken);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                await Task.FromException(e);
            }
        }

        public async Task DeleteAll(Guid userId)
        {
            try
            {
                IEnumerable<RefreshToken> refreshTokens = await _context.RefreshTokens
                 .Where(t => t.UserId == userId)
                 .ToListAsync();

                _context.RefreshTokens.RemoveRange(refreshTokens);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                await Task.FromException(e);
            }

        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var refreshToken = _context.RefreshTokens.FirstOrDefault(t => t.Id == id);
                if (refreshToken is null)
                    await Task.FromException(new Exception("refreshToken is null"));

                _context.RefreshTokens.Remove(refreshToken);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                await Task.FromException(e);
            }
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);

            if (refreshToken is null)
                await Task.FromException(new Exception("refreshToken is null"));

            return refreshToken;
        }
    }
}
