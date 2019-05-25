using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace xLiAd.ExpressionMove
{
    /// <summary>
    /// 类型对应的提供者，由配置构建而来。
    /// </summary>
    public class MoverTypeProvider : IMoverTypeProvider
    {
        private readonly IMoverTypeConfigExpression configExpression;
        public MoverTypeProvider(IMoverTypeConfigExpression configExpression)
        {
            this.configExpression = configExpression;
        }

        public ExpressionMover<T, Tk> Load<T, Tk>(Expression<Func<T, Tk>> expression)
        {
            var mv = new ExpressionMover<T,Tk>(expression);
            var mappers = configExpression.GetMappers<T>();
            mappers.ForEach(x => mv.SetMapper(x));
            return mv;
        }
    }

    public interface IMoverTypeProvider
    {
        ExpressionMover<T, Tk> Load<T, Tk>(Expression<Func<T, Tk>> expression);
    }
}
