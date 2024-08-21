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
    public partial class NhapMatHang : Form
    {
        SqlConnection con = new SqlConnection("Server=DESKTOP-ERABJSN\\HAI;Database=QLNhaSach;Integrated Security=True;");
        public NhapMatHang()
        {
            InitializeComponent();
        }
        private SqlConnection CreateConnection()
        {
            con.Open();
            return con;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            string maMH = txtMaMH.Text;
            string tenMH = txtTenMH.Text;
            string query = "INSERT INTO MatHang (MaMH, TenMH) VALUES (@MaMH, @TenMH)";
            // Kết nối tới cơ sở dữ liệu
            {
                con.Open();
                // Tạo đối tượng Command
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    // Thêm các tham số
                    command.Parameters.AddWithValue("@MaMH", maMH);
                    command.Parameters.AddWithValue("@TenMH", tenMH);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Thêm mặt hàng thành công!");
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
