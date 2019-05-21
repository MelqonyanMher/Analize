using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Analizer.NetCore.Models
{
    public class FireRiskItam
    {
        public Guid Id { get; set; }

        public Guid CityId { get; set; }

        public DateTime Day { get; set; }

        public double Temperature { get; set; }

        public double DewPoint { get; set; }

        public double CompIndicatorDay { get; set; }

        public int Precipitation { get; set; }

        public double CompIndicator { get; set; }

        public byte ClassOfFireRisk { get; set; }



        public City City { get; set; }

    }
}
