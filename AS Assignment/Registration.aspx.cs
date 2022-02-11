using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.IO;
using System.Configuration;

namespace AS_Assignment
{
    public partial class Registration : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDB"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        private object activationcode;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [Obsolete]
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                int scores = checkPassword(tb_password.Text);
                string status = "";
                switch (scores)
                {
                    case 1:
                        status = "Very Weak";
                        break;
                    case 2:
                        status = "Weak";
                        break;
                    case 3:
                        status = "Medium";
                        break;
                    case 4:
                        status = "strong";
                        break;
                    case 5:
                        status = "very strong";
                        break;
                    default:
                        break;
                }
                lbl_feedback.Text = "Your password is " + status;
                if (scores < 4)
                {
                    lbl_feedback.ForeColor = Color.Red;
                    return;
                }
                lbl_feedback.ForeColor = Color.Green;

                string pwd = tb_password.Text.ToString().Trim(); ;
            
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

                createAccount();

                SqlConnection connection = new SqlConnection(MYDBConnectionString);
                string sql = "INSERT INTO Auditlog VALUES(@UserId,@LogAction,@LogDatetime,@LogNote)";
                SqlCommand command = new SqlCommand(sql, connection);
                try
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@UserId", tb_email.Text.Trim());
                        command.Parameters.AddWithValue("@LogAction", "Register");
                        command.Parameters.AddWithValue("@LogDatetime", DateTime.Now.ToString("dd/MM/yyyy"));
                        command.Parameters.AddWithValue("@LogNote", "Registered Successfully");
                        connection.Open();
                        command.ExecuteNonQuery();

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }

                finally { connection.Close(); }

                Response.Redirect("Verify.aspx?emailadd=" + HttpUtility.UrlEncode(tb_email.Text));
            }
        }

        [Obsolete]
        protected void createAccount()
        {
            string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
            string filePath = "~/Uploads/" + fileName;
            FileUpload1.PostedFile.SaveAs(Server.MapPath(filePath));
            Random random = new Random();
            activationcode = random.Next(1001, 9999).ToString();
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@FirstName,@LastName,@CreditCard,@Email,@PasswordHash,@PasswordSalt,@DOB,@Image,@IV,@Key,@Locked,@Lockdatetime,@Status,@Activationcode)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FirstName", tb_firstName.Text.Trim());
                            cmd.Parameters.AddWithValue("@LastName", tb_lastName.Text.Trim());
                            cmd.Parameters.AddWithValue("@Creditcard", Convert.ToBase64String(encryptData(tb_creditCard.Text.Trim())));
                            cmd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@DOB", tb_DoB.Text.Trim());
                            cmd.Parameters.AddWithValue("@Image", filePath);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@Locked", 0);
                            cmd.Parameters.AddWithValue("@Lockdatetime", DBNull.Value);
                            cmd.Parameters.AddWithValue("@Status", "Unverified");
                            cmd.Parameters.AddWithValue("@Activationcode", activationcode);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            sendcode();

        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }

        private int checkPassword(string password)
        {
            int score = 0;
            if (password.Length < 12)
            {
                return 1;
            }
            else
            {
                score = 1;
            }
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
                if (Regex.IsMatch(password, "[A-Z]"))
                {
                    score++;
                    if (Regex.IsMatch(password, "[0-9]"))
                    {
                        score++;
                        if (Regex.IsMatch(password, "[^A-Za-z0-9]"))
                        {
                            score++;
                        }

                    }
                }
            }
            return score;
        }

        [Obsolete]
        private void sendcode()
        {
            var fromEmail = ConfigurationSettings.AppSettings["mail"];
            var fromPassword = ConfigurationSettings.AppSettings["pass"];
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential(fromEmail,fromPassword);
            smtp.EnableSsl = true;
            MailMessage msg = new MailMessage();
            msg.Subject = "Activation Code to Verify Email Address";
            msg.Body = "Dear " + tb_firstName.Text + " " + tb_lastName.Text + ",\nYour Activation Code is: " + activationcode + "\n\n\nThanks & Regards,\nSITConnect";
            string toaddress = tb_email.Text;
            msg.To.Add(toaddress);
            string fromaddress = "SITConnect <sitconnect4u@gmail.com>";
            msg.From = new MailAddress(fromaddress);
            try
            {
                smtp.Send(msg);
            }
            catch
            {
                throw;
            }

        }
    }
}