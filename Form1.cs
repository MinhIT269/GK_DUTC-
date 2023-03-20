using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiSieuThi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadDTG();
        }
        private void LoadDTG()
        {
            int cnt = 0;
            string str = " Select masanpham as 'Mã sản phẩm', tensanpham as 'Tên sản phẩm', nhasx.tennhasx as 'Nhà Sản Xuất', ngayNhapHang as 'Ngày Nhập Hàng'," +
                          " \r\n matHang.ten_mathang as 'Mặt hàng', tinhtrang as 'Tình trạng'" +
                          "\r\n from sanpham Inner Join matHang  On sanpham.id_matHang = matHang.id_matHang" +
                          "\r\n inner join nhasx on nhasx.id_matHang = matHang.id_matHang";
            DataTable dt = new DataTable();
            dt = DBhelper.Instance.getRecord(str);
            dt.Columns.Add(new DataColumn { ColumnName = "STT", DataType = typeof(int) });
            dt.Columns["STT"].SetOrdinal(0);
            foreach (DataRow item in dt.Rows)
            {
                item["STT"] = cnt++;
            }
            dataGridView1.DataSource = dt;
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show( "Bạn có muốn xóa sản phẩm","Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                List<string> list = new List<string>();
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                    {
                        list.Add(item.Cells["Mã sản phẩm"].Value.ToString());
                    }
                }
                foreach (string item in list)
                {
                    string str = "Delete from sanpham where masanpham = '" + item + "'";
                    DBhelper.Instance.ExectuteNonQuery(str);
                }
                LoadDTG();
            }
            else return;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string str = " Select masanpham as 'Mã sản phẩm', tensanpham as 'Tên sản phẩm', nhasx.tennhasx as 'Nhà Sản Xuất', ngayNhapHang as 'Ngày Nhập Hàng'," +
                          " \r\n matHang.ten_mathang as 'Mặt hàng', tinhtrang as 'Tình trạng'" +
                          "\r\n from sanpham Inner Join matHang  On sanpham.id_matHang = matHang.id_matHang" +
                          "\r\n inner join nhasx on nhasx.id_matHang = matHang.id_matHang";
            if (comboBox1.SelectedItem.ToString() == "Tên sản phẩm")
            {
                str += " ORDER BY tensanpham";
            }
            else if (comboBox1.SelectedItem.ToString() == "Mã sản phẩm")
            {
                str += " Order by masanpham desc";
            }
            // Bổ sung sau khi nộp bài 
            else if (comboBox1.SelectedItem.ToString() == "Ngày tháng")
            {
                str += " Order by ngayNhapHang desc";
            }
            else if (comboBox1.SelectedItem.ToString() == "Nhà SX")
            {
                str += " Order by tennhasx desc";
            }
           
            dataGridView1.DataSource = DBhelper.Instance.getRecord(str); ;
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            string str = " Select masanpham as 'Mã sản phẩm', tensanpham as 'Tên sản phẩm', nhasx.tennhasx as 'Nhà Sản Xuất', ngayNhapHang as 'Ngày Nhập Hàng'," +
                          " \r\n matHang.ten_mathang as 'Mặt hàng', tinhtrang as 'Tình trạng'" +
                          "\r\n from sanpham Inner Join matHang  On sanpham.id_matHang = matHang.id_matHang" +
                          "\r\n inner join nhasx on nhasx.id_matHang = matHang.id_matHang";
            string temp = txt_search.Text.ToString();
            if (temp != "")
            {
                str += " where masanpham = '" + temp + "'";
            }
            DBhelper.Instance.getRecord(str);
            dataGridView1.DataSource = DBhelper.Instance.getRecord(str); ;
            
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                string str = dataGridView1.SelectedRows[0].Cells["Mã sản phẩm"].Value.ToString();
                DetailForm f = new DetailForm(str);
                f.d += new DetailForm.Mydel(LoadDTG);
                f.Show();
              
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            DetailForm f = new DetailForm("");
            f.d += new DetailForm.Mydel(LoadDTG);
            f.Show();
        }
    }
}
