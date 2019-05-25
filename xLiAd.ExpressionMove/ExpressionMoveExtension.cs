using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace xLiAd.ExpressionMove
{
    /// <summary>
    /// 扩展方法静态类
    /// </summary>
    public static class ExpressionMoveExtension
    {
        /// <summary>
        /// 给表条式建立Mover
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Tk"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static ExpressionMover<T,Tk> BuildMover<T,Tk>(this Expression<Func<T, Tk>> expression)
        {
            var mover = new ExpressionMover<T, Tk>(expression);
            return mover;
        }
        /// <summary>
        /// 从一个字段表达式里找到MemberInfo 反射
        /// </summary>
        /// <param name="lambdaExpression"></param>
        /// <returns></returns>
        public static MemberInfo GetMember(this LambdaExpression lambdaExpression)
        {
            var fd = new ExpressionMemberInfoFinder(lambdaExpression);
            return fd.Member;
        }
    }
}
