namespace FD.Simple.DB
{
    public enum EDBEntityState
    {
        /// <summary>
        /// 未修改
        /// </summary>
        UnChanged = 0,

        /// <summary>
        /// 新增
        /// </summary>
        Added = 2,

        /// <summary>
        /// 修改
        /// </summary>
        Modified = 4,

        /// <summary>
        /// 删除
        /// </summary>
        Deleted = 8
    }
}