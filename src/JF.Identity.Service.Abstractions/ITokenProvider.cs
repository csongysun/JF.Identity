using JF.Identity.Common;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace JF.Identity.Service
{
    /// <summary>
    /// Token provider, the token is string and contains claims, could expired.
    /// </summary>
    public interface ITokenProvider
    {
        /// <summary>
        /// Generate a token.
        /// </summary>
        /// <param name="claims">the claims to be write to token.</param>
        /// <param name="option">the options that use to generate token</param>
        /// <returns>token string and expired time</returns>
        (string token, DateTimeOffset expireTime) Generate(IEnumerable<Claim> claims, TokenOption option);
        /// <summary>
        /// Validate a token.
        /// </summary>
        /// <param name="token">the token to be validated.</param>
        /// <param name="option">the options that use to validate the token</param>
        /// <returns>if the token is validated, return the claims payload, or return null when validate failed.</returns>
        ClaimsPrincipal Validate(string token, TokenOption option);
    }
}
