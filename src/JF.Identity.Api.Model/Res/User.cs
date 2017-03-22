using ProtoBuf;
using System;

namespace JF.Identity.Api.Model.Res
{
    [ProtoContract]
    public class UserRes
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        [ProtoMember(2)]
        public string Nickname { get; set; }
        [ProtoMember(3)]
        public string Email { get; set; }
        [ProtoMember(4)]
        public string RefreshToken { get; set; }
        [ProtoMember(5)]
        public string Token { get; set; }
        [ProtoMember(6)]
        public DateTime TokenExpires { get; set; }
    }
}
