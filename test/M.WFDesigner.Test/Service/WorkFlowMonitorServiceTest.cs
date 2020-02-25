
using System;
using Autofac;
using GW.ApiLoader.Utils;
using M.WFDesigner.Repository;
using M.WFDesigner.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace M.WFDesigner.Test.Service
{
    [TestClass]
    public class WorkFlowMonitorServiceTest:UnitTestBase
    {
        private WorkFlowMonitorService service
        {
            get { return this.Container.Resolve<WorkFlowMonitorService>(); }
        }

        #region 获取存活任务接口测试

        /// <summary>
        /// 获取存活任务接口测试
        /// </summary>
        [TestMethod]
        public void GetFlowsByServiceIdoTest()
        {
            WFMonitorSearchModel en=new WFMonitorSearchModel();
            en.DataId = "10001Q4QZCBR50000A01";
            en.Status = -999;
            var res = this.service.GetFlowsByServiceIdo(en);

            Console.WriteLine(this.Json.Serialize(res));
        }

        #endregion


        #region 流程运行数据接口

        /// <summary>
        /// 流程运行数据接口
        /// </summary>
        [TestMethod]
        public void GetWfInsExecInfoTest()
        {
            string finsId = "00001Q4QZCBR50000A01";
            var res = this.service.GetWfInsExecInfo(finsId);

            Console.WriteLine(this.Json.Serialize(res));
        }

        #endregion
    }
}
