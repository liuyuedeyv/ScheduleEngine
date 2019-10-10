namespace M.WorkFlow.Model
{
    public enum ETaskType
    {
        /// <summary>
        /// 开始节点
        /// </summary>
        Start = 1,
        /// <summary>
        /// 结束节点
        /// </summary>
        End = 2,
        /// <summary>
        /// 普通任务节点
        /// </summary>
        Work = 3,
        /// <summary>
        /// 异步任务节点
        /// </summary>
        WorkAsync = 4,
        /// <summary>
        /// 并行节点
        /// </summary>
        BingXing = 5,
        /// <summary>
        /// 聚合节点
        /// </summary>
        JuHe = 6,
        /// <summary>
        /// 决策节点
        /// </summary>
        Model = 7
    }
}
