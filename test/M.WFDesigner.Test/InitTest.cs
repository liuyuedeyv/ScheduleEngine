using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using FD.Simple.DB;
using FD.Simple.Utils;
using GW.ApiLoader.TestUtils.Utils;
using GW.ApiLoader.Utils;
using M.WorkFlow.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace M.WFDesigner.Test
{
    /// <summary>
    /// 初始化测试
    /// </summary>
    [TestClass]
    public class InitTest : UnitTestBase
    {
        #region 创建实体类    

        /// <summary>
        /// 创建实体类
        /// </summary>
        [TestMethod]
        public void CreateEntity()
        {
            string slnName = "ACSA.sln"; //1.设置解决方案名称
            string nameSpace = "M11001.ACSA.Repository.Entity";//2.实体类的命名空间

            string path = Path.Combine("M11001.ACSA", "Repository", "Entity", "auto"); ; //3.设置实体类对象输出路径(相对于解决方案.sln的输出路径)
            IDBContext acs = this.Container.ResolveNamed<IDBContext>("acs");//4.数据库信息

            //5.设置表对象
            Dictionary<string, string> tables = new Dictionary<string, string>();
            tables.Add("ACSA_FLOW", "");
            tables.Add("ACSA_TASK", "");
            /*以下下不需要修改*/
            EntityHelper entityHelper = new EntityHelper(slnName, path, acs, EDbType.SqlServer);

            entityHelper.Tables = EntityHelper.UpdateTablesInfo(tables, entityHelper.GetACSTableInfo());//(非acs库.可以自行实现,或者直接赋值tables即可)
            entityHelper.NameSpace = nameSpace;
            entityHelper.CreateEntityFiles(entityHelper.GetACSColumnInfo());//(非acs库.可以自行实现,或者不传GetACSColumnInfo())

        }

        #endregion
        #region 创建实体类    

        /// <summary>
        /// 创建实体类
        /// </summary>
        [TestMethod]
        public void TestLog()
        {
            this.Log.LogDebug("Test");
            this.Log.LogError("测试", new Exception("1231"));
        }

        #endregion

        #region 数据库连接测试

        /// <summary>
        /// 数据库连接测试
        /// </summary>
        [TestMethod]
        public void TestDb()
        {

            Console.WriteLine("数据库连接字符串:" + this.Config[this.DefualtDbSetting]);
            var rtn = this._db.ExecuteScalar("select '数据库测试'").ConvertTostring();
            Console.WriteLine(rtn);
            Assert.AreEqual("数据库测试", rtn);
        }

        #endregion
        #region 数据库连接测试

        /// <summary>
        /// 数据库连接测试
        /// </summary>
        [TestMethod]
        public void JsonTest()
        {
            Dictionary<string, string> tables = new Dictionary<string, string>();
            tables.Add("TestKey", "TestValue");

            Console.WriteLine(this.Json.Serialize(tables));
        }

        #endregion

        #region MyRegion

        /// <summary>
        /// MyRegion
        /// </summary>
        [TestMethod]
        public void TestMethod()
        {
            var col = this._da.Query(WFTinsEntity.TableCode)
                .FixField("*")
                .Where(TableFilter.New().Equals("ID", "00001Q1FJ6JYK0000A03"))
                .Query<WFTinsEntity>();

            Console.WriteLine(this.Json.Serialize(col.Data.FirstOrDefault().Edate));
        }

        #endregion
    }
}
