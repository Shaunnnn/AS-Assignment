using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AS_Assignment
{
    public partial class HomePage : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDB"].ConnectionString;
        byte[] Key;
        byte[] IV;
        byte[] Creditcard = null;
        string userID = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                userID = (string) Session["userID"];
                displayUserProfile(userID);

                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("login.aspx", false);
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }

        protected void btn_LogOut_Click(object sender, EventArgs e)
        {
            userID = (string)Session["userID"];
            SqlConnection con = new SqlConnection(MYDBConnectionString);
            string sql2 = "INSERT INTO Auditlog VALUES(@UserId,@LogAction,@LogDatetime,@LogNote)";
            SqlCommand cmd = new SqlCommand(sql2, con);
            try
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@UserId", userID);
                    cmd.Parameters.AddWithValue("@LogAction", "Account Logout");
                    cmd.Parameters.AddWithValue("@LogDatetime", DateTime.Now.ToString("dd/MM/yyyy"));
                    cmd.Parameters.AddWithValue("@LogNote", "Account Logout Successful");
                    con.Open();
                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { con.Close(); }

            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            string logout = "You have logout!";
            Response.Redirect("Login.aspx?msg= " + HttpUtility.UrlEncode(logout), false);
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }

        protected void btn_Change_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangePassword.aspx",false);
        }


        protected void displayUserProfile(string userid)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select * FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Email"] != DBNull.Value)
                        {
                            lbl_userID.Text = reader["Email"].ToString();
                        }
                        if (reader["FirstName"] != DBNull.Value && reader["LastName"] != DBNull.Value)
                        {
                            lbl_fullName.Text = reader["FirstName"].ToString() + " " + reader["LastName"].ToString();
                        }
                        if (reader["DOB"] != DBNull.Value)
                        {
                            lbl_DoB.Text = reader["DOB"].ToString();
                        }
                        if (reader["Creditcard"] != DBNull.Value)
                        {
                            Creditcard = Convert.FromBase64String(reader["Creditcard"].ToString());
                        }
                        if (reader["IV"] != DBNull.Value)
                        {
                            IV = Convert.FromBase64String(reader["IV"].ToString());
                        }
                        if (reader["Key"] != DBNull.Value)
                        {
                            Key = Convert.FromBase64String(reader["Key"].ToString());
                        }
                    }
                    lbl_creditCard.Text = decryptData(Creditcard);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }
        protected string decryptData(byte[] cipherText)
        {
            string plainText = null;

            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;

                ICryptoTransform decryptTransform = cipher.CreateDecryptor();

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.ToString()); }
            finally { }
            return plainText;
        }

    }
}