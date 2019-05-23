using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Analizer.NetCore.Services
{
    public interface IDataReaderFromBaseDb
    {
        IEnumerable<DataItam> GetElemsFromDb(DateTime? startDay);
        IEnumerable<DataItam> GetElemsFromDbByYear(int year);

    }
}
