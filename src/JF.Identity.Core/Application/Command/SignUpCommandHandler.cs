using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JF.Common;
using JF.Domain.Command;

namespace JF.Identity.Core.Application.Command
{
    public class SignUpCommandHandler: ICommandHandler<SignUpCommand, XError>
    {
        public Task<XError> HandleAsync(SignUpCommand command, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(XError.Ok);
        }
    }
}
