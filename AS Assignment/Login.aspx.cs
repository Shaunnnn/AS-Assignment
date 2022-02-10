using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Data;

namespace AS_Assignment
{
    public partial class Login : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDB"].ConnectionString;
        static string errorMsg = "";
        public class MyObject {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }

/*        public void ProcessRequest(HttpContext ctx)
        {
            ctx.Response.Write(
                "The page \"" + ctx.Request.QueryString["msg"] + "\" was not found.");
        }*/
        protected void Page_Load(object sender, EventArgs e)
        {
            Label4.Text = HttpUtility.UrlDecode(Request.QueryString["msg"]);
            Label4.Text = HttpUtility.UrlDecode(Request.QueryString["msg2"]);
            if (IsPostBack)
            {
                
            }
            else
            {
                Session["invalidloginattempt"] = null;
            }

        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string input = tb_userid.Text;
                bool lockstatus;
                DateTime locktimedate = DateTime.Now;
                SqlConnection connection = new SqlConnection(MYDBConnectionString);
                string sql = "select locked,lockdatetime FROM Account WHERE EMAIL = @userid";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@userid", input);

                try
                {
                    connection.Open();
                    command.CommandText = sql;
                    command.Connection = connection;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            if (reader["Locked"] != DBNull.Value)
                            {
                                lockstatus = Convert.ToBoolean(reader["Locked"]);
                                if (lockstatus == true)
                                {
                                    locktimedate = Convert.ToDateTime(reader["LockDateTime"].ToString());
                                    locktimedate = Convert.ToDateTime(locktimedate.ToString());
                                    DateTime cdatetime = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyy HH:mm:ss"));
                                    TimeSpan ts = cdatetime.Subtract(locktimedate);
                                    Int32 minuteslocked = Convert.ToInt32(ts.TotalMinutes);
                                    Int32 pendingminutes = 15 - minuteslocked;
                                    if (pendingminutes <= 0)
                                    {
                                        unlockaccount();
                                        SqlConnection con = new SqlConnection(MYDBConnectionString);
                                        string sql2 = "INSERT INTO Auditlog VALUES(@UserId,@LogAction,@LogDatetime,@LogNote)";
                                        SqlCommand cmd = new SqlCommand(sql2, connection);
                                        try
                                        {
                                            using (SqlDataAdapter sda = new SqlDataAdapter())
                                            {
                                                cmd.CommandType = CommandType.Text;
                                                cmd.Parameters.AddWithValue("@UserId", tb_userid.Text.Trim());
                                                cmd.Parameters.AddWithValue("@LogAction", "Account Login");
                                                cmd.Parameters.AddWithValue("@LogDatetime", DateTime.Now.ToString("dd/MM/yyyy"));
                                                cmd.Parameters.AddWithValue("@LogNote", "Account unlocked");
                                                con.Open();
                                                cmd.ExecuteNonQuery();

                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new Exception(ex.ToString());
                                        }

                                        finally { con.Close(); }
                                    }
                                    else
                                    {
                                        lbl_error.Text = "Your account has been locked for 15mins";
                                    }
                                }
                                else
                                {
                                    if (ValidateCaptcha())
                                    {
                                        string pwd = tb_pwd.Text.ToString().Trim();
                                        string userid = tb_userid.Text.ToString().Trim();

                                        SHA512Managed hashing = new SHA512Managed();
                                        string dbHash = getDBHash(userid);
                                        string dbSalt = getDBSalt(userid);
                                        try
                                        {
                                            if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                                            {
                                                string pwdWithSalt = pwd + dbSalt;
                                                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                                                string userHash = Convert.ToBase64String(hashWithSalt);

                                                if (userHash.Equals(dbHash))
                                                {
                                                    Session["UserID"] = userid;
                                                    string guid = Guid.NewGuid().ToString();
                                                    Session["AuthToken"] = guid;
                                                    Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                                                    SqlConnection con = new SqlConnection(MYDBConnectionString);
                                                    string sql2 = "INSERT INTO Auditlog VALUES(@UserId,@LogAction,@LogDatetime,@LogNote)";
                                                    SqlCommand cmd = new SqlCommand(sql2, con);
                                                    try
                                                    {
                                                        using (SqlDataAdapter sda = new SqlDataAdapter())
                                                        {
                                                            cmd.CommandType = CommandType.Text;
                                                            cmd.Parameters.AddWithValue("@UserId", tb_userid.Text.Trim());
                                                            cmd.Parameters.AddWithValue("@LogAction", "Account Login");
                                                            cmd.Parameters.AddWithValue("@LogDatetime", DateTime.Now.ToString("dd/MM/yyyy"));
                                                            cmd.Parameters.AddWithValue("@LogNote", "Account Login Successfully");
                                                            con.Open();
                                                            cmd.ExecuteNonQuery();

                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        throw new Exception(ex.ToString());
                                                    }

                                                    finally { con.Close(); }
                                                    Response.Redirect("HomePage.aspx", false);

                                                }
                                                else
                                                {
                                                    int attemptcount;
                                                    if (Session["invalidloginattempt"] != null)
                                                    {
                                                        attemptcount = Convert.ToInt16(Session["invalidloginattempt"].ToString());
                                                        attemptcount = attemptcount + 1;
                                                    }
                                                    else
                                                    {
                                                        attemptcount = 1;
                                                    }
                                                    Session["invalidloginattempt"] = attemptcount;
                                                    if (attemptcount == 3)
                                                    {
                                                        lbl_error.Text = "Your account has been locked for 15mins";
                                                        changelockstatus();
                                                        SqlConnection con = new SqlConnection(MYDBConnectionString);
                                                        string sql2 = "INSERT INTO Auditlog VALUES(@UserId,@LogAction,@LogDatetime,@LogNote)";
                                                        SqlCommand cmd = new SqlCommand(sql2, con);
                                                        try
                                                        {
                                                            using (SqlDataAdapter sda = new SqlDataAdapter())
                                                            {
                                                                cmd.CommandType = CommandType.Text;
                                                                cmd.Parameters.AddWithValue("@UserId", tb_userid.Text.Trim());
                                                                cmd.Parameters.AddWithValue("@LogAction", "Account Login");
                                                                cmd.Parameters.AddWithValue("@LogDatetime", DateTime.Now.ToString("dd/MM/yyyy"));
                                                                cmd.Parameters.AddWithValue("@LogNote", "Account is Locked");
                                                                con.Open();
                                                                cmd.ExecuteNonQuery();

                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            throw new Exception(ex.ToString());
                                                        }

                                                        finally { con.Close(); }
                                                    }
                                                    else
                                                    {
                                                        lbl_error.Text = "Userid or password is not valid. You have " + (3 - attemptcount) + " login attempts. Please try to login again";
                                                        SqlConnection con = new SqlConnection(MYDBConnectionString);
                                                        string sql2 = "INSERT INTO Auditlog VALUES(@UserId,@LogAction,@LogDatetime,@LogNote)";
                                                        SqlCommand cmd = new SqlCommand(sql2, con);
                                                        try
                                                        {
                                                            using (SqlDataAdapter sda = new SqlDataAdapter())
                                                            {
                                                                cmd.CommandType = CommandType.Text;
                                                                cmd.Parameters.AddWithValue("@UserId", tb_userid.Text.Trim());
                                                                cmd.Parameters.AddWithValue("@LogAction", "Account Login");
                                                                cmd.Parameters.AddWithValue("@LogDatetime", DateTime.Now.ToString("dd/MM/yyyy"));
                                                                cmd.Parameters.AddWithValue("@LogNote", "Account Login Unsuccessfully");
                                                                con.Open();
                                                                cmd.ExecuteNonQuery();

                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            throw new Exception(ex.ToString());
                                                        }

                                                        finally { con.Close(); }
                                                    }
                                                }
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            throw new Exception(ex.ToString());
                                        }
                                        finally { }
                                    }
                                    else
                                    {
                                        lblMessage.Text = "Validate captcha to prove that you are a human.";
                                    }
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }

                finally { connection.Close(); }
            }

        }

        protected string getDBSalt(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PASSWORDSALT FROM ACCOUNT WHERE EMAIL=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return s;

        }

        protected string getDBHash(string userid)
        {

            string h = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE EMAIL =@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return h;
        }

        public bool ValidateCaptcha()
        {
            bool result = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
                (" https://www.google.com/recaptcha/api/siteverify?secret=6LeR0RseAAAAAKTZMV5hboXX_HiPI7iRYMsK6d64 &response=" + captchaResponse);

            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        void changelockstatus()
        {
            string userid = tb_userid.Text;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string format = "MM/dd/yyy HH:mm:ss";
            string sql = "Update Account set Locked=1, Lockdatetime='" + DateTime.Now.ToString(format) + "' WHERE EMAIL = @userid";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@userid", userid);
            try
            {
                connection.Open();
                command.CommandText = sql;
                command.Connection = connection;
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
        }

        void unlockaccount()
        {
            string userid = tb_userid.Text;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "Update Account set Locked=0, Lockdatetime=NULL WHERE EMAIL = @userid";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@userid", userid);
            try
            {
                connection.Open();
                command.CommandText = sql;
                command.Connection = connection;
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
        }


    }
    
}