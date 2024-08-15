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
    public class BaoCaoBanHangCTVController : Controller
    {

        SqlConnection con = new SqlConnection();
        // GET: BaoCaoBanHangCTV
       
        public ActionResult BaoCaoDTTinhLuongCTV_Fill()
        {
            return View();
        }

       
        public ActionResult BaoCaoDTTinhLuongCTV(Account Acc)
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);
            string Pname = "[usp_BaoCaoBanHang_CTV]";
            var fromDate = Request.Cookies["From_date"].Value;
            var toDate = Request.Cookies["To_date"].Value;
            var Dvcs = Request.Cookies["MA_DVCS"].Value;
           

            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@_Tu_Ngay", fromDate);
                    cmd.Parameters.AddWithValue("@_Den_Ngay", toDate);
                    cmd.Parameters.AddWithValue("@_ma_dvcs", Dvcs);
                    sda.Fill(ds);

                }
            }
            return View(ds);
        }
        public ActionResult BaoCaoDTTinhLuongCTV_Fill_CongTy()
        {
            return View();
        }
        public ActionResult BaoCaoDTTinhLuongCTV_CongTy(Account Acc)
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);

            string Pname = "[usp_BaoCaoBanHang_CTV]";
            var Dvcs = Request.Cookies["MA_DVCS"].Value;


            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@_Tu_Ngay", Acc.From_date);
                    cmd.Parameters.AddWithValue("@_Den_Ngay", Acc.To_date);
                    cmd.Parameters.AddWithValue("@_ma_dvcs", Dvcs);
                    sda.Fill(ds);

                }
            }
            return View(ds);
        }
    }
}