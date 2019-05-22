using Analizer.NetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Analizer.NetCore.Services
{
    public class DataRiderMeneger : IDataRiderMeneger
    {
        private FireRiskContext _context;
        public DataRiderMeneger(FireRiskContext context)
        {
            _context = context;
        }
        
        public async Task GetDataAndFillDb()
        {
            City[] c =_context.Cities.ToArray();
            List<DataItam> datas = new List<DataItam>();//read data from db orderby date
            FireRiskItam[] fireRiskArr = _context.Itams.OrderBy(i => i.Day).TakeLast(11).ToArray();
            List<FireRiskItam> fireRiskItams = new List<FireRiskItam>();
            foreach(DataItam  d in datas)
            {
                for(int i = 0; i < c.Length; i++)
                {
                    if(d.City == c[i].Name)
                    {
                        FireRiskItam itm = new FireRiskItam()
                        {
                            Day = d.Day,
                            CityId = c[i].Id,
                            Precipitation = d.Precipitation > 3 ? d.Precipitation : 0,
                            DewPoint = d.DewPoint,
                            Temperature = d.Temperature,
                            CompIndicatorDay = d.Temperature * (d.Temperature - d.DewPoint),
                            CompIndicator = d.Temperature * (d.Temperature - d.DewPoint) + (d.Precipitation > 3 ? 0 : fireRiskArr.Single(j => j.CityId == c[i].Id).CompIndicator)
                        };

                        for(int k = 0; k < fireRiskArr.Length; k++)
                        {
                            if(fireRiskArr[i].CityId == itm.CityId)
                            {
                                fireRiskArr[i] = itm;
                            }
                        }
                        fireRiskItams.Add(itm);
                        break;
                    }
                }
            }
            await _context.Itams.AddRangeAsync(fireRiskItams);
        }

        public Task GetDataForYearAndFillDb(int year)
        {
            throw new NotImplementedException();
        }
    }
}
