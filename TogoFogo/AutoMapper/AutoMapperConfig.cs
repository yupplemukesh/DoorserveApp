using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TogoFogo.Models;

namespace TogoFogo.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SMSGateway, SMSGatewayModel>().ReverseMap();
            
        });
        }
    }
}