using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSales
{
    public partial class ProductModule : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string stitle = "Market";
        Product product;
        public ProductModule()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
           //product = pd;
            LoadBrand();
            LoadCategory();
        }

        public void LoadCategory()
        {
            cboCategory.Items.Clear();
            cboCategory.DataSource = dbcon.getTable("SELECT * FROM Category");
            cboCategory.DisplayMember = "Catg_Title";
            cboCategory.ValueMember = "Catg_Id";
        }

        public void LoadBrand()
        {
            cboBrand.Items.Clear();
            cboBrand.DataSource = dbcon.getTable("SELECT * FROM Brand");
            cboBrand.DisplayMember = "Br_Title";
            cboBrand.ValueMember = "Br_Id";
        }

        public void LoadProduct(string id)
        {
            try
            {
                cm = new SqlCommand("SELECT * FROM Product WHERE Pr_Id = '" + id + "'", cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    txtPcode.Text = dr[0].ToString();
                    txtBarcode.Text = dr[1].ToString();
                    txtPrName.Text = dr[10].ToString();
                    txtPriceOpt.Text = dr[6].ToString();
                    txtPriceRozn.Text = dr[5].ToString();
                    txtQuantity.Text = dr[7].ToString();
                    UDReOrder.Value = int.Parse(dr[8].ToString());
                    //imgProduct.Image = Image.FromFile("C:/base/stud/actual/Supermarket CRM/SQL/Insert/images/" + dr[12].ToString());
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            finally 
            { 
                cn.Close();
                dr.Close();
            }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void Clear()
        {
            txtPcode.Clear();
            txtBarcode.Clear();
            txtPrName.Clear();
            txtPriceOpt.Clear();
            cboBrand.SelectedIndex = 0;
            cboCategory.SelectedIndex = 0;
            UDReOrder.Value = 1;

            //txtPcode.Enabled = true;
            //txtPcode.Focus();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Ви хочете додати новий продукт?", "Додати", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO Product(Pr_Barcode, Pr_Title, Pr_Brand, Pr_Category, Pr_Price, Pr_PriceOpt, Pr_Reorder)VALUES (@Pr_Barcode,@Pr_Title,@Pr_Brand,@Pr_Category,@Pr_Price, @Pr_PriceOpt, @Pr_Reorder)", cn);
                    cm.Parameters.AddWithValue("@Pr_Barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@Pr_Title", txtPrName.Text);
                    cm.Parameters.AddWithValue("@Pr_Brand", cboBrand.SelectedValue);
                    cm.Parameters.AddWithValue("@Pr_Category", cboCategory.SelectedValue);
                    cm.Parameters.AddWithValue("@Pr_PriceOpt", double.Parse(txtPriceOpt.Text));
                    cm.Parameters.AddWithValue("@Pr_Price", double.Parse(txtPriceRozn.Text));
                    cm.Parameters.AddWithValue("@Pr_Reorder", UDReOrder.Value);
                    cn.Open();
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Продукт було успішно додано.", stitle);
                    Clear();
                    //product.LoadProduct();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Ви хочете оновити дані про продукт?", "Оновити дані", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("UPDATE Product SET Pr_Barcode=@barcode,Pr_Title=@Pr_Title,Pr_Brand=@bid,PR_Category=@cid,Pr_Price=@price, Pr_PriceOpt=@priceOpt, Pr_Reorder=@reorder WHERE Pr_Id = @Pr_id", cn);
                    cm.Parameters.AddWithValue("@Pr_Id", txtPcode.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@Pr_Title", txtPrName.Text);
                    cm.Parameters.AddWithValue("@bid", cboBrand.SelectedValue);
                    cm.Parameters.AddWithValue("@cid", cboCategory.SelectedValue);
                    cm.Parameters.AddWithValue("@priceOpt", double.Parse(txtPriceOpt.Text));
                    cm.Parameters.AddWithValue("@price", double.Parse(txtPriceRozn.Text));
                    cm.Parameters.AddWithValue("@reorder", UDReOrder.Value);
                    cn.Open();
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Дані про продукт були успішно оновлені.", stitle);
                    Clear();
                    this.Dispose();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void ProductModule_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
