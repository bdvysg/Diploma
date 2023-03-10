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
    public partial class LookUpProduct : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;        
        Cashier cashier;
        int StartIndex = 0;
        int RowCount = 25;
        int i = 0;

        public LookUpProduct(Cashier cash)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            cashier = cash;
            LoadProduct();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
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
                                           dr[4].ToString(),
                                           dr[1].ToString(),
                                           dr[2].ToString(),
                                           dr[3].ToString(),
                                           dr[5].ToString(),
                                           dr[6].ToString(),
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
                Qty qty = new Qty(cashier);
                qty.ProductDetails(dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString(), double.Parse(dgvProduct.Rows[e.RowIndex].Cells[7].Value.ToString()), cashier.lblTranNo.Text, int.Parse(dgvProduct.Rows[e.RowIndex].Cells[8].Value.ToString()));
                qty.ShowDialog();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void LookUpProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }

        private void dgvProduct_Scroll(object sender, ScrollEventArgs e)
        {
            int displayedRows = dgvProduct.DisplayedRowCount(true);
            int lastVisibleRowIndex = dgvProduct.FirstDisplayedScrollingRowIndex + displayedRows - 1;
            try
            {
                if (lastVisibleRowIndex >= dgvProduct.RowCount - 1)
                {
                    StartIndex += 25;
                    cm = new SqlCommand("exec GetProductsList '" + txtSearch.Text + "', " + StartIndex.ToString() + ", " + RowCount.ToString(), cn);
                    cn.Open();
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        i++;
                        dgvProduct.Rows.Add(i, dr[0].ToString(),
                                               dr[4].ToString(),
                                               dr[1].ToString(),
                                               dr[2].ToString(),
                                               dr[3].ToString(),
                                               dr[5].ToString(),
                                               dr[6].ToString(),
                                               dr[8].ToString()
                                            );
                    }
                }
            }
            catch( Exception ex ) 
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
