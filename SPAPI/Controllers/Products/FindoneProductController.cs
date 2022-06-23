using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace SPAPI.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _config;
        public ProductsController(IConfiguration config)
        {
            _config = config;
        }

        /* public string[] test()
         {
             return new string[] { "test1","test2","test3","test4","test5" };
         }*/
        [HttpGet]
        public JsonResult Product()
        {
            string query = @"select * from dbo.Products";

            DataTable table = new DataTable();
            string sqlDataSource = _config.GetConnectionString("ShopDB");
            using (SqlConnection connect = new SqlConnection(sqlDataSource))
            {
                connect.Open();
                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    using(SqlDataAdapter adapter = new SqlDataAdapter())
                    {
                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                        connect.Close();
                    }
                   
                }
            }
            Console.WriteLine("getData");
            return new JsonResult(table);

        }
        [HttpGet("FindOneProduct")]
        public JsonResult FindOneProduct([FromBody] string ProID)
        {
            Console.WriteLine("Hi bro");
            string query = $"select * from dbo.Products p inner join dbo.ProductDetails pd on p.ProdDetailID = pd.ProdDetailID inner join dbo.Brands b on b.BrandID = pd.BrandID where p.ProdID = '{ProID}'";

            DataTable table = new DataTable();
            string sqlDataSource = _config.GetConnectionString("ShopDB");
            using (SqlConnection connect = new SqlConnection(sqlDataSource))
            {
                connect.Open();
                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter())
                    {
                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                        connect.Close();
                    }

                }
            }
           /* Console.WriteLine("getData");
            CustomerAccountModel user = new CustomerAccountModel();
            foreach (DataRow item in table.Rows)
            {
                user.CustID = item["CustID"].ToString();
                user.CustUsername = item["CustUsername"].ToString();
                user.CustPassword = item["CustPassword"].ToString();
            }*/
            return new JsonResult(table);
        }

    }
}
