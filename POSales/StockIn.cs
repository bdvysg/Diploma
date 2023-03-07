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
    public partial class StockIn : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string stitle = "Market";
        MainForm main;
        public StockIn(MainForm mn)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            main = mn;
            LoadSupplier();
            GetRefeNo();
        }

        public void GetRefeNo()
        {
            Random rnd = new Random();
            //txtRefNo.Clear();
            //txtRefNo.Text += rnd.Next();
        }

        public void LoadSupplier()
        {
            //cbSupplier.Items.Clear();
            //cbSupplier.DataSource = dbcon.getTable("SELECT * FROM Supplier");
            //cbSupplier.DisplayMember = "Sup_Title";
        }
    }
}
