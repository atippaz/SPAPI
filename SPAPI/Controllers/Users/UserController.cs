using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SPAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace SPAPI.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        string sqlDataSource ="";
        private readonly IConfiguration _config;
        public UserController(IConfiguration config)
        {
            _config = config;
            sqlDataSource = _config.GetConnectionString("ShopDB");
        }
        [HttpPost("Register")]
        public JsonResult Register([FromBody] JObject Name )
        {
            string name = Name.ToString();
            Console.WriteLine(name);
            //string query = @"select * from dbo.Products";
            DataTable table = new DataTable();
            /*string sqlDataSource = _config.GetConnectionString("ShopDB");
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
            }*/
            try
            {
                Console.WriteLine("test");
                foreach (JProperty prop in Name.Properties())
                {
                    Console.WriteLine(prop);
                    Console.WriteLine("sleep");
                    /*foreach (JProperty property in prop)
                    {
                        Console.WriteLine(property.Name + " - " + property.Value);
                    }*/
                    Console.WriteLine(prop.Name +" = "+ prop.Value);
                    Console.WriteLine("fi");
                    //or.....
                    /*foreach (KeyValuePair<string, JToken> property in prop)
                    {
                        Console.WriteLine(property.Key + " - " + property.Value);
                    }*/
                }
                return new JsonResult(table); ;
            }
            catch (Exception ex)
            {
                return new JsonResult(table); ;
            }
            Console.WriteLine(name);
            return new JsonResult(table);
        }
        [HttpPost("Login")]
        public JsonResult Login([FromBody] JObject Data)
        {
            DataTable table = new DataTable();
            string username="";
            string password = "";
            try
            {
                foreach (JProperty prop in Data.Properties())
                {
                    if(prop.Name == "username")
                    {
                        username = prop.Value.ToString();
                        Console.WriteLine(prop.Value.ToString() + "has coming");
                    }
                    else if(prop.Name == "password")
                    {
                        password = prop.Value.ToString();
                        Console.WriteLine("Password is "+ prop.Value.ToString());

                    }
                }
                using (SqlConnection connect = new SqlConnection(sqlDataSource))
                {
                    connect.Open();
                    string cmd = $"select * from dbo.CustomerAccounts c Where c.CustUsername = '{username}'";
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
                Console.WriteLine("cmd has finish");
               CustomerAccountModel user = new CustomerAccountModel();
                foreach (DataRow item in table.Rows)
                {
                    user.CustID = item["CustID"].ToString();
                    user.CustUsername = item["CustUsername"].ToString();
                    user.CustPassword = item["CustPassword"].ToString();
                }
                Console.WriteLine("user id is :"+user.CustID+" and user is "+user.CustUsername);
                if (user.CustUsername != "" || user.CustUsername != null)
                {
                    if (user.CustPassword != "" && BCrypt.Net.BCrypt.Verify(password, user.CustPassword)) {

                        table.Columns.Remove("CustPassword");
                    }
                    else
                    {
                        table = new DataTable();
                    }
                }
                else
                {
                    table = new DataTable();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                table = new DataTable();
            }
            return new JsonResult(table);
        }
    }

}
