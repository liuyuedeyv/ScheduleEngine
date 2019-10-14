//using Autofac;
//using FD.Simple.Utils.Agent;
//using M.WFEngine.Flow;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

//namespace M.ACSA.Service
//{
//    /**
//     * 流程引擎配置
//     * 1、实现抽象基类WFEngine.Service.WorkFlowService
//     * 2、配置定时执行event事件、注册引擎dll到容器
//     * */


//    public class WorkFlowService : WFEngine.Service.WorkFlowService
//    {
//        public override string GetAppName(string mqId)
//        {
//            return "acsa";
//        }
//    }
//    /// <summary>
//    /// 
//    /// </summary>
//    public class WFEngineSettingService : CustomTimer, IRegisterModuleType
//    {
//        public bool DisabledDefaultRegister => false;

//        IWorkFlow _workflow;
//        public void Application_Started(IApplicationBuilder app)
//        {
//            _workflow = app.ApplicationServices.GetService<IWorkFlow>();
//        }

//        public override int ExeTask()
//        {
//            uint batchCount = 20;
//            var actualBatchCount = _workflow.ProcessWftEvent(batchCount);
//            return batchCount == actualBatchCount ? 0 : 10;
//        }

//        public void Register(ContainerBuilder services, IConfiguration configuration)
//        {
//            services.RegisterBllModule("M.WFEngine.dll");
//        }
//    }
//}
