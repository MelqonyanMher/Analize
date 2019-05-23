using Analizer.NetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Analizer.NetCore.Services
{
    public class DataReaderFromBaseDb : IDataReaderFromBaseDb
    {
        private HidrometDbContext _context;
        
        public DataReaderFromBaseDb(HidrometDbContext context)
        {
            _context = context;
        }
        public IEnumerable<DataItam> GetElemsFromDb(DateTime? startDay)
        {
            List<DataItam> data = new List<DataItam>();
            List<Hidromet> dt;
            if (startDay != null)
            {
                 dt = _context.Hidromet.Where(itm => itm.Date.Value.Day > startDay.Value.Day).ToList();
            }
            else
            {
                dt = _context.Hidromet.Where(itm => itm.Date.Value.Year==DateTime.Now.Year ).Where(itm=>itm.Date.Value.Month>5&&itm.Date.Value.Month<9).ToList();
            }
            foreach(var itam in dt)
            {
                data.Add(new DataItam
                {
                    City = itam.City,
                    Temperature = itam.Temperature,
                    Day = itam.Date.Value,
                    DewPoint = itam.DewPoint,
                    Precipitation = itam.Precipitation
                });
            }
            return data;
        }

        public IEnumerable<DataItam> GetElemsFromDbByYear(int year)
        {
            
        }
    }
}
