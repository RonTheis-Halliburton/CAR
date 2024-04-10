using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web.UI;
using Telerik.Web.UI;

namespace CAR.Forms
{
    public partial class InputBox : System.Web.UI.Page
    {
        public string AdminContact = ConfigurationManager.AppSettings["AdminContact"].ToString();
        public string ErrException = "If problem persists, please contact the administrator: " + ConfigurationManager.AppSettings["AdminContact"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (MySession.Current.SessionUserID == null || string.IsNullOrEmpty(MySession.Current.SessionUserID))
            {
                this.Session.Abandon();
                string script = "GetParentPageCreate();";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", script, true);
            }
        }

        protected void RadComboBox5_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            int itemsPerRequest = 100;
            try
            {

                SanitizeString removeChar;
                removeChar = new SanitizeString();
                string srchText = removeChar.SanitizeTextString(e.Text.Trim());


                GetRecord getNbr;
                getNbr = new GetRecord();
                getNbr.CarNbr = srchText;

                DataTable dt = getNbr.Exec_GetCarNbr_Datatable();

                int itemOffset = e.NumberOfItems;
                int endOffset = Math.Min(itemOffset + itemsPerRequest, dt.Rows.Count);
                e.EndOfItems = endOffset == dt.Rows.Count;

                for (int i = itemOffset; i < endOffset; i++)
                {
                    RadComboBoxItem item;
                    item = new RadComboBoxItem();

                    item.Font.Bold = true;

                    item.Text = dt.Rows[i]["CAR_NBR"].ToString().Trim() + " " + dt.Rows[i]["DEL_STATUS"].ToString().Trim();
                    item.ToolTip = dt.Rows[i]["CAR_NBR"].ToString().Trim();
                    item.Value = dt.Rows[i]["OID"].ToString().Trim();

                    item.Attributes.Add(key: "OID", value: dt.Rows[i]["OID"].ToString().Trim());
                    item.Attributes.Add(key: "CAR_NBR", value: dt.Rows[i]["CAR_NBR"].ToString().Trim());


                    if (bool.Parse(dt.Rows[i]["DEL_FLG"].ToString()) == true)
                    {
                        item.Enabled = false;
                        item.ForeColor = Color.Red;
                    }

                    this.RadComboBox5.Items.Add(item);
                    item.DataBind();
                }
                e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
            }
            catch (Exception ex)
            {
                LogException(ex);
                e.Message = "No matches - " + ex.Message;
            }
        }


        private static string GetStatusMessage(int offset, int total)
        {
            if (total <= 0)
                return "No matches";

            return String.Format(format: "<b>1</b>-<b>{0}</b> out of <b>{1}</b>", arg0: offset, arg1: total);
        }

        // Application event log
        private void LogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog();

            appLog.ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionSource = "InputBox.aspx";
            appLog.ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionUrl = string.Empty;
            appLog.ExceptionTargetSite = ex.TargetSite.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionStackTrace = ex.StackTrace.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.AppLogEvent();
        }
    }
}