using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
public class GetFtp
{

    public SqlDataAdapter Sda = new SqlDataAdapter();
    public string ConStr = ConfigurationManager.ConnectionStrings["dbQDMSLocal"].ConnectionString;
    public string ErrException = ConfigurationManager.AppSettings["ErrorMessage"] + ConfigurationManager.AppSettings["AdminContact"];


    public DataTable Credential()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[FTP_XFER].[HSP_FTP_XFER_INFO]";
            cmd.Parameters.Add(parameterName: "@REMOTE_FLG", sqlDbType: SqlDbType.Int).Value = 0;
            cmd.Connection = conn;
            cmd.CommandTimeout = 0;

            cmd.Connection = conn;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();

                DataTable dt;
                dt = new DataTable();

                Sda.SelectCommand = cmd;
                Sda.Fill(dt);
                return dt;
            }
            catch (SqlException sqlex)
            {
                LogException(sqlex);

                throw new Exception(ErrException);
            }
        }
    }


    // Helper routine that logs SqlException details to the
    // Application event log
    private void LogException(SqlException sqlex)
    {
        string strSource = "AppLog_Class_GetFTP";
        string strMessageAll;
        strMessageAll = "Exception Number : " + sqlex.Number + "(" + sqlex.Message + ") has occurred";


        SqlLog logIt;
        logIt = new SqlLog();

        logIt.MessageAll = strMessageAll.ToString();
        foreach (SqlError sqle in sqlex.Errors)
        {
            logIt.Message = sqle.Message.ToString();
            logIt.Number = sqle.Number.ToString();
            logIt.Procedure = sqle.Procedure.ToString();
            logIt.Server = sqle.Server.ToString();
            logIt.Source = strSource;
            logIt.State = sqle.State.ToString();
            logIt.Serverity = sqle.Class.ToString();
            logIt.LineNumber = sqle.LineNumber.ToString();
        }

        logIt.LogSqlEvent();
    }
}