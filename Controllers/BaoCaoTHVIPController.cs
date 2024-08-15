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
    public class BaoCaoTHVIPController : Controller
    {
        // GET: BaoCaoTHVIP
        SqlConnection con = new SqlConnection();

   
        public ActionResult BaoCaoTTTHVIP_Fill()
        {
            return View();
        }
        public ActionResult BaoCaoTTTHVIP()
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);
            string Ma_Dvcs = Request.Cookies["MA_DVCS"].Value;
            string From_date = Request.Cookies["From_date"].Value;
            string To_date = Request.Cookies["To_date"].Value;
            string Pname = "[usp_BaoCaoTinhHinhTHVIP_SAP]";

          

            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
               

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {

                    cmd.Parameters.AddWithValue("@_Tu_Ngay", From_date);
                    cmd.Parameters.AddWithValue("@_Den_Ngay", To_date);
                    cmd.Parameters.AddWithValue("@_ma_dvcs", Ma_Dvcs);
                 
                    sda.Fill(ds);

                }
            }


            return View(ds);

        }
    }
}