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
using static web4.Controllers.LoadDataController;
namespace web4.Controllers
{
    public class BangKeHoaDonController : Controller
    {
        SqlConnection con = new SqlConnection();
 
        // GET: BangKeHoaDon
        public ActionResult Index()
        {
            return View();
        }
     
        public ActionResult bangkehoadon(Account Acc)
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);
            Acc.Ma_DvCs_1 = Request.Cookies["MA_DVCS"].Value;
            Acc.UserName = Request.Cookies["UserName"].Value;
            string Pname = "[usp_BKSaleOrder_SAP]";

            Acc.UserName = Request.Cookies["UserName"].Value;

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
                    cmd.Parameters.AddWithValue("@_username", Acc.UserName);
                    sda.Fill(ds);

                }
            }


            return View(ds);
           

        }
        public ActionResult bangkehoadon_Fill()
        {
            return View();
        }


        public ActionResult danhsachhoadon_SAP(Account Acc)
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);
            List<MauInChungTu> dmDlist = LoadData.LoadDmDt("", Request);
            List<BKHoaDonGiaoHang> dmDlistVT = LoadData.LoadDmVt();
            List<BKHoaDonGiaoHang> dmDlistTDV = LoadData.LoadDmTDV(Request);
            ViewBag.DataItems = dmDlist;
            ViewBag.DataTDV = dmDlistTDV;
            ViewBag.DataVT = dmDlistVT;
            var Ma_TDV = Request.Cookies["Ma_TDV"].Value;
            var Ma_Dt = Request.Cookies["Ma_Dt"].Value;
            var Ma_Vt = Request.Cookies["Ma_Vt"].Value;
            string Pname = "[usp_DanhSachHoaDon_SAP]";
          
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
                    cmd.Parameters.AddWithValue("@_Ma_Dt",Ma_Dt);
                    cmd.Parameters.AddWithValue("@_Ma_CbNv", Ma_TDV);
                    cmd.Parameters.AddWithValue("@_Tinh_Trang", Acc.Tinh_Trang);
                    cmd.Parameters.AddWithValue("@_username", Acc.UserName);
                    //cmd.Parameters.AddWithValue("@_Ma_DvBh", Ma_DvBh);

                    sda.Fill(ds);

                }
            }


            return View(ds);

        }
                           
        public ActionResult danhsachhoadon_SAP_CN(Account Acc)
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);

            //Acc.UserName = Request.Cookies["UserName"].Value;
            //string query = "exec usp_Vth_BC_BHCNTK_CN @_ngay_Ct1 = '" + Acc.From_date + "',@_Ngay_Ct2 ='"+ Acc.To_date+"',@_Ma_Dvcs='"+ Acc.Ma_DvCs_1+"'";
            string Pname = "[usp_DanhSachHoaDon_SAP]";
            //var Ma_DvBh = Request.Cookies["Ma_DvBh"].Value;
            Acc.UserName = Request.Cookies["UserName"].Value;

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
                    cmd.Parameters.AddWithValue("@_Ma_Dt", Acc.Ma_dt);
                    cmd.Parameters.AddWithValue("@_Tinh_Trang", Acc.Tinh_Trang);
                    cmd.Parameters.AddWithValue("@_username", Acc.UserName);
                    //cmd.Parameters.AddWithValue("@_Ma_DvBh", Ma_DvBh);
                    sda.Fill(ds);

                }
            }
            

            return View(ds);

        }
       

      

       
        public ActionResult DanhSachHoaDon_Fill()
        {
            List<MauInChungTu> dmDlist = LoadData.LoadDmDt("", Request);
            List<BKHoaDonGiaoHang> dmDlistTDV = LoadData.LoadDmTDV(Request);
            List<BKHoaDonGiaoHang> dmDlistVT = LoadData.LoadDmVt();
            ViewBag.DataItems = dmDlist;
            ViewBag.DataTDV = dmDlistTDV;
            ViewBag.DataVT = dmDlistVT;
            return View();
        }
        public ActionResult DanhSachHoaDon_Fill_HCM()
        {
            return View();
        }
        public ActionResult bangkehoadon_Fill_HCM()
        {
            return View();
        }
    }
}