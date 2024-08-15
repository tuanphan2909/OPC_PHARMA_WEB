using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using web4.Models;
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
    public class ChiTietSPTraThuongController : Controller
    {
        // GET: ChiTietSPTraThuong
        SqlConnection con = new SqlConnection();
        public ActionResult ChiTietSPTraThuong_Fill()
        {
            return View();
        }
        public ActionResult ChiTietSPTraThuong()
        {
           
            string ma_dvcs;
            var fromDate = Request.Cookies["From_date"].Value;
            var toDate = Request.Cookies["To_Date"].Value;
            if (Request.Cookies["Ma_Dvcs_2"] != null)
            {
                ma_dvcs = Request.Cookies["Ma_Dvcs_2"].Value;
            }
            else
            {
                ma_dvcs = Request.Cookies["MA_DVCS"].Value;
            }
          
            DataSet ds = new DataSet();

            SqlConnectionHelper.ConnectSQL(con);

            string Pname = "[Usp_DanhSachTrungBay_Detail]";

            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@_Ngay_Ct1", fromDate);
                    cmd.Parameters.AddWithValue("@_Ngay_Ct2", toDate);
                    cmd.Parameters.AddWithValue("@_ma_dvcs", ma_dvcs);
                    sda.Fill(ds);

                }
            }
            return View(ds);
        }
    }
}