using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace xLiAd.ExpressionMove
{
    public class MoverTypeMapper<T,TTarget> : IMoverTypeMapper
    {
        private List<MoverTypeMapperItem> moverTypeMapperItems = new List<MoverTypeMapperItem>();

        public void Add<TKeyFrom, TKeyTo>(Expression<Func<T,TKeyFrom>> expressionFrom, Expression<Func<TTarget, TKeyTo>> expressionTo)
        {
            moverTypeMapperItems.Add(new MoverTypeMapperItem(expressionFrom, expressionTo));
        }

        public MemberInfo GetMember(string fromName)
        {
            var i = moverTypeMapperItems.Find(x => x.FromFieldName == fromName);
            return i?.ToMember;
        }
    }

    public interface IMoverTypeMapper
    {
        MemberInfo GetMember(string fromName);
    }
}
