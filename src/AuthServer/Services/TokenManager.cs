using AuthServer.Configuration;
using AuthServer.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthServer.Services
{
    public class TokenManager
    {
        private readonly AuthConfiguration _auth;
        public TokenManager(IOptions<AuthConfiguration> options)
        {
            _auth = options.Value;
        }

        public string GenerateToken(IEnumerable<Claim> claims = null, bool isRefresh = false)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(isRefresh ?
                                                                              _auth.RefreshTokenSecret :
                                                                              _auth.AccessTokenSecret));

            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                _auth.Issuer,
                _auth.Audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(_auth.AccessTokenExpirationMinutes),
                signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return GenerateToken(isRefresh: true);
        }

        public bool ValidateRefreshToken(string refreshToken)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_auth.RefreshTokenSecret)),
                ValidIssuer = _auth.Issuer,
                ValidAudience = _auth.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                new JwtSecurityTokenHandler().ValidateToken(refreshToken, validationParameters, out var st);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GenerateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "User")
            };

            return GenerateToken(claims);
        }
    }
}
