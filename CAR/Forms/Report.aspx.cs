using System;
using System.Data;
using System.Web.UI;

namespace CAR.Forms
{
    public partial class Report : System.Web.UI.Page
    {
        SanitizeString removeChar = new SanitizeString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (MySession.Current.SessionUserID == null || string.IsNullOrEmpty(MySession.Current.SessionUserID))
            {
                this.Session.Abandon();
                string script = "GetParentPage();";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", script, true);
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
                            if (!string.IsNullOrEmpty(row["QlikViewReportLink"].ToString()))
                            {
                                this.LinkButton1.Attributes.Add(key: "href", value: row["QlikViewReportLink"].ToString());
                                this.LinkButton1.Attributes.Add(key: "Target", value: "_blank");

                                string strScrip = "window.open('" + row["QlikViewReportLink"].ToString() + "', 'windowScript', 'Target=_blank,menubar=yes,location=yes,resizable=yes,scrollbars=yes,status=yes');";
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "windowScript", script: strScrip, addScriptTags: true);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogException(ex);

                    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
                }
            }
        }

        // Application event log
        private void LogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog();

            appLog.ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionSource = "Internal_Report.aspx";
            appLog.ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionUrl = MySession.Current.SessionHostName.ToString().ToUpper();
            appLog.ExceptionTargetSite = ex.TargetSite.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionStackTrace = ex.StackTrace.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.AppLogEvent();
        }
    }
}