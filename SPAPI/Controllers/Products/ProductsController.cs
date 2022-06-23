using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json.Linq;
using SPAPI.Models;
namespace SPAPI.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class FindoneProductController : ControllerBase
    {
        private readonly IConfiguration _config;
        public FindoneProductController(IConfiguration config)
        {
            _config = config;
        }

        /* public string[] test()
         {
             return new string[] { "test1","test2","test3","test4","test5" };
         }*/
        [HttpPost]
        public JsonResult FindOneProduct([FromBody] JObject ProID)
        {
            string prodID = "";
            Console.WriteLine(ProID);
            foreach (JProperty prop in ProID.Properties())
            {
                if (prop.Name == "ProID")
                {
                    prodID = prop.Value.ToString();
                    Console.WriteLine(prop.Value.ToString() + "has coming");
                }
            }
            Console.WriteLine("Hi bro:" + prodID);
            string query = $"select * from dbo.Products p inner join dbo.ProductDetails pd on p.ProdDetailID = pd.ProdDetailID inner join dbo.Brands b on b.BrandID = pd.BrandID inner join dbo.ProductTypes pt on pt.TypeID = pd.TypeID where p.ProdID = '{prodID}'";

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

            table.Columns.Remove("BrandID1");
            table.Columns.Remove("ProdDetailID1");
            table.Columns.Remove("ID1");
            table.Columns.Remove("ID3");
            table.Columns.Remove("TypeID1");
            table.Columns.Remove("ID2");
            return new JsonResult(table);
        }

    }
}
