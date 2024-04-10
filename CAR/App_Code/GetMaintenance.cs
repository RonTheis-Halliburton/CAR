using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


public class GetMaintenance
{

    public SqlDataAdapter Sda = new SqlDataAdapter();
    public string ConStr = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
    public string ErrException = ConfigurationManager.AppSettings["ErrorMessage"] + ConfigurationManager.AppSettings["AdminContact"];

    public DataTable Exec_GetMaintenanceExternal()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_MAINTENANCE_EXTERNAL";
            cmd.Connection = conn;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();

                DataTable dt;
                dt = new DataTable();

                Sda.SelectCommand = cmd;
                Sda.Fill(dt);

                conn.Close();
                return dt;
            }
            catch (SqlException sqlex)
            {
                LogException(sqlex);

                throw new Exception(ErrException);
            }
        }
    }


    public DataTable Exec_GetMaintenanceInternal()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_MAINTENANCE_INTERNAL";
            cmd.Connection = conn;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();

                DataTable dt;
                dt = new DataTable();

                Sda.SelectCommand = cmd;
                Sda.Fill(dt);

                conn.Close();
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
        string strSource = "Internal_Class_GetMaintenance";
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