using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class GetSearch
{
    public string SrchField { set; get; }
    public string SrchText { set; get; }
    public string CarOid { set; get; }
    public string StatusEv { set; get; }
    public string StatusOid { set; get; }
    public string AreaOid { set; get; }
    public string FindingTypeOid { set; get; }
    public string PlantOid { set; get; }
    public string PslOid { set; get; }
    public string CategoryOid { set; get; }
    public string ApiIsoOid { set; get; }
    public string SearchDate { set; get; }
    public string DateFrom { set; get; }
    public string DateTo { set; get; }
    public string CarAdmin { set; get; }
    public string Deleted { set; get; }
    public string Indexed { set; get; }
    public string OriginatorManager { set; get; }
    public string OriginatorManagerID { set; get; }
    public string OriginatorManagerEmail { set; get; }
    public string OriginatorID { set; get; }

    public string ConStr = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
    public string ErrException = "An error has occurred.  If problem persists, please contact the administrator: " + ConfigurationManager.AppSettings["AdminContact"];

    public DataTable SearchCar()
    {
        using (DataTable dt = new DataTable())
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                using (SqlConnection conn = new SqlConnection(ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[SP_Search_CAR]";
                        cmd.Parameters.Add(parameterName: "@SearchText", sqlDbType: SqlDbType.VarChar, size: 150).Value = SrchText;
                        cmd.Parameters.Add(parameterName: "@SearchByField", sqlDbType: SqlDbType.VarChar, size: -1).Value = SrchField;
                        cmd.Parameters.Add(parameterName: "@StatusOid", sqlDbType: SqlDbType.VarChar, size: -1).Value = StatusOid;
                        cmd.Parameters.Add(parameterName: "@AreaOid", sqlDbType: SqlDbType.VarChar, size: -1).Value = AreaOid;
                        cmd.Parameters.Add(parameterName: "@FindingTypeOid", sqlDbType: SqlDbType.VarChar, size: -1).Value = FindingTypeOid;
                        cmd.Parameters.Add(parameterName: "@PlantOid", sqlDbType: SqlDbType.VarChar, size: -1).Value = PlantOid;
                        cmd.Parameters.Add(parameterName: "@PslOid", sqlDbType: SqlDbType.VarChar, size: -1).Value = PslOid;
                        cmd.Parameters.Add(parameterName: "@CategoryOid", sqlDbType: SqlDbType.VarChar, size: -1).Value = CategoryOid;
                        cmd.Parameters.Add(parameterName: "@ApiIso", sqlDbType: SqlDbType.VarChar, size: -1).Value = ApiIsoOid;
                        cmd.Parameters.Add(parameterName: "@SearchDate", sqlDbType: SqlDbType.VarChar, size: 150).Value = SearchDate;
                        cmd.Parameters.Add(parameterName: "@DateTo", sqlDbType: SqlDbType.VarChar, size: 150).Value = DateTo;
                        cmd.Parameters.Add(parameterName: "@DateFrom", sqlDbType: SqlDbType.VarChar, size: 150).Value = DateFrom;
                        cmd.Parameters.Add(parameterName: "@CarAdmin", sqlDbType: SqlDbType.VarChar, size: 10).Value = CarAdmin;
                        cmd.Parameters.Add(parameterName: "@Deleted", sqlDbType: SqlDbType.VarChar, size: 15).Value = Deleted;
                        cmd.Parameters.Add(parameterName: "@Indexed", sqlDbType: SqlDbType.VarChar, size: 15).Value = Indexed;

                        cmd.Parameters.Add(parameterName: "@OriginatorManager", sqlDbType: SqlDbType.VarChar, size: 150).Value = OriginatorManager;
                        cmd.Parameters.Add(parameterName: "@OriginatorManagerID", sqlDbType: SqlDbType.VarChar, size: 150).Value = OriginatorManagerID;
                        cmd.Parameters.Add(parameterName: "@OriginatorManagerEmail", sqlDbType: SqlDbType.VarChar, size: 150).Value = OriginatorManagerEmail;

                        cmd.Connection = conn;

                        try
                        {
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);

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
            }
        }
    }

    public DataTable SearchCarActive()
    {
        using (DataTable dt = new DataTable())
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                using (SqlConnection conn = new SqlConnection(ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[SP_Search_CAR_ACTIVE]";
                        cmd.Parameters.Add(parameterName: "@PlantOid", sqlDbType: SqlDbType.VarChar, size: -1).Value = PlantOid;
                        cmd.Parameters.Add(parameterName: "@PslOid", sqlDbType: SqlDbType.VarChar, size: -1).Value = PslOid;
                        cmd.Parameters.Add(parameterName: "@OriginatorID", sqlDbType: SqlDbType.VarChar, size: 10).Value = OriginatorID;
                        cmd.Parameters.Add(parameterName: "@CarAdmin", sqlDbType: SqlDbType.VarChar, size: 10).Value = CarAdmin;
                        cmd.Parameters.Add(parameterName: "@OriginatorManager", sqlDbType: SqlDbType.VarChar, size: 150).Value = OriginatorManager;
                        cmd.Parameters.Add(parameterName: "@OriginatorManagerID", sqlDbType: SqlDbType.VarChar, size: 150).Value = OriginatorManagerID;
                        cmd.Parameters.Add(parameterName: "@OriginatorManagerEmail", sqlDbType: SqlDbType.VarChar, size: 150).Value = OriginatorManagerEmail;

                        cmd.Connection = conn;

                        try
                        {
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);

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
            }
        }
    }


    public DataTable SearchCarByOid()
    {
        using (DataTable dt = new DataTable())
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                using (SqlConnection conn = new SqlConnection(ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[SP_Search_CAR_BY_OID]";
                        cmd.Parameters.Add(parameterName: "@Car_Oid", sqlDbType: SqlDbType.Int, size: 0).Value = CarOid;
                        cmd.Parameters.Add(parameterName: "@CarAdmin", sqlDbType: SqlDbType.VarChar, size: 10).Value = CarAdmin;
                        cmd.Connection = conn;

                        try
                        {
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);

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
            }
        }
    }

    // Helper routine that logs SqlException details to the
    // Application event log
    private void LogException(SqlException sqlex)
    {
        string strSource = "Internal_Class_GetSearch";
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