
namespace FD.Simple.DB
{
    public class QueryModel
    {
        public string Fields { get; set; }
        public string Tables { get; set; }
        public string Where { get; set; }
        public string SortField { get; set; }
        public uint Start { get; set; }
        public uint End { get; set; }
    }
}
