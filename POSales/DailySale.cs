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
    public partial class DailySale : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        public string solduser;
        MainForm main;
        public DailySale(MainForm mn)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            main = mn;
            LoadCashier();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadCashier()
        {
            cboCashier.Items.Clear();
            cboCashier.Items.Add("Всі касири");
            cn.Open();
            cm = new SqlCommand("SELECT Emp_Name + ' ' + Emp_Surname AS username FROM [User] INNER JOIN Employee ON Emp_Id = Usr_Employee WHERE Usr_Role = 1", cn);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                cboCashier.Items.Add(dr["username"].ToString());
            }
            dr.Close();
            cn.Close();
        }
        public void LoadSold()
        {
            int i = 0;
            double total = 0;
            dgvSold.Rows.Clear();
            cn.Open();
            if(cboCashier.Text=="Всі касири")
            {
                cm = new SqlCommand("select c.Crt_Id, c.Crt_Transno, c.Crt_Product, p.Pr_Title, c.Crt_Price, c.Crt_Qty, c.Crt_Discount, c.Crt_Total from Cart as c inner join Product as p on c.Crt_Product = p.Pr_Id where status = 2 and Crt_Date between '" + dtFrom.Value + "' and '" + dtTo.Value + "'", cn);
            }
            else
            {
                cm = new SqlCommand("select c.Crt_Id, c.Crt_Transno, c.Crt_Product, p.Pr_Title, c.Crt_Price, c.Crt_Qty, c.Crt_Discount, c.Crt_Total, Emp_Name + ' ' + Emp_Surname as Cashier from Cart as c inner join Employee ON Emp_Id = Crt_Employee inner join Product as p on c.Crt_Product = p.Pr_Id where status = 2 and Crt_Date between '" + dtFrom.Value + "' and '" + dtTo.Value + "'" + " and Cashier like '" + cboCashier.Text + "'", cn);
            }
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                total += double.Parse(dr[7].ToString());
                dgvSold.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
            }
            dr.Close();
            cn.Close();
            lblTotal.Text = total.ToString("#,##0.00");
        }

        private void cboCashier_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSold();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadSold();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            LoadSold();
        }

        private void DailySale_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape)
            {
                this.Dispose();
            }
        }

        private void dgvSold_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvSold.Columns[e.ColumnIndex].Name;
            if(colName=="Cancel")
            {
                CancelOrder cancelOrder = new CancelOrder(this);
                cancelOrder.txtId.Text = dgvSold.Rows[e.RowIndex].Cells[1].Value.ToString();
                cancelOrder.txtTransno.Text = dgvSold.Rows[e.RowIndex].Cells[2].Value.ToString();
                cancelOrder.txtPcode.Text = dgvSold.Rows[e.RowIndex].Cells[3].Value.ToString();
                cancelOrder.txtDesc.Text = dgvSold.Rows[e.RowIndex].Cells[4].Value.ToString();
                cancelOrder.txtPrice.Text = dgvSold.Rows[e.RowIndex].Cells[5].Value.ToString();
                cancelOrder.txtQty.Text = dgvSold.Rows[e.RowIndex].Cells[6].Value.ToString();
                cancelOrder.txtDisc.Text = dgvSold.Rows[e.RowIndex].Cells[7].Value.ToString();
                cancelOrder.txtTotal.Text = dgvSold.Rows[e.RowIndex].Cells[8].Value.ToString();
                cancelOrder.txtCancelBy.Text = solduser;
                cancelOrder.ShowDialog();

            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            POSReport report = new POSReport();
            string param = "Date From: " + dtFrom.Value.ToShortDateString() + " To: " + dtTo.Value.ToShortDateString();
               
            if (cboCashier.Text == "All Cashier")
            {
                report.LoadDailyReport("select c.id, c.transno, c.pcode, p.pdesc, c.price, c.qty, c.disc as discount, c.total from tbCart as c inner join tbProduct as p on c.pcode = p.pcode where status like 'Sold' and sdate between '" + dtFrom.Value + "' and '" + dtTo.Value + "'", param, cboCashier.Text);
            }
            else
            {
                report.LoadDailyReport("select c.id, c.transno, c.pcode, p.pdesc, c.price, c.qty, c.disc as discount, c.total from tbCart as c inner join tbProduct as p on c.pcode = p.pcode where status like 'Sold' and sdate between '" + dtFrom.Value + "' and '" + dtTo.Value + "' and cashier like '" + cboCashier.Text + "'", param, cboCashier.Text);
            }
            report.ShowDialog();
        }

        private void dgvSold_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvSold.Columns[e.ColumnIndex].Name;
            if(colName=="Cancel")
            {
                CancelOrder cancel = new CancelOrder(this);
                cancel.txtId.Text = dgvSold.Rows[e.RowIndex].Cells[1].Value.ToString();
                cancel.txtTransno.Text = dgvSold.Rows[e.RowIndex].Cells[2].Value.ToString();
                cancel.txtPcode.Text = dgvSold.Rows[e.RowIndex].Cells[3].Value.ToString();
                cancel.txtDesc.Text = dgvSold.Rows[e.RowIndex].Cells[4].Value.ToString();
                cancel.txtPrice.Text = dgvSold.Rows[e.RowIndex].Cells[5].Value.ToString();
                cancel.txtQty.Text = dgvSold.Rows[e.RowIndex].Cells[6].Value.ToString();
                cancel.txtDisc.Text = dgvSold.Rows[e.RowIndex].Cells[7].Value.ToString();
                cancel.txtTotal.Text = dgvSold.Rows[e.RowIndex].Cells[8].Value.ToString();
                if(lblTitle.Visible==false)
                    cancel.txtCancelBy.Text = main.lblUsername.Text;
                else
                    cancel.txtCancelBy.Text = solduser;
                cancel.ShowDialog();
            }
        }
    }
}
