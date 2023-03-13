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
    public partial class UserProperties : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();

        UserAccount account;
        public string username;
        public UserProperties(UserAccount user)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            account = user;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

  
        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                if ((MessageBox.Show("Ви хочете оновити дані про акаунт?", "Оновлення", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes))
                 {
                    cn.Open();
                    cm = new SqlCommand("UPDATE [User] SET Usr_Role = CASE @role WHEN 'Адміністратор' THEN 2 WHEN 'Касир' THEN 1 END, Usr_IsActivate = CASE @isactivate WHEN 'Так' THEN 1 WHEN 'Ні' THEN 0 END WHERE Usr_Username='" + username + "'",cn);
                    cm.Parameters.AddWithValue("@role", cbRole.Text);
                    cm.Parameters.AddWithValue("@isactivate", cbActivate.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Дані аккаунта були успішно змінені!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    account.LoadUser();
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void UserProperties_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
