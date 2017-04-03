using CSYS.Common;
using CSYS.Identity.Store;
using JF.Identity.Store.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JF.Identity.Store
{
    public interface IUserStore: IUserStore<User>
    {
        Task<(Error, User)> CreateAndRetrieveAsync(User user, CancellationToken cancellationToken = default(CancellationToken));
        Task<Error> SignInAsync(User user, CancellationToken cancellationToken = default(CancellationToken));
    }
}
