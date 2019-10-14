using Autofac;
using FD.Simple.Utils.Agent;
using M.WFEngine.Flow;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WebApiClient;


namespace M.WFEngine.Service
{
    public class RegisterType : CustomTimer, IRegisterModuleType
    {
        public bool DisabledDefaultRegister => false;


        IWorkFlow _workflow;
        public void Application_Started(IApplicationBuilder app)
        {
            _workflow = app.ApplicationServices.GetService<IWorkFlow>();
        }
        public override int ExeTask()
        {
            uint batchCount = 20;
            var actualBatchCount = _workflow.ProcessWftEvent(batchCount);
            return batchCount == actualBatchCount ? 0 : 10;
        }

        public void Register(ContainerBuilder services, IConfiguration configuration)
        {
            string appUrl = "http://localhost:5004/acsa/registerwf";
            HttpApi.Register<IAppService>().ConfigureHttpApiConfig(c =>
            {
                c.HttpHost = new Uri(appUrl);
                c.FormatOptions.DateTimeFormat = DateTimeFormats.ISO8601_WithMillisecond;
            }); ;

        }

    }
}
