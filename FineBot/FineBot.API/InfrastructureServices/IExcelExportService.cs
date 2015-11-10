using System.Collections.Generic;

namespace FineBot.API.InfrastructureServices
{
    public interface IExcelExportService<T>
    {
        byte[] WriteObjectData(List<T> objectData, string sheetName);
    }
}
