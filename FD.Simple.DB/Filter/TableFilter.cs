﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FD.Simple.DB
{
    [DataContract]
    public class TableFilter
    {
        public string MainTable { get; set; }

        #region Constructor

        public static TableFilter New()
        {
            return new TableFilter();
        }

        /// <summary>
        /// 作为容器的构造函数
        /// </summary>
        /// <param name="and"></param>
        /// <param name="not"></param>
        public TableFilter(string filterDisplay = "", bool and = true, bool not = false)
        {
            this.FilterDisplay = filterDisplay;
            this.and = and;
            this.Not = not;
            this.childFilters = new List<TableFilter>();
        }

        public TableFilter Join(string leftCol, string rightCol)
        {

            return this;
        }
        public TableFilter SetAnd()
        {
            this.and = true;
            return this;
        }
        public TableFilter SetOr()
        {
            this.and = false;
            return this;
        }
        /// <summary>
        /// 作为条件的构造函数
        /// </summary>
        /// <param name="and"></param>
        /// <param name="not"></param>
        private TableFilter(EOperateMode operateMode, string filterName, object filterValue, bool not)
        {
            this.OperateMode = operateMode;
            this.FilterName = filterName;
            this.FilterValue = filterValue;
            this.Not = not;
        }

        #endregion Constructor

        #region Field

        /// <summary>
        /// 是否为容器
        /// </summary>
        public bool IsContainer
        {
            get
            {
                return ChildFilters != null;
            }
        }

        #endregion Field

        #region Property

        #region ChildFilters

        /// <summary>
        /// 存放子过滤条件
        /// </summary>
        [DataMember(Name = "ChildFilters")]
        private List<TableFilter> childFilters;

        public List<TableFilter> ChildFilters
        {
            get
            {
                return this.childFilters;
            }
        }

        #endregion ChildFilters

        #region LastAddedFilter

        /// <summary>
        /// 最后添加的过滤条件
        /// </summary>
        public TableFilter LastAddedFilter
        {
            get;
            set;
        }

        #endregion LastAddedFilter

        #region And

        [DataMember(Name = "And")]
        private bool and;

        /// <summary>
        /// 子条件 是And,否Or
        /// </summary>
        public bool And
        {
            get
            {
                return this.and;
            }
            set
            {
                if (this.IsContainer)
                    this.and = value;
                else
                    throw new Exception("非容器类型的条件无法设置And值");
            }
        }

        #endregion And

        #region Not

        /// <summary>
        /// 是否取反
        /// </summary>
        [DataMember]
        public bool Not
        { get; set; }

        #endregion Not

        #region OperateMode

        /// <summary>
        /// 操作符类型
        /// </summary>
        [DataMember]
        public EOperateMode OperateMode;

        #endregion OperateMode

        #region FilterName

        private string filterName;

        /// <summary>
        /// 过滤字段
        /// </summary>
        [DataMember]
        public string FilterName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(MainTable) && filterName.IndexOf('.') == -1)
                {
                    return MainTable + "." + filterName;
                }
                else
                {
                    return filterName;
                }
            }
            set { filterName = value; }
        }

        #endregion FilterName

        #region FilterValue

        /// <summary>
        /// 过滤字段的值
        /// </summary>
        [DataMember]
        public object FilterValue { get; set; }

        #endregion FilterValue

        #region FilterDisplay

        /// <summary>
        /// 过滤条件可视化名称
        /// </summary>
        [DataMember]
        public string FilterDisplay { get; set; }

        #endregion FilterDisplay

        #endregion Property

        #region AddFilter

        /// <summary>
        /// 添加过滤条件
        /// </summary>
        /// <param name="operateMode">过滤操作符</param>
        /// <param name="filterValue">过滤字段,如果值为空,抛出异常</param>
        /// <param name="filterValue">过滤字段的值,如果值为空,则不加入此过滤信息 例外：ISNULL操作符</param>
        /// <param name="not">是否取反</param>
        /// <returns>返回this</returns>
        public TableFilter AddFilter(EOperateMode operateMode, string filterName, object filterValue, bool not = false)
        {
            if (operateMode == EOperateMode.IsNull)
            {
                filterValue = "0";//占位符。因为解析filter的时候filtervalue为空会跳过这个条件
            }
            if (string.IsNullOrWhiteSpace(filterName))
            {
                throw new Exception("过滤条件字段名称能为空, 无法添加");
            }
            if (operateMode != EOperateMode.IsNull && (filterValue == null || string.IsNullOrWhiteSpace(filterValue.ToString())))
            {
                return this;
            }
            var xmlFilter = new TableFilter(operateMode, filterName, filterValue, not);
            this.LastAddedFilter = xmlFilter;
            this.ChildFilters.Add(xmlFilter);
            return this;
        }

        /// <summary>
        /// 为容器添加子容器
        /// </summary>
        /// <param name="childFilter">子容器</param>
        /// <returns>返回this</returns>
        public TableFilter AddFilter(TableFilter childFilter)
        {
            this.ChildFilters.Add(childFilter);
            this.LastAddedFilter = childFilter;
            return this;
        }

        /// <summary>
        /// 左匹配LIKE%
        /// </summary>
        /// <param name="filterValue">过滤字段,如果值为空,抛出异常</param>
        /// <param name="filterValue">过滤字段的值,如果值为空,则不加入此过滤信息</param>
        /// <param name="not">是否取反</param>
        /// <returns>返回this</returns>
        public TableFilter LikeAll(string filterName, object filterValue, bool not = false)
        {
            return this.AddFilter(EOperateMode.LikeAll, filterName, filterValue, not);
        }

        /// <summary>
        /// 左匹配LIKE%
        /// </summary>
        /// <param name="filterValue">过滤字段,如果值为空,抛出异常</param>
        /// <param name="filterValue">过滤字段的值,如果值为空,则不加入此过滤信息</param>
        /// <param name="not">是否取反</param>
        /// <returns>返回this</returns>
        public TableFilter LikeLeft(string filterName, object filterValue, bool not = false)
        {
            return this.AddFilter(EOperateMode.LikeLeft, filterName, filterValue, not);
        }

        /// <summary>
        /// 右匹配%LIKE
        /// </summary>
        /// <param name="filterValue">过滤字段,如果值为空,抛出异常</param>
        /// <param name="filterValue">过滤字段的值,如果值为空,则不加入此过滤信息</param>
        /// <param name="not">是否取反</param>
        /// <returns>返回this</returns>
        public TableFilter LikeRight(string filterName, object filterValue, bool not = false)
        {
            return this.AddFilter(EOperateMode.LikeRight, filterName, filterValue, not);
        }

        /// <summary>
        /// 等于=
        /// </summary>
        /// <param name="filterValue">过滤字段,如果值为空,抛出异常</param>
        /// <param name="filterValue">过滤字段的值,如果值为空,则不加入此过滤信息</param>
        /// <param name="not">是否取反</param>
        /// <returns>返回this</returns>
        public TableFilter Equals(string filterName, object filterValue, bool not = false)
        {
            return this.AddFilter(EOperateMode.Equals, filterName, filterValue, not);
        }

        /// <summary>
        /// isnull
        /// </summary>
        /// <param name="filterName"></param>
        /// <returns></returns>
        public TableFilter IsNull(string filterName)
        {
            return this.AddFilter(EOperateMode.IsNull, filterName, 0, false);
        }

        /// <summary>
        /// not null
        /// </summary>
        /// <param name="filterName"></param>
        /// <returns></returns>
        public TableFilter NotNull(string filterName)
        {
            return this.AddFilter(EOperateMode.IsNull, filterName, 0, true);
        }

        /// <summary>
        /// 不等于&lt;&gt;
        /// </summary>
        /// <param name="filterValue">过滤字段,如果值为空,抛出异常</param>
        /// <param name="filterValue">过滤字段的值,如果值为空,则不加入此过滤信息</param>
        /// <param name="not">是否取反</param>
        /// <returns>返回this</returns>
        public TableFilter NotEquals(string filterName, object filterValue, bool not = false)
        {
            return this.AddFilter(EOperateMode.NotEquals, filterName, filterValue, not);
        }

        /// <summary>
        /// 大于&gt;
        /// </summary>
        /// <param name="filterValue">过滤字段,如果值为空,抛出异常</param>
        /// <param name="filterValue">过滤字段的值,如果值为空,则不加入此过滤信息</param>
        /// <param name="not">是否取反</param>
        /// <returns>返回this</returns>
        public TableFilter Great(string filterName, object filterValue, bool not = false)
        {
            return this.AddFilter(EOperateMode.Great, filterName, filterValue, not);
        }

        /// <summary>
        /// 大于等于&gt;=
        /// </summary>
        /// <param name="filterValue">过滤字段,如果值为空,抛出异常</param>
        /// <param name="filterValue">过滤字段的值,如果值为空,则不加入此过滤信息</param>
        /// <param name="not">是否取反</param>
        /// <returns>返回this</returns>
        public TableFilter GreateEquals(string filterName, object filterValue, bool not = false)
        {
            return this.AddFilter(EOperateMode.GreateEquals, filterName, filterValue, not);
        }

        /// <summary>
        /// 小于&lt;
        /// </summary>
        /// <param name="filterValue">过滤字段,如果值为空,抛出异常</param>
        /// <param name="filterValue">过滤字段的值,如果值为空,则不加入此过滤信息</param>
        /// <param name="not">是否取反</param>
        /// <returns>返回this</returns>
        public TableFilter Less(string filterName, object filterValue, bool not = false)
        {
            return this.AddFilter(EOperateMode.Less, filterName, filterValue, not);
        }

        /// <summary>
        /// 小于等于&lt;=
        /// </summary>
        /// <param name="filterValue">过滤字段,如果值为空,抛出异常</param>
        /// <param name="filterValue">过滤字段的值,如果值为空,则不加入此过滤信息</param>
        /// <param name="not">是否取反</param>
        /// <returns>返回this</returns>
        public TableFilter LessEquals(string filterName, object filterValue, bool not = false)
        {
            return this.AddFilter(EOperateMode.LessEquals, filterName, filterValue, not);
        }

        /// <summary>
        /// 在...里面 IN,仅用于ID字段
        /// </summary>
        /// <param name="filterValue">过滤字段,如果值为空,抛出异常</param>
        /// <param name="filterValue">过滤字段的值,如果值为空,则不加入此过滤信息</param>
        /// <param name="not">是否取反</param>
        /// <returns>返回this</returns>
        public TableFilter In(string filterName, object filterValue, bool not = false)
        {
            return this.AddFilter(EOperateMode.In, filterName, filterValue, not);
        }

        /// <summary>
        /// 在...里面 IN,仅用于ID字段
        /// </summary>
        /// <param name="filterValue">过滤字段,如果值为空,抛出异常</param>
        /// <param name="not">是否取反</param>
        /// <returns>返回this</returns>
        public TableFilter In(string filterName, bool not = false)
        {
            return this.AddFilter(EOperateMode.IsNull, filterName, null, not);
        }

        #endregion AddFilter

        #region GetFilterNameList

        /// <summary>
        /// 获取过滤字段和值
        /// </summary>
        /// <param name="dict"></param>
        public void GetFilterNameList(ref Dictionary<string, object> dict)
        {
            string result = string.Empty;
            //判断是否为容器
            if (this.IsContainer)
            {
                //加载子过滤的字符串
                foreach (var child in this.ChildFilters)
                {
                    child.GetFilterNameList(ref dict);
                }
            }
            else
            {
                dict[this.FilterName] = this.FilterValue;
            }
        }

        #endregion GetFilterNameList
    }
}