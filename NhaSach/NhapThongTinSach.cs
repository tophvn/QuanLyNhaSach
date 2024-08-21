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
    public partial class NhapThongTinSach : Form
    {
        SqlConnection con;

        public NhapThongTinSach()
        {
            InitializeComponent();
            con = new SqlConnection("Server=DESKTOP-ERABJSN\\HAI;Database=QLNhaSach;Integrated Security=True;");
            con.Open();
        }

        private void NhapThongTinSach_Load(object sender, EventArgs e)
        {
            LoadComboboxNhaPP();
            LoadMaSach();
        }

        private void LoadComboboxNhaPP()
        {
            try
            {
                string query = "SELECT TenNPP FROM NhaPhanPhoi";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        cbbNhaPP.Items.Add(reader["TenNPP"].ToString());
                    }
                    reader.Close(); // Đóng DataReader
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }

        private void LoadMaSach()
        {
            try
            {
                string query = "SELECT MAX(MaSach) FROM Sach";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        int maxMaSach = Convert.ToInt32(result);
                        txtMaSach.Text = (maxMaSach + 1).ToString();
                    }
                    else
                    {
                        txtMaSach.Text = "1";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }

        private void ResetForm()
        {
            txtTenSach.Clear();
            cbbTheLoai.SelectedIndex = -1;
            cbbNhaPP.SelectedIndex = -1;
            txtDonGia.Clear();
            cbbDonVi.SelectedIndex = -1;
            txtSoLuong.Clear();
            txtMoTa.Clear();
            LoadMaSach();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "INSERT INTO Sach (MaSach, TenSach, DonVi, SoLuongTon, DonGia, GiaNhap, TheLoai, MaNPP, MoTa) " +
                               "VALUES (@MaSach, @TenSach, @DonVi, @SoLuongTon, @DonGia, @GiaNhap, @TheLoai, @MaNPP, @MoTa)";

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@MaSach", txtMaSach.Text);
                    command.Parameters.AddWithValue("@TenSach", txtTenSach.Text);
                    command.Parameters.AddWithValue("@DonVi", cbbDonVi.SelectedItem);
                    command.Parameters.AddWithValue("@SoLuongTon", txtSoLuong.Text);
                    command.Parameters.AddWithValue("@DonGia", txtDonGia.Text);
                    command.Parameters.AddWithValue("@GiaNhap", 0); 
                    command.Parameters.AddWithValue("@TheLoai", DBNull.Value); // Chưa có thông tin về Mã Mặt hàng
                    command.Parameters.AddWithValue("@MaNPP", GetMaNhaPP(cbbNhaPP.SelectedItem.ToString()));
                    command.Parameters.AddWithValue("@MoTa", txtMoTa.Text);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Thêm sách thành công");
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }

        private int GetMaNhaPP(string tenNPP)
        {
            int maNPP = 0;
            try
            {
                string query = "SELECT MaNPP FROM NhaPhanPhoi WHERE TenNPP = @TenNPP";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@TenNPP", tenNPP);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        maNPP = Convert.ToInt32(reader["MaNPP"]);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
            return maNPP;
        }

        private void NhapThongTinSach_FormClosing_1(object sender, FormClosingEventArgs e)
        {

        }
    }
}