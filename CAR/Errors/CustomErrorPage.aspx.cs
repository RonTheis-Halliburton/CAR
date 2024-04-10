using System;
using System.Configuration;
namespace CAR.Errors
{
    public partial class CustomErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Title = "Unexpected site error";
                this.Label1.Text = "We are sorry... An unexpected site error has occured.";
                this.Label2.Text = "The problem has been logged and will be investigated.";
                this.Label3.Text = "If problem persists, please contact the administrator: " + ConfigurationManager.AppSettings["AdminContact"];
                this.Label4.Text = "Thank you for your patience.";
            }
        }
    }
}