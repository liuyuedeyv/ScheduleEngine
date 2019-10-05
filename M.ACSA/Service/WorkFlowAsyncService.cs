using Autofac;
using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using M.WorkFlow.Engine;
using M.WorkFlow.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using System.IO;

namespace M.WorkFlow
{
    public class WorkFlowAsyncService : CustomTimer, IRegisterModuleType
    {
        uint batchCount = 20;
        public bool DisabledDefaultRegister => false;

        DataAccess _dataAccess;
        IWorkFlow _workflow;

        public void Application_Started(IApplicationBuilder app)
        {
            _dataAccess = app.ApplicationServices.GetService<DataAccess>();
            _workflow = app.ApplicationServices.GetService<IWorkFlow>();
        }

        public override int ExeTask()
        {
            var filter = TableFilter.New().Equals("status", 0);//.Equals("waitcallback", 0);
            var jobs = _dataAccess.Query(WFTEventEntity.TableCode).FixField("*").Paging(1, batchCount).Where(filter).QueryList<WFTEventEntity>();


            Parallel.ForEach<WFTEventEntity>(jobs,
                new ParallelOptions()
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                }, (job) =>
                {
                    _workflow.Run(job);
                });
            return batchCount == jobs.Count() ? 0 : 10;
        }

        public void Register(ContainerBuilder services, IConfiguration configuration)
        {
            RegisterBllModule(services, "M.WFEngine.dll");
        }

        /// <summary>
        /// 后续此功能增加到程序集
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="dllName"></param>
        private void RegisterBllModule(ContainerBuilder containerBuilder, string dllName)
        {
            var basePath = AppContext.BaseDirectory;
            Assembly moduleAssembly = null;
            moduleAssembly = Assembly.LoadFrom(Path.Combine(basePath, dllName));

            //autowired  class实现
            var propertySelector = new AutowiredPropertySelector();

            var typeAutowired = moduleAssembly.GetTypes().Where(t => t.GetCustomAttribute<AutowiredAttribute>() != null && !t.GetTypeInfo().IsAbstract);
            foreach (var t in typeAutowired)
            {
                if (t.GetInterfaces().Count() == 0)
                {
                    containerBuilder.RegisterType(t).PropertiesAutowired(propertySelector, false).InstancePerLifetimeScope();
                }
                else
                {
                    containerBuilder.RegisterType(t).AsImplementedInterfaces().PropertiesAutowired(propertySelector).InstancePerLifetimeScope();
                }
            }
        }
    }
}
