using System.Collections.Generic;

namespace FD.Simple.DB
{
    /// <summary>
    /// 数据库参数
    /// </summary>
    public class DBParamCollection : IEnumerable<DBParam>
    {

        public string GetStringPlus
        {
            get
            {
                return this.StringPlus;
            }
        }
        /// <summary>
        /// 字符串拼接符号  pg 用"||"，其他为+
        /// </summary>
        private readonly string StringPlus = "+";
        private readonly string PreParam = "@";
        private readonly string SuffixParam = "";
        private Dictionary<string, DBParam> DictParms { get; set; }

        /// <summary>
        ///请通过 IDBContext实例 GetNewParamCollection 获取参数集合
        /// </summary>
        /// <param name="changeParamName">是否自动修改传入参数的名字</param>
        internal DBParamCollection(string preParam = "@", string suffix = "", string stringPlus = "+")
        {
            this.StringPlus = stringPlus;
            this.PreParam = preParam;
            this.SuffixParam = suffix;
            this.DictParms = new Dictionary<string, DBParam>();
        }

        public void Add(DBParam item)
        {
            int paramIndex = 0;
            while (paramIndex < 2100)
            {
                var paramName = this.PreParam + item.ParamName + this.SuffixParam;
                if (paramIndex > 0)
                    paramName += paramIndex.ToString();
                if (this.DictParms.ContainsKey(paramName))
                {
                    if (this.DictParms[paramName].ParamValue.Equals(item.ParamValue))
                    {
                        item.ParamName = paramName;
                        return;
                    }
                    paramIndex++;
                }
                else
                {
                    item.ParamName = paramName;
                    this.DictParms[paramName] = item;
                    return;
                }
            }
            if (paramIndex == 2100)
            {
                throw new System.Exception("参数超出上限");
            }
        }

        public void Add(DBParamCollection collection)
        {
            if (collection == null)
                return;
            foreach (var p in collection)
            {
                this.DictParms.Add(p.ParamName, p);
            }
        }

        public int Count
        {
            get
            {
                return this.DictParms.Count;
            }
        }

        public IEnumerator<DBParam> GetEnumerator()
        {
            return this.DictParms.Values.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.DictParms.Values.GetEnumerator();
        }
    }
}