using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Analizer.NetCore.Models
{
    [DataContract]
    public class GraphModel
    {
        public GraphModel(string d,int lev)
        {
            Day = d;
            Level = lev;
        }
        [DataMember(Name ="day")]
        public string Day { get; set; }

        [DataMember(Name ="level")]
        public int Level { get; set; }
    }
}
