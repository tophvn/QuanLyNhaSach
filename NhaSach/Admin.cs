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
    public partial class Admin : Form
    {
        private SqlConnection con = new SqlConnection("Server=DESKTOP-ERABJSN\\HAI;Database=QLNhaSach;Integrated Security=True;");
        public Admin()
        {
            InitializeComponent();
            load_dtgrid_User();
        }

        //Tab quản lý tài khoản
        private void load_dtgrid_User()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT TaiKhoan, MatKhau, Quyen FROM DangNhap", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dtg_User.DataSource = dt;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string taiKhoan = txtTaiKhoan.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();
            string quyen = cbbQuyen.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(taiKhoan) || string.IsNullOrEmpty(matKhau) || string.IsNullOrEmpty(quyen))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO DangNhap (TaiKhoan, MatKhau, Quyen) VALUES (@TaiKhoan, @MatKhau, @Quyen)", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@TaiKhoan", taiKhoan);
                    cmd.Parameters.AddWithValue("@MatKhau", matKhau);
                    cmd.Parameters.AddWithValue("@Quyen", quyen);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm tài khoản: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
                load_dtgrid_User();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (dtg_User.SelectedRows.Count == 1)
            {
                string taiKhoan = txtTaiKhoan.Text.Trim();
                string matKhau = txtMatKhau.Text.Trim();
                string quyen = cbbQuyen.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(taiKhoan) || string.IsNullOrEmpty(matKhau) || string.IsNullOrEmpty(quyen))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                try
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE DangNhap SET MatKhau = @MatKhau, Quyen = @Quyen WHERE TaiKhoan = @TaiKhoan", con))
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@MatKhau", matKhau);
                        cmd.Parameters.AddWithValue("@Quyen", quyen);
                        cmd.Parameters.AddWithValue("@TaiKhoan", taiKhoan);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Lưu tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lammoi();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy tài khoản để lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu tài khoản: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                    load_dtgrid_User();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một tài khoản để lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dtg_User.SelectedRows.Count == 1)
            {
                string taiKhoan = dtg_User.SelectedRows[0].Cells["TaiKhoan"].Value.ToString();
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM DangNhap WHERE TaiKhoan = @TaiKhoan", con))
                        {
                            con.Open();
                            cmd.Parameters.AddWithValue("@TaiKhoan", taiKhoan);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lammoi();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy tài khoản để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa tài khoản: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        con.Close();
                        load_dtgrid_User();
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn tài khoản để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dtg_User.SelectedRows.Count == 1)
            {
                txtTaiKhoan.Text = dtg_User.SelectedRows[0].Cells["TaiKhoan"].Value.ToString();
                txtMatKhau.Text = dtg_User.SelectedRows[0].Cells["MatKhau"].Value.ToString();
                cbbQuyen.SelectedItem = dtg_User.SelectedRows[0].Cells["Quyen"].Value.ToString();
                txtTaiKhoan.Enabled = false;
            }
        }

        private void lammoi()
        {
            cbbQuyen.SelectedItem = null;
            txtTaiKhoan.Text = "";
            txtMatKhau.Text = "";
            txtTaiKhoan.Enabled = true;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            lammoi();
        }

        private void Admin_FormClosed(object sender, FormClosedEventArgs e)
        {
            //DialogResult r;
            //r = MessageBox.Show("Bạn có muốn thoát?", "Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            con.Close();
            //Kho sách
            SqlDataAdapter sda = new SqlDataAdapter("SELECT SUM(SoLuongTon) FROM Sach", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            SL_Sach.Text = dt.Rows[0][0].ToString();

            //Doanh Thu
            SqlDataAdapter sda1 = new SqlDataAdapter("SELECT SUM(ThanhTien) FROM ChiTietHoaDon", con);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            SL_DoanhThu.Text = dt1.Rows[0][0].ToString();

            //Tài Khoản
            SqlDataAdapter sda2 = new SqlDataAdapter("SELECT COUNT(*) FROM DangNhap", con);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            SL_TaiKhoan.Text = dt2.Rows[0][0].ToString();
            con.Close();
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
    }
}
