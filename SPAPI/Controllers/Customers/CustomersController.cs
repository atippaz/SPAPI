using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SPAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace SPAPI.Controllers.Customers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        string sqlDataSource ="";
        private readonly IConfiguration _config;
        public CustomersController(IConfiguration config)
        {
            _config = config;
            sqlDataSource = _config.GetConnectionString("ShopDB");
        }
        [HttpPost("GetBankAccount")]
        public JsonResult GetBankAccount()
        {
            DataTable table = new DataTable();
            try
            {
                string query = $"select * from dbo.BankAccounts b inner join dbo.BankMoney bm on b.BankNumber = bm.BankNumber";
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
            }
            catch (Exception ex)
            {
                table = new DataTable();
            }
           
            return new JsonResult(table);
        }
       
    }

}
