using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TogoFogo.Models;
using TogoFogo.Models.ClientData;

namespace TogoFogo.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SMSGateway, SMSGatewayModel>().ReverseMap();
                cfg.CreateMap<SMSGateway, SMTPGatewayModel>().ReverseMap();

            });
        }
    }
}