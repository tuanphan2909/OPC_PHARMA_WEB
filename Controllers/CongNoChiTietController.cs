using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web4.Models;
using System.Web.Mvc;
using System.Net;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using OfficeOpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using OfficeOpenXml.Table;
using Newtonsoft.Json;
using System.Globalization;
using static web4.Controllers.SERVERController;
using static web4.Controllers.LoadDataController;
namespace web4.Controllers
{
    public class CongNoChiTietController : Controller
    {
       
        SqlConnection con = new SqlConnection();

        public ActionResult CongNoChiTietKH_Fill()
        {
            string ma_dvcs = Request.Cookies["MA_DVCS"] != null ? Request.Cookies["MA_DVCS"].Value : string.Empty;
            List<MauInChungTu> dmDlist = LoadData.LoadDmDt("",Request);
            List<BKHoaDonGiaoHang> dmDlistTDV = LoadData.LoadDmTDV(Request);
            ViewBag.DataItems = dmDlist;
            ViewBag.DataTDV = dmDlistTDV;
            return View();
        }
        public ActionResult CongNoChiTietKH()
        {
            List<MauInChungTu> dmDlist = LoadData.LoadDmDt("",Request);
            List<BKHoaDonGiaoHang> dmDlistTDV = LoadData.LoadDmTDV(Request);
            string ma_dvcs = Request.Cookies["MA_DVCS"].Value;
            var fromDate = Request.Cookies["From_date"].Value;
            var toDate = Request.Cookies["To_Date"].Value;
            var MaDt = Request.Cookies["Ma_Dt"] != null ? Request.Cookies["Ma_Dt"].Value : string.Empty;
            var MaTDV = Request.Cookies["Ma_TDV"].Value;
            DataSet ds = new DataSet();
            if (ma_dvcs == "OPC_B1")
            {
                string ma_dvcsFirst3Chars = ma_dvcs == "OPC_B1" ? ma_dvcs.Substring(0, 3) : ma_dvcs;
                string first3Chars = ma_dvcsFirst3Chars.Substring(0, 3);
                ma_dvcs = first3Chars;
            }
            ViewBag.DataTDV = dmDlistTDV;
            ViewBag.DataItems = dmDlist;
            SqlConnectionHelper.ConnectSQL(con);
       
            string Pname = "[usp_RpCongNoPL5_SAP]";

            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@_Tu_Ngay", fromDate);
                    cmd.Parameters.AddWithValue("@_Den_Ngay", toDate);
                    cmd.Parameters.AddWithValue("@_Ma_Dt", MaDt);
                    cmd.Parameters.AddWithValue("@_Ma_CbNv", MaTDV);
                    cmd.Parameters.AddWithValue("@_Ma_DvCs", ma_dvcs);
                    sda.Fill(ds);

                }
            }
            return View(ds);
        }
        public ActionResult CongNoChiTietKH_ETC_Fill()
        {
            string ma_dvcs = Request.Cookies["MA_DVCS"] != null ? Request.Cookies["MA_DVCS"].Value : string.Empty;
            List<MauInChungTu> dmDlist = LoadData.LoadDmDt("", Request);
            List<BKHoaDonGiaoHang> dmDlistTDV = LoadData.LoadDmTDV(Request);
            ViewBag.DataItems = dmDlist;
            ViewBag.DataTDV = dmDlistTDV;
            return View();
        }
        public ActionResult CongNoChiTietKH2_Fill()
        {

            return View();
        }
        public ActionResult CongNoChiTietKH2_Index()
        {
            string ma_dvcs = Request.Cookies["MA_DVCS_2"] != null ? Request.Cookies["MA_DVCS_2"].Value : string.Empty;
            List<MauInChungTu> dmDlist = LoadData.LoadDmDt2(Request);
            List<BKHoaDonGiaoHang> dmDlistTDV = LoadData.LoadDmTDV2(Request);
            ViewBag.DataItems = dmDlist;
            ViewBag.DataTDV = dmDlistTDV;
            return View();
        }
        public ActionResult Index()
        {
            List<MauInChungTu> dmDlist = LoadData.LoadDmDt2(Request);
            List<BKHoaDonGiaoHang> dmDlistTDV = LoadData.LoadDmTDV2(Request);
            ViewBag.DataTDV = dmDlistTDV;
            ViewBag.DataItems = dmDlist;
           
            return View();
        }
        public ActionResult CongNoChiTietKH2_Sub()
        {
            return View();
        }
        public ActionResult CongNoChiTietKH2()
        {
            List<MauInChungTu> dmDlist = LoadData.LoadDmDt2(Request);
            List<BKHoaDonGiaoHang> dmDlistTDV = LoadData.LoadDmTDV2(Request);
            string ma_dvcs = Request.Cookies["MA_DVCS_2"].Value;
            var fromDate = Request.Cookies["From_date"].Value;
            var toDate = Request.Cookies["To_Date"].Value;
            var MaDt = Request.Cookies["Ma_Dt"] != null ? Request.Cookies["Ma_Dt"].Value : string.Empty;
            var MaTDV = Request.Cookies["Ma_TDV"].Value;
            DataSet ds = new DataSet();
            if (ma_dvcs == "OPC_B1")
            {
                string ma_dvcsFirst3Chars = ma_dvcs == "OPC_B1" ? ma_dvcs.Substring(0, 3) : ma_dvcs;
                string first3Chars = ma_dvcsFirst3Chars.Substring(0, 3);
                ma_dvcs = first3Chars;
            }
            ViewBag.DataTDV = dmDlistTDV;
            ViewBag.DataItems = dmDlist;
            SqlConnectionHelper.ConnectSQL(con);
    
            string Pname = "[usp_RpCongNoPL5_SAP]";

            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@_Tu_Ngay", fromDate);
                    cmd.Parameters.AddWithValue("@_Den_Ngay", toDate);
                    cmd.Parameters.AddWithValue("@_Ma_Dt", MaDt);
                    cmd.Parameters.AddWithValue("@_Ma_CbNv", MaTDV);
                    cmd.Parameters.AddWithValue("@_Ma_DvCs", ma_dvcs);
                    sda.Fill(ds);

                }
            }
            return View(ds);
        }
        public ActionResult CongNoChiTietKH_ETC()
        {
            List<MauInChungTu> dmDlist = LoadData.LoadDmDt("", Request);
            List<BKHoaDonGiaoHang> dmDlistTDV = LoadData.LoadDmTDV(Request);
            string ma_dvcs = Request.Cookies["MA_DVCS"].Value;
            var fromDate = Request.Cookies["From_date"].Value;
            var toDate = Request.Cookies["To_Date"].Value;
            var MaDt = Request.Cookies["Ma_Dt"] != null ? Request.Cookies["Ma_Dt"].Value : string.Empty;
            var MaTDV = Request.Cookies["Ma_TDV"].Value;
            DataSet ds = new DataSet();
            if (ma_dvcs == "OPC_B1")
            {
                string ma_dvcsFirst3Chars = ma_dvcs == "OPC_B1" ? ma_dvcs.Substring(0, 3) : ma_dvcs;
                string first3Chars = ma_dvcsFirst3Chars.Substring(0, 3);
                ma_dvcs = first3Chars;
            }
            ViewBag.DataTDV = dmDlistTDV;
            ViewBag.DataItems = dmDlist;
            SqlConnectionHelper.ConnectSQL(con);
     
            string Pname = "[usp_RpCongNoPL5_SAP]";

            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@_Tu_Ngay", fromDate);
                    cmd.Parameters.AddWithValue("@_Den_Ngay", toDate);
                    cmd.Parameters.AddWithValue("@_Ma_Dt", MaDt);
                    cmd.Parameters.AddWithValue("@_Ma_CbNv", MaTDV);
                    cmd.Parameters.AddWithValue("@_Ma_DvCs", ma_dvcs);
                    sda.Fill(ds);

                }
            }
            return View(ds);
        }
        public ActionResult CongNoChiTietKH_ETC_In()
        {
            
            string ma_dvcs = Request.Cookies["MA_DVCS"].Value;
            var fromDate = Request.Cookies["From_date"].Value;
            var toDate = Request.Cookies["To_Date"].Value;
            var MaDt = Request.Cookies["Ma_Dt"] != null ? Request.Cookies["Ma_Dt"].Value : string.Empty;
            var MaTDV = Request.Cookies["Ma_TDV"].Value;
            DataSet ds = new DataSet();
            if (ma_dvcs == "OPC_B1")
            {
                string ma_dvcsFirst3Chars = ma_dvcs == "OPC_B1" ? ma_dvcs.Substring(0, 3) : ma_dvcs;
                string first3Chars = ma_dvcsFirst3Chars.Substring(0, 3);
                ma_dvcs = first3Chars;
            }

            SqlConnectionHelper.ConnectSQL(con);
          
            string Pname = "[usp_RpCongNoPL5_SAP]";

            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@_Tu_Ngay", fromDate);
                    cmd.Parameters.AddWithValue("@_Den_Ngay", toDate);
                    cmd.Parameters.AddWithValue("@_Ma_Dt", MaDt);
                    cmd.Parameters.AddWithValue("@_Ma_CbNv", MaTDV);
                    cmd.Parameters.AddWithValue("@_Ma_DvCs", ma_dvcs);
                    sda.Fill(ds);

                }
            }
            return View(ds);
        }
      
       
    }
}