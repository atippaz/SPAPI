using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SPAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace SPAPI.Controllers.Order
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        string sqlDataSource ="";
        private readonly IConfiguration _config;
        public OrderController(IConfiguration config)
        {
            _config = config;
            sqlDataSource = _config.GetConnectionString("ShopDB");
        }
        [HttpPost]
        public bool Order([FromBody] JObject orderDetail )
        {
            string CustID = "";
            string CreateDate = "";
            string ShipID = "";
               string OrderStatus = "";
               string Annotation = "";
               string ProdID = "";
              int ProdCost = 0;
               int ProdPrice = 0;
            int ProdQty = 0;
            try
            {
                foreach (JProperty prop in orderDetail.Properties())
                {
                    if (prop.Name == "CustID")
                    {
                        CustID = prop.Value.ToString();
                    }
                    else if (prop.Name == "CreateDate")
                    {
                        CreateDate = prop.Value.ToString();
                    }
                    else if (prop.Name == "ShipID")
                    {
                        ShipID = prop.Value.ToString();
                    }
                    else if (prop.Name == "OrderStatus")
                    {
                        OrderStatus = prop.Value.ToString();
                    }
                    else if (prop.Name == "Annotation")
                    {
                        Annotation = prop.Value.ToString();
                    }
                    else if (prop.Name == "ProdQty")
                    {
                        ProdQty = Convert.ToInt32(prop.Value.ToString());
                    }
                    else if (prop.Name == "ProdID")
                    {
                        ProdID = prop.Value.ToString();
                    }
                    else if (prop.Name == "ProdCost")
                    {
                        ProdCost = Convert.ToInt32(prop.Value.ToString());
                    }
                    else if (prop.Name == "ProdPrice")
                    {
                        ProdPrice = Convert.ToInt32(prop.Value.ToString());
                    }
                }
                int remainqty = 0;
                DataTable table = new DataTable();
                string query = $"select ProdQty from[dbo].[Products] p where [ProdID] ='{ProdID}'";
                Console.WriteLine(query);
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
                
                foreach (DataRow mon in table.Rows)
                {
                    remainqty = Convert.ToInt32(mon["ProdQty"].ToString());
                }

                query = $"UPDATE [dbo].[Products] SET [ProdQty] = {remainqty - ProdQty} WHERE [ProdID] ='{ProdID}'";
                Console.WriteLine(query);
                using (SqlConnection connect = new SqlConnection(sqlDataSource))
                {
                    connect.Open();
                    using (SqlCommand command = new SqlCommand(query, connect))
                    {
                        command.ExecuteNonQuery();
                        connect.Close();
                    }
                }
                string caID = "";
                table = new DataTable();
                query = $"SELECT [CaID] FROM.[dbo].[CustomerAddress] ca where ca.CustID = '{CustID}'";
                Console.WriteLine(query);
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
                foreach (DataRow mon in table.Rows)
                {
                    caID = mon["CaID"].ToString();
                }
                query = $"INSERT INTO[dbo].[Orders]  ([CustID],[CreateDate],[ShipID],[OrStatusID],[CaID],[Annotation]) VALUES ('{CustID}','{CreateDate}','{ShipID}','{OrderStatus}','{caID}','{Annotation}')";
                Console.WriteLine(query);
                //  query = $"INSERT INTO[dbo].[Orders]  ([CustID],[OrderDate],[CreateDate],[ShipID],[OrStatusID],[CaID],[Annotation]) VALUES ('{CustID}',{null},'{CreateDate}','{ShipID}','{caID}','{Annotation}')";
                using (SqlConnection connect = new SqlConnection(sqlDataSource))
                {
                    connect.Open();
                    using (SqlCommand command = new SqlCommand(query, connect))
                    {
                        command.ExecuteNonQuery();
                        connect.Close();
                    }
                }
                table = new DataTable();
                query = $"SELECT [OrderID] FROM[ShopDB].[dbo].[Orders] o where (o.CustID ='{CustID}' AND o.CaID = '{caID}') AND (o.Annotation = '{Annotation}'AND o.ShipID = '{ShipID}')";
                Console.WriteLine(query);
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
                string orderID = "";
                foreach (DataRow mon in table.Rows)
                {
                    orderID = mon["OrderID"].ToString();
                }

                query = $"INSERT INTO[dbo].[OrderDetails]([OrderID],[ProdID],[ProdQty],[ProdPrice] ,[ProdCost])VALUES( '{orderID}','{ProdID}','{ProdQty}','{ProdPrice}','{ProdCost}')";
                Console.WriteLine(query);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
         
               
            }
        }
    }

