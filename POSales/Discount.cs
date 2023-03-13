using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSales
{
    public partial class Discount : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();        
        string stitle = "Market";
        Cashier cashier;
        public Discount(Cashier cash)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            cashier = cash;            
            this.KeyPreview = true;
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Discount_KeyDown(object sender, KeyEventArgs e)
        {
     
            if (e.KeyCode == Keys.Escape) this.Dispose();
            else if (e.KeyCode == Keys.Enter) btnSave.PerformClick();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Додати знижку?", stitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("SELECT * FROM Client WHERE Cl_CardNumber = @card", cn);   
                    cm.Parameters.AddWithValue("@card", txtCardNum.Text.Trim());
                    var res = cm.ExecuteReader();
                    if (res.HasRows)
                    {
                        cn.Close();
                        cn.Open();
                        cm = new SqlCommand("UPDATE Cart SET Crt_Disc_percent = 5 WHERE Crt_Transno = @transno", cn);
                        cm.Parameters.AddWithValue("@transno", cashier.lblTranNo.Text);
                        cm.ExecuteNonQuery();
                        cashier.LoadCart();
                    }
                    else
                    {
                        MessageBox.Show("Не знайдено клієнта с даним номером картки!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, stitle);
            }
            finally
            {
                cn.Close();
                this.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Client client = new Client();
            client.Show();
        }
    }
}
