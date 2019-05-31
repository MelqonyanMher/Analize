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

            List<DataItam> returnData = new List<DataItam>();
            IQueryable<IGrouping<int, Hidromet>> dbData;

            if (startDay != null)
            {
                if (startDay.Value.DayOfYear == DateTime.Now.DayOfYear)
                {
                    return null;
                }
                dbData = _context.Hidromet.Where
                   (itm => itm.Date.Value.Year == startDay.Value.Year
                   && itm.Date.Value.DayOfYear > startDay.Value.DayOfYear
                   && itm.Date.Value.Month < 9)
                   .GroupBy(itm => itm.Date.Value.DayOfYear);
            }
            else
            {
                dbData = _context.Hidromet.Where
                    (itm => itm.Date.Value.Year == DateTime.Now.Year
                    && itm.Date.Value.Month > 5
                    && itm.Date.Value.Month < 9)
                    .GroupBy(itm => itm.Date.Value.DayOfYear);
            }
            foreach (var dayGrupItam in dbData)
            {
                var dtGroupedCity = dayGrupItam.GroupBy(itm => itm.City);
                foreach (var itam in dtGroupedCity)
                {
                    returnData.Add(new DataItam
                    {
                        City = itam.First().City,
                        Temperature = itam.Single(itm => itm.Date.Value.Hour == 12).Temperature,
                        Day = itam.Single(itm => itm.Date.Value.Hour == 12).Date.Value,
                        DewPoint = itam.Single(itm => itm.Date.Value.Hour == 12).DewPoint,
                        Precipitation = itam.Sum(itm => itm.Precipitation)
                    });
                }
            }
            return returnData;
        }

        public IEnumerable<DataItam> GetElemsFromDbByYear(int year)
        {
            List<DataItam> returnData = new List<DataItam>();
            IQueryable<IGrouping<int, Hidromet>> dbData;

            dbData = _context.Hidromet.Where
               (itm => itm.Date.Value.Year == year
               && itm.Date.Value.Month>5
               && itm.Date.Value.Month < 9)
               .GroupBy(itm => itm.Date.Value.DayOfYear);

            if (dbData.Count() == 0)
            {
                return null;
            }

            foreach (var dayGrupItam in dbData)
            {
                var dtGroupedCity = dayGrupItam.GroupBy(itm => itm.City);
                foreach (var itam in dtGroupedCity)
                {
                    returnData.Add(new DataItam
                    {
                        City = itam.First().City,
                        Temperature = itam.Single(itm => itm.Date.Value.Hour == 12).Temperature,
                        Day = itam.Single(itm => itm.Date.Value.Hour == 12).Date.Value,
                        DewPoint = itam.Single(itm => itm.Date.Value.Hour == 12).DewPoint,
                        Precipitation = itam.Sum(itm => itm.Precipitation)
                    });
                }
            }
            return returnData;
        }
    }
}
