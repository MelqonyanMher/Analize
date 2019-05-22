using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Analizer.NetCore.Services
{
    public class DataItam
    {
        public string City { get; set; }

        public DateTime Day { get; set; }

        public double Temperature { get; set; }

        public double DewPoint { get; set; }

        public int Precipitation { get; set; }

    }
}
