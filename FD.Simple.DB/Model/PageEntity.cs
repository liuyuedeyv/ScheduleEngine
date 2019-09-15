namespace FD.Simple.DB.Model
{
    public class PageEntity : FD.Simple.Utils.Agent.IFooParameter
    {
        /// <summary>
        /// 页面索引，从1开始
        /// </summary>
        public uint PageIndex { get; set; } = 1;

        /// <summary>
        /// 页面大小
        /// </summary>
        public uint PageSize { get; set; } = 10;
    }
}
