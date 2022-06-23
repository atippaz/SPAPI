using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SPAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace SPAPI.Controllers.Brand
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        string sqlDataSource ="";
        private readonly IConfiguration _config;
        public BrandController(IConfiguration config)
        {
            _config = config;
            sqlDataSource = _config.GetConnectionString("ShopDB");
        }

        /* public JsonResult Brand([FromBody] string ProdID)
         {

             DataTable table = new DataTable();
             using (SqlConnection connect = new SqlConnection(sqlDataSource))
             {
                 connect.Open();
                 string cmd = $"select * from dbo.Products p Where b.BrandID = '{username}'";
                 using (SqlCommand command = new SqlCommand(cmd, connect))
                 {
                     using (SqlDataAdapter adapter = new SqlDataAdapter())
                     {
                         adapter.SelectCommand = command;
                         adapter.Fill(table);
                         connect.Close();
                     }

                 }
             }
             CustomerAccountModel user = new CustomerAccountModel();
             foreach (DataRow item in table.Rows)
             {
                 user.CustID = item["CustID"].ToString();
                 user.CustUsername = item["CustUsername"].ToString();
                 user.CustPassword = item["CustPassword"].ToString();
             }
             return new JsonResult(table);
         }*/
        [HttpGet]
        public JsonResult Brand()
        {

            DataTable table = new DataTable();
            using (SqlConnection connect = new SqlConnection(sqlDataSource))
            {
                connect.Open();
                string cmd = $"select * from dbo.Brands";
                using (SqlCommand command = new SqlCommand(cmd, connect))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter())
                    {
                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                        connect.Close();
                    }

                }
            }
          
            return new JsonResult(table);
        }
    }

}
