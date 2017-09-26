using System;
using System.Collections.Generic;
using System.Text;
using JF.Domain.Command;

namespace JF.Identity.Grain.Auth.Model
{
    public class SignInResult : CommandResult<UserInfo>
    {
        public new static SignInResult Ok(UserInfo payload) => new SignInResult
        {
            Succeed = true,
            Payload = payload
        };
        public static SignInResult UserNotFound = new SignInResult
        {
            Succeed = false,
            ErrorMessage = nameof(UserNotFound)
        };
    }

}
