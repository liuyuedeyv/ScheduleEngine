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

namespace M.WorkFlow
{
    public class WorkFlowAsyncService : CustomTimer, IRegisterModuleType
    {
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
            var filter = TableFilter.New().Equals("status", 0);
            var jobs = _dataAccess.Query(WFMQEntity.TableCode).FixField("*").Paging(1, 10).Where(filter).QueryList<WFMQEntity>();
            foreach (var job in jobs)
            {
                _workflow.Run(job);
            }
            return 1000;
        }

        public void Register(ContainerBuilder services, IConfiguration configuration)
        {

        }
    }
}
