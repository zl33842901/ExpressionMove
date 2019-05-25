using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace xLiAd.ExpressionMove.Test
{
    public class UnitTest2
    {
        private List<Model2> L2 = new List<Model2>() { new Model2() { CreateTime = DateTime.Now.AddSeconds(-1), Id = 5, Name = "a" } };
        private List<Model3> L3 = new List<Model3>()
        {
            new Model3(){ DateTime = DateTime.Today, id = 5, MyName = "a"},
            new Model3(){ DateTime = DateTime.Today, id = 6, MyName = "a"}
        };
        private MoverTypeConfiguration Configuration { get; } = new MoverTypeConfiguration(x =>
        {
            x.CreateMap<Model1, Model3>()
            .FieldMap(x => x.Id, x => x.id)
            .FieldMap(x => x.Name, x => x.MyName)
            .FieldMap(x => x.CreateTime, x => x.DateTime);
        });

        [Fact]
        public void Test1()
        {
            Expression<Func<Model1, bool>> expression = x => x.Id == 5 && x.Name == "a" && x.CreateTime < DateTime.Now;
            var provider = Configuration.Build();
            var result = provider.Load(expression).MoveTo<Model3>();
            Assert.NotNull(result);
            var l = L3.Where(result.Compile()).ToList();
            Assert.NotEmpty(l);
        }
    }
}
