using System;
using System.Collections.Generic;
using System.Text;
using JF.Domain.Command;

namespace JF.Identity.Grain.Auth.Model
{
    public class SignInResult : CommandResult<UserInfo>
    {
    }

}
