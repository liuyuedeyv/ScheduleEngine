using System.Runtime.Serialization;

namespace FD.Simple.DB
{
    /// <summary>
    /// 操作符类型
    /// </summary>
    [DataContract]
    public enum EOperateMode
    {
        /// <summary>
        /// 全匹配%LIKE%
        /// </summary>
        LikeAll,

        /// <summary>
        /// 左匹配LIKE%
        /// </summary>
        LikeLeft,

        /// <summary>
        /// 右匹配%LIKE
        /// </summary>
        LikeRight,

        /// <summary>
        /// 等于=
        /// </summary>
        Equals,

        /// <summary>
        /// 不等于&lt;&gt;
        /// </summary>
        NotEquals,

        /// <summary>
        /// 大于&gt;
        /// </summary>
        Great,

        /// <summary>
        /// 大于等于&gt;=
        /// </summary>
        GreateEquals,

        /// <summary>
        /// 小于&lt;
        /// </summary>
        Less,

        /// <summary>
        /// 小于等于&lt;=
        /// </summary>
        LessEquals,

        /// <summary>
        /// 在...里面 IN
        /// </summary>
        In,

        /// <summary>
        /// 是否为空 IS NULL
        /// </summary>
        IsNull,

        /// <summary>
        /// 是否为空 IS NOT NULL
        /// </summary>
        IsNotNull
    }
}