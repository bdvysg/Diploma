using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSales
{
    public partial class Record : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        public Record()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            LoadCriticalItems();
            LoadEmployees();
            LoadReceipts();
        }

        public void LoadTopSelling()
        {
            int i = 0;
            dgvTopSelling.Rows.Clear();

            try
            {
                var series = new System.Windows.Forms.DataVisualization.Charting.Series("Series1");
                series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                chart1.Series.Clear(); // Очистка предыдущих данных, если они есть
                cn.Open();
                cm = new SqlCommand("exec GetTopSellingProducts @startDate, @endDate, 5", cn);
                cm.Parameters.AddWithValue("@startDate", dtFromTopSell.Value.ToString("yyyyMMdd"));
                cm.Parameters.AddWithValue("@endDate", dtToTopSell.Value.ToString("yyyyMMdd"));
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvTopSelling.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
                    series.Points.AddXY(dr[1].ToString(), int.Parse(dr[2].ToString()));
                }
                chart1.Series.Add(series);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dr.Close();
                cn.Close();
            }
        }

        public void LoadCriticalItems()
        {
            try
            {
                dgvCriticalItems.Rows.Clear();
                int i = 0;
                cn.Open();
                cm = new SqlCommand("exec GetListOfCriticalItems", cn);
                dr = cm.ExecuteReader();
                while(dr.Read())
                {
                    i++;
                    dgvCriticalItems.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());

                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void LoadStockInHist()
        {
            int i = 0;
            dgvStockIn.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT s.*, Sup_Name, CASE Sti_IsConfirmed WHEN 1 THEN 'Підтверджено' WHEN 0 THEN 'Не підтверджено' END FROM StockIn s INNER JOIN Supplier ON Sup_Id = Sti_SupplierId WHERE Sti_Date BETWEEN '" + dtFromStockIn.Value.ToString("yyyyMMdd") + "' AND '" + dtToStockIn.Value.ToString("yyyyMMdd") + "' AND Sti_IsConfirmed = 1", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvStockIn.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[6].ToString(), dr[7].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void btnLoadTopSell_Click(object sender, EventArgs e)
        {
            LoadTopSelling();
        }

        private void btnLoadStockIn_Click(object sender, EventArgs e)
        {
            LoadStockInHist();
        }

        private void btnPrintTopSell_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("exec GetTopSellingProducts @startDate, @endDate, 10", cn);
                cm.Parameters.AddWithValue("@startDate", dtFromTopSell.Value.ToString("yyyyMMdd"));
                cm.Parameters.AddWithValue("@endDate", dtToTopSell.Value.ToString("yyyyMMdd"));
                dr = cm.ExecuteReader();
                Report report = new Report();
                report.GenerateReport("Топ продаж товарів з " + dtFromTopSell.Value.ToString("d") + " по " + dtToTopSell.Value.ToString("d"), dr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dr.Close();
                cn.Close();
            }
        }

        private void btnPrintInventoryList_Click(object sender, EventArgs e)
        {
            POSReport report = new POSReport();
            report.LoadInventory("SELECT * FROM vwInventoryList");
            report.ShowDialog();
        }


        private void btnPrintStockIn_Click(object sender, EventArgs e)
        {
            
        }

        private void btnPrintCriticalItems_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("exec GetListOfCriticalItems", cn);
                dr = cm.ExecuteReader();
                Report report = new Report();
                report.GenerateReport("Список критичних залишків", dr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dr.Close();
                cn.Close();
            }
        }

        private void dgvStockIn_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            StockInModule stockInModule = new StockInModule();
            stockInModule.LoadDoc(dgvStockIn.Rows[e.RowIndex].Cells[1].Value.ToString());
            stockInModule.Show();
        }

        private void LoadEmployees()
        {
            int i = 0;
            try
            {
                cn.Open();
                cm = new SqlCommand("exec GetEmployeeList", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvEmployees.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dr.Close();
                cn.Close();
            }
        }

        private void LoadReceipts()
        {
            int i = 0;
            try
            {
                cn.Open();
                cm = new SqlCommand("exec GetReceiptList", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvReceipts.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dr.Close();
                cn.Close();
            }
        }

        private void dgvReceipts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            cn.Open();
            cm = new SqlCommand("exec GetReceiptInfo @transno", cn);
            cm.Parameters.AddWithValue("@transno", dgvReceipts.Rows[e.RowIndex].Cells[1].Value);
            dr = cm.ExecuteReader();

            Report report = new Report();
            report.GenerateReceipt(dr);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            try
            {
                var series = new System.Windows.Forms.DataVisualization.Charting.Series("Series1");
                series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                chart2.Series.Clear(); // Очистка предыдущих данных, если они есть
                cn.Open();
                cm = new SqlCommand("exec GetProfit @dateStart, @dateEnd", cn);
                cm.Parameters.AddWithValue("@dateStart", dtFromProfit.Value.ToString("yyyyMMdd"));
                cm.Parameters.AddWithValue("@dateEnd", dtToProfit.Value.ToString("yyyyMMdd"));
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvProfit.Rows.Add(i, dr[0].ToString(), dr[1].ToString());
                    series.Points.AddXY(dr[0].ToString(), double.Parse(dr[1].ToString()));
                }
                chart2.Series.Add(series);
                var axisY = chart2.ChartAreas[0].AxisY;
                axisY.LabelStyle.Format = "C";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dr.Close();
                cn.Close();
            }
        }

        private void btnPrintProfit_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("exec GetProfit @dateStart, @dateEnd", cn);
                cm.Parameters.AddWithValue("@dateStart", dtFromProfit.Value.ToString("yyyyMMdd"));
                cm.Parameters.AddWithValue("@dateEnd", dtToProfit.Value.ToString("yyyyMMdd"));
                dr = cm.ExecuteReader();
                Report report = new Report();
                report.GenerateReport("Прибуток за період: " + dtFromProfit.Value.ToString("d") + " - " + dtToProfit.Value.ToString("d"), dr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dr.Close();
                cn.Close();
            }
        }

        private void dgvProfit_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2) // Замените yourCurrencyColumnIndex на индекс столбца с валютой
            {
                if (e.Value != null)
                {
                    decimal value = decimal.Parse(e.Value.ToString()); // Предполагается, что значение является числом типа decimal
                    e.Value = value.ToString("C"); // Форматирование значения с использованием знака валюты
                }
            }
        }

        private void dgvTopSelling_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2) // Замените yourCurrencyColumnIndex на индекс столбца с валютой
            {
                if (e.Value != null)
                {
                    decimal value = decimal.Parse(e.Value.ToString()); // Предполагается, что значение является числом типа decimal
                    e.Value = value.ToString("C"); // Форматирование значения с использованием знака валюты
                }
            }
        }

        private void dgvCriticalItems_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 5) // Замените yourCurrencyColumnIndex на индекс столбца с валютой
            {
                if (e.Value != null)
                {
                    decimal value = decimal.Parse(e.Value.ToString()); // Предполагается, что значение является числом типа decimal
                    e.Value = value.ToString("C"); // Форматирование значения с использованием знака валюты
                }
            }
        }

        private void dgvReceipts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 4) // Замените yourCurrencyColumnIndex на индекс столбца с валютой
            {
                if (e.Value != null)
                {
                    decimal value = decimal.Parse(e.Value.ToString()); // Предполагается, что значение является числом типа decimal
                    e.Value = value.ToString("C"); // Форматирование значения с использованием знака валюты
                }
            }
        }
    }
}
