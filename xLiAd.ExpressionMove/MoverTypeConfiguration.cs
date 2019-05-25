using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.ExpressionMove
{
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
