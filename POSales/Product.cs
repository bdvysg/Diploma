﻿using System;
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
    public partial class Product : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        int StartIndex = 0;
        int RowCount = 25;
        int i = 0;
        public Product()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            LoadProduct();
        }

        public void LoadProduct()
        {
            i = 0;
            StartIndex = 0;
            dgvProduct.Rows.Clear();
            cm = new SqlCommand("exec GetProductsList '" + txtSearch.Text+ "', " + StartIndex.ToString() + ", " + 50,cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(),
                                       dr[1].ToString(),
                                       dr[2].ToString(),
                                       dr[3].ToString(),
                                       dr[4].ToString(),
                                       dr[5].ToString(),
                                       dr[6].ToString(),
                                       dr[7].ToString(),
                                       dr[8].ToString(),
                                       dr[9].ToString()
                                    ); 
            }
            dr.Close();
            cn.Close();
        }

        private void dgvProduct_Scroll(object sender, ScrollEventArgs e)
        {
            int displayedRows = dgvProduct.DisplayedRowCount(true);
            int lastVisibleRowIndex = dgvProduct.FirstDisplayedScrollingRowIndex + displayedRows - 1;
            if (lastVisibleRowIndex >= dgvProduct.RowCount - 1)
            {
                StartIndex += 25;
                cm = new SqlCommand("exec GetProductsList '" + txtSearch.Text + "', " + StartIndex.ToString() + ", " + RowCount.ToString(), cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvProduct.Rows.Add(i, dr[0].ToString(),
                                           dr[1].ToString(),
                                           dr[2].ToString(),
                                           dr[3].ToString(),
                                           dr[4].ToString(),
                                           dr[5].ToString(),
                                           dr[6].ToString(),
                                           dr[7].ToString(),
                                           dr[8].ToString(),
                                           dr[9].ToString()

                                        );
                }
                dr.Close();
                cn.Close();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProductModule productModule = new ProductModule();
            productModule.ShowDialog();
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProduct.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                ProductModule product = new ProductModule();
                //product.txtPcode.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
                //product.txtBarcode.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
                //product.txtPdesc.Text = dgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString();
                //product.cboBrand.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
                //product.cboCategory.Text = dgvProduct.Rows[e.RowIndex].Cells[5].Value.ToString();
                //product.txtPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[6].Value.ToString();
                //product.UDReOrder.Value = int.Parse(dgvProduct.Rows[e.RowIndex].Cells[7].Value.ToString());
                product.LoadProduct(dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString());

                product.txtPcode.Enabled = false;
                product.btnSave.Enabled = false;
                product.btnUpdate.Enabled = true;
                product.label1.Text = "Редагувати товар";
                product.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Видалити обраний товар", "Видалити", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM Product WHERE Pr_id = " + dgvProduct[1, e.RowIndex].Value.ToString(), cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Товар було успішно видалено .", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LoadProduct();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }
    }
}
