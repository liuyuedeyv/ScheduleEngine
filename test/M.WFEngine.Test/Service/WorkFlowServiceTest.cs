using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Autofac;
using FD.Simple.Utils;
using GW.ApiLoader.Utils;
using M.WFEngine.Flow;
using M.WFEngine.Flow.DAL;
using M.WFEngine.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace M.WFEngine.Test.Service
{
    [TestClass]
    public class WorkFlowServiceTest:UnitTestBase
    {
        private WorkFlowService service
        {
            get { return this.Container.Resolve<WorkFlowService>(); }
        }

        #region 异步写入错误信息测试

        /// <summary>
        /// 异步写入错误信息测试
        /// </summary>
        [TestMethod]
        public void CallbackWFForTaskErrorTest()
        {
            string mqId = "00001Q4QZCCK80000A04";
            string errorMsg = Guid.NewGuid().ToString();
            this.UpdateWfTeventData(mqId, 1, 0);
            var res=this.service.CallbackWFForTaskError(mqId, errorMsg);
            Assert.IsTrue(res.WarnResult.Message.IndexOf("已经回调,无法继续处理") > 1);

            this.UpdateWfTeventData(mqId, 1, 1);
            res = this.service.CallbackWFForTaskError(mqId, errorMsg);
            Console.WriteLine(this.Json.Serialize(res));
            Assert.AreEqual(null,res.WarnResult);

            res = this.service.CallbackWFForTaskError(mqId, errorMsg);
            Assert.IsTrue(res.WarnResult.Message.IndexOf("已标记为异常")>1);


        }
        private void UpdateWfTeventData(string id,int status,int waitcallback)
        {
            this._db.ExecuteNonQuery($"update wftevent set status='{status}',waitcallback={waitcallback} where id='{id}'");
        }
        #endregion

        #region 废弃操作

        /// <summary>
        /// 废弃操作
        /// </summary>
        [TestMethod]
        public void GiveUpTest()
        {
            //1 查找一个审批中的单子
            DataTable dt = this._db.ExecuteDataset("select e.id,e.finsid from wftevent e left join wffins as f on e.finsid=f.id where f.status=1 order by f.cdate asc limit 1;").Tables[0];
            var mqid = dt.Rows[0]["id"].ConvertTostring();
            var finsid = dt.Rows[0]["finsid"].ConvertTostring(); 
            var res= this.service.giveup(mqid, "测试废弃");

            Console.WriteLine(this.Json.Serialize(res));
            Assert.AreEqual(null,res.WarnResult);
            var wffins = this.Container.Resolve<WfFinsDal>().Get(finsid);
            Assert.AreEqual(EFlowStatus.GiveUp.GetHashCode(),wffins.Status);

        }

        #endregion
    }
}
