using System;
using System.Collections.Generic;
using System.Text;

namespace JF.Identity.Service
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);

    }
}
