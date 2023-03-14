using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace POSales
{
    public class Report
    {
        private string documentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Звіти");

        public void Test()
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook workbook = excelApp.Workbooks.Add();
            Worksheet worksheet = workbook.ActiveSheet;

            worksheet.Cells[1, 1] = "Найбільш продані товари";
            worksheet.Cells[2, 1] = "Згенеровано - " + DateTime.Now.ToString();
            worksheet.Cells[4, 1] = "John";
            worksheet.Cells[5, 2] = 25;

            worksheet.Range["A1:B1"].Font.Bold = true;
            worksheet.Range["A3:B5"].Borders.Color = Color.Black;
            worksheet.Range["A3:B5"].Borders.Weight = 2;
            worksheet.Range["A1:D1"].Merge();
            worksheet.Range["A1"].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            worksheet.Columns[1].AutoFit();

            //workbook.SaveAs("Report.xlsx");
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

        public void TopSellingProduct()
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cm = new SqlCommand();
            DBConnect dbcon = new DBConnect();
            SqlDataReader dr;
            cn = new SqlConnection(dbcon.myConnection());

            cn.Open();
            cm = new SqlCommand("exec GetTopSellingProducts '20000101', '20231212', 100", cn);
            dr = cm.ExecuteReader();
            GenerateReport("Топ продаж за весь час", dr);
        }

        public void GenerateReport(string reportName, SqlDataReader dr)
        {
            int columns = dr.FieldCount;
            char ch = (char)('A' + columns - 1);


            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook workbook = excelApp.Workbooks.Add();
            Worksheet worksheet = workbook.ActiveSheet;

            worksheet.Cells[1, 1] = reportName;
            worksheet.Range["A1"].Font.Bold = true;
            worksheet.Range["A1:" + ch + "1"].Merge();
            worksheet.Range["A1"].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            worksheet.Range["A2:" + ch + "2"].Merge();
            worksheet.Range["A2"].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            worksheet.Cells[2, 1] = "Згенеровано - " + DateTime.Now.ToString();

            for (int i = 0; i < dr.FieldCount; i++)
            {
                string columnName = dr.GetName(i);
                worksheet.Cells[4, i + 1] = columnName;
            }
            worksheet.Range["A4:" + ch + "4"].Font.Bold = true;

            int row = 4;
            while (dr.Read())
            {
                row++;
                for (int col = 1; col <= dr.FieldCount; col++)
                {
                    worksheet.Cells[row, col] = dr[col - 1];
                    worksheet.Columns[col].AutoFit();
                }
            }
            worksheet.Range["A4:" + ch + row].Borders.Color = Color.Black;

            excelApp.Visible = true;

            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(excelApp);
        }
    }
}
