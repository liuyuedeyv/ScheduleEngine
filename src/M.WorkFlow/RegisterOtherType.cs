using System;
using System.Collections.Generic;
using System.Text;
using FD.Simple.Utils.Agent;
using M.WFDesigner.RefData;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace M.WFDesigner
{
    public class RegisterOtherType:IRegisterType
    {
        public void Register(IServiceCollection services, IConfiguration configuration)
        { 
            services.AddSingleton<ICache, CacheFactory>();
            services.AddRefdata();
        }

        public void Application_Started(IApplicationBuilder app)
        {
             
        }

        public bool DisabledDefaultRegister { get; }
    }
}
