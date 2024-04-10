using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class SqlLog
{
    public string Number { set; get; }
    public string Procedure { set; get; }
    public string Server { set; get; }
    public string State { set; get; }
    public string Source { set; get; }
    public string Serverity { set; get; }
    public string Message { set; get; }
    public string LineNumber { set; get; }
    public string MessageAll { set; get; }


    public void LogSqlEvent()
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
        {
            SqlCommand cmd;
            cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[spSQLEventLog]";
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue(parameterName: "@Number", value: Number);
            cmd.Parameters.AddWithValue(parameterName: "@Procedure", value: Procedure);
            cmd.Parameters.AddWithValue(parameterName: "@Server", value: Server);
            cmd.Parameters.AddWithValue(parameterName: "@State", value: State);
            cmd.Parameters.AddWithValue(parameterName: "@Source", value: Source);
            cmd.Parameters.AddWithValue(parameterName: "@Serverity", value: Serverity);
            cmd.Parameters.AddWithValue(parameterName: "@Message", value: Message);
            cmd.Parameters.AddWithValue(parameterName: "@LineNumber", value: LineNumber);
            cmd.Parameters.AddWithValue(parameterName: "@MessageAll", value: MessageAll);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}