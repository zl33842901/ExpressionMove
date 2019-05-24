using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace xLiAd.ExpressionMove
{
    /// <summary>
    /// 提供替换功能的对外类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public class ExpressionMover<T, TReturn>
    {
        private readonly Expression<Func<T, TReturn>> expression;
        public ExpressionMover(Expression<Func<T, TReturn>> expression)
        {
            this.expression = expression;
        }

        private ConcurrentDictionary<Type, IMoverTypeMapper> moverTypeMappers = new ConcurrentDictionary<Type, IMoverTypeMapper>();

        /// <summary>
        /// 替换表达式参数为目标类
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <returns></returns>
        public Expression<Func<TTarget, TReturn>> MoveTo<TTarget>()
        {
            var mapper = GetMapper<TTarget>();
            var epr = new ExpressionParamReplacer<T, TTarget, TReturn>(expression, mapper);
            return epr.GetResult();
        }

        private MoverTypeMapper<T, TTarget> GetMapper<TTarget>(bool createWhenNoExists = false)
        {
            MoverTypeMapper<T, TTarget> mapper = null;
            if (moverTypeMappers.ContainsKey(typeof(TTarget)))
            {
                mapper = moverTypeMappers[typeof(TTarget)] as MoverTypeMapper<T, TTarget>;
                return mapper;
            }
            else
            {
                if (createWhenNoExists)
                {
                    mapper = new MoverTypeMapper<T, TTarget>();
                    moverTypeMappers[typeof(TTarget)] = mapper;
                    return mapper;
                }
                else
                    return null;
            }
        }

        public MoverTypeMapper<T, TTarget> TypeMapper<TTarget>()
        {
            return GetMapper<TTarget>(true);
        }
    }
}
