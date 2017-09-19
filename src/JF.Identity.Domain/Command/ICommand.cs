using System;
using System.Collections.Generic;
using System.Text;

namespace JF.Identity.Domain.Command
{
    public interface ICommand<out TResult>
    {
    }

    public interface ICommand : ICommand<CommandResult>
    {
    }
}
