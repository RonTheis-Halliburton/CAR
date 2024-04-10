using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class UpdateRecord
{
    public SqlDataAdapter Sda = new SqlDataAdapter();
    public string ConStr = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
    public string ErrException = ConfigurationManager.AppSettings["ErrorMessage"] + ConfigurationManager.AppSettings["AdminContact"];

    public string Car_Oid { set; get; }
    public string Root_Cause { set; get; }
    public string Del_Flg { set; get; }
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
    public string Purchase_Order_Nbr { set; get; }
    public string Production_Order_Nbr { set; get; }
    public string Api_Audit_Nbr { set; get; }
    public string Maintenance_Order_Nbr { set; get; }
    public string Equipment_Nbr { set; get; }
    public string Vndr_Nbr { set; get; }
    public string Vendor_Nm { set; get; }
    public string Issued_To_Usr_Id { set; get; }
    public string Issued_To_Usr_Name { set; get; }
    public string Issued_To_Usr_Email { set; get; }
    public string Loc_Country_Oid { set; get; }
    public string Loc_Supplier { set; get; }
    public string Resp_Person_Usr_Id { set; get; }
    public string Non_Conform_Rsn { set; get; }
    public string Similar_Instance { set; get; }
    public string Similar_Instance_Y_N { set; get; }
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
    public string Remarks { set; get; }
    public string Last_Updt_by { set; get; }
    public string Car_Status_Oid { set; get; }
    public string Updt_by { set; get; }

    //public string Day_29_Notify_Sent { set; get; }
    //public string Day_29_Notify_Dt { set; get; }
    //public string Day_15_Notify_Sent { set; get; }
    //public string Day_15_Notify_Dt { set; get; }
    //public string Day_1_Notify_Sent { set; get; }
    //public string Day_1_Notify_Dt { set; get; }

    public string ExecDeleteCar()
    {
        string uReturn = string.Empty;

        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            try
            {
                SqlCommand cmd;
                cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "[SP_DELETE_CAR]"
                };
                cmd.Parameters.Add(parameterName: "@CAR_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Car_Oid;
                cmd.Parameters.Add(parameterName: "@USER_ID", sqlDbType: SqlDbType.VarChar, size: 15).Value = User_Id;
                cmd.Parameters.Add(parameterName: "@DEL_FLG", sqlDbType: SqlDbType.Int, size: 0).Value = Del_Flg;

                SqlParameter paramResult;
                paramResult = new SqlParameter(parameterName: "@Result", dbType: SqlDbType.VarChar, size: -1)
                {
                    Value = string.Empty,
                    Direction = ParameterDirection.Output
                };
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

    public string ExecDeleteCarApiIso()
    {
        string uReturn = string.Empty;

        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            try
            {
                SqlCommand cmd;
                cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "[SP_DELETE_CAR_API_ISO]"
                };
                cmd.Parameters.Add(parameterName: "@CAR_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Car_Oid;

                SqlParameter paramResult;
                paramResult = new SqlParameter(parameterName: "@Result", dbType: SqlDbType.VarChar, size: -1)
                {
                    Value = string.Empty,
                    Direction = ParameterDirection.Output
                };
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


    public string ExecUpdateQdms()
    {
        string uReturn = string.Empty;

        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            try
            {
                SqlCommand cmd;
                cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "[SP_UPDATE_QDMS]"
                };
                cmd.Parameters.Add(parameterName: "@CAR_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Car_Oid;
                cmd.Parameters.Add(parameterName: "@UPDT_BY", sqlDbType: SqlDbType.VarChar, size: 150).Value = Updt_by;

                SqlParameter paramResult;
                paramResult = new SqlParameter(parameterName: "@Result", dbType: SqlDbType.VarChar, size: -1)
                {
                    Value = string.Empty,
                    Direction = ParameterDirection.Output
                };
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


    public string ExecOpenCar()
    {
        string uReturn = string.Empty;

        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            try
            {
                SqlCommand cmd;
                cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "[SP_OPEN_CAR]"
                };
                cmd.Parameters.Add(parameterName: "@CAR_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Car_Oid;

                SqlParameter paramResult;
                paramResult = new SqlParameter(parameterName: "@Result", dbType: SqlDbType.VarChar, size: -1)
                {
                    Value = string.Empty,
                    Direction = ParameterDirection.Output
                };
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

    public string ExecUpdateCar()
    {
        string uReturn = string.Empty;

        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[SP_UPDATE_CAR]"
            };
            cmd.Parameters.Add(parameterName: "@OID", sqlDbType: SqlDbType.Int, size: 0).Value = Car_Oid;
            cmd.Parameters.Add(parameterName: "@CAR_STATUS_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Car_Status_Oid;
            cmd.Parameters.Add(parameterName: "@AREA_DESCRIPT_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Area_Descript_Oid;
            cmd.Parameters.Add(parameterName: "@DATE_ISSUED", sqlDbType: SqlDbType.VarChar, size: 50).Value = Date_Issued;
            cmd.Parameters.Add(parameterName: "@ISSUED_TO_USR_ID", sqlDbType: SqlDbType.VarChar, size: 50).Value = Issued_To_Usr_Id;
            cmd.Parameters.Add(parameterName: "@ISSUED_TO_USR_NAME", sqlDbType: SqlDbType.VarChar, size: 150).Value = Issued_To_Usr_Name;
            cmd.Parameters.Add(parameterName: "@ISSUED_TO_USR_EMAIL", sqlDbType: SqlDbType.VarChar, size: 150).Value = Issued_To_Usr_Email;
            cmd.Parameters.Add(parameterName: "@DUE_DT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Due_dt;
            cmd.Parameters.Add(parameterName: "@ORIGINATOR_USR_ID", sqlDbType: SqlDbType.VarChar, size: 50).Value = Originator_Usr_Id;
            cmd.Parameters.Add(parameterName: "@FACILITY_NAME_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Facility_Name_Oid;
            cmd.Parameters.Add(parameterName: "@LOC_SUPPLIER", sqlDbType: SqlDbType.VarChar, size: 50).Value = Loc_Supplier;
            cmd.Parameters.Add(parameterName: "@VNDR_NBR", sqlDbType: SqlDbType.VarChar, size: 20).Value = Vndr_Nbr;
            cmd.Parameters.Add(parameterName: "@VENDOR_NM", sqlDbType: SqlDbType.VarChar, size: 100).Value = Vendor_Nm;
            cmd.Parameters.Add(parameterName: "@FINDING_DESC", sqlDbType: SqlDbType.VarChar, size: -1).Value = Finding_Desc;
            cmd.Parameters.Add(parameterName: "@FINDING_TYPE_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Finding_Type_Oid;
            cmd.Parameters.Add(parameterName: "@PSL_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Psl_Oid;
            cmd.Parameters.Add(parameterName: "@DESC_OF_IMPROVEMENT", sqlDbType: SqlDbType.VarChar, size: -1).Value = Desc_Of_Improvement;
            cmd.Parameters.Add(parameterName: "@CATEGORY_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Category_Oid;
            cmd.Parameters.Add(parameterName: "@LOC_COUNTRY_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Loc_Country_Oid;
            cmd.Parameters.Add(parameterName: "@AUDIT_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Audit_Nbr;
            cmd.Parameters.Add(parameterName: "@QNOTE_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Qnote_Nbr;
            cmd.Parameters.Add(parameterName: "@CPI_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Cpi_Nbr;
            cmd.Parameters.Add(parameterName: "@MATERIAL_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Material_Nbr;
            cmd.Parameters.Add(parameterName: "@PURCHASE_ORDER_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Purchase_Order_Nbr;
            cmd.Parameters.Add(parameterName: "@PRODUCTION_ORDER_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Production_Order_Nbr;
            cmd.Parameters.Add(parameterName: "@API_AUDIT_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Api_Audit_Nbr;
            cmd.Parameters.Add(parameterName: "@MAINTENANCE_ORDER_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Maintenance_Order_Nbr;
            cmd.Parameters.Add(parameterName: "@EQUIPMENT_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Equipment_Nbr;

            cmd.Parameters.Add(parameterName: "@RESP_PERSON_USR_ID", sqlDbType: SqlDbType.VarChar, size: 100).Value = Resp_Person_Usr_Id;
            cmd.Parameters.Add(parameterName: "@NON_CONFORM_RSN", sqlDbType: SqlDbType.VarChar, size: -1).Value = Non_Conform_Rsn;
            cmd.Parameters.Add(parameterName: "@ROOT_CAUSE", sqlDbType: SqlDbType.VarChar, size: -1).Value = Root_Cause;
            cmd.Parameters.Add(parameterName: "@SIMILAR_INSTANCE", sqlDbType: SqlDbType.VarChar, size: -1).Value = Similar_Instance;
            cmd.Parameters.Add(parameterName: "@CORR_ACTION_TAKEN", sqlDbType: SqlDbType.VarChar, size: -1).Value = Corr_Action_Taken;
            cmd.Parameters.Add(parameterName: "@CORR_ACTION_TAKEN_DT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Corr_Action_Taken_Dt;
            cmd.Parameters.Add(parameterName: "@PRECLUDE_ACTION", sqlDbType: SqlDbType.VarChar, size: -1).Value = Preclude_Action;
            cmd.Parameters.Add(parameterName: "@PRECLUDE_ACTION_DT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Preclude_Action_Dt;
            cmd.Parameters.Add(parameterName: "@ACTION_TAKEN_BY_USR_ID", sqlDbType: SqlDbType.VarChar, size: 100).Value = Action_Taken_By_Usr_Id;
            cmd.Parameters.Add(parameterName: "@RESPONSE_DT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Response_Dt;
            cmd.Parameters.Add(parameterName: "@DUE_DT_EXT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Due_Dt_Ext;
            cmd.Parameters.Add(parameterName: "@REISSUED_TO_USR_ID", sqlDbType: SqlDbType.VarChar, size: 100).Value = Reissued_To_Usr_Id;
            cmd.Parameters.Add(parameterName: "@REISSUED_DT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Reissued_Dt;
            cmd.Parameters.Add(parameterName: "@RECEIVED_DT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Received_Dt;
            cmd.Parameters.Add(parameterName: "@FOLLOW_UP_REQD", sqlDbType: SqlDbType.VarChar, size: 10).Value = Follow_Up_Reqd;
            cmd.Parameters.Add(parameterName: "@FOLLOW_UP_DT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Follow_Up_Dt;
            cmd.Parameters.Add(parameterName: "@VERIFY_DT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Verify_Dt;
            cmd.Parameters.Add(parameterName: "@VERIFIED_BY_USR_ID", sqlDbType: SqlDbType.VarChar, size: 100).Value = Verify_By_Usr_Id;
            cmd.Parameters.Add(parameterName: "@RESPONSE_ACCEPT_BY_USR_ID", sqlDbType: SqlDbType.VarChar, size: 100).Value = Response_Accept_By_Usr_Id;
            ////cmd.Parameters.Add(parameterName: "@CLOSE_DT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Close_Dt;
            cmd.Parameters.Add(parameterName: "@REMARKS", sqlDbType: SqlDbType.VarChar, size: -1).Value = Remarks;
            cmd.Parameters.Add(parameterName: "@LST_UPDT_BY", sqlDbType: SqlDbType.VarChar, size: 100).Value = Last_Updt_by;
            cmd.Parameters.Add(parameterName: "@SIMILAR_INSTANCE_Y_N", sqlDbType: SqlDbType.VarChar, size: 5).Value = Similar_Instance_Y_N;
            ////cmd.Parameters.Add(parameterName: "@Day_29_Notify_Sent", sqlDbType: SqlDbType.VarChar, size: -1).Value = Day_29_Notify_Sent;
            ////cmd.Parameters.Add(parameterName: "@Day_29_Notify_Dt", sqlDbType: SqlDbType.VarChar, size: 50).Value = Day_29_Notify_Dt;
            ////cmd.Parameters.Add(parameterName: "@Day_15_Notify_Sent", sqlDbType: SqlDbType.VarChar, size: -1).Value = Day_15_Notify_Sent;
            ////cmd.Parameters.Add(parameterName: "@Day_15_Notify_Dt", sqlDbType: SqlDbType.VarChar, size: 50).Value = Day_15_Notify_Dt;
            ////cmd.Parameters.Add(parameterName: "@Day_1_Notify_Sent", sqlDbType: SqlDbType.VarChar, size: -1).Value = Day_1_Notify_Sent;
            ////cmd.Parameters.Add(parameterName: "@Day_1_Notify_Dt", sqlDbType: SqlDbType.VarChar, size: 50).Value = Day_1_Notify_Dt;

            SqlParameter paramResult;
            paramResult = new SqlParameter(parameterName: "@Result", dbType: SqlDbType.VarChar, size: -1)
            {
                Value = string.Empty,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(paramResult);


            try
            {
                cmd.Connection = conn;

                conn.Open();
                SqlDataReader myReader = cmd.ExecuteReader();
                uReturn = paramResult.Value.ToString();

                myReader.Dispose();
            }
            catch (SqlException sqlex)
            {
                LogException(sqlex);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        return uReturn;
    }

    /// <summary>
    /// Deletes EFFECTIVENESS_VALIDATIONS_PICKED records for this CAR OID and list
    /// of supplied eff val description OIDs supplied.  Supply it the full list of
    /// possible EFD OIDs that could ever exist
    /// </summary>
    /// <param name="CarOID"></param>
    /// <param name="EffValDescOids"></param>
    /// <returns></returns>
    public string ExecDeleteEV(params int[] EffValDescOids)
    {
        List<string> uReturn = new List<string>();

        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[SP_DELETE_CAR_EV]"
            };
            cmd.Parameters.Add(parameterName: "@CORR_ACTION_REQUEST_OID", sqlDbType: SqlDbType.Int, size: 0).Value = this.Car_Oid;
            cmd.Parameters.Add(parameterName: "@EFF_VALIDATION_DESCRIPTION_OID", sqlDbType: SqlDbType.Int, size: 0);
            SqlParameter paramResult;
            paramResult = new SqlParameter(parameterName: "@Result", dbType: SqlDbType.VarChar, size: -1)
            {
                Value = string.Empty,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(paramResult);


            try
            {
                cmd.Connection = conn;

                conn.Open();
                foreach (int efdOID in EffValDescOids)
                {
                    cmd.Parameters["@EFF_VALIDATION_DESCRIPTION_OID"].Value = efdOID;
                    cmd.ExecuteNonQuery();
                    uReturn.Add(paramResult.Value.ToString());
                }

            }
            catch (SqlException sqlex)
            {
                LogException(sqlex);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        return uReturn.Count == 0 ? "Success" : string.Join("\r\n", uReturn);

    }

    /// <summary>
    /// Insert EFFECTIVENESS_VALIDATIONS_PICKED records for this CAR OID and list
    /// of supplied eff val description OIDs supplied.  Supply it only the list
    /// of EFD OIDs you want inserted
    /// </summary>
    /// <param name="CarOID"></param>
    /// <param name="EffValDescOids"></param>
    /// <returns></returns>
    public string ExecUpdateEV(params int[] EffValDescOids)
    {
        List<string> uReturn = new List<string>();

        using (SqlConnection conn = new SqlConnection(ConStr))
        {
            SqlCommand cmd;
            cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[SP_INSERT_EFFECTIVENESS_VALIDATIONS_PICKED]"
            };
            cmd.Parameters.Add(parameterName: "@CORR_ACTION_REQUEST_OID", sqlDbType: SqlDbType.Int, size: 0).Value = this.Car_Oid;
            cmd.Parameters.Add(parameterName: "@EFF_VALIDATION_DESCRIPTION_OID", sqlDbType: SqlDbType.Int, size: 0);
            SqlParameter paramResult;
            paramResult = new SqlParameter(parameterName: "@Result", dbType: SqlDbType.VarChar, size: -1)
            {
                Value = string.Empty,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(paramResult);


            try
            {
                cmd.Connection = conn;

                conn.Open();
                foreach (int efdOID in EffValDescOids)
                {
                    cmd.Parameters["@EFF_VALIDATION_DESCRIPTION_OID"].Value = efdOID;
                    cmd.ExecuteNonQuery();
                    uReturn.Add(paramResult.Value.ToString());
                }

            }
            catch (SqlException sqlex)
            {
                LogException(sqlex);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        return uReturn.Count == 0 ? "Success" : string.Join("\r\n", uReturn);

    }

    ////////public string ExecUpdateCarCompleteLater()
    ////////{
    ////////    string uReturn = string.Empty;

    ////////    using (SqlConnection conn = new SqlConnection(ConStr))
    ////////    {
    ////////        SqlCommand cmd;
    ////////        cmd = new SqlCommand();
    ////////        cmd.CommandType = CommandType.StoredProcedure;
    ////////        cmd.CommandText = "[SP_UPDATE_CAR_COMPLETE_LATER]";
    ////////        cmd.Parameters.Add(parameterName: "@OID", sqlDbType: SqlDbType.Int, size: 0).Value = Car_Oid;
    ////////        cmd.Parameters.Add(parameterName: "@CAR_STATUS_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Car_Status_Oid;
    ////////        cmd.Parameters.Add(parameterName: "@AREA_DESCRIPT_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Area_Descript_Oid;
    ////////        cmd.Parameters.Add(parameterName: "@DATE_ISSUED", sqlDbType: SqlDbType.VarChar, size: 50).Value = Date_Issued;
    ////////        cmd.Parameters.Add(parameterName: "@ISSUED_TO_USR_ID", sqlDbType: SqlDbType.VarChar, size: 50).Value = Issued_To_Usr_Id;
    ////////        cmd.Parameters.Add(parameterName: "@DUE_DT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Due_dt;
    ////////        cmd.Parameters.Add(parameterName: "@ORIGINATOR_USR_ID", sqlDbType: SqlDbType.VarChar, size: 50).Value = Originator_Usr_Id;
    ////////        cmd.Parameters.Add(parameterName: "@FACILITY_NAME_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Facility_Name_Oid;
    ////////        cmd.Parameters.Add(parameterName: "@LOC_SUPPLIER", sqlDbType: SqlDbType.VarChar, size: 50).Value = Loc_Supplier;
    ////////        cmd.Parameters.Add(parameterName: "@VNDR_NBR", sqlDbType: SqlDbType.VarChar, size: 20).Value = Vndr_Nbr;
    ////////        cmd.Parameters.Add(parameterName: "@VENDOR_NM", sqlDbType: SqlDbType.VarChar, size: 100).Value = Vendor_Nm;
    ////////        cmd.Parameters.Add(parameterName: "@FINDING_DESC", sqlDbType: SqlDbType.VarChar, size: -1).Value = Finding_Desc;
    ////////        cmd.Parameters.Add(parameterName: "@FINDING_TYPE_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Finding_Type_Oid;
    ////////        cmd.Parameters.Add(parameterName: "@PSL_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Psl_Oid;
    ////////        cmd.Parameters.Add(parameterName: "@DESC_OF_IMPROVEMENT", sqlDbType: SqlDbType.VarChar, size: -1).Value = Desc_Of_Improvement;
    ////////        cmd.Parameters.Add(parameterName: "@CATEGORY_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Category_Oid;
    ////////        cmd.Parameters.Add(parameterName: "@LOC_COUNTRY_OID", sqlDbType: SqlDbType.Int, size: 0).Value = Loc_Country_Oid;
    ////////        cmd.Parameters.Add(parameterName: "@AUDIT_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Audit_Nbr;
    ////////        cmd.Parameters.Add(parameterName: "@QNOTE_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Qnote_Nbr;
    ////////        cmd.Parameters.Add(parameterName: "@CPI_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Cpi_Nbr;
    ////////        cmd.Parameters.Add(parameterName: "@MATERIAL_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Material_Nbr;
    ////////        cmd.Parameters.Add(parameterName: "@PURCHASE_ORDER_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Purchase_Order_Nbr;
    ////////        cmd.Parameters.Add(parameterName: "@PRODUCTION_ORDER_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Production_Order_Nbr;
    ////////        cmd.Parameters.Add(parameterName: "@API_AUDIT_NBR", sqlDbType: SqlDbType.VarChar, size: 50).Value = Api_Audit_Nbr;

    ////////        cmd.Parameters.Add(parameterName: "@RESP_PERSON_USR_ID", sqlDbType: SqlDbType.VarChar, size: 100).Value = Resp_Person_Usr_Id;
    ////////        cmd.Parameters.Add(parameterName: "@NON_CONFORM_RSN", sqlDbType: SqlDbType.VarChar, size: -1).Value = Non_Conform_Rsn;
    ////////        cmd.Parameters.Add(parameterName: "@ROOT_CAUSE", sqlDbType: SqlDbType.VarChar, size: -1).Value = Root_Cause;
    ////////        cmd.Parameters.Add(parameterName: "@SIMILAR_INSTANCE", sqlDbType: SqlDbType.VarChar, size: -1).Value = Similar_Instance;
    ////////        cmd.Parameters.Add(parameterName: "@CORR_ACTION_TAKEN", sqlDbType: SqlDbType.VarChar, size: -1).Value = Corr_Action_Taken;
    ////////        cmd.Parameters.Add(parameterName: "@CORR_ACTION_TAKEN_DT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Corr_Action_Taken_Dt;
    ////////        cmd.Parameters.Add(parameterName: "@PRECLUDE_ACTION", sqlDbType: SqlDbType.VarChar, size: -1).Value = Preclude_Action;
    ////////        cmd.Parameters.Add(parameterName: "@PRECLUDE_ACTION_DT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Preclude_Action_Dt;
    ////////        cmd.Parameters.Add(parameterName: "@ACTION_TAKEN_BY_USR_ID", sqlDbType: SqlDbType.VarChar, size: 100).Value = Action_Taken_By_Usr_Id;
    ////////        cmd.Parameters.Add(parameterName: "@RESPONSE_DT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Response_Dt;
    ////////        cmd.Parameters.Add(parameterName: "@DUE_DT_EXT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Due_Dt_Ext;
    ////////        cmd.Parameters.Add(parameterName: "@REISSUED_TO_USR_ID", sqlDbType: SqlDbType.VarChar, size: 100).Value = Reissued_To_Usr_Id;
    ////////        cmd.Parameters.Add(parameterName: "@REISSUED_DT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Reissued_Dt;
    ////////        cmd.Parameters.Add(parameterName: "@RECEIVED_DT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Received_Dt;
    ////////        cmd.Parameters.Add(parameterName: "@FOLLOW_UP_REQD", sqlDbType: SqlDbType.VarChar, size: 10).Value = Follow_Up_Reqd;
    ////////        cmd.Parameters.Add(parameterName: "@FOLLOW_UP_DT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Follow_Up_Dt;
    ////////        cmd.Parameters.Add(parameterName: "@VERIFY_DT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Verify_Dt;
    ////////        cmd.Parameters.Add(parameterName: "@VERIFIED_BY_USR_ID", sqlDbType: SqlDbType.VarChar, size: 100).Value = Verify_By_Usr_Id;
    ////////        cmd.Parameters.Add(parameterName: "@RESPONSE_ACCEPT_BY_USR_ID", sqlDbType: SqlDbType.VarChar, size: 100).Value = Response_Accept_By_Usr_Id;
    ////////        cmd.Parameters.Add(parameterName: "@CLOSE_DT", sqlDbType: SqlDbType.VarChar, size: 50).Value = Close_Dt;
    ////////        cmd.Parameters.Add(parameterName: "@REMARKS", sqlDbType: SqlDbType.VarChar, size: -1).Value = Remarks;
    ////////        cmd.Parameters.Add(parameterName: "@LST_UPDT_BY", sqlDbType: SqlDbType.VarChar, size: 100).Value = Last_Updt_by;
    ////////        cmd.Parameters.Add(parameterName: "@SIMILAR_INSTANCE_Y_N", sqlDbType: SqlDbType.VarChar, size: 5).Value = Similar_Instance_Y_N;

    ////////        SqlParameter paramResult;
    ////////        paramResult = new SqlParameter(parameterName: "@Result", dbType: SqlDbType.VarChar, size: -1);
    ////////        paramResult.Value = string.Empty;
    ////////        paramResult.Direction = ParameterDirection.Output;
    ////////        cmd.Parameters.Add(paramResult);


    ////////        try
    ////////        {
    ////////            cmd.Connection = conn;

    ////////            conn.Open();
    ////////            SqlDataReader myReader = cmd.ExecuteReader();
    ////////            uReturn = paramResult.Value.ToString();

    ////////            myReader.Dispose();
    ////////        }
    ////////        catch (SqlException sqlex)
    ////////        {
    ////////            LogException(sqlex);
    ////////        }
    ////////        finally
    ////////        {
    ////////            cmd.Dispose();
    ////////        }
    ////////    }

    ////////    return uReturn;
    ////////}

    // Helper routine that logs SqlException details to the
    // Application event log
    private void LogException(SqlException sqlex)
    {
        string strSource = "Internal_Class_UpdateRecord";
        string strMessageAll;
        strMessageAll = "Exception Number : " + sqlex.Number + "(" + sqlex.Message + ") has occurred";

        SqlLog logIt = new SqlLog
        {
            MessageAll = strMessageAll.ToString()
        };
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