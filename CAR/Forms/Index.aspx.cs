using ComponentPro.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CAR.Forms
{
    public partial class Index : System.Web.UI.Page
    {

        public string AdminContact = ConfigurationManager.AppSettings["AdminContact"].ToString();
        public string ErrException = "If problem persists, please contact the administrator: " + ConfigurationManager.AppSettings["AdminContact"];
        public DateTime DefaultDate = DateTime.Parse("1/1/1900 12:00:00 AM");

        public string FtpSrvNm = ConfigurationManager.AppSettings["FtpSrvNm"].ToString();
        public string FtpSrvDir = ConfigurationManager.AppSettings["FtpSrvDir"].ToString();
        public string FtpSrvSharedNm = ConfigurationManager.AppSettings["FtpSrvSharedNm"].ToString();
        public string FtpU = ConfigurationManager.AppSettings["FtpU"].ToString();
        public string FtpP = ConfigurationManager.AppSettings["FtpP"].ToString();
        public string FtpSrvType = "/" + ConfigurationManager.AppSettings["FtpSrvType"].ToString();

        public string FtpSrvDirQdms = ConfigurationManager.AppSettings["FtpSrvDirQdms"].ToString();
        //public string FtpSrvSharedNmQdms = ConfigurationManager.AppSettings["FtpSrvSharedNmQdms"].ToString();
        public string FtpUQdms = ConfigurationManager.AppSettings["FtpUQdms"].ToString();
        public string FtpPQdms = ConfigurationManager.AppSettings["FtpPQdms"].ToString();


        //DataTable objDT;
        //DataRow objDR;

        SanitizeString removeChar = new SanitizeString();
        QdmsUtil myQdms = new QdmsUtil();

        MyUtil.HtmlUtility htmlUtil = MyUtil.HtmlUtility.Instance;

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
                    if (Request.QueryString["OID"] != null && Request.QueryString["CAR_NBR"] != null && Request.QueryString["Status_NM"] != null && Request.QueryString["Year_Issued"] != null)
                    {

                        string[] srvArray = myQdms.GetImageSysServ(MySession.Current.SessionUserID);


                        if (srvArray.Length > 0)
                        {
                            this.Header.Title = Server.HtmlEncode(htmlUtil.SanitizeHtml("Index to QDMS - CAR Nbr:  " + Request.QueryString["CAR_NBR"].ToString() + "     [Status:  " + Request.QueryString["Status_NM"].ToString() + "]"));

                            this.CAR_OID.Value = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Request.QueryString["OID"].ToString())));
                            this.CAR_NBR.Value = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Request.QueryString["CAR_NBR"].ToString())));
                            this.YEAR_ISSUE.Value = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Request.QueryString["Year_Issued"].ToString())));

                            LoadExistingCar(removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Request.QueryString["OID"].ToString()))));
                            LoadDocument(removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Request.QueryString["OID"].ToString()))));
                            LoadQdms(removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Request.QueryString["OID"].ToString()))));

                        }
                        else
                        {
                            Response.Redirect("../RegistrationQdms.aspx", false);
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

        //void MakeIndexFileCart()
        //{
        //    DataTable objDT;
        //    objDT = new DataTable(tableName: "IndexQDMS");
        //    objDT.Columns.Add(columnName: "OID", type: typeof(string));
        //    objDT.Columns.Add(columnName: "CAR_OID", type: typeof(string));
        //    objDT.Columns.Add(columnName: "CAR_NBR", type: typeof(string));
        //    objDT.Columns.Add(columnName: "FileSize", type: typeof(string));
        //    objDT.Columns.Add(columnName: "FileSize_Byte", type: typeof(string));
        //    objDT.Columns.Add(columnName: "FileName", type: typeof(string));
        //    objDT.Columns.Add(columnName: "FileNameAlias", type: typeof(string));
        //    objDT.Columns.Add(columnName: "UploadedBy", type: typeof(string));
        //    objDT.Columns.Add(columnName: "DateUploaded", type: typeof(string));
        //    objDT.Columns.Add(columnName: "FtpRemotePath", type: typeof(string));
        //    objDT.Columns.Add(columnName: "Year_Issued", type: typeof(string));
        //    this.Session["IndexQDMS"] = objDT;
        //}

        protected void LoadQdms(string CarOid)
        {
            GetRecord getFile;
            getFile = new GetRecord();
            getFile.CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.CAR_OID.Value)));

            using (DataTable dt = getFile.Exec_GetCar_Qdms_Datatable())
            {
                if (dt.Rows.Count > 0)
                {
                    this.RadButton2.Visible = true;
                    this.RadGrid2.DataSource = dt;
                    this.RadGrid2.DataBind();

                    this.Session["IndexQDMS"] = dt;
                }
                else
                {
                    this.RadButton2.Visible = false;
                    this.RadGrid2.DataSource = string.Empty;
                    this.RadGrid2.DataBind();

                    this.Session["IndexQDMS"] = null;
                }
            }
        }

        protected void LoadDocument(string CarOid)
        {
            GetRecord getFile;
            getFile = new GetRecord();
            getFile.CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(CarOid)));

            using (DataTable dt = getFile.Exec_GetCar_Document_Datatable())
            {
                this.RadGrid1.DataSource = dt;
                this.RadGrid1.DataBind();

                this.Session["IndexDoc"] = dt;
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            object obj = this.Session["IndexDoc"];
            if ((!(obj == null)))
            {
                this.RadGrid1.DataSource = (DataTable)(obj);
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                string oid = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OID"].ToString();
                string car_Oid = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CAR_OID"].ToString();
                string car_Nbr = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CAR_NBR"].ToString();
                string year_Issued = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["YEAR_ISSUED"].ToString();
                string file_Name_Alias = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FILENAMEALIAS"].ToString();

                GridDataItem dataItem;
                dataItem = e.Item as GridDataItem;

                if (e.CommandName == "Download")
                {
                    using (Sftp clientFtp = new Sftp())
                    {
                        if (dataItem != null)
                        {
                            string fileName = dataItem["FILENAME"].Text;
                            string fileSize = dataItem["FILESIZE"].Text;
                            string filePath = dataItem["FTPREMOTEPATH"].Text.Trim();

                            string ftpRemoteFile = string.Empty;

                            clientFtp.Connect(FtpSrvNm, 22);

                            if (clientFtp.IsConnected)
                            {
                                //// *** Authenticate FTP server
                                clientFtp.Authenticate(FtpU, FtpP);

                                if (clientFtp.IsAuthenticated)
                                {
                                    ftpRemoteFile = FtpSrvType + "/" + year_Issued + "/" + car_Nbr + "/" + file_Name_Alias;

                                    if (clientFtp.FileExists(ftpRemoteFile))
                                    {
                                        var uri = new Uri(filePath);
                                        HttpResponse rp = HttpContext.Current.Response;
                                        rp.Clear();
                                        rp.ClearContent();
                                        rp.ClearHeaders();
                                        rp.Buffer = false;

                                        using (WebClient wc = new WebClient())
                                        {
                                            rp.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
                                            rp.ContentType = "application/octet-stream";
                                            rp.AddHeader("Content-Length", fileSize);
                                            byte[] dataFile = wc.DownloadData(filePath);
                                            rp.BinaryWrite(dataFile);
                                        }

                                        rp.Flush();
                                        rp.SuppressContent = true;
                                        rp.End();
                                    }
                                    else
                                    {
                                        int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                        Exception aex = new Exception("Unable to locate file to download:  " + ftpRemoteFile);
                                        aex.Data.Add(key: "TargetSite", value: "OID=" + oid + ", CAR_OID=" + car_Oid + ", File Alias=" + file_Name_Alias);
                                        aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                        CustomLogException(aex);
                                        this.RadWindowManager1.RadAlert("Error...Unable to locate selected file.<br><br>" + ErrException, 350, 200, "Error Alert", string.Empty, string.Empty);
                                    }
                                }
                                else
                                {
                                    int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                    Exception aex = new Exception("FTP Failed Authentication");
                                    aex.Data.Add(key: "TargetSite", value: "FTP Authentication");
                                    aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                    CustomLogException(aex);
                                    this.RadWindowManager1.RadAlert("FTP Failed Authentication<br><br>" + ErrException, 350, 200, "FTP Alert", string.Empty, string.Empty);
                                }
                            }
                            else
                            {
                                int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                Exception aex = new Exception("FTP Failed to connect server " + FtpSrvNm);
                                aex.Data.Add(key: "TargetSite", value: "FTP Server Connection");
                                aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                CustomLogException(aex);
                                this.RadWindowManager1.RadAlert("FTP Failed to connect server " + FtpSrvNm + ".<br><br>" + ErrException, 350, 200, "FTP Alert", string.Empty, string.Empty);
                            }

                            clientFtp.Disconnect();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            }
        }

        protected void RadGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            object obj = this.Session["IndexQDMS"];
            if ((!(obj == null)))
            {
                this.RadGrid2.DataSource = (DataTable)(obj);
            }
        }

        protected void RadGrid2_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                string oid = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OID"].ToString();
                string car_Oid = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CAR_OID"].ToString();
                string car_Nbr = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CAR_NBR"].ToString();
                string year_Issued = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["YEAR_ISSUED"].ToString();
                string file_Name = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FILENAME"].ToString();

                GridDataItem dataItem;
                dataItem = e.Item as GridDataItem;

                if (e.CommandName == "Download")
                {
                    using (Sftp clientFtp = new Sftp())
                    {
                        if (dataItem != null)
                        {
                            string fileName = dataItem["FILENAME"].Text;
                            string fileSize = dataItem["FILESIZE"].Text;
                            string filePath = dataItem["FTPREMOTEPATH"].Text.Trim();

                            string ftpRemoteFile = string.Empty;

                            clientFtp.Connect(FtpSrvNm, 22);

                            if (clientFtp.IsConnected)
                            {
                                //// *** Authenticate FTP server
                                clientFtp.Authenticate(FtpU, FtpP);

                                if (clientFtp.IsAuthenticated)
                                {
                                    ftpRemoteFile = FtpSrvType + "/" + year_Issued + "/" + car_Nbr + "/QDMS/" + file_Name;

                                    if (clientFtp.FileExists(ftpRemoteFile))
                                    {
                                        var uri = new Uri(filePath);
                                        HttpResponse rp = HttpContext.Current.Response;
                                        rp.Clear();
                                        rp.ClearContent();
                                        rp.ClearHeaders();
                                        rp.Buffer = false;

                                        using (WebClient wc = new WebClient())
                                        {
                                            rp.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
                                            rp.ContentType = "application/octet-stream";
                                            rp.AddHeader("Content-Length", fileSize);
                                            byte[] dataFile = wc.DownloadData(filePath);
                                            rp.BinaryWrite(dataFile);
                                        }

                                        rp.Flush();
                                        rp.SuppressContent = true;
                                        rp.End();
                                    }
                                    else
                                    {
                                        int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                        Exception aex = new Exception("Unable to locate file to download:  " + ftpRemoteFile);
                                        aex.Data.Add(key: "TargetSite", value: "OID=" + oid + ", CAR_OID=" + car_Oid + ", File Alias=" + file_Name);
                                        aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                        CustomLogException(aex);
                                        this.RadWindowManager1.RadAlert("Error...Unable to locate selected file.<br/><br/>Possible the package has been accepted and indexed to QDMS.<br/><br/>" + ErrException, 400, 250, "Error Alert", string.Empty, string.Empty);
                                    }
                                }
                                else
                                {
                                    int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                    Exception aex = new Exception("FTP Failed Authentication");
                                    aex.Data.Add(key: "TargetSite", value: "FTP Authentication");
                                    aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                    CustomLogException(aex);
                                    this.RadWindowManager1.RadAlert("FTP Failed Authentication<br/><br/>" + ErrException, 400, 250, "FTP Alert", string.Empty, string.Empty);
                                }
                            }
                            else
                            {
                                int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                Exception aex = new Exception("FTP Failed to connect server " + FtpSrvNm);
                                aex.Data.Add(key: "TargetSite", value: "FTP Server Connection");
                                aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                CustomLogException(aex);
                                this.RadWindowManager1.RadAlert("FTP Failed to connect server " + FtpSrvNm + ".<br/><br/>" + ErrException, 400, 250, "FTP Alert", string.Empty, string.Empty);
                            }

                            clientFtp.Disconnect();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                this.RadWindowManager1.RadAlert("Document Indexing Error<br/><br/>" + removeChar.SanitizeQuoteString(ex.Message) + "<br/><br/>" + ErrException, 400, 250, "Alert", string.Empty, string.Empty);
            }
        }

        protected void LoadExistingCar(string CarOid)
        {
            //try
            //{
            //    //RadTreeNode nodeStart = this.RadTreeView1.Nodes[0];

            GetRecord xCar;
            xCar = new GetRecord();
            xCar.CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(CarOid)));

            using (DataTable dt = xCar.Exec_Get_Existing_Car_By_Oid_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    LoadPreviewLiteralValue(1, row["ORIGINATOR_USR_ID"].ToString().Trim().ToUpper(), 0, 0);
                    LoadPreviewLiteralValue(2, row["ORIGINATOR_USR_NM"].ToString().Trim().ToUpper(), 0, 0);
                    LoadPreviewLiteralValue(3, row["DATE_ISSUED"].ToString(), 0, 0); // Date Issued
                    LoadPreviewLiteralValue(4, row["DUE_DT"].ToString(), 0, 0); // Due Date
                    LoadPreviewLiteralValue(5, row["FINDING_TYPE_NM"].ToString(), 0, 0);  // Finding Type
                    LoadPreviewLiteralValue(6, row["AREA_DESCRIPT_NM"].ToString(), 0, 0); //Area 
                    LoadPreviewLiteralValue(7, row["PSL_NM"].ToString(), 0, 0);  //PSL
                    LoadPreviewLiteralValue(8, row["FACILITY_NAME"].ToString(), 0, 0);  // Plant
                    LoadPreviewLiteralValue(9, row["CATEGORY_NM"].ToString(), 0, 0);  // Category

                    string[] strApiiso = row["API_ISO_ELEM"].ToString().Split('|');
                    foreach (string api in strApiiso)
                    {
                        RadTreeNode node = this.RadTreeView1.Nodes[0];
                        if (node != null)
                        {
                            RadTreeNode nodeData = node.Nodes[0];
                            if (nodeData != null)
                            {
                                RadComboBox radComboBox = (RadComboBox)nodeData.FindControl("RadComboBox1");
                                if (radComboBox != null)
                                {

                                    RadComboBoxItem item;
                                    item = new RadComboBoxItem();

                                    item.Value = api;
                                    item.Text = api;

                                    radComboBox.Items.Add(item);
                                }
                            }
                        }
                    }

                    LoadPreviewLiteralValue(19, row["RESP_PERSON_USR_NM"].ToString(), 0, 0);
                    LoadPreviewLiteralValue(24, row["RESP_PERSON_USR_ID"].ToString(), 0, 0);

                    LoadPreviewLiteralValue(13, row["AUDIT_NBR"].ToString(), 0, 0); // Audit Number
                    LoadPreviewLiteralValue(14, row["QNOTE_NBR"].ToString(), 0, 0); // Q Note Number
                    LoadPreviewLiteralValue(15, row["CPI_NBR"].ToString(), 0, 0); // CPI Number
                    LoadPreviewLiteralValue(16, row["MATERIAL_NBR"].ToString(), 0, 0); // Material Number
                    LoadPreviewLiteralValue(17, row["PURCHASE_ORDER_NBR"].ToString(), 0, 0); // Purchase Order Number
                    LoadPreviewLiteralValue(18, row["PRODUCTION_ORDER_NBR"].ToString(), 0, 0); // Production Order Number
                    LoadPreviewLiteralValue(20, row["API_AUDIT_NBR"].ToString(), 0, 0); // API Audit Number

                    LoadPreviewTextBoxValue(19, row["FINDING_DESC"].ToString(), 0, 0); // Description of Finding
                    LoadPreviewTextBoxValue(20, row["DESC_OF_IMPROVEMENT"].ToString(), 0, 0); // Description of Improvement

                    LoadPreviewLiteralValue(10, row["VNDR_NBR"].ToString(), 1, 0);  //Vendor Num
                    LoadPreviewLiteralValue(11, row["VENDOR_NM"].ToString(), 1, 0); //Vendor Name


                    LoadPreviewLiteralValue(12, row["ISSUED_TO_USR_NM"].ToString(), 2, 0);
                    LoadPreviewLiteralValue(21, row["ISSUED_TO_USR_ID"].ToString(), 2, 0);
                    LoadPreviewLiteralValue(22, row["LOC_COUNTRY_NM"].ToString(), 2, 0);
                    LoadPreviewLiteralValue(23, row["LOC_SUPPLIER"].ToString(), 2, 0);

                    LoadPreviewTextBoxValue(20, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["NON_CONFORM_RSN"].ToString()))), 3, 0);
                    LoadPreviewTextBoxValue(20, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["ROOT_CAUSE"].ToString()))), 4, 0);
                    LoadPreviewTextBoxValue(20, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["SIMILAR_INSTANCE"].ToString()))), 5, 0);
                    LoadPreviewTextBoxValue(20, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["CORR_ACTION_TAKEN"].ToString()))), 6, 0);
                    LoadPreviewTextBoxValue(20, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["PRECLUDE_ACTION"].ToString()))), 7, 0);

                    LoadPreviewLiteralValue(24, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["CORR_ACTION_TAKEN_DT"].ToString()))), 6, 0);
                    LoadPreviewLiteralValue(24, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["PRECLUDE_ACTION_DT"].ToString()))), 7, 0);

                    LoadPreviewLiteralValue(25, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["ACTION_TAKEN_BY_USR_NM"].ToString()))), 8, 0);
                    LoadPreviewLiteralValue(26, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["ACTION_TAKEN_BY_USR_ID"].ToString()))), 8, 0);



                    LoadPreviewLiteralValue(28, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["DUE_DT_EXT"].ToString()))), 9, 0);
                    LoadPreviewLiteralValue(29, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["REISSUED_TO_USR_NM"].ToString()))), 9, 0);
                    LoadPreviewLiteralValue(30, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["REISSUED_TO_USR_ID"].ToString()))), 9, 0);
                    LoadPreviewLiteralValue(33, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["FOLLOW_UP_REQD_TEXT"].ToString()))), 9, 0);
                    LoadPreviewLiteralValue(34, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["FOLLOW_UP_DT"].ToString()))), 9, 0);
                    LoadPreviewLiteralValue(32, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["RECEIVED_DT"].ToString()))), 9, 0);
                    LoadPreviewLiteralValue(35, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["VERIFY_DT"].ToString()))), 9, 0);

                    LoadPreviewLiteralValue(36, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["VERIFIED_BY_USR_NM"].ToString()))), 9, 0);
                    LoadPreviewLiteralValue(37, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["VERIFIED_BY_USR_ID"].ToString()))), 9, 0);
                    LoadPreviewLiteralValue(38, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["RESPONSE_ACCEPT_BY_USR_NM"].ToString()))), 9, 0);
                    LoadPreviewLiteralValue(39, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["RESPONSE_ACCEPT_BY_USR_ID"].ToString()))), 9, 0);
                    LoadPreviewLiteralValue(39, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["RESPONSE_ACCEPT_BY_USR_ID"].ToString()))), 9, 0);
                    LoadPreviewLiteralValue(41, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["CAR_STATUS_NM"].ToString()))), 9, 0);
                    LoadPreviewLiteralValue(40, DateTime.Parse(row["CLOSE_DT"].ToString()).ToShortDateString(), 9, 0);

                    LoadPreviewTextBoxValue(21, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["REMARKS"].ToString()))), 9, 0);

                    ExpandTreeviewNode(0);

                }
            }

        }

        protected void LoadPreviewTextBoxValue(int ControlNum, string ControlVal, int parentLevel, int childLevel)
        {
            RadTreeNode node = this.RadTreeView1.Nodes[parentLevel];
            if (node != null)
            {
                RadTreeNode nodeData = node.Nodes[childLevel];

                if (nodeData != null)
                {
                    RadTextBox radTextBox = (RadTextBox)nodeData.FindControl("RadTextBox" + ControlNum.ToString());

                    if (radTextBox != null)
                    {
                        radTextBox.Text = ControlVal;
                    }
                }
            }
        }

        private void ExpandTreeviewNode(int nTreeNode)
        {
            foreach (RadTreeNode node in this.RadTreeView1.GetAllNodes())
            {
                if (node.GetAllNodes() != null)
                {
                    node.BackColor = Color.White;
                    node.ForeColor = Color.Black;
                    node.BorderStyle = BorderStyle.None;

                    node.Font.Bold = false;
                    node.Expanded = false;
                    //node.Selected = false;

                    if (node.Index == nTreeNode)
                    {
                        //node.ForeColor = Color.Blue;
                        node.Font.Bold = true;
                        node.Expanded = true;
                        //node.Selected = true;
                    }
                    //else
                    //{
                    //    //node.ForeColor = Color.Black;
                    //    node.Font.Bold = false;
                    //    node.Expanded = false;
                    //    node.Selected = false;
                    //}
                }
            }
        }
        protected void LoadPreviewLiteralValue(int ControlNum, string ControlVal, int parentLevel, int childLevel)
        {
            RadTreeNode node = this.RadTreeView1.Nodes[parentLevel];
            if (node != null)
            {
                RadTreeNode nodeData = node.Nodes[childLevel];

                if (nodeData != null)
                {
                    Literal literal = (Literal)nodeData.FindControl("Literal" + ControlNum.ToString());
                    if (literal != null)
                    {
                        literal.Text = htmlUtil.SanitizeHtml(ControlVal);
                    }
                }
            }
        }

        protected void RadButton1_Click(object sender, EventArgs e)
        {

            PackageCar(this.CAR_OID.Value);

        }

        protected void RadButton2_Click(object sender, EventArgs e)
        {
            //Accept and Load to QDMS
            try
            {
                object objWebServer = MySession.Current.SessionWebServer;
                object objHostName = MySession.Current.SessionHostName;
                object objUname = MySession.Current.SessionFullName;

                bool ftpConnected = false;
                bool ftpQdmsConnected = false;

                int indx = 0;

                string ftpRemoteQdmsPath = string.Empty;
                string ftpRemoteFolder = string.Empty;
                string ftpRemoteFile = string.Empty;
                string ftpRemoteXml = string.Empty;
                string ftpRemoteCar = string.Empty;

                foreach (GridDataItem dataItem in this.RadGrid2.MasterTableView.Items)
                {
                    if (!htmlUtil.IsDate(dataItem["QdmsDate"].Text.Trim()))
                    {
                        indx++;

                        ///+++ Connecting to CAR FTP server
                        using (Sftp clientFtp = new Sftp())
                        {
                            try
                            {
                                clientFtp.Connect(FtpSrvNm, 22);

                                if (clientFtp.IsConnected)
                                {
                                    ftpConnected = true;

                                    //// *** Authenticate FTP server
                                    clientFtp.Authenticate(FtpU, FtpP);

                                    if (clientFtp.IsAuthenticated)
                                    {
                                        ftpRemoteCar = FtpSrvType + "/" + this.YEAR_ISSUE.Value.ToString() + "/" + dataItem["CAR_NBR"].Text.ToString();
                                        ftpRemoteFile = ftpRemoteCar + "/QDMS/" + dataItem["FILENAME"].Text.ToString();
                                        ftpRemoteXml = ftpRemoteCar + "/QDMS/" + dataItem["FILENAME"].Text.ToString().Replace(".pdf", ".XML");

                                        if (clientFtp.FileExists(ftpRemoteFile))
                                        {
                                            ///+++ Connecting to QDMS FTP server
                                            using (Ftp clientFtpQdms = new Ftp())
                                            {
                                                try
                                                {
                                                    clientFtpQdms.Connect(FtpSrvNm, 2122);

                                                    if (clientFtpQdms.IsConnected)
                                                    {
                                                        ftpQdmsConnected = true;

                                                        //Authenticate QDMS FTP server
                                                        clientFtpQdms.Authenticate(FtpUQdms, FtpPQdms);

                                                        if (clientFtpQdms.IsAuthenticated)
                                                        {
                                                            if (objWebServer.ToString().Trim().ToUpper() == "TESTSITE" || objWebServer.ToString().Trim() == "localhost")
                                                            {
                                                                ftpRemoteQdmsPath = "/Test_Folder_Delete_Me";

                                                                if (!clientFtpQdms.DirectoryExists(ftpRemoteQdmsPath))
                                                                {
                                                                    //clientFtpQdms.BeginCreateDirectory(ftpRemoteQdmsPath);
                                                                    clientFtpQdms.CreateDirectory(ftpRemoteQdmsPath);
                                                                }
                                                            }

                                                            //////++++ Copy files from CAR to QDMS FTP server
                                                            ////clientFtpQdms.CopyFrom(clientFtp, ftpRemoteFile, ftpRemoteQdmsPath + "/" + dataItem["FILENAME"].Text.ToString());

                                                            using (MemoryStream ms = new MemoryStream())
                                                            {
                                                                clientFtp.DownloadFile(ftpRemoteFile, ms);
                                                                ms.Position = 0;
                                                                using (Stream fileStream = ms)
                                                                {
                                                                    clientFtpQdms.CopyFrom(fileStream, ftpRemoteQdmsPath + "/" + dataItem["FILENAME"].Text.ToString());
                                                                }
                                                            }

                                                            Thread.Sleep(millisecondsTimeout: 2000); //Give FTP upload extra time to finish

                                                            if (clientFtpQdms.FileExists(ftpRemoteQdmsPath + "/" + dataItem["FILENAME"].Text.ToString()))
                                                            {
                                                                //////clientFtpQdms.CopyFrom(clientFtp, ftpRemoteXml, ftpRemoteQdmsPath + "/" + dataItem["FILENAME"].Text.ToString().Replace(".pdf", ".XML"));

                                                                using (MemoryStream ms = new MemoryStream())
                                                                {
                                                                    clientFtp.DownloadFile(ftpRemoteXml, ms);
                                                                    ms.Position = 0;
                                                                    using (Stream fileStream = ms)
                                                                    {

                                                                        clientFtpQdms.CopyFrom(fileStream, ftpRemoteQdmsPath + "/" + dataItem["FILENAME"].Text.ToString().Replace(".pdf", ".XML"));
                                                                    }
                                                                }


                                                                Thread.Sleep(millisecondsTimeout: 2000); //Give FTP upload extra time to finish

                                                                if (clientFtpQdms.FileExists(ftpRemoteQdmsPath + "/" + dataItem["FILENAME"].Text.ToString().Replace(".pdf", ".XML")))
                                                                {
                                                                    string uReturn = string.Empty;

                                                                    UpdateRecord upQdms;
                                                                    upQdms = new UpdateRecord();
                                                                    upQdms.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(dataItem["CAR_OID"].Text.ToString())));
                                                                    upQdms.Updt_by = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(objUname.ToString())));

                                                                    uReturn = upQdms.ExecUpdateQdms();
                                                                    if (uReturn.ToString() == "Successfully")
                                                                    {

                                                                        LoadDocument(removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(dataItem["CAR_OID"].Text.ToString()))));
                                                                        LoadQdms(removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(dataItem["CAR_OID"].Text.ToString()))));

                                                                        this.RadButton1.Enabled = false;
                                                                        this.RadButton2.Enabled = false;

                                                                        // Delete an entire directory.
                                                                        if (clientFtp.DirectoryExists(ftpRemoteCar))
                                                                        {
                                                                            clientFtp.DeleteDirectory(ftpRemoteCar, true);
                                                                        }

                                                                        ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Selected package has been sent to QDMS.');", addScriptTags: true);
                                                                    }
                                                                    else
                                                                    {
                                                                        ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Error...Unable to update QDMS - " + removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(uReturn.ToString())) + ".  " + ErrException + "');", addScriptTags: true);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Error...Unable to verify document/file exist.  Possible QDMS FTP file transmit failure.  Please try again. " + ErrException + "');", addScriptTags: true);
                                                                }

                                                            }
                                                            else
                                                            {
                                                                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Error...Unable to verify document/file exist.  Possible QDMS FTP file transmit failure.  Please try again. " + ErrException + "');", addScriptTags: true);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Failed to authenticate QDMS FTP server " + removeChar.SanitizeQuoteString(FtpSrvNm) + ". " + ErrException + "');", addScriptTags: true);
                                                            if (ftpQdmsConnected)
                                                                clientFtpQdms.Disconnect();
                                                            return;
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogException(ex);
                                                    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(ex.Message)) + ". " + ErrException + "');", addScriptTags: true);

                                                    if (ftpQdmsConnected)
                                                        clientFtpQdms.Disconnect();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Error...Unable to verify document/file exist.  Please try again. " + ErrException + "');", addScriptTags: true);
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Failed to authenticate FTP server " + removeChar.SanitizeQuoteString(FtpSrvNm) + ". " + ErrException + "');", addScriptTags: true);
                                        if (ftpConnected)
                                            clientFtp.Disconnect();
                                        return;
                                    }
                                }
                                else
                                {
                                    int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                    Exception aex = new Exception("FTP Failed to connect server " + FtpSrvNm);
                                    aex.Data.Add(key: "TargetSite", value: "FTP Server Connection");
                                    aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                    CustomLogException(aex);
                                    this.RadWindowManager1.RadAlert("FTP Failed to connect server " + FtpSrvNm + ".<br><br>" + ErrException, 350, 200, "FTP Alert", string.Empty, string.Empty);
                                }

                                clientFtp.Disconnect();
                            }
                            catch (Exception ex)
                            {
                                LogException(ex);
                                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(ex.Message)) + ". " + ErrException + "');", addScriptTags: true);

                                if (ftpConnected)
                                    clientFtp.Disconnect();
                            }
                        }
                    }
                }

                //+++ no package to index
                if (indx == 0)
                {
                    this.RadWindowManager1.RadAlert("There is not a valid package to index.<br><br>Please verify and try again.<br><br>" + ErrException, 350, 200, "FTP Alert", string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(ex.Message)) + ". " + ErrException + "');", addScriptTags: true);
            }
        }


        private void PackageCar(string CarOid)
        {
            try
            {
                string fileName;
                object objUid = MySession.Current.SessionUserID;
                object objWebServer = MySession.Current.SessionWebServer;
                object objHostName = MySession.Current.SessionHostName;

                string filePrefix = string.Empty;
                filePrefix = DateTime.Now.ToString(format: "yyyyMMddHHmmss");

                if (objHostName != null)
                {
                    filePrefix += "_" + objHostName;
                }
                else
                {
                    filePrefix += "_NoHostName";
                }

                using (Sftp clientFtp = new Sftp())
                {
                    string ftpRemoteYearPath = string.Empty;
                    //string ftpRemotePath = string.Empty;
                    string ftpRemoteCarPath = string.Empty;
                    string ftpRemoteQdmsPath = string.Empty;
                    string filePath = string.Empty;
                    int pageCount = 0;

                    clientFtp.Connect(FtpSrvNm, 22);

                    if (clientFtp.IsConnected)
                    {
                        //// *** Authenticate FTP server
                        clientFtp.Authenticate(FtpU, FtpP);

                        if (clientFtp.IsAuthenticated)
                        {

                            //// ** If not exist, create Server Type Folder
                            if (!clientFtp.DirectoryExists(FtpSrvType))
                            {
                                clientFtp.CreateDirectory(FtpSrvType);
                            }


                            ftpRemoteYearPath = FtpSrvType + "/" + this.YEAR_ISSUE.Value;

                            //// ** If not exist, create Year Folder
                            if (!clientFtp.DirectoryExists(ftpRemoteYearPath))
                            {
                                clientFtp.CreateDirectory(ftpRemoteYearPath);
                            }

                            ftpRemoteCarPath = ftpRemoteYearPath + "/" + this.CAR_NBR.Value.ToString();

                            //// ** If not exist, create CAR Folder
                            if (!clientFtp.DirectoryExists(ftpRemoteCarPath))
                            {
                                clientFtp.CreateDirectory(ftpRemoteCarPath);
                            }

                            ftpRemoteQdmsPath = ftpRemoteCarPath + "/QDMS";


                            ////// ******************** Remove this when done
                            /////
                            //if (clientFtp.DirectoryExists(ftpRemoteQdmsPath))
                            //{
                            //    clientFtp.DeleteDirectory(ftpRemoteQdmsPath, true);
                            //}
                            ////// ******************** 


                            //// ** If not exist, create QDMS Folder
                            if (!clientFtp.DirectoryExists(ftpRemoteQdmsPath))
                            {
                                clientFtp.CreateDirectory(ftpRemoteQdmsPath);
                            }

                            fileName = filePrefix + "_CAR_" + this.CAR_NBR.Value.ToString() + "_" + objUid.ToString().Trim().ToLower() + ".pdf";

                            MemoryStream outputMemoryStream;
                            outputMemoryStream = new MemoryStream();

                            iTextSharp.text.Document.Compress = true;

                            iTextSharp.text.Document document;
                            document = new iTextSharp.text.Document();

                            PdfCopy copy;
                            copy = new PdfCopy(document, outputMemoryStream);
                            copy.CloseStream = false;

                            PdfImportedPage page;

                            PdfEvents pdfPageEventHelper = new PdfEvents();
                            copy.PageEvent = pdfPageEventHelper;

                            document.AddTitle("Halliburton Corrective Action");
                            document.AddSubject("CAR " + this.CAR_NBR.Value.ToString());
                            document.AddCreator("Corrective Action");
                            document.AddAuthor(removeChar.SanitizeQuoteString(MySession.Current.SessionFullName));

                            copy.CreateXmpMetadata();

                            document.Open();

                            PdfReader.unethicalreading = true;  // override this permission checking mechanism


                            using (MemoryStream carStream = GetCarStream(CarOid))
                            {
                                //Get CAR information
                                using (Stream mainFileStream = carStream)
                                {
                                    //// *** Read and validate pdf page
                                    byte[] fileBinaryData = new byte[1024 * 100];
                                    BinaryReader br = new BinaryReader(mainFileStream);
                                    fileBinaryData = br.ReadBytes((Int32)mainFileStream.Length);

                                    if (fileBinaryData != null)
                                    {
                                        PdfReader reader;
                                        reader = new PdfReader(fileBinaryData);

                                        pageCount = reader.NumberOfPages;

                                        int i = 0;
                                        while (i < reader.NumberOfPages)
                                        {
                                            i++;

                                            page = copy.GetImportedPage(reader, i);
                                            copy.AddPage(page);
                                        }

                                        //Get documents
                                        foreach (GridDataItem dataItem in this.RadGrid1.MasterTableView.Items)
                                        {
                                            CheckBox ckBox;
                                            ckBox = dataItem.FindControl(id: "CheckBox1") as CheckBox;

                                            if (ckBox.Checked)
                                            {
                                                //// *** Insert blank page
                                                Byte[] bytes = new byte[1024 * 100];
                                                CreatePdf blankPdf = new CreatePdf();

                                                bytes = blankPdf.GetBlankPage();

                                                if (bytes != null)
                                                {
                                                    PdfReader readerData;
                                                    readerData = new PdfReader(bytes);

                                                    page = copy.GetImportedPage(readerData, 1);
                                                    copy.AddPage(page);
                                                }

                                                //// *** Get file from server
                                                filePath = ftpRemoteCarPath + "/" + dataItem["FileNameAlias"].Text.ToString().Trim();
                                                if (clientFtp.FileExists(filePath))
                                                {
                                                    using (MemoryStream ms = new MemoryStream())
                                                    {
                                                        clientFtp.DownloadFile(filePath, ms);
                                                        ms.Position = 0;

                                                        using (Stream fileStream = ms)
                                                        {
                                                            //// *** Read and validate pdf page
                                                            byte[] fileByteData = new byte[1024 * 100];
                                                            BinaryReader brData = new BinaryReader(fileStream);
                                                            fileByteData = brData.ReadBytes((Int32)fileStream.Length);

                                                            if (fileByteData != null)
                                                            {
                                                                PdfReader readerData;
                                                                readerData = new PdfReader(fileByteData);

                                                                pageCount = pageCount + readerData.NumberOfPages;

                                                                int x = 0;
                                                                while (x < readerData.NumberOfPages)
                                                                {
                                                                    x++;

                                                                    page = copy.GetImportedPage(readerData, x);
                                                                    copy.AddPage(page);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            document.Close();

                            outputMemoryStream.Flush(); //Always caches me out  
                            outputMemoryStream.Position = 0;

                            clientFtp.UploadFile(outputMemoryStream, ftpRemoteQdmsPath + "/" + fileName);

                            if (clientFtp.FileExists(ftpRemoteQdmsPath + "/" + fileName))
                            {
                                SftpFileInfo file_Info = (SftpFileInfo)clientFtp.GetItemInfo(ftpRemoteQdmsPath + "/" + fileName);

                                DateTime modifiedDateTime = clientFtp.GetLastWriteTime(ftpRemoteQdmsPath + "/" + fileName);

                                string xmlFilePath = ftpRemoteQdmsPath + "/" + fileName.ToString().Replace(".pdf", "") + ".XML";

                                ftpRemoteQdmsPath = "\\" + "\\" + FtpSrvNm + "\\" + FtpSrvSharedNm + "\\" + FtpSrvType.Replace("/", "") + "\\" + this.YEAR_ISSUE.Value.ToString() + "\\" + this.CAR_NBR.Value.ToString() + "\\QDMS\\" + fileName;


                                string[] docReturn = { string.Empty, string.Empty };

                                CreateRecord doc;
                                doc = new CreateRecord();
                                doc.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.CAR_OID.Value)));
                                doc.File_Size = file_Info.Length.ToString();
                                doc.File_Name = file_Info.Name.ToString();
                                doc.File_NameAlias = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.CAR_NBR.Value)));
                                doc.Uploaded_By = MySession.Current.SessionFullName;
                                doc.Ftp_RemotePath = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(ftpRemoteQdmsPath)));

                                docReturn = doc.ExecCreateCar_QDMS();

                                if (docReturn[0].ToString() == "Successfully")
                                {
                                    LoadQdms(removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.CAR_OID.Value))));


                                    ////*** Create XML
                                    string xmlFile = string.Empty;

                                    StringBuilder xML;
                                    xML = new StringBuilder();
                                    xML.AppendLine(value: "<?xml version=" + "\"" + "1.0" + "\"" + " encoding=" + "\"" + "utf-8" + "\"" + "?>");
                                    xML.AppendLine(value: "<ImageIndexData>");

                                    xML.AppendLine(value: "    <Action>ADD</Action>");
                                    xmlFile = xmlFilePath;
                                    xML.AppendLine(value: "    <DocOID>0</DocOID>");


                                    GetRecord xCar;
                                    xCar = new GetRecord();
                                    xCar.CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(CarOid)));

                                    using (DataTable dt = xCar.Exec_Get_Existing_Car_By_Oid_Datatable())
                                    {
                                        foreach (DataRow row in dt.Rows)
                                        {
                                            xML.AppendLine(value: "    <PlntNo>" + row["PLNT_CD"].ToString().Trim() + "</PlntNo>");
                                            xML.AppendLine(value: "    <Field1>" + row["CAR_NBR"].ToString().Trim() + "</Field1>");
                                            xML.AppendLine(value: "    <Field2/>");
                                            xML.AppendLine(value: "    <Field3/>");
                                            xML.AppendLine(value: "    <Field4/>");
                                            xML.AppendLine(value: "    <Field5/>");
                                            xML.AppendLine(value: "    <Field6/>");
                                            xML.AppendLine(value: "    <Field7/>");
                                            xML.AppendLine(value: "    <Field8/>");
                                            xML.AppendLine(value: "    <Field9/>");
                                            xML.AppendLine(value: "    <Field10/>");
                                            xML.AppendLine(value: "    <DocCmplt>True</DocCmplt>");
                                            xML.AppendLine(value: "    <ScanDt>" + modifiedDateTime.ToString(format: "yyyy-MM-dd HH:mm:ss") + "</ScanDt>");
                                            xML.AppendLine(value: "    <DocTypOID>1097</DocTypOID>");
                                            xML.AppendLine(value: "    <PageCount>" + pageCount.ToString() + "</PageCount>");
                                            xML.AppendLine(value: "    <SourceFileNM>" + file_Info.Name.ToString() + "</SourceFileNM>");
                                            xML.AppendLine(value: "    <Comments/>");
                                            xML.AppendLine(value: "    <UserID>" + objUid.ToString() + "</UserID>");  //User ID that indexed the document.  
                                            xML.AppendLine(value: "    <DOC_SOURCE>CAR SYSTEM</DOC_SOURCE>");

                                            xML.AppendLine(value: "</ImageIndexData>");

                                            byte[] byteXml;
                                            byteXml = Encoding.ASCII.GetBytes(xML.ToString());
                                            using (MemoryStream memStream = new MemoryStream(byteXml))
                                            {
                                                clientFtp.UploadFile(memStream, xmlFilePath);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                        Exception aex = new Exception("FTP Failed to connect server " + FtpSrvNm);
                        aex.Data.Add(key: "TargetSite", value: "FTP Server Connection");
                        aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                        CustomLogException(aex);
                        this.RadWindowManager1.RadAlert("FTP Failed to connect server " + FtpSrvNm + ".<br><br>" + ErrException, 350, 200, "FTP Alert", string.Empty, string.Empty);
                    }

                    clientFtp.Disconnect();
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name.ToString() != "ThreadAbortException")
                {
                    LogException(ex);
                    throw new Exception("Error Creating PDF..." + ex.Message);
                }
            }
        }


        private MemoryStream GetCarStream(string CarOid)
        {
            //string fileName;
            float[] Width2 = new float[] { 3f, 12f };
            float[] Width21 = new float[] { 15f, 30f };
            float[] Width3 = new float[] { 1f, 6f, 1f };
            float[] Width4 = new float[] { 2f, 5f, 2f, 5f };
            float[] Width41 = new float[] { 3f, 5f, 4f, 5f };
            float[] Width42 = new float[] { 3f, 5f, 2f, 2f };
            float[] Width43 = new float[] { 1f, 5f, 1f, 1f };
            float[] Width5 = new float[] { 1f, 1f, 1f, 4f, 1f };
            float[] Width51 = new float[] { 1f, 5f, 1f, 1f, 1f };
            float[] Width6 = new float[] { 1f, 1f, 1f, 6f, 1f, 1f };

            iTextSharp.text.Font font11Bold = FontFactory.GetFont("ARIAL", 11, 1, iTextSharp.text.BaseColor.WHITE);
            iTextSharp.text.Font font10Bold = FontFactory.GetFont("ARIAL", 10, 1);
            iTextSharp.text.Font font10 = FontFactory.GetFont("ARIAL", 10);

            MemoryStream myMemoryStream = new MemoryStream();

            iTextSharp.text.Document doc;
            doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, 10, 10, 20, 20);

            PdfWriter wri = PdfWriter.GetInstance(doc, myMemoryStream);

            wri.CloseStream = false;
            doc.Open();

            GetRecord xCar;
            xCar = new GetRecord();
            xCar.CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(CarOid)));

            using (DataTable dt = xCar.Exec_Get_Existing_Car_By_Oid_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    doc.AddTitle("Halliburton Corrective Action");
                    doc.AddSubject("CAR " + row["CAR_NBR"].ToString());
                    doc.AddCreator("Corrective Action");
                    doc.AddAuthor(removeChar.SanitizeQuoteString(MySession.Current.SessionFullName));


                    //********* Create General Info Header *********//
                    PdfPTable tableInfo = new PdfPTable(2);
                    tableInfo.TotalWidth = 580f;
                    tableInfo.LockedWidth = true;
                    tableInfo.SetWidths(Width21);
                    tableInfo.SpacingAfter = 7f;

                    PdfPCell headerInfo = new PdfPCell(new Phrase("Halliburton - Corrective Action Request #" + row["CAR_NBR"].ToString(), font11Bold));
                    headerInfo.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

                    headerInfo.Colspan = 2;
                    tableInfo.AddCell(headerInfo);

                    //*** Col1 Header
                    PdfPTable tblCol1 = new PdfPTable(1);
                    tblCol1.AddCell(new Phrase("Originator", font10Bold));
                    tblCol1.AddCell(new Phrase("Date Issued", font10Bold));
                    tblCol1.AddCell(new Phrase("Due Date", font10Bold));
                    tblCol1.AddCell(new Phrase("Finding Type", font10Bold));
                    tblCol1.AddCell(new Phrase("Area", font10Bold));
                    tblCol1.AddCell(new Phrase("PSL", font10Bold));
                    tblCol1.AddCell(new Phrase("Plant", font10Bold));

                    string[] strApiHeader = row["API_ISO_ELEM"].ToString().Split('|');
                    for (int i = 0; i < (strApiHeader.Length); i++)
                    {
                        if (i == 0)
                        {
                            tblCol1.AddCell(new Phrase("API/ISO Reference", font10Bold));
                        }
                        else
                        {
                            tblCol1.AddCell(new Phrase(" ", font10Bold));
                        }
                    }

                    tblCol1.AddCell(new Phrase("Audit Number", font10Bold));
                    tblCol1.AddCell(new Phrase("Q Note Number", font10Bold));
                    tblCol1.AddCell(new Phrase("CPI Number", font10Bold));
                    tblCol1.AddCell(new Phrase("Material Number", font10Bold));
                    tblCol1.AddCell(new Phrase("Purchase Order Number", font10Bold));
                    tblCol1.AddCell(new Phrase("Production Order Number", font10Bold));
                    tblCol1.AddCell(new Phrase("API Audit Number", font10Bold));
                    tblCol1.AddCell(new Phrase("Category", font10Bold));
                    tblCol1.AddCell(new Phrase("Responsible Person", font10Bold));
                    tblCol1.AddCell(new Phrase("Vendor Number", font10Bold));
                    tblCol1.AddCell(new Phrase("Vendor Name", font10Bold));
                    tblCol1.AddCell(new Phrase("Issued To", font10Bold));
                    tblCol1.AddCell(new Phrase("Location/Country", font10Bold));
                    tblCol1.AddCell(new Phrase("Location/City State", font10Bold));

                    tblCol1.AddCell(new Phrase("Action Taken By", font10Bold));
                    tblCol1.AddCell(new Phrase("Reponse Date", font10Bold));

                    PdfPCell nestCol1 = new PdfPCell(tblCol1);
                    nestCol1.Padding = 0f;
                    tableInfo.AddCell(nestCol1);

                    //*** Col2 Data
                    PdfPTable tblCol2 = new PdfPTable(1);
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["ORIGINATOR_USR_NM"].ToString())))), font10));
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["DATE_ISSUED"].ToString())))), font10));
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["DUE_DT"].ToString())))), font10));
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["FINDING_TYPE_NM"].ToString())))), font10));
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["AREA_DESCRIPT_NM"].ToString())))), font10));
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["PSL_NM"].ToString())))), font10));
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["FACILITY_NAME"].ToString())))), font10));

                    string[] strApiiso = row["API_ISO_ELEM"].ToString().Split('|');

                    if (strApiiso.Length == 0)
                    {
                        tblCol2.AddCell(new Phrase(row["API_ISO_ELEM"].ToString(), font10));
                    }
                    else
                    {
                        foreach (string api in strApiiso)
                        {
                            tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(api.ToString())))), font10));
                        }
                    }

                    tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["AUDIT_NBR"].ToString()))), font10));
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["QNOTE_NBR"].ToString()))), font10));
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["CPI_NBR"].ToString()))), font10));
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["MATERIAL_NBR"].ToString()))), font10));
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["PURCHASE_ORDER_NBR"].ToString()))), font10));
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["PRODUCTION_ORDER_NBR"].ToString()))), font10));
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["API_AUDIT_NBR"].ToString()))), font10));
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["CATEGORY_NM"].ToString()))), font10));

                    if (row["RESP_PERSON_USR_NM"].ToString().Trim().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["RESP_PERSON_USR_NM"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["VNDR_NBR"].ToString().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["VNDR_NBR"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["VENDOR_NM"].ToString().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["VENDOR_NM"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["ISSUED_TO_USR_NM"].ToString().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["ISSUED_TO_USR_NM"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["LOC_COUNTRY_NM"].ToString().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["LOC_COUNTRY_NM"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["LOC_SUPPLIER"].ToString().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["LOC_SUPPLIER"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["ACTION_TAKEN_BY_USR_NM"].ToString().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["ACTION_TAKEN_BY_USR_NM"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["RESPONSE_DT"].ToString().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["RESPONSE_DT"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    PdfPCell nestCol2 = new PdfPCell(tblCol2);
                    nestCol2.Padding = 0f;
                    tableInfo.AddCell(nestCol2);

                    tableInfo.SpacingAfter = 7f;
                    doc.Add(tableInfo);

                    //********* Create Description of Finding Header *********//
                    PdfPTable tableDof = new PdfPTable(1);
                    tableDof.TotalWidth = 580f;
                    tableDof.LockedWidth = true;
                    //tableInfo.SetWidths(Width41);
                    tableDof.SpacingAfter = 7f;

                    PdfPCell headerDof = new PdfPCell(new Phrase("Description of Finding", font11Bold));
                    headerDof.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

                    tableDof.AddCell(headerDof);

                    //*** Col Dof
                    PdfPTable tblCol_Dof = new PdfPTable(1);
                    tblCol_Dof.AddCell(new Phrase(row["FINDING_DESC"].ToString(), font10));

                    PdfPCell nestDof = new PdfPCell(tblCol_Dof);
                    nestDof.Padding = 0f;

                    tableDof.AddCell(nestDof);

                    tableDof.SpacingAfter = 7f;
                    doc.Add(tableDof);


                    //********* Create Description of Improvement Header *********//
                    PdfPTable tableDoi = new PdfPTable(1);
                    tableDoi.TotalWidth = 580f;
                    tableDoi.LockedWidth = true;
                    tableDoi.SpacingAfter = 7f;

                    PdfPCell headerDoi = new PdfPCell(new Phrase("Description of Improvement", font11Bold));
                    headerDoi.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

                    tableDoi.AddCell(headerDoi);

                    //*** Col Dof
                    PdfPTable tblCol_Doi = new PdfPTable(1);
                    tblCol_Doi.AddCell(new Phrase(row["DESC_OF_IMPROVEMENT"].ToString(), font10));

                    PdfPCell nestDoi = new PdfPCell(tblCol_Doi);
                    nestDoi.Padding = 0f;

                    tableDoi.AddCell(nestDoi);

                    tableDoi.SpacingAfter = 7f;
                    doc.Add(tableDoi);

                    doc.NewPage();

                    //********* Question 1  *********//
                    PdfPTable tableQ1 = new PdfPTable(1);
                    tableQ1.TotalWidth = 580f;
                    tableQ1.LockedWidth = true;
                    tableQ1.SpacingAfter = 7f;

                    PdfPCell headerQ1 = new PdfPCell(new Phrase("Why did this nonconformance occur?", font11Bold));
                    headerQ1.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

                    tableQ1.AddCell(headerQ1);

                    PdfPTable tblCol_Q1 = new PdfPTable(1);

                    if (row["NON_CONFORM_RSN"].ToString().Length > 0)
                    {
                        tblCol_Q1.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["NON_CONFORM_RSN"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol_Q1.AddCell(new Phrase(" ", font10));
                    }

                    PdfPCell nestQ1 = new PdfPCell(tblCol_Q1);
                    nestQ1.Padding = 0f;

                    tableQ1.AddCell(nestQ1);

                    tableQ1.SpacingAfter = 7f;
                    doc.Add(tableQ1);

                    //********* Question 2  *********//
                    PdfPTable tableQ2 = new PdfPTable(1);
                    tableQ2.TotalWidth = 580f;
                    tableQ2.LockedWidth = true;
                    tableQ2.SpacingAfter = 7f;

                    PdfPCell headerQ2 = new PdfPCell(new Phrase("What is the root cause / potential root cause of the non-conformance?", font11Bold));
                    headerQ2.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

                    tableQ2.AddCell(headerQ2);

                    PdfPTable tblCol_Q2 = new PdfPTable(1);

                    if (row["ROOT_CAUSE"].ToString().Length > 0)
                    {
                        tblCol_Q2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["ROOT_CAUSE"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol_Q2.AddCell(new Phrase(" ", font10));
                    }

                    PdfPCell nestQ2 = new PdfPCell(tblCol_Q2);
                    nestQ2.Padding = 0f;

                    tableQ2.AddCell(nestQ2);

                    tableQ2.SpacingAfter = 7f;
                    doc.Add(tableQ2);

                    //********* Question 3  *********//
                    PdfPTable tableQ3 = new PdfPTable(1);
                    tableQ3.TotalWidth = 580f;
                    tableQ3.LockedWidth = true;
                    tableQ3.SpacingAfter = 7f;

                    PdfPCell headerQ3 = new PdfPCell(new Phrase("Are there similar instances of this nonconformance in your area of responsibility? (Y/N)", font11Bold));
                    headerQ3.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

                    tableQ3.AddCell(headerQ3);

                    PdfPTable tblCol_Q3 = new PdfPTable(1);
                    tblCol_Q3.AddCell(new Phrase(row["SIMILAR_INSTANCE_Y_N_TEXT"].ToString(), font10));

                    PdfPCell nestQ3 = new PdfPCell(tblCol_Q3);
                    nestQ3.Padding = 0f;

                    tableQ3.AddCell(nestQ3);

                    if (row["SIMILAR_INSTANCE_Y_N_TEXT"].ToString().ToUpper() == "YES")
                    {
                        PdfPTable tblCol_Q3Y = new PdfPTable(1);
                        tblCol_Q3Y.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["SIMILAR_INSTANCE"].ToString()))), font10));

                        PdfPCell nestQ3Y = new PdfPCell(tblCol_Q3Y);
                        nestQ3Y.Padding = 0f;

                        tableQ3.AddCell(nestQ3Y);
                    }
                    tableQ3.SpacingAfter = 7f;
                    doc.Add(tableQ3);


                    //********* Question 4  *********//
                    PdfPTable tableQ4 = new PdfPTable(1);
                    tableQ4.TotalWidth = 580f;
                    tableQ4.LockedWidth = true;
                    tableQ4.SpacingAfter = 7f;

                    PdfPCell headerQ4 = new PdfPCell(new Phrase("What action was taken (or is planned) to correct this nonconformance?", font11Bold));
                    headerQ4.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

                    tableQ4.AddCell(headerQ4);

                    PdfPTable tblCol_Q4 = new PdfPTable(1);

                    if (row["CORR_ACTION_TAKEN"].ToString().Length > 0)
                    {
                        tblCol_Q4.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["CORR_ACTION_TAKEN"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol_Q4.AddCell(new Phrase(" ", font10));
                    }

                    PdfPCell nestQ4 = new PdfPCell(tblCol_Q4);
                    nestQ4.Padding = 0f;

                    tableQ4.AddCell(nestQ4);

                    tableQ4.SpacingAfter = 7f;
                    doc.Add(tableQ4);

                    PdfPTable tableQ4D = new PdfPTable(1);
                    tableQ4D.TotalWidth = 580f;
                    tableQ4D.LockedWidth = true;
                    tableQ4D.SpacingAfter = 7f;

                    PdfPCell headerQ4D = new PdfPCell(new Phrase("What is the scheduled implementation date?", font11Bold));
                    headerQ4D.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

                    tableQ4D.AddCell(headerQ4D);

                    PdfPTable tblCol_Q4D = new PdfPTable(1);

                    if (row["CORR_ACTION_TAKEN_DT"].ToString().Length > 0)
                    {
                        tblCol_Q4D.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["CORR_ACTION_TAKEN_DT"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol_Q4D.AddCell(new Phrase(" ", font10));
                    }

                    PdfPCell nestQ4D = new PdfPCell(tblCol_Q4D);
                    nestQ4D.Padding = 0f;

                    tableQ4D.AddCell(nestQ4D);

                    tableQ4D.SpacingAfter = 7f;
                    doc.Add(tableQ4D);

                    //********* Question 5  *********//
                    PdfPTable tableQ5 = new PdfPTable(1);
                    tableQ5.TotalWidth = 580f;
                    tableQ5.LockedWidth = true;
                    tableQ5.SpacingAfter = 7f;

                    PdfPCell headerQ5 = new PdfPCell(new Phrase("What action was taken (or is planned) to preclude this and similar non-conformances?", font11Bold));
                    headerQ5.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

                    tableQ5.AddCell(headerQ5);

                    PdfPTable tblCol_Q5 = new PdfPTable(1);

                    if (row["PRECLUDE_ACTION"].ToString().Length > 0)
                    {
                        tblCol_Q5.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["PRECLUDE_ACTION"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol_Q5.AddCell(new Phrase(" ", font10));
                    }

                    PdfPCell nestQ5 = new PdfPCell(tblCol_Q5);
                    nestQ5.Padding = 0f;

                    tableQ5.AddCell(nestQ5);

                    tableQ5.SpacingAfter = 7f;
                    doc.Add(tableQ5);

                    PdfPTable tableQ5D = new PdfPTable(1);
                    tableQ5D.TotalWidth = 580f;
                    tableQ5D.LockedWidth = true;
                    tableQ5D.SpacingAfter = 7f;

                    PdfPCell headerQ5D = new PdfPCell(new Phrase("What is the scheduled implementation date? ", font11Bold));
                    headerQ5D.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

                    tableQ5D.AddCell(headerQ5D);

                    PdfPTable tblCol_Q5D = new PdfPTable(1);

                    if (row["PRECLUDE_ACTION_DT"].ToString().Length > 0)
                    {
                        tblCol_Q5D.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["PRECLUDE_ACTION_DT"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol_Q5D.AddCell(new Phrase(" ", font10));
                    }

                    PdfPCell nestQ5D = new PdfPCell(tblCol_Q5D);
                    nestQ5D.Padding = 0f;

                    tableQ5D.AddCell(nestQ5D);

                    tableQ5D.SpacingAfter = 7f;
                    doc.Add(tableQ5D);

                    doc.NewPage();

                    //********* Create Originator Header *********//
                    PdfPTable tableOrig = new PdfPTable(2);
                    tableOrig.TotalWidth = 580f;
                    tableOrig.LockedWidth = true;
                    tableOrig.SetWidths(Width21);
                    tableOrig.SpacingAfter = 7f;

                    PdfPCell headerOrig = new PdfPCell(new Phrase("Originator Use Only", font11Bold));
                    headerOrig.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

                    headerOrig.Colspan = 2;
                    tableOrig.AddCell(headerOrig);

                    //*** Col1 Header
                    PdfPTable tblCol = new PdfPTable(1);
                    tblCol.AddCell(new Phrase("Due Date Extension", font10Bold));
                    tblCol.AddCell(new Phrase("Re-Issued To", font10Bold));
                    tblCol.AddCell(new Phrase("Date Re-Issued", font10Bold));
                    tblCol.AddCell(new Phrase("Follow-up Required", font10Bold));
                    tblCol.AddCell(new Phrase("Date to Follow-up", font10Bold));
                    tblCol.AddCell(new Phrase("Date Recieved", font10Bold));
                    tblCol.AddCell(new Phrase("Date Verified", font10Bold));
                    tblCol.AddCell(new Phrase("Verified By", font10Bold));
                    tblCol.AddCell(new Phrase("Response Accepted By", font10Bold));
                    tblCol.AddCell(new Phrase("Status", font10Bold));
                    tblCol.AddCell(new Phrase("Close Date", font10Bold));
                    tblCol.AddCell(new Phrase("How was effectiveness validated?", font10Bold));

                    PdfPCell nestCol = new PdfPCell(tblCol);
                    nestCol.Padding = 0f;
                    tableOrig.AddCell(nestCol);

                    //*** Col2 Data
                    PdfPTable tblCol_2 = new PdfPTable(1);

                    if (row["DUE_DT_EXT"].ToString().Length > 0)
                    {
                        tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["DUE_DT_EXT"].ToString())))), font10));
                    }
                    else
                    {
                        tblCol_2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["REISSUED_TO_USR_NM"].ToString().Length > 0)
                    {
                        tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["REISSUED_TO_USR_NM"].ToString())))), font10));
                    }
                    else
                    {
                        tblCol_2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["REISSUED_DT_TEXT"].ToString().Length > 0)
                    {
                        tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["REISSUED_DT_TEXT"].ToString())))), font10));
                    }
                    else
                    {
                        tblCol_2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["FOLLOW_UP_REQD_TEXT"].ToString().Length > 0)
                    {
                        tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["FOLLOW_UP_REQD_TEXT"].ToString())))), font10));
                    }
                    else
                    {
                        tblCol_2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["FOLLOW_UP_DT"].ToString().Length > 0)
                    {
                        tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["FOLLOW_UP_DT"].ToString())))), font10));
                    }
                    else
                    {
                        tblCol_2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["RECEIVED_DT"].ToString().Length > 0)
                    {
                        tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["RECEIVED_DT"].ToString())))), font10));
                    }
                    else
                    {
                        tblCol_2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["VERIFY_DT"].ToString().Length > 0)
                    {
                        tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["VERIFY_DT"].ToString())))), font10));
                    }
                    else
                    {
                        tblCol_2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["VERIFIED_BY_USR_NM"].ToString().Length > 0)
                    {
                        tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["VERIFIED_BY_USR_NM"].ToString())))), font10));
                    }
                    else
                    {
                        tblCol_2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["RESPONSE_ACCEPT_BY_USR_NM"].ToString().Length > 0)
                    {
                        tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["RESPONSE_ACCEPT_BY_USR_NM"].ToString())))), font10));
                    }
                    else
                    {
                        tblCol_2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["CAR_STATUS_NM"].ToString().Length > 0)
                    {
                        tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["CAR_STATUS_NM"].ToString())))), font10));
                    }
                    else
                    {
                        tblCol_2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["CLOSE_DT"].ToString().Length > 0)
                    {
                        tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["CLOSE_DT"].ToString())))), font10));
                    }
                    else
                    {
                        tblCol_2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["REMARKS"].ToString().Length > 0)
                    {
                        tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["REMARKS"].ToString())))), font10));
                    }
                    else
                    {
                        tblCol_2.AddCell(new Phrase(" ", font10));
                    }

                    PdfPCell nestCol_2 = new PdfPCell(tblCol_2);
                    nestCol_2.Padding = 0f;
                    tableOrig.AddCell(nestCol_2);

                    doc.Add(tableOrig);

                    doc.Close();

                    myMemoryStream.Flush(); //Always caches me out  
                    myMemoryStream.Position = 0;
                }
            }

            return myMemoryStream;
        }



        protected void ToggleFileRowSelection_RadGrid1(object sender, EventArgs e)
        {
            ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
            GridDataItem dataItem;
            dataItem = ((sender as CheckBox).NamingContainer as GridItem) as GridDataItem;

            if ((sender as CheckBox).Checked)
            {
                dataItem["FILENAME"].Style["color"] = "red";

            }
            else
            {
                dataItem["FILENAME"].Style["color"] = "black";
            }

        }


        // Application event log
        private void LogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog();

            appLog.ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionSource = "Internal_Forms_Index.aspx";
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
            appLog.ExceptionSource = "Internal_Forms_Document.aspx";
            appLog.ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionUrl = "CustomLogException";
            appLog.ExceptionTargetSite = ex.Data["TargetSite"].ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionStackTrace = ex.Data["StackTrace"].ToString().Replace(oldValue: "'", newValue: "`");

            appLog.AppLogEvent();
        }
    }
}