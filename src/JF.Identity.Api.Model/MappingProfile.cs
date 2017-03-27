﻿using AutoMapper;
using JF.Identity.Proto;
using JF.Identity.Store.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace JF.Identity.Api.Model
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SignUpReq, User>()
                .ConstructUsing(model => new User
                {
                    Email = model.Email
                });
            CreateMap<User, UserRes>()
                .ConvertUsing(model => new UserRes
                {
                    Id = model.Id,
                    Nickname = model.Nickname,
                    Email = model.Email,
                    RefreshToken = model.RefreshToken,
                    Token = model.Token,
                    TokenExpires = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(model.TokenExpires)
                });
        }
    }
}