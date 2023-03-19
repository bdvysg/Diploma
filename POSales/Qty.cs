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
    public partial class Qty : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string stitle = "Point Of Sales";
        private string pcode;
        private double price;
        private String transno;
        private int qty;
        Cashier cashier;

        public Qty(Cashier cash)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            cashier = cash;

        }

        public void ProductDetails(string pcode, double price, string transno, int qty)
        {
            this.pcode = pcode;
            this.price = price;
            this.transno = transno;
            this.qty = qty;
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == 13) && (txtQty.Text != string.Empty))
            {
                try
                {
                    string id = "";
                    int cart_qty = 0;
                    bool found = false;
                    cn.Open();
                    cm = new SqlCommand("Select * from Cart Where Crt_Transno = @transno and Crt_Product = @pcode", cn);
                    cm.Parameters.AddWithValue("@transno", transno);
                    cm.Parameters.AddWithValue("@pcode", pcode);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        id = dr["Crt_Id"].ToString();
                        cart_qty = int.Parse(dr["Crt_Qty"].ToString());
                        found = true;
                    }
                    else 
                    { 
                        found = false; 
                    }
                    dr.Close();
                    cn.Close();

                    if (found)
                    {
                        if (qty < (int.Parse(txtQty.Text) + cart_qty))
                        {
                            MessageBox.Show("Помилка. Неможливо зарахувати товару більше ніж є в наявності - " + qty, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        cn.Open();
                        cm = new SqlCommand("Update Cart set Crt_Qty = Crt_Qty + " + int.Parse(txtQty.Text) + " Where Crt_Id= " + id, cn);
                        cm.ExecuteReader();
                        cn.Close();
                        cashier.txtBarcode.Clear();
                        cashier.txtBarcode.Focus();
                        cashier.LoadCart();
                        this.Dispose();
                    }
                    else
                    {
                        if (qty < (int.Parse(txtQty.Text) + cart_qty))
                        {
                            MessageBox.Show("Помилка. Неможливо зарахувати товару більше ніж є в наявності - " + qty, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        cn.Open();
                        cm = new SqlCommand("INSERT INTO Cart(Crt_Transno, Crt_Product, Crt_Price, Crt_Qty, Crt_Cashier) VALUES(@transno, @pcode, @price, @qty, (SELECT TOP 1 Emp_Id FROM Employee INNER JOIN [User] ON Emp_Id = Usr_Employee WHERE Usr_Username = '" + cashier.lblUsername.Text + "'))", cn);
                        cm.Parameters.AddWithValue("@transno", transno);
                        cm.Parameters.AddWithValue("@pcode", pcode);
                        cm.Parameters.AddWithValue("@price", price);
                        cm.Parameters.AddWithValue("@qty", int.Parse(txtQty.Text));
                        cm.ExecuteNonQuery();
                        cn.Close();
                        cashier.txtBarcode.Clear();
                        cashier.txtBarcode.Focus();
                        cashier.LoadCart();
                        this.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, stitle);
                }
            }
        }

        private void Qty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
