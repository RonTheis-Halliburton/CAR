using ComponentPro.Net;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CAR.Forms
{
    public partial class Document : System.Web.UI.Page
    {
        static Document()
        {
            ComponentPro.Licensing.Common.LicenseManager.SetLicenseKey(ComponentPro.LicenseKey.Key);
        }

        public string AdminContact = ConfigurationManager.AppSettings["AdminContact"].ToString();
        public string ErrException = "If problem persists, please contact the administrator: " + ConfigurationManager.AppSettings["AdminContact"];
        public string FtpSrvNm = ConfigurationManager.AppSettings["FtpSrvNm"].ToString();
        public string FtpSrvDir = ConfigurationManager.AppSettings["FtpSrvDir"].ToString();
        public string FtpSrvSharedNm = ConfigurationManager.AppSettings["FtpSrvSharedNm"].ToString();
        public string FtpU = ConfigurationManager.AppSettings["FtpU"].ToString();
        public string FtpP = ConfigurationManager.AppSettings["FtpP"].ToString();
        public string FtpSrvType = "/" + ConfigurationManager.AppSettings["FtpSrvType"].ToString();

        SanitizeString removeChar = new SanitizeString();
        MyUtil.HtmlUtility htmlUtil = MyUtil.HtmlUtility.Instance;
        //private Rectangle lETTER;
        //private int v1;
        //private int v2;
        //private int v3;
        //private int v4;

        //public Document(Rectangle lETTER, int v1, int v2, int v3, int v4)
        //{
        //    this.lETTER = lETTER;
        //    this.v1 = v1;
        //    this.v2 = v2;
        //    this.v3 = v3;
        //    this.v4 = v4;
        //}

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
                        this.CAR_OID.Value = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Request.QueryString["OID"].ToString())));
                        this.CAR_NBR.Value = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Request.QueryString["CAR_NBR"].ToString())));
                        this.YEAR_ISSUED.Value = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Request.QueryString["YEAR_ISSUED"].ToString())));

                        this.Header.Title = Server.HtmlEncode(htmlUtil.SanitizeHtml("File Upload - CAR Nbr:  " + Request.QueryString["CAR_NBR"].ToString() + "     [Status:  " + Request.QueryString["Status_NM"].ToString() + "]"));

                        //CreateRootFolder();

                        this.Session["FileUpload"] = null;
                        this.RadGrid1.MasterTableView.SortExpressions.Clear();
                        this.RadGrid1.DataSource = (DataTable)this.Session["FileUpload"];
                        this.RadGrid1.DataBind();

                        LoadDocument();
                    }
                }
                catch (Exception ex)
                {
                    LogException(ex);
                    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
                }
            }
        }


        protected void CreateRootFolder()
        {
            //try
            //{

            using (Sftp clientFtp = new Sftp())
            {
                //string ftpRemotePath = string.Empty;
                string ftpRemoteYearPath = string.Empty;

                clientFtp.Connect(FtpSrvNm, 22);

                if (clientFtp.IsConnected)
                {
                    //// *** Authenticate FTP server
                    clientFtp.Authenticate(FtpU, FtpP);

                    if (clientFtp.IsAuthenticated)
                    {
                        ////ftpRemotePath = "/" + FtpSrvType;

                        //// ** If not exist, create Server Type Folder
                        if (!clientFtp.DirectoryExists(FtpSrvType))
                        {
                            clientFtp.CreateDirectory(FtpSrvType);
                        }

                        ftpRemoteYearPath = FtpSrvType + "/" + this.YEAR_ISSUED.Value;

                        if (!clientFtp.DirectoryExists(ftpRemoteYearPath))
                        {
                            clientFtp.CreateDirectory(ftpRemoteYearPath);
                        }
                    }
                    else
                    {
                        int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                        Exception aex = new Exception("Create Source Folder - FTP Failed Authentication");
                        aex.Data.Add(key: "TargetSite", value: "FTP Authentication");
                        aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                        CustomLogException(aex);

                        ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Create Source Folder - FTP Failed Authentication.\\n\\n" + ErrException + "');", addScriptTags: true);
                    }
                }
                else
                {
                    int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                    Exception aex = new Exception("Create Source Folder - FTP Failed to connect server " + FtpSrvNm);
                    aex.Data.Add(key: "TargetSite", value: "FTP Server Connection");
                    aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                    CustomLogException(aex);

                    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Create Source Folder - FTP Failed to connect server .\\n\\n" + ErrException + "');", addScriptTags: true);
                }

                clientFtp.Disconnect();
            }
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);
            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            //}
        }


        protected void CreateCarFolder()
        {
            //try
            //{

            using (Sftp clientFtp = new Sftp())
            {
                clientFtp.Connect(FtpSrvNm, 22);

                if (clientFtp.IsConnected)
                {
                    //// *** Authenticate FTP server
                    clientFtp.Authenticate(FtpU, FtpP);

                    if (clientFtp.IsAuthenticated)
                    {
                        string ftpRemoteCarPath = string.Empty;
                        ftpRemoteCarPath = FtpSrvType + "/" + this.YEAR_ISSUED.Value + "/" + this.CAR_NBR.Value;

                        if (!clientFtp.DirectoryExists(ftpRemoteCarPath))
                        {
                            clientFtp.CreateDirectory(ftpRemoteCarPath);
                        }
                    }
                    else
                    {
                        int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                        Exception aex = new Exception("Create Source Folder - FTP Failed Authentication");
                        aex.Data.Add(key: "TargetSite", value: "FTP Authentication");
                        aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                        CustomLogException(aex);

                        ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Create Source Folder - FTP Failed Authentication.\\n\\n" + ErrException + "');", addScriptTags: true);
                    }
                }
                else
                {
                    int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                    Exception aex = new Exception("Create Source Folder - FTP Failed to connect server " + FtpSrvNm);
                    aex.Data.Add(key: "TargetSite", value: "FTP Server Connection");
                    aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                    CustomLogException(aex);
                }

                clientFtp.Disconnect();
            }
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);
            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            //}
        }

        void MakeFileCart()
        {
            DataTable objDT;
            objDT = new DataTable(tableName: "FileUpload");
            objDT.Columns.Add(columnName: "OID", type: typeof(string));
            objDT.Columns.Add(columnName: "CAR_OID", type: typeof(string));
            objDT.Columns.Add(columnName: "CAR_NBR", type: typeof(string));
            objDT.Columns.Add(columnName: "FileSize", type: typeof(string));
            objDT.Columns.Add(columnName: "FileSize_Byte", type: typeof(string));
            objDT.Columns.Add(columnName: "FileName", type: typeof(string));
            objDT.Columns.Add(columnName: "FileNameAlias", type: typeof(string));
            objDT.Columns.Add(columnName: "UploadedBy", type: typeof(string));
            objDT.Columns.Add(columnName: "DateUploaded", type: typeof(string));
            objDT.Columns.Add(columnName: "FtpRemotePath", type: typeof(string));
            objDT.Columns.Add(columnName: "Year_Issued", type: typeof(string));
            this.Session["FileUpload"] = objDT;
        }


        protected void LoadDocument()
        {
            GetRecord getFile;
            getFile = new GetRecord
            {
                CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.CAR_OID.Value)))
            };

            using (DataTable dt = getFile.Exec_GetCar_Document_Datatable())
            {
                this.RadGrid1.DataSource = dt;
                this.RadGrid1.DataBind();

                this.Session["FileUpload"] = dt;
            }
        }

        protected void AsyncUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            try
            {
                DataTable objDT;
                DataRow objDR;

                this.Label6.Text = string.Empty;

                if (e.IsValid == true)
                {
                    string strAliasFileName = string.Empty;

                    string strFileName = e.File.FileName.ToString();
                    string strExtension = e.File.GetExtension().ToString().Replace(oldValue: ".", newValue: "").ToLower();
                    string strFileType = e.File.ContentType.ToString();
                    string strFileSize = e.File.ContentLength.ToString();

                    object obj = this.Session["FileUpload"];
                    objDT = (DataTable)obj;

                    if (objDT == null)
                    {
                        MakeFileCart();
                        LoadDocument();
                    }

                    using (Sftp clientFtp = new Sftp())
                    {
                        string ftpRemotePath = string.Empty;
                        string ftpRemoteCarPath = string.Empty;

                        clientFtp.Connect(FtpSrvNm, 22);

                        if (clientFtp.IsConnected)
                        {
                            //// *** Authenticate FTP server
                            clientFtp.Authenticate(FtpU, FtpP);

                            if (clientFtp.IsAuthenticated)
                            {

                                CreateRootFolder();

                                ftpRemoteCarPath = FtpSrvType + "/" + this.YEAR_ISSUED.Value + "/" + this.CAR_NBR.Value;

                                if (!clientFtp.DirectoryExists(ftpRemoteCarPath))
                                {
                                    clientFtp.CreateDirectory(ftpRemoteCarPath);
                                }

                                if (clientFtp.DirectoryExists(ftpRemoteCarPath))
                                {
                                    strAliasFileName = DateTime.Now.ToString(format: "yyyyMMddHHmmss") + "." + strExtension;
                                    ///ftpRemotePath = "\\" + "\\" + FtpSrvNm + "\\" + FtpSrvSharedNm + "\\DOCUMENTS\\" + FtpSrvType.Replace("/", "") + "\\" + this.YEAR_ISSUED.Value + "\\" + this.CAR_NBR.Value + "\\" + strAliasFileName;
                                    ftpRemotePath = "\\" + "\\" + FtpSrvNm + "\\" + FtpSrvSharedNm + "\\" + FtpSrvType.Replace("/", "") + "\\" + this.YEAR_ISSUED.Value + "\\" + this.CAR_NBR.Value + "\\" + strAliasFileName;

                                    //*** Upload file                                    
                                    using (Stream fileStream = e.File.InputStream)
                                    {


                                        //clientFtp.UploadFile(fileStream, ftpRemoteCarPath + "/" + strAliasFileName);

                                        //// *** validate pdf pages
                                        byte[] fileBinaryData = new byte[1024 * 100];
                                        BinaryReader br = new BinaryReader(fileStream);
                                        fileBinaryData = br.ReadBytes((Int32)fileStream.Length);

                                        PdfReader reader;
                                        reader = new PdfReader(fileBinaryData);

                                        if (reader.NumberOfPages == 0)
                                        {
                                            ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('Error... File size is  " + fileBinaryData.Length.ToString() + "\\nPlease verfiy and try again.');", addScriptTags: true);
                                        }
                                        else
                                        {
                                            MemoryStream ms = new MemoryStream(fileBinaryData);
                                            clientFtp.UploadFile(ms, ftpRemoteCarPath + "/" + strAliasFileName);

                                            ms.Dispose();

                                            //clientFtp.UploadFile(fileStream, ftpRemoteCarPath + "/" + strAliasFileName);

                                        }
                                    }

                                    Thread.Sleep(millisecondsTimeout: 2000); //Give FTP upload extra time to finish


                                    //// ***  Check file made it to destination, write record to database
                                    if (clientFtp.FileExists(ftpRemoteCarPath + "/" + strAliasFileName))
                                    {
                                        string[] docReturn = { string.Empty, string.Empty };

                                        CreateRecord doc;
                                        doc = new CreateRecord
                                        {
                                            Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.CAR_OID.Value))),
                                            File_Size = strFileSize,
                                            File_Name = strFileName,
                                            File_NameAlias = strAliasFileName,
                                            Uploaded_By = MySession.Current.SessionFullName,
                                            Ftp_RemotePath = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(ftpRemotePath)))
                                        };

                                        docReturn = doc.ExecCreateCar_Document();

                                        if (docReturn[0].ToString() == "Successfully")
                                        {
                                            objDR = objDT.NewRow();
                                            objDR["OID"] = docReturn[1].ToString();
                                            objDR["CAR_OID"] = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.CAR_OID.Value)));
                                            objDR["FileSize"] = strFileSize.ToString();
                                            objDR["FileSize_Byte"] = htmlUtil.FileSizeByte(e.File.ContentLength);
                                            objDR["FileName"] = strFileName.ToString();
                                            objDR["FileNameAlias"] = strAliasFileName.ToString();
                                            objDR["UploadedBy"] = MySession.Current.SessionFullName.ToString().Replace("'", "`");
                                            objDR["DateUploaded"] = DateTime.Now.ToString();
                                            objDR["FtpRemotePath"] = ftpRemotePath;
                                            objDR["CAR_NBR"] = this.CAR_NBR.Value;
                                            objDR["Year_Issued"] = this.YEAR_ISSUED.Value;

                                            objDT.Rows.Add(objDR);
                                            this.Session["FileUpload"] = objDT;

                                            this.RadGrid1.DataSource = objDT;
                                            this.RadGrid1.DataBind();
                                        }
                                        else
                                        {
                                            ///** If failed write to database, delete file.
                                            if (clientFtp.FileExists(ftpRemoteCarPath + "/" + strAliasFileName))
                                            {
                                                clientFtp.DeleteFile(ftpRemoteCarPath + "/" + strAliasFileName);
                                            }

                                            int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                            Exception aex = new Exception("SQL Failed Insert Record:  " + ftpRemoteCarPath.ToString().ToUpper() + "/" + strAliasFileName);
                                            aex.Data.Add(key: "TargetSite", value: "CAR_OID:  " + this.CAR_OID.Value);
                                            aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                            CustomLogException(aex);
                                            this.Label6.Text = "DB Failed Insert Record.  " + ErrException;

                                            this.RadWindowManager1.RadAlert("DB Failed Insert Record.<br/><br/>" + ErrException, 400, 250, "Alert", string.Empty, string.Empty);
                                        }
                                    }
                                    else
                                    {
                                        this.Label6.Text = "Unable to locate the uploaded file";

                                        int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                        Exception aex = new Exception("Unable to locate the uploaded file:  " + ftpRemoteCarPath.ToString().ToUpper() + "/" + strAliasFileName);
                                        aex.Data.Add(key: "TargetSite", value: "Unable to locate the uploaded file");
                                        aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                        CustomLogException(aex);
                                        this.Label6.Text = "Unable to locate the uploaded file.  " + ErrException;

                                        this.RadWindowManager1.RadAlert("Unable to locate the uploaded file.<br/><br/>" + ErrException, 400, 250, "Alert", string.Empty, string.Empty);
                                    }
                                }
                                else
                                {
                                    int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                    Exception aex = new Exception("Remote Path Not Found:  " + ftpRemoteCarPath.ToString().ToUpper());
                                    aex.Data.Add(key: "TargetSite", value: "Folder Not Found");
                                    aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                    CustomLogException(aex);
                                    this.Label6.Text = Server.HtmlEncode(htmlUtil.SanitizeHtml("Remote Path Not Found:  " + ftpRemoteCarPath.ToString().ToUpper()));

                                    this.RadWindowManager1.RadAlert("Remote Path Not Found:  " + ftpRemoteCarPath.ToString().ToUpper() + ".<br/><br/>" + ErrException, 400, 250, "Alert", string.Empty, string.Empty);
                                }
                            }
                            else
                            {

                                int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                Exception aex = new Exception("FTP Failed Authentication");
                                aex.Data.Add(key: "TargetSite", value: "FTP Authentication");
                                aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                CustomLogException(aex);
                                this.Label6.Text = "FTP Failed Authentication";

                                this.RadWindowManager1.RadAlert("FTP Failed Authentication<br/><br/>" + ErrException, 400, 250, "Alert", string.Empty, string.Empty);
                            }
                        }
                        else
                        {
                            int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                            Exception aex = new Exception("FTP Failed to connect server " + FtpSrvNm);
                            aex.Data.Add(key: "TargetSite", value: "FTP Server Connection");
                            aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                            CustomLogException(aex);
                            this.Label6.Text = "FTP Failed to connect server " + FtpSrvNm;

                            this.RadWindowManager1.RadAlert("FTP Failed to connect " + FtpSrvNm + ".<br/><br/>" + ErrException, 400, 250, "Alert", string.Empty, string.Empty);
                        }

                        clientFtp.Disconnect();
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                this.RadWindowManager1.RadAlert("Document Error<br/><br/>" + removeChar.SanitizeQuoteString(ex.Message) + "<br/><br/>" + ErrException, 400, 250, "Alert", string.Empty, string.Empty);
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            object obj = this.Session["FileUpload"];
            if ((!(obj == null)))
            {
                this.RadGrid1.DataSource = (DataTable)(obj);
            }
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem item = e.Item as GridDataItem;

                if (item != null)
                {
                    TableCell uploadedByCell = item["UploadedBy"];
                    ImageButton deleteImg = item.FindControl("ImageButton1") as ImageButton;

                    if (deleteImg != null)
                    {
                        if (MySession.Current.SessionCarAdmin == true || MySession.Current.SessionSystemAdmin == true)
                        {
                            //Admin can delete
                        }
                        else
                        {
                            if (uploadedByCell.Text.Trim().ToLower() != MySession.Current.SessionFullName.ToString().Trim().ToLower().Replace("'", "`"))
                            {
                                deleteImg.Enabled = false;
                                deleteImg.ToolTip = "Uploaded by " + Server.HtmlDecode(uploadedByCell.Text);
                                deleteImg.ImageUrl = "~/Img/Lock.png";
                                deleteImg.Controls.Clear();
                                deleteImg.Attributes.Clear();
                            }
                        }
                    }
                }
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                this.Label6.Text = string.Empty;
                string oid = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OID"].ToString();
                string car_Oid = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CAR_OID"].ToString();
                string car_Nbr = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CAR_NBR"].ToString();
                string year_Issued = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["YEAR_ISSUED"].ToString();
                string file_Name_Alias = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FILENAMEALIAS"].ToString();

                GridDataItem dataItem;
                dataItem = e.Item as GridDataItem;

                if (e.CommandName == "Download2")
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
                                        this.RadWindowManager1.RadAlert("Error...Unable to locate selected file.\\n\\n" + ErrException, 350, 200, "Error Alert", string.Empty, string.Empty);
                                    }
                                }
                                else
                                {
                                    int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                    Exception aex = new Exception("FTP Failed Authentication");
                                    aex.Data.Add(key: "TargetSite", value: "FTP Authentication");
                                    aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                    CustomLogException(aex);
                                    this.RadWindowManager1.RadAlert("FTP Failed Authentication\\n\\n" + ErrException, 400, 250, "Alert", string.Empty, string.Empty);
                                }
                            }
                            else
                            {
                                int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                Exception aex = new Exception("FTP Failed to connect server " + FtpSrvNm);
                                aex.Data.Add(key: "TargetSite", value: "FTP Server Connection");
                                aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                CustomLogException(aex);
                                this.RadWindowManager1.RadAlert("FTP Failed to connect server " + FtpSrvNm + ".\\n\\n" + ErrException, 400, 250, "Alert", string.Empty, string.Empty);
                            }

                            clientFtp.Disconnect();
                        }
                    }
                }

                if (e.CommandName == "Download")
                {
                    if (dataItem != null)
                    {
                        using (Sftp clientFtp = new Sftp())
                        {
                            string fileName = dataItem["FILENAME"].Text;
                            string fileSize = dataItem["FILESIZE"].Text;
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
                                        Stream fileStream = clientFtp.GetDownloadStream(ftpRemoteFile);
                                        using (MemoryStream ms = new MemoryStream())
                                        {
                                            fileStream.CopyTo(ms);

                                            Response.Clear();
                                            Response.ClearHeaders();
                                            Response.AddHeader("Content-Type", "Application/octet-stream");
                                            Response.AddHeader("Content-Length", fileSize);
                                            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                                            Response.AddHeader("Connection", "Keep-Alive");
                                            Response.BinaryWrite(ms.ToArray());
                                            Response.Flush();
                                            Response.End();
                                        }
                                    }
                                    else
                                    {
                                        int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                        Exception aex = new Exception("Unable to locate file:  " + ftpRemoteFile);
                                        aex.Data.Add(key: "TargetSite", value: "OID=" + oid + ", CAR_OID=" + car_Oid + ", File Alias=" + file_Name_Alias);
                                        aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                        CustomLogException(aex);
                                        this.Label6.Text = "Delete Failed...";

                                        this.RadWindowManager1.RadAlert("Error...Unable to locate file to delete.\\n\\n" + ErrException, 400, 250, "Alert", string.Empty, string.Empty);
                                    }
                                }
                                else
                                {
                                    int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                    Exception aex = new Exception("FTP Failed Authentication");
                                    aex.Data.Add(key: "TargetSite", value: "FTP Authentication");
                                    aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                    CustomLogException(aex);
                                    this.Label6.Text = "FTP Failed Authentication";

                                    this.RadWindowManager1.RadAlert("FTP Failed Authentication\\n\\n" + ErrException, 400, 250, "Alert", string.Empty, string.Empty);
                                }
                            }
                            else
                            {
                                int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                Exception aex = new Exception("FTP Failed to connect server " + FtpSrvNm);
                                aex.Data.Add(key: "TargetSite", value: "FTP Server Connection");
                                aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                CustomLogException(aex);
                                this.Label6.Text = "FTP Failed to connect server " + FtpSrvNm;

                                this.RadWindowManager1.RadAlert("FTP Failed to connect server " + FtpSrvNm + ".\\n\\n" + ErrException, 400, 250, "Alert", string.Empty, string.Empty);
                            }

                            clientFtp.Disconnect();
                        }
                    }
                }

                if (e.CommandName == "Delete")
                {
                    using (Sftp clientFtp = new Sftp())
                    {
                        string ftpRemoteFolder = string.Empty;
                        string ftpRemoteFile = string.Empty;

                        clientFtp.Connect(FtpSrvNm, 22);

                        if (clientFtp.IsConnected)
                        {
                            //// *** Authenticate FTP server
                            clientFtp.Authenticate(FtpU, FtpP);

                            if (clientFtp.IsAuthenticated)
                            {
                                ftpRemoteFolder = FtpSrvType + "/" + year_Issued + "/" + car_Nbr;
                                ftpRemoteFile = ftpRemoteFolder + "/" + file_Name_Alias;

                                if (clientFtp.FileExists(ftpRemoteFile))
                                {
                                    //Delete selected file
                                    clientFtp.DeleteFile(ftpRemoteFile);

                                    Thread.Sleep(millisecondsTimeout: 2000); //Give FTP extra time to finish

                                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
                                    {
                                        using (SqlCommand cmdDelete = new SqlCommand())
                                        {
                                            cmdDelete.CommandType = CommandType.Text;
                                            cmdDelete.Connection = conn;
                                            conn.Open();

                                            cmdDelete.CommandText = " Delete From [CORRECTIVE_ACTION_DOCUMENT] Where [OID]=@OID";
                                            cmdDelete.Parameters.AddWithValue(parameterName: "@OID", value: Server.HtmlEncode(htmlUtil.SanitizeHtml(oid.ToString())));
                                            cmdDelete.ExecuteNonQuery();
                                        }
                                    }

                                    this.Session["FileUpload"] = null;
                                    LoadDocument();

                                    // Remove an empty directory.
                                    long totalFolderSize = clientFtp.GetDirectorySize(ftpRemoteFolder, true);

                                    if (totalFolderSize == 0)
                                    {
                                        clientFtp.DeleteDirectory(ftpRemoteFolder);
                                    }
                                }
                                else
                                {
                                    int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                    Exception aex = new Exception("Unable to locate file:  " + ftpRemoteFile);
                                    aex.Data.Add(key: "TargetSite", value: "OID=" + oid + ", CAR_OID=" + car_Oid + ", File Alias=" + file_Name_Alias);
                                    aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                    CustomLogException(aex);
                                    this.Label6.Text = "Delete Failed...";

                                    this.RadWindowManager1.RadAlert("Error...Unable to locate file to delete.\\n\\n" + ErrException, 400, 250, "Alert", string.Empty, string.Empty);
                                }
                            }
                            else
                            {
                                int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                Exception aex = new Exception("FTP Failed Authentication");
                                aex.Data.Add(key: "TargetSite", value: "FTP Authentication");
                                aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                CustomLogException(aex);
                                this.Label6.Text = "FTP Failed Authentication";

                                this.RadWindowManager1.RadAlert("FTP Failed Authentication\\n\\n" + ErrException, 400, 250, "Alert", string.Empty, string.Empty);
                            }
                        }
                        else
                        {
                            int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                            Exception aex = new Exception("FTP Failed to connect server " + FtpSrvNm);
                            aex.Data.Add(key: "TargetSite", value: "FTP Server Connection");
                            aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                            CustomLogException(aex);
                            this.Label6.Text = "FTP Failed to connect server " + FtpSrvNm;

                            this.RadWindowManager1.RadAlert("FTP Failed to connect server " + FtpSrvNm + ".\\n\\n" + ErrException, 400, 250, "Alert", string.Empty, string.Empty);
                        }

                        clientFtp.Disconnect();
                    }

                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + ".\\n\\n" + ErrException + "');", addScriptTags: true);
            }
            //finally
            //{
            //    this.AsyncUpload1.Enabled = true;
            //}
        }

        protected void BtnDummy1_Click(object sender, EventArgs e)
        {
            this.AsyncUpload1.Enabled = true;
        }

        protected void RadAjaxPanel1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            //try
            //{

            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);

            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            //}
        }

        // Application event log
        private void LogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog
            {
                ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionSource = "Internal_Forms_Document.aspx",
                ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionUrl = string.Empty,
                ExceptionTargetSite = ex.TargetSite.ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionStackTrace = ex.StackTrace.ToString().Replace(oldValue: "'", newValue: "`")
            };
            appLog.AppLogEvent();
        }

        private void CustomLogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog
            {
                ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionSource = "Internal_Forms_Document.aspx",
                ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionUrl = "CustomLogException",
                ExceptionTargetSite = ex.Data["TargetSite"].ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionStackTrace = ex.Data["StackTrace"].ToString().Replace(oldValue: "'", newValue: "`")
            };

            appLog.AppLogEvent();
        }

    }
}