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
    /// ��ʼ������
    /// </summary>
    [TestClass]
    public class InitTest : UnitTestBase
    {
        #region ����ʵ����    

        /// <summary>
        /// ����ʵ����
        /// </summary>
        [TestMethod]
        public void CreateEntity()
        {
            string slnName = "ACSA.sln"; //1.���ý����������
            string nameSpace = "M11001.ACSA.Repository.Entity";//2.ʵ����������ռ�

            string path = Path.Combine("M11001.ACSA", "Repository", "Entity", "auto"); ; //3.����ʵ����������·��(����ڽ������.sln�����·��)
            IDBContext acs = this.Container.ResolveNamed<IDBContext>("acs");//4.���ݿ���Ϣ

            //5.���ñ����
            Dictionary<string, string> tables = new Dictionary<string, string>();
            tables.Add("ACSA_FLOW", "");
            tables.Add("ACSA_TASK", "");
            /*�����²���Ҫ�޸�*/
            EntityHelper entityHelper = new EntityHelper(slnName, path, acs, EDbType.SqlServer);

            entityHelper.Tables = EntityHelper.UpdateTablesInfo(tables, entityHelper.GetACSTableInfo());//(��acs��.��������ʵ��,����ֱ�Ӹ�ֵtables����)
            entityHelper.NameSpace = nameSpace;
            entityHelper.CreateEntityFiles(entityHelper.GetACSColumnInfo());//(��acs��.��������ʵ��,���߲���GetACSColumnInfo())

        }

        #endregion
        #region ����ʵ����    

        /// <summary>
        /// ����ʵ����
        /// </summary>
        [TestMethod]
        public void TestLog()
        {
            this.Log.LogDebug("Test");
            this.Log.LogError("����", new Exception("1231"));
        }

        #endregion

        #region ���ݿ����Ӳ���

        /// <summary>
        /// ���ݿ����Ӳ���
        /// </summary>
        [TestMethod]
        public void TestDb()
        {

            Console.WriteLine("���ݿ������ַ���:" + this.Config[this.DefualtDbSetting]);
            var rtn = this._db.ExecuteScalar("select '���ݿ����'").ConvertTostring();
            Console.WriteLine(rtn);
            Assert.AreEqual("���ݿ����", rtn);
        }

        #endregion
        #region ���ݿ����Ӳ���

        /// <summary>
        /// ���ݿ����Ӳ���
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
