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
    public partial class DangNhap : Form
    {
        SqlConnection con = new SqlConnection("Server=DESKTOP-ERABJSN\\HAI;Database=QLNhaSach;Integrated Security=True;");
        public DangNhap()
        {
            InitializeComponent();
        }

        //Tạo và mở kết nối SQL
        private SqlConnection CreateConnection()
        {
            con.Open();
            return con;
        }

        // Lấy vai trò từ cơ sở dữ liệu dựa vào tên tài khoản
        private string ChucVu(string userName, SqlConnection con)
        {
            string chucVu = "";

            string query = "SELECT Quyen FROM DangNhap WHERE TaiKhoan = @TaiKhoan";
            using (SqlCommand command = new SqlCommand(query, con))
            {
                command.Parameters.AddWithValue("@TaiKhoan", userName);
                chucVu = command.ExecuteScalar()?.ToString();
            }

            return chucVu;
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string query = "SELECT COUNT(*) FROM DangNhap WHERE TaiKhoan = @TaiKhoan AND MatKhau = @MatKhau";
            SqlConnection con = CreateConnection();
            using (SqlCommand command = new SqlCommand(query, con))
            {
                command.Parameters.AddWithValue("@TaiKhoan", txtUser.Text);
                command.Parameters.AddWithValue("@MatKhau", txtPass.Text);
                int count = Convert.ToInt32(command.ExecuteScalar());

                if (count == 1)
                {
                    string chucVu = ChucVu(txtUser.Text, con); // Lấy vai trò từ database

                    if (chucVu == "Admin")
                    {
                        Admin ad = new Admin();
                        ad.Show();
                    }
                    else if (chucVu == "Nhân viên")
                    {
                        NhanVien nv = new NhanVien();
                        nv.Show();
                    }

                    this.Hide();
                }
                else
                {
                    string message = "Tên đăng nhập hoặc mật khẩu không đúng!";
                    MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                con.Close();
            }
        }

        private void cbHienMK_CheckedChanged(object sender, EventArgs e)
        {
            txtPass.UseSystemPasswordChar = !cbHienMK.Checked;
        }

    }
}