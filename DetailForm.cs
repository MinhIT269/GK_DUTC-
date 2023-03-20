using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiSieuThi
{
    public partial class DetailForm : Form
    {
        public delegate void Mydel();
        public Mydel d { get; set; }
        public string MSP { get; set; }
        public DetailForm(string str)
        {
            MSP = str;
            InitializeComponent();
            GUI();
            Loadcbb();
            LoadcbbNhaSX();
        }
        private void Loadcbb()
        {
            List<string> db = new List<string>();
            string query = "Select ten_mathang from matHang";
            DataTable db1 = new DataTable();
            db1 = DBhelper.Instance.getRecord(query);
            for (int i = 0; i < db1.Rows.Count; i++)
            {
                db.Add(db1.Rows[i][0].ToString());
            }
            cb_MatHang.Items.AddRange(db.ToArray());
        }
        private void LoadcbbNhaSX()
        {
            List<string> db = new List<string>();
            string query = "Select tennhasx from nhasx";
            DataTable db1 = new DataTable();
            db1 = DBhelper.Instance.getRecord(query);
            for (int i = 0; i < db1.Rows.Count; i++)
            {
                db.Add(db1.Rows[i][0].ToString());
            }
            Cb_NhaSX.Items.AddRange(db.ToArray());
        }
        private void btn_Huy_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Thông báo", "Bạn muốn thoát", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.Close();
            }
            else return;
        }
        private void GUI()
        {
            if (MSP != "")
            {
                DataTable dt = new DataTable();
                string str = " Select masanpham as 'Mã sản phẩm', tensanpham as 'Tên sản phẩm', nhasx.tennhasx as 'Nhà Sản Xuất', ngayNhapHang as 'Ngày Nhập Hàng'," +
                          " \r\n matHang.ten_mathang as 'Mặt hàng', tinhtrang as 'Tình trạng'" +
                          "\r\n from sanpham Inner Join matHang  On sanpham.id_matHang = matHang.id_matHang" +
                          "\r\n inner join nhasx on nhasx.id_matHang = matHang.id_matHang";
                str += " where masanpham = '" + MSP + "'";
                dt = DBhelper.Instance.getRecord(str);
                txt_MaSP.Text = MSP;
                txt_TenSP.Text = dt.Rows[0]["Tên sản phẩm"].ToString();
                dateTimePicker1.Text = dt.Rows[0]["Ngày Nhập Hàng"].ToString();
                cb_MatHang.Text = dt.Rows[0]["Mặt hàng"].ToString();
                Cb_NhaSX.Text = dt.Rows[0]["Nhà Sản Xuất"].ToString();
                rad_Con.Checked = bool.Parse(dt.Rows[0]["Tình trạng"].ToString());
                txt_MaSP.Enabled = false;
            }
        }
        private bool check(string msp)
        {
            DataTable dt = new DataTable();
            string str = " Select masanpham as 'Mã sản phẩm', tensanpham as 'Tên sản phẩm', nhasx.tennhasx as 'Nhà Sản Xuất', ngayNhapHang as 'Ngày Nhập Hàng'," +
                     " \r\n matHang.ten_mathang as 'Mặt hàng', tinhtrang as 'Tình trạng'" +
                     "\r\n from sanpham Inner Join matHang  On sanpham.id_matHang = matHang.id_matHang" +
                     "\r\n inner join nhasx on nhasx.id_matHang = matHang.id_matHang";
            dt = DBhelper.Instance.getRecord(str);
            bool check = false;
            foreach (DataRow item in dt.Rows)
            {
                if (msp.CompareTo(item[0]) == 0)
                {
                    check = true;
                    break;
                }
            }
            return check;
        }
        private void SyncDB(List<string> li)
        {
            if (check(li[0]))
            {
                Update(li);
            }
            else
            {
                 Add(li);
            }
        }
        private void Update(List<string> li)
        {
            string query = "UPDATE sanpham";
            query += "  SET tensanpham = '" + li[1] + "'";
            query += " , ngayNhapHang =  '" + li[2] + "'";
            query += " ,id_matHang = '" + li[3] + "'";
            query += " ,id_nhasanxuat = '" + li[4] + "'";
            query += " , tinhtrang = '" + li[5] + "'";
            query += " WHERE masanpham = '" + li[0] + "'";
            DBhelper.Instance.ExectuteNonQuery(query);

        }
        private void Add(List<string> li)
        {
            string str = string.Format("Insert into sanpham(masanpham,tensanpham,ngayNhapHang,tinhtrang,id_matHang,id_nhasanxuat)" +
                "VALUES('{0}','{1}','{2}','{3}','{4}','{5}')", li[0], li[1], li[2], li[5], li[3], li[4]);
            DBhelper.Instance.ExectuteNonQuery(str);
        }
        private void updateData()
        {
            List<string> list = new List<string>();
            list.Add(txt_MaSP.Text.ToString());
            list.Add(txt_TenSP.Text.ToString());
            list.Add(Convert.ToString(Convert.ToDateTime(dateTimePicker1.Text.ToString())));
            string str = "Select id_matHang from matHang where ten_mathang = '" + cb_MatHang.Text.ToString() + "'";
            DataTable t = new DataTable();
            t = DBhelper.Instance.getRecord(str);  
            string s = t.Rows[0][0].ToString();
            list.Add(s);
            string query = "select id_nhasanxuat from nhasx where tennhasx = '"+Cb_NhaSX.Text.ToString()+"'";
            DataTable dt = new DataTable();
            dt = DBhelper.Instance.getRecord(query);
            string id = dt.Rows[0][0].ToString();
            list.Add(id);
            if (rad_Con.Checked) list.Add("true");
            else list.Add("false");
            SyncDB(list);
            d();
            this.Close();
        }
        private void btn_OK_Click(object sender, EventArgs e)
        {
            updateData();
            this.Close();
        }
    }
}
