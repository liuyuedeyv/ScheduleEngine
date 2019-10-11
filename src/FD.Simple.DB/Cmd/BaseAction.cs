namespace FD.Simple.DB.Cmd
{
    public abstract class BaseAction<T>
    {
        public BaseAction(IDBContext dbContext, string tableCode)
        {
            this.DbContext = dbContext;
            this.ParamCollection = dbContext.GetNewParamCollection();

            this.TableCode = tableCode;
        }
        /// <summary>
        /// 参数集合
        /// </summary>
        internal DBParamCollection ParamCollection { get; set; }
        /// <summary>
        /// 数据库执行对象
        /// </summary>
        internal IDBContext DbContext { get; set; }
        /// <summary>
        /// 执行表
        /// </summary>
        internal string TableCode { get; set; }
        /// <summary>
        /// 生成的主查询sql语句
        /// </summary>
        internal string MainSql { get; set; }
        /// <summary>
        /// 生成的查询数量的sql语句
        /// </summary>
        internal string SecondarySql { get; set; }
        /// <summary>
        /// 全部执行
        /// </summary>
        /// <returns></returns>
        public virtual T WhereAll()
        {
            return Where(null);
        }
        /// <summary>
        /// 执行部分数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public abstract T Where(TableFilter filter);
        /// <summary>
        /// 执行命令
        /// </summary>
        public abstract int ExecuteNonQuery();
        public override string ToString()
        {
            return MainSql.ToLower();
        }
    }
}
