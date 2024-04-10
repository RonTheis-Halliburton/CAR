using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CAR.Forms
{
    public partial class Message : System.Web.UI.Page
    {
        string smtpHost = ConfigurationManager.AppSettings["smptHostMail"].ToString();
        string noReplyContact = ConfigurationManager.AppSettings["NoReplyContact"].ToString();
        public string HsnPortal = ConfigurationManager.AppSettings["HsnPortal"].ToString();

        string TestVendorName = ConfigurationManager.AppSettings["TestVendorName"].ToString();
        string TestVendorUserID = ConfigurationManager.AppSettings["TestVendorUserID"].ToString();
        string TestVendorCity = ConfigurationManager.AppSettings["TestVendorCity"].ToString();
        string TestVendorCountry = ConfigurationManager.AppSettings["TestVendorCountry"].ToString();

        public string AdminContact = ConfigurationManager.AppSettings["AdminContact"].ToString();
        public string ErrException = "If problem persists, please contact the administrator: " + ConfigurationManager.AppSettings["AdminContact"];
        public DateTime DefaultDate = DateTime.Parse("1/1/1900 12:00:00 AM");

        SanitizeString removeChar = new SanitizeString();

        MyUtil.HtmlUtility htmlUtil = MyUtil.HtmlUtility.Instance;

        private const int ItemsPerRequest = 100;

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
                    if (Request.QueryString["OID"] != null && Request.QueryString["CAR_NBR"] != null && Request.QueryString["Status_NM"] != null)
                    {
                        this.Header.Title = Server.HtmlEncode(htmlUtil.SanitizeHtml("Send Message - " + Request.QueryString["CAR_NBR"].ToString() + "     [Status:  " + Request.QueryString["Status_NM"].ToString() + "]"));

                        this.HiddenCarOid.Value = Request.QueryString["OID"].ToString();
                        this.HiddenCarNbr.Value = Request.QueryString["CAR_NBR"].ToString();
                        this.HiddenAreaOid.Value = "0";
                        this.Panel1.Visible = false;

                        LoadHesRecipientSent(Request.QueryString["OID"].ToString());

                        if (htmlUtil.IsNumeric(Request.QueryString["areaOid"].ToString()))
                        {
                            this.HiddenAreaOid.Value = Request.QueryString["areaOid"].ToString();

                            if (int.Parse(Request.QueryString["areaOid"].ToString()) == 2)  //Supplier
                            {
                                this.HiddenVendorNumber.Value = Request.QueryString["vendorNbr"].ToString();
                                this.HiddenVendorName.Value = Request.QueryString["vendorNm"].ToString();

                                this.Panel1.Visible = true;

                                this.RadListBox1.Height = Unit.Pixel(160);
                                this.RadListBox3.Items.Clear();

                                LoadVendorRecipient();

                                LoadVendorRecipientSent(Request.QueryString["OID"].ToString(), Request.QueryString["vendorNbr"].ToString());

                            }
                        }
                    }
                    else
                    {
                        this.Header.Title = "Error - Unable to locate CAR data.  " + ErrException;
                    }
                }
                catch (Exception ex)
                {
                    LogException(ex);

                    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
                }
            }
        }

        protected void RadButton5_Click(object sender, EventArgs e)
        {
            try
            {
                string msgReturn = string.Empty;
                msgReturn = SendMsgHes(this.HiddenCarOid.Value, this.HiddenCarNbr.Value);

                if (msgReturn == "Successfully")
                {
                    string[] mailReturn = { string.Empty, string.Empty };

                    CreateRecord mail;
                    mail = new CreateRecord();
                    mail.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.HiddenCarOid.Value)));
                    mail.Email_Subject = "Updated Corrective Action Request " + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.HiddenCarNbr.Value)));
                    mail.Email_Message = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox9.Text.Trim())));
                    mail.User_Id = MySession.Current.SessionUserID.ToString().ToUpper();
                    mail.User_Name = MySession.Current.SessionFullName.ToString();

                    mailReturn = mail.ExecCreateCar_Email();

                    if (mailReturn[0].ToString() == "Successfully")
                    {
                        foreach (RadListBoxItem node1 in this.RadListBox1.CheckedItems)
                        {
                            if (node1.Checked && node1.Value != "Check All")
                            {
                                string sReturn = string.Empty;

                                CreateRecord sendTo;
                                sendTo = new CreateRecord();
                                sendTo.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.HiddenCarOid.Value)));
                                sendTo.Car_Email_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(mailReturn[1].ToString())));
                                sendTo.User_Id = node1.Attributes["UserId"].ToString().ToUpper().Trim();
                                sendTo.User_Name = node1.Attributes["FullName"].ToString().Trim();
                                sendTo.User_Email = node1.Attributes["UserEmail"].ToString().Trim();
                                sendTo.Vendor_Nm = "0";

                                sReturn = sendTo.ExecCreateCar_Email_Sent_To();

                                if (sReturn.ToString() == "Successfully")
                                {
                                    //Do nothing
                                }
                                else
                                {
                                    int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                    Exception aex = new Exception(removeChar.SanitizeQuoteString(sReturn.ToString()));
                                    aex.Data.Add(key: "TargetSite", value: "sendTo.ExecCreateCar_Email_Sent_To()");
                                    aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                    CustomLogException(aex);
                                }
                            }
                        }

                        if (this.HiddenAreaOid.Value.ToString() == "2") //Supplier
                        {
                            msgReturn = SendMsgVendor(this.HiddenCarOid.Value, this.HiddenCarNbr.Value);

                            if (msgReturn == "Successfully")
                            {

                                foreach (RadListBoxItem node3 in this.RadListBox3.CheckedItems)
                                {
                                    if (node3.Checked && node3.Value != "Check All")
                                    {
                                        string sReturn = string.Empty;

                                        CreateRecord sendTo;
                                        sendTo = new CreateRecord();
                                        sendTo.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.HiddenCarOid.Value)));
                                        sendTo.Car_Email_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(mailReturn[1].ToString())));
                                        sendTo.User_Id = node3.Attributes["UserId"].ToString().ToUpper().Trim();
                                        sendTo.User_Name = node3.Attributes["FullName"].ToString().Trim();
                                        sendTo.User_Email = node3.Attributes["UserEmail"].ToString().Trim();
                                        sendTo.Vendor_Nm = node3.Attributes["VendorNumber"].ToString().Trim(); ;

                                        sReturn = sendTo.ExecCreateCar_Email_Sent_To();

                                        if (sReturn.ToString() == "Successfully")
                                        {
                                            //Do nothing
                                        }
                                        else
                                        {
                                            int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                            Exception aex = new Exception(removeChar.SanitizeQuoteString(sReturn.ToString()));
                                            aex.Data.Add(key: "TargetSite", value: "sendTo.ExecCreateCar_Email_Sent_To()");
                                            aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                            CustomLogException(aex);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);

                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            }
            finally
            {
                this.RadButton5.Enabled = false;
                this.RadButton2.Text = "Close";
            }
        }

        protected void RadButton13_Click(object sender, EventArgs e)
        {
            try
            {
                RadComboBox radComboBox8;
                radComboBox8 = (RadComboBox)this.RadListBox1.Header.FindControl(id: "RadComboBox8");

                if (radComboBox8 != null)
                {
                    if (!string.IsNullOrEmpty(radComboBox8.Text.Trim()) && !string.IsNullOrEmpty(radComboBox8.SelectedValue.ToString().Trim()))
                    {
                        RadListBoxItem itemFound = this.RadListBox1.FindItemByValue(radComboBox8.SelectedValue.Trim().ToUpper());

                        if (itemFound == null)
                        {
                            RadListBoxItem item;
                            item = new RadListBoxItem();

                            item.Text = this.HiddenRecipientName.Value.ToString();
                            item.Value = radComboBox8.SelectedValue.Trim().ToUpper();
                            item.ToolTip = this.HiddenRecipientName.Value.ToString();
                            item.Checked = true;
                            item.Checkable = true;
                            item.Selected = true;
                            item.Attributes["UserId"] = this.HiddenRecipientId.Value.ToUpper();
                            item.Attributes["UserEmail"] = this.HiddenRecipientEmail.Value;
                            item.Attributes["FullName"] = this.HiddenRecipientName.Value;

                            this.RadListBox1.Items.Add(item);
                            this.RadListBox1.SortItems();
                        }
                        else
                        {
                            itemFound.Selected = true;
                        }
                        radComboBox8.Text = string.Empty;
                        this.HiddenRecipientId.Value = string.Empty;
                        this.HiddenRecipientEmail.Value = string.Empty;
                        this.HiddenRecipientName.Value = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);

                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            }
        }


        protected void LoadVendorRecipient()
        {
            if (MySession.Current.SessionWebServer.ToString().ToUpper() == "TESTSITE")
            {
                RadListBoxItem item;
                item = new RadListBoxItem();

                item.Font.Bold = true;

                item.Text = TestVendorName;
                item.ToolTip = TestVendorName;
                item.Value = TestVendorUserID;

                item.Attributes.Add(key: "UserId", value: TestVendorUserID);
                item.Attributes.Add(key: "FullName", value: TestVendorName);
                item.Attributes.Add(key: "CITY", value: TestVendorCity);
                item.Attributes.Add(key: "CNTRY_NM", value: TestVendorCountry);
                item.Attributes.Add(key: "UserEmail", value: MySession.Current.SessionEmail.ToString());

                item.Selected = true;
                item.Checked = true;

                this.RadListBox3.Items.Add(item);
                this.RadListBox3.SortItems();

            }
            else
            {
                GetVendors ven;
                ven = new GetVendors();
                ven.SearchText = string.Empty;
                ven.VendorNumber = Server.HtmlEncode(htmlUtil.SanitizeHtml(this.HiddenVendorNumber.Value));

                using (DataTable dt = ven.GetVendorUserList())
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        RadListBoxItem item;
                        item = new RadListBoxItem();

                        item.Text = row["FullName"].ToString();
                        item.Value = row["UserID"].ToString().Trim().ToUpper();
                        item.ToolTip = row["FullName"].ToString();
                        item.Checkable = true;
                        item.Attributes["UserId"] = row["UserID"].ToString().ToUpper();
                        item.Attributes["UserEmail"] = row["EmailAddr"].ToString().ToLower();
                        item.Attributes["FullName"] = row["FullName"].ToString();
                        item.Attributes["VendorNumber"] = this.HiddenVendorNumber.Value.ToString().Trim();

                        this.RadListBox3.Items.Add(item);
                        this.RadListBox3.SortItems();
                    }
                }
            }
        }


        protected void LoadHesRecipientSent(string Car_Oid)
        {

            if (htmlUtil.IsNumeric(Car_Oid))
            {
                GetRecord hes;
                hes = new GetRecord();
                hes.CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Car_Oid)));
                hes.VendorNbr = "0";

                using (DataTable dt = hes.Exec_GetUser_Sent_Datatable())
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        RadListBoxItem item;
                        item = new RadListBoxItem();

                        item.Text = row["User_Name"].ToString();
                        item.Value = row["User_ID"].ToString().ToUpper();
                        item.ToolTip = row["User_Name"].ToString();
                        item.Checked = true;
                        item.Checkable = true;
                        item.Selected = true;
                        item.Attributes["UserId"] = row["User_ID"].ToString().ToUpper();
                        item.Attributes["UserEmail"] = row["User_Email"].ToString();
                        item.Attributes["FullName"] = row["User_Name"].ToString();

                        this.RadListBox1.Items.Add(item);
                        this.RadListBox1.SortItems();
                    }
                }
            }
        }

        protected void LoadEmailSent()
        {
            GetRecord mailSent;
            mailSent = new GetRecord();
            mailSent.CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.HiddenCarOid.Value)));

            using (DataTable dt = mailSent.Exec_GetEmail_Sent_Datatable())
            {
                this.RadGrid2.DataSource = dt;
                this.RadGrid2.DataBind();

                this.Session["EmailSent"] = dt;
            }
        }


        protected void LoadVendorRecipientSent(string Car_Oid, string Vendor_Nbr)
        {

            if (htmlUtil.IsNumeric(Car_Oid))
            {
                GetRecord vend;
                vend = new GetRecord();
                vend.CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Car_Oid)));
                vend.VendorNbr = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Vendor_Nbr))); ;

                using (DataTable dt = vend.Exec_GetUser_Sent_Datatable())
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        RadListBoxItem itemFound = this.RadListBox3.FindItemByValue(row["User_ID"].ToString().ToUpper());
                        if (itemFound != null)
                        {
                            itemFound.Checked = true;
                        }
                    }
                }
            }
        }

        protected void RadComboBox8_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                RadComboBox radComboBox8;
                radComboBox8 = (RadComboBox)this.RadListBox1.Header.FindControl(id: "RadComboBox8");

                if (radComboBox8 != null)
                {
                    radComboBox8.Items.Clear();

                    string srchText = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(e.Text.Trim())));

                    HesUsers hesUser;
                    hesUser = new HesUsers();
                    hesUser.SearchText = srchText;

                    using (DataTable dt = hesUser.GetHesUsers())
                    {
                        int itemOffset = e.NumberOfItems;
                        int endOffset = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count);
                        e.EndOfItems = endOffset == dt.Rows.Count;

                        for (int i = itemOffset; i < endOffset; i++)
                        {
                            RadComboBoxItem item;
                            item = new RadComboBoxItem();

                            item.Font.Bold = true;

                            item.Text = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                            item.ToolTip = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                            item.Value = dt.Rows[i]["UserID"].ToString().Trim().ToUpper();

                            item.Attributes.Add(key: "UserId", value: dt.Rows[i]["UserID"].ToString().Trim().ToUpper());
                            item.Attributes.Add(key: "FullName", value: dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim());
                            item.Attributes.Add(key: "CITY", value: dt.Rows[i]["CITY"].ToString().Trim());
                            item.Attributes.Add(key: "CNTRY_NM", value: dt.Rows[i]["CNTRY_NM"].ToString().Trim());
                            item.Attributes.Add(key: "Email", value: dt.Rows[i]["Email"].ToString().Trim());
                            radComboBox8.Items.Add(item);

                            item.DataBind();
                        }
                        e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
                    }
                }
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


        private string SendMsgHes(string carOid, string carNbr)
        {
            string uReturn = "Successfully";
            string HostName = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;

            try
            {
                using (MailMessage objEmail = new MailMessage())
                {
                    MailAddress frmAddr;
                    frmAddr = new MailAddress(noReplyContact.ToString().Trim(), "NoReply");

                    MailAddress frmOriginatorAddr;
                    frmOriginatorAddr = new MailAddress(MySession.Current.SessionEmail.ToString(), MySession.Current.SessionFullName.ToString());

                    objEmail.From = frmAddr;
                    objEmail.Bcc.Add(frmOriginatorAddr);

                    string carStatus = string.Empty;
                    string bodyMsg = string.Empty;
                    string bobyTestWeb = string.Empty;

                    if (MySession.Current.SessionWebServer.ToString().ToUpper() == "TESTSITE")
                    {
                        bobyTestWeb = "<font size=3 face=Arial color=red><b>***  This is a Test...Testing...Please disregard to this message  ***</b></font> <br><br>";
                    }

                    //// ** HES Recipients
                    foreach (RadListBoxItem item in this.RadListBox1.CheckedItems)
                    {
                        if (item.Checked)
                        {
                            if (item.Attributes["UserEmail"].ToString().Contains("@"))
                            {
                                objEmail.Bcc.Add(item.Attributes["UserEmail"].ToString());
                            }
                        }
                    }

                    GetRecord xCar;
                    xCar = new GetRecord();
                    xCar.CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carOid)));

                    using (DataTable dt = xCar.Exec_Get_Existing_Car_By_Oid_Datatable())
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            carStatus = row["CAR_STATUS_NM"].ToString().ToUpper() + " - ";
                        }
                    }

                    objEmail.IsBodyHtml = true;
                    objEmail.Subject = carStatus + "Updated Corrective Action Request " + htmlUtil.SanitizeHtml(Server.HtmlDecode(carNbr));

                    if (this.HiddenAreaOid.Value.ToString() == "2") //Supplier
                    {
                        objEmail.Subject += " (" + htmlUtil.SanitizeHtml(Server.HtmlDecode(this.HiddenVendorName.Value.ToString())) + ")";
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td align='center' style='background-color: #18B5F0'><b>***  Please do not reply to this email. This is an unmonitored address, and replies to this email cannot be responded to or read. ***</b></td></tr>");
                    sb.Append("<tr><td></td></tr>");
                    sb.Append("<tr><td>See attached Corrective Action Request <a href=" + HostName + "/default.aspx?form=car&indx=" + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carOid))) + "> " + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carNbr))) + "</a></td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br /><br />");
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td><b><u>Message from " + MySession.Current.SessionFullName.ToString() + ":</u></b></td></tr>");
                    sb.Append("<tr><td><pre>");
                    sb.Append(htmlUtil.SanitizeHtml(this.RadTextBox9.Text.Trim()));
                    sb.Append("</pre></td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br /><br />");
                    sb.Append("<br /><br />");

                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td><b><u>Message sent to the following recipient(s):</u></b></td></tr>");
                    foreach (RadListBoxItem item in this.RadListBox1.CheckedItems)
                    {
                        if (item.Checked)
                        {
                            if (item.Attributes["UserEmail"].ToString().Contains("@"))
                            {
                                sb.Append("<tr><td>");
                                sb.Append(htmlUtil.SanitizeHtml(item.Text.Trim()));
                                sb.Append("</td></tr>");
                            }
                        }
                    }

                    foreach (RadListBoxItem item in this.RadListBox3.CheckedItems)
                    {
                        if (item.Checked)
                        {
                            if (item.Attributes["UserEmail"].ToString().Contains("@"))
                            {
                                sb.Append("<tr><td>");
                                sb.Append(htmlUtil.SanitizeHtml(item.Text.Trim()));
                                sb.Append("</td></tr>");
                            }
                        }
                    }

                    sb.Append("</table>");
                    sb.Append("<br />");
                    sb.Append("<hr />");
                    sb.Append("This e - mail, including any attached files, may contain confidential and privileged information for the sole use of the intended recipient.  Any review, use, distribution, or disclosure by others is strictly prohibited. If you are not the intended recipient(or authorized to receive information for the intended recipient), please delete all copies of this message.");


                    objEmail.Priority = MailPriority.High;
                    objEmail.Body = bobyTestWeb + bodyMsg + sb.ToString(); ;

                    CreatePdf pdf = new CreatePdf();
                    pdf.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carOid)));

                    using (MemoryStream memoryStream = pdf.GetPdfFile())
                    {
                        byte[] bytes = memoryStream.ToArray();
                        memoryStream.Close();

                        Attachment attach = new Attachment(new MemoryStream(bytes), "CAR_" + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carNbr))) + ".pdf");

                        objEmail.Attachments.Add(attach);

                    }

                    using (SmtpClient smtpMailObj = new SmtpClient())
                    {
                        smtpMailObj.Host = smtpHost;
                        smtpMailObj.Send(objEmail);
                    }
                }
            }
            catch (Exception ex)
            {
                uReturn = ex.Message;
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Send mail failure: " + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            }
            finally
            {
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Email notification has been sent.');", addScriptTags: true);
            }

            return uReturn;
        }


        private string SendMsgVendor(string carOid, string carNbr)
        {
            string uReturn = "Successfully";
            try
            {
                using (MailMessage objEmail = new MailMessage())
                {
                    MailAddress frmAddr;
                    frmAddr = new MailAddress(noReplyContact.ToString().Trim(), "NoReply");

                    MailAddress frmOriginatorAddr;
                    frmOriginatorAddr = new MailAddress(MySession.Current.SessionEmail.ToString(), MySession.Current.SessionFullName.ToString());

                    objEmail.From = frmAddr;
                    objEmail.Bcc.Add(frmOriginatorAddr);

                    string carStatus = string.Empty;
                    string bodyMsg = string.Empty;
                    string bobyTestWeb = string.Empty;

                    if (MySession.Current.SessionWebServer.ToString().ToUpper() == "TESTSITE")
                    {
                        bobyTestWeb = "<font size=3 face=Arial color=red><b>***  This is a Test...Testing...Please disregard this message  ***</b></font> <br><br>";
                    }

                    //// ** Vendor Recipients
                    foreach (RadListBoxItem item in this.RadListBox3.CheckedItems)
                    {
                        if (item.Checked)
                        {
                            if (item.Attributes["UserEmail"].ToString().Contains("@"))
                            {
                                objEmail.Bcc.Add(item.Attributes["UserEmail"].ToString());
                            }
                        }
                    }

                    GetRecord xCar;
                    xCar = new GetRecord();
                    xCar.CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carOid)));

                    using (DataTable dt = xCar.Exec_Get_Existing_Car_By_Oid_Datatable())
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            carStatus = row["CAR_STATUS_NM"].ToString().ToUpper() + " - ";
                        }
                    }

                    objEmail.IsBodyHtml = true;
                    objEmail.Subject = carStatus + "Updated Corrective Action Request " + htmlUtil.SanitizeHtml(Server.HtmlDecode(carNbr)) + " (" + htmlUtil.SanitizeHtml(Server.HtmlDecode(this.HiddenVendorName.Value.ToString())) + ")";

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td align='center' style='background-color: #18B5F0'><b>***  Please do not reply to this email. This is an unmonitored address, and replies to this email cannot be responded to or read. ***</b></td></tr>");
                    sb.Append("<tr><td></td></tr>");
                    sb.Append("<tr><td>See attached Corrective Action Request - " + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carNbr))) + "</td></tr>");
                    sb.Append("<tr><td>For further information, please go to the <a href=" + HsnPortal + ">  Halliburton Supplier Network</a></td></tr>");


                    sb.Append("</table>");
                    sb.Append("<br /><br />");
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td><b><u>Message from " + MySession.Current.SessionFullName.ToString() + ":</u></b></td></tr>");
                    sb.Append("<tr><td><pre>");
                    sb.Append(htmlUtil.SanitizeHtml(this.RadTextBox9.Text.Trim()));
                    sb.Append("</pre></td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br /><br />");
                    sb.Append("<br /><br />");

                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td><b><u>Message sent to the following recipient(s):</u></b></td></tr>");
                    foreach (RadListBoxItem item in this.RadListBox1.CheckedItems)
                    {
                        if (item.Checked)
                        {
                            if (item.Attributes["UserEmail"].ToString().Contains("@"))
                            {
                                sb.Append("<tr><td>");
                                sb.Append(htmlUtil.SanitizeHtml(item.Text.Trim()));
                                sb.Append("</td></tr>");
                            }
                        }
                    }

                    foreach (RadListBoxItem item in this.RadListBox3.CheckedItems)
                    {
                        if (item.Checked)
                        {
                            if (item.Attributes["UserEmail"].ToString().Contains("@"))
                            {
                                sb.Append("<tr><td>");
                                sb.Append(htmlUtil.SanitizeHtml(item.Text.Trim()));
                                sb.Append("</td></tr>");
                            }
                        }
                    }

                    sb.Append("</table>");
                    sb.Append("<br />");
                    sb.Append("<hr />");
                    sb.Append("This e - mail, including any attached files, may contain confidential and privileged information for the sole use of the intended recipient.  Any review, use, distribution, or disclosure by others is strictly prohibited. If you are not the intended recipient(or authorized to receive information for the intended recipient), please delete all copies of this message.");


                    objEmail.Priority = MailPriority.High;
                    objEmail.Body = bobyTestWeb + bodyMsg + sb.ToString(); ;

                    CreatePdf pdf = new CreatePdf();
                    pdf.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carOid)));

                    using (MemoryStream memoryStream = pdf.GetPdfFile())
                    {
                        byte[] bytes = memoryStream.ToArray();
                        memoryStream.Close();

                        Attachment attach = new Attachment(new MemoryStream(bytes), "CAR_" + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carNbr))) + ".pdf");

                        objEmail.Attachments.Add(attach);

                    }

                    using (SmtpClient smtpMailObj = new SmtpClient())
                    {
                        smtpMailObj.Host = smtpHost;
                        smtpMailObj.Send(objEmail);
                    }

                }
            }
            catch (Exception ex)
            {
                uReturn = ex.Message;
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Send mail failure: " + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            }
            finally
            {
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Email notification has been sent.');", addScriptTags: true);
            }

            return uReturn;
        }

        protected void RadTabStrip1_TabClick(object sender, RadTabStripEventArgs e)
        {
            try
            {
                this.RadTabStrip1.Tabs[e.Tab.Index].Selected = true;

                if (e.Tab.Index == 1)
                {
                    LoadEmailSent();
                }

            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + ". " + ErrException + "');", addScriptTags: true);
            }
        }

        protected void RadGrid2_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            object obj = this.Session["EmailSent"];
            if ((!(obj == null)))
            {
                this.RadGrid2.DataSource = (DataTable)(obj);
            }
        }

        protected void RadGrid2_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }

        // Application event log
        private void LogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog();

            appLog.ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionSource = "Internal_Forms_Message.aspx";
            appLog.ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionUrl = string.Empty;
            appLog.ExceptionTargetSite = ex.TargetSite.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionStackTrace = ex.StackTrace.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.AppLogEvent();
        }

        private void CustomLogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog();

            appLog.ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionSource = "Internal_Forms_Message.aspx";
            appLog.ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionUrl = "CustomLogException";
            appLog.ExceptionTargetSite = ex.Data["TargetSite"].ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionStackTrace = ex.Data["StackTrace"].ToString().Replace(oldValue: "'", newValue: "`");

            appLog.AppLogEvent();
        }
    }
}