using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Analizer.NetCore.Services
{
    public interface IDataRiderMeneger
    {
        Task GetDataAndFillDb();
        Task GetDataByYearAndFillDb(int year);
        Task DeleteAsync();

    }
}
