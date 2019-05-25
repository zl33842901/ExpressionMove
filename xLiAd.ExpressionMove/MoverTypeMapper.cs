using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace xLiAd.ExpressionMove
{
    /// <summary>
    /// 模型类 对应关系
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    public class MoverTypeMapper<T,TTarget> : IMoverTypeMapper<T, TTarget>
    {
        private List<MoverTypeMapperItem> moverTypeMapperItems = new List<MoverTypeMapperItem>();

        public IMoverTypeMapper<T, TTarget> FieldMap<TKeyFrom, TKeyTo>(Expression<Func<T,TKeyFrom>> expressionFrom, Expression<Func<TTarget, TKeyTo>> expressionTo)
        {
            moverTypeMapperItems.Add(new MoverTypeMapperItem(expressionFrom, expressionTo));
            return this;
        }

        public MemberInfo GetMember(string fromName)
        {
            var i = moverTypeMapperItems.Find(x => x.FromFieldName == fromName);
            return i?.ToMember;
        }

        public Type SourceType => typeof(T);

        public Type TargetType => typeof(TTarget);
    }

    public interface IMoverTypeMapper<T, TTarget> : IMoverTypeMapper
    {
        IMoverTypeMapper<T, TTarget> FieldMap<TKeyFrom, TKeyTo>(Expression<Func<T, TKeyFrom>> expressionFrom, Expression<Func<TTarget, TKeyTo>> expressionTo);
    }

    public interface IMoverTypeMapper
    {
        MemberInfo GetMember(string fromName);
        Type SourceType { get; }
        Type TargetType { get; }
    }
}
