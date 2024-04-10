using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CAR
{
    public partial class Main : Page
    {
        private int internalSite = 0;  //-- 0=site on, 1= site off
        private int notificationFlag = 0;  //-- 0=notifcation off, 1= notification on

        public string AppVersion = ConfigurationManager.AppSettings["AppVersion"].ToString();
        public string AppLastUpdated = ConfigurationManager.AppSettings["AppLastUpdated"].ToString();
        public string AdminContact = ConfigurationManager.AppSettings["AdminContact"].ToString();
        public string DataMartConStr = ConfigurationManager.ConnectionStrings["dbConnDataMart"].ConnectionString;
        public string SmtpHost = ConfigurationManager.AppSettings["SmtpHostMail"].ToString();
        public string ErrMessage = ConfigurationManager.AppSettings["ErrorMessage"].ToString();
        private static readonly SanitizeString sanitizeString = new SanitizeString();
        readonly SanitizeString removeChar = sanitizeString;
        private static readonly MyUtil.HtmlUtility instance = MyUtil.HtmlUtility.Instance;
        readonly MyUtil.HtmlUtility htmlUtil = instance;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetAllowResponseInBrowserHistory(allow: false);

            if (MySession.Current.SessionUserID == null || string.IsNullOrEmpty(MySession.Current.SessionUserID.ToString()))
            {
                this.Session.Abandon();
                Response.Redirect(url: "Default.aspx");
            }

            if (!Page.IsPostBack)
            {
                try
                {
                    GetMaintenance getMain;
                    getMain = new GetMaintenance();
                    using (DataTable dt = getMain.Exec_GetMaintenanceInternal())
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (htmlUtil.IsNumeric(row["SiteStatus_Flag"].ToString()))
                            {
                                internalSite = int.Parse(row["SiteStatus_Flag"].ToString());
                                notificationFlag = int.Parse(row["NotificationFlag"].ToString());

                                if (internalSite == 0)
                                {
                                    this.RadPushButton1.Enabled = false;

                                    this.Div1.InnerText = Server.HtmlDecode(row["OfflineComments"].ToString().Trim());

                                    if (!MySession.Current.SessionSystemAdmin)
                                    {
                                        this.RadMenu1.Enabled = false;
                                        this.RadNotification1.ShowCloseButton = false;
                                    }
                                    this.RadNotification1.Show();
                                }

                                if (notificationFlag == 1)
                                {
                                    this.Div2.InnerText = Server.HtmlDecode(row["Notification"].ToString().Trim());

                                    this.RadNotification2.AutoCloseDelay = int.Parse(row["NotificationAutoCloseDelay"].ToString());
                                    this.RadNotification2.ShowInterval = int.Parse(row["NotificationShowInterval"].ToString());
                                    this.RadNotification2.Position = (NotificationPosition)Enum.Parse(typeof(Telerik.Web.UI.NotificationPosition), row["NotificationPosition"].ToString());
                                    this.RadNotification2.Show();

                                }
                            }
                        }
                    }

                    this.Label1.Text = MySession.Current.SessionFullName;
                    this.Label200.Text = " [" + MySession.Current.SessionUserID + "]";
                    this.Label500.Text = "Version:  " + AppVersion;
                    this.Label600.Text = "Last Updated:  " + AppLastUpdated;

                    this.RadPushButton1.Text = "Not " + MySession.Current.SessionFullName + "? ";
                    this.RadMenu1.Items[0].Selected = true;
                    this.RadPane1.ContentUrl = "~/Forms/Active.aspx";

                    foreach (RadMenuItem item in this.RadMenu1.Items)
                    {
                        if (item.Value == "SystemAdmin")
                        {
                            item.Visible = MySession.Current.SessionSystemAdmin;
                        }
                    }

                    if (MySession.Current.SessionWebServer.ToString() == "localhost"
                   || MySession.Current.SessionWebServer.ToString() == "np2apph786v")
                    {
                        this.Label8.Font.Size = new FontUnit(value: 10);
                        this.Label8.ForeColor = Color.Red;
                        this.Label8.Text = " - NOTE:  This site is for testing ONLY!";
                        MySession.Current.SessionWebServer = "TESTSITE";
                    }


                    //+++++++++++ Direct to page clicked from email
                    if (!string.IsNullOrEmpty(MySession.Current.SessionUrlForm.ToString()))
                    {
                        if (MySession.Current.SessionUrlForm.ToString().ToUpper() == "CAR")
                        {
                            this.RadMenu1.Items[2].Selected = true;
                            this.RadPane1.ContentUrl = "Forms/Search.aspx";
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogException(ex);

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
                }
            }
        }

        protected void RadPushButton1_Click(object sender, EventArgs e)
        {
            this.Label1.Text = string.Empty;
            this.Label200.Text = string.Empty;
            this.Session.Abandon();
            this.Session.Clear();
            Response.Redirect(url: "Login.aspx");
        }

        // Application event log
        private void LogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog
            {
                ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionSource = "Internal_Main.aspx",
                ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionUrl = MySession.Current.SessionHostName.ToString().ToUpper(),
                ExceptionTargetSite = ex.TargetSite.ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionStackTrace = ex.StackTrace.ToString().Replace(oldValue: "'", newValue: "`")
            };
            appLog.AppLogEvent();
        }

    }
}