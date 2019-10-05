using FD.Simple.DB;
using FD.Simple.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace M.WFEngine.Util
{
    public static class DynamicLambda
    {
        /// <summary>
        /// 根据tablfiter 查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="tableFilter"></param>
        /// <returns></returns>
        public static IEnumerable<T> QueryDynamic<T>(this IEnumerable<T> data, TableFilter tableFilter)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "n");
            var filter = CreateExpressByFilter(tableFilter, param);
            if (filter == null)
                filter = Expression.Constant(true);
            var query = Expression.Lambda<Func<T, bool>>(filter, param);
            return data.AsQueryable().Where(query);
        }

        private static Expression CreateExpressByFilter(TableFilter tableFilter, ParameterExpression param)
        {
            Expression totalExpr = null;
            if (!tableFilter.IsContainer)
            {
                if (tableFilter.FilterValue.ConvertTostring() == string.Empty && tableFilter.OperateMode != EOperateMode.IsNull)
                {
                    return totalExpr;
                }
                Expression left = null;
                Expression right = null;
                var tmpValue = tableFilter.FilterValue;
                switch (tableFilter.OperateMode)
                {
                    case EOperateMode.LikeAll:
                        left = Expression.Call(param, "GetStringProperty", null, new Expression[] { Expression.Constant(tableFilter.FilterName) });
                        right = Expression.Constant(tmpValue);
                        totalExpr = Expression.Call(left, "Contains", null, new Expression[] { right });
                        break;

                    case EOperateMode.LikeLeft:
                        left = Expression.Call(param, "GetStringProperty", null, new Expression[] { Expression.Constant(tableFilter.FilterName) });
                        right = Expression.Constant(tmpValue);
                        totalExpr = Expression.Call(left, "StartsWith", null, new Expression[] { right });
                        break;

                    case EOperateMode.LikeRight:
                        left = Expression.Call(param, "GetStringProperty", null, new Expression[] { Expression.Constant(tableFilter.FilterName) });
                        right = Expression.Constant(tmpValue);
                        totalExpr = Expression.Call(left, "EndsWith", null, new Expression[] { right });
                        break;

                    case EOperateMode.Equals:
                        left = Expression.Call(param, "GetStringProperty", null, new Expression[] { Expression.Constant(tableFilter.FilterName) });
                        right = Expression.Constant(tmpValue.ConvertTostring());
                        totalExpr = Expression.Equal(left, right);
                        break;

                    case EOperateMode.NotEquals:
                        left = Expression.Call(param, "GetStringProperty", null, new Expression[] { Expression.Constant(tableFilter.FilterName) });
                        right = Expression.Constant(tmpValue);
                        totalExpr = Expression.NotEqual(left, right);
                        break;

                    case EOperateMode.Great:
                        left = Expression.Call(param, "GetDoubleProperty", null, new Expression[] { Expression.Constant(tableFilter.FilterName) });
                        right = Expression.Constant(tmpValue.ConvertTodouble());
                        totalExpr = Expression.GreaterThan(left, right);
                        break;

                    case EOperateMode.GreateEquals:
                        left = Expression.Call(param, "GetDoubleProperty", null, new Expression[] { Expression.Constant(tableFilter.FilterName) });
                        right = Expression.Constant(tmpValue.ConvertTodouble());
                        totalExpr = Expression.GreaterThanOrEqual(left, right);
                        break;

                    case EOperateMode.Less:
                        left = Expression.Call(param, "GetDoubleProperty", null, new Expression[] { Expression.Constant(tableFilter.FilterName) });
                        right = Expression.Constant(tmpValue.ConvertTodouble());
                        totalExpr = Expression.LessThan(left, right);
                        break;

                    case EOperateMode.LessEquals:
                        left = Expression.Call(param, "GetDoubleProperty", null, new Expression[] { Expression.Constant(tableFilter.FilterName) });
                        right = Expression.Constant(tmpValue.ConvertTodouble());
                        totalExpr = Expression.LessThanOrEqual(left, right);
                        break;

                    case EOperateMode.In:
                        left = Expression.Call(param, "GetStringProperty", null, new Expression[] { Expression.Constant(tableFilter.FilterName) });
                        right = Expression.Constant(tmpValue.ConvertTostring());
                        totalExpr = Expression.Call(right, "Contains", null, new Expression[] { left });
                        break;

                    case EOperateMode.IsNull:
                        left = Expression.Call(param, "GetStringProperty", null, new Expression[] { Expression.Constant(tableFilter.FilterName) });
                        totalExpr = Expression.Call(typeof(string), "IsNullOrWhiteSpace", null, new Expression[] { left });
                        break;

                    default:
                        break;
                }
            }
            else
            {
                foreach (var childFilter in tableFilter.ChildFilters)
                {
                    var childExpression = CreateExpressByFilter(childFilter, param);
                    if (childExpression != null)
                    {
                        if (tableFilter.And)
                        {
                            if (totalExpr == null)
                            {
                                totalExpr = childExpression;
                            }
                            else
                            {
                                totalExpr = Expression.And(childExpression, totalExpr);
                            }
                        }
                        else
                        {
                            if (totalExpr == null)
                            {
                                totalExpr = childExpression;
                            }
                            else
                            {
                                totalExpr = Expression.Or(childExpression, totalExpr);
                            }
                        }
                    }
                }
            }
            if (tableFilter.Not)
            {
                totalExpr = Expression.Not(totalExpr);
            }
            return totalExpr;
        }

        private static Expression GetConvertExpression(this Expression instanceExpr, Type targetType)
        {
            var mediateType = instanceExpr.Type;
            if (mediateType == typeof(object))
            {
                return Expression.Convert(instanceExpr, targetType);
            }
            else
            {
                var mediateExpr = Expression.Convert(instanceExpr, typeof(object));
                return Expression.Convert(mediateExpr, targetType);
            }
            throw new Exception("表达式错误");
        }
    }
}
