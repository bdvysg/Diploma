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
    public partial class Category : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        public Category()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            LoadCategory();
        }
        public void LoadCategory()
        {
            int i = 0;
            dgvCategory.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT * FROM Category ORDER BY Catg_Title", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvCategory.Rows.Add(i, dr["Catg_Id"].ToString(), dr["Catg_Title"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CategoryModule module = new CategoryModule(this);
            module.ShowDialog();
        }

        private void dgvCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCategory.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                if (MessageBox.Show("Ви хочете видалити запис?", "Видалити запис", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM Category WHERE Catg_Id = " + dgvCategory[1, e.RowIndex].Value.ToString(), cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Запис було успішно видалено.", "Market", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            else if (colName == "Edit")
            {
                CategoryModule module = new CategoryModule(this);
                module.lblId.Text = dgvCategory[1, e.RowIndex].Value.ToString();
                module.txtCategory.Text = dgvCategory[2, e.RowIndex].Value.ToString();
                module.btnSave.Enabled = false;
                module.btnUpdate.Enabled = true;
                module.Text = "Редагування категорії";
                module.ShowDialog();
            }
            LoadCategory();
        }
    }
}
