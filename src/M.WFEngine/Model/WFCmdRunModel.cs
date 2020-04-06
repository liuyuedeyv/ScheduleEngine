using FD.Simple.Utils.Agent;

namespace M.WFEngine.Model
{
    /// <summary>
    /// 处理代办任务，传回服务端的对象
    /// </summary>
    public class WFCmdRunModel:IFooParameter
    {
        /// <summary>
        /// 流程实例ID，取自wfoins表，对应id
        /// </summary>
        public string OinsId { get; set; }
        /// <summary>
        /// 流程命令id，取自wfcmd表，对应id
        /// </summary>
        public string CmdId { get; set; }
        /// <summary>
        /// 操作备注
        /// </summary>
        public string CmdMemo { get; set; }

        /// <summary>
        /// 如果下一个处理节点为指定处理人，需要传入此参数
        /// </summary>
        public string NextOperUserId { get; set; }
    }
}
