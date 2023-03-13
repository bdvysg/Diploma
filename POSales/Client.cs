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
    public partial class Client : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        public Client()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "" || txtCard.Text != "")
            {
                try
                {
                    cm = new SqlCommand("INSERT INTO Client (Cl_Name, Cl_CardNumber) VALUES(@name, @card)", cn);
                    cm.Parameters.AddWithValue("@name", txtName.Text);
                    cm.Parameters.AddWithValue("@card", txtCard.Text);
                    cn.Open();
                    var res = cm.ExecuteNonQuery();
                    if (res > 0)
                    {
                        MessageBox.Show("Клієнта було успішно зареєстровано!");
                        this.Close();
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
            else
            {
                MessageBox.Show("Введіть коректні дані!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
