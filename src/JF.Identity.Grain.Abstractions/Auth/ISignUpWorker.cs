using JF.Domain.Command;
using JF.Identity.Grain.Auth.Model;
using JF.Identity.Grain.Commands;

namespace JF.Identity.Grain
{
    public interface ISignUpWorker: 
        ICommandHandler<SignUpCommand>
    {
    }
}
