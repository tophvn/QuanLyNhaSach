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
    public partial class NhapSachKM : Form
    {
        SqlConnection con = new SqlConnection("Server=DESKTOP-ERABJSN\\HAI;Database=QLNhaSach;Integrated Security=True;");
        private DateTime batDau;
        public NhapSachKM(string tenKM)
        {
            InitializeComponent();
        }
        private SqlConnection CreateConnection()
        {
            con.Open();
            return con;
        }

        public NhapSachKM(string tenKM, DateTime batDau) : this(tenKM)
        {
            this.batDau = batDau;
        }

        private void bt_OK_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ các điều khiển trên form
            string selectedTenSach = cbbTenSach.SelectedItem?.ToString();
            string donGia = txtDonGia.Text;
            string selectedMaKM = cbbMa.SelectedItem?.ToString();

            // Kiểm tra và xử lý dữ liệu
            if (string.IsNullOrEmpty(selectedTenSach))
            {
                MessageBox.Show("Vui lòng chọn tên sách!");
                return;
            }

            if (string.IsNullOrEmpty(donGia))
            {
                MessageBox.Show("Vui lòng nhập đơn giá!");
                return;
            }

            float donGiaValue;
            if (!float.TryParse(donGia, out donGiaValue))
            {
                MessageBox.Show("Đơn giá không hợp lệ!");
                return;
            }

            if (string.IsNullOrEmpty(selectedMaKM))
            {
                MessageBox.Show("Vui lòng chọn mã KM!");
                return;
            }

            // Thực hiện thao tác với CSDL
            using (SqlConnection connection = CreateConnection())
            {
                // Sử dụng connection để thao tác với CSDL
                string query = "INSERT INTO ChiTietKhuyeMai (MaSach, DonGiaKM, MaKM) VALUES (@MaSach, @DonGiaKM, @MaKM)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaSach", selectedTenSach);
                    command.Parameters.AddWithValue("@DonGiaKM", donGiaValue);
                    command.Parameters.AddWithValue("@MaKM", selectedMaKM);
                    command.ExecuteNonQuery();
                }
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtDonGia_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbbMa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
