using Analizer.NetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Analizer.NetCore.Services
{
    public interface IFireRiskMeneger
    {
        IEnumerable<FireRiskItam> GetToDayItams();

        IEnumerable<FireRiskItam> GetCityItam(string cityName);

        IEnumerable<FireRiskItam> GetCityItam(string cityName,int year);
    }
}
