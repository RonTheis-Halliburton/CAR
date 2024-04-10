using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

public class GetRecord
{

    public string Oid { set; get; }
    public string CarNbr { set; get; }
    public string CarOid { set; get; }
    public string VendorNbr { set; get; }

    public SqlDataAdapter Sda = new SqlDataAdapter();
    public string ConStr = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
    public string VendorBarcodeConStr = ConfigurationManager.ConnectionStrings["dbVendorBarcode"].ConnectionString;
    public string ErrException = ConfigurationManager.AppSettings["ErrorMessage"] + ConfigurationManager.AppSettings["AdminContact"];

    public DataTable Exec_GetQdmsPlant_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(VendorBarcodeConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetPlant";
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

    public DataTable Exec_GetArea_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_AREA";
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

    public DataTable Exec_GetDocument_Type_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_DOCUMENT_TYPE";
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

    public DataTable Exec_GetAPI_ISO_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_API_ISO_REF";
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

    public DataTable Exec_GetCategory_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_CATEGORY";
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

    public DataTable Exec_GetFindingType_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_FINDING_TYPE";
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

    public DataTable Exec_GetFacility_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_FACILITY_NAMES";
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

    public DataTable Exec_GetCarNbr_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_CAR_NBR";
            cmd.Parameters.Add(parameterName: "@CarNbr", sqlDbType: SqlDbType.VarChar, size: -1).Value = CarNbr;
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

    public DataTable Exec_GetCar_Api_Iso_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_CAR_API_ISO";
            cmd.Parameters.Add(parameterName: "@Car_OId", sqlDbType: SqlDbType.Int, size: 0).Value = CarOid;
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

    public DataTable Exec_GetCar_Document_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_CAR_DOCUMENT";
            cmd.Parameters.Add(parameterName: "@Car_OId", sqlDbType: SqlDbType.Int, size: 0).Value = CarOid;
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

    public DataTable Exec_GetCar_Qdms_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_CAR_DOCUMENT_QDMS";
            cmd.Parameters.Add(parameterName: "@Car_OId", sqlDbType: SqlDbType.Int, size: 0).Value = CarOid;
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

    public DataTable Exec_GetUser_Sent_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_CAR_EMAIL_SENT";
            cmd.Parameters.Add(parameterName: "@CAR_OID", sqlDbType: SqlDbType.Int, size: 0).Value = CarOid;
            cmd.Parameters.Add(parameterName: "@VENDOR_NBR", sqlDbType: SqlDbType.VarChar, size: 10).Value = VendorNbr;
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

    public DataTable Exec_GetEmail_Sent_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_CAR_EMAIL";
            cmd.Parameters.Add(parameterName: "@CAR_OID", sqlDbType: SqlDbType.Int, size: 0).Value = CarOid;
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

    public DataTable Exec_Get_Existing_Car_By_Oid_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "SP_GET_EXISTING_CAR_BY_OID"
            };
            cmd.Parameters.Add(parameterName: "@Car_Oid", sqlDbType: SqlDbType.Int, size: -1).Value = CarOid;
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

    public DataTable Exec_Get_Existing_EV_By_Oid_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "SP_GET_EFF_VALIDATIONS_PICKED_BY_CAR_OID"
            };
            cmd.Parameters.Add(parameterName: "@Oid", sqlDbType: SqlDbType.Int, size: -1).Value = CarOid;
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

    public DataTable Exec_GetCountry_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_COUNTRY_LIST";
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

    public DataTable Exec_GetSearchDate_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_SEARCH_DATE";
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

    public DataTable Exec_Get_Other_Ref_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_OTHER_REF";
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

    public DataTable Exec_GetPsl_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_PSL";
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

    public DataTable Exec_GetSarchBy_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_SEARCH_BY";
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

    public DataTable Exec_GetStatus_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_CAR_STATUS";
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

    public DataTable Exec_GetEV_Datatable()
    {
        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_EFF_VALIDATIONS";
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

    // This gets a list of the effectiveness validation description OIDs
    //   currently in use in the system
    public List<int> Exec_GetEVD_Oids()
    {
        DataTable dt = null;

        try
        {
            dt = this.Exec_GetEV_Datatable();
            return dt.Rows.Cast<DataRow>().Select(x => (int)x["OID"]).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(
              string.Format("Error in {0}.{1}:\r\n{2}",
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), ex);
        }
    }

    // Helper routine that logs SqlException details to the
    // Application event log
    private void LogException(SqlException sqlex)
    {
        string strSource;
        string strMessageAll;

        strSource = "Class_GetRecord";
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