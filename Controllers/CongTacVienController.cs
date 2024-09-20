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
using System.Threading.Tasks;

namespace web4.Controllers
{
    public class CongTacVienController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand sqlc = new SqlCommand();
        SqlDataReader dt;
        // GET: BaoCaoTienVeCN
       
                                                                                                                                                                                                                                                              
  

        // GET: CongTacVien
        public ActionResult index()
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);

            string Ma_DvCs = Request.Cookies["MA_DVCS"].Value;
            //Acc.UserName = Request.Cookies["UserName"].Value;
            //string query = "exec usp_Vth_BC_BHCNTK_CN @_ngay_Ct1 = '" + Acc.From_date + "',@_Ngay_Ct2 ='"+ Acc.To_date+"',@_Ma_Dvcs='"+ Acc.Ma_DvCs_1+"'";
            string Pname = "Danhsach_CTV";
          

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
        //public List<CTV> LoadDmDt(string Ma_dvcs)   
        //{
        //    SqlConnectionHelper.ConnectSQL(con);

        //    Ma_dvcs = Request.Cookies["ma_dvcs"].Value;
        //    List<CTV> dataItems = new List<CTV>();
        //    string appendedString = Ma_dvcs == "OPC" ? "_B1_0120" : "_01"; // Dòng này cộng chuỗi dựa trên giá trị của Ma_dvcs
        //    using (SqlConnection connection = new SqlConnection(con.ConnectionString))
        //    {
        //        connection.Open();

        //        using (SqlCommand command = new SqlCommand("[usp_DmDtTdv_SAP_MauIn]", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@_Ma_Dvcs", Ma_dvcs + "_01" );
        //            command.CommandTimeout = 950;
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    CTV dataItem = new CTV
        //                    {

        //                        Ma_Dt = reader["ma_dt"].ToString(),
        //                        Ten_Dt = reader["ten_dt"].ToString(),
        //                        Dvcs = reader["Dvcs"].ToString()

        //                    };
        //                    dataItems.Add(dataItem);
        //                }
        //            }
        //        }
        //    }

        //    return dataItems;
        //}

        public List<CTV> LoadDmVt()
        {
            SqlConnectionHelper.ConnectSQL(con);

            //Ma_dvcs = Request.Cookies["ma_dvcs"].Value;
            List<CTV> dataItems = new List<CTV>();
            //string appendedString = Ma_dvcs == "OPC_B1" ? "_010203" : "_01"; // Dòng này cộng chuỗi dựa trên giá trị của Ma_dvcs
            using (SqlConnection connection = new SqlConnection(con.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("[usp_DanhMucVt_SAP]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    // command.Parameters.AddWithValue("@_Ma_Dvcs", "OPC_HN_01");
                    command.CommandTimeout = 950;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CTV dataItem = new CTV
                            {

                                Ma_vt = reader["Ma_Vt"].ToString(),
                                Ten_Vt = reader["Ten_Vt"].ToString()



                            };
                            dataItems.Add(dataItem);
                        }
                    }
                }
            }

            return dataItems;
        }
        public ActionResult InputCTV()
        {
            List<MauInChungTu> dmDlist = LoadData.LoadDmDt("",Request);
            List<CTV> DmVt = LoadDmVt();
           
            ViewBag.DataItems = dmDlist;
            ViewBag.DataItems2 = DmVt;

            return View();
        }


        //public ActionResult SaveCtV(CTV CTV)
        //{

        //    connectSQL();
        //    using (SqlConnection connection = new SqlConnection(con.ConnectionString))
        //    {
        //        connection.Open();
        //        try
        //        {

        //            using (SqlCommand command = new SqlCommand("InsertB30CtvData", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.AddWithValue("@_Ngay_Ct", CTV.Ngay_Ct);
        //                command.Parameters.AddWithValue("@_so_Ct", CTV.So_Ct);
        //                command.Parameters.AddWithValue("@_Dvcs", CTV.Dvcs);
        //                command.Parameters.AddWithValue("@_Ma_Dt", CTV.Ma_Dt);
        //                command.Parameters.AddWithValue("@_Ma_vt", CTV.Ma_vt);
        //                command.Parameters.AddWithValue("@_Ten_Vt", CTV.Ten_Vt);
        //                command.Parameters.AddWithValue("@_Han_Muc", CTV.Han_Muc);

        //                command.ExecuteNonQuery();

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            if (con.State == ConnectionState.Open)
        //            {
        //                con.Close();
        //            }
        //            ex.Message.ToString();
        //        }
        //        return View();
        //    }

        //}
        public ActionResult SaveCtV(CTV CTV)
        {
            string result = "Error!";
            SqlConnectionHelper.ConnectSQL(con);
            if (CTV != null && CTV.Details != null)
            {
                try
                {
                    var detailsTable = new DataTable();
                    detailsTable.Columns.Add("Ma_vt", typeof(int));
                    detailsTable.Columns.Add("Ten_Vt", typeof(string));
                    detailsTable.Columns.Add("Ngay_Ct", typeof(string));
                    detailsTable.Columns.Add("Han_Muc", typeof(int));
                    detailsTable.Columns.Add("Ma_dvcs", typeof(string));
                    detailsTable.Columns.Add("Ma_Dt", typeof(string));

                    foreach (var detail in CTV.Details)
                    {
                        detailsTable.Rows.Add(detail.Ma_vt, detail.Ten_Vt,detail.Ngay_Ct1, detail.Han_Muc,detail.Ma_dvcs, detail.Ma_Dt);
                    }

                    using (var connection = new SqlConnection(con.ConnectionString))
                    {
                        connection.Open();

                        using (var command = new SqlCommand("InsertCongTacVien_SAP", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@_Ngay_Ct", CTV.Ngay_Ct);
                            command.Parameters.AddWithValue("@_so_Ct", CTV.So_Ct);
                            command.Parameters.AddWithValue("@_Ten_Dt", CTV.Ten_Dt);
                            command.Parameters.AddWithValue("@_Dvcs", CTV.Dvcs);
                            command.Parameters.AddWithValue("@_Ma_Dt", CTV.Ma_Dt);

                            // Pass details as a TVP parameter
                            var detailsParam = command.Parameters.AddWithValue("@_Details", detailsTable);
                            detailsParam.SqlDbType = SqlDbType.Structured;
                            detailsParam.TypeName = "B30CTVDetailType"; // Replace with your actual type name

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
        public ActionResult UpdateCTV(CTV CTV)
        {
            string result = "Error!";
            SqlConnectionHelper.ConnectSQL(con);
            if (CTV != null && CTV.Details != null)
            {
                try
                {
                    var detailsTable = new DataTable();
                    detailsTable.Columns.Add("Ma_vt", typeof(int));
                    detailsTable.Columns.Add("Ten_Vt", typeof(string));
                    detailsTable.Columns.Add("Ngay_Ct", typeof(string));
                    detailsTable.Columns.Add("Han_Muc", typeof(float));
                    detailsTable.Columns.Add("Ma_dvcs", typeof(string));
                    detailsTable.Columns.Add("Ma_Dt", typeof(string));
                    foreach (var detail in CTV.Details)
                    {
                        detailsTable.Rows.Add(detail.Ma_vt, detail.Ten_Vt, detail.Ngay_Ct1, detail.Han_Muc, detail.Ma_dvcs, detail.Ma_Dt);
                    }

                    using (var connection = new SqlConnection(con.ConnectionString))
                    {
                        connection.Open();

                        using (var command = new SqlCommand("UpdateCongTacVien_SAP", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@_Ngay_Ct", CTV.Ngay_Ct);
                            command.Parameters.AddWithValue("@_so_Ct", CTV.So_Ct);
                            command.Parameters.AddWithValue("@_Ten_Dt", CTV.Ten_Dt);
                            command.Parameters.AddWithValue("@_Dvcs", CTV.Dvcs);
                            command.Parameters.AddWithValue("@_Ma_Dt", CTV.Ma_Dt);
                            command.Parameters.AddWithValue("@_CTVId", CTV.CTVId);

                            // Pass details as a TVP parameter
                            var detailsParam = command.Parameters.AddWithValue("@_Details", detailsTable);
                            detailsParam.SqlDbType = SqlDbType.Structured;
                            detailsParam.TypeName = "B30CTVDetailType"; // Replace with your actual type name

                            command.ExecuteNonQuery();

                            result = "Success! Đã sửa";
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
       
        public async Task<List<CTV>> LoadDmVtAsync()
        {
            SqlConnectionHelper.ConnectSQL(con);
            List<CTV> dataItems = new List<CTV>();

            using (SqlConnection connection = new SqlConnection(con.ConnectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("[usp_DanhMucVt_SAP]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 50;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            CTV dataItem = new CTV
                            {
                                Ma_vt = reader["Ma_Vt"].ToString(),
                                Ten_Vt = reader["Ten_Vt"].ToString()
                            };
                            dataItems.Add(dataItem);
                        }
                    }
                }
            }

            return dataItems;
        }

        public async Task<ActionResult> EditCTV()
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);

            // Load data asynchronously
            List<CTV> DmVt = await LoadDmVtAsync();
            ViewBag.DataItems2 = DmVt;

            string Pname = "[EditHanMucCTV]";
            string ctvId = Request.QueryString["CTVId"];
            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 50;
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@_CTVId", ctvId);

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    await Task.Run(() => sda.Fill(ds));
                }
            }

            return View(ds);
        }


        public async Task<ActionResult> CoppyCTV()
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);
            List<MauInChungTu> dmDlist =  LoadData.LoadDmDt("", Request);
            List<CTV> DmVt = await LoadDmVtAsync();

            ViewBag.DataItems = dmDlist;
            ViewBag.DataItems2 = DmVt;

            string Pname = "[EditHanMucCTV]";
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

        public ActionResult Delete(CTV ctv)
        {
            string result = "Error!";
            SqlConnectionHelper.ConnectSQL(con);
            try
            {
                using (var connection = new SqlConnection(con.ConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("[DELETE_HanMucCTV]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@_STT", ctv.CTVId);
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