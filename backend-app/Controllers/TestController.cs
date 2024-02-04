using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using backend_app.Models;

namespace backend_app.Controllers
{
    [RoutePrefix("api/Test")]

    public class TestController : ApiController
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        SqlCommand cmd = null;
        SqlDataAdapter adapter = null;

        [HttpPost]
        [Route("Registration")]
        public string Registration(Customer customer)
        {
            string msg = string.Empty;
            try
            {
                cmd = new SqlCommand("usp_Registration", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", customer.Name);
                cmd.Parameters.AddWithValue("@PhoneNo", customer.PhoneNo);
                cmd.Parameters.AddWithValue("@Address", customer.Address);
                cmd.Parameters.AddWithValue("@Password", customer.Password);
                cmd.Parameters.AddWithValue("@IsActive", customer.IsActive);
                

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    msg = "Data Inserted";
                }
                else
                {
                    msg = "Error";
                }
               
            }
            catch (Exception e) {
                msg = e.Message;
            }
            return msg;

        }

        [HttpPost]
        [Route("Login")]
        public string Login(Customer customer)
        {
            string msg = string.Empty;
            try
            {
                adapter = new SqlDataAdapter("usp_Login", conn);
                adapter.SelectCommand.CommandType= CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.AddWithValue("@Name", customer.Name);
                adapter.SelectCommand.Parameters.AddWithValue("@Password", customer.Password);
                DataTable dt = new DataTable(); 
                adapter.Fill(dt);   
                if (dt.Rows.Count > 0) {
                    msg = "User is valid";
                }
                else { 
                    msg = "User is Invalid"; 
                } 
            }
            catch (Exception e)
            {
                msg = e.Message;
            }
            return msg;

        }
    }
}
