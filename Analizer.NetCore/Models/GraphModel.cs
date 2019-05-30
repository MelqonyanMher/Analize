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
            Label = d;
            Y = lev;
        }
        [DataMember(Name = "label")]
        public string Label { get; set; }

        [DataMember(Name ="y")]
        public Nullable<double> Y { get; set; }
    }
}
