using AuthServer.Models.Requests;
using AuthServer.Models.Responses;

namespace AuthServer.Interfaces
{
    public interface IIdentityService 
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshTokenAsync(RefreshRequest request);
    }
}
