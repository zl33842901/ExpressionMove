using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;
using System.Linq;

namespace xLiAd.ExpressionMove.Test
{
    public class UnitTest3
    {
        private List<Model4> L4 = new List<Model4>()
        {
            new Model4(){ id = 3, intvalues = ",1,2,3," , values = ",a,b,c," },
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
    }
}
