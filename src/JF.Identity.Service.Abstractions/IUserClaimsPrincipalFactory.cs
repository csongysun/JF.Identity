using JF.Identity.Store.Model;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JF.Identity.Service
{
    public interface IUserClaimsPrincipalFactory
    {
        Task<ClaimsPrincipal> CreateAsync(User user);
    }
}
