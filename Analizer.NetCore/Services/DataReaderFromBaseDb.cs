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
            IQueryable<IGrouping<int,Hidromet>> dt;
            if (startDay != null)
            {
                 dt = _context.Hidromet.Where
                    (itm =>itm.Date.Value.Year > startDay.Value.Year
                    && itm.Date.Value.DayOfYear > startDay.Value.DayOfYear
                    &&  itm.Date.Value.Month <9)
                    .GroupBy(itm=>itm.Date.Value.DayOfYear);
            }
            else
            {
                dt = _context.Hidromet.Where
                    (itm => itm.Date.Value.Year == DateTime.Now.Year
                    && itm.Date.Value.Month > 5
                    && itm.Date.Value.Month < 9)
                    .GroupBy(itm=>itm.Date.Value.DayOfYear);
            }
            foreach (var dayGrupItam in dt)
            {
                var dtGroupedCity = dayGrupItam.GroupBy(itm => itm.City);
                foreach (var itam in dtGroupedCity)
                {
                    data.Add(new DataItam
                    {
                        City = itam.First().City,
                        Temperature = itam.Single(itm => itm.Date.Value.Hour == 12).Temperature,
                        Day = itam.Single(itm => itm.Date.Value.Hour == 12).Date.Value,
                        DewPoint = itam.Single(itm => itm.Date.Value.Hour == 12).DewPoint,
                        Precipitation = itam.Sum(itm => itm.Precipitation)
                    });
                }
            }
            return data;
        }

        public IEnumerable<DataItam> GetElemsFromDbByYear(int year)
        {
            List<DataItam> data = new List<DataItam>();
            var dt = _context.Hidromet.Where(itm => itm.Date.Value.Year == year && itm.Date.Value.Month > 5 && itm.Date.Value.Month < 9).GroupBy(itm => itm.Date.Value.DayOfYear);

            foreach (var dayGrupItam in dt)
            {
                var dtGroupedCity = dayGrupItam.GroupBy(itm => itm.City);
                foreach (var itam in dtGroupedCity)
                {
                    data.Add(new DataItam
                    {
                        City = itam.First().City,
                        Temperature = itam.Single(itm => itm.Date.Value.Hour == 12).Temperature,
                        Day = itam.Single(itm => itm.Date.Value.Hour == 12).Date.Value,
                        DewPoint = itam.Single(itm => itm.Date.Value.Hour == 12).DewPoint,
                        Precipitation = itam.Sum(itm => itm.Precipitation)
                    });
                }
            }
            return data;
        }
    }
}
