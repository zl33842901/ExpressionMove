﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace xLiAd.ExpressionMove
{
    public static class ExpressionExtension
    {
        private static readonly ConcurrentDictionary<string, Func<object, object[], object>> Cache = new ConcurrentDictionary<string, Func<object, object[], object>>();

        #region 表达式类型字典
        /// <summary>
        /// 表达式类型字典
        /// </summary>
        private static readonly Dictionary<ExpressionType, string> NodeTypeDic = new Dictionary<ExpressionType, string>
        {
            {ExpressionType.AndAlso," AND "},
            {ExpressionType.OrElse," OR "},
            {ExpressionType.Equal," = "},
            {ExpressionType.NotEqual," != "},
            {ExpressionType.LessThan," < "},
            {ExpressionType.LessThanOrEqual," <= "},
            {ExpressionType.GreaterThan," > "},
            {ExpressionType.GreaterThanOrEqual," >= "}
        };
        #endregion

        #region 获取表达式类型转换结果
        /// <summary>
        /// 获取表达式类型转换结果
        /// </summary>
        /// <param name="node">二元表达式</param>
        /// <returns></returns>
        public static string GetExpressionType(this BinaryExpression node)
        {
            var nodeTypeDic = NodeTypeDic[node.NodeType];

            string nodeType = null;
            if (node.Right.NodeType == ExpressionType.Constant && ((ConstantExpression)node.Right).Value == null)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.Equal:
                        nodeType = " IS ";
                        break;
                    case ExpressionType.NotEqual:
                        nodeType = " IS NOT ";
                        break;
                }
            }

            return !string.IsNullOrEmpty(nodeType) ? nodeType : nodeTypeDic;
        }
        #endregion

        #region 获取最底层成员表达式
        /// <summary>
        /// 获取最底层成员表达式
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static MemberExpression GetRootMember(this MemberExpression e)
        {
            if (e.Expression == null || e.Expression.NodeType == ExpressionType.Constant)
                return e;

            return e.Expression.NodeType == ExpressionType.MemberAccess
                ? ((MemberExpression)e.Expression).GetRootMember()
                : null;
        }
        #endregion

        #region 转换成一元表达式并取值
        /// <summary>
        /// 转换成一元表达式并取值
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static object ToConvertAndGetValue(this Expression expression)
        {
            if (expression.Type != typeof(object))
                expression = Expression.Convert(expression, typeof(object));

            var lambdaExpression = Expression.Lambda<Func<object>>(expression);
            return lambdaExpression.Compile().Invoke();
        }
        #endregion

        public static object MemberToValue(this MemberExpression memberExpression)
        {
            var topMember = GetRootMember(memberExpression);
            if (topMember == null)
                return null;
            //throw new InvalidOperationException("需计算的条件表达式只支持由 MemberExpression 和 ConstantExpression 组成的表达式");

            return memberExpression.MemberToValue(topMember);
        }

        public static object MemberToValue(this MemberExpression memberExpression, MemberExpression topMember)
        {
            if (topMember.Expression == null)
            {
                //var aquire = Cache.GetOrAdd(memberExpression.ToString(), key => GetStaticProperty(memberExpression));
                var aquire = GetStaticProperty(memberExpression);
                return aquire(null, null);
            }
            else
            {
                //var aquire = Cache.GetOrAdd(memberExpression.ToString(), key => GetInstanceProperty(memberExpression, topMember));

                var aquire = GetInstanceProperty(memberExpression, topMember);
                return aquire((topMember.Expression as ConstantExpression).Value, null);
            }
        }

        private static Func<object, object[], object> GetInstanceProperty(Expression e, MemberExpression topMember)
        {
            var parameter = Expression.Parameter(typeof(object), "local");
            var parameters = Expression.Parameter(typeof(object[]), "args");
            var castExpression = Expression.Convert(parameter, topMember.Member.DeclaringType);
            var localExpression = topMember.Update(castExpression);
            var replaceExpression = ExpressionModifier.Replace(e, topMember, localExpression);
            replaceExpression = Expression.Convert(replaceExpression, typeof(object));
            var compileExpression = Expression.Lambda<Func<object, object[], object>>(replaceExpression, parameter, parameters);
            return compileExpression.Compile();
        }

        private static Func<object, object[], object> GetStaticProperty(Expression e)
        {
            var parameter = Expression.Parameter(typeof(object), "local");
            var parameters = Expression.Parameter(typeof(object[]), "args");
            var convertExpression = Expression.Convert(e, typeof(object));
            var compileExpression = Expression.Lambda<Func<object, object[], object>>(convertExpression, parameter, parameters);
            return compileExpression.Compile();
        }
        /// <summary>
        /// 根据字段名找到对应可能的字段名
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string[] GetMayBeName(string s)
        {
            List<string> result = new List<string>();
            result.Add(s);
            if (s.Length < 2)
                return result.ToArray();
            bool camel = s.Contains("_");
            bool anyUpper = Regex.IsMatch(s.Substring(1), "[A-Z]+");
            if (camel)
            {
                var ss = s.Substring(1);
                foreach (var n in names)
                    ss = ss.Replace(n.Value, n.Key);
                ss = s.Substring(0, 1).ToUpper() + ss;
                if (ss != s)
                    result.Add(ss);
            }
            else if (anyUpper)
            {
                var ss = s.Substring(1);
                foreach (var n in names)
                    ss = ss.Replace(n.Key, n.Value);
                ss = s.Substring(0, 1).ToLower() + ss;
                if (ss != s)
                    result.Add(ss);
            }
            return result.ToArray();
        }
        private static Dictionary<string, string> names = new Dictionary<string, string>()
        {
            { "A", "_a" },
            { "B", "_b" },
            { "C", "_c" },
            { "D", "_d" },
            { "E", "_e" },
            { "F", "_f" },
            { "G", "_g" },
            { "H", "_h" },
            { "I", "_i" },
            { "J", "_j" },
            { "K", "_k" },
            { "L", "_l" },
            { "M", "_m" },
            { "N", "_n" },
            { "O", "_o" },
            { "P", "_p" },
            { "Q", "_q" },
            { "R", "_r" },
            { "S", "_s" },
            { "T", "_t" },
            { "U", "_u" },
            { "V", "_v" },
            { "W", "_w" },
            { "X", "_x" },
            { "Y", "_y" },
            { "Z", "_z" }
        };
    }
}
