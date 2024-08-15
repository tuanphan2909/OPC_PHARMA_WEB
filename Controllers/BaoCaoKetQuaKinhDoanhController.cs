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
    public class BaoCaoKetQuaKinhDoanhController : Controller
    {
        SqlConnection con = new SqlConnection();
        // GET: BaoCaoKetQuaKinhDoanh
    
        public ActionResult BaoCaoKetQuaKinhDoanh_Fill()
        {
        
            return View();
        }
        public ActionResult BaoCaoKetQuaKinhDoanh()
        {
         
            string ma_dvcs = Request.Cookies["MA_DVCS"].Value;
            var fromDate = Request.Cookies["From_date"].Value;
            var toDate = Request.Cookies["To_Date"].Value;
            var MaDt = Request.Cookies["Ma_Dt"] != null ? Request.Cookies["Ma_Dt"].Value : string.Empty;

            DataSet ds = new DataSet();
        
            SqlConnectionHelper.ConnectSQL(con);
            string Pname = "[usp_BaoCaoKetQuaKinhDoanh_SAP]";

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
                    //cmd.Parameters.AddWithValue("@_Ma_CbNv", MaTDV);
                    cmd.Parameters.AddWithValue("@_Ma_DvCs", ma_dvcs);
                    sda.Fill(ds);

                }
            }
            return View(ds);
        }
     

      
    }
}