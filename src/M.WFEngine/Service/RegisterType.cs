using Autofac;
using FD.Simple.Utils.Agent;
using M.WFEngine.Flow;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace M.WFEngine.Service
{
    public class RegisterType : CustomTimer, IRegisterType
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

        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            //TODO:此代码应该增加到容器中
            services.AddHttpClient();
        }
    }
}
