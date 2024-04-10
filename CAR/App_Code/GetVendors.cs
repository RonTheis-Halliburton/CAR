using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


public class GetVendors
{
    public DataTable Dt = new DataTable();
    public SqlDataAdapter Sda = new SqlDataAdapter();
    public string ConStr = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
    public string RfaConStr = ConfigurationManager.ConnectionStrings["dbConnRFA"].ConnectionString;
    public string ErrException = ConfigurationManager.AppSettings["ErrorMessage"] + ConfigurationManager.AppSettings["AdminContact"];

    public string SearchText { get; set; }
    public string VendorNumber { get; set; }
    public string UserId { get; set; }
    public string Active { get; set; }

    //public DataTable GetVendorList()
    //{
    //    using (SqlConnection conn = new SqlConnection(RfaConStr))
    //    {
    //        SqlCommand cmd;
    //        cmd = new SqlCommand();
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.CommandText = "[spGETVENDOR]";
    //        cmd.Parameters.Add(parameterName: "@Text", sqlDbType: SqlDbType.VarChar, size: -1).Value = SearchText;
    //        cmd.Connection = conn;

    //        try
    //        {
    //            conn.Open();
    //            Sda.SelectCommand = cmd;
    //            Sda.Fill(Dt);

    //            conn.Close();
    //            return Dt;
    //        }
    //        catch (SqlException sqlex)
    //        {
    //            LogException(sqlex);

    //            throw new Exception("Error Processing Request:  " + sqlex.Message.Replace(oldValue: "'", newValue: "`") + ".  " + ErrException);
    //        }
    //        finally
    //        {
    //            Sda.Dispose();
    //            cmd.Dispose();
    //        }
    //    }
    //}

    public DataTable GetVendorList()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[sp_GET_VNDR]";
            cmd.Parameters.Add(parameterName: "@VendorNumber", sqlDbType: SqlDbType.VarChar, size: 15).Value = SearchText;
            cmd.Connection = conn;

            try
            {
                conn.Open();
                Sda.SelectCommand = cmd;
                Sda.Fill(Dt);

                conn.Close();
                return Dt;
            }
            catch (SqlException sqlex)
            {
                LogException(sqlex);

                throw new Exception("Error Processing Request:  " + sqlex.Message.Replace(oldValue: "'", newValue: "`") + ".  " + ErrException);
            }
            finally
            {
                Sda.Dispose();
                cmd.Dispose();
            }
        }
    }


    public DataTable GetVendorUserList()
    {
        using (SqlConnection conn = new SqlConnection(RfaConStr))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[spGetVendorRecipientList]";
            cmd.Parameters.Add("@SearchText", SqlDbType.VarChar, 150).Value = SearchText;
            cmd.Parameters.Add("@VendorNumber", SqlDbType.VarChar, 15).Value = VendorNumber;

            cmd.Connection = conn;

            try
            {
                conn.Open();
                Sda.SelectCommand = cmd;
                Sda.Fill(Dt);

                conn.Close();
                return Dt;
            }
            catch (SqlException sqlex)
            {
                LogException(sqlex);

                throw new Exception(ErrException);
            }
            finally
            {
                Sda.Dispose();
                cmd.Dispose();
            }
        }
    }

    public DataTable GetVendorUserByUid()
    {
        using (SqlConnection conn = new SqlConnection(RfaConStr))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[spGetVendorRecipient]";
            cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 15).Value = UserId;
            cmd.Parameters.Add("@VendorNumber", SqlDbType.VarChar, 15).Value = VendorNumber;
            cmd.Parameters.Add("@Active", SqlDbType.VarChar, 150).Value = Active;

            cmd.Connection = conn;

            try
            {
                conn.Open();
                Sda.SelectCommand = cmd;
                Sda.Fill(Dt);

                conn.Close();
                return Dt;
            }
            catch (SqlException sqlex)
            {
                LogException(sqlex);

                throw new Exception(ErrException);
            }
            finally
            {
                Sda.Dispose();
                cmd.Dispose();
            }
        }
    }

    // Helper routine that logs SqlException details to the
    // Application event log
    private void LogException(SqlException sqlex)
    {
        string strSource = "Internal_Class_GetVendors";
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