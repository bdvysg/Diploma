using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
namespace POSales
{
    public class Report
    {
        private string documentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Звіти");

        public void Test()
        {
            Application excelApp = new Application();
            Workbook workbook = excelApp.Workbooks.Add();
            Worksheet worksheet = workbook.ActiveSheet;

            worksheet.Cells[1, 1] = "Name";
            worksheet.Cells[1, 2] = "Age";
            worksheet.Cells[2, 1] = "John";
            worksheet.Cells[2, 2] = 25;

            worksheet.Range["A1:B1"].Font.Bold = true;
            worksheet.Range["A1:B1"].Borders.LineStyle = XlLineStyle.xlContinuous;
            worksheet.Columns[1].AutoFit();

            workbook.SaveAs("Report.xlsx");
            excelApp.Visible = true;
            
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(excelApp);
        }

        public string GenerateDocName(string name)
        {
            string docName;
            DateTime currTime = DateTime.Now;
            docName = name + " " + currTime.ToString("dd-MM-yyyy HH-mm");
            return docName;
        }
    }
}
