using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiSieuThi
{
    class DBhelper
    {
        private static DBhelper _Instance;
        private SqlConnection sqlCon;
        //private SqlCommand cmd;
        private DBhelper(string s)
        {
            sqlCon = new SqlConnection(s);
        }
        public static DBhelper Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new DBhelper(@"Data Source=DESKTOP-BC5FMVK\SQLEXPRESS;Initial Catalog=QuanLiSieuThi;Integrated Security=True");
                return _Instance;
            }
            private set { }
        }
        //Lấy dữ liệu
        public DataTable getRecord(string str)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(str, sqlCon);
            sqlCon.Open();
            da.Fill(dt);
            sqlCon.Close();
            return dt;
        }
        //Truy vấn cơ sở dữ liệu
        public void ExectuteNonQuery(string query)
        {

            SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
            sqlCon.Open();
            sqlcmd.ExecuteNonQuery();
            sqlCon.Close();
        }
    }
}
