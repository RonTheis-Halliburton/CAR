using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class QdmsUtil
{


    public string[] GetImageSysServ(string userId)
    {
        //string dbString = string.Empty;
        string[] arraySrv;
        arraySrv = new string[0];


        DataSourceHelperCS getSource;
        getSource = new DataSourceHelperCS();

        using (SqlCommand myCommand = new SqlCommand())
        {
            myCommand.CommandText = "Select Distinct(DataSource) as DataSource, InitialCatalog, [UID], PWD, PlantDataSource, PlantNameDataSource, URL, QDMS, FILE_SVR, CoverPage, AllowDeleteFile From tblImageSystemDB Order By PlantDataSource Desc";
            using (DataTable dt = getSource.GetData(myCommand))
            {
                arraySrv = new string[dt.Rows.Count];

                int n = 0;
                try
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        SqlConnectionStringBuilder oCsbd;
                        oCsbd = new SqlConnectionStringBuilder();
                        oCsbd.ConnectTimeout = 5;

                        if (row["QDMS"].ToString() == "1")
                        {
                            string ftpUpload = string.Empty;

                            GetFtp ftpSrv;
                            ftpSrv = new GetFtp();
                            using (DataTable dtSrv = ftpSrv.Credential())
                            {
                                foreach (DataRow rowSrv in dtSrv.Rows)
                                {
                                    ftpUpload = rowSrv["Upload_DIR"].ToString();
                                }
                            }

                            var dbQdms = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["dbQDMS"].ConnectionString);

                            oCsbd.DataSource = dbQdms.DataSource.ToString();
                            oCsbd.InitialCatalog = dbQdms.InitialCatalog.ToString();
                            oCsbd.Password = dbQdms.Password.ToString();
                            oCsbd.UserID = dbQdms.UserID.ToString();

                            if (TryOpenConnection(oCsbd.ConnectionString))
                            {

                                getSource.USR_ID = userId;
                                getSource.PLNT_CD = row["PlantDataSource"].ToString();
                                getSource.FILE_SVR = row["FILE_SVR"].ToString();
                                getSource.UPLOAD_DIR = ftpUpload;
                                getSource.URL = row["URL"].ToString();

                                using (DataTable dtA = getSource.GetDataQDMS(oCsbd.ConnectionString))
                                {
                                    if (dtA.Rows.Count > 0)
                                    {
                                        foreach (DataRow rowA in dtA.Rows)
                                        {
                                            arraySrv[n] = oCsbd.DataSource.ToString() + "|" + row["PlantDataSource"].ToString() + " - " + row["PlantNameDataSource"].ToString() + "|" + oCsbd.ConnectionString.ToString() + "|" + rowA["UPLOAD_DIR"].ToString() + "|" + rowA["FILE_SVR"].ToString().Replace(oldValue: "\\", newValue: "") + "|0|" + string.Empty + "|" + string.Empty + "|" + string.Empty + "|" + string.Empty + "|" + rowA["ADMIN"].ToString() + "|" + rowA["URL"].ToString() + "|" + row["PlantDataSource"].ToString() + "|" + row["QDMS"].ToString() + "|QDMS|" + row["CoverPage"].ToString() + "|" + row["AllowDeleteFile"].ToString() + "|" + row["PlantNameDataSource"].ToString();
                                            n++;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            arraySrv[0] = string.Empty;
                        }
                    }


                    List<string> list;
                    list = new List<string>();
                    list.AddRange(arraySrv);

                    for (int iCount = list.Count - 1; iCount >= 0; iCount--)
                    {
                        if (null == list[iCount])
                        {
                            list.RemoveAt(iCount);
                        }
                    }

                    arraySrv = list.ToArray();
                }
                catch (Exception ex)
                {
                    LogException(ex);
                }
            }
        }

        return arraySrv;
    }


    public bool TryOpenConnection(string connectionString)
    {
        try
        {
            var conn = new SqlConnection(connectionString);
            conn.Open();

            return true;
        }
        catch (SqlException exception)
        {
            return false;
            throw new ArgumentException("Function TryOpenConnection:  " + exception.Message);
        }
    }

    private void LogException(Exception ex)
    {
        Applog appLog;
        appLog = new Applog();

        appLog.ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`");
        appLog.ExceptionSource = "QdmsUtil.cs";
        appLog.ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`");
        appLog.ExceptionUrl = "";
        appLog.ExceptionTargetSite = ex.TargetSite.ToString().Replace(oldValue: "'", newValue: "`");
        appLog.ExceptionStackTrace = ex.StackTrace.ToString().Replace(oldValue: "'", newValue: "`");
        appLog.AppLogEvent();
    }
}