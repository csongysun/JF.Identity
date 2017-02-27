using CSYS.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace JF.Identity.Api
{
    public static class ApiErrorDescriber
    {
        public static Error DefaultError => new Error
        {
            Code = nameof(DefaultError),
            Description = Resources.DefaultError
        };
        public static Error ModelNotValid => new Error
        {
            Code = nameof(ModelNotValid),
            Description = Resources.ModelNotValid
        };
        public static Error SignUpNotOpen => new Error
        {
            Code = nameof(SignUpNotOpen),
            Description = Resources.SignUpNotOpen
        };
        public static Error EntityNotFound(string key, string Name) => new Error
        {
            Code = nameof(EntityNotFound),
            Description = Resources.EntityNotFound(key, Name)
        };

    }
}
