using JF.Identity.Common;
using JF.Identity.Store.Model;
using System.Threading.Tasks;

namespace JF.Identity.Manager
{
    public interface ISignInManager
    {

        Task<(XError err, User user)> PasswordSignInAsync(string key, string password);

        /// <summary>
        /// Login by refresh token.
        /// </summary>
        /// <param name="refreshToken">refresh token</param>
        /// <returns>Error msg or the logined user</returns>
        Task<(XError err, User user)> RefreshLoginAsync(string refreshToken);

        Task SignOutAsync();

    }
}
