using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Analizer.NetCore.Services
{
    public interface IDbFillMeneger
    {
        Task GetDataAndFillDbAsync();
        Task GetDataByYearAndFillDbAsync(int year);
        Task DeleteAsync();

    }
}
