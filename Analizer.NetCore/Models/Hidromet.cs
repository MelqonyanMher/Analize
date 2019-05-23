using System;
using System.Collections.Generic;

namespace Analizer.NetCore.Models
{
    public partial class Hidromet
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string City { get; set; }
        public double Temperature { get; set; }
        public double DewPoint { get; set; }
        public int Precipitation { get; set; }
        public int Wind { get; set; }
        public int MeterologicalPressure { get; set; }
    }
}
