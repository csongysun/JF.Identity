using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ProtoBuf;

namespace JF.Identity.Api.Model.Req
{

    [ProtoContract]
    public class LoginReq
    {
        [Required]
        [ProtoMember(1)]
        public string Email { get; set; }
        [Required]
        [ProtoMember(2)]
        public string Password { get; set; }
    }
}
