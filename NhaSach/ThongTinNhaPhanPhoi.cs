using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace NhaSach
{
    public partial class ThongTinNhaPhanPhoi : Form
    {
        SqlConnection con = new SqlConnection("Server=DESKTOP-ERABJSN\\HAI;Database=QLNhaSach;Integrated Security=True;");
        public ThongTinNhaPhanPhoi()
        {
            InitializeComponent();
        }

        private SqlConnection CreateConnection()
        {
            if (con.State == ConnectionState.Closed) 
            {
                con.Open();
            }
            return con;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMaNPP.Text) || string.IsNullOrEmpty(txtNhaPP.Text) || string.IsNullOrEmpty(txtSDT.Text) || string.IsNullOrEmpty(txtDiaChi.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông Báo");
                    return;
                }
                using (SqlConnection connection = CreateConnection())
                {
                    string query = "INSERT INTO NhaPhanPhoi (MaNPP, TenNPP, SDT, DiaChi) " +
                                   "VALUES (@MaNPP, @TenNPP, @SDT, @DiaChi)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaNPP", txtMaNPP.Text);
                        command.Parameters.AddWithValue("@TenNPP", txtNhaPP.Text);
                        command.Parameters.AddWithValue("@SDT", txtSDT.Text);
                        command.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                        command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Thêm nhà phân phối thành công!", "Thông Báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            this.Close();
        }
    }
}