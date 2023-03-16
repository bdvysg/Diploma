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

        public void GenerateStockInDoc(SqlDataReader dr)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook workbook = excelApp.Workbooks.Add();
            Worksheet worksheet = workbook.ActiveSheet;

            worksheet.Cells[2, 2] = "Продавець:";
            worksheet.Range["B2"].Font.Bold = true;
            worksheet.Range["B2"].Font.Underline = true;
            worksheet.Range["C2:E2"].Merge();

            worksheet.Cells[3, 2] = "Замовник:";
            worksheet.Range["B3"].Font.Bold = true;
            worksheet.Range["B3"].Font.Underline = true;
            worksheet.Range["C3:E3"].Merge();

            worksheet.Cells[5, 2] = "Номер:";
            worksheet.Range["B5"].Font.Bold = true;
            worksheet.Range["B5"].Font.Underline = true;
            worksheet.Range["C5:E5"].Merge();

            worksheet.Cells[6, 2] = "Примітка:";
            worksheet.Range["B6"].Font.Bold = true;
            worksheet.Range["B6"].Font.Underline = true;
            worksheet.Range["C6:E6"].Merge();

            worksheet.Cells[8, 1] = "No";
            for (int i = 7; i < dr.FieldCount - 1; i++)
            {
                string columnName = dr.GetName(i);
                worksheet.Cells[8, i - 5] = columnName;
            }
            worksheet.Range["A8:D8"].Font.Bold = true;
            worksheet.Range["A8:D8"].Borders.Color = Color.Black;

            int row = 8;
            int j = 0;
            while (dr.Read())
            {
                j++;
                row++;
                for (int col = 7; col < dr.FieldCount - 1; col++)
                {
                    worksheet.Cells[row, 1] = j.ToString();
                    worksheet.Cells[row, col - 5] = dr[col];
                    worksheet.Cells[row + 1, 4] = dr[10];
                    worksheet.Columns[col - 5].AutoFit();

                    worksheet.Cells[2, 3] = dr[3];
                    worksheet.Cells[3, 3] = dr[1];
                    worksheet.Cells[5, 3] = dr[0];
                }
            }
            worksheet.Columns[1].AutoFit();
            worksheet.Range["A9:D" + row].Borders.Color = Color.Black;

            worksheet.Range["D" + (row + 1)].Font.Bold = true;
            worksheet.Range["D" + (row + 1)].Borders.Color = Color.Black;

            excelApp.Visible = true;

            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(excelApp);
        }

        public void GenerateReceipt(SqlDataReader dr)
        {
            string cashier = "";
            string store = "";
            string address = "";
            string date = "";
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook workbook = excelApp.Workbooks.Add();
            Worksheet worksheet = workbook.ActiveSheet;

            worksheet.Cells[2, 2] = "Товарний чек:";
            worksheet.Range["B2"].Font.Bold = true;
            worksheet.Range["B2"].Font.Underline = true;
            worksheet.Range["C2:E2"].Merge();


            worksheet.Cells[5, 1] = "№";
            for (int i = 0; i < dr.FieldCount - 6; i++)
            {
                string columnName = dr.GetName(i);
                worksheet.Cells[5, i + 2] = columnName;
            }
            worksheet.Range["A5:G5"].Font.Bold = true;
            worksheet.Range["A5:G5"].Borders.Color = Color.Black;

            int row = 5;
            int j = 0;
            while (dr.Read())
            {
                j++;
                row++;
                for (int col = 0; col < dr.FieldCount - 6; col++)
                {
                    worksheet.Cells[row, 1] = j.ToString();
                    worksheet.Cells[row, col + 2] = dr[col];
                    worksheet.Cells[row + 1, 7] = dr[6];
                    worksheet.Cells[2, 3] = dr[7];
                    worksheet.Columns[col + 1].AutoFit();

                    cashier = dr[8].ToString();
                    store = dr[9].ToString();
                    address = dr[10].ToString();
                    date = dr[11].ToString();
                }
            }
            worksheet.Columns[1].AutoFit();
            worksheet.Range["A6:G" + row].Borders.Color = Color.Black;

            worksheet.Range["C6:C" + row].NumberFormat = "#,##0.00 ₴";
            worksheet.Range["F6:F" + row].NumberFormat = "#,##0.00 ₴";
            worksheet.Range["G6:G" + row + 1].NumberFormat = "#,##0.00 ₴";
            //worksheet.Range["E6:E" + row].NumberFormat = "#,##0.00 %";

            worksheet.Range["G" + (row + 1)].Font.Bold = true;
            worksheet.Range["G" + (row + 1)].Borders.Color = Color.Black;

            worksheet.Cells[row + 3, 2] = "Касир: ";
            worksheet.Cells[row + 3, 3] = cashier;
            worksheet.Range["B" + row + 3].Font.Bold = true;
            worksheet.Range["B" + row + 3].Font.Underline = true;

            worksheet.Cells[row + 4, 2] = "Магазин:";
            worksheet.Cells[row + 4, 3] = store;
            worksheet.Range["B" + row + 4].Font.Bold = true;
            worksheet.Range["B" + row + 4].Font.Underline = true;

            worksheet.Cells[row + 5, 2] = "Адреса:";
            worksheet.Cells[row + 5, 3] = address;
            worksheet.Range["B" + row + 5].Font.Bold = true;
            worksheet.Range["B" + row + 5].Font.Underline = true;

            worksheet.Cells[row + 6, 2] = "Дата:";
            worksheet.Cells[row + 6, 3] = date;
            worksheet.Range["B" + row + 6].Font.Bold = true;
            worksheet.Range["B" + row + 6].Font.Underline = true;


            excelApp.Visible = true;

            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(excelApp);
        }
    }
}
