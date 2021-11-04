using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;
using System.Linq;

namespace xLiAd.ExpressionMove.Test
{
    /// <summary>
    /// 当你的领域对象和你的数据对象不一致时
    /// </summary>
    public class UnitTest3
    {
        private List<Model4> L4 = new List<Model4>()
        {
            new Model4(){ id = 3, intvalues = ",1,2,3," , values = ",a,b,c," },//在数据库里要这样存。
            new Model4(){ id = 5, intvalues = ",5,6,7," , values = ",a,b,c,"}
        };
        private MoverTypeConfiguration Configuration { get; } = new MoverTypeConfiguration(x =>
        {
            x.CreateMap<Model1, Model4>()
            .FieldMap(x => x.Id, x => x.id)
            .FieldMap(x => x.IntValues, x => x.intvalues)
            .FieldMap(x => x.Values, x => x.values);
        });

        [Fact]
        public void Test1()
        {
            Expression<Func<Model1, bool>> expression = x => x.Id == 5 && x.IntValues.Contains(6) && x.Values.Contains("b");
            var provider = Configuration.Build();
            var result = provider.Load(expression).MoveTo<Model4>();
            Assert.NotNull(result);
            var l = L4.Where(result.Compile()).ToList();
            Assert.NotEmpty(l);
        }

        [Fact]
        public void Test2()
        {
            Expression<Func<Model1, object>> expression = x => x.IntValues;
            var provider = Configuration.Build();
            var result = provider.Load(expression).MoveTo<Model4>();
            Assert.NotNull(result);
        }

        [Fact]
        public void Test3()
        {
            Expression<Func<Model5, object>> expression = x => x.CreateTime;
            var provider = Configuration.Build();
            var result = provider.Load(expression).MoveTo<Model6>();
            Assert.NotNull(result);
        }

        [Fact]
        public void Test4()
        {
            Expression<Func<Model6, object>> expression = x => x.create_time;
            var provider = Configuration.Build();
            var result = provider.Load(expression).MoveTo<Model5>();
            Assert.NotNull(result);
        }

        [Fact]
        public void Test5()
        {
            var s = "CreateTime";
            var ss = ExpressionExtension.GetMayBeName(s);
        }

        [Fact]
        public void Test6()
        {
            var s = "create_time";
            var ss = ExpressionExtension.GetMayBeName(s);
        }
    }
}
