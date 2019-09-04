using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doorserve.Models;
using doorserve.Models.ClientData;
using doorserve.Models.Gateway;

namespace doorserve.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<SMSGatewayModel, GatewayModel>().ReverseMap();
                cfg.CreateMap<SMTPGatewayModel, GatewayModel>().ReverseMap();
                cfg.CreateMap<NotificationGatewayModel, GatewayModel>().ReverseMap();
            });
        }
    }
}