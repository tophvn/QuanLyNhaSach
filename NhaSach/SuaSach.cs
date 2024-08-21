using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NhaSach
{
    public partial class SuaSach : Form
    {
        SqlConnection con = new SqlConnection("Server=DESKTOP-ERABJSN\\HAI;Database=QLNhaSach;Integrated Security=True;");
        private int sachId;

        public SuaSach(int selectedSachId)
        {
            InitializeComponent();
            this.sachId = selectedSachId;
            LoadComboboxNhaPP();
            LoadSachInfo();
        }

        public SuaSach()
        {
        }

        private void LoadSachInfo()
        {
            try
            {
                con.Open();
                string query = "SELECT * FROM Sach WHERE MaSach = @MaSach";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@MaSach", sachId);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        cbbMaSach.SelectedItem = reader["MaSach"].ToString();
                        txtTenSach.Text = reader["TenSach"].ToString();
                        cbbDonVi.SelectedItem = reader["DonVi"].ToString();
                        txtSoLuong.Text = reader["SoLuongTon"].ToString();
                        txtDonGia.Text = reader["DonGia"].ToString();
                        txtGiaNhap.Text = reader["GiaNhap"].ToString();
                        cbbTheLoai.SelectedItem = reader["TheLoai"].ToString();
                        cbbNhaPP.SelectedItem = GetTenNhaPP(Convert.ToInt32(reader["MaNPP"]));
                        txtMoTa.Text = reader["MoTa"].ToString();
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void LoadComboboxNhaPP()
        {
            try
            {
                con.Open();
                string query = "SELECT TenNPP FROM NhaPhanPhoi";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        cbbNhaPP.Items.Add(reader["TenNPP"].ToString());
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private string GetTenNhaPP(int maNPP)
        {
            string tenNPP = "";
            try
            {
                con.Open();
                string query = "SELECT TenNPP FROM NhaPhanPhoi WHERE MaNPP = @MaNPP";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@MaNPP", maNPP);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        tenNPP = reader["TenNPP"].ToString();
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return tenNPP;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string query = "UPDATE Sach SET TenSach = @TenSach, DonVi = @DonVi, SoLuongTon = @SoLuongTon, " +
                               "DonGia = @DonGia, GiaNhap = @GiaNhap, TheLoai = @TheLoai, MaNPP = @MaNPP, MoTa = @MoTa " +
                               "WHERE MaSach = @MaSach";

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@MaSach", cbbMaSach.SelectedItem);
                    command.Parameters.AddWithValue("@TenSach", txtTenSach.Text);
                    command.Parameters.AddWithValue("@DonVi", cbbDonVi.SelectedItem);
                    command.Parameters.AddWithValue("@SoLuongTon", txtSoLuong.Text);
                    command.Parameters.AddWithValue("@DonGia", txtDonGia.Text);
                    command.Parameters.AddWithValue("@GiaNhap", txtGiaNhap.Text);
                    command.Parameters.AddWithValue("@TheLoai", cbbTheLoai.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@MaNPP", GetMaNhaPP(cbbNhaPP.SelectedItem.ToString()));
                    command.Parameters.AddWithValue("@MoTa", txtMoTa.Text);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Sửa sách thành công");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
            finally
            {
                con.Close();
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

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
