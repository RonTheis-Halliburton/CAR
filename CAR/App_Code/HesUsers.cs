using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class HesUsers
{
    public string Country { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string SearchText { get; set; }
    public string Active { get; set; }
    public string EmployeeID { get; set; }
    public string Department { get; set; }
    public string GivenName { get; set; }
    public string SN { get; set; }
    public string HomeDirectory { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public string MiddleName { get; set; }
    public string UserID { get; set; }
    public string Phone { get; set; }

    public string Manager { get; set; }
    public string ManagerID { get; set; }
    public string ManagerEmail { get; set; }

    public DataTable Dt = new DataTable();
    public SqlDataAdapter Sda = new SqlDataAdapter();
    public string ConStr = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
    public string VendorBarcodeConStr = ConfigurationManager.ConnectionStrings["dbVendorBarcode"].ConnectionString;
    public string ErrException = ConfigurationManager.AppSettings["ErrorMessage"] + ConfigurationManager.AppSettings["AdminContact"];

    public DataTable Get_System_Admin()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[SP_GET_SYSTEM_ADMIN]";
            cmd.Parameters.Add(parameterName: "@UserID", sqlDbType: SqlDbType.VarChar, size: -1).Value = UserID;
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

    public DataTable GetHesUsers()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[sp_Get_HES_Users]";
            cmd.Parameters.Add(parameterName: "@SearchValue", sqlDbType: SqlDbType.VarChar, size: -1).Value = SearchText;
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

    public DataTable GetHesUserByUid()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[sp_Get_HES_BY_UID]";
            cmd.Parameters.Add(parameterName: "@UserID", sqlDbType: SqlDbType.VarChar, size: 15).Value = UserID;
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

    public DataTable GetPrnl()
    {
        using (SqlConnection conn = new SqlConnection(VendorBarcodeConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[spGetPRNL]";
            cmd.Parameters.Add(parameterName: "@Usr_ID", sqlDbType: SqlDbType.VarChar, size: 15).Value = UserID;
            cmd.Parameters.Add(parameterName: "@EMail", sqlDbType: SqlDbType.VarChar, size: 150).Value = Email;
            cmd.Parameters.Add(parameterName: "@FullName", sqlDbType: SqlDbType.VarChar, size: 150).Value = DisplayName;
            cmd.Connection = conn;

            try
            {
                conn.Open();
                Sda.SelectCommand = cmd;
                Sda.Fill(Dt);
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


    // Helper routine that logs SqlException details to the
    // Application event log
    private void LogException(SqlException sqlex)
    {
        string strSource = "Class_HESUsers";
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
