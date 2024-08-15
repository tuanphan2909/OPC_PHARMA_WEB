using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using web4.Models;
using System.Web.Mvc;
using static web4.Controllers.SERVERController;
namespace web4.Controllers
{
    public class BaoCaoBanHangTDVController : Controller
    {
        SqlConnection con = new SqlConnection();
        // GET: BangKeHoaDon
        public ActionResult Index()
        {
            return View();
        }
      
        public ActionResult BaoCaoBanHangTDV(Account Acc)
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);
            string Pname = "[usp_BaoCaoBanHangTDV_SAP]";
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
        public ActionResult BaoCaoBanHangTDV_Fill(Account Acc)
        {
            return View();
        }
        public ActionResult BaoCaoBanHangTDV_Nt(Account Acc)
        {


            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);
           
            string Pname = "[usp_BaoCaoBanHangTDVNT_SAP]";
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
        public ActionResult BaoCaoBanHangTDV_Nt_Fill(Account Acc)
        {
            return View();
        }
        public ActionResult BaoCaoBanHangTDV_DB_Fill()
        {
            return View();
        }
        public ActionResult BaoCaoBanHangTDV_DB(Account Acc)
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);
         
            string Pname = "[usp_BaoCaoBanHangTDV_SAP]";
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
    }
}