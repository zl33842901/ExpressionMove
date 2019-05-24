using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace xLiAd.ExpressionMove
{
    /// <summary>
    /// MemberInfo对应查找类
    /// </summary>
    internal class MemberInfoReplacer
    {
        private readonly MemberInfo memberInfo;
        internal MemberInfoReplacer(MemberInfo memberInfo)
        {
            this.memberInfo = memberInfo;
        }

        internal MemberInfo GetMember(Type type)
        {
            var m = type.GetMember(memberInfo.Name, BindingFlags.Public | BindingFlags.Instance);
            if (m.Length < 1)
                throw new Exception($"未找到对应的字段！{{{memberInfo.Name}}}");
            else if (m.Length > 1)
                throw new Exception($"找到了多个字段！{{{memberInfo.Name}}}");
            else
                return m[0];
        }
    }
}
