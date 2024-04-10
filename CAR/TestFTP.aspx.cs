using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
using System.IO;
using ComponentPro.Net;

namespace CAR
{
    public partial class TestFTP : System.Web.UI.Page
    {

        static TestFTP()
        {
            ComponentPro.Licensing.Common.LicenseManager.SetLicenseKey(ComponentPro.LicenseKey.Key);
        }

        public string FtpSrvNm = ConfigurationManager.AppSettings["FtpSrvNm"].ToString();
        public string FtpSrvDir = ConfigurationManager.AppSettings["FtpSrvDir"].ToString();
        public string FtpSrvSharedNm = ConfigurationManager.AppSettings["FtpSrvSharedNm"].ToString();
        public string FtpUsr = ConfigurationManager.AppSettings["FtpU"].ToString();
        public string FtpPwd = ConfigurationManager.AppSettings["FtpP"].ToString();
        

        public string FtpSrvType = ConfigurationManager.AppSettings["FtpSrvType"].ToString();
        //public string FtpSrvType = string.Empty;

        SanitizeString removeChar = new SanitizeString();
        MyUtil.HtmlUtility htmlUtil = MyUtil.HtmlUtility.Instance;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.Label5.Text = "\\" + FtpSrvNm + "\\" + FtpSrvSharedNm + "\\" + FtpSrvDir + "\\" + FtpSrvType;

                CreateRootFolder();
                ListFolder();
                ListFile();
            }
        }

        protected void RadButton1_Click(object sender, EventArgs e)
        {
            this.Label1.Text = string.Empty;

            using (Sftp clientFtp = new Sftp())
            {
                string ftpRemotePath = string.Empty;
                                
                clientFtp.Connect(FtpSrvNm, 22);
                
                if (clientFtp.IsConnected)
                {
                    ////Authenticate FTP server
                    clientFtp.Authenticate(FtpUsr, FtpPwd);

                    if (clientFtp.IsAuthenticated)
                    {
                        this.Label1.Text = "SFTP Connected and Authenticate successfully";
                    }
                    else
                    {
                        this.Label1.Text = "SFTP authentication failed with UID and PWD";
                    }
                }
                else
                {
                    this.Label1.Text = "Failed to connect server " + FtpSrvNm;
                }

                clientFtp.Disconnect();
            }
        }

        protected void CreateRootFolder()
        {
            this.Label2.Text = string.Empty;

            using (Sftp clientFtp = new Sftp())
            {
                string ftpRemotePath = string.Empty;
                               
                clientFtp.Connect(FtpSrvNm, 22);

                if (clientFtp.IsConnected)
                {
                    ////Authenticate FTP server
                    clientFtp.Authenticate(FtpUsr, FtpPwd);

                    if (clientFtp.IsAuthenticated)
                    {

                        ftpRemotePath = "/" + FtpSrvType;

                        if (!clientFtp.DirectoryExists(ftpRemotePath))
                        {
                            clientFtp.CreateDirectory(ftpRemotePath);
                            this.Label2.Text = "Created...";

                            ListFile();
                        }
                    }
                    else
                    {
                        this.Label2.Text = "SFTP authentication failed with UID and PWD";
                    }
                }
                else
                {
                    this.Label2.Text = "Failed to connect server " + FtpSrvNm;
                }

                clientFtp.Disconnect();
            }
        }

        protected void CreateFolder(string folderName)
        {
            CreateRootFolder();

            this.Label2.Text = string.Empty;

            using (Sftp clientFtp = new Sftp())
            {
                string ftpRemotePath = string.Empty;

                clientFtp.Connect(FtpSrvNm, 22);

                if (clientFtp.IsConnected)
                {
                    ////Authenticate FTP server
                    clientFtp.Authenticate(FtpUsr, FtpPwd);

                    if (clientFtp.IsAuthenticated)
                    {
                        //ftpRemotePath = "/"  + FtpSource + "/" + folderName;
                        ftpRemotePath = "/" + FtpSrvType + "/" + folderName;

                        if (!clientFtp.DirectoryExists(ftpRemotePath))
                        {
                            clientFtp.CreateDirectory(ftpRemotePath);
                            this.Label2.Text = "Created...";

                            ListFile();
                        }
                    }
                    else
                    {
                        this.Label2.Text = "FTP authentication failed with UID and PWD";
                    }
                }
                else
                {
                    this.Label2.Text = "Failed to connect server " + FtpSrvNm;
                }

                clientFtp.Disconnect();
            }
        }

        protected void DeleteFolder(string folderName)
        {
            this.Label3.Text = string.Empty;

            using (Sftp clientFtp = new Sftp())
            {
                string ftpRemotePath = string.Empty;

                clientFtp.Connect(FtpSrvNm, 22);

                if (clientFtp.IsConnected)
                {
                    ////Authenticate FTP server
                    clientFtp.Authenticate(FtpUsr, FtpPwd);

                    if (clientFtp.IsAuthenticated)
                    {
                        ftpRemotePath = "/" + FtpSrvType + "/" + folderName;
                        //ftpRemotePath = "/" + folderName;

                        if (clientFtp.DirectoryExists(ftpRemotePath))
                        {
                            clientFtp.DeleteDirectory(ftpRemotePath, true);
                            this.Label3.Text = "Deleted...";
                            ListFile();
                        }
                    }
                    else
                    {
                        this.Label3.Text = "SFTP authentication failed with UID and PWD";
                    }
                }
                else
                {
                    this.Label3.Text = "Failed to connect server " + FtpSrvNm;
                }

                clientFtp.Disconnect();
            }
        }

        protected void ListFolder()
        {
            this.Label4.Text = string.Empty;

            using (Sftp clientFtp = new Sftp())
            {
                string ftpRemotePath = string.Empty;

                clientFtp.Connect(FtpSrvNm, 22);

                if (clientFtp.IsConnected)
                {
                    ////Authenticate FTP server
                    clientFtp.Authenticate(FtpUsr, FtpPwd);

                    if (clientFtp.IsAuthenticated)
                    {
                        this.RadComboBox1.Text = string.Empty;
                        this.RadComboBox1.Items.Clear();

                        ftpRemotePath = "/" + FtpSrvType;

                        clientFtp.SetCurrentDirectory(ftpRemotePath);

                        SftpFileInfoCollection files = (SftpFileInfoCollection)clientFtp.ListDirectory();

                        foreach (SftpFileInfo ftpFile in files)
                        {
                            RadComboBoxItem item;
                            item = new RadComboBoxItem();
                            item.Value = ftpFile.Name.Replace("/", "");
                            item.Text = ftpFile.Name.Replace("/", "");

                            this.RadComboBox1.Items.Add(item);
                        }

                        ListFile();
                    }
                    else
                    {
                        this.Label4.Text = "SFTP authentication failed with UID and PWD";
                    }
                }
                else
                {
                    this.Label4.Text = "Failed to connect server " + FtpSrvNm;
                }

                clientFtp.Disconnect();
            }
        }

        protected void ListFile()
        {
            this.Label7.Text = string.Empty;

            using (Sftp clientFtp = new Sftp())
            {
                string ftpRemotePath = string.Empty;

                clientFtp.Connect(FtpSrvNm, 22);

                if (clientFtp.IsConnected)
                {
                    ////Authenticate FTP server
                    clientFtp.Authenticate(FtpUsr, FtpPwd);

                    if (clientFtp.IsAuthenticated)
                    {
                        if (this.RadComboBox1.Items.Count > 0)
                        {
                            this.RadComboBox2.Text = string.Empty;
                            this.RadComboBox2.Items.Clear();

                            clientFtp.SetCurrentDirectory("/" + FtpSrvType);
                            ftpRemotePath = this.RadComboBox1.SelectedValue;

                            if (clientFtp.DirectoryExists(ftpRemotePath))
                            {
                                foreach (string fileName in clientFtp.ListName(ftpRemotePath))
                                {
                                    RadComboBoxItem item;
                                    item = new RadComboBoxItem();
                                    item.Value = fileName.Replace(ftpRemotePath + "/", string.Empty);
                                    item.Text = fileName.Replace(ftpRemotePath + "/", string.Empty);

                                    this.RadComboBox2.Items.Add(item);
                                }
                            }
                        }
                    }
                    else
                    {
                        this.Label7.Text = "SFTP authentication failed with UID and PWD";
                    }
                }
                else
                {
                    this.Label7.Text = "Failed to connect server " + FtpSrvNm;
                }

                clientFtp.Disconnect();
            }
        }

        protected void DeleteFile()
        {
            this.Label7.Text = string.Empty;

            using (Sftp clientFtp = new Sftp())
            {
                string ftpRemotePath = string.Empty;
                string ftpRemoteFile = string.Empty;

                clientFtp.Connect(FtpSrvNm, 22);

                if (clientFtp.IsConnected)
                {
                    ////Authenticate FTP server
                    clientFtp.Authenticate(FtpUsr, FtpPwd);

                    if (clientFtp.IsAuthenticated)
                    {
                        clientFtp.SetCurrentDirectory("/" + FtpSrvType);

                        ftpRemotePath = this.RadComboBox1.SelectedValue;

                        ftpRemoteFile = ftpRemotePath + "/" + this.RadComboBox2.SelectedValue;

                        if (clientFtp.DirectoryExists(ftpRemotePath))
                        {
                            if (clientFtp.FileExists(ftpRemoteFile))
                            {
                                clientFtp.DeleteFile(ftpRemoteFile);
                            }
                            ListFile();
                        }
                    }
                    else
                    {
                        this.Label7.Text = "SFTP authentication failed with UID and PWD";
                    }
                }
                else
                {
                    this.Label7.Text = "Failed to connect server " + FtpSrvNm;
                }

                clientFtp.Disconnect();
            }
        }

        protected void DownloadFile()
        {
            try
            {
                this.Label7.Text = string.Empty;

                using (Sftp clientFtp = new Sftp())
                {

                    string ftpRemotePath = string.Empty;
                    string ftpRemoteFile = string.Empty;

                    clientFtp.Connect(FtpSrvNm, 22);

                    if (clientFtp.IsConnected)
                    {
                        ////Authenticate FTP server
                        clientFtp.Authenticate(FtpUsr, FtpPwd);

                        if (clientFtp.IsAuthenticated)
                        {
                            clientFtp.SetCurrentDirectory("/" + FtpSrvType);

                            ftpRemotePath = this.RadComboBox1.SelectedValue;

                            ftpRemoteFile = ftpRemotePath + "/" + this.RadComboBox2.SelectedValue;

                            if (clientFtp.DirectoryExists(ftpRemotePath))
                            {
                                if (clientFtp.FileExists(ftpRemoteFile))
                                {

                                    Stream fileStream = clientFtp.GetDownloadStream(ftpRemoteFile);
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        fileStream.CopyTo(ms);
                                        Response.AddHeader("content-disposition", "attachment;filename=Hai.pdf");
                                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                        Response.BinaryWrite(ms.ToArray());
                                        Response.End();
                                    }

                                }

                            }
                        }
                        else
                        {
                            this.Label7.Text = "SFTP authentication failed with UID and PWD";
                        }
                    }
                    else
                    {
                        this.Label7.Text = "Failed to connect server " + FtpSrvNm;
                    }

                    clientFtp.Disconnect();

                    HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                    HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                    HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event
                }
            }
            catch (Exception ex)
            {
                LogException(ex);

                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + ex.Message + "');", addScriptTags: true);
            }
            finally
            {
                //Response.End();

                HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event
            }
        }


        //void client_DownloadFileCompleted(object sender, AsyncMethodCompletedEventArgs e)
        //{
        //    Ftp client = (Ftp)sender;
        //    try
        //    {
        //        long transferred = client.EndDownloadFile(e.AsyncResult);
        //        Console.WriteLine("Bytes transferred: " + transferred);
        //    }
        //    catch (Exception exc)
        //    {
        //        Console.WriteLine("Error: " + exc.ToString());
        //    }
        //}


        protected void RadButton2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.RadTextBox1.Text.Trim()))
            {
                CreateFolder(this.RadTextBox1.Text.Trim());
            }
            ListFolder();
        }

        protected void RadButton3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.RadTextBox2.Text.Trim()))
            {
                DeleteFolder(this.RadTextBox2.Text.Trim());
            }

            ListFolder();
        }

        protected void RadButton4_Click(object sender, EventArgs e)
        {
            ListFolder();
        }

        protected void BtnDummy1_Click(object sender, EventArgs e)
        {
            this.AsyncUpload1.Enabled = true;
        }

        protected void AsyncUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            try
            {

                string strFileName = e.File.FileName.ToString();
                string strExtension = e.File.GetExtension().ToString().Replace(oldValue: ".", newValue: "").ToLower();
                string strFileType = e.File.ContentType.ToString();

                using (Sftp clientFtp = new Sftp())
                {
                    string ftpRemotePath = string.Empty;

                    //clientFtp.Connect(FtpSrvNm, 21, (SecurityMode)0);
                    clientFtp.Connect(FtpSrvNm, 22);

                    if (clientFtp.IsConnected)
                    {
                        ////Authenticate FTP server
                        clientFtp.Authenticate(FtpUsr, FtpPwd);

                        if (clientFtp.IsAuthenticated)
                        {
                            clientFtp.SetCurrentDirectory("/" + FtpSrvType);

                            //ftpRemotePath = "/" + this.RadComboBox1.SelectedValue;
                            ftpRemotePath = this.RadComboBox1.SelectedValue;

                            using (Stream fileStream = e.File.InputStream)
                            {
                                clientFtp.UploadFile(fileStream, ftpRemotePath + "/" + strFileName);
                            }

                            if (clientFtp.FileExists(ftpRemotePath + "/" + strFileName))
                            {
                                this.Label6.Text = "Upload successfully";
                                ListFile();
                            }
                            else
                            {
                                this.Label6.Text = "Unable to locate the uploaded file";
                            }
                        }
                        else
                        {
                            this.Label6.Text = "SFTP authentication failed with UID and PWD";
                        }
                    }
                    else
                    {
                        this.Label6.Text = "Failed to connect server " + FtpSrvNm;
                    }

                    clientFtp.Disconnect();
                }
            }
            catch (Exception ex)
            {
                LogException(ex);

                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            }
            finally
            {
                ListFile();
            }
        }

        protected void RadButton5_Click(object sender, EventArgs e)
        {
            ListFile();
        }

        protected void RadComboBox1_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ListFile();
        }

        protected void RadButton6_Click(object sender, EventArgs e)
        {
            DeleteFile();
        }

        protected void RadButton7_Click(object sender, EventArgs e)
        {
            DownloadFile();
        }


        private void LogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog();

            appLog.ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionSource = "TestFTP.aspx";
            appLog.ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionUrl = string.Empty;
            appLog.ExceptionTargetSite = ex.TargetSite.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionStackTrace = ex.StackTrace.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.AppLogEvent();
        }
    }
}