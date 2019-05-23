using Analizer.NetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Analizer.NetCore.Services
{
    public class DbFillMeneger : IDbFillMeneger
    {
        private FireRiskContext _context;
        public DbFillMeneger(FireRiskContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync()
        {
            _context.RemoveRange(_context.Itams.Where(itm => itm.Day.Year != DateTime.Now.Year));
            await _context.SaveChangesAsync();
        }

        public async Task GetDataAndFillDb()
        {
            List<DataItam> datas = new List<DataItam>(); //add part read form db
            List<FireRiskItam> addingItams = new List<FireRiskItam>();
            DateTime lastDay = _context.Itams.OrderBy(i => i.Day).Last().Day;
            City[] citys = _context.Cities.ToArray();
            for(int i = 0; i < citys.Length; i++)
            {
                var d = datas.Where(itm => itm.Day > lastDay)
                    .Where(itm => itm.City == citys[i].Name).OrderBy(itm => itm.Day).ToArray();
                var lastKPItam = _context.Itams.Where(itm => itm.CityId == citys[i].Id).OrderBy(itm=>itm.Day).Last();

                double lastKP = 0.0;

                if (lastKPItam != null)
                {
                     lastKP = lastKPItam.CompIndicator;
                }

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
                        CompIndicator = d[j].Temperature * (d[j].Temperature - d[j].DewPoint) + (d[j].Precipitation > 3 ? 0 : lastKP)
                    };
                    itam.ClassOfFireRisk =(byte) (itam.CompIndicator < 301 ? 1
                        : itam.CompIndicator < 1001 ? 2
                        : itam.CompIndicator < 4001 ? 3
                        : itam.CompIndicator < 10001 ? 4
                        : 5);
                    lastKP = itam.CompIndicator;
                    addingItams.Add(itam);

                }
            }
            await _context.Itams.AddRangeAsync(addingItams);
            await _context.SaveChangesAsync();
        }

        public async Task GetDataByYearAndFillDb(int year)
        {
            List<DataItam> datas = new List<DataItam>();//Add part read from db
            List<FireRiskItam> addingItams = new List<FireRiskItam>();
            City[] citys = _context.Cities.ToArray();
            for (int i = 0; i < citys.Length; i++)
            {
                var d = datas.Where(itm => itm.City == citys[i].Name).OrderBy(itm => itm.Day).ToArray();
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
                        CompIndicator = d[j].Temperature * (d[j].Temperature - d[j].DewPoint) + (d[j].Precipitation > 3 ? 0 : lastKP)
                    };
                    lastKP = itam.CompIndicator;
                    addingItams.Add(itam);

                }
            }
            await _context.Itams.AddRangeAsync(addingItams);
            await _context.SaveChangesAsync();
        }
    }
}
