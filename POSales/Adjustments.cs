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
    public partial class Adjustments : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        MainForm main;
        int _qty;
        int StartIndex = 0;
        int RowCount = 25;
        int i = 0;
        public Adjustments(MainForm mn)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            main = mn;
            LoadStock();
            lblUsername.Text = main.lblUsername.Text;

        }
        public void LoadStock()
        {
            i = 0;
            StartIndex = 0;
            dgvAdjustment.Rows.Clear();
            try
            {
                cm = new SqlCommand("exec GetProductsList '" + txtSearch.Text + "', " + StartIndex.ToString() + ", " + RowCount.ToString(), cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvAdjustment.Rows.Add(i, dr[0].ToString(),
                                           dr[1].ToString(),
                                           dr[2].ToString(),
                                           dr[3].ToString(),
                                           dr[4].ToString(),
                                           dr[5].ToString(),
                                           dr[6].ToString(),
                                           dr[7].ToString(),
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

        private void dgvAdjustment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvAdjustment.Columns[e.ColumnIndex].Name;
            if(colName=="Select")
            {
                lblPcode.Text = dgvAdjustment.Rows[e.RowIndex].Cells[1].Value.ToString();
                lblDesc.Text = dgvAdjustment.Rows[e.RowIndex].Cells[2].Value.ToString() + " " + dgvAdjustment.Rows[e.RowIndex].Cells[3].Value.ToString();
                _qty = int.Parse(dgvAdjustment.Rows[e.RowIndex].Cells[8].Value.ToString());
                btnSave.Enabled = true;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadStock();
        }

        public void Clear()
        {
            lblDesc.Text = "";
            lblPcode.Text = "";
            txtQty.Clear();
            txtRemark.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                //update stock
                if(int.Parse(txtQty.Text)>_qty)
                {
                    MessageBox.Show("Не можна списати більше, ніж є в наявності.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                cm = new SqlCommand("INSERT INTO Adjustment(Adj_Product, Adj_Qty, Adj_Remarks, Adj_User) VALUES (@product, @qty, @remarks, @user)", cn);
                cm.Parameters.AddWithValue("@product", lblPcode.Text);
                cm.Parameters.AddWithValue("@qty", txtQty.Text);
                cm.Parameters.AddWithValue("@remarks", txtRemark.Text);
                cm.Parameters.AddWithValue("@user", lblUsername.Text);
                cn.Open();
                var res = cm.ExecuteNonQuery();
                if (res > 0)
                {
                    MessageBox.Show("Товар було успішно списано.", "Списання", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    btnSave.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Сталася помилка при списані.", "Списання", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                cn.Close();
                LoadStock();
            }
        }

        private void dgvAdjustment_Scroll(object sender, ScrollEventArgs e)
        {
            int displayedRows = dgvAdjustment.DisplayedRowCount(true);
            int lastVisibleRowIndex = dgvAdjustment.FirstDisplayedScrollingRowIndex + displayedRows - 1;
            if (lastVisibleRowIndex >= dgvAdjustment.RowCount - 1)
            {
                StartIndex += 25;
                cm = new SqlCommand("exec GetProductsList '" + txtSearch.Text + "', " + StartIndex.ToString() + ", " + RowCount.ToString(), cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvAdjustment.Rows.Add(i, dr[0].ToString(),
                                           dr[1].ToString(),
                                           dr[2].ToString(),
                                           dr[3].ToString(),
                                           dr[4].ToString(),
                                           dr[5].ToString(),
                                           dr[6].ToString(),
                                           dr[7].ToString(),
                                           dr[8].ToString()

                                        );
                }
                dr.Close();
                cn.Close();
            }
        }
    }
}
