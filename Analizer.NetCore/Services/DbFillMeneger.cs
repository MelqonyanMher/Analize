using Analizer.NetCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Analizer.NetCore.Services
{
    public class DbFillMeneger : IDbFillMeneger
    {
        private FireRiskContext _context;
        private IDataReaderFromBaseDb _dBMeneger;

        public DbFillMeneger(FireRiskContext context, IDataReaderFromBaseDb baseMeneger)
        {
            _context = context;
            _dBMeneger = baseMeneger;
        }

        public async Task DeleteAsync()
        {
            var otherYearInDb = _context.Itams.Where(itm => itm.Day.Year != DateTime.Now.Year);

            if (otherYearInDb.Count() > 0)
            {
                _context.RemoveRange(otherYearInDb);
                await _context.SaveChangesAsync();
            }
            else return;
        }

        public async Task GetDataAndFillDbAsync()
        {

            FireRiskItam anyItam = _context.Itams.OrderBy(itm => itm.Day).LastOrDefault();
            DateTime? startDay = null;
            
            if (anyItam != null)
            {
                if (anyItam.Day.DayOfYear == DateTime.Now.DayOfYear)
                {
                    return;
                }
                startDay = anyItam.Day;
            }

            List<DataItam> datasFromDb = _dBMeneger.GetElemsFromDb(startDay).ToList();

            if (datasFromDb == null)
            {
                return;
            }

            List<FireRiskItam> addingItams = new List<FireRiskItam>();

            var dt = datasFromDb.GroupBy(itm => itm.City);
            foreach (var groupedByCityItam in dt)
            {
                var lastKPItam = _context.Itams.Include(itm => itm.City).OrderBy(itm => itm.Day).LastOrDefault(itm => itm.City.Name == groupedByCityItam.First().City);
                var lastKP = 0.0;

                if (lastKPItam != null)
                {
                    lastKP = lastKPItam.CompIndicator;
                }

                foreach (var itam in groupedByCityItam)
                {
                    var fri = new FireRiskItam
                    {
                        CityId = _context.Cities.SingleOrDefault(itm => itm.Name == itam.City).Id,
                        Temperature = itam.Temperature,
                        Day = itam.Day,
                        Precipitation = itam.Precipitation > 3 ? itam.Precipitation : 0,
                        DewPoint = itam.DewPoint,
                        CompIndicatorDay = itam.Temperature * (itam.Temperature - itam.DewPoint),
                        CompIndicator = itam.Temperature * (itam.Temperature - itam.DewPoint) + (itam.Precipitation > 3 ? 0 : lastKP)

                    };
                    fri.ClassOfFireRisk = (byte)(fri.CompIndicator < 301 ? 1
                        : fri.CompIndicator < 1001 ? 2
                        : fri.CompIndicator < 4001 ? 3
                        : fri.CompIndicator < 10001 ? 4 : 5);

                    addingItams.Add(fri);
                    lastKP = fri.CompIndicator;
                }
            }
            await _context.Itams.AddRangeAsync(addingItams);
            await _context.SaveChangesAsync();
        }

        public async Task GetDataByYearAndFillDbAsync(int year)
        {
            FireRiskItam anyItam = _context.Itams.FirstOrDefault(itm => itm.Day.Year == year);

            if (anyItam != null)
            {
                return;
            }

            List<DataItam> datas = _dBMeneger.GetElemsFromDbByYear(year).ToList();

            if (datas.Count == 0)
            {
                return;
            }

            List<FireRiskItam> addingItams = new List<FireRiskItam>();
            City[] citys = _context.Cities.ToArray();

            for (int i = 0; i < citys.Length; i++)
            {
                var d = datas.Where(itm => itm.City == citys[i].Name)
                             .OrderBy(itm => itm.Day)
                             .ToArray();
                var lastKP = 0.0;

                for (int j = 0; j < d.Length; j++)
                {
                    FireRiskItam itam = new FireRiskItam()
                    {
                        Day = d[j].Day,
                        CityId = citys[i].Id,
                        Precipitation = d[j].Precipitation > 3 ? d[j].Precipitation : 0,
                        DewPoint = d[j].DewPoint,
                        Temperature = d[j].Temperature,
                        CompIndicatorDay = d[j].Temperature * (d[j].Temperature - d[j].DewPoint),
                        CompIndicator = d[j].Temperature 
                                            * (d[j].Temperature - d[j].DewPoint) 
                                            + (d[j].Precipitation > 3 ? 0 : lastKP)
                    };
                    lastKP = itam.CompIndicator;
                    itam.ClassOfFireRisk = (byte)(itam.CompIndicator < 301 ? 1
                        : itam.CompIndicator < 1001 ? 2
                        : itam.CompIndicator < 4001 ? 3
                        : itam.CompIndicator < 10001 ? 4 : 5);

                    addingItams.Add(itam);
                }
            }
            await _context.Itams.AddRangeAsync(addingItams);
            await _context.SaveChangesAsync();
        }
    }
}
