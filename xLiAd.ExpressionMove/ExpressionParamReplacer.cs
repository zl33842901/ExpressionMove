using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace xLiAd.ExpressionMove
{
    /// <summary>
    /// 表达式参数访问者
    /// </summary>
    /// <typeparam name="TFrom"></typeparam>
    /// <typeparam name="TTo"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    internal class ExpressionParamReplacer<TFrom, TTo, TKey> : ExpressionVisitor
    {
        private readonly Expression<Func<TFrom, TKey>> expression;
        private readonly MoverTypeMapper<TFrom, TTo> moverTypeMapper;
        private Expression<Func<TTo, TKey>> result;
        private ParameterExpression par;
        internal ExpressionParamReplacer(Expression<Func<TFrom, TKey>> expression, MoverTypeMapper<TFrom, TTo> moverTypeMapper)
        {
            this.expression = expression;
            this.moverTypeMapper = moverTypeMapper;
        }

        internal Expression<Func<TTo, TKey>> GetResult()
        {
            if(result == null)
            {
                par = Expression.Parameter(typeof(TTo), expression.Parameters[0].Name);
                var exp = Visit(expression.Body);
                result = Expression.Lambda(exp, par) as Expression<Func<TTo, TKey>>;
            }
            return result;
        }
        /// <summary>
        /// 只替换访问
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression != null && node.Expression.Type == typeof(TFrom))
            {
                MemberInfo toMember;
                if(moverTypeMapper != null)
                {
                    toMember = moverTypeMapper.GetMember(node.Member.Name);
                }
                else
                {
                    MemberInfoReplacer memberInfoReplacer = new MemberInfoReplacer(node.Member);
                    toMember = memberInfoReplacer.GetMember(typeof(TTo));
                }
                return Expression.MakeMemberAccess(par, toMember);
            }
            else
                return base.VisitMember(node);
        }


    }
}
