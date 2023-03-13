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
    public partial class ChangePassword : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        Cashier cashier;
        public ChangePassword(Cashier cash)
        {
            InitializeComponent();
            cashier = cash;
            lblUsername.Text = cashier.lblUsername.Text;
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                string oldpass = dbcon.getPassword(lblUsername.Text);
                if(oldpass != txtPass.Text)
                {
                    MessageBox.Show("Невірний пароль, спробуйте знову!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    txtPass.Visible = false;
                    btnNext.Visible = false;

                    txtNewPass.Visible = true;
                    txtComPass.Visible = true;
                    btnSave.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtNewPass.Text != txtComPass.Text)
                {
                    MessageBox.Show("Паролі не співпадають!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if(MessageBox.Show("Змінити пароль?","Підтвердити",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                    {
                        dbcon.ExecuteQuery("UPDATE [User] set Usr_Password = '" + txtNewPass.Text + "' WHERE Usr_Username = '" + lblUsername.Text + "'");
                        MessageBox.Show("Пароль було успішно оновлено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка");
            }
        }

        private void ChangePassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }

        }
    }
}
