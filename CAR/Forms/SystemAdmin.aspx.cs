using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;


namespace CAR.Forms
{
    public partial class SystemAdmin : System.Web.UI.Page
    {
        public string AdminContact = ConfigurationManager.AppSettings["AdminContact"].ToString();
        public string ErrException = "If problem persists, please contact the administrator: " + ConfigurationManager.AppSettings["AdminContact"];
        SanitizeString removeChar = new SanitizeString();

        MyUtil.HtmlUtility htmlUtil = MyUtil.HtmlUtility.Instance;
        Helpers.HtmlUtility antiXss = Helpers.HtmlUtility.Instance;

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
                LoadPortalStatus();
            }
        }

        protected void LoadPortalStatus()
        {
            //try
            //{
            if (this.RadComboBox22.SelectedIndex == 0) //Internal
            {
                GetMaintenance getMain;
                getMain = new GetMaintenance();
                using (DataTable dt = getMain.Exec_GetMaintenanceInternal())
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        this.RadComboBox23.SelectedValue = row["SiteStatus_Flag"].ToString();

                        this.RadTextBox4.Text = htmlUtil.SanitizeHtml(Server.HtmlEncode(row["OffLineComments"].ToString()));
                        this.RadTextBox5.Text = htmlUtil.SanitizeHtml(Server.HtmlEncode(row["Notification"].ToString()));

                        this.RadNumericTextBox2.Text = htmlUtil.SanitizeHtml(Server.HtmlEncode(row["NotificationAutoCloseDelay"].ToString()));
                        this.RadNumericTextBox3.Text = htmlUtil.SanitizeHtml(Server.HtmlEncode(row["NotificationShowInterval"].ToString()));

                        RadComboBoxItem comboItem24 = this.RadComboBox24.FindItemByValue(row["NotificationFlag"].ToString());
                        if (comboItem24 != null)
                        {
                            comboItem24.Selected = true;
                        }

                        RadComboBoxItem comboItem25 = this.RadComboBox25.FindItemByValue(row["NotificationPosition"].ToString());
                        if (comboItem25 != null)
                        {
                            comboItem25.Selected = true;
                        }

                    }
                }
            }
            else
            {
                GetMaintenance getMain;
                getMain = new GetMaintenance();
                using (DataTable dt = getMain.Exec_GetMaintenanceExternal())
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        this.RadComboBox23.SelectedValue = row["SiteStatus_Flag"].ToString();

                        this.RadTextBox4.Text = htmlUtil.SanitizeHtml(Server.HtmlEncode(row["OffLineComments"].ToString()));
                        this.RadTextBox5.Text = htmlUtil.SanitizeHtml(Server.HtmlEncode(row["Notification"].ToString()));

                        this.RadNumericTextBox2.Text = htmlUtil.SanitizeHtml(Server.HtmlEncode(row["NotificationAutoCloseDelay"].ToString()));
                        this.RadNumericTextBox3.Text = htmlUtil.SanitizeHtml(Server.HtmlEncode(row["NotificationShowInterval"].ToString()));

                        RadComboBoxItem comboItem24 = this.RadComboBox24.FindItemByValue(row["NotificationFlag"].ToString());
                        if (comboItem24 != null)
                        {
                            comboItem24.Selected = true;
                        }

                        RadComboBoxItem comboItem25 = this.RadComboBox25.FindItemByValue(row["NotificationPosition"].ToString());
                        if (comboItem25 != null)
                        {
                            comboItem25.Selected = true;
                        }

                    }
                }
            }

            //GetMaintenance getMain;
            //getMain = new GetMaintenance();
            //using (DataTable dt = getMain.Exec_GetMaintenance())
            //{
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        if (this.RadComboBox22.SelectedIndex == 0) //Internal
            //        {
            //            this.RadComboBox23.SelectedValue = row["InternalSite"].ToString();
            //        }
            //        else
            //        {
            //            this.RadComboBox23.SelectedValue = row["ExternalSite"].ToString();
            //        }

            //        this.RadTextBox4.Text = row["OffLineComments"].ToString();
            //        this.RadTextBox5.Text = row["Notification"].ToString();

            //        this.RadNumericTextBox2.Text = row["NotificationAutoCloseDelay"].ToString();
            //        this.RadNumericTextBox3.Text = row["NotificationShowInterval"].ToString();

            //        RadComboBoxItem comboItem24 = this.RadComboBox24.FindItemByValue(row["NotificationFlag"].ToString());
            //        if (comboItem24 != null)
            //        {
            //            comboItem24.Selected = true;
            //        }

            //        RadComboBoxItem comboItem25 = this.RadComboBox25.FindItemByValue(row["NotificationPosition"].ToString());
            //        if (comboItem25 != null)
            //        {
            //            comboItem25.Selected = true;
            //        }

            //    }
            //}
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);

            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
        }


        protected void RadComboBox22_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            LoadPortalStatus();
        }

        protected void RadButton12s_Click(object sender, EventArgs e)
        {
            try
            {
                Maintenance upt;
                upt = new Maintenance
                {
                    Portal = this.RadComboBox22.SelectedValue,
                    Status = this.RadComboBox23.SelectedValue,
                    OfflineComments = removeChar.SanitizeQuoteString(Server.HtmlEncode(antiXss.SanitizeHtml(RadTextBox4.Text.Trim()))).ToString(),
                    NotificationFlag = this.RadComboBox24.SelectedValue,
                    Notification = removeChar.SanitizeQuoteString(Server.HtmlEncode(antiXss.SanitizeHtml(RadTextBox5.Text.Trim()))).ToString(),
                    NotificationPosition = this.RadComboBox25.SelectedValue,
                    NotificationAutoCloseDelay = this.RadNumericTextBox2.Text,
                    NotificationShowInterval = this.RadNumericTextBox3.Text
                };

                Label4.Text = upt.Exec_UpdatePortal();

            }
            catch (Exception ex)
            {
                LogException(ex);

            }
        }

        protected void RadTabStrip1_TabClick(object sender, RadTabStripEventArgs e)
        {
            this.RadTabStrip1.SelectedIndex = e.Tab.Index;

            if (e.Tab.Index == 0)
            {
                this.RadTextBox1.Focus();
            }

            if (e.Tab.Index == 1)
            {
                LoadPortalStatus();
                this.RadComboBox22.Focus();
            }
        }

        // Application event log
        private void LogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog();

            appLog.ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionSource = "SystemAdmin.aspx";
            appLog.ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionUrl = string.Empty;
            appLog.ExceptionTargetSite = ex.TargetSite.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionStackTrace = ex.StackTrace.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.AppLogEvent();
        }
    }
}