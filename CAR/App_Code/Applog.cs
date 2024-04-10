using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class Applog
{
    public string ExceptionMsg { set; get; }
    public string ExceptionType { set; get; }
    public string ExceptionSource { set; get; }
    public string ExceptionUrl { set; get; }
    public string ExceptionTargetSite { set; get; }
    public string ExceptionStackTrace { set; get; }

    public void AppLogEvent()
    {

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
        {
            conn.Open();

            SqlCommand cmd;
            cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[spAppLogException]",
                Connection = conn
            };

            cmd.Parameters.AddWithValue(parameterName: "@ExceptionMsg", value: ExceptionMsg);
            cmd.Parameters.AddWithValue(parameterName: "@ExceptionType", value: ExceptionType);
            cmd.Parameters.AddWithValue(parameterName: "@ExceptionSource", value: ExceptionSource);
            cmd.Parameters.AddWithValue(parameterName: "@ExceptionUrl", value: ExceptionUrl);
            cmd.Parameters.AddWithValue(parameterName: "@ExceptionTargetSite", value: ExceptionTargetSite);
            cmd.Parameters.AddWithValue(parameterName: "@ExceptionStackTrace", value: ExceptionStackTrace);

            cmd.ExecuteNonQuery();
        }
    }
}