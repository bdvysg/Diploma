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
    public partial class Login : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;

        public string _pass = "";
        public bool _isactivate;
        public Login()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            txtName.Focus();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вийти з програми?", "Вийти", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string _username = "", _name = "", _role = "";
            try
            {
                bool found;
                cn.Open();
                cm = new SqlCommand("exec GetUserInfo @username, @password", cn);
                cm.Parameters.AddWithValue("@username", txtName.Text);
                cm.Parameters.AddWithValue("@password", txtPass.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if(dr.HasRows)
                {
                    found = true;
                    _username = dr["Usr_Username"].ToString();
                    _name = dr["Usr_Name"].ToString();
                    _role = dr["Ur_Title"].ToString();
                    _pass = dr["Usr_Password"].ToString();
                    _isactivate = bool.Parse(dr["Usr_Isactivate"].ToString());

                }
                else
                { 
                    found = false; 
                }
                dr.Close();
                cn.Close();

                if(found)
                {
                    if(!_isactivate)
                    {
                        MessageBox.Show("Аккаунт деактивовано адміністратором. Неможливо ввійти", "Аккаунт деактивовано", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if(_role=="Касир")
                    {
                        txtName.Clear();
                        txtPass.Clear();
                        Cashier cashier = new Cashier();
                        cashier.lblUsername.Text = _username;
                        cashier.lblname.Text = _name + " | " + _role;
                        cashier.Show();
                        this.Hide();
                    }
                    else
                    {
                        txtName.Clear();
                        txtPass.Clear();
                        
                        MainForm main = new MainForm();
                        main.lblUsername.Text = _name;
                        main.lblName.Text = _name;
                        main._pass = _pass;
                        main.Show();
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Неправильне ім'я користувача або пароль", "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вийти з програми?", "Вийти", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void txtPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==13)
            {
                btnLogin.PerformClick();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Report report = new Report();
            report.TopSellingProduct();
        }
    }
}
