﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace Web.Configuration
{
    public class RevokeAuthenticationEvents : CookieAuthenticationEvents
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger _logger;

        public RevokeAuthenticationEvents(IMemoryCache cache, ILogger<RevokeAuthenticationEvents> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var userId = context.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            var identityKey = context.Request.Cookies[ConfigureCookieSettings.IdentifierCookieName];

            if (_cache.TryGetValue($"{userId.Value}:{identityKey}", out var revokeKeys))
            {
                _logger.LogDebug($"Access has been revoked for: {userId.Value}.");
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}
