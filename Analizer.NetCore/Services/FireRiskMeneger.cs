using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Analizer.NetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Analizer.NetCore.Services
{
    public class FireRiskMeneger : IFireRiskMeneger
    {
        private FireRiskContext _context;
        private IDbFillMeneger _dbFill;
        public FireRiskMeneger(FireRiskContext context,IDbFillMeneger dataFill)
        {
            _context = context;
            _dbFill = dataFill;
        }

        public void DeleteHistory()
        {
            _dbFill.DeleteAsync().Wait();
        }

        public  IEnumerable<FireRiskItam> GetCityItam(string cityName)
        {
             _dbFill.GetDataAndFillDbAsync().Wait();
             return _context.Itams.Include(itm => itm.City)
                .Where(itm => itm.City.Name.ToUpper() == cityName.ToUpper());
        }
        public IEnumerable<FireRiskItam> GetCityItam(string cityName,int year)
        {
            _dbFill.GetDataByYearAndFillDbAsync(year).Wait();
            return _context.Itams.Include(itm => itm.City)
                .Where(itm => itm.City.Name.ToUpper() == cityName.ToUpper() 
                && itm.Day.Year==year)
                .OrderBy(itm=>itm.Day);
        }

        public IEnumerable<FireRiskItam> GetToDayItams()
        {
             _dbFill.GetDataAndFillDbAsync().Wait();
            if (DateTime.Now.Hour < 12)
            {
                return _context.Itams.Where(itm => itm.Day.DayOfYear == DateTime.Now.DayOfYear - 1).Include(itm => itm.City);
            }
            else
            {
                return _context.Itams.Where(itm => itm.Day.DayOfYear == DateTime.Now.DayOfYear +10).Include(itm => itm.City);
            }
        }
    }
}
