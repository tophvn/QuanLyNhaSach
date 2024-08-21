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
    public partial class ThemNhaPhanPhoi : Form
    {
        SqlConnection con;
        string connectionString = "Server=DESKTOP-ERABJSN\\HAI;Database=QLNhaSach;Integrated Security=True;";
        public ThemNhaPhanPhoi()
        {
            InitializeComponent();
            con = new SqlConnection(connectionString);
            con.Open();
            LoadMaNPP();
        }

        private void LoadMaNPP()
        {
            string query = "SELECT MAX(MaNPP) FROM NhaPhanPhoi";
            using (SqlCommand command = new SqlCommand(query, con))
            {
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    int maxMaNPP = Convert.ToInt32(result);
                    txtMaNPP.Text = (maxMaNPP + 1).ToString();
                }
                else
                {
                    txtMaNPP.Text = "1";
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtNhaPP.Text) || string.IsNullOrEmpty(txtSDT.Text) || string.IsNullOrEmpty(txtDiaChi.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông Báo");
                    return;
                }
                // Tạo mã nhà phân phối mới
                int newMaNPP = Convert.ToInt32(txtMaNPP.Text);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO NhaPhanPhoi (MaNPP, TenNPP, SDT, DiaChi) " + "VALUES (@MaNPP, @TenNPP, @SDT, @DiaChi)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaNPP", newMaNPP);
                        command.Parameters.AddWithValue("@TenNPP", txtNhaPP.Text);
                        command.Parameters.AddWithValue("@SDT", txtSDT.Text);
                        command.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                        command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Thêm nhà phân phối thành công!", "Thông Báo");
                    LoadMaNPP(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
