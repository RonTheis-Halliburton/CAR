using CAR.App_Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;


namespace CAR
{
    public partial class Default : Page
    {
        public string ErrException = "If problem persists, please contact the administrator: " + ConfigurationManager.AppSettings["AdminContact"];
        SanitizeString removeChar = new SanitizeString();
        MySession varSession = new MySession();
        public string[] ArrayIdentity;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                try
                {
                    //if (MySessionVar.SessionUserID == null || string.IsNullOrEmpty(MySessionVar.SessionUserID.ToString().Trim()))
                    //{
                    ArrayIdentity = HttpContext.Current.Request.LogonUserIdentity.Name.ToString().Trim().ToLower().Split(separator: new char[] { '\\' });

                    if (ArrayIdentity.Length > 0)
                    {
                        //varSession.SessionUserID = ArrayIdentity[1].ToString();
                        //varSession.SessionDomainName = ArrayIdentity[0].ToString();

                        MySession.Current.SessionDomainName = ArrayIdentity[0].ToString();
                        MySession.Current.SessionUserID = ArrayIdentity[1].ToString();
                    }
                    //}

                    MySession.Current.SessionWebServer = Request.ServerVariables["SERVER_NAME"].ToString().Replace(oldValue: ".corp.halliburton.com", newValue: string.Empty).Replace(oldValue: ".halliburton.com", newValue: string.Empty);
                    MySession.Current.SessionHostName = Environment.GetEnvironmentVariable(variable: "COMPUTERNAME").ToString().ToUpper();

                    Ldap ldap;
                    ldap = new Ldap
                    {
                        UserID = MySession.Current.SessionUserID.ToString()
                    };

                    if (ldap.IsUserExists())
                    {

                        if (Request.QueryString["form"] != null && Request.QueryString["Indx"] != null)
                        {
                            MySession.Current.SessionUrlForm = Server.HtmlEncode(Request.QueryString["form"].ToString());
                            MySession.Current.SessionUrlNdex = Server.HtmlEncode(Request.QueryString["Indx"].ToString());
                        }

                        LoadUserInfo();
                        Response.Redirect(url: "Main.aspx", endResponse: false);

                    }
                    else
                    {
                        this.Session.Abandon();
                        Response.Redirect(url: "Authorize.htm", endResponse: false);
                    }
                }
                catch (Exception ex)
                {
                    LogException(ex);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + ErrException + "<br>" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
                }
            }
        }


        protected void LoadUserInfo()
        {
            //try
            //{
            Ldap userAD;
            userAD = new Ldap
            {
                UserID = MySession.Current.SessionUserID
            };
            List<HesUsers> listOfProperty = userAD.GetADUsers();

            foreach (HesUsers hesUsers in listOfProperty)
            {
                MySession.Current.SessionEmail = hesUsers.Email;
                MySession.Current.SessionFullName = hesUsers.DisplayName;
                MySession.Current.SessionFirstName = hesUsers.GivenName;
                MySession.Current.SessionLastName = hesUsers.SN;
                MySession.Current.SessionUserCity = hesUsers.City;
                MySession.Current.SessionUserCountry = hesUsers.Country;
                MySession.Current.SessionManager = hesUsers.Manager;
                MySession.Current.SessionManagerID = hesUsers.ManagerID;
                MySession.Current.SessionManagerEmail = hesUsers.ManagerEmail;
            }

            HesUsers hesUser;
            hesUser = new HesUsers
            {
                UserID = MySession.Current.SessionUserID
            };
            using (DataTable dt = hesUser.Get_System_Admin())
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        MySession.Current.SessionCarAdmin = bool.Parse(row["ADMIN"].ToString());
                        MySession.Current.SessionSystemAdmin = bool.Parse(row["SYSTEM_ADMIN"].ToString());
                    }
                }
                else
                {
                    MySession.Current.SessionCarAdmin = false;
                    MySession.Current.SessionSystemAdmin = false;
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);
            //    ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + ErrException + "<br>" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
        }




        // Application event log
        private void LogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog
            {
                ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionSource = "Internal_Default.aspx",
                ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionUrl = MySession.Current.SessionHostName.ToString().ToUpper(),
                ExceptionTargetSite = ex.TargetSite.ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionStackTrace = ex.StackTrace.ToString().Replace(oldValue: "'", newValue: "`")
            };
            appLog.AppLogEvent();
        }
    }
}
