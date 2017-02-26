using JF.Identity.Common;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JF.Identity.Service
{
    /// <summary>
    /// Json-Web-Token provider
    /// </summary>
    public class JwtTokenProvider : ITokenProvider
    {
        /// <summary>
        /// Generate a jwt token.
        /// </summary>
        /// <param name="claims">the claims to be write to token.</param>
        /// <param name="option">the options that use to generate token</param>
        /// <returns>token string and expired time</returns>
        public (string token, DateTimeOffset expireTime) Generate(IEnumerable<Claim> claims, TokenOption option)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(option.SecretKey));
            var expireTime = DateTimeOffset.UtcNow.Add(option.Expiration);
            var jwt = new JwtSecurityToken(
                issuer: option.Issuer,
                audience: option.Audience,
                claims: claims,
                expires: expireTime.UtcDateTime,
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
            );
            return (new JwtSecurityTokenHandler().WriteToken(jwt), expireTime);
        }

        /// <summary>
        /// Validate a jwt token.
        /// </summary>
        /// <param name="token">the token to be validated.</param>
        /// <param name="option">the options that use to validate the token</param>
        /// <returns>if the token is validated, return the claims payload, or return null when validate failed.</returns>
        public virtual ClaimsPrincipal Validate(string token, TokenOption option)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(option.SecretKey));
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = secretKey,
                ValidateIssuer = true,
                ValidIssuer = option.Issuer,
                ValidateAudience = true,
                ValidAudience = option.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            try
            {
                var principal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out var validatedToken);
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
