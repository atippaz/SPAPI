using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json.Linq;

namespace SPAPI.Controllers.Purchase
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IConfiguration _config;
        public PurchaseController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public bool Purchase([FromBody] JObject purchase)
        {
            
            string BankID = "";
            int cost = 0;
            foreach (JProperty prop in purchase.Properties())
            {
                if (prop.Name == "BankID")
                {
                    BankID = prop.Value.ToString();
                }
                else if(prop.Name == "Cost")
                {
                    cost = Convert.ToInt32(prop.Value.ToString());
                }
            }
            string query = $"select BankMoney from dbo.BankMoney bm where bm.BankNumber = '{BankID}'";
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
            int money = 0;
            foreach (DataRow mon in table.Rows)
            {
                money = Convert.ToInt32(mon["BankMoney"].ToString());
            }
            if(money < cost)
            {
                return false;
            }
            else
            {
                money -= cost;
                query = $"UPDATE [dbo].[BankMoney] SET [BankMoney] = {money} WHERE BankNumber = '{BankID}'";
                using (SqlConnection connect = new SqlConnection(sqlDataSource))
                {
                    connect.Open();
                    using (SqlCommand command = new SqlCommand(query, connect))
                    {
                        command.ExecuteNonQuery();
                            connect.Close();
                    }
                }
                return true;
            }
        }
    }
}
