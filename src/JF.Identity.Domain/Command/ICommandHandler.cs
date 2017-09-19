using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace JF.Identity.Domain.Command
{
    public interface ICommandHandler<in TCommand, TResult> : IGrainWithIntegerKey
        where TCommand : ICommand<TResult>
    {
        Task<TResult> HandleAsync(TCommand command);
    }

    public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, CommandResult>
        where TCommand : ICommand
    {
    }
}
