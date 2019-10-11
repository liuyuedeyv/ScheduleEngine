using System.Collections.Generic;

namespace FD.Simple.DB
{
    public class DBCollection<T>
        where T : DBEntity, new()
    {
        public IEnumerable<T> Data;
        public int RecordsTotal;
        public uint PageSize;
        public uint PageIndex;
    }
}