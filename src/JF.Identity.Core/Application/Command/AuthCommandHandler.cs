using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JF.Common;
using JF.Domain.Command;

namespace JF.Identity.Core.Application.Command
{
    public class AuthCommandHandler: ICommandHandler<SignUpCommand>
    {
        public Task<CommandResult> HandleAsync(SignUpCommand command, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new CommandResult());
        }
    }
}
