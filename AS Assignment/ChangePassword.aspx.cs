using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AS_Assignment
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDB"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Change_Click(object sender, EventArgs e)
        {

            string pwd = tb_npwd.Text.ToString().Trim(); ;

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];

            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);

            SHA512Managed hashing = new SHA512Managed();

            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

            finalHash = Convert.ToBase64String(hashWithSalt);

            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;

            changePassword();
            SqlConnection con = new SqlConnection(MYDBConnectionString);
            string sql2 = "INSERT INTO Auditlog VALUES(@UserId,@LogAction,@LogDatetime,@LogNote)";
            SqlCommand cmd = new SqlCommand(sql2, con);
            try
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@UserId", tb_userid.Text.Trim());
                    cmd.Parameters.AddWithValue("@LogAction", "Account Change Password");
                    cmd.Parameters.AddWithValue("@LogDatetime", DateTime.Now.ToString("dd/MM/yyyy"));
                    cmd.Parameters.AddWithValue("@LogNote", "Account Password Changed");
                    con.Open();
                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { con.Close(); }
            string verify = "password changed!";
            Response.Redirect("Login.aspx?msg= " + HttpUtility.UrlEncode(verify), false);

        }

        protected void changePassword()
        {
            string userid = tb_userid.Text;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "Update Account set PasswordHash= '" + finalHash + "', PasswordSalt= '" + salt + "' WHERE EMAIL = @userid";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@userid", userid);
            try
            {
                connection.Open();
                command.CommandText = sql;
                command.Parameters.AddWithValue("@PasswordHash", finalHash);
                command.Parameters.AddWithValue("@PasswordSalt", salt);
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