using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.ExpressionMove
{
    /// <summary>
    /// Mapper 配置类，提供 CreateMap GetMapper 等方法
    /// </summary>
    public class MoverTypeConfigExpression : IMoverTypeConfigExpression
    {
        internal List<IMoverTypeMapper> Mappers = new List<IMoverTypeMapper>();
        public IMoverTypeMapper<T, TTarget> CreateMap<T, TTarget>()
        {
            var mapper = new MoverTypeMapper<T, TTarget>();
            Mappers.Add(mapper);
            return mapper;
        }

        public IMoverTypeMapper<T, TTarget> GetMapper<T, TTarget>()
        {
            var m = Mappers.Find(x => x.SourceType == typeof(T) && x.TargetType == typeof(TTarget));
            return m as IMoverTypeMapper<T, TTarget>;
        }

        public List<IMoverTypeMapper> GetMappers<T>()
        {
            var l = Mappers.FindAll(x => x.SourceType == typeof(T));
            return l;
        }
    }

    public interface IMoverTypeConfigExpression
    {
        IMoverTypeMapper<T, TTarget> CreateMap<T, TTarget>();

        IMoverTypeMapper<T, TTarget> GetMapper<T, TTarget>();

        List<IMoverTypeMapper> GetMappers<T>();
    }
}
