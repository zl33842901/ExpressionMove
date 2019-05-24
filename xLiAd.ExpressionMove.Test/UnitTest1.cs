using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace xLiAd.ExpressionMove.Test
{
    public class UnitTest1
    {
        private List<Model2> L2 = new List<Model2>() { new Model2() { CreateTime = DateTime.Now.AddSeconds(-1), Id = 5, Name = "a" } };
        private List<Model3> L3 = new List<Model3>()
        {
            new Model3(){ DateTime = DateTime.Today, id = 5, MyName = "a"},
            new Model3(){ DateTime = DateTime.Today, id = 6, MyName = "a"}
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

        [Fact]
        public void Test2()
        {
            Expression<Func<Model1, bool>> expression = x => x.Id == 5 && x.Name == "a" && x.CreateTime < DateTime.Now;
            var mover = expression.BuildMover();
            var tm = mover.TypeMapper<Model3>();
            tm.Add(x => x.Id, x => x.id);
            tm.Add(x => x.Name, x => x.MyName);
            tm.Add(x => x.CreateTime, x => x.DateTime);
            Expression<Func<Model3, bool>> result = mover.MoveTo<Model3>();
            Assert.NotNull(result);
            var l = L3.Where(result.Compile()).ToList();
            Assert.NotEmpty(l);
        }
    }
}