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
        private readonly LambdaExpression expression;
        private readonly MoverTypeMapper<TFrom, TTo> moverTypeMapper;
        private Expression<Func<TTo, TKey>> result;
        private ParameterExpression par;
        internal ExpressionParamReplacer(Expression<Func<TFrom, TKey>> expression, MoverTypeMapper<TFrom, TTo> moverTypeMapper)
        {
            this.expression = TrimExpression.Trim(expression) as LambdaExpression;
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
                MemberInfo toMember = getMember(node.Member);
                return Expression.MakeMemberAccess(par, toMember);
            }
            else
                return base.VisitMember(node);
        }
        /// <summary>
        /// 根据原属性 获得新属性
        /// </summary>
        /// <param name="tmi"></param>
        /// <returns></returns>
        private MemberInfo getMember(MemberInfo tmi)
        {
            MemberInfo toMember = null;
            if (moverTypeMapper != null)
            {
                toMember = moverTypeMapper.GetMember(tmi.Name);
            }
            if (toMember == null)
            {
                MemberInfoReplacer memberInfoReplacer = new MemberInfoReplacer(tmi);
                toMember = memberInfoReplacer.GetMember(typeof(TTo));
            }
            if (toMember == null)
                throw new Exception($"未找到对应的字段{{{tmi.Name}}}");
            return toMember;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if(node.Arguments.Count == 2 && node.Arguments[0] is MemberExpression me && node.Arguments[1] is ConstantExpression constant)
            {
                if(me.Expression != null && me.Expression.Type == typeof(TFrom))
                {
                    MemberInfo nmi = getMember(me.Member);
                    if(((System.Reflection.PropertyInfo)nmi).PropertyType != ((System.Reflection.PropertyInfo)me.Member).PropertyType)//字段类型不一致。
                    {
                        if(node.Method.Name == "Contains" && ((System.Reflection.PropertyInfo)nmi).PropertyType == typeof(string))
                        {
                            //这块硬处理一下。
                            var c = Expression.Constant("," + constant.Value.ToString() + ",", typeof(string));
                            var mmi = typeof(string).GetMethod("Contains", BindingFlags.Instance | BindingFlags.Public, Type.DefaultBinder, new Type[] { typeof(string) }, new ParameterModifier[] { new ParameterModifier(1) });
                            var method = Expression.Call(Expression.MakeMemberAccess(par, nmi), mmi, c);
                            return method;
                        }
                    }
                }
            }
            return base.VisitMethodCall(node);
        }
    }
}
