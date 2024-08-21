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
    public partial class ThemSach : Form
    {
        SqlConnection con = new SqlConnection("Server=DESKTOP-ERABJSN\\HAI;Database=QLNhaSach;Integrated Security=True;");
        //SqlConnection con;
        private int sachId;

        public ThemSach()
        {
            InitializeComponent();
            con.Open();
        }

        public ThemSach(int sachId)
        {
            this.sachId = sachId;
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

        private void lammoi()
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
                if (string.IsNullOrEmpty(txtMaSach.Text) || string.IsNullOrEmpty(txtTenSach.Text) || cbbDonVi.SelectedItem == null || string.IsNullOrEmpty(txtSoLuong.Text) || string.IsNullOrEmpty(txtDonGia.Text) || string.IsNullOrEmpty(txtGiaNhap.Text) || cbbTheLoai.SelectedItem == null || cbbNhaPP.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; 
                }
                string query = "INSERT INTO Sach (MaSach, TenSach, DonVi, SoLuongTon, DonGia, GiaNhap, TheLoai, MaNPP, MoTa) " + "VALUES (@MaSach, @TenSach, @DonVi, @SoLuongTon, @DonGia, @GiaNhap, @TheLoai, @MaNPP, @MoTa)";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@MaSach", txtMaSach.Text);
                    command.Parameters.AddWithValue("@TenSach", txtTenSach.Text);
                    command.Parameters.AddWithValue("@DonVi", cbbDonVi.SelectedItem);
                    command.Parameters.AddWithValue("@SoLuongTon", txtSoLuong.Text);
                    command.Parameters.AddWithValue("@DonGia", txtDonGia.Text);
                    command.Parameters.AddWithValue("@GiaNhap", txtGiaNhap.Text);
                    command.Parameters.AddWithValue("@TheLoai", cbbTheLoai.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@MaNPP", GetMaNhaPP(cbbNhaPP.SelectedItem.ToString()));
                    command.Parameters.AddWithValue("@MoTa", txtMoTa.Text);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Thêm sách thành công");
                    lammoi();
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

        private void ThemSach_Load(object sender, EventArgs e)
        {
            txtMaSach.Enabled = false;
            LoadComboboxNhaPP();
            LoadMaSach();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}