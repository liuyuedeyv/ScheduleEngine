namespace M.WFEngine.Util
{
    public class MutexConfig
    {
        /// <summary>
        /// 调度引擎聚合节点mutex锁前缀
        /// </summary>
        public const string WORKFLOWLOCKPRE = "WF_JuHe_";
        /// <summary>
        /// 调度引擎聚合节点mutex锁超时时间，haomiao
        /// </summary>
        public const int WORKFLOWLOCKTIMEOUT = 20000;
    }
}
