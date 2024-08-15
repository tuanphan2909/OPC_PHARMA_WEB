using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web4.Models;

namespace web4.Controllers
{
    public class SERVERController : Controller
    {
    
        public static class SqlConnectionHelper
        {
            public static void ConnectSQL(SqlConnection con)
            {
                con.ConnectionString = "Data source= " + "118.69.109.109" + ";database=" + "SAP_OPC" + ";uid=OPC;password=OpcaBc@135#";
            }
        }

    }
}