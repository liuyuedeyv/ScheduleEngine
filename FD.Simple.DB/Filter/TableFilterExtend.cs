using System.Collections.Generic;

namespace FD.Simple.DB
{
    static class TableFilterExtend
    {
        public static string Build(this TableFilter tableFilter, DBParamCollection parameters)
        {
            TableFilterBuilder b = new TableFilterBuilder();
            return b.ParseTableFilter(tableFilter, parameters);
        }

        public static bool IsNullOrEmpty(this object o)
        {
            if (null == o || System.DBNull.Value == o)
            {
                return true;
            }
            return string.IsNullOrWhiteSpace(o.ToString());
        }
        public static string ConvertTostring(this object o, string defaultValue = null)
        {
            if (defaultValue == null)
            {
                defaultValue = string.Empty;
            }
            if (o == null || o == System.DBNull.Value)
            {
                return defaultValue;
            }
            try
            {
                return o.ToString();
            }
            catch (System.Exception)
            {
                return defaultValue;
            }
        }
    }

    public class TableFilterBuilder
    {
        #region Analyze TabelFilter  解析Tablefilte

        public string ParseTableFilter(TableFilter tableFilter, DBParamCollection parameters)
        {
            if (null == tableFilter)
            {
                return "1=1";
            }
            string result = string.Empty;
            if (tableFilter.IsContainer)
            {
                #region 遍历加载子条件

                foreach (var child in tableFilter.ChildFilters)
                {
                    child.MainTable = tableFilter.MainTable;
                    var filterString = ParseTableFilter(child, parameters);
                    if (string.IsNullOrWhiteSpace(filterString))
                        continue;
                    if (!string.IsNullOrEmpty(result))
                        result += tableFilter.And ? " AND " : " OR ";
                    result += filterString;
                }
                if (!string.IsNullOrEmpty(result))
                {
                    result = string.Format("{0}({1})", tableFilter.Not ? "NOT" : string.Empty, result);
                }
                else
                {
                    result = "1=1";
                }

                #endregion 遍历加载子条件
            }
            else
            {
                if (tableFilter.FilterValue.IsNullOrEmpty() && tableFilter.OperateMode != EOperateMode.IsNull & tableFilter.OperateMode != EOperateMode.IsNotNull)
                {
                    return result;
                }
                if (tableFilter.OperateMode == EOperateMode.In)
                {
                    string parmsString = string.Empty;
                    foreach (var value in tableFilter.FilterValue.ConvertTostring().Split(','))
                    {
                        var param = new DBParam(tableFilter.FilterName, value);
                        parameters.Add(param);
                        parmsString += param.ParamName + ",";
                    }
                    result = string.Format("{0}{1} IN ({2})", tableFilter.FilterName, (tableFilter.Not ? " NOT" : string.Empty), parmsString.TrimEnd(','));
                }
                else
                {
                    var paramValue = string.Empty;
                    if (tableFilter.OperateMode == EOperateMode.LikeLeft)
                    {
                        paramValue = $"{tableFilter.FilterValue}%";
                    }
                    else if (tableFilter.OperateMode == EOperateMode.LikeRight)
                    {
                        paramValue = $"%{tableFilter.FilterValue}";
                    }
                    else if (tableFilter.OperateMode == EOperateMode.LikeRight)
                    {
                        paramValue = $"%{tableFilter.FilterValue}%";
                    }
                    var param = new DBParam(tableFilter.FilterName, paramValue.IsNullOrEmpty() ? tableFilter.FilterValue : paramValue);
                    if (tableFilter.OperateMode != EOperateMode.IsNull && tableFilter.OperateMode != EOperateMode.IsNotNull)
                    {
                        parameters.Add(param);
                    }
                    result = tableFilter.FilterName + GetOperateFilter(tableFilter.OperateMode, param.ParamName, parameters.GetStringPlus);
                    if (tableFilter.Not)
                    {
                        result = "NOT(" + result + ")";
                    }
                }
            }
            return result;
        }

        static Dictionary<EOperateMode, string> dictOperateMode;
        private static object async = new object();

        protected static Dictionary<EOperateMode, string> DictOperateMode
        {
            get
            {
                //TODO:暂时用count!=12 来解决  dicOperateMode 偶尔缺少记录问题，以后细查
                if (dictOperateMode == null || dictOperateMode.Count != 12)
                {
                    lock (async)
                    {
                        if (dictOperateMode == null || dictOperateMode.Count != 12)
                        {
                            dictOperateMode = new Dictionary<EOperateMode, string>
                            {
                                [EOperateMode.Equals] = "=",
                                [EOperateMode.Great] = ">",
                                [EOperateMode.GreateEquals] = ">=",
                                [EOperateMode.In] = "IN",
                                [EOperateMode.Less] = "<",
                                [EOperateMode.LessEquals] = "<=",
                                [EOperateMode.LikeAll] = "LIKE",
                                [EOperateMode.LikeLeft] = "LIKE",
                                [EOperateMode.LikeRight] = "LIKE",
                                [EOperateMode.NotEquals] = "<>",
                                [EOperateMode.IsNull] = "IS NULL",
                                [EOperateMode.IsNotNull] = "IS NOT NULL"
                            };
                        }
                    }
                }
                return dictOperateMode;
            }
        }

        private string GetOperateFilter(EOperateMode operateMode, string parmsName, string stringPlus)
        {
            var result = string.Empty;
            var operatorStr = DictOperateMode[operateMode];
            if (operateMode == EOperateMode.IsNull || operateMode == EOperateMode.IsNotNull)
                result = operatorStr;
            else if (operateMode == EOperateMode.In)
                result = operatorStr + "(" + parmsName + ")";
            else
                result = operatorStr + " " + parmsName;
            return " " + result;
        }

        /// <summary>
        ///  生成@xxxx参数
        /// </summary>
        /// <param name="tableFilter"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string ParseTableDBParam(TableFilter tableFilter, DBParamCollection parameters)
        {
            string result = string.Empty;
            foreach (var child in tableFilter.ChildFilters)
            {
                foreach (var value in child.FilterValue.ConvertTostring().Split(','))
                {
                    var param = new DBParam(child.FilterName, value);
                    parameters.Add(param);
                    result += param.ParamName + ",";
                }
            }
            return result.TrimEnd(',');
        }

        /// <summary>
        /// insert 生成values()参数,与列名
        /// </summary>
        /// <param name="tableFilter"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string ParseTableColumns(TableFilter tableFilter, DBParamCollection parameters, out string columns)
        {
            string result = string.Empty;
            columns = string.Empty;
            foreach (var child in tableFilter.ChildFilters)
            {
                if (child.FilterValue.IsNullOrEmpty())
                {
                    continue;
                }
                foreach (var value in child.FilterValue.ConvertTostring().Split(','))
                {
                    var param = new DBParam(child.FilterName, value);
                    columns += child.FilterName + ",";
                    parameters.Add(param);
                    result += param.ParamName + ",";
                }
            }
            columns = columns.TrimEnd(',');
            return result.TrimEnd(',');
        }

        #endregion Analyze TabelFilter  解析Tablefilte
    }
}
