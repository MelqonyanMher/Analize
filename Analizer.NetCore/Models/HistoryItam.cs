using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Analizer.NetCore.Models
{
    public class HistoryItam
    {
        [Required]
        public int Year { get; set; }

        [Required]
        public string City { get; set; }
    }
}
