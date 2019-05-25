using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.ExpressionMove
{
    /// <summary>
    /// 提供 配置委托 的类
    /// </summary>
    public class MoverTypeConfiguration
    {
        private readonly MoverTypeConfigExpression configExpression = new MoverTypeConfigExpression();
        public MoverTypeConfiguration(Action<IMoverTypeConfigExpression> action)
        {
            action(configExpression);
        }

        public IMoverTypeProvider Build()
        {
            return new MoverTypeProvider(this.configExpression);
        }
    }
}
