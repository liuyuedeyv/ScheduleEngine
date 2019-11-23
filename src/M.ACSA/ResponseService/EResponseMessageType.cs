namespace M.ACSA.ResponseService
{
    public enum EResponseMessageType
    {
        /// <summary>
        /// 获取流程业务数据
        /// </summary>
        GetWorkflowServiceBisdata = 1,
        /// <summary>
        /// 获取决策节点业务数据
        /// </summary>
        GetModelTaskBisdata = 2,
        /// <summary>
        /// 从远端获取变量
        /// </summary>
        GetVariable = 3
    }
}
