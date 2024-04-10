using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace CAR.Forms
{
    public partial class ViewDocx : System.Web.UI.Page
    {
        public string AdminContact = ConfigurationManager.AppSettings["AdminContact"].ToString();
        public string ErrMessage = ConfigurationManager.AppSettings["ErrorMessage"].ToString();
        public string ErrException = "If problem persists, please contact the administrator: " + ConfigurationManager.AppSettings["AdminContact"];

        SanitizeString removeChar = new SanitizeString();
        MyUtil.HtmlUtility htmlUtil = MyUtil.HtmlUtility.Instance;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["OID"] != null && Request.QueryString["Car_Nbr"] != null)
            {
                if (Int32.Parse(Request.QueryString["OID"].ToString()) > 0)
                {
                    PrintToWord(Request.QueryString["OID"].ToString(), Request.QueryString["Car_Nbr"].ToString());
                }
                else
                {
                    Response.Write("Invalid Record ID.  Please verify and try again.");
                }
            }
        }


        private void PrintToWord(string CarOid, string CarNbr)
        {
            try
            {
                string fileName = CarNbr + ".docx";


                CreateDocx docx = new CreateDocx();
                docx.Car_Oid = CarOid;

                using (MemoryStream mStream = docx.GetDocxFile())
                {
                    byte[] renderedBytes = null;
                    renderedBytes = mStream.ToArray();

                    Response.AppendHeader("Content-Disposition", Convert.ToString("attachment; filename=") + fileName);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

                    Response.BinaryWrite(renderedBytes);

                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name.ToString() != "ThreadAbortException")
                {
                    LogException(ex);
                    throw new Exception("Error Report To PDF..." + ex.Message);
                }
            }

        }

        private void LogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog();

            appLog.ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionSource = "ViewDocx.aspx";
            appLog.ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionUrl = MySession.Current.SessionHostName.ToString().ToUpper();
            appLog.ExceptionTargetSite = ex.TargetSite.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionStackTrace = ex.StackTrace.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.AppLogEvent();
        }
    }
}