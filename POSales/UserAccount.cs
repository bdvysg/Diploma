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
    public partial class UserAccount : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        MainForm main;
        public string username;
        string name;
        string role;
        string accstatus;
        public UserAccount(MainForm mn)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            main = mn;
            LoadEmployee();
            LoadUser();
        }

        public void LoadUser()
        {
            int i = 0;
            dgvUser.Rows.Clear();
            cm = new SqlCommand("SELECT Usr_Username, Emp_Name + ' ' + Emp_Surname AS Name, CASE Usr_IsActivate WHEN 1 THEN 'Так' WHEN 0 THEN 'Ні' END AS activate, Ur_Title FROM [User] INNER JOIN Employee ON Emp_Id = Usr_Employee INNER JOIN UserRole ON Usr_Role = Ur_Id", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvUser.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString());
            }
            dr.Close();
            cn.Close();
        }

        public void Clear()
        {
            //txtName.Clear();
            txtPass.Clear();
            txtRePass.Clear();
            txtUsername.Clear();
            cbRole.Text = "";
            txtUsername.Focus();
        }
        public void LoadEmployee()
        {
            cbName.Items.Clear();
            cbName.DataSource = dbcon.getTable("SELECT Emp_Id, Emp_Name + ' ' + Emp_Surname AS name FROM Employee");
            cbName.DisplayMember = "name";
            cbName.ValueMember = "Emp_id";
            cbName.SelectedIndex = -1;
        }

        private void btnAccSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPass.Text != txtRePass.Text)
                {
                    MessageBox.Show("Паролі не співпадають!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                cn.Open();
                cm = new SqlCommand("Insert into [User](Usr_Username, Usr_Password, Usr_Role, Usr_Employee) Values (@username, @password, CASE @role WHEN 'Касир' THEN 1 WHEN 'Адміністратор' THEN 2 END, ISNULL((SELECT TOP 1 Emp_Id FROM Employee WHERE Emp_Name + ' ' + Emp_Surname = @name), 1))", cn);
                cm.Parameters.AddWithValue("@username", txtUsername.Text);
                cm.Parameters.AddWithValue("@password", txtPass.Text);
                cm.Parameters.AddWithValue("@role", cbRole.Text);
                cm.Parameters.AddWithValue("@name", cbName.Text);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Новий аккаунт було успішно створено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Clear();
                LoadUser();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Помилка");
            }
        }

        private void btnAccCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnPassSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCurPass.Text != main._pass )
                {
                    MessageBox.Show("Неправильний поточний пароль!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if(txtNPass.Text != txtRePass2.Text)
                {
                    MessageBox.Show("Підтвердження паролю та пароль не співпадають!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                dbcon.ExecuteQuery("UPDATE [User] SET Usr_Password = '" + txtNPass.Text + "' WHERE Usr_Username = '" + lblUsername.Text + "'");
                MessageBox.Show("Пароль було успішно змінено!", "Зміна паролю", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка");
            }
        }

        private void UserAccount_Load(object sender, EventArgs e)
        {
            lblUsername.Text = main.lblUsername.Text;
        }

        private void btnPassCancel_Click(object sender, EventArgs e)
        {
            ClearCP();
        }

        public void ClearCP()
        {
            txtCurPass.Clear();
            txtNPass.Clear();
            txtRePass2.Clear();
        }

        private void dgvUser_SelectionChanged(object sender, EventArgs e)
        {
            int i = dgvUser.CurrentRow.Index;
            username = dgvUser[1, i].Value.ToString();
            name = dgvUser[2, i].Value.ToString(); 
            role = dgvUser[4, i].Value.ToString();
            accstatus = dgvUser[3, i].Value.ToString();
            if (lblUsername.Text == username)
            {
                btnRemove.Enabled = false;
                btnResetPass.Enabled = false;
                lblAccNote.Text = "Щоб змінити пароль перейдіть до вкладти зміни паролю.";

            }
            else
            {
                btnRemove.Enabled = true;
                btnResetPass.Enabled = true;
                lblAccNote.Text = "Щоб змінити пароль для " + username + ", натисніть змінити пароль.";
            }
            gbUser.Text = "Пароль для " + username;
            
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show("Ви впевнені що хочете видалити данний акаунт", "Видалення акаунту", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes))
            {
                dbcon.ExecuteQuery("DELETE FROM [User] WHERE Usr_Username = '" + username + "'");
                MessageBox.Show("Акаунт було успішно видалено!");
                LoadUser();
            }
        }

        private void btnResetPass_Click(object sender, EventArgs e)
        {
            ResetPassword reset = new ResetPassword(this);
            reset.ShowDialog();
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            UserProperties properties = new UserProperties(this);
            properties.Text = name +"\\"+ username +" Налаштування";
            properties.cbRole.Text = role;
            properties.cbActivate.Text = accstatus;
            properties.username = username;
            properties.ShowDialog();
        }
    }
}
