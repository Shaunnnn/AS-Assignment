using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AS_Assignment
{
    public partial class Verify : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            Label3.Text = "The activation code has been sent to " + Request.QueryString["emailadd"].ToString();
        }

        protected void btn_Verify_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select * FROM ACCOUNT WHERE EMAIL='" + Request.QueryString["emailadd"] + "'";
            SqlCommand command = new SqlCommand(sql, connection);
            try
            {
                connection.Open();
                command.CommandText = sql;
                command.Connection = connection;
                command.ExecuteNonQuery();
                string activationcode;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Activationcode"] != DBNull.Value)
                        {
                            activationcode = reader["Activationcode"].ToString();
                            if(activationcode == tb_vcode.Text)
                            {
                                changestatus();

                                SqlConnection con = new SqlConnection(MYDBConnectionString);
                                string sql2 = "INSERT INTO Auditlog VALUES(@UserId,@LogAction,@LogDatetime,@LogNote)";
                                SqlCommand cmd = new SqlCommand(sql2, con);
                                try
                                {
                                    using (SqlDataAdapter sda = new SqlDataAdapter())
                                    {
                                        cmd.CommandType = CommandType.Text;
                                        cmd.Parameters.AddWithValue("@UserId", Request.QueryString["emailadd"]);
                                        cmd.Parameters.AddWithValue("@LogAction", "Account verification");
                                        cmd.Parameters.AddWithValue("@LogDatetime", DateTime.Now.ToString("dd/MM/yyyy"));
                                        cmd.Parameters.AddWithValue("@LogNote", "Account Verified");
                                        con.Open();
                                        cmd.ExecuteNonQuery();

                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception(ex.ToString());
                                }

                                finally { con.Close(); }
                                Response.Redirect("Login.aspx?msg= Your account has been verified!", false);

                            }
                            else
                            {
                                lbl_error.Text = "You have entered invalid Activation Code, kindly check your mail inbox";
                                SqlConnection con = new SqlConnection(MYDBConnectionString);
                                string sql2 = "INSERT INTO Auditlog VALUES(@UserId,@LogAction,@LogDatetime,@LogNote)";
                                SqlCommand cmd = new SqlCommand(sql2, con);
                                try
                                {
                                    using (SqlDataAdapter sda = new SqlDataAdapter())
                                    {
                                        cmd.CommandType = CommandType.Text;
                                        cmd.Parameters.AddWithValue("@UserId", Request.QueryString["emailadd"]);
                                        cmd.Parameters.AddWithValue("@LogAction", "Account verification");
                                        cmd.Parameters.AddWithValue("@LogDatetime", DateTime.Now.ToString("dd/MM/yyyy"));
                                        cmd.Parameters.AddWithValue("@LogNote", "Account fail to Verify");
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
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
        }

        private void changestatus()
        {
            var emailadd = Request.QueryString["emailadd"];
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "Update Account set Status='Verified' WHERE EMAIL = '" + emailadd + "'";
            SqlCommand command = new SqlCommand(sql, connection);
            try
            {
                connection.Open();
                command.CommandText = sql;
                command.Parameters.AddWithValue("@Status", "Verified");
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