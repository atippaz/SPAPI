using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace SPAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly IConfiguration _config;
        public ProvinceController(IConfiguration config)
        {
            _config = config;
        }
        [HttpGet]
        /* public string[] test()
         {
             return new string[] { "test1","test2","test3","test4","test5" };
         }*/
        public JsonResult Get()
        {
            string query = @"select * from dbo.Provinces";
            SqlDataReader Reader;
            DataTable table = new DataTable();
            string sqlDataSource = _config.GetConnectionString("Postals");
            using (SqlConnection connect = new SqlConnection(sqlDataSource))
            {
                connect.Open();
                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    Reader = command.ExecuteReader();
                    table.Load(Reader);
                    Reader.Close();
                    connect.Close();
                }
            }
            return new JsonResult(table);
        }
    }
}
