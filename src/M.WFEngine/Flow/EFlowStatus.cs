namespace M.WFEngine.Flow
{
    /// <summary>
    /// 流程状态
    /// </summary>
    public enum EFlowStatus
    {
        /// <summary>
        /// 草稿状态
        /// </summary>
        NotStarted = 0,
        /// <summary>
        /// 运行中
        /// </summary>
        Starting = 1,
        /// <summary>
        /// 结束
        /// </summary>
        Completed = 2
    }
}
