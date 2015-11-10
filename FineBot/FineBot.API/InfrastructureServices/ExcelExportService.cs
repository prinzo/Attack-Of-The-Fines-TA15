using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace FineBot.API.InfrastructureServices
{
    public class ExcelExportService<T> : IExcelExportService<T> where T : class
    {
        public byte[] WriteObjectData(List<T> objectData, string sheetName)
        {
            using (var package = new ExcelPackage())
            {
                var workbook = package.Workbook;
                var workSheet = workbook.Worksheets.Add(sheetName);

                var objectProperties = typeof(T).GetProperties();

                var headerCount = 0;

                foreach (var heading in objectProperties)
                {
                    headerCount++;
                    workSheet.Cells[1, headerCount].Value = heading.Name;
                    workSheet.Cells[1, headerCount].Style.Font.Bold = true;
                    workSheet.Column(headerCount).AutoFit();

                }

                var rowCounter = 1;
                foreach (var item in objectData)
                {
                    rowCounter++;
                    var columnCounter = 0;
                    WriteToFile(workSheet, rowCounter, columnCounter, objectProperties, item);
                }
                return package.GetAsByteArray();
            }
        }

        public void WriteToFile(ExcelWorksheet excelWorksheet, int rowCounter, int columnCounter, PropertyInfo[] propertyInfos, T objectData)
        {
            foreach (var property in propertyInfos)
            {
                columnCounter++;
                var val = property.GetValue(objectData, null);
                if (val is DateTime)
                {
                    val = val.ToString();
                }
                excelWorksheet.Cells[rowCounter, columnCounter].Value = val ?? "";
                excelWorksheet.Column(columnCounter).AutoFit();

            }
        }
    }

}
