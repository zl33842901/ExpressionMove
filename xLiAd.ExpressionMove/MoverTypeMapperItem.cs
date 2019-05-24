using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace xLiAd.ExpressionMove
{
    /// <summary>
    /// 两个类的某个字段对应关系
    /// </summary>
    /// <typeparam name="TFrom"></typeparam>
    /// <typeparam name="TFromKey"></typeparam>
    /// <typeparam name="TTo"></typeparam>
    /// <typeparam name="TToKey"></typeparam>
    public class MoverTypeMapperItem
    {
        private readonly LambdaExpression expressionFrom;
        private readonly LambdaExpression expressionTo;
        public MoverTypeMapperItem(LambdaExpression expressionFrom, LambdaExpression expressionTo)
        {
            this.expressionFrom = expressionFrom;
            this.expressionTo = expressionTo;
        }

        private MemberInfo fromMember;
        public MemberInfo FromMember
        {
            get
            {
                if(fromMember == null)
                {
                    fromMember = expressionFrom.GetMember();
                }
                return fromMember;
            }
        }

        private MemberInfo toMember;
        public MemberInfo ToMember
        {
            get
            {
                if(toMember == null)
                {
                    toMember = expressionTo.GetMember();
                }
                return toMember;
            }
        }

        public string FromFieldName => FromMember?.Name;
        public string ToFieldName => ToMember?.Name;
    }
}
