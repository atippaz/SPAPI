using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SPAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace SPAPI.Controllers.Type
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        string sqlDataSource ="";
        private readonly IConfiguration _config;
        public TypeController(IConfiguration config)
        {
            _config = config;
            sqlDataSource = _config.GetConnectionString("ShopDB");
        }
        [HttpGet]
        public JsonResult Type()
        {

            DataTable table = new DataTable();
            using (SqlConnection connect = new SqlConnection(sqlDataSource))
            {
                connect.Open();
                string cmd = $"select * from dbo.ProductTypes";
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
