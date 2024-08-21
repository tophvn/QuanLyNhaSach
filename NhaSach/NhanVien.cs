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
    public partial class NhanVien : Form
    {
        SqlConnection con = new SqlConnection("Server=DESKTOP-ERABJSN\\HAI;Database=QLNhaSach;Integrated Security=True;");
        DataTable dataTable;
        public NhanVien()
        {
            InitializeComponent();
        }

        private void loadSach()
        {
            string query = "SELECT * FROM Sach"; 
            SqlDataAdapter adp = new SqlDataAdapter(query, con);
            DataTable dataTable = new DataTable();
            adp.Fill(dataTable);
            dtgHienThiSach.DataSource = dataTable;
        }

        //truy vấn dữ liệu bảng Sach
        private DataTable GetSachData() 
        {
            DataTable dataTable = new DataTable();
            string query = "SELECT * FROM Sach";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
            }
            return dataTable;
        }

        private void NhanVien_Load(object sender, EventArgs e)
        {
            loadSach();
            dataTable = GetSachData();
            DTG_DanhSach.DataSource = dataTable;
            LoadMaSachComboBox();

            txtTenSach.Enabled = false;
            txtGia.Enabled = false;
        }

        private void btnLamMoiDTG_Click(object sender, EventArgs e)
        {
            loadSach();
        }


//Tab quan lý sách
        private void btnThemNPP_Click(object sender, EventArgs e)
        {
            ThemNhaPhanPhoi tnpp = new ThemNhaPhanPhoi();
            tnpp.Show();
        }

        private void btnThemSach_Click(object sender, EventArgs e)
        {
            ThemSach ntts = new ThemSach();
            ntts.Show();
        }

        private void btnXoaSach_Click(object sender, EventArgs e)
        {
            if (dtgHienThiSach.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Bạn muốn xóa sách này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    int selectedRowIndex = dtgHienThiSach.SelectedRows[0].Index;
                    int masach = Convert.ToInt32(dtgHienThiSach.Rows[selectedRowIndex].Cells["MaSach"].Value);
                    try
                    {
                        // Xóa sách dựa theo ma sach
                        string query = "DELETE FROM Sach WHERE MaSach = @MaSach";
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            command.Parameters.AddWithValue("@MaSach", masach);
                            con.Open();
                            command.ExecuteNonQuery();
                            con.Close();
                        }
                        MessageBox.Show("Xóa sách thành công.", "Thông Báo");
                        loadSach();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa.","Thông Báo");
            }
        }

        private void btnSuaSach_Click(object sender, EventArgs e)
        {
            con.Open();

            if (dtgHienThiSach.SelectedRows.Count > 0)
            {
                // Lấy giá trị của cột "MaSach" từ hàng được chọn
                int selectedSachId = Convert.ToInt32(dtgHienThiSach.SelectedRows[0].Cells["MaSach"].Value);
                // Mở form sửa sách và truyền mã sách cần sửa
                SuaSach suaSachForm = new SuaSach(selectedSachId);
                this.Hide(); // Đóng form hiện tại
                suaSachForm.Show();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sách để sửa.");
            };
            con.Close();
        }


        //Tab thanh toán
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenSach.Text) || string.IsNullOrWhiteSpace(txtTenKH.Text) || string.IsNullOrWhiteSpace(txtSoLuong.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
            }
            else
            {
                try
                {
                    //Tạo mã chi tiết hóa don
                    string maCTHD;
                    string getMaxMaCTHDQuery = "SELECT MAX(CAST(ISNULL(MaCTHD, 0) AS INT)) FROM ChiTietHoaDon";
                    using (SqlCommand getMaxMaCTHDCommand = new SqlCommand(getMaxMaCTHDQuery, con))
                    {
                        con.Open();
                        object result = getMaxMaCTHDCommand.ExecuteScalar(); //lấy giá trị tra về
                        con.Close();

                        int maxMaCTHD = result == DBNull.Value ? 0 : Convert.ToInt32(result); //Handle DBNull
                        maxMaCTHD++;
                        maCTHD = maxMaCTHD.ToString();
                    }

                    string selectedMaSach = cbbMaSach.SelectedItem.ToString();

                    // Thêm dòng mới vào DataGridView
                    DTG_HoaDon.Rows.Add(maCTHD, selectedMaSach, txtTenSach.Text, txtTenKH.Text, txtSoLuong.Text, txtGia.Text, (Convert.ToInt32(txtSoLuong.Text) * Convert.ToDecimal(txtGia.Text)).ToString());

                    // Thực hiện INSERT vào bảng ChiTietHoaDon
                    string insertQuery = "INSERT INTO ChiTietHoaDon (MaCTHD, MaSach, TenSach, TenKH, SoLuong, DonGia, ThanhTien) VALUES (@MaCTHD, @MaSach, @TenSach, @TenKH, @SoLuong, @DonGia, @ThanhTien)";
                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, con))
                    {
                        insertCommand.Parameters.AddWithValue("@MaCTHD", maCTHD);
                        insertCommand.Parameters.AddWithValue("@MaSach", selectedMaSach);
                        insertCommand.Parameters.AddWithValue("@TenSach", txtTenSach.Text);
                        insertCommand.Parameters.AddWithValue("@TenKH", txtTenKH.Text);
                        insertCommand.Parameters.AddWithValue("@SoLuong", txtSoLuong.Text);
                        insertCommand.Parameters.AddWithValue("@DonGia", txtGia.Text);
                        insertCommand.Parameters.AddWithValue("@ThanhTien", (Convert.ToInt32(txtSoLuong.Text) * Convert.ToDecimal(txtGia.Text)).ToString());

                        con.Open();
                        insertCommand.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                }
            }
        }

        private void LoadMaSachComboBox()
        {
            // Lặp qua DataTable để thêm mã sách vào ComboBox
            foreach (DataRow row in dataTable.Rows)
            {
                string maSach = row["MaSach"].ToString();
                cbbMaSach.Items.Add(maSach);
            }
        }

        private void cbbMaSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbMaSach.SelectedItem != null)
            {
                string selectedMaSach = cbbMaSach.SelectedItem.ToString();
                DataRow[] rows = dataTable.Select("MaSach = '" + selectedMaSach + "'");
                cbbMaSach.Text = rows[0]["MaSach"].ToString();
                txtTenSach.Text = rows[0]["TenSach"].ToString();
                txtGia.Text = rows[0]["DonGia"].ToString();        
            }
        }

        private void lammoi()
        {
            cbbMaSach.SelectedItem = null;
            txtTenSach.Clear();
            txtTenKH.Clear();
            txtSoLuong.Clear();
            txtGia.Clear();
        }

        private void LamMoi_Click(object sender, EventArgs e)
        {
            lammoi();
        }

        private void NhanVien_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        private void btn_LamMoiDTG_Click(object sender, EventArgs e)
        {
            
        }

        private void DangXuat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn đăng xuất?", "Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (result == DialogResult.No)
            {
                return;
            }
            DangNhap obj = new DangNhap();
            obj.Show();
            this.Hide();
        }

        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            InHoaDon tnpp = new InHoaDon();
            tnpp.Show();
        }
    }
}