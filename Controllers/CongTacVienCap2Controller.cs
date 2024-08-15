using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using web4.Models;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using System.Data.Entity.Validation;
using static web4.Controllers.SERVERController;
using static web4.Controllers.LoadDataController;
namespace web4.Controllers
{
    public class CongTacVienCap2Controller : Controller
    {
        SqlConnection con = new SqlConnection();

        // GET: BaoCaoTienVeCN


      

        // GET: CongTacVien
        public ActionResult index()
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);
            string Ma_DvCs = Request.Cookies["MA_DVCS"].Value;
            //Acc.UserName = Request.Cookies["UserName"].Value;
            //string query = "exec usp_Vth_BC_BHCNTK_CN @_ngay_Ct1 = '" + Acc.From_date + "',@_Ngay_Ct2 ='"+ Acc.To_date+"',@_Ma_Dvcs='"+ Acc.Ma_DvCs_1+"'";
            string Pname = "Danhsach_CTV_Cap2";


            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;


                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {

                    cmd.Parameters.AddWithValue("@_Ma_Dvcs", Ma_DvCs);

                    sda.Fill(ds);

                }
            }


            return View(ds);
        }
       
       
        public ActionResult InputCTVCap2()
        {
            List<CTVCap2> dmDlist = LoadData.LoadDmDtCap2("",Request);
            List<BKHoaDonGiaoHang> DmVt = LoadData.LoadDmVt();

            ViewBag.DataItems = dmDlist;
            ViewBag.DataItems2 = DmVt;

            return View();
        }


       
        public ActionResult SaveCtV(CTVCap2 CTV)
        {
            string result = "Error!";
            SqlConnectionHelper.ConnectSQL(con);
            if (CTV != null && CTV.Details != null)
            {
                try
                {
                    var detailsTable = new DataTable();
                    detailsTable.Columns.Add("Ma_vt", typeof(string));
                    detailsTable.Columns.Add("Ten_Vt", typeof(string));
                    detailsTable.Columns.Add("So_Luong", typeof(int));
                    detailsTable.Columns.Add("Don_Gia", typeof(decimal));
                    detailsTable.Columns.Add("Thanh_Tien", typeof(decimal));


                    foreach (var detail in CTV.Details)
                    {
                        detailsTable.Rows.Add(detail.Ma_vt, detail.Ten_Vt, detail.So_Luong, detail.Don_Gia,detail.Thanh_Tien);
                    }

                    using (var connection = new SqlConnection(con.ConnectionString))
                    {
                        connection.Open();

                        using (var command = new SqlCommand("[InsertCongTacVienCap2_SAP]", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@_Ngay_Ct", CTV.Ngay_Ct);
                            command.Parameters.AddWithValue("@_so_Ct", CTV.So_Ct);
                            command.Parameters.AddWithValue("@_Ten_Dt", CTV.Ten_Dt);
                            command.Parameters.AddWithValue("@_Dvcs", CTV.Dvcs);
                            command.Parameters.AddWithValue("@_Ma_Dt", CTV.Ma_Dt);
                            command.Parameters.AddWithValue("@_Thue", CTV.Thue);
                            command.Parameters.AddWithValue("@_Tien_Thue", CTV.Tien_Thue);
                            command.Parameters.AddWithValue("@_Tien_Truoc_Thue", CTV.Tien_Truoc_Thue);
                            command.Parameters.AddWithValue("@_Tien_Sau_Thue", CTV.Tien_Sau_Thue);

                            // Pass details as a TVP parameter
                            var detailsParam = command.Parameters.AddWithValue("@_Details", detailsTable);
                            detailsParam.SqlDbType = SqlDbType.Structured;
                            detailsParam.TypeName = "B30CTVCap2DetailType"; // Replace with your actual type name

                            command.ExecuteNonQuery();

                            result = "Success! Đã Lưu";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately
                    result = $"Error! {ex.Message}";
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateCTV(CTVCap2 CTV)
        {
            string result = "Error!";
            SqlConnectionHelper.ConnectSQL(con);
            if (CTV != null && CTV.Details != null)
            {
                try
                {
                    var detailsTable = new DataTable();
                    detailsTable.Columns.Add("Ma_vt", typeof(string));
                    detailsTable.Columns.Add("Ten_Vt", typeof(string));
                    detailsTable.Columns.Add("So_Luong", typeof(int));
                    detailsTable.Columns.Add("Don_Gia", typeof(decimal));
                    detailsTable.Columns.Add("Thanh_Tien", typeof(decimal));

                    foreach (var detail in CTV.Details)
                    {
                        detailsTable.Rows.Add(detail.Ma_vt, detail.Ten_Vt, detail.So_Luong,detail.Don_Gia,detail.Thanh_Tien);
                    }

                    using (var connection = new SqlConnection(con.ConnectionString))
                    {
                        connection.Open();

                        using (var command = new SqlCommand("UpdateCongTacVienCap2_SAP", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@_Ngay_Ct", CTV.Ngay_Ct);
                            command.Parameters.AddWithValue("@_so_Ct", CTV.So_Ct);
                            command.Parameters.AddWithValue("@_Ten_Dt", CTV.Ten_Dt);
                            command.Parameters.AddWithValue("@_Dvcs", CTV.Dvcs);
                            command.Parameters.AddWithValue("@_Ma_Dt", CTV.Ma_Dt);
                            command.Parameters.AddWithValue("@_Thue", CTV.Thue);
                            command.Parameters.AddWithValue("@_Tien_Thue", CTV.Tien_Thue);
                            command.Parameters.AddWithValue("@_Tien_Truoc_Thue", CTV.Tien_Truoc_Thue);
                            command.Parameters.AddWithValue("@_Tien_Sau_Thue", CTV.Tien_Sau_Thue);
                            command.Parameters.AddWithValue("@_CTVId", CTV.CTVId);

                            // Pass details as a TVP parameter
                            var detailsParam = command.Parameters.AddWithValue("@_Details", detailsTable);
                            detailsParam.SqlDbType = SqlDbType.Structured;
                            detailsParam.TypeName = "B30CTVCap2DetailType"; // Replace with your actual type name

                            command.ExecuteNonQuery();

                            result = "Success! Đã Sửa";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately
                    result = $"Error! {ex.Message}";
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditCTVCap2()
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);
            List<CTVCap2> dmDlist = LoadData.LoadDmDtCap2("",Request);
            List<BKHoaDonGiaoHang> DmVt = LoadData.LoadDmVt();

            ViewBag.DataItems = dmDlist;
            ViewBag.DataItems2 = DmVt;

            string Pname = "[EditHanMucCTVCap2]";
            string ctvId = Request.QueryString["CTVId"];
            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;


                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {

                    cmd.Parameters.AddWithValue("@_CTVId", ctvId);
                    sda.Fill(ds);

                }
            }


            return View(ds);
        }

        public ActionResult CoppyCTVCap2()
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);
            List<CTVCap2> dmDlist = LoadData.LoadDmDtCap2("",Request);
            List<BKHoaDonGiaoHang> DmVt = LoadData.LoadDmVt();

            ViewBag.DataItems = dmDlist;
            ViewBag.DataItems2 = DmVt;

            string Pname = "[EditHanMucCTVCap2]";
            string ctvId = Request.QueryString["CTVId"];
            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;


                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {

                    cmd.Parameters.AddWithValue("@_CTVId", ctvId);
                    sda.Fill(ds);

                }
            }


            return View(ds);
        }
    }


}