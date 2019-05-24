using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace xLiAd.ExpressionMove
{
    /// <summary>
    /// 表达式中的 MemberInfo 查找器
    /// </summary>
    public class ExpressionMemberInfoFinder : ExpressionVisitor
    {
        private readonly LambdaExpression lambdaExpression;
        public ExpressionMemberInfoFinder(LambdaExpression lambdaExpression)
        {
            this.lambdaExpression = lambdaExpression;
        }

        private MemberInfo member;
        private bool computed = false;

        public MemberInfo Member
        {
            get
            {
                if(!computed)
                {
                    Visit(lambdaExpression.Body);
                    computed = true;
                }
                return member;
            }
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            member = node.Member;
            return base.VisitMember(node);
        }
    }
}
