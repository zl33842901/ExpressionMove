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
            var names = ExpressionExtension.GetMayBeName(memberInfo.Name);
            foreach(var name in names)
            {
                var m = type.GetMember(name, BindingFlags.Public | BindingFlags.Instance);
                if (m.Length < 1)
                    continue;
                else if (m.Length > 1)
                    throw new Exception($"找到了多个字段！{{{name}}}");
                else
                    return m[0];
            }
            throw new Exception($"未找到对应的字段！{{{memberInfo.Name}}}");
        }
    }
}
