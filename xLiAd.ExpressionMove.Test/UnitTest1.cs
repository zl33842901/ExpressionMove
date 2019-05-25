using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace xLiAd.ExpressionMove.Test
{
    /// <summary>
    /// 当字段完全一样时，不必使用 MoverTypeConfiguration
    /// </summary>
    public class UnitTest1
    {
        private List<Model2> L2 = new List<Model2>() { new Model2() { CreateTime = DateTime.Now.AddSeconds(-1), Id = 5, Name = "a" } };
        private List<Model3> L3 = new List<Model3>()
        {
            new Model3(){ CreateTime = DateTime.Today, id = 5, MyName = "a"},
            new Model3(){ CreateTime = DateTime.Today, id = 6, MyName = "a"}
        };
        [Fact]
        public void Test1()
        {
            Expression<Func<Model1, bool>> expression = x => x.Id == 5 && x.Name == "a" && x.CreateTime < DateTime.Now;
            Expression<Func<Model2, bool>> result = expression.BuildMover().MoveTo<Model2>();

            Assert.NotNull(result);
            var l = L2.Where(result.Compile()).ToList();
            Assert.NotEmpty(l);
        }
    }
}
