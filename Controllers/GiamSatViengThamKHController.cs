using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using web4.Models;
using System.Web.Mvc;

namespace web4.Controllers
{
    public class GiamSatViengThamKHController : Controller
    {
        // GET: GiamSatViengThamKH
        SqlConnection con = new SqlConnection();
        SqlCommand sqlc = new SqlCommand();
      
        // GET: BaoCaoTienVeCN
        public ActionResult Index()
        {
            return View();
        }
        public void connectSQL()
        {
            con.ConnectionString = "Data source= " + "118.69.109.109" + ";database=" + "SAP_OPC" + ";uid=OPC;password=OpcaBc@135#";
        }
        public ActionResult GiamSatViengThamKH_Fill()
        {
            return View();
        }
        public ActionResult GiamSatViengThamKH()
        {
            DataSet ds = new DataSet();
            connectSQL();
            string MaCbNv = Request.Cookies["Ma_CbNv"].Value;
            string Dvcs = Request.Cookies["MA_DVCS"].Value;
            string From_date = Request.QueryString["From_date"];
            string To_date = Request.QueryString["To_date"];
            //string query = "exec usp_Vth_BC_BHCNTK_CN @_ngay_Ct1 = '" + Acc.From_date + "',@_Ngay_Ct2 ='"+ Acc.To_date+"',@_Ma_Dvcs='"+ Acc.Ma_DvCs_1+"'";
            string Pname = "[usp_GiamSatViengThamKH_Android]";
            switch (Dvcs)
            {
                case "OPC_TP":
                    Dvcs = "A02"; break;
                case "OPC_CT":
                    Dvcs = "A03"; break;
                case "OPC_TG":
                    Dvcs = "A04"; break;
                case "OPC_MD":
                    Dvcs = "A05"; break;
                case "OPC_VT":
                    Dvcs = "A06"; break;
                case "OPC_NT":
                    Dvcs = "A07"; break;
                case "OPC_DN":
                    Dvcs = "A08"; break;
                case "OPC_NA":
                    Dvcs = "A09"; break;
                case "OPC_HN":
                    Dvcs = "A10"; break;

            }
            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@_Ma_CbNv", MaCbNv);
                    cmd.Parameters.AddWithValue("@_Ma_Dvcs", Dvcs);
                    cmd.Parameters.AddWithValue("@_Ngay_Ct1", From_date);
                    cmd.Parameters.AddWithValue("@_Ngay_Ct2", To_date);
                    sda.Fill(ds);

                }
            }
            return View(ds);
        }
    }
}