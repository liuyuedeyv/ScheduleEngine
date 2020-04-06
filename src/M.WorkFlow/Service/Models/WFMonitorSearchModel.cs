using FD.Simple.Utils.Agent;

namespace M.WFDesigner.Repository
{
    public class WFMonitorSearchModel : PageEntity
    {
        /// <summary>
        /// 流程服务ID查询流程中数据
        /// </summary>
        public string ServiceId { get; set; }
        /// <summary>
        /// 根据业务ID查询流程中数据
        /// </summary>
        public string DataId { get; set; }
        /// <summary>
        /// 流程名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
    }


    public class PageEntity : IFooParameter
    {
        public uint PageIndex { get; set; }

        public uint PageSize { get; set; }
    }
}
