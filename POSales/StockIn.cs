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
    public partial class StockIn : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string stitle = "Market";
        MainForm main;
        public StockIn(MainForm mn)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            main = mn;
            LoadStockInList();
        }

        public void LoadStockInList()
        {
            dgvStockIn.Rows.Clear();
            int i = 0;
            try
            {
                cm = new SqlCommand("exec GetStockInList", cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvStockIn.Rows.Add(i, dr[0].ToString(),
                                           dr[3].ToString(),
                                           dr[1].ToString(),
                                           dr[2].ToString(),
                                           //dr[4].ToString(),
                                           dr[5].ToString()
                                        );
                    if (dr[5].ToString() == "Підтверджено")
                    {
                        dgvStockIn.Rows[i - 1].Cells[6].Value = Image.FromFile("../../Image/open_lock.jpg");
                    }
                    else
                    {
                        dgvStockIn.Rows[i - 1].Cells[6].Value = Image.FromFile("../../Image/locked_lock.png");
                    }
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            StockInModule stockInModule = new StockInModule();
            stockInModule.btnSave.Enabled = true;

            try
            {
                cn.Open();
                cm = new SqlCommand("insert into StockIn (Sti_Date) values (NULL)", cn);
                cm.ExecuteNonQuery();
                cm = new SqlCommand("SELECT TOP 1 Sti_Id FROM StockIn ORDER BY Sti_Id DESC", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    stockInModule.LoadDoc(dr[0].ToString());
                }
                stockInModule.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message);
            }
            finally
            {
                cn.Close();
                dr.Close();
            }    
        }

        private void dgvStockIn_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            StockInModule stockInModule = new StockInModule();
            stockInModule.LoadDoc(dgvStockIn.Rows[e.RowIndex].Cells[1].Value.ToString());
            stockInModule.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            LoadStockInList();
        }

        private void dgvStockIn_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && dgvStockIn.Rows[e.RowIndex].Cells[5].Value.ToString() == "Підтверджено")
            {
                e.CellStyle.BackColor = Color.LightGreen;
            }
        }

        private void dgvStockIn_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvStockIn.Columns[e.ColumnIndex].Name;
            if (colName == "Delete" && dgvStockIn.Rows[e.RowIndex].Cells[5].Value.ToString() != "Підтверджено")
            {
                if (MessageBox.Show("Видалити обране замовлення", "Видалити", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("DELETE FROM StockIn WHERE Sti_Id = " + dgvStockIn.Rows[e.RowIndex].Cells[1].Value.ToString(), cn);
                    cn.Open();
                    int res = cm.ExecuteNonQuery();
                    cn.Close();
                    if (res > 0)
                    {
                        MessageBox.Show("Замовлення було успішно видалено!");
                        LoadStockInList();
                    }
                    else
                    {
                        MessageBox.Show("Сталася помилка при видалені замовлення!");
                    }
                }
            }
        }
    }
}
