using Autofac;
using FD.Simple.Utils.Serialize;
using M.WFDesigner.Repository;
using Xunit;
using Xunit.Abstractions;

namespace M.WorkFlow.Test
{
    public class WorkFlowMonitorTest : BaseTest
    {
        public WorkFlowMonitorTest(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {

        }

        [Fact]
        public void Test1()
        {
            var service = _container.Resolve<IWorkFlowMonitor>();
            var json = _container.Resolve<IJsonConverter>();

            FD.Simple.DB.Model.PageEntity page = new FD.Simple.DB.Model.PageEntity();
            page.PageIndex = 1;
            page.PageSize = 10;
            var coll = service.GetRuningData(page);

            _testOutputHelper.WriteLine($"数据量为:{coll.RecordsTotal}");
            _testOutputHelper.WriteLine(json.Serialize(coll.Data));


            var serviceId = "00001PF1OUJQJ0000A02";
            WFMonitorSearchModel searchModel = new WFMonitorSearchModel();
            searchModel.PageIndex = 1;
            searchModel.PageSize = 10;
            searchModel.ServiceId = serviceId;
            coll = service.GetRuningDataByService(searchModel);
            _testOutputHelper.WriteLine($"数据量为:{coll.RecordsTotal}");
            _testOutputHelper.WriteLine(json.Serialize(coll.Data));

            foreach (var item in coll.Data)
            {
                Assert.Equal(serviceId, item.ServiceId);
            }
        }
    }
}
