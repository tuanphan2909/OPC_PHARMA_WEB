using DevExpress.Office.Import.OpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web4.Models;
using static web4.Controllers.SERVERController;
namespace web4.Controllers
{
    public class BaoCaoDoanhThuGamHangTheoDoiTuongController : Controller
    {

        SqlConnection con = new SqlConnection();
        
        public ActionResult Index()
        {
            return View();
        }
    

        public ActionResult BaoCaoDoanhThuGamHangTheoDoiTuong(Account Acc)
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);
            string Pname = "[usp_BaoCaoDoanhThuGamHang_SAP]";
            Acc.UserName = Response.Cookies["UserName"].Value;

            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                Acc.Ma_DvCs_1 = Request.Cookies["MA_DVCS"].Value;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {

                    cmd.Parameters.AddWithValue("@_Tu_Ngay", Acc.From_date);
                    cmd.Parameters.AddWithValue("@_Den_Ngay", Acc.To_date);
                    cmd.Parameters.AddWithValue("@_ma_dvcs", Acc.Ma_DvCs_1);
                    sda.Fill(ds);

                }
            }


            return View(ds);
        }
        public ActionResult BaoCaoDoanhThuGamHangTheoDoiTuong_Fill()
        {
            return View();
        }

    }
}