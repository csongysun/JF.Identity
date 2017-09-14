using System;
using System.Collections.Generic;
using System.Text;

namespace JF.Identity.Grain
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
