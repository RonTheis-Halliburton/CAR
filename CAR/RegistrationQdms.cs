using CAR.App_Code;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;

namespace CAR
{
    public partial class RegistrationQdms : Page
    {
        public string UserAccess = "Error..Unable to locate your access profile for QDMS.  Click on the Registration button to register for access.  If you feel this is an error, please contact FHOUNBCIT";

        public string ErrException = "If problem persists, please contact the administrator: " + ConfigurationManager.AppSettings["AdminContact"];
        string adminContact = ConfigurationManager.AppSettings["AdminContact"].ToString();
        string smtpHost = ConfigurationManager.AppSettings["SMTPHostMail"].ToString();
        string[] arrayIdentity;

        GetRecord getRecord = new GetRecord();
        SanitizeString removeChar = new SanitizeString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (MySession.Current.SessionUserID == null || string.IsNullOrEmpty(MySession.Current.SessionUserID))
            {
                this.Session.Abandon();
                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "ParentPage();", addScriptTags: true);
            }


            if (!IsPostBack)
            {
                this.Label3.Font.Size = 12;
                this.Label3.Font.Bold = true;
                this.Label3.ForeColor = Color.Red;

                this.Label3.Text = UserAccess;


                arrayIdentity = HttpContext.Current.Request.LogonUserIdentity.Name.ToString().Trim().ToLower().Split(separator: new char[] { '\\' });
                Ldap userExist;
                userExist = new Ldap();

                userExist.UserID = arrayIdentity[1].ToString().Trim().ToLower();

                if (userExist.IsUserExists())
                {
                    this.RadTextBox1.Text = arrayIdentity[1].ToString().Trim().ToLower();

                    HesUsers hesUser;
                    hesUser = new HesUsers();
                    hesUser.UserID = MySession.Current.SessionUserID;
                    hesUser.Email = string.Empty;
                    hesUser.DisplayName = string.Empty;

                    using (DataTable dt = hesUser.GetPrnl())
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            this.RadTextBox2.Text = row["FirstName"].ToString();
                            this.RadTextBox3.Text = row["LastName"].ToString();

                            this.HiddenField1.Value = row["Email"].ToString();
                            this.HiddenField2.Value = row["City"].ToString();
                            this.HiddenField3.Value = row["CNTRY_NM"].ToString();
                        }
                    }

                    LoadPlant();
                }
            }
        }

        protected void LoadPlant()
        {
            using (DataTable dt = getRecord.Exec_GetQdmsPlant_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    RadComboBoxItem item;
                    item = new RadComboBoxItem();
                    item.Value = row["Oid"].ToString().Trim();
                    item.Text = row["PlantName"].ToString().Trim();
                    item.ToolTip = row["PlantName"].ToString();
                    this.RadComboBox2.Items.Add(item);
                }
            }
        }

        protected void RadButton1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Label1.Text = "Your information has been submitted successfully.";
                this.Label2.Text = "Once access has been set up, you will receive a notification via Email.";

                CallSendMsg();

                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "CloseRegistration('Registration');", addScriptTags: true);

            }
            catch (Exception ex)
            {
                LogException(ex);

                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            }
        }


        void CallSendMsg()
        {
            using (MailMessage objEmail = new MailMessage())
            {
                MailAddress frmAddr;
                frmAddr = new MailAddress(this.HiddenField1.Value, this.HiddenField1.Value);

                string bodyMsg = string.Empty;
                string bobyTestWeb = string.Empty;

                objEmail.From = frmAddr;
                objEmail.To.Add(adminContact);
                objEmail.CC.Add(frmAddr);

                objEmail.IsBodyHtml = true;
                objEmail.Subject = "Corrective Action/QDMS - User Registration";
                bodyMsg += "<table>";
                bodyMsg += "<tr><td><font size=2 face=Arial><b>User ID:</b></font></td><td><font size=2 face=Arial>" + Server.HtmlEncode(this.RadTextBox1.Text) + "</font></td></tr>";
                bodyMsg += "<tr><td><font size=2 face=Arial><b>First Name:</b></font></td><td><font size=2 face=Arial>" + Server.HtmlEncode(this.RadTextBox2.Text) + "</font></td></tr>";
                bodyMsg += "<tr><td><font size=2 face=Arial><b>Last Name:</b></font></td><td><font size=2 face=Arial>" + Server.HtmlEncode(this.RadTextBox3.Text) + "</font></td></tr>";
                bodyMsg += "<tr><td><font size=2 face=Arial><b>Plant(s):</b></font></td><td></td></tr>";

                foreach (RadComboBoxItem item in this.RadComboBox2.Items)
                {
                    if (item.Checked)
                    {
                        bodyMsg += "<tr><td></td><td><font size=2 face=Arial>" + item.Text + "</font></td></tr>";
                    }
                }

                bodyMsg += "<tr><td valign=top cospan=2><font size=2 face=Arial><b>What is your purpose for requesting access?</b></font></td><td></td></tr>";
                bodyMsg += "<tr><td cospan=2><font size=2 face=Arial><pre>" + Server.HtmlEncode(this.txtPurpose.Text) + "</pre></font></td></tr>";
                bodyMsg += "</table>";

                objEmail.Priority = MailPriority.High;
                objEmail.Body = bobyTestWeb + bodyMsg;

                using (SmtpClient smtpMailObj = new SmtpClient())
                {
                    smtpMailObj.Host = smtpHost;
                    smtpMailObj.Send(objEmail);
                }
            }
        }

        private void LogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog();

            appLog.ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionSource = "Registration.aspx";
            appLog.ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionUrl = MySession.Current.SessionWebServer.ToString().ToUpper();
            appLog.ExceptionTargetSite = ex.TargetSite.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionStackTrace = ex.StackTrace.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.AppLogEvent();
        }


    }
}