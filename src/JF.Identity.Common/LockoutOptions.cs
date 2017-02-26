using System;
using System.Collections.Generic;
using System.Text;

namespace JF.Identity.Common
{
    public class LockoutOptions
    {
        public bool AllowedForNewUsers { get; set; } = true;
        public int MaxFailedAccessAttempts { get; set; } = 5;
        public TimeSpan DefaultLockoutTimeSpan { get; set; } = TimeSpan.FromMinutes(5);
    }
}
