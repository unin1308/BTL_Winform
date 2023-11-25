using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    internal class ProcessDatabase
    {
        SqlConnection con;

        public void KetNoi()
        {
            con = new SqlConnection(@"Data Source=DESKTOP-FQOD3P8\LE;Initial Catalog=QuanLyHocVien;Integrated Security=True");
            //if (con.State != ConnectionState.Open)
            con.Open();
        }

        public void DongKetNoi()
        {
            if (con.State != ConnectionState.Closed)
                con.Close();
            con.Dispose();
        }

        public DataTable DocBang(string sql)
        {
            KetNoi();
            DataTable tb = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            da.Fill(tb);
            DongKetNoi();
            return tb;
        }

        public void CapNhat(string sql)
        {
            SqlCommand cm = new SqlCommand();
            KetNoi();
            cm.CommandText = sql;
            cm.Connection = con;
            cm.ExecuteNonQuery();
            DongKetNoi();
            cm.Dispose();
        }

        public void Trigger()
        {
            KetNoi();
            SqlCommand cmd = new SqlCommand(); //Đối tượng để thực hiện lệnh
            cmd.CommandText = "create or alter trigger setSiSo on HocVien for insert, update, delete as " +
                "begin " +
                "declare @malop nvarchar(50) " +
				"declare @malop2 nvarchar(50) " +
				"select @malop = MaLop from inserted " +
				"select @malop2 = MaLop from deleted " +
				"update LopHoc set SiSo = (select count(MaHocVien) from HocVien join LopHoc " +
                "on HocVien.MaLop = LopHoc.MaLop " +
                "where HocVien.MaLop = @malop) " +
                "where MaLop = @malop " +
				"update LopHoc set SiSo = (select count(MaHocVien) from HocVien join LopHoc " +
				"on HocVien.MaLop = LopHoc.MaLop " +
				"where HocVien.MaLop = @malop2) " +
				"where MaLop = @malop2 " +
				"end";
            cmd.Connection = con;
            try
            {
                cmd.ExecuteNonQuery(); //Thực hiện câu lệnh
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            DongKetNoi();
        }
    }
}
