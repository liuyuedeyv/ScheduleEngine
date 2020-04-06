using Autofac;
using FD.Simple.Utils.Agent;
using M.WFEngine.Flow;
using M.WFEngine.Task;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;


namespace M.WFEngine.Service
{
    public class RegisterType : CustomTimer, IRegisterType, IRegisterModuleType
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
            //var actualBatchCount = _workflow.ProcessWftEvent(batchCount);
            //return batchCount == actualBatchCount ? 0 : 10;
            return 10;
        }

        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            //重试三次的策略，间隔时间1S、2S、3S秒
            services.AddHttpClient<TaskWorkAsyncSendHttp>().
                AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000 * _)));
            //TODO:此代码应该增加到容器中
            services.AddHttpClient();
        }

        public void Register(ContainerBuilder services, IConfiguration configuration)
        {
            AutowiredPropertySelector val = new AutowiredPropertySelector();

            services.RegisterType<TaskEnd>().AsImplementedInterfaces().PropertiesAutowired(val).SingleInstance();
            services.RegisterType<TaskJuHe>().AsImplementedInterfaces().PropertiesAutowired(val).SingleInstance();
            services.RegisterType<TaskModel>().AsImplementedInterfaces().PropertiesAutowired(val).SingleInstance();
            services.RegisterType<TaskParallel>().AsImplementedInterfaces().PropertiesAutowired(val).SingleInstance();
            services.RegisterType<TaskStart>().AsImplementedInterfaces().PropertiesAutowired(val).SingleInstance();
            services.RegisterType<TaskWork>().AsImplementedInterfaces().PropertiesAutowired(val).SingleInstance();
            services.RegisterType<TaskWorkAsyncSendHttp>().AsImplementedInterfaces().PropertiesAutowired(val).SingleInstance();
            services.RegisterType<TaskWorkAsyncSendMQ>().AsImplementedInterfaces().PropertiesAutowired(val).SingleInstance();
            services.RegisterType<TaskOperator>().AsImplementedInterfaces().PropertiesAutowired(val).SingleInstance();
        }
    }
}
