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
    public partial class InHoaDon : Form
    {
        SqlConnection con = new SqlConnection("Server=DESKTOP-ERABJSN\\HAI;Database=QLNhaSach;Integrated Security=True;");

        public InHoaDon()
        {
            InitializeComponent();
        }

        private DataTable LoadLatestData()
        {
            DataTable dtb = new DataTable("ChiTietHoaDon");
            string query = "SELECT MaCTHD, TenSach, MaSach, TenKH, SoLuong, DonGia, ThanhTien FROM ChiTietHoaDon";
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dtb);
            return dtb;
        }

        private void InHoaDon_Load(object sender, EventArgs e)
        {
            DataTable dtb = LoadLatestData();
            CrystalReport4 report = new CrystalReport4();
            report.SetDataSource(dtb);
            crystalReportViewer1.ReportSource = report;
            crystalReportViewer1.Refresh();
        }
    }
}