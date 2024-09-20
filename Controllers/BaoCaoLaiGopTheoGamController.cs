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
    public class BaoCaoLaiGopTheoGamController : Controller
    {
        SqlConnection con = new SqlConnection();
        // GET: BaoCaoLaiGopTheoGam
        public ActionResult BCQT_LaiGopTheoGam_Fill()
        {
            return View();
        }
        public ActionResult BCQT_LaiGopTheoGam(Account Acc)
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);
            string Pname = "[usp_BCQT_LaiGopTheoGam_SAP]";
            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@_Tu_Ngay", Acc.From_date);
                    cmd.Parameters.AddWithValue("@_Den_Ngay",Acc.To_date);
               
                    sda.Fill(ds);

                }
            }


            return View(ds);
        }
    }
}