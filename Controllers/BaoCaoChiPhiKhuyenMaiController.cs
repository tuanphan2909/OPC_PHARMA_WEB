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
    public class BaoCaoChiPhiKhuyenMaiController : Controller
    {
        // GET: BaoCaoChiPhiKhuyenMai
        SqlConnection con = new SqlConnection();

        public ActionResult BaoCaoChiPhiKM_Fill()
        {
            return View();
        }
        public ActionResult BaoCaoChiPhiKM(Account Acc)
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);

            string Pname = "[usp_BaoCaoChiPhiKhuyenMai_SAP]";
            ViewBag.ProcedureName = Pname;
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