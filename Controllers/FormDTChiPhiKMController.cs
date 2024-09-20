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
    public class FormDTChiPhiKMController : Controller
    {
        SqlConnection con = new SqlConnection();
        // GET: FormDTChiPhiKM
        public ActionResult Index()
        {

            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);

     
            //Acc.UserName = Request.Cookies["UserName"].Value;
            //string query = "exec usp_Vth_BC_BHCNTK_CN @_ngay_Ct1 = '" + Acc.From_date + "',@_Ngay_Ct2 ='"+ Acc.To_date+"',@_Ma_Dvcs='"+ Acc.Ma_DvCs_1+"'";
            string Pname = "DS_DuTruDTChiPhiKM_OTC";


            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;


                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {

                  

                    sda.Fill(ds);

                }
            }


            return View(ds);
        }
        public ActionResult Insert()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Save(List<DT_ChiPhi_KM> CPKMList)
        {
            string result = "Error!";

            if (CPKMList == null || !CPKMList.Any())
            {
                return Json("Error! Dữ liệu không hợp lệ.", JsonRequestBehavior.AllowGet);
            }

            try
            {
                SqlConnectionHelper.ConnectSQL(con);

                using (var connection = new SqlConnection(con.ConnectionString))
                {
                    connection.Open();
                    Guid rowId = Guid.NewGuid();
                    foreach (var CPKM in CPKMList)
                    {
                        using (var command = new SqlCommand("InsertDTChiPhiKM_OTC_SAP", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@_RowId", rowId);
                            command.Parameters.AddWithValue("@_Ngay_Ct", CPKM.Ngay_Ct);
                            command.Parameters.AddWithValue("@_So_Ct", CPKM.So_Ct);
                            command.Parameters.AddWithValue("@_Ma_Dvcs", CPKM.Ma_Dvcs);
                            command.Parameters.AddWithValue("@_Ma_CTKM", CPKM.Ma_CTKM);
                            command.Parameters.AddWithValue("@_Ten_CTKM", CPKM.Ten_CTKM);
                            command.Parameters.AddWithValue("@_Doanh_Thu", CPKM.Doanh_Thu);
                            command.Parameters.AddWithValue("@_CPKM", CPKM.CPKM);
                            command.Parameters.AddWithValue("@_Ty_Le", CPKM.Ty_Le);
                            command.Parameters.AddWithValue("@_Active", "1");

                            command.ExecuteNonQuery();
                        }
                    }

                    result = "Success! Đã Lưu";
                }
            }
            catch (SqlException sqlEx)
            {
                result = $"SQL Error! {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                result = $"Error! {ex.Message}";
            }

            if (result.StartsWith("Success"))
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new HttpStatusCodeResult(500, result);
            }
        }
        public ActionResult Edit()
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);
          

            string Pname = "[EditDTChiPhiKM_OTC_SAP]";
            string RowId = Request.QueryString["RowId"];
            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 50;
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

               

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@_RowId", RowId);
                    sda.Fill(ds);
                }
            }

            return View(ds);
        }
        public ActionResult Update(List<DT_ChiPhi_KM> CPKMList)
        {
            string result = "Error!";
            var rowId = Request.Cookies["RowId"]?.Value;

            if (CPKMList == null || !CPKMList.Any() || string.IsNullOrEmpty(rowId))
            {
                return Json("Error! Dữ liệu không hợp lệ hoặc RowId không có.", JsonRequestBehavior.AllowGet);
            }

            try
            {
                SqlConnectionHelper.ConnectSQL(con);

                using (var connection = new SqlConnection(con.ConnectionString))
                {
                    connection.Open();
                    Guid rowId2 = Guid.NewGuid();
                    foreach (var CPKM in CPKMList)
                    {
                        using (var command = new SqlCommand("UpdateDTChiPhiKM_OTC_SAP", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@_RowId", rowId2);
                            command.Parameters.AddWithValue("@_RowId_Old", rowId);
                            command.Parameters.AddWithValue("@_Ngay_Ct", CPKM.Ngay_Ct);
                            command.Parameters.AddWithValue("@_So_Ct", CPKM.So_Ct);
                            command.Parameters.AddWithValue("@_Ma_Dvcs", CPKM.Ma_Dvcs);
                            command.Parameters.AddWithValue("@_Ma_CTKM", CPKM.Ma_CTKM);
                            command.Parameters.AddWithValue("@_Ten_CTKM", CPKM.Ten_CTKM);
                            command.Parameters.AddWithValue("@_Doanh_Thu", CPKM.Doanh_Thu);
                            command.Parameters.AddWithValue("@_CPKM", CPKM.CPKM);
                            command.Parameters.AddWithValue("@_Ty_Le", CPKM.Ty_Le);
                            command.Parameters.AddWithValue("@_Active", "1");

                            command.ExecuteNonQuery();
                        }
                    }

                    result = "Success! Đã Lưu";
                }

                // If the update is successful, call the Delete method with the same RowId from the cookie
                if (result.StartsWith("Success"))
                {
                    var deleteResult = Delete(new DT_ChiPhi_KM { RowId = rowId });

                    var deleteContent = (deleteResult as ContentResult)?.Content;

                    if (!(deleteContent != null && deleteContent.Contains("Success")))
                    {
                        return new HttpStatusCodeResult(500, deleteContent);
                    }


                }
            }
            catch (SqlException sqlEx)
            {
                result = $"SQL Error! {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                result = $"Error! {ex.Message}";
            }

            if (result.StartsWith("Success"))
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new HttpStatusCodeResult(500, result);
            }
        }
        //public ActionResult Delete_Update(DT_ChiPhi_KM CPKM)
        //{
        //    string result = "Error!";
        //    SqlConnectionHelper.ConnectSQL(con);
        //    try
        //    {
        //        using (var connection = new SqlConnection(con.ConnectionString))
        //        {
        //            connection.Open();
        //            using (var command = new SqlCommand("[Delete_DTChiPhiKM_OTC]", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.AddWithValue("@_RowId", CPKM.RowId);
        //                command.ExecuteNonQuery();
        //                result = "Success! Đã Xoá";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = $"Error! {ex.Message}";
        //        // Ghi log chi tiết lỗi nếu cần
        //    }

        //    return Content(result);
        //}
        public ActionResult Delete(DT_ChiPhi_KM CPKM)
        {
            string result = "Error!";
            SqlConnectionHelper.ConnectSQL(con);
            try
            {
                using (var connection = new SqlConnection(con.ConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("[Delete_DTChiPhiKM_OTC]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@_RowId", CPKM.RowId);
                        command.ExecuteNonQuery();
                        result = "Success! Đã Xoá";
                    }
                }
            }
            catch (Exception ex)
            {
                result = $"Error! {ex.Message}";
                // Ghi log chi tiết lỗi nếu cần
            }

            return Content(result);
        }
        public ActionResult DinhChi(DT_ChiPhi_KM CPKM)
        {
            string result = "Error!";
            SqlConnectionHelper.ConnectSQL(con);
            try
            {
                using (var connection = new SqlConnection(con.ConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("[DinhChi_DTChiPhiKM_OTC]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@_RowId", CPKM.RowId);
                        command.ExecuteNonQuery();
                        result = "Success! Đã Xoá";
                    }
                }
            }
            catch (Exception ex)
            {
                result = $"Error! {ex.Message}";
                // Ghi log chi tiết lỗi nếu cần
            }

            return Content(result);
        }

    }
}