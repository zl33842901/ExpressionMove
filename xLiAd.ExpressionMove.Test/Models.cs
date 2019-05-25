using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.ExpressionMove.Test
{
    public class Model1
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public string[] Values { get; set; }//["a", "b", "c"]
        public int[] IntValues { get; set; }//[1, 2, 3]
    }
    public class Model2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
    }
    public class Model3
    {
        public int id { get; set; }
        public string MyName { get; set; }
        public DateTime CreateTime { get; set; }
    }
    public class Model4
    {
        public int id { get; set; }
        public string values { get; set; }//",a,b,c,"
        public string intvalues { get; set; }//",1,2,3,"
    }
}
