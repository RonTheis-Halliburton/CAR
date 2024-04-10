using System;
using System.Web;

/// <summary>
/// Summary description for MySession
/// </summary>
/// 
[Serializable]
public class MySession
{
    // **** add your session properties here:
    //public string ssLogged { get; set; }
    public string SessionUserID { get; set; }
    //public string SessionUserOid { get; set; }
    public string SessionDomainName { get; set; }
    public string SessionFirstName { get; set; }
    public string SessionLastName { get; set; }
    public string SessionFullName { get; set; }
    public string SessionEmail { get; set; }
    public bool SessionCarAdmin { get; set; }
    public bool SessionSystemAdmin { get; set; }
    public string SessionUserCity { get; set; }
    public string SessionUserCountry { get; set; }
    public string SessionUrlForm { get; set; }
    public string SessionUrlNdex { get; set; }
    public string SessionWebServer { get; set; }
    public string SessionHostName { get; set; }
    public string SessionManager { get; set; }
    public string SessionManagerID { get; set; }
    public string SessionManagerEmail { get; set; }

    public MySession()
    {
        SessionUserID = string.Empty;
        SessionDomainName = string.Empty;
        SessionFirstName = string.Empty;
        SessionLastName = string.Empty;
        SessionFullName = string.Empty;
        SessionEmail = string.Empty;
        SessionCarAdmin = false;
        SessionSystemAdmin = false;
        SessionWebServer = string.Empty;
        SessionHostName = string.Empty;
        SessionUserCity = string.Empty;
        SessionUserCountry = string.Empty;
        SessionUrlForm = string.Empty;
        SessionUrlNdex = string.Empty;
        SessionManager = string.Empty;
        SessionManagerID = string.Empty;
        SessionManagerEmail = string.Empty;
    }

    // Gets the current session.
    public static MySession Current
    {
        get
        {
            MySession session;
            session = (MySession)HttpContext.Current.Session["__MySession__"];
            if (session == null)
            {
                session = new MySession();
                HttpContext.Current.Session["__MySession__"] = session;
            }
            return session;
        }
    }

}