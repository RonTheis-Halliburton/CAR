using System.Configuration;
using System.Data;
using System.Data.SqlClient;


public class CreateRecord
{
    public SqlDataAdapter Sda = new SqlDataAdapter();
    public string ConStr = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
    public string ErrException = ConfigurationManager.AppSettings["ErrorMessage"] + ConfigurationManager.AppSettings["AdminContact"];

    public string Car_Oid { set; get; }
    public string Car_Email_Oid { set; get; }
    public string Created_By { set; get; }
    public string Originator_Usr_Id { set; get; }
    public string Date_Issued { set; get; }
    public string Due_dt { set; get; }
    public string Finding_Type_Oid { set; get; }
    public string Area_Descript_Oid { set; get; }
    public string Psl_Oid { set; get; }
    public string Facility_Name_Oid { set; get; }
    public string Finding_Desc { set; get; }
    public string Desc_Of_Improvement { set; get; }
    public string Api_Iso_Oid { set; get; }
    public string Category_Oid { set; get; }
    public string Audit_Nbr { set; get; }
    public string Qnote_Nbr { set; get; }
    public string Cpi_Nbr { set; get; }
    public string Material_Nbr { set; get; }
    public string Maintenance_Order_Nbr { set; get; }
    public string Equipment_Nbr { set; get; }
    public string Purchase_Order_Nbr { set; get; }
    public string Production_Order_Nbr { set; get; }
    public string Api_Audit_Nbr { set; get; }
    public string Vndr_Nbr { set; get; }
    public string Vendor_Nm { set; get; }
    public string Issued_To_Usr_Id { set; get; }
    public string Issued_To_Usr_Email { set; get; }
    public string Issued_To_Usr_Name { set; get; }
    public string Loc_Country_Oid { set; get; }
    public string Loc_Supplier { set; get; }
    public string Resp_Person_Usr_Id { set; get; }
    public string Non_Conform_Rsn { set; get; }
    public string Similar_Instance { set; get; }
    public string Corr_Action_Taken { set; get; }
    public string Corr_Action_Taken_Dt { set; get; }
    public string Preclude_Action { set; get; }
    public string Preclude_Action_Dt { set; get; }
    public string Action_Taken_By_Usr_Id { set; get; }
    public string Response_Dt { set; get; }
    public string Due_Dt_Ext { set; get; }
    public string Reissued_To_Usr_Id { set; get; }
    public string Reissued_Dt { set; get; }
    public string Received_Dt { set; get; }
    public string Follow_Up_Reqd { set; get; }
    public string Follow_Up_Dt { set; get; }
    public string Verify_Dt { set; get; }
    public string Verify_By_Usr_Id { set; get; }
    public string Response_Accept_By_Usr_Id { set; get; }
    public string Close_Dt { set; get; }
    public string User_Id { set; get; }
    public string User_Name { set; get; }
    public string User_Email { set; get; }
    public string Email_Subject { set; get; }
    public string Email_Message { set; get; }

    public string File_Size { set; get; }
    public string File_Name { set; get; }
    public string File_NameAlias { set; get; }
    public string Uploaded_By { set; get; }
    public string Ftp_RemotePath { set; get; }


    public string[] ExecCreateCar()
    {
        string[] uReturn = { string.Empty, string.Empty, string.Empty, string.Empty };

        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[SP_CREATE_CAR]";
            cmd.Parameters.Add(parameterName: "@CREATE_BY", sqlDbType: SqlDbType.VarChar, size: 8).Value = Created_By;
            cmd.Parameters.Add(parameterName: "@ORIGINATOR_USR_ID", sqlDbType: SqlDbType.VarChar, size: 50).Value = Originator_Usr_Id;
            cmd.Parameters.Add(parameterName: "@DATE_ISSUED", sqlDbType: SqlDbType.VarChar, size: 25).Value = Date_Issued;
            cmd.Parameters.Add(parameterName: "@DUE_DT", sqlDbType: SqlDbType.VarChar, size: 25).Value = Due_dt;
            cmd.Parameters.Add(parameterName: "@FINDING_TYPE_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Finding_Type_Oid;
            cmd.Parameters.Add(parameterName: "@AREA_DESCRIPT_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Area_Descript_Oid;
            cmd.Parameters.Add(parameterName: "@PSL_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Psl_Oid;
            cmd.Parameters.Add(parameterName: "@FACILITY_NAME_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Facility_Name_Oid;
            cmd.Parameters.Add(parameterName: "@FINDING_DESC", sqlDbType: SqlDbType.VarChar, size: -1).Value = Finding_Desc;
            cmd.Parameters.Add(parameterName: "@DESC_OF_IMPROVEMENT", sqlDbType: SqlDbType.VarChar, size: -1).Value = Desc_Of_Improvement;
            cmd.Parameters.Add(parameterName: "@CATEGORY_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Category_Oid;
            cmd.Parameters.Add(parameterName: "@AUDIT_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Audit_Nbr;
            cmd.Parameters.Add(parameterName: "@QNOTE_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Qnote_Nbr;
            cmd.Parameters.Add(parameterName: "@CPI_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Cpi_Nbr;
            cmd.Parameters.Add(parameterName: "@MATERIAL_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Material_Nbr;
            cmd.Parameters.Add(parameterName: "@PURCHASE_ORDER_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Purchase_Order_Nbr;
            cmd.Parameters.Add(parameterName: "@PRODUCTION_ORDER_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Production_Order_Nbr;
            cmd.Parameters.Add(parameterName: "@API_AUDIT_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Api_Audit_Nbr;
            cmd.Parameters.Add(parameterName: "@VNDR_NBR", sqlDbType: SqlDbType.VarChar, size: 20).Value = Vndr_Nbr;
            cmd.Parameters.Add(parameterName: "@VENDOR_NM", sqlDbType: SqlDbType.VarChar, size: 100).Value = Vendor_Nm;
            cmd.Parameters.Add(parameterName: "@ISSUED_TO_USR_ID", sqlDbType: SqlDbType.VarChar, size: 50).Value = Issued_To_Usr_Id;
            cmd.Parameters.Add(parameterName: "@ISSUED_TO_USR_EMAIL", sqlDbType: SqlDbType.VarChar, size: 150).Value = Issued_To_Usr_Email;
            cmd.Parameters.Add(parameterName: "@ISSUED_TO_USR_NAME", sqlDbType: SqlDbType.VarChar, size: 150).Value = Issued_To_Usr_Name;
            cmd.Parameters.Add(parameterName: "@LOC_COUNTRY_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Loc_Country_Oid;
            cmd.Parameters.Add(parameterName: "@LOC_SUPPLIER", sqlDbType: SqlDbType.VarChar, size: 50).Value = Loc_Supplier;
            cmd.Parameters.Add(parameterName: "@MAINTENANCE_ORDER_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Maintenance_Order_Nbr;
            cmd.Parameters.Add(parameterName: "@EQUIPMENT_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Equipment_Nbr;

            SqlParameter paramResult;
            paramResult = new SqlParameter(parameterName: "@Result", dbType: SqlDbType.VarChar, size: -1);
            paramResult.Value = string.Empty;
            paramResult.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramResult);

            SqlParameter paramNewOid;
            paramNewOid = new SqlParameter(parameterName: "@New_OID", dbType: SqlDbType.VarChar, size: -1);
            paramNewOid.Value = 0;
            paramNewOid.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramNewOid);

            SqlParameter paramCarYr;
            paramCarYr = new SqlParameter(parameterName: "@Return_CAR_YR", dbType: SqlDbType.VarChar, size: -1);
            paramCarYr.Value = 0;
            paramCarYr.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramCarYr);

            SqlParameter paramCarNbr;
            paramCarNbr = new SqlParameter(parameterName: "@Return_CAR_INCR_NBR", dbType: SqlDbType.VarChar, size: -1);
            paramCarNbr.Value = 0;
            paramCarNbr.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramCarNbr);

            try
            {
                cmd.Connection = conn;

                conn.Open();
                SqlDataReader myReader = cmd.ExecuteReader();
                uReturn[0] = paramResult.Value.ToString();
                uReturn[1] = paramNewOid.Value.ToString();
                uReturn[2] = paramCarYr.Value.ToString();
                uReturn[3] = paramCarNbr.Value.ToString();

                myReader.Dispose();
            }
            catch (SqlException sqlex)
            {
                LogException(sqlex);

                uReturn[0] = ErrException + "..." + sqlex.Message.ToString();
                uReturn[1] = ErrException;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        return uReturn;
    }

    public string ExecCreateCar_Api_Iso()
    {
        string uReturn = string.Empty;

        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            try
            {
                SqlCommand cmd;
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[SP_CREATE_CAR_API_ISO]";
                cmd.Parameters.Add(parameterName: "@CAR_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Car_Oid;
                cmd.Parameters.Add(parameterName: "@API_ISO_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Api_Iso_Oid;

                SqlParameter paramResult;
                paramResult = new SqlParameter(parameterName: "@Result", dbType: SqlDbType.VarChar, size: -1);
                paramResult.Value = string.Empty;
                paramResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramResult);

                cmd.Connection = conn;

                conn.Open();
                SqlDataReader myReader = cmd.ExecuteReader();
                uReturn = paramResult.Value.ToString();

                myReader.Dispose();
            }
            catch (SqlException sqlex)
            {
                LogException(sqlex);

                uReturn = ErrException;
            }

        }

        return uReturn;
    }

    public string[] ExecCreateCar_Email()
    {
        string[] uReturn = { string.Empty, string.Empty };

        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[SP_CREATE_CAR_EMAIL]";
            cmd.Parameters.Add(parameterName: "@CAR_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Car_Oid;
            cmd.Parameters.Add(parameterName: "@Subject", sqlDbType: SqlDbType.VarChar, size: -1).Value = Email_Subject;
            cmd.Parameters.Add(parameterName: "@Message", sqlDbType: SqlDbType.VarChar, size: -1).Value = Email_Message;
            cmd.Parameters.Add(parameterName: "@Created_User_ID", sqlDbType: SqlDbType.VarChar, size: 15).Value = User_Id;
            cmd.Parameters.Add(parameterName: "@Created_User_NM", sqlDbType: SqlDbType.VarChar, size: 150).Value = User_Name;

            SqlParameter paramResult;
            paramResult = new SqlParameter(parameterName: "@Result", dbType: SqlDbType.VarChar, size: -1);
            paramResult.Value = string.Empty;
            paramResult.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramResult);

            SqlParameter paramNewOid;
            paramNewOid = new SqlParameter(parameterName: "@New_OID", dbType: SqlDbType.VarChar, size: -1);
            paramNewOid.Value = 0;
            paramNewOid.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramNewOid);

            try
            {
                cmd.Connection = conn;
                conn.Open();

                SqlDataReader myReader = cmd.ExecuteReader();
                uReturn[0] = paramResult.Value.ToString();
                uReturn[1] = paramNewOid.Value.ToString();

                myReader.Dispose();
            }
            catch (SqlException sqlex)
            {
                LogException(sqlex);

                uReturn[0] = ErrException + "..." + sqlex.Message.ToString();
                uReturn[1] = ErrException;
            }
            finally
            {
                cmd.Dispose();
            }

        }

        return uReturn;
    }

    public string ExecCreateCar_Email_Sent_To()
    {
        string uReturn = string.Empty;

        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            try
            {
                SqlCommand cmd;
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[SP_CREATE_CAR_EMAIL_SENT_TO]";
                cmd.Parameters.Add(parameterName: "@CAR_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Car_Oid;
                cmd.Parameters.Add(parameterName: "@CAR_EMAIL_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Car_Email_Oid;
                cmd.Parameters.Add(parameterName: "@USER_ID", sqlDbType: SqlDbType.VarChar, size: 15).Value = User_Id;
                cmd.Parameters.Add(parameterName: "@USER_NAME", sqlDbType: SqlDbType.VarChar, size: 150).Value = User_Name;
                cmd.Parameters.Add(parameterName: "@USER_EMAIL", sqlDbType: SqlDbType.VarChar, size: 250).Value = User_Email;
                cmd.Parameters.Add(parameterName: "@VENDOR_NBR", sqlDbType: SqlDbType.VarChar, size: 15).Value = Vendor_Nm;

                SqlParameter paramResult;
                paramResult = new SqlParameter(parameterName: "@Result", dbType: SqlDbType.VarChar, size: -1);
                paramResult.Value = string.Empty;
                paramResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramResult);

                cmd.Connection = conn;

                conn.Open();
                SqlDataReader myReader = cmd.ExecuteReader();
                uReturn = paramResult.Value.ToString();

                myReader.Dispose();
            }
            catch (SqlException sqlex)
            {
                LogException(sqlex);

                uReturn = ErrException;
            }

        }

        return uReturn;
    }


    public string[] ExecCreateCar_QDMS()
    {
        string[] uReturn = { string.Empty, string.Empty };

        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[SP_CREATE_CAR_DOCUMENT_QDMS]";
            cmd.Parameters.Add(parameterName: "@CAR_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Car_Oid;
            cmd.Parameters.Add(parameterName: "@FileSize", sqlDbType: SqlDbType.Int, size: 0).Value = File_Size;
            cmd.Parameters.Add(parameterName: "@FileName", sqlDbType: SqlDbType.VarChar, size: 150).Value = File_Name;
            cmd.Parameters.Add(parameterName: "@FileNameAlias", sqlDbType: SqlDbType.VarChar, size: 150).Value = File_NameAlias;
            cmd.Parameters.Add(parameterName: "@UploadedBy", sqlDbType: SqlDbType.VarChar, size: 150).Value = Uploaded_By;
            cmd.Parameters.Add(parameterName: "@FtpRemotePath", sqlDbType: SqlDbType.VarChar, size: 150).Value = Ftp_RemotePath;

            SqlParameter paramResult;
            paramResult = new SqlParameter(parameterName: "@Result", dbType: SqlDbType.VarChar, size: -1);
            paramResult.Value = string.Empty;
            paramResult.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramResult);

            SqlParameter paramNewOid;
            paramNewOid = new SqlParameter(parameterName: "@New_OID", dbType: SqlDbType.VarChar, size: -1);
            paramNewOid.Value = 0;
            paramNewOid.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramNewOid);

            try
            {
                cmd.Connection = conn;
                conn.Open();

                SqlDataReader myReader = cmd.ExecuteReader();
                uReturn[0] = paramResult.Value.ToString();
                uReturn[1] = paramNewOid.Value.ToString();

                myReader.Dispose();
            }
            catch (SqlException sqlex)
            {
                LogException(sqlex);

                uReturn[0] = ErrException + "..." + sqlex.Message.ToString();
                uReturn[1] = ErrException;
            }
            finally
            {
                cmd.Dispose();
            }

        }

        return uReturn;
    }

    public string[] ExecCreateCar_Document()
    {
        string[] uReturn = { string.Empty, string.Empty };

        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[SP_CREATE_CAR_DOCUMENT]";
            cmd.Parameters.Add(parameterName: "@CAR_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Car_Oid;
            cmd.Parameters.Add(parameterName: "@FileSize", sqlDbType: SqlDbType.Int, size: 0).Value = File_Size;
            cmd.Parameters.Add(parameterName: "@FileName", sqlDbType: SqlDbType.VarChar, size: 150).Value = File_Name;
            cmd.Parameters.Add(parameterName: "@FileNameAlias", sqlDbType: SqlDbType.VarChar, size: 150).Value = File_NameAlias;
            cmd.Parameters.Add(parameterName: "@UploadedBy", sqlDbType: SqlDbType.VarChar, size: 150).Value = Uploaded_By;
            cmd.Parameters.Add(parameterName: "@FtpRemotePath", sqlDbType: SqlDbType.VarChar, size: 150).Value = Ftp_RemotePath;

            SqlParameter paramResult;
            paramResult = new SqlParameter(parameterName: "@Result", dbType: SqlDbType.VarChar, size: -1);
            paramResult.Value = string.Empty;
            paramResult.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramResult);

            SqlParameter paramNewOid;
            paramNewOid = new SqlParameter(parameterName: "@New_OID", dbType: SqlDbType.VarChar, size: -1);
            paramNewOid.Value = 0;
            paramNewOid.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramNewOid);

            try
            {
                cmd.Connection = conn;
                conn.Open();

                SqlDataReader myReader = cmd.ExecuteReader();
                uReturn[0] = paramResult.Value.ToString();
                uReturn[1] = paramNewOid.Value.ToString();

                myReader.Dispose();
            }
            catch (SqlException sqlex)
            {
                LogException(sqlex);

                uReturn[0] = ErrException + "..." + sqlex.Message.ToString();
                uReturn[1] = ErrException;
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
        string strSource = "Internal_Class_CreateRecord";
        string strMessageAll;
        strMessageAll = "Exception Number : " + sqlex.Number + "(" + sqlex.Message + ") has occurred";

        SqlLog logIt = new SqlLog();
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