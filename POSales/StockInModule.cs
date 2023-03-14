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
    public partial class StockInModule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string _id;

        public StockInModule()
        {
            InitializeComponent();
            LoadSupplier();
            cn = new SqlConnection(dbcon.myConnection());
            btnUpdate.Enabled = true;
        }

        public void LoadSupplier()
        {
            cbSuppliers.Items.Clear();
            cbSuppliers.DataSource = dbcon.getTable("SELECT * FROM Supplier");
            cbSuppliers.DisplayMember = "Sup_Name";
            cbSuppliers.SelectedIndex = -1;
        }

        public void LoadDoc(string id)
        {
            dgvStockInProducts.Rows.Clear();
            int i = 0;
            _id = id;
            try
            {
                dgvStockInProducts.Rows.Clear();
                cm = new SqlCommand("exec GetStockInInfo " + id, cn);
                cn.Open();
                dr = cm.ExecuteReader();
                if (dr.HasRows) 
                {
                    while (dr.Read())
                    {
                        if (dr[6].ToString() != "" && dr[7].ToString() != "" && dr[8].ToString() != "")
                        {
                            i++;
                            dgvStockInProducts.Rows.Add(i, dr[6].ToString(),
                                                           dr[7].ToString(),
                                                           dr[8].ToString(),
                                                           dr[9].ToString()
                                                        );
                        }
                        cbSuppliers.Text = dr[3].ToString();
                        textBox1.Text = dr[0].ToString();
                        textBox2.Text = dr[1].ToString();
                        if (dr[2].ToString() != "")
                        {
                            dateTimePicker1.Value = DateTime.Parse(dr[2].ToString());
                        }
                        if (dr[5].ToString() == "Підтверджено")
                        {
                            btnAdd.Enabled = false;
                            btnConfirm.Enabled = false;
                            btnUpdate.Enabled = false;
                            updQuantity.Enabled = false;
                            btnSave.Enabled = false;
                        }
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

        private void dgvStockInProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ProductModule product = new ProductModule();
            product.LoadProduct(dgvStockInProducts.Rows[e.RowIndex].Cells[1].Value.ToString());
            product.txtPcode.Enabled = false;
            product.btnSave.Enabled = false;
            product.btnUpdate.Enabled = true;
            product.label1.Text = "Інформація про товар";
            product.ShowDialog();
        }

        private void dgvStockInProducts_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            textBox3.Text = dgvStockInProducts.Rows[e.RowIndex].Cells[3].Value.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProductStockIn product = new ProductStockIn(this);
            product.Show();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                cm = new SqlCommand("UPDATE StockIn SET Sti_Date = @date, Sti_StockInBy = @stockinby, Sti_SupplierId = (SELECT TOP 1 Sup_Id FROM Supplier WHERE Sup_Name LIKE @supplier) WHERE Sti_Id = @id", cn);
                cm.Parameters.AddWithValue("@id", textBox1.Text);
                cm.Parameters.AddWithValue("@date", dateTimePicker1.Value);
                cm.Parameters.AddWithValue("@stockinby", textBox2.Text);
                cm.Parameters.AddWithValue("@supplier", cbSuppliers.Text);
                cn.Open();
                var res = cm.ExecuteNonQuery();
                if (res > 0)
                {
                    MessageBox.Show("Замовлення було успішно оновлено!");
                }
                else
                {
                    MessageBox.Show("Жоден запис не було оновлено!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                cm = new SqlCommand("UPDATE StockIn SET Sti_Date = @date, Sti_StockInBy = @stockinby, Sti_SupplierId = (SELECT TOP 1 Sup_Id FROM Supplier WHERE Sup_Name LIKE @supplier) WHERE Sti_Id = @id", cn);
                cm.Parameters.AddWithValue("@id", textBox1.Text);
                cm.Parameters.AddWithValue("@date", dateTimePicker1.Value);
                cm.Parameters.AddWithValue("@stockinby", textBox2.Text);
                cm.Parameters.AddWithValue("@supplier", cbSuppliers.Text);
                cn.Open();
                var res = cm.ExecuteNonQuery();
                if (res > 0)
                {
                    MessageBox.Show("Замовлення було успішно оновлено!");
                }
                else
                {
                    MessageBox.Show("Помилка при оновлені запису!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cn.Close();
            }

        }

        private void dgvStockInProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvStockInProducts.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                if (MessageBox.Show("Видалити обраний товар?", "Видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        cm = new SqlCommand("DELETE FROM StockInProduct WHERE Sip_Doc = @id AND Sip_Product = @product", cn);
                        cm.Parameters.AddWithValue("@id", textBox1.Text);
                        cm.Parameters.AddWithValue("@product", dgvStockInProducts.Rows[e.RowIndex].Cells[1].Value.ToString());
                        cn.Open();
                        var res = cm.ExecuteNonQuery();
                        if (res > 0)
                        {
                            MessageBox.Show("Товар було успішно видалено!");
                        }
                        else
                        {
                            MessageBox.Show("Помилка при видалені товару!");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        cn.Close();
                        LoadDoc(textBox1.Text);
                    }
                }
            }
        }

        private void updQuantity_Click(object sender, EventArgs e)
        {
            try
            {
                cm = new SqlCommand("UPDATE StockInProduct SET Sip_Quantity = @qty WHERE Sip_Doc = @doc AND Sip_Product = @product", cn);
                cm.Parameters.AddWithValue("@doc", textBox1.Text);
                cm.Parameters.AddWithValue("@qty", textBox3.Text);
                cm.Parameters.AddWithValue("@product", dgvStockInProducts.CurrentRow.Cells[1].Value.ToString());
                cn.Open();
                cm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cn.Close();
                LoadDoc(textBox1.Text);
            }

        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                cm = new SqlCommand("UPDATE StockIn SET Sti_IsConfirmed = CASE Sti_IsConfirmed WHEN 0 THEN 1 WHEN 1 THEN 0 END WHERE Sti_id = @doc", cn);
                cm.Parameters.AddWithValue("@doc", textBox1.Text);
                cn.Open();
                cm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cn.Close();
                LoadDoc(textBox1.Text);
            }

        }

        private void btnPrintStockIn_Click(object sender, EventArgs e)
        {
            cm = new SqlCommand("exec GetStockInInfo " + _id, cn);
            cn.Open();
            dr = cm.ExecuteReader();
            Report report = new Report();
            report.GenerateStockInDoc(dr);
        }
    }
}
