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
    public partial class ProductStockIn : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string stitle = "Market";
        StockInModule stockIn;
        int StartIndex = 0;
        int RowCount = 25;
        int i = 0;
        public ProductStockIn(StockInModule stk)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            stockIn = stk;
            LoadProduct();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            stockIn.LoadDoc(stockIn.textBox1.Text);
            stockIn.Select();
            this.Dispose();
        }

        public void LoadProduct()
        {
            i = 0;
            StartIndex = 0;
            dgvProduct.Rows.Clear();
            try
            {
                cm = new SqlCommand("exec GetProductsList '" + txtSearch.Text + "', " + StartIndex.ToString() + ", " + RowCount.ToString(), cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvProduct.Rows.Add(i, dr[0].ToString(),
                                           dr[1].ToString(),
                                           dr[8].ToString()
                                        );
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

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProduct.Columns[e.ColumnIndex].Name;
            if (colName == "Select")
            {
                if (MessageBox.Show("Додати цей товар?", stitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    addStockIn(dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString());
                }
            }
        }

        public void addStockIn(string productId)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("INSERT INTO StockInProduct (Sip_Doc, Sip_Product) VALUES (@id, @productId)", cn);
                cm.Parameters.AddWithValue("@id", stockIn.textBox1.Text);
                cm.Parameters.AddWithValue("@productId", productId);
                if (cm.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Товар успішно додано", stitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Сталася помилка при додавані товару", stitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, stitle);
            }
            finally
            {
                cn.Close();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void ProductStockIn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }

        private void dgvProduct_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ProductModule product = new ProductModule();
            product.LoadProduct(dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString());
            product.label1.Text = "Інформація про товар";
            product.txtPcode.Enabled = false;
            product.btnSave.Enabled = false;
            product.btnUpdate.Enabled = true;
            product.ShowDialog();
        }

        private void dgvProduct_Scroll(object sender, ScrollEventArgs e)
        {
            int displayedRows = dgvProduct.DisplayedRowCount(true);
            int lastVisibleRowIndex = dgvProduct.FirstDisplayedScrollingRowIndex + displayedRows - 1;
            if (lastVisibleRowIndex >= dgvProduct.RowCount - 1)
            {
                StartIndex += 25;
                try
                {
                    cm = new SqlCommand("exec GetProductsList '" + txtSearch.Text + "', " + StartIndex.ToString() + ", " + RowCount.ToString(), cn);
                    cn.Open();
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        i++;
                        dgvProduct.Rows.Add(i, dr[0].ToString(),
                                               dr[1].ToString(),
                                               dr[8].ToString()
                                            );
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
        }
    }
}
