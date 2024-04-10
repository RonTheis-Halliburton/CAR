using CAR.App_Code;
using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;


namespace CAR
{
    public partial class Login : Page
    {

        MySession varSession = new MySession();

        string domain = ConfigurationManager.AppSettings["Domain"].ToString();
        string appVersion = ConfigurationManager.AppSettings["appVersion"].ToString();
        public string ErrException = "An error has occurred.  If problem persists, please contact the administrator: " + ConfigurationManager.AppSettings["AdminContact"];
        SanitizeString removeChar = new SanitizeString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MySession.Current.SessionUserID = null;
                this.Session.Abandon();

                this.Label1.Text = Server.HtmlEncode(appVersion);
                this.txtUserID.Focus();
                this.lblStatus.Text = "Not logged yet.";
                this.RadButton1.Text = "Log in";

            }
        }


        protected void RadButton1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Label7.Text = string.Empty;
                LdapAuthentication adAuth;
                adAuth = new LdapAuthentication();

                if (adAuth.IsAuthenticated(domain, Server.HtmlEncode(this.txtUserID.Text), Encrypt(Server.HtmlEncode(this.txtPCode.Text))))
                {
                    varSession.SessionUserID = Server.HtmlEncode(this.txtUserID.Text.Trim().ToLower());
                    MySession.Current.SessionUserID = Server.HtmlEncode(this.txtUserID.Text.Trim().ToLower());
                    Response.Redirect(url: "Default.aspx", endResponse: false);

                }
            }
            catch (Exception ex)
            {
                this.Session.Abandon();
                this.lblStatus.Text = "Error occured";
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Unable to insert line item.Reason: " + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            }
        }


        public string Encrypt(string textToBeEncrypted)
        {
            string keyStore = "YOUDUMMY";
            string encryptedData = string.Empty;

            RijndaelManaged rijndaelCipher = null;

            try
            {
                rijndaelCipher = new RijndaelManaged();

                byte[] plainText;
                plainText = System.Text.Encoding.Unicode.GetBytes(textToBeEncrypted);

                byte[] salt;
                salt = Encoding.ASCII.GetBytes(keyStore.Length.ToString());

                PasswordDeriveBytes secretKey;
                secretKey = new PasswordDeriveBytes(keyStore, salt, "SHA256", 100000);

                //Creates a symmetric encryptor object. 
                ICryptoTransform encryptor;
                encryptor = rijndaelCipher.CreateEncryptor(secretKey.GetBytes(cb: 32), secretKey.GetBytes(cb: 16));

                MemoryStream memoryStream;
                memoryStream = new MemoryStream();

                //Defines a stream that links data streams to cryptographic transformations
                CryptoStream cryptoStream;
                cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

                cryptoStream.Write(plainText, offset: 0, count: plainText.Length);

                //Writes the final state and clears the buffer
                cryptoStream.FlushFinalBlock();
                byte[] cipherBytes;
                cipherBytes = memoryStream.ToArray();
                memoryStream.Close();
                //cryptoStream.Close();

                encryptedData = Convert.ToBase64String(cipherBytes);
            }
            //catch (Exception ex)
            //{
            //    LogException(ex);
            //}
            finally
            {
                if (rijndaelCipher != null)
                {
                    rijndaelCipher.Clear();
                }
            }

            return encryptedData;
        }


        // Application event log
        private void LogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog();

            appLog.ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionSource = "Login.aspx";
            appLog.ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionUrl = string.Empty;
            appLog.ExceptionTargetSite = ex.TargetSite.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionStackTrace = ex.StackTrace.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.AppLogEvent();
        }
    }
}