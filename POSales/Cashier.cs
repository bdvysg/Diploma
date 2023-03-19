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
    public partial class Cashier : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;

        int qty;
        string id;
        string price;
            
        string stitle = "Market";
        public Cashier()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            GetTranNo();
            lblDate.Text = DateTime.Now.ToShortDateString();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вийти з програми?", "Вихід", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        public void slide(Button button)
        {
            panelSlide.BackColor = Color.White;
            panelSlide.Height = button.Height;
            panelSlide.Top = button.Top;
        }
        #region button
        private void btnNTran_Click(object sender, EventArgs e)
        {
            slide(btnNTran);
            GetTranNo();

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            slide(btnSearch);
            LookUpProduct lookUp = new LookUpProduct(this);
            lookUp.LoadProduct();
            lookUp.ShowDialog();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            slide(btnDiscount);
            Discount discount = new Discount(this);
            discount.ShowDialog();            
        }

        private void btnSettle_Click(object sender, EventArgs e)
        {
            slide(btnSettle);
            Settle settle = new Settle(this);
            settle.txtSale.Text = lblDisplayTotal.Text;
            settle.ShowDialog();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            slide(btnClear);
            if (MessageBox.Show("Видалити всі товари с кошика?", "Видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                cn.Open();
                cm = new SqlCommand("Delete from Cart where Crt_Transno like'" + lblTranNo.Text + "'", cn);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Усі товари було успішно видаленно!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCart();
            }
        }

        private void btnDSales_Click(object sender, EventArgs e)
        {
            slide(btnDSales);
            DailySale dailySale = new DailySale(new MainForm());
            dailySale.solduser = lblUsername.Text;
            dailySale.dtFrom.Enabled = false;
            dailySale.dtTo.Enabled = false;
            dailySale.cboCashier.Enabled = false;
            dailySale.cboCashier.Text = lblUsername.Text;
            dailySale.picClose.Visible = true;
            dailySale.lblTitle.Visible = true;
            dailySale.ShowDialog();
        }

        private void btnPass_Click(object sender, EventArgs e)
        {
            slide(btnPass);
            ChangePassword change = new ChangePassword(this);
            change.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            slide(btnLogout);
            if(dgvCash.Rows.Count > 0)
            {
                MessageBox.Show("Неможливо вийти з програми. У вас не закритий кошик", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Вийти с приложення?", "Вихід", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                Login login = new Login();
                login.ShowDialog();
            }
        }
        #endregion button

        public void LoadCart()
        {
            try
            {
                bool hascart = false;
                int i = 0;
                double total = 0;
                double discount = 0;
                dgvCash.Rows.Clear();
                cn.Open();
                cm = new SqlCommand("SELECT c.Crt_Id, c.Crt_Product, p.Pr_Title, c.Crt_Price, ISNULL(c.Crt_Qty, 0), ISNULL(c.Crt_Discount, 0), ISNULL(c.Crt_Total, 0) FROM Cart AS c INNER JOIN Product AS p ON c.Crt_Product = p.Pr_id WHERE c.Crt_Transno LIKE @transno and c.Crt_Status = 1", cn);
                cm.Parameters.AddWithValue("@transno", lblTranNo.Text);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {

                    i++;
                    total += Convert.ToDouble(dr[6].ToString());
                    discount += Convert.ToDouble(dr[5].ToString()) * Convert.ToDouble(dr[4].ToString());
                    dgvCash.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), double.Parse(dr[6].ToString()).ToString("#,##0.00"));
                    hascart = true;
                }
                dr.Close();
                cn.Close();
                lblSaleTotal.Text = total.ToString("#,##0.00");
                lblDiscount.Text = discount.ToString("#,##0.00");
                GetCartTotal();
                if (hascart) { btnClear.Enabled = true; btnSettle.Enabled = true; btnDiscount.Enabled = true; }
                else { btnClear.Enabled = false; btnSettle.Enabled = false; btnDiscount.Enabled = false; }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, stitle);
            }
            finally
            {
                cn.Close();
                dr.Close();
            }
        
        }

        public void GetCartTotal()
        {
            double discount = double.Parse(lblDiscount.Text);
            double sales = double.Parse(lblSaleTotal.Text);
            double vat = sales * 0.12;
            double vatable = sales - vat;

            lblVat.Text = vat.ToString("#,##0.00");
            lblVatable.Text = vatable.ToString("#,##0.00");
            lblDisplayTotal.Text = sales.ToString("#,##0.00");
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimer.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        public void GetTranNo()
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                int count;
                string transno;
                cn.Open();
                cm = new SqlCommand("SELECT TOP 1 Crt_Transno FROM Cart WHERE Crt_Transno LIKE '" + sdate + "%' ORDER BY Crt_Id desc", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    transno = dr[0].ToString();
                    count = int.Parse(transno.Substring(8, 4));
                    lblTranNo.Text = sdate + (count + 1);
                }
                else
                {
                    transno = sdate + "1001";
                    lblTranNo.Text = transno;
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {

                cn.Close();
                MessageBox.Show(ex.Message, stitle);

            }
            
        }

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtBarcode.Text == string.Empty) return;
                else
                {
                    string _pcode;
                    double _price;
                    int _qty;
                    cn.Open();
                    cm = new SqlCommand("SELECT * FROM Product INNER  WHERE Pr_Barcode LIKE '" + txtBarcode.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if(dr.HasRows)
                    {
                        qty = int.Parse(dr["qty"].ToString());
                        _pcode = dr["pcode"].ToString();
                        _price = double.Parse(dr["price"].ToString());
                        _qty = int.Parse(txtQty.Text);
                       
                        dr.Close();
                        cn.Close();
                        //insert to tbCart
                        AddToCart(_pcode, _price, _qty);
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void AddToCart(string _pcode, double _price,int _qty)
        {
            try
            {
                string id = "";
                int cart_qty = 0;
                bool found = false;
                cn.Open();
                cm = new SqlCommand("Select * from Cart Where Crt_Transno = @transno and Crt_Product = @pcode", cn);
                cm.Parameters.AddWithValue("@transno", lblTranNo.Text);
                cm.Parameters.AddWithValue("@pcode", _pcode);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    id = dr["Crt_Id"].ToString();
                    cart_qty = int.Parse(dr["Crt_Qty"].ToString());
                    found = true;
                }
                else found = false;
                dr.Close();
                cn.Close();

                if (found)
                {
                    if (qty < (int.Parse(txtQty.Text) + cart_qty))
                    {
                        MessageBox.Show("Unable to procced. Remaining quantity on hand is " + qty, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    cn.Open();
                    cm = new SqlCommand("Update Cart set Crt_Qty = (Crt_Qty + " + _qty + ")Where Crt_Id= '" + id + "'", cn);
                    cm.ExecuteReader();
                    cn.Close();
                    txtBarcode.SelectionStart = 0;
                    txtBarcode.SelectionLength = txtBarcode.Text.Length;
                    LoadCart();                    
                }
                else
                {
                    if (qty < (int.Parse(txtQty.Text) + cart_qty))
                    {
                        MessageBox.Show("Unable to procced. Remaining qty on hand is" + qty, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    cn.Open();
                    cm = new SqlCommand("INSERT INTO tbCart(transno, pcode, price, qty, sdate, cashier)VALUES(@transno, @pcode, @price, @qty, @sdate, @cashier)", cn);
                    cm.Parameters.AddWithValue("@transno", lblTranNo.Text);
                    cm.Parameters.AddWithValue("@pcode", _pcode);
                    cm.Parameters.AddWithValue("@price", _price);
                    cm.Parameters.AddWithValue("@qty", _qty);
                    cm.Parameters.AddWithValue("@sdate", DateTime.Now);
                    cm.Parameters.AddWithValue("@cashier", lblUsername.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    LoadCart();
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message, stitle);
            }
        }

        private void dgvCash_SelectionChanged(object sender, EventArgs e)
        {
            int i = dgvCash.CurrentRow.Index;
            id = dgvCash[1, i].Value.ToString();
            price = dgvCash[7, i].Value.ToString();
        }

        private void dgvCash_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCash.Columns[e.ColumnIndex].Name;
           

            if (colName == "Delete")
            {
                if (MessageBox.Show("Видалити даний товар?", "Видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dbcon.ExecuteQuery("Delete from Cart where Crt_Id  = " + dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString());
                    MessageBox.Show("Товар було успішно видаленно.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCart();
                }
            }
            else if (colName == "colAdd")
            {
                int i = 0;
                cn.Open();
                cm = new SqlCommand("SELECT OnStk_Quantity FROM OnStock WHERE OnStk_Product = " + dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString(), cn);
                i = int.Parse(cm.ExecuteScalar().ToString());
                cn.Close();
                if (int.Parse(dgvCash.Rows[e.RowIndex].Cells[5].Value.ToString()) < i)
                {
                    dbcon.ExecuteQuery("UPDATE Cart SET Crt_Qty = Crt_Qty + " + int.Parse(txtQty.Text) + " WHERE Crt_Transno LIKE '" + lblTranNo.Text + "'  AND Crt_Product = " + dgvCash.Rows[e.RowIndex].Cells[2].Value.ToString());
                    LoadCart();
                }
                else
                {
                    MessageBox.Show("В наявності лише " + i + " даного товару!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if (colName == "colReduce")
            {
                int i = 0;
                cn.Open();
                cm = new SqlCommand("SELECT OnStk_Quantity FROM OnStock WHERE OnStk_Product = " + dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString(), cn);
                i = int.Parse(cm.ExecuteScalar().ToString());
                cn.Close();
                if (int.Parse(dgvCash.Rows[e.RowIndex].Cells[5].Value.ToString()) < i)
                {
                    dbcon.ExecuteQuery("UPDATE Cart SET Crt_Qty = Crt_Qty - " + int.Parse(txtQty.Text) + " WHERE Crt_Transno LIKE '" + lblTranNo.Text + "'  AND Crt_Product = " + dgvCash.Rows[e.RowIndex].Cells[2].Value.ToString());
                    LoadCart();
                }
                else
                {
                    MessageBox.Show("В наявності лише " + i + " даного товару!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        public void Noti()
        {
            int i = 0;
            cn.Open();
            cm = new SqlCommand("SELECT * FROM vwCriticalItems", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                Alert alert = new Alert(new MainForm());
                alert.lblPcode.Text = dr["pcode"].ToString();              
                alert.showAlert(i + ". " + dr["pdesc"].ToString() + " - " + dr["qty"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void Cashier_Load(object sender, EventArgs e)
        {
            //Noti();
        }

        private void Cashier_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
