using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ProtoBuf;

namespace JF.Identity.Api.Model.Req
{
    [ProtoContract]
    public class SignUpReq
    {
        [EmailAddress]
        [Required]
        [ProtoMember(1)]
        public string Email { get; set; }
        [Required]
        [ProtoMember(2)]
        public string Password { get; set; }
        [Required]
        [ProtoMember(3)]
        public string Nickname { get; set; }
    }

}
