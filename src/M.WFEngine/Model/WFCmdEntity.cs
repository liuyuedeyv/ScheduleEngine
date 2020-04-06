namespace M.WFEngine.Model
{
    public class WFCmdEntity
    {

        public string Id { get; set; }

        public string FlowId { get; set; }

        public string TaskId { get; set; }

        public EWFCmdType CmdType { get; set; }

        public int OrderId { get; set; }

        public string CmdDis { get; set; }
    }

    public enum EWFCmdType
    {
        /// <summary>
        /// 提交，正常流转
        /// </summary>
        Submit = 2,
        /// <summary>
        /// 流程拒绝，驳回
        /// </summary>
        Reject = 3,
        /// <summary>
        /// 退回
        /// </summary>
        Return = 4
    }
}
