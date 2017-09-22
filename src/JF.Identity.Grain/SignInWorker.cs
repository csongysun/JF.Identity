using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JF.Identity.Grain.Auth.Model;
using JF.Identity.Grain.Commands;

namespace JF.Identity.Grain
{
    public class SignInWorker: ISignInWorker
    {
        public SignInWorker()
        {

        }

        public Task<SignInResult> HandleAsync(SignInCommand command)
        {
            
            throw new NotImplementedException();
        }
    }
}
