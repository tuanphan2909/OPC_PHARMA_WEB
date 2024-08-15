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
    public class ChiTietCongNoPhaiTraController : Controller
    {
        // GET: ChiTietCongNoPhaiTra
        SqlConnection con = new SqlConnection();

        public ActionResult ChiTietCongNoPhaiTra_Fill()
        {
            List<MauInChungTu> dmDlist = LoadData.LoadDmDt("", Request);
            ViewBag.DataItems = dmDlist;
            return View();
        }
      
        public ActionResult ChiTietCongNoPhaiTra()
        {
            List<MauInChungTu> dmDlist = LoadData.LoadDmDt("", Request);
            ViewBag.DataItems = dmDlist;
            var fromDate = Request.Cookies["From_date"].Value;
            var toDate = Request.Cookies["To_date"].Value;
            var Ma_Dt = Request.Cookies["Ma_Dt"].Value;
            var Ma_Dvcs = Request.Cookies["MA_DVCS"].Value;
            DataSet ds = new DataSet();

            string Pname = "[usp_ChiTietCongNoPhaiTraHTT_SAP]";
            SqlConnectionHelper.ConnectSQL(con);
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
                    cmd.Parameters.AddWithValue("@_Ma_Dt", Ma_Dt);
                    cmd.Parameters.AddWithValue("@_ma_dvcs", Ma_Dvcs);
                    sda.Fill(ds);
                }    
            }    
                return View(ds);
        }
    }
}