using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Analizer.NetCore.Models
{
    public class City
    {
        public Guid Id { get; set; }

        public string Name { get; set; }



        public ICollection<FireRiskItam> FireRiskItams { get; set; }
    }
}
