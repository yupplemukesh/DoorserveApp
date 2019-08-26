using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doorserve.Models;
using doorserve.Models.ClientData;

namespace doorserve.AutoMapper
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