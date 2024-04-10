using System.Configuration;
using System.Data;
using System.Data.SqlClient;


public class Maintenance
{
    public string Portal { set; get; }
    public string Status { set; get; }
    public string OfflineComments { set; get; }
    public string Notification { set; get; }
    public string NotificationFlag { set; get; }
    public string NotificationPosition { set; get; }
    public string NotificationAutoCloseDelay { set; get; }
    public string NotificationShowInterval { set; get; }

    public SqlDataAdapter Sda = new SqlDataAdapter();
    public string ConStr = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
    public string ErrException = ConfigurationManager.AppSettings["ErrorMessage"] + ConfigurationManager.AppSettings["AdminContact"];

    public string Exec_UpdatePortal()
    {
        string uReturn = string.Empty;

        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[SP_UPDATE_ON_OFFLINE]";
            cmd.Parameters.Add(parameterName: "@Portal", sqlDbType: SqlDbType.Int, size: 0).Value = Portal;
            cmd.Parameters.Add(parameterName: "@Status", sqlDbType: SqlDbType.Int, size: 0).Value = Status;
            cmd.Parameters.Add(parameterName: "@OfflineComments", sqlDbType: SqlDbType.VarChar, size: -1).Value = OfflineComments;
            cmd.Parameters.Add(parameterName: "@Notification", sqlDbType: SqlDbType.VarChar, size: -1).Value = Notification;
            cmd.Parameters.Add(parameterName: "@NotificationFlag", sqlDbType: SqlDbType.Int, size: 0).Value = NotificationFlag;
            cmd.Parameters.Add(parameterName: "@NotificationAutoCloseDelay", sqlDbType: SqlDbType.Int, size: 0).Value = NotificationAutoCloseDelay;
            cmd.Parameters.Add(parameterName: "@NotificationShowInterval", sqlDbType: SqlDbType.Int, size: 0).Value = NotificationShowInterval;
            cmd.Parameters.Add(parameterName: "@NotificationPosition", sqlDbType: SqlDbType.VarChar, size: 150).Value = NotificationPosition;

            SqlParameter paramResult;
            paramResult = new SqlParameter(parameterName: "@Result", dbType: SqlDbType.VarChar, size: 100);
            paramResult.Value = string.Empty;
            paramResult.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramResult);

            cmd.Connection = conn;
            try
            {
                conn.Open();
                SqlDataReader myReader = cmd.ExecuteReader();
                uReturn = paramResult.Value.ToString();

                myReader.Dispose();
            }
            catch (SqlException sqlex)
            {
                LogException(sqlex);

                uReturn = ErrException + "..." + sqlex.Message.ToString();
            }
            finally
            {
                cmd.Dispose();
            }
        }

        return uReturn;
    }

    // Helper routine that logs SqlException details to the
    // Application event log
    private void LogException(SqlException sqlex)
    {
        string strSource = "Internal_Class_Maintenance";
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