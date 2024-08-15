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
namespace web4.Controllers
{
    public class BaoCaoDoanhThuGVController : Controller
    {
        SqlConnection con = new SqlConnection();
       
        // GET: BaoCaoDoanhThuGV
        public ActionResult BaoCaoDoanhThuGV_Fill()
        {
            return View();
        }
        public ActionResult BaoCaoDoanhThuGV()
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);
            var dvcs = Request.Cookies["MA_DVCS"].Value;
           
            var Fromdate = Request.Cookies["From_date"].Value;
            var Todate = Request.Cookies["To_date"].Value;
            var user = Request.Cookies["UserName"].Value;
            string Pname = "[usp_BaoCaoDoanhThuGV_LINEITEM_SAP]";
          if(user == "opctckt001" || user == "Admin"||user =="vpct-nhatanh")
            {
                 dvcs = Request.Cookies["MA_DVCS_2"].Value;
            }
            ViewBag.ProcedureName = Pname;

            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
            
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {

                    cmd.Parameters.AddWithValue("@_Tu_Ngay", Fromdate);
                    cmd.Parameters.AddWithValue("@_Den_Ngay", Todate);
                    cmd.Parameters.AddWithValue("@_ma_dvcs", dvcs);
                   
                 

                    sda.Fill(ds);

                }

            }
            return View(ds);
        }
    }
}