using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Analizer.NetCore.Models;

namespace Analizer.NetCore.Services
{
    public class FireRiskMeneger : IFireRiskMeneger
    {
        private FireRiskContext _context;
        public FireRiskMeneger(FireRiskContext context)
        {
            _context = context;
        }
        public  IEnumerable<FireRiskItam> GetCityItam(string cityName)
        {
            if (_context.Itams.OrderBy(i => i.Day).Last().Day.Day == DateTime.Now.Day)
            {
                return _context.Itams.Where(p => p.City.Name == cityName.ToUpper());
            }
        }
        public IEnumerable<FireRiskItam> GetCityItam(string cityName,int year)
        {
            if (_context.Itams.OrderBy(i => i.Day).Last().Day.Day == DateTime.Now.Day)
            {
                return _context.Itams.Where(p => p.City.Name == cityName.ToUpper());
            }
        }

        public IEnumerable<FireRiskItam> GetToDayItams()
        {
            if (_context.Itams.OrderBy(i => i.Day).Last().Day.Day == DateTime.Now.Day)
            {
                return _context.Itams.Where(p => p.Day.Day == DateTime.Now.Day);
            }
            else
            {

            }
        }
    }
}
