using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class DataSourceHelperCS
{
    public string VendorBarcodeConStr = ConfigurationManager.ConnectionStrings["dbVendorBarcode"].ConnectionString;
    public string RfaConStr = ConfigurationManager.ConnectionStrings["dbConnRFA"].ConnectionString;
    public string DataMartConStr = ConfigurationManager.ConnectionStrings["dbConnDataMart"].ConnectionString;
    public string ErrException = "An error has occured...If problem persists, please contact the administrator: " + ConfigurationManager.AppSettings["AdminContact"];
    public SqlDataAdapter Sda = new SqlDataAdapter();

    public string USR_ID { set; get; }
    public string FILE_SVR { set; get; }
    public string PLNT_CD { set; get; }
    public string UPLOAD_DIR { set; get; }
    public string URL { set; get; }

    public DataTable GetData(SqlCommand cmd)
    {
        DataTable dt;
        dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(VendorBarcodeConStr))
        {
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            cmd.Connection = conn;
            SqlDataAdapter sAdapt;
            sAdapt = new SqlDataAdapter();

            try
            {
                conn.Open();

                sAdapt.SelectCommand = cmd;
                sAdapt.Fill(dt);

                cmd.Dispose();
                return dt;

            }
            catch (SqlException sqlex)
            {
                LogException(sqlex);

                throw new Exception(ErrException);
            }
        }
    }

    public DataTable GetDataQDMS(string dbConStr)
    {
        using (SqlConnection conn = new SqlConnection(dbConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "eCert.HSP_GET_ECERT_USER_DATA";
            cmd.Parameters.Add(parameterName: "@USR_ID", sqlDbType: SqlDbType.VarChar, size: 20).Value = USR_ID;
            cmd.Parameters.Add(parameterName: "@PLNT_CD", sqlDbType: SqlDbType.VarChar, size: 4).Value = PLNT_CD;
            cmd.Parameters.Add(parameterName: "@FILE_SVR", sqlDbType: SqlDbType.VarChar, size: 150).Value = FILE_SVR;
            cmd.Parameters.Add(parameterName: "@UPLOAD_DIR", sqlDbType: SqlDbType.VarChar, size: 200).Value = UPLOAD_DIR;
            cmd.Parameters.Add(parameterName: "@URL", sqlDbType: SqlDbType.VarChar, size: 200).Value = URL;
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

    public DataTable GetDataRemote(SqlCommand cmd, string dbConStr)
    {
        DataTable dt;
        dt = new DataTable();
        using (SqlConnection dBConn = new SqlConnection(dbConStr))
        {
            cmd.CommandType = CommandType.Text;
            cmd.Connection = dBConn;
            cmd.CommandTimeout = 0;
            SqlDataAdapter sAdapt;
            sAdapt = new SqlDataAdapter();

            try
            {
                dBConn.Open();

                sAdapt.SelectCommand = cmd;
                sAdapt.Fill(dt);

                return dt;
            }
            catch (SqlException sqlex)
            {
                LogException(sqlex);

                throw new Exception(ErrException);
            }
        }
    }

    public DataTable GetRfaDataTable(SqlCommand cmd)
    {
        DataTable dt;
        dt = new DataTable();
        using (SqlConnection dBConn = new SqlConnection(RfaConStr))
        {
            cmd.CommandType = CommandType.Text;
            cmd.Connection = dBConn;
            cmd.CommandTimeout = 0;
            SqlDataAdapter sAdapt;
            sAdapt = new SqlDataAdapter();

            try
            {
                dBConn.Open();

                sAdapt.SelectCommand = cmd;
                sAdapt.Fill(dt);

                return dt;
            }
            catch (SqlException sqlex)
            {
                LogException(sqlex);

                throw new Exception(ErrException);
            }
        }
    }

    public DataTable GetDataMartDataTable(SqlCommand cmd)
    {
        DataTable dt;
        dt = new DataTable();
        using (SqlConnection dBConn = new SqlConnection(DataMartConStr))
        {
            cmd.CommandType = CommandType.Text;
            cmd.Connection = dBConn;
            cmd.CommandTimeout = 0;
            SqlDataAdapter sAdapt;
            sAdapt = new SqlDataAdapter();

            try
            {
                dBConn.Open();

                sAdapt.SelectCommand = cmd;
                sAdapt.Fill(dt);

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
        string strSource = "Class_DataSourceHelperCS";
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