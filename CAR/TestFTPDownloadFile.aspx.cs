using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComponentPro.Net;
using ComponentPro;
using Telerik.Web.UI;
using System.IO;
using iTextSharp.text.pdf;

namespace CAR
{
    public partial class TestFTPDownloadFile : System.Web.UI.Page
    {

        public string FtpSrvNm = ConfigurationManager.AppSettings["FtpSrvNm"].ToString();
        //public string FtpSrvDir = ConfigurationManager.AppSettings["FtpSrvDir"].ToString();
        //public string FtpSrvSharedNm = ConfigurationManager.AppSettings["FtpSrvSharedNm"].ToString();

        public string FtpU = ConfigurationManager.AppSettings["FtpU"].ToString();
        public string FtpP = ConfigurationManager.AppSettings["FtpP"].ToString();

        public string FtpSrvTypeQdms = ConfigurationManager.AppSettings["FtpSrvTypeQdms"].ToString();


        public string FtpSrvDirQdms = ConfigurationManager.AppSettings["FtpSrvDirQdms"].ToString();
        public string FtpSrvSharedNmQdms = ConfigurationManager.AppSettings["FtpSrvSharedNmQdms"].ToString();
        public string FtpUQdms = ConfigurationManager.AppSettings["FtpUQdms"].ToString();
        public string FtpPQdms = ConfigurationManager.AppSettings["FtpPQdms"].ToString();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.Label5.Text = "\\" + FtpSrvNm + "\\" + FtpSrvSharedNmQdms + "\\" + FtpSrvDirQdms + "\\" + FtpSrvTypeQdms;
                CreateRootFolder();
                ListFolder();
                ListFile();
            }
        }


        protected void RadButton1_Click(object sender, EventArgs e)
        {
            this.Label1.Text = string.Empty;

            using (Ftp clientFtp = new Ftp())
            {
                string ftpRemotePath = string.Empty;

                clientFtp.Connect(FtpSrvNm, 2122);

                if (clientFtp.IsConnected)
                {
                    ////Authenticate FTP server
                    clientFtp.Authenticate(FtpUQdms, FtpPQdms);

                    if (clientFtp.IsAuthenticated)
                    {
                        this.Label1.Text = "FTP Connected and Authenticate successfully";
                    }
                    else
                    {
                        this.Label1.Text = "FTP authentication failed with UID and PWD";
                    }
                }
                else
                {
                    this.Label1.Text = "Failed to connect server " + FtpSrvNm;
                }

                clientFtp.Disconnect();
            }
        }


        protected void RadButton2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.RadTextBox1.Text.Trim()))
            {
                CreateFolder(this.RadTextBox1.Text.Trim());
            }
            ListFolder();
        }

        protected void RadButton4_Click(object sender, EventArgs e)
        {
            ListFolder();
        }


        protected void RadButton5_Click(object sender, EventArgs e)
        {
            ListFile();
        }

        protected void RadButton3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.RadTextBox2.Text.Trim()))
            {
                DeleteFolder(this.RadTextBox2.Text.Trim());
            }

            ListFolder();
        }


        protected void RadComboBox1_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ListFile();
        }


        protected void DeleteFolder(string folderName)
        {
            this.Label3.Text = string.Empty;

            using (Ftp clientFtp = new Ftp())
            {
                string ftpRemotePath = string.Empty;

                clientFtp.Connect(FtpSrvNm, 2122);

                if (clientFtp.IsConnected)
                {
                    ////Authenticate FTP server
                    clientFtp.Authenticate(FtpUQdms, FtpPQdms);

                    if (clientFtp.IsAuthenticated)
                    {
                        ftpRemotePath = "/" + FtpSrvTypeQdms + "/" + folderName;

                        if (clientFtp.DirectoryExists(ftpRemotePath))
                        {
                            clientFtp.DeleteDirectory(ftpRemotePath, true);
                            this.Label3.Text = "Deleted...";
                            ListFile();
                        }
                    }
                    else
                    {
                        this.Label3.Text = "FTP authentication failed with UID and PWD";
                    }
                }
                else
                {
                    this.Label3.Text = "Failed to connect server " + FtpSrvNm;
                }

                clientFtp.Disconnect();
            }
        }

        protected void CreateFolder(string folderName)
        {
            CreateRootFolder();

            this.Label2.Text = string.Empty;

            using (Ftp clientFtp = new Ftp())
            {
                string ftpRemotePath = string.Empty;

                clientFtp.Connect(FtpSrvNm, 2122);

                if (clientFtp.IsConnected)
                {
                    ////Authenticate FTP server
                    clientFtp.Authenticate(FtpUQdms, FtpPQdms);

                    if (clientFtp.IsAuthenticated)
                    {
                        ftpRemotePath = "/" + FtpSrvTypeQdms + "/" + folderName;

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


        protected void CreateRootFolder()
        {
            this.Label2.Text = string.Empty;

            using (Ftp clientFtp = new Ftp())
            {
                string ftpRemotePath = string.Empty;

                clientFtp.Connect(FtpSrvNm, 2122);

                if (clientFtp.IsConnected)
                {
                    ////Authenticate FTP server
                    clientFtp.Authenticate(FtpUQdms, FtpPQdms);

                    if (clientFtp.IsAuthenticated)
                    {

                        ftpRemotePath = "/" + FtpSrvTypeQdms;

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

        protected void ListFolder()
        {
            this.Label4.Text = string.Empty;

            using (Ftp clientFtp = new Ftp())
            {
                string ftpRemotePath = string.Empty;

                clientFtp.Connect(FtpSrvNm, 2122);

                if (clientFtp.IsConnected)
                {
                    ////Authenticate FTP server
                    clientFtp.Authenticate(FtpUQdms, FtpPQdms);

                    if (clientFtp.IsAuthenticated)
                    {
                        this.RadComboBox1.Text = string.Empty;
                        this.RadComboBox1.Items.Clear();

                        //ftpRemotePath = "/NEWFILES";
                        //clientFtp.SetCurrentDirectory(ftpRemotePath);

                        FtpFileInfoCollection files = (FtpFileInfoCollection)clientFtp.ListDirectory();

                        foreach (FtpFileInfo ftpFile in files)
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
                        this.Label4.Text = "FTP authentication failed with UID and PWD";
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

            using (Ftp clientFtp = new Ftp())
            {
                string ftpRemotePath = string.Empty;

                clientFtp.Connect(FtpSrvNm, 2122);

                if (clientFtp.IsConnected)
                {
                    ////Authenticate FTP server
                    clientFtp.Authenticate(FtpUQdms, FtpPQdms);

                    if (clientFtp.IsAuthenticated)
                    {
                        if (this.RadComboBox1.Items.Count > 0)
                        {
                            this.RadComboBox2.Text = string.Empty;
                            this.RadComboBox2.Items.Clear();

                            //clientFtp.SetCurrentDirectory("/" + FtpSrvTypeQdms);
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
                        this.Label7.Text = "FTP authentication failed with UID and PWD";
                    }
                }
                else
                {
                    this.Label7.Text = "Failed to connect server " + FtpSrvNm;
                }

                clientFtp.Disconnect();
            }
        }

        protected void RadButton6_Click(object sender, EventArgs e)
        {
            DeleteFile();
        }



        protected void DeleteFile()
        {
            this.Label7.Text = string.Empty;

            using (Ftp clientFtp = new Ftp())
            {
                string ftpRemotePath = string.Empty;
                string ftpRemoteFile = string.Empty;

                clientFtp.Connect(FtpSrvNm, 2122);

                if (clientFtp.IsConnected)
                {
                    ////Authenticate FTP server
                    clientFtp.Authenticate(FtpUQdms, FtpPQdms);

                    if (clientFtp.IsAuthenticated)
                    {
                        //clientFtp.SetCurrentDirectory("/" + FtpSrvTypeQdms);

                        ftpRemotePath = this.RadComboBox1.SelectedValue;

                        ftpRemoteFile = ftpRemotePath + "/" + this.RadComboBox2.SelectedValue;

                        if (clientFtp.DirectoryExists(ftpRemotePath))
                        {
                            if (clientFtp.FileExists(ftpRemoteFile))
                            {
                                clientFtp.DeleteFile(ftpRemoteFile);

                                this.Label7.Text = "Deleted...";
                            }
                            ListFile();
                        }
                    }
                    else
                    {
                        this.Label7.Text = "FTP authentication failed with UID and PWD";
                    }
                }
                else
                {
                    this.Label7.Text = "Failed to connect server " + FtpSrvNm;
                }

                clientFtp.Disconnect();
            }
        }

        //protected void CopyToAnoterLocation()
        //{
        //    try
        //    {
        //        this.Label7.Text = string.Empty;

        //        using (Sftp clientFtp = new Sftp())
        //        {

        //            string ftpRemotePath = string.Empty;
        //            string ftpRemoteFile = string.Empty;
        //            string ftpRemoteQdmsPath = string.Empty;

        //            clientFtp.Connect(FtpSrvNm, 22);

        //            if (clientFtp.IsConnected)
        //            {
        //                ////Authenticate FTP server
        //                clientFtp.Authenticate(FtpU, FtpP);

        //                if (clientFtp.IsAuthenticated)
        //                {
        //                    ftpRemotePath = "/TEST_CAR";

        //                    //ftpRemoteFile = ftpRemotePath + "/" + "Binder1.pdf";

        //                    ftpRemoteFile = FtpSrvTypeQdms + "/2019/2019-00001/20190816102555.pdf";
        //                    ftpRemoteQdmsPath = FtpSrvTypeQdms + "/2019/2019-00001/QDMS";


        //                    //// ******************** Remove this when done
        //                    ///
        //                    if (clientFtp.DirectoryExists(ftpRemoteQdmsPath))
        //                    {
        //                        clientFtp.DeleteDirectory(ftpRemoteQdmsPath, true);
        //                    }
        //                    //// ******************** 


        //                    //// ** If not exist, create QDMS Folder
        //                    if (!clientFtp.DirectoryExists(ftpRemoteQdmsPath))
        //                    {
        //                        clientFtp.CreateDirectory(ftpRemoteQdmsPath);
        //                    }

        //                    if (clientFtp.DirectoryExists(ftpRemotePath))
        //                    {
        //                        if (clientFtp.FileExists(ftpRemoteFile))
        //                        {

        //                            using (MemoryStream ms = new MemoryStream())
        //                            {
        //                                clientFtp.DownloadFile(ftpRemoteFile, ms);
        //                                //ms.Flush(); //Always catches me out  
        //                                ms.Position = 0;

        //                                using (Stream fileStream = ms)
        //                                {

        //                                    //// *** validate pdf pages
        //                                    byte[] fileBinaryData = new byte[1024 * 100];
        //                                    BinaryReader br = new BinaryReader(fileStream);
        //                                    fileBinaryData = br.ReadBytes((Int32)fileStream.Length);

        //                                    PdfReader reader;
        //                                    reader = new PdfReader(fileBinaryData);

        //                                    if (reader.NumberOfPages > 0)
        //                                    {
        //                                        using (MemoryStream msNew = new MemoryStream(fileBinaryData))
        //                                        {
        //                                            clientFtp.UploadFile(msNew, ftpRemoteQdmsPath + "/Hai_TEST_" + ms.Length.ToString() + ".pdf");
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    this.Label7.Text = "FTP authentication failed with UID and PWD";
        //                }
        //            }
        //            else
        //            {
        //                this.Label7.Text = "Failed to connect server " + FtpSrvNm;
        //            }

        //            clientFtp.Disconnect();


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogException(ex);

        //        ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + ex.Message + "');", addScriptTags: true);
        //    }
        //   finally
        //    {
        //        this.Label7.Text = "Copied to another location";
        //    }
        //}

        //protected void DownloadFile()
        //{
        //    try
        //    {
        //        this.Label7.Text = string.Empty;

        //        using (Sftp clientFtp = new Sftp())
        //        {

        //            string ftpRemotePath = string.Empty;
        //            string ftpRemoteFile = string.Empty;
        //            string ftpRemoteQdmsPath = string.Empty;


        //            clientFtp.Connect(FtpSrvNm, 22);

        //            if (clientFtp.IsConnected)
        //            {
        //                ////Authenticate FTP server
        //                clientFtp.Authenticate(FtpU, FtpP);

        //                if (clientFtp.IsAuthenticated)
        //                {
        //                    ftpRemotePath = "/TEST_CAR";

        //                    //ftpRemoteFile = ftpRemotePath + "/" + "Binder1.pdf";

        //                    ftpRemoteFile = FtpSrvTypeQdms + "/2019/2019-00001/20190816102555.pdf";
        //                    ftpRemoteQdmsPath = FtpSrvTypeQdms + "/2019/2019-00001/QDMS";


        //                    if (clientFtp.DirectoryExists(ftpRemotePath))
        //                    {
        //                        if (clientFtp.FileExists(ftpRemoteFile))
        //                        {

        //                            SftpFileInfo info = (SftpFileInfo)clientFtp.GetItemInfo(ftpRemoteFile);


        //                            string fileSize = info.Length.ToString();
        //                            string fileName = info.Name.ToString();

        //                            Stream fileStream = clientFtp.GetDownloadStream(ftpRemoteFile);

        //                            using (MemoryStream ms = new MemoryStream())
        //                            {
        //                                fileStream.CopyTo(ms);

        //                                Response.Clear();
        //                                Response.ClearHeaders();
        //                                Response.AddHeader("Content-Type", "Application/octet-stream");
        //                                Response.AddHeader("Content-Length", fileSize);
        //                                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
        //                                Response.AddHeader("Connection", "Keep-Alive");
        //                                Response.BinaryWrite(ms.ToArray());
        //                                Response.Flush();
        //                                Response.End();

        //                            }
        //                                ////byte[] buf = new byte[fileStream.Length];  //declare arraysize
        //                                ////fileStream.Read(buf, 0, buf.Length);

        //                                ////Byte[] bytes = (Byte[])buf;

        //                                ////Response.Buffer = true;
        //                                ////Response.Clear();
        //                                ////Response.ClearHeaders();
        //                                ////Response.BufferOutput = true;
        //                                ////Response.AppendHeader("Content-Disposition", "attachment;filename=hai.pdf");
        //                                ////Response.AppendHeader("Content-Length", bytes.Length.ToString());
        //                                ////Response.ContentType = "application/pdf";
        //                                ////Response.BinaryWrite(bytes);

        //                                //byte[] buffer = new byte[32768];
        //                                //using (MemoryStream ms = new MemoryStream())
        //                                //{
        //                                //    while (true)
        //                                //    {
        //                                //        int read = fileStream.Read(buffer, 0, buffer.Length);
        //                                //        if (read >= 0)
        //                                //        {
        //                                //            ms.Write(buffer, 0, read);
        //                                //        }                                            
        //                                //    }

        //                                //    Response.Buffer = true;
        //                                //    Response.Charset = string.Empty;
        //                                //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //                                //    Response.ContentType = "application/pdf";
        //                                //    Response.AddHeader(name: "content-disposition", value: "attachment;filename=hai.pdf");

        //                                //    Response.BinaryWrite(buffer);
        //                                //}




        //                                //byte[] buffer = new byte[1024];
        //                                //long byteCount;

        //                                //while ((byteCount = fileStream.Read(buffer, 0, buffer.Length)) > 0)
        //                                //{
        //                                //    if (Context.Response.IsClientConnected)
        //                                //    {
        //                                //        Context.Response.ContentType = "application/pdf";
        //                                //        Context.Response.AddHeader(name: "content-disposition", value: "attachment;filename=" + "Hai.pdf");
        //                                //        Context.Response.OutputStream.Write(buffer, 0, buffer.Length);
        //                                //        Context.Response.Flush();
        //                                //    }
        //                                //}
        //                            }
        //                        }
        //                }
        //                else
        //                {
        //                    this.Label7.Text = "FTP authentication failed with UID and PWD";
        //                }
        //            }
        //            else
        //            {
        //                this.Label7.Text = "Failed to connect server " + FtpSrvNm;
        //            }

        //            clientFtp.Disconnect();

        //            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
        //            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
        //            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogException(ex);

        //        ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + ex.Message + "');", addScriptTags: true);
        //    }
        //    finally
        //    {
        //        //Response.End();

        //        HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
        //        HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
        //        HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event
        //    }
        //}


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