using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using web4.Models;
using static web4.Controllers.SERVERController;
using static web4.Controllers.LoadDataController;
namespace web4.Controllers
{
    public class B20HopDongMuaBanController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand sqlc = new SqlCommand();
        SqlDataReader dt;
        public B20HopDongMuaBanController()
        {
            SqlConnectionHelper.ConnectSQL(con);
        }


        public ActionResult Index()
        {
            List<B20HDMuaBan> b20HopDongMuaBanList = GetB20HopDongMuaBanData();
            return View(b20HopDongMuaBanList);
        }

        private List<B20HDMuaBan> GetB20HopDongMuaBanData()
        {
            List<B20HDMuaBan> b20HopDongMuaBanList = new List<B20HDMuaBan>();

            using (SqlConnection conn = new SqlConnection(con.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM B20HopDongMuaBan", conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            B20HDMuaBan hd = new B20HDMuaBan
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Ma_Dvcs = reader["Ma_Dvcs"] == DBNull.Value ? string.Empty : reader["Ma_Dvcs"].ToString(),
                                Ma_Dt = reader["Ma_Dt"] == DBNull.Value ? string.Empty : reader["Ma_Dt"].ToString(),
                                Ten_Dt = reader["Ten_Dt"] == DBNull.Value ? string.Empty : reader["Ten_Dt"].ToString(),
                                Dia_Chi = reader["Dia_Chi"] == DBNull.Value ? string.Empty : reader["Dia_Chi"].ToString(),
                                Ma_TDV = reader["Ma_TDV"] == DBNull.Value ? string.Empty : reader["Ma_TDV"].ToString(),
                                Ten_TDV = reader["Ten_TDV"] == DBNull.Value ? string.Empty : reader["Ten_TDV"].ToString(),
                                Loai_KH = reader["Loai_KH"] == DBNull.Value ? string.Empty : reader["Loai_KH"].ToString(),
                                Payment_ID = reader["Payment_ID"] == DBNull.Value ? string.Empty : reader["Payment_ID"].ToString(),
                                Payment_Text = reader["Payment_Text"] == DBNull.Value ? string.Empty : reader["Payment_Text"].ToString(),
                                CK = reader["CK"] == DBNull.Value ? string.Empty : reader["CK"].ToString(),
                                MST = reader["MST"] == DBNull.Value ? string.Empty : reader["MST"].ToString(),
                                Ma_Dai_Dien = reader["Ma_Dai_Dien"] == DBNull.Value ? string.Empty : reader["Ma_Dai_Dien"].ToString(),
                                Ten_Dai_Dien = reader["Ten_Dai_Dien"] == DBNull.Value ? string.Empty : reader["Ten_Dai_Dien"].ToString(),
                                So_Dien_Thoai = reader["So_Dien_Thoai"] == DBNull.Value ? string.Empty : reader["So_Dien_Thoai"].ToString(),
                                Ngay_Ky = reader.IsDBNull(reader.GetOrdinal("Ngay_Ky")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("Ngay_Ky")),
                                So_Hop_Dong = reader["So_Hop_Dong"] == DBNull.Value ? string.Empty : reader["So_Hop_Dong"].ToString(),
                                Ky_Hieu_HD = reader["Ky_Hieu_HD"] == DBNull.Value ? string.Empty : reader["Ky_Hieu_HD"].ToString(),
                                Hop_Dong_VIP = reader.IsDBNull(reader.GetOrdinal("Hop_Dong_VIP")) ? 0 : (reader.GetBoolean(reader.GetOrdinal("Hop_Dong_VIP")) ? 1 : 0),
                                Doanh_Thu_Nam = reader.IsDBNull(reader.GetOrdinal("Doanh_Thu_Nam")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Doanh_Thu_Nam")),
                                Doanh_Thu_Q1 = reader.IsDBNull(reader.GetOrdinal("Doanh_Thu_Q1")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Doanh_Thu_Q1")),
                                Doanh_Thu_Q2 = reader.IsDBNull(reader.GetOrdinal("Doanh_Thu_Q2")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Doanh_Thu_Q2")),
                                Doanh_Thu_Q3 = reader.IsDBNull(reader.GetOrdinal("Doanh_Thu_Q3")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Doanh_Thu_Q3")),
                                Doanh_Thu_Q4 = reader.IsDBNull(reader.GetOrdinal("Doanh_Thu_Q4")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Doanh_Thu_Q4")),
                                TG_Ky_HD = reader["TG_Ky_HD"] == DBNull.Value ? string.Empty : reader["TG_Ky_HD"].ToString(),
                                TG_Th_HD = reader["TG_Th_HD"] == DBNull.Value ? string.Empty : reader["TG_Th_HD"].ToString(),
                                Isactive = reader.IsDBNull(reader.GetOrdinal("Isactive")) ? 0 : (reader.GetBoolean(reader.GetOrdinal("Isactive")) ? 1 : 0)
                            };
                            b20HopDongMuaBanList.Add(hd);
                        }
                    }
                }
            }

            return b20HopDongMuaBanList;
        }

        // Override Json method to set MaxJsonLength
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue // Set MaxJsonLength to the maximum value
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(B20HDMuaBan model, string SelectedBranch)
        {
            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("ModelState is invalid");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine("Validation Error: " + error.ErrorMessage);
                    }
                }

                // Re-populate the data for dropdowns and other controls
                model.DataItemsDt = LoadData.LoadDmDt(SelectedBranch,Request);
                model.DataItemsTdv = LoadDmTDV(SelectedBranch);
                model.PaymentData = GetPaymentData();
                model.CKData = GetCKData();
                model.SelectedBranch = SelectedBranch;

                return View(model);
            }

            try
            {
                System.Diagnostics.Debug.WriteLine("Before SQL Insert");
                using (SqlConnection conn = new SqlConnection(con.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO B20HopDongMuaBan (Ma_Dvcs, Ma_Dt, Ten_Dt, Dia_Chi, Ma_TDV, Ten_TDV, Loai_KH, Payment_ID, Payment_Text, CK, MST, Ma_Dai_Dien, Ten_Dai_Dien, So_Dien_Thoai, Ngay_Ky, So_Hop_Dong, Ky_Hieu_HD, Hop_Dong_VIP, Doanh_Thu_Nam, Doanh_Thu_Q1, Doanh_Thu_Q2, Doanh_Thu_Q3, Doanh_Thu_Q4, TG_Ky_HD, TG_Th_HD, Isactive) VALUES (@Ma_Dvcs, @Ma_Dt, @Ten_Dt, @Dia_Chi, @Ma_TDV, @Ten_TDV, @Loai_KH, @Payment_ID, @Payment_Text, @CK, @MST, @Ma_Dai_Dien, @Ten_Dai_Dien, @So_Dien_Thoai, @Ngay_Ky, @So_Hop_Dong, @Ky_Hieu_HD, @Hop_Dong_VIP, @Doanh_Thu_Nam, @Doanh_Thu_Q1, @Doanh_Thu_Q2, @Doanh_Thu_Q3, @Doanh_Thu_Q4, @TG_Ky_HD, @TG_Th_HD, @Isactive)", conn))
                    {
                        cmd.Parameters.AddWithValue("@Ma_Dvcs", (object)SelectedBranch ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Ma_Dt", (object)model.Ma_Dt ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Ten_Dt", string.IsNullOrWhiteSpace(model.Ten_Dt) ? " " : model.Ten_Dt);
                        cmd.Parameters.AddWithValue("@Dia_Chi", string.IsNullOrWhiteSpace(model.Dia_Chi) ? " " : model.Dia_Chi);
                        cmd.Parameters.AddWithValue("@Ma_TDV", (object)model.Ma_TDV ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Ten_TDV", string.IsNullOrWhiteSpace(model.Ten_TDV) ? " " : model.Ten_TDV);
                        cmd.Parameters.AddWithValue("@Loai_KH", string.IsNullOrWhiteSpace(model.Loai_KH) ? " " : model.Loai_KH);
                        cmd.Parameters.AddWithValue("@Payment_ID", (object)model.Payment_ID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Payment_Text", string.IsNullOrWhiteSpace(model.Payment_Text) ? " " : model.Payment_Text);
                        cmd.Parameters.AddWithValue("@CK", (object)model.CK ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MST", string.IsNullOrWhiteSpace(model.MST) ? " " : model.MST);
                        cmd.Parameters.AddWithValue("@Ma_Dai_Dien", string.IsNullOrWhiteSpace(model.Ma_Dai_Dien) ? " " : model.Ma_Dai_Dien);
                        cmd.Parameters.AddWithValue("@Ten_Dai_Dien", string.IsNullOrWhiteSpace(model.Ten_Dai_Dien) ? " " : model.Ten_Dai_Dien);
                        cmd.Parameters.AddWithValue("@So_Dien_Thoai", string.IsNullOrWhiteSpace(model.So_Dien_Thoai) ? " " : model.So_Dien_Thoai);
                        cmd.Parameters.AddWithValue("@Ngay_Ky", model.Ngay_Ky);
                        cmd.Parameters.AddWithValue("@So_Hop_Dong", (object)model.So_Hop_Dong ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Ky_Hieu_HD", string.IsNullOrWhiteSpace(model.Ky_Hieu_HD) ? " " : model.Ky_Hieu_HD);
                        cmd.Parameters.AddWithValue("@Hop_Dong_VIP", model.Hop_Dong_VIP);
                        cmd.Parameters.AddWithValue("@Doanh_Thu_Nam", model.Doanh_Thu_Nam);
                        cmd.Parameters.AddWithValue("@Doanh_Thu_Q1", model.Doanh_Thu_Q1);
                        cmd.Parameters.AddWithValue("@Doanh_Thu_Q2", model.Doanh_Thu_Q2);
                        cmd.Parameters.AddWithValue("@Doanh_Thu_Q3", model.Doanh_Thu_Q3);
                        cmd.Parameters.AddWithValue("@Doanh_Thu_Q4", model.Doanh_Thu_Q4);
                        cmd.Parameters.AddWithValue("@TG_Ky_HD", string.IsNullOrWhiteSpace(model.TG_Ky_HD) ? " " : model.TG_Ky_HD);
                        cmd.Parameters.AddWithValue("@TG_Th_HD", string.IsNullOrWhiteSpace(model.TG_Th_HD) ? " " : model.TG_Th_HD);
                        cmd.Parameters.AddWithValue("@Isactive", model.Isactive);

                        cmd.ExecuteNonQuery();
                    }
                }

                System.Diagnostics.Debug.WriteLine("After SQL Insert");
                TempData["SuccessMessage"] = "Hợp đồng đã được thêm thành công!";
                return RedirectToAction("Index");
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"SQL error: {ex.Message}");
                TempData["ErrorMessage"] = "SQL Error: " + ex.Message;
                // Additional logging or error handling
            }
            catch (InvalidOperationException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Invalid operation: {ex.Message}");
                TempData["ErrorMessage"] = "Invalid Operation: " + ex.Message;
                // Additional logging or error handling
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("StackTrace: " + ex.StackTrace);
            }

            // Re-populate the data for dropdowns and other controls
            model.DataItemsDt = LoadData.LoadDmDt(SelectedBranch,Request);
            model.DataItemsTdv = LoadDmTDV(SelectedBranch);
            model.PaymentData = GetPaymentData();
            model.CKData = GetCKData();
            model.SelectedBranch = SelectedBranch;

            return View(model);
        }

        // LOAD DATA 
        [HttpGet]
        public JsonResult LoadDataWithProgress(string branch)
        {
            try
            {
                // Simulate the progress
                for (int progress = 25; progress <= 100; progress += 25)
                {
                    Thread.Sleep(1000); // Simulate a delay
                }

                return Json(new { success = true, progress = 100 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Create(string branch)
        {
            var dataItemsDt = LoadData.LoadDmDt(branch,Request);
            var dataItemsTdv = LoadDmTDV(branch);
            var paymentData = GetPaymentData();
            var ckData = GetCKData();

            var model = new B20HDMuaBan
            {
                SelectedBranch = branch,
                DataItemsDt = dataItemsDt,
                DataItemsTdv = dataItemsTdv,
                PaymentData = paymentData, // Add this line
                CKData = ckData, // Add this line
                Loai_KH = "OTC_L" // Default value for Loai_KH
            };

            return View(model);
        }
        /*
         *   private string ModifyBranchCodeForDt(string branch)
        {
            // Append '_01' to the branch code.
            return $"{branch}_01";
        }

         */

        [HttpGet]
        public JsonResult GetBranchData(string branchCode)
        {
            var branchData = LoadData.LoadDmDt(branchCode, Request); // Lấy dữ liệu từ CSDL dựa trên branchCode
            return Json(branchData, JsonRequestBehavior.AllowGet);
        }

     

        public List<BKHoaDonGiaoHang> LoadDmTDV(string Ma_dvcs)
        {
            List<BKHoaDonGiaoHang> dataItems = new List<BKHoaDonGiaoHang>();

            using (SqlConnection connection = new SqlConnection(con.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("[usp_DanhSachTDV]", connection))
                {
                    command.CommandTimeout = 950;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@_Ma_Dvcs", Ma_dvcs);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            throw new Exception("No rows returned from the stored procedure.");
                        }

                        var columnNames = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();

                        while (reader.Read())
                        {
                            BKHoaDonGiaoHang dataItem = new BKHoaDonGiaoHang
                            {
                                Ma_CbNv = reader["Ma_CbNv"].ToString(),
                                hoten = reader["hoten"].ToString(),
                                Ma_Dvcs = columnNames.Contains("Ma_Dvcs") ? reader["Ma_Dvcs"].ToString() : string.Empty,
                                Ma_Business = columnNames.Contains("Ma_Business") ? reader["Ma_Business"].ToString() : string.Empty
                            };
                            dataItems.Add(dataItem);
                        }
                    }
                }
            }

            return dataItems;
        }

        private List<PaymentData> GetPaymentData()
        {
            // Ideally, this data should come from a database. For demonstration, it's hardcoded here.
            return new List<PaymentData>
            {
                new PaymentData { PaymentId = "1001", PaymentText = "10 days net" },
                new PaymentData { PaymentId = "1002", PaymentText = "20 days net" },
                new PaymentData { PaymentId = "1003", PaymentText = "30 days net" },
                new PaymentData { PaymentId = "1004", PaymentText = "45 days net" },
                new PaymentData { PaymentId = "1005", PaymentText = "60 days net" },
                new PaymentData { PaymentId = "1006", PaymentText = "90 days net" },
                new PaymentData { PaymentId = "1007", PaymentText = "7 days net" },
                new PaymentData { PaymentId = "1008", PaymentText = "13 days 2% cash discount, 13 days net" },
                new PaymentData { PaymentId = "1009", PaymentText = "10 days 5% cash discount, 20 days net" },
                new PaymentData { PaymentId = "1010", PaymentText = "10 days 3% cash discount, 20 days net" },
                new PaymentData { PaymentId = "1011", PaymentText = "13 days 2% cash discount, 23 days net" },
                new PaymentData { PaymentId = "1012", PaymentText = "18 days 2% cash discount, 18 days net" },
                new PaymentData { PaymentId = "1013", PaymentText = "33 days 2% cash discount, 45 days net" },
                new PaymentData { PaymentId = "1014", PaymentText = "33 days 2% cash discount, 60 days net" },
                new PaymentData { PaymentId = "1015", PaymentText = "33 days 2% cash discount, 90 days net" },
                new PaymentData { PaymentId = "1016", PaymentText = "63 days 2% cash discount, 63 days net" },
                new PaymentData { PaymentId = "1017", PaymentText = "63 days 2% cash discount, 90 days net" },
                new PaymentData { PaymentId = "1018", PaymentText = "30 days 2% cash discount, 30 days net  " },
                new PaymentData { PaymentId = "1019", PaymentText = "30 days 2% cash discount, 45 days 1% cash discount, 45 days net" },
                new PaymentData { PaymentId = "1020", PaymentText = "14 days 3% cash discount, 30 days 2% cash discount, 45 days net" },
                new PaymentData { PaymentId = "1021", PaymentText = "14 days 2% cash discount, 30 days 1.5% cash discount, 45 days net" },
                new PaymentData { PaymentId = "1022", PaymentText = "14 days 3% cash discount, 30 days 1% cash discount, 60 days net" },
                new PaymentData { PaymentId = "1023", PaymentText = "14 days 4% cash discount, 30 days 2% cash discount, 60 days net" },
                new PaymentData { PaymentId = "1024", PaymentText = "Before end of month 4%,before 15 of next month 2%, before 15 in 2 months due net" },
                new PaymentData { PaymentId = "1025", PaymentText = "Before 15 of next month 2% cash, before end of next month due net" },
                new PaymentData { PaymentId = "1026", PaymentText = "45 days from Invoice Date, End of month" },
                new PaymentData { PaymentId = "1027", PaymentText = "End of Month of Invoice Date, 45 days" },
                new PaymentData { PaymentId = "1028", PaymentText = "30 days from Invoice Date, End of month" },
                new PaymentData { PaymentId = "1029", PaymentText = "End of Month of Invoice Date, 30 days" },
                new PaymentData { PaymentId = "1030", PaymentText = "60 days from Invoice Date, End of month" },
                new PaymentData { PaymentId = "1031", PaymentText = "End of Month of Invoice Date, 60 days" },
                new PaymentData { PaymentId = "1032", PaymentText = "75 days from Invoice Date, End of month" },
                new PaymentData { PaymentId = "1033", PaymentText = "End of Month of Invoice Date, 75 days" },
                new PaymentData { PaymentId = "Z001", PaymentText = "15 days 2% cash discount, 20 days net" },
                new PaymentData { PaymentId = "Z002", PaymentText = "33 days 2% cash discount, 33 days net" },
                new PaymentData { PaymentId = "Z003", PaymentText = "33 days 2% cash discount, 48 days 1% cash discount, 48 days net" },
                new PaymentData { PaymentId = "Z004", PaymentText = "48 days 2% cash discount, 48 days net" },
                new PaymentData { PaymentId = "Z005", PaymentText = "10 days 2% cash discount, 10 days net" },
                new PaymentData { PaymentId = "Z006", PaymentText = "43 days 2% cash discount, 43 days net" },
                new PaymentData { PaymentId = "Z007", PaymentText = "7 days 2% cash discount, 7 days net" },
                new PaymentData { PaymentId = "Z008", PaymentText = "15 days 2% cash discount, 15 days net" },
                new PaymentData { PaymentId = "Z009", PaymentText = "30 days 2% cash discount, 30 days net" },
                new PaymentData { PaymentId = "Z010", PaymentText = "45 days 2% cash discount, 45 days net" },
                new PaymentData { PaymentId = "Z011", PaymentText = "30 days 2% cash discount, 40 days net" },
                new PaymentData { PaymentId = "Z012", PaymentText = "15 days net" },
                new PaymentData { PaymentId = "Z013", PaymentText = "45 days 2% cash discount, 60 days 1% cash discount, 60 days net" },
                new PaymentData { PaymentId = "Z014", PaymentText = "33 days 2% cash discount, 48 days 1% cash discount, 90 days net" },
                new PaymentData { PaymentId = "Z015", PaymentText = "33 days 2% cash discount, 48 days 1% cash discount, 60 days net" },
                new PaymentData { PaymentId = "Z019", PaymentText = "63 days 2% cash discount, 63 days net" },
                new PaymentData { PaymentId = "Z020", PaymentText = "18 days 2% cash discount, 30 days net" },
                new PaymentData { PaymentId = "Z021", PaymentText = "33 days 2% cash discount, 45 days 1% cash discount, 90 days net" },
                new PaymentData { PaymentId = "Z022", PaymentText = "33 days 2% cash discount, 120 days net" },
                new PaymentData { PaymentId = "Z023", PaymentText = "33 days 2% cash discount, 180 days net" },
                new PaymentData { PaymentId = "Z024", PaymentText = "33 days 2% cash discount, 45 days 1% cash discount, 45 days net" },
                new PaymentData { PaymentId = "Z025", PaymentText = "5 days net" },
                new PaymentData { PaymentId = "Z026", PaymentText = "18 days net" },
                new PaymentData { PaymentId = "Z027", PaymentText = "33 days net" },
                new PaymentData { PaymentId = "Z028", PaymentText = "3 days net" },
                new PaymentData { PaymentId = "Z029", PaymentText = "13 days net" },
                new PaymentData { PaymentId = "Z030", PaymentText = "25 days 2% cash discount, 25 days net" },
                new PaymentData { PaymentId = "Z031", PaymentText = "40 days 2% cash discount, 40 days net" },
                new PaymentData { PaymentId = "Z032", PaymentText = "28 days 2% cash discount, 28 days net" },
                new PaymentData { PaymentId = "Z033", PaymentText = "35 days 2% cash discount, 35 days net" },
            };
        }

        public List<KeyValuePair<string, string>> GetCKData()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("V1", "NhomCK0200"),
                new KeyValuePair<string, string>("V2", "NhomCK0400"),
                new KeyValuePair<string, string>("V3", "NhomCK3023"),
                new KeyValuePair<string, string>("V4", "NhomCK3033"),
                new KeyValuePair<string, string>("V5", "NhomCK3233"),
                new KeyValuePair<string, string>("V6", "NhomCK5055"),
                new KeyValuePair<string, string>("V7", "NhomCK5255"),
                new KeyValuePair<string, string>("V8", "NhomCK5455"),
                new KeyValuePair<string, string>("V9", "NhomCK5555"),
                new KeyValuePair<string, string>("VA", "NhomCK3333"),
                new KeyValuePair<string, string>("VB", "NhomCK0000"),
                new KeyValuePair<string, string>("VC", "NhomCK2022"),
                new KeyValuePair<string, string>("VD", "NhomCK0300"),
                new KeyValuePair<string, string>("VE", "NhomCK0500"),
                new KeyValuePair<string, string>("VF", "NhomCK3323"),
                new KeyValuePair<string, string>("VG", "NhomCK3533"),
                new KeyValuePair<string, string>("VH", "NhomCK2222"),
                new KeyValuePair<string, string>("VI", "NhomCK1031010"),
                new KeyValuePair<string, string>("Z1", "Khách hàng nội bộ"),
                new KeyValuePair<string, string>("Z2", "NhomCK10101010")
            };
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = GetB20HopDongMuaBanData().FirstOrDefault(x => x.Id == id);

            if (model == null)
            {
                return HttpNotFound();
            }

            model.DataItemsDt = LoadData.LoadDmDt(model.Ma_Dvcs,Request);
            model.DataItemsTdv = LoadDmTDV(model.Ma_Dvcs);
            model.PaymentData = GetPaymentData();
            model.CKData = GetCKData();

            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Edit(B20HDMuaBan model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(con.ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("UPDATE B20HopDongMuaBan SET Ma_Dvcs = @Ma_Dvcs, Ma_Dt = @Ma_Dt, Ten_Dt = @Ten_Dt, Dia_Chi = @Dia_Chi, Ma_TDV = @Ma_TDV, Ten_TDV = @Ten_TDV, Loai_KH = @Loai_KH, Payment_ID = @Payment_ID, Payment_Text = @Payment_Text, CK = @CK, MST = @MST, Ma_Dai_Dien = @Ma_Dai_Dien, Ten_Dai_Dien = @Ten_Dai_Dien, So_Dien_Thoai = @So_Dien_Thoai, Ngay_Ky = @Ngay_Ky, So_Hop_Dong = @So_Hop_Dong, Ky_Hieu_HD = @Ky_Hieu_HD, Hop_Dong_VIP = @Hop_Dong_VIP, Doanh_Thu_Nam = @Doanh_Thu_Nam, Doanh_Thu_Q1 = @Doanh_Thu_Q1, Doanh_Thu_Q2 = @Doanh_Thu_Q2, Doanh_Thu_Q3 = @Doanh_Thu_Q3, Doanh_Thu_Q4 = @Doanh_Thu_Q4, TG_Ky_HD = @TG_Ky_HD, TG_Th_HD = @TG_Th_HD, Isactive = @Isactive WHERE Id = @Id", conn))
                        {
                            cmd.Parameters.AddWithValue("@Id", model.Id);
                            cmd.Parameters.AddWithValue("@Ma_Dvcs", (object)model.Ma_Dvcs ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Ma_Dt", (object)model.Ma_Dt ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Ten_Dt", (object)model.Ten_Dt ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Dia_Chi", (object)model.Dia_Chi ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Ma_TDV", (object)model.Ma_TDV ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Ten_TDV", (object)model.Ten_TDV ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Loai_KH", (object)model.Loai_KH ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Payment_ID", (object)model.Payment_ID ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Payment_Text", (object)model.Payment_Text ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@CK", (object)model.CK ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@MST", (object)model.MST ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Ma_Dai_Dien", (object)model.Ma_Dai_Dien ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Ten_Dai_Dien", (object)model.Ten_Dai_Dien ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@So_Dien_Thoai", (object)model.So_Dien_Thoai ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Ngay_Ky", model.Ngay_Ky);
                            cmd.Parameters.AddWithValue("@So_Hop_Dong", (object)model.So_Hop_Dong ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Ky_Hieu_HD", (object)model.Ky_Hieu_HD ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Hop_Dong_VIP", model.Hop_Dong_VIP);
                            cmd.Parameters.AddWithValue("@Doanh_Thu_Nam", model.Doanh_Thu_Nam);
                            cmd.Parameters.AddWithValue("@Doanh_Thu_Q1", model.Doanh_Thu_Q1);
                            cmd.Parameters.AddWithValue("@Doanh_Thu_Q2", model.Doanh_Thu_Q2);
                            cmd.Parameters.AddWithValue("@Doanh_Thu_Q3", model.Doanh_Thu_Q3);
                            cmd.Parameters.AddWithValue("@Doanh_Thu_Q4", model.Doanh_Thu_Q4);
                            cmd.Parameters.AddWithValue("@TG_Ky_HD", (object)model.TG_Ky_HD ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@TG_Th_HD", (object)model.TG_Th_HD ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Isactive", model.Isactive);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    TempData["SuccessMessage"] = "Hợp đồng đã được cập nhật thành công!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error: " + ex.Message;
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = GetB20HopDongMuaBanData().FirstOrDefault(x => x.Id == id);

            if (model == null)
            {
                return HttpNotFound();
            }

            model.DataItemsDt = LoadData.LoadDmDt(model.Ma_Dvcs,Request);
            model.DataItemsTdv = LoadDmTDV(model.Ma_Dvcs);
            model.PaymentData = GetPaymentData();
            model.CKData = GetCKData();

            return View("Delete", model);
        }

        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Attempting to delete record with Id: {id}");
                using (SqlConnection conn = new SqlConnection(con.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM B20HopDongMuaBan WHERE Id = @Id", conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine($"Rows affected: {rowsAffected}");
                        if (rowsAffected > 0)
                        {
                            return Json(new { success = true });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Không tìm thấy hợp đồng để xóa." });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in DeleteConfirmed: {ex.Message}");
                return Json(new { success = false, message = "Xảy ra lỗi trong quá trình xóa. Vui lòng thử lại.", errorDetails = ex.ToString() });
            }
        }
    }

    public class PaymentData
    {
        public string PaymentId { get; set; }
        public string PaymentText { get; set; }
    }
}
