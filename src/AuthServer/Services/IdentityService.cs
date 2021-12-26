using AuthServer.Infrastructure;
using AuthServer.Interfaces;
using AuthServer.Models.Requests;
using AuthServer.Models.Responses;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly Authenticator _authenticator;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly TokenManager _tokenManager;

        public IdentityService(
            IRefreshTokenService refreshTokenService,
            Authenticator authenticator,
            UserManager<User> userManager,
            TokenManager tokenManager,
            SignInManager<User> signInManager)
        {
            _refreshTokenService = refreshTokenService;
            _authenticator = authenticator;
            _userManager = userManager;
            _tokenManager = tokenManager;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                var resultLogin = await _userManager.CheckPasswordAsync(user, request.Password);

                if (resultLogin)
                    return await _authenticator.Authenticate(user);
                else
                    return new AuthResponse { Errors = new[] { "Login or Password don't correct." } };
            }
            else
                return new AuthResponse { Errors = new[] { "Login or Password don't correct." } };
        }
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
                return new AuthResponse { Errors = new[] { "User with this Email already exists." } };

            if (request.Password != request.ConfirmPassword)
                return new AuthResponse { Errors = new[] { "Password mismatch." } };

            var user = new User
            {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber
            };

            var resultCreate = await _userManager.CreateAsync(user, request.Password);

            if (!resultCreate.Succeeded)
                return new AuthResponse { Errors = resultCreate.Errors.Select(e => e.Description) };

            return new AuthResponse { Success = true };
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshRequest request)
        {
            bool isValidRefreshToken = _tokenManager.ValidateRefreshToken(request.RefreshToken);

            if (!isValidRefreshToken)
                return new AuthResponse { Errors = new[] { "Refresh Token not found" } };

            var refreshToken = await _refreshTokenService.GetByTokenAsync(request.RefreshToken);

            if (refreshToken is null)
                return new AuthResponse { Errors = new[] { "Refresh not found" } };

            await _refreshTokenService.DeleteAsync(refreshToken.UserId);

            var user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());

            if (user == null)
                return new AuthResponse { Errors = new[] { "User not found" } };

            return await _authenticator.Authenticate(user);

        }

    }
}
