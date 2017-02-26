using AutoMapper;
using JF.Identity.Api.Model.Req;
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
            CreateMap<SignUpModel, User>()
                .ConstructUsing(model => new User
                {
                    Email = model.Email
                });
        }
    }
}
