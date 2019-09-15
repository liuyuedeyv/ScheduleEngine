
namespace FD.Simple.DB
{
    public class DBContextModel
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// 连接超时时间
        /// </summary>
        public int ConnectionTimeout { get; set; }
        /// <summary>
        /// cmd执行超时时间
        /// </summary>
        public int CommandTimeout { get; set; }
    }
}
