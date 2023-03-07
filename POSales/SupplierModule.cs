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
    public partial class SupplierModule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        string stitle = "Market";
        Supplier supplier;
        public SupplierModule(Supplier sp)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            supplier = sp;
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void Clear()
        {
            txtAddress.Clear();
            txtConPerson.Clear();
            txtEmail.Clear();
            txtFaxNo.Clear();
            txtPhone.Clear();
            txtSupplier.Clear();

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            txtSupplier.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Зберегти дані про постачальника?.", "Зберегти", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("Insert into Supplier (Sup_Name, Sup_Address, Sup_ContactPerson, Sup_Phone, Sup_Email, Sup_Fax) values (@supplier, @address, @contactperson, @phone, @email, @fax) ", cn);
                    cm.Parameters.AddWithValue("@supplier", txtSupplier.Text);
                    cm.Parameters.AddWithValue("@address", txtAddress.Text);
                    cm.Parameters.AddWithValue("@contactperson", txtConPerson.Text);
                    cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cm.Parameters.AddWithValue("@email", txtEmail.Text);
                    cm.Parameters.AddWithValue("@fax", txtFaxNo.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Дані були успішно збережені!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    supplier.LoadSupplier();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, stitle);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Оновити дані про постачальника.", "Оновити", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("Update Supplier set Sup_Name=@supplier, Sup_Address=@address, Sup_ContactPerson=@contactperson, Sup_Phone=@phone, Sup_Email=@email, Sup_Fax=@fax WHERE Sup_Id=@id ", cn);
                    cm.Parameters.AddWithValue("@id", lblId.Text);
                    cm.Parameters.AddWithValue("@supplier", txtSupplier.Text);
                    cm.Parameters.AddWithValue("@address", txtAddress.Text);
                    cm.Parameters.AddWithValue("@contactperson", txtConPerson.Text);
                    cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cm.Parameters.AddWithValue("@email", txtEmail.Text);
                    cm.Parameters.AddWithValue("@fax", txtFaxNo.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Дані були успішно оновлені!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Помилка!");
            }
        }

        private void SupplierModule_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
