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
        private IDataRiderMeneger _dataReader;
        public FireRiskMeneger(FireRiskContext context,IDataRiderMeneger dataReader)
        {
            _context = context;
            _dataReader = dataReader;
        }

        public void DeleteHistory()
        {
            _dataReader.DeleteAsync();
        }

        public  IEnumerable<FireRiskItam> GetCityItam(string cityName)
        {
             _dataReader.GetDataAndFillDb();
             return _context.Itams.Include(itm => itm.City).Where(itm => itm.City.Name.ToUpper() == cityName.ToUpper());
        }
        public IEnumerable<FireRiskItam> GetCityItam(string cityName,int year)
        {
            _dataReader.GetDataByYearAndFillDb(year);
            return _context.Itams.Include(itm => itm.City).Where(itm => itm.City.Name.ToUpper() == cityName.ToUpper());
        }

        public IEnumerable<FireRiskItam> GetToDayItams()
        {
            _dataReader.GetDataAndFillDb();
            return _context.Itams.Where(itm => itm.Day.Day == DateTime.Now.Day);
        }
    }
}
