using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CAR.Forms
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly SqlDataAdapter Sda = new SqlDataAdapter();
        private readonly string ConStr = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
               
        private static readonly string v = ConfigurationManager.AppSettings["smptHostMail"].ToString();
        private readonly string smtpHost = v;
        private static readonly string v1 = ConfigurationManager.AppSettings["NoReplyContact"].ToString();
        private readonly string noReplyContact = v1;
        private readonly string HsnPortal = ConfigurationManager.AppSettings["HsnPortal"].ToString();
        private static readonly string v2 = ConfigurationManager.AppSettings["TestVendorName"].ToString();
        private readonly string TestVendorName = v2;
        private static readonly string v3 = ConfigurationManager.AppSettings["TestVendorUserID"].ToString();
        private readonly string TestVendorUserID = v3;
        private readonly string TestVendorCity = ConfigurationManager.AppSettings["TestVendorCity"].ToString();
        private readonly string TestVendorCountry = ConfigurationManager.AppSettings["TestVendorCountry"].ToString();

        private readonly string AdminContact = ConfigurationManager.AppSettings["AdminContact"].ToString();
        private readonly string ErrException = "If problem persists, please contact the administrator: " + ConfigurationManager.AppSettings["AdminContact"];
        private readonly DateTime DefaultDate = DateTime.Parse("1/1/1900 12:00:00 AM");
        private readonly SanitizeString removeChar = new SanitizeString();

        private readonly MyUtil.HtmlUtility htmlUtil = MyUtil.HtmlUtility.Instance;

        private const int ItemsPerRequest = 100;

        public string SesUsrId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (MySession.Current.SessionUserID == null || string.IsNullOrEmpty(MySession.Current.SessionUserID))
            {
                Session.Abandon();
                string script = "GetParentPage();";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", script, true);
            }
            else
            {
               SesUsrId = MySession.Current.SessionUserID.ToString().ToUpper();
            }

            if (!Page.IsPostBack)
            {
                try
                {
                    if (Request.QueryString["OID"] != null && Request.QueryString["CAR_NBR"] != null && Request.QueryString["Status_NM"] != null)
                    {
                        Header.Title = Server.HtmlEncode(htmlUtil.SanitizeHtml("Edit - " + Request.QueryString["CAR_NBR"].ToString() + "     [Status:  " + Request.QueryString["Status_NM"].ToString() + "]"));

                        RadTabStrip1.Tabs[0].Visible = true;
                        RadMultiPage1.PageViews[0].Visible = true;

                        LoadDefaultData();

                        LoadExistingCar(Request.QueryString["OID"].ToString());
                    }
                    else
                    {
                        Header.Title = "Error - Unable to locate CAR data.  " + ErrException;
                    }

                    ////Removed tabs below per David Dimerson - 2/19/2020
                    RadTabStrip1.Tabs[1].Visible = false;
                    RadMultiPage1.PageViews[1].Visible = false;

                    RadTabStrip1.Tabs[2].Visible = false;
                    RadMultiPage1.PageViews[2].Visible = false;
                    ////###################

                }
                catch (Exception ex)
                {
                    LogException(ex);

                    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
                }
            }
        }

        protected void LoadDefaultData()
        {
            Label1.Text = "0";
            Label2.Text = "0";
            Label3.Text = "0";

            RadTextBox1.Text = string.Empty;
            RadTextBox2.Text = string.Empty;
            RadTextBox3.Text = string.Empty;
            RadTextBox4.Text = string.Empty;
            RadTextBox5.Text = string.Empty;
            RadTextBox6.Text = string.Empty;
            RadTextBox8.Text = string.Empty;
            RadTextBox9.Text = string.Empty;
            RadTextBox10.Text = string.Empty;
            RadTextBox12.Text = string.Empty;
            RadTextBox13.Text = string.Empty;

            RadTextBox11.Text = string.Empty;
            RadTextBox14.Text = string.Empty;

            RadComboBox3.Items.Clear();
            RadComboBox3.ClearSelection();
            RadComboBox3.Text = string.Empty;

            RadComboBox10.Text = string.Empty;
            RadComboBox10.Items.Clear();
            RadComboBox10.ClearSelection();

            LoadDefaultRecipient(RadComboBox3);
            LoadFindingType(RadComboBox9);

            LoadArea(RadComboBox2);
            LoadFacility(RadComboBox5);
            LoadPsl(RadComboBox4);
            LoadApi_Iso(RadComboBox6);
            LoadCountry(RadComboBox10);
            LoadCategory(RadComboBox12);

            LoadStatus(RadComboBox20);
            LoadEV(RadComboBox21);

            HiddenOriginatorUserId.Value = MySession.Current.SessionUserID.ToString().ToUpper();
            HiddenOriginatorUserName.Value = MySession.Current.SessionFullName.ToString();
            HiddenOriginatorUserEmail.Value = MySession.Current.SessionEmail.ToString();

            HiddenIssuedToUserId.Value = string.Empty;
            HiddenIssuedToUserName.Value = string.Empty;
            HiddenIssuedToUserEmail.Value = string.Empty;

            HiddenRecipientId.Value = string.Empty;
            HiddenRecipientName.Value = string.Empty;
            HiddenRecipientEmail.Value = string.Empty;

            HiddenVendorNumber.Value = string.Empty;
            HiddenVendorName.Value = string.Empty;

            HiddenCountryOid.Value = "0";
            HiddenAreaOid.Value = "0";

            RadComboBox11.Text = string.Empty;
            RadComboBox11.Items.Clear();
            RadComboBox11.ClearSelection();

            HiddenVendorNumber.Value = string.Empty;
            HiddenVendorName.Value = string.Empty;

            RadComboBox7.Text = string.Empty;
            RadComboBox7.Items.Clear();
            RadComboBox7.ClearSelection();

            HiddenRecipientId.Value = string.Empty;
            HiddenRecipientName.Value = string.Empty;
            HiddenRecipientEmail.Value = string.Empty;

            RadListBox1.Items.Clear(); //** HES Recipients
            RadListBox2.Items.Clear(); // ** Supplier Recipients
            RadListBox3.Items.Clear();// ** Notification sent to

            Label3.Text = string.Empty;

            RadDatePicker1.MinDate = DateTime.Now.AddMonths(-12);
            RadDatePicker1.SelectedDate = DateTime.Today;

            RadDatePicker2.MinDate = DateTime.Today.AddDays(-1);
            RadDatePicker2.SelectedDate = DateTime.Today.AddDays(30);

            //this.Panel2.Visible = false;


            //RadTab tabVendor = this.RadTabStrip1.FindTabByText("Vendor");
            //RadTab tabIssuedTo = this.RadTabStrip1.FindTabByText("Issued To");
            //RadTab tabNotification = this.RadTabStrip1.FindTabByText("Notification");
            //RadTab tabFinish = this.RadTabStrip1.FindTabByText("Finish");
            //tabVendor.Enabled = tabIssuedTo.Enabled = tabNotification.Enabled = tabFinish.Enabled = false;
        }

        protected void LoadExistingCar(string CarOid)
        {

            GetRecord xCar;
            xCar = new GetRecord
            {
                CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(CarOid)))
            };

            using (DataTable dt = xCar.Exec_Get_Existing_Car_By_Oid_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    Label1.Text = Server.HtmlEncode(htmlUtil.SanitizeHtml(CarOid));
                    Label2.Text = Server.HtmlEncode(htmlUtil.SanitizeHtml(row["CAR_NBR"].ToString()));
                    Label3.Text = Server.HtmlEncode(htmlUtil.SanitizeHtml(row["CAR_NBR"].ToString()));

                    SelectComboBoxByValue(RadComboBox9, row["FINDING_TYPE_OID"].ToString());
                    SelectComboBoxByValue(RadComboBox2, row["AREA_DESCRIPT_OID"].ToString());
                    SelectComboBoxByValue(RadComboBox5, row["FACILITY_NAME_OID"].ToString());
                    SelectComboBoxByValue(RadComboBox4, row["PSL_OID"].ToString());
                    SelectComboBoxByValue(RadComboBox10, row["LOC_COUNTRY_OID"].ToString());
                    SelectComboBoxByValue(RadComboBox12, row["CATEGORY_OID"].ToString());

                    GetRecord api;
                    api = new GetRecord
                    {
                        CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(CarOid)))
                    };

                    using (DataTable dtApi = api.Exec_GetCar_Api_Iso_Datatable())
                    {
                        foreach (DataRow rowApi in dtApi.Rows)
                        {
                            RadComboBoxItem itemFound;
                            itemFound = RadComboBox6.FindItemByValue(rowApi["API_ISO_ELEMENTS_OID"].ToString());

                            if (itemFound != null)
                            {
                                itemFound.Checked = true;

                                RadComboBoxItem item;
                                item = new RadComboBoxItem();

                                item.Value = itemFound.Value;
                                item.Text = itemFound.Text;
                            }
                        }
                    }

                    RadTextBox1.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["AUDIT_NBR"].ToString()));
                    RadTextBox2.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["QNOTE_NBR"].ToString()));
                    RadTextBox3.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["CPI_NBR"].ToString()));
                    RadTextBox4.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["MATERIAL_NBR"].ToString()));
                    RadTextBox5.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["PURCHASE_ORDER_NBR"].ToString()));
                    RadTextBox6.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["PRODUCTION_ORDER_NBR"].ToString()));
                    RadTextBox8.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["API_AUDIT_NBR"].ToString()));
                    RadTextBox10.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["LOC_SUPPLIER"].ToString()));

                    RadTextBox12.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["FINDING_DESC"].ToString()));

                    RadTextBox22.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["MAINTENANCE_ORDER_NBR"].ToString()));
                    RadTextBox23.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["EQUIPMENT_NBR"].ToString()));

                    //Only Originator can update Finding Descr.  If not originator, reload original Finding Descr.
                    if (row["ORIGINATOR_USR_ID"].ToString().Trim().ToUpper() != MySession.Current.SessionUserID.Trim().ToUpper() ||
                         MySession.Current.SessionCarAdmin == true)
                    {
                        RadTextBox12.ReadOnly = true;
                    }

                    //Originator Used Only
                    if (row["ORIGINATOR_USR_ID"].ToString().Trim().ToUpper() == MySession.Current.SessionUserID.Trim().ToUpper())
                         //|| MySession.Current.SessionCarAdmin == true)
                         //|| row["CREATE_BY"].ToString().Trim().ToUpper() == MySession.Current.SessionUserID.Trim().ToUpper()
                         //|| row["RESP_PERSON_USR_ID"].ToString().Trim().ToUpper() == MySession.Current.SessionUserID.Trim().ToUpper())
                    {
                        RadDatePicker6.Enabled = true;
                        RadDatePicker7.Enabled = true;
                        RadDatePicker8.Enabled = true;
                        RadDatePicker9.Enabled = true;
                        RadDatePicker10.Enabled = true;

                        RadComboBox16.Enabled = true;
                        RadComboBox17.Enabled = true;
                        RadComboBox18.Enabled = true;
                        RadComboBox19.Enabled = true;

                        RadComboBox20.Enabled = true;
                        RadComboBox21.Enabled = true;
                        RadTextBox18.Enabled = true;
                        if (RadComboBox20.SelectedValue.ToString() == "3") //Closed
                        {
                            carEV.Visible = true;
                            carEVd.Visible = true;
                        }
                        else
                        {
                            carEV.Visible = false;
                            carEVd.Visible = false;
                        }
                        RadButton34.Enabled = true;
                    }
                    else
                    {
                        RadDatePicker6.Enabled = false;
                        RadDatePicker7.Enabled = false;
                        RadDatePicker8.Enabled = false;
                        RadDatePicker9.Enabled = false;
                        RadDatePicker10.Enabled = false;

                        RadComboBox16.Enabled = false;
                        RadComboBox17.Enabled = false;
                        RadComboBox18.Enabled = false;
                        RadComboBox19.Enabled = false;

                        RadComboBox20.Enabled = false;
                        RadComboBox21.Enabled = false;
                        RadTextBox18.Enabled = false;
                        if (RadComboBox20.SelectedValue.ToString() == "3") //Closed
                        {
                            carEV.Visible = true;
                            carEVd.Visible = true;
                        }
                        else
                        {
                            carEV.Visible = false;
                            carEVd.Visible = false;
                        }
                        RadButton34.Enabled = false;
                    }

                    RadTextBox13.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["DESC_OF_IMPROVEMENT"].ToString()));
                    RadTextBox11.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["NON_CONFORM_RSN"].ToString()));
                    RadTextBox14.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["ROOT_CAUSE"].ToString()));

                    SelectComboBoxByValue(RadComboBox14, row["SIMILAR_INSTANCE_Y_N_CHAR"].ToString());
                    RadTextBox15.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["SIMILAR_INSTANCE"].ToString()));

                    if (RadComboBox14.SelectedValue.ToString().ToUpper() == "Y")
                    {
                        Panel3.Visible = true;
                    }
                    else
                    {
                        Panel3.Visible = false;
                    }

                    RadTextBox16.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["CORR_ACTION_TAKEN"].ToString()));

                    if (htmlUtil.IsDate(row["CORR_ACTION_TAKEN_DT"].ToString()))
                    {
                        if (DateTime.Parse(row["CORR_ACTION_TAKEN_DT"].ToString().Trim()) != DefaultDate)
                        {
                            RadDatePicker3.SelectedDate = DateTime.Parse(row["CORR_ACTION_TAKEN_DT"].ToString());
                        }
                    }

                    RadTextBox17.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["PRECLUDE_ACTION"].ToString()));

                    if (htmlUtil.IsDate(row["PRECLUDE_ACTION_DT"].ToString()))
                    {
                        if (DateTime.Parse(row["PRECLUDE_ACTION_DT"].ToString().Trim()) != DefaultDate)
                        {
                            RadDatePicker4.SelectedDate = DateTime.Parse(row["PRECLUDE_ACTION_DT"].ToString());
                        }
                    }

                    if (htmlUtil.IsDate(row["RESPONSE_DT"].ToString()))
                    {
                        if (DateTime.Parse(row["RESPONSE_DT"].ToString().Trim()) != DefaultDate)
                        {
                            RadDatePicker5.SelectedDate = DateTime.Parse(row["RESPONSE_DT"].ToString());
                        }
                    }

                    if (htmlUtil.IsDate(row["DATE_ISSUED"].ToString()))
                    {
                        RadDatePicker1.MinDate = DateTime.Parse(row["DATE_ISSUED"].ToString());
                        RadDatePicker1.SelectedDate = DateTime.Parse(row["DATE_ISSUED"].ToString());
                    }

                    if (htmlUtil.IsDate(row["DUE_DT"].ToString()))
                    {
                        RadDatePicker2.MinDate = DateTime.Parse(row["DUE_DT"].ToString());
                        RadDatePicker2.SelectedDate = DateTime.Parse(row["DUE_DT"].ToString());
                    }

                    if (!string.IsNullOrEmpty(row["ORIGINATOR_USR_ID"].ToString()))
                    {
                        LoadHesRecipient(RadComboBox3, htmlUtil.SanitizeHtml(Server.HtmlDecode(row["ORIGINATOR_USR_ID"].ToString())));

                        if (RadComboBox3.SelectedItem != null)
                        {
                            HiddenOriginatorUserId.Value = RadComboBox3.SelectedItem.Attributes["UserID"].ToString().ToUpper();
                            HiddenOriginatorUserName.Value = RadComboBox3.SelectedItem.Attributes["FullName"].ToString();
                            HiddenOriginatorUserEmail.Value = RadComboBox3.SelectedItem.Attributes["Email"].ToString();
                        }
                    }


                    if (int.Parse(row["AREA_DESCRIPT_OID"].ToString()) == 2) //Supplier, Vendor
                    {
                        HiddenVendorNumber.Value = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["VNDR_NBR"].ToString()));
                        HiddenVendorName.Value = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["VENDOR_NM"].ToString()));

                        LoadVendor(RadComboBox11, htmlUtil.SanitizeHtml(Server.HtmlDecode(row["VNDR_NBR"].ToString())));

                        if (!string.IsNullOrEmpty(row["ISSUED_TO_USR_ID"].ToString().Trim()))
                        {

                            ////Manually added recipient
                            ///
                            if (row["ISSUED_TO_USR_ID"].ToString().Trim().ToUpper() == "NON_HSN")
                            {
                                RadComboBoxItem item;
                                item = new RadComboBoxItem();

                                item.Text = row["ISSUED_TO_USR_NM"].ToString().Trim();
                                item.Value = row["ISSUED_TO_USR_ID"].ToString().Trim().ToUpper();
                                item.ToolTip = row["ISSUED_TO_USR_NM"].ToString().Trim();
                                item.Selected = true;
                                item.Attributes["UserId"] = row["ISSUED_TO_USR_ID"].ToString().Trim().ToUpper();
                                item.Attributes["Email"] = row["ISSUED_TO_USR_EMAIL"].ToString().Trim().ToUpper();
                                item.Attributes["FullName"] = row["ISSUED_TO_USR_NM"].ToString().Trim();
                                item.Attributes["VendorNumber"] = row["VNDR_NBR"].ToString().Trim();

                                RadComboBox7.Items.Add(item);
                                RadComboBox7.SortItems();

                                RadTextBox7.Text = row["ISSUED_TO_USR_EMAIL"].ToString().Trim().ToUpper();

                            }
                            else
                            {
                                LoadVendorRecipient(RadComboBox7, htmlUtil.SanitizeHtml(Server.HtmlDecode(row["ISSUED_TO_USR_ID"].ToString())), htmlUtil.SanitizeHtml(Server.HtmlDecode(row["VNDR_NBR"].ToString())));
                            }
                        }

                        if (!string.IsNullOrEmpty(row["ACTION_TAKEN_BY_USR_ID"].ToString()))
                        {
                            LoadVendorRecipient(RadComboBox15, htmlUtil.SanitizeHtml(Server.HtmlDecode(row["ACTION_TAKEN_BY_USR_ID"].ToString())), htmlUtil.SanitizeHtml(Server.HtmlDecode(row["VNDR_NBR"].ToString())));
                        }

                        if (!string.IsNullOrEmpty(row["RESP_PERSON_USR_ID"].ToString()))
                        {
                            LoadVendorRecipient(RadComboBox13, htmlUtil.SanitizeHtml(Server.HtmlDecode(row["RESP_PERSON_USR_ID"].ToString())), htmlUtil.SanitizeHtml(Server.HtmlDecode(row["VNDR_NBR"].ToString())));

                            if (string.IsNullOrEmpty(RadComboBox13.Text.Trim()))
                            {
                                LoadHesRecipient(RadComboBox13, htmlUtil.SanitizeHtml(Server.HtmlDecode(row["RESP_PERSON_USR_ID"].ToString())));
                            }
                        }

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(row["ACTION_TAKEN_BY_USR_ID"].ToString()))
                        {
                            LoadHesRecipient(RadComboBox15, htmlUtil.SanitizeHtml(Server.HtmlDecode(row["ACTION_TAKEN_BY_USR_ID"].ToString())));
                        }

                        if (!string.IsNullOrEmpty(row["ISSUED_TO_USR_ID"].ToString().Trim()))
                        {
                            LoadHesRecipient(RadComboBox7, htmlUtil.SanitizeHtml(Server.HtmlDecode(row["ISSUED_TO_USR_ID"].ToString())));
                        }

                        if (!string.IsNullOrEmpty(row["RESP_PERSON_USR_ID"].ToString().Trim()))
                        {
                            LoadHesRecipient(RadComboBox13, htmlUtil.SanitizeHtml(Server.HtmlDecode(row["RESP_PERSON_USR_ID"].ToString())));
                        }

                    }

                    if (RadComboBox13.SelectedItem != null)
                    {
                        HiddenResponsibleUserId.Value = RadComboBox13.SelectedItem.Attributes["UserID"].ToString().ToUpper();
                        HiddenResponsibleUserName.Value = RadComboBox13.SelectedItem.Attributes["FullName"].ToString();
                        HiddenResponsibleUserEmail.Value = RadComboBox13.SelectedItem.Attributes["Email"].ToString();
                    }

                    if (RadComboBox15.SelectedItem != null)
                    {
                        HiddenActionTakenUserId.Value = RadComboBox15.SelectedItem.Attributes["UserID"].ToString().ToUpper();
                        HiddenActionTakenUserName.Value = RadComboBox15.SelectedItem.Attributes["FullName"].ToString();
                        HiddenActionTakenUserEmail.Value = RadComboBox15.SelectedItem.Attributes["Email"].ToString();
                    }

                    if (RadComboBox7.SelectedItem != null)
                    {
                        HiddenIssuedToUserId.Value = RadComboBox7.SelectedItem.Attributes["UserID"].ToString().ToUpper();
                        HiddenIssuedToUserName.Value = RadComboBox7.SelectedItem.Attributes["FullName"].ToString();
                        HiddenIssuedToUserEmail.Value = RadComboBox7.SelectedItem.Attributes["Email"].ToString();
                    }

                    //++++++  Origniator Use Only
                    if (htmlUtil.IsDate(row["DUE_DT_EXT"].ToString()))
                    {
                        if (DateTime.Parse(row["DUE_DT_EXT"].ToString().Trim()) != DefaultDate)
                        {
                            RadDatePicker6.SelectedDate = DateTime.Parse(row["DUE_DT_EXT"].ToString());
                        }
                    }

                    if (!string.IsNullOrEmpty(row["REISSUED_TO_USR_ID"].ToString()))
                    {
                        LoadHesRecipient(RadComboBox16, htmlUtil.SanitizeHtml(Server.HtmlDecode(row["REISSUED_TO_USR_ID"].ToString())));

                        if (RadComboBox16.SelectedItem != null)
                        {
                            HiddenReIssuedUserId.Value = RadComboBox16.SelectedItem.Attributes["UserID"].ToString().ToUpper();
                            HiddenReIssuedUserName.Value = RadComboBox16.SelectedItem.Attributes["FullName"].ToString();
                            HiddenReIssuedUserEmail.Value = RadComboBox16.SelectedItem.Attributes["Email"].ToString();
                        }
                    }

                    if (htmlUtil.IsDate(row["REISSUED_DT"].ToString()))
                    {
                        if (DateTime.Parse(row["REISSUED_DT"].ToString().Trim()) != DefaultDate)
                        {
                            RadDatePicker7.SelectedDate = DateTime.Parse(row["REISSUED_DT"].ToString());
                        }
                    }

                    if (htmlUtil.IsDate(row["RECEIVED_DT"].ToString()))
                    {
                        if (DateTime.Parse(row["RECEIVED_DT"].ToString().Trim()) != DefaultDate)
                        {
                            RadDatePicker8.SelectedDate = DateTime.Parse(row["RECEIVED_DT"].ToString());
                        }
                    }

                    SelectComboBoxByValue(RadComboBox19, row["FOLLOW_UP_REQD"].ToString());

                    if (htmlUtil.IsDate(row["FOLLOW_UP_DT"].ToString()))
                    {
                        if (DateTime.Parse(row["FOLLOW_UP_DT"].ToString().Trim()) != DefaultDate)
                        {
                            RadDatePicker9.SelectedDate = DateTime.Parse(row["FOLLOW_UP_DT"].ToString());
                        }
                    }

                    if (htmlUtil.IsDate(row["VERIFY_DT"].ToString()))
                    {
                        if (DateTime.Parse(row["VERIFY_DT"].ToString().Trim()) != DefaultDate)
                        {
                            RadDatePicker10.SelectedDate = DateTime.Parse(row["VERIFY_DT"].ToString());
                        }
                    }

                    if (!string.IsNullOrEmpty(row["VERIFIED_BY_USR_ID"].ToString()))
                    {
                        LoadHesRecipient(RadComboBox17, htmlUtil.SanitizeHtml(Server.HtmlDecode(row["VERIFIED_BY_USR_ID"].ToString())));

                        if (RadComboBox17.SelectedItem != null)
                        {
                            HiddenVerifiedByUserId.Value = RadComboBox17.SelectedItem.Attributes["UserID"].ToString().ToUpper();
                            HiddenVerifiedByUserName.Value = RadComboBox17.SelectedItem.Attributes["FullName"].ToString();
                            HiddenVerifiedByUserEmail.Value = RadComboBox17.SelectedItem.Attributes["Email"].ToString();
                        }
                    }

                    if (!string.IsNullOrEmpty(row["RESPONSE_ACCEPT_BY_USR_ID"].ToString()))
                    {
                        LoadHesRecipient(RadComboBox18, htmlUtil.SanitizeHtml(Server.HtmlDecode(row["RESPONSE_ACCEPT_BY_USR_ID"].ToString())));

                        if (RadComboBox18.SelectedItem != null)
                        {
                            HiddenAcceptedByUserId.Value = RadComboBox18.SelectedItem.Attributes["UserID"].ToString().ToUpper();
                            HiddenAcceptedByUserName.Value = RadComboBox18.SelectedItem.Attributes["FullName"].ToString();
                            HiddenAcceptedByUserEmail.Value = RadComboBox18.SelectedItem.Attributes["Email"].ToString();
                        }
                    }

                    SelectComboBoxByValue(RadComboBox20, row["CAR_STATUS_OID"].ToString());

                    if (htmlUtil.IsDate(row["CLOSE_DT"].ToString()))
                    {
                        if (DateTime.Parse(row["CLOSE_DT"].ToString().Trim()) != DefaultDate)
                        {
                            LoadPreviewLiteralValue(40, DateTime.Parse(row["CLOSE_DT"].ToString()).ToShortDateString(), 9, 0);
                        }
                        else
                        {
                            LoadPreviewLiteralValue(40, string.Empty, 9, 0);
                        }
                    }

                    RadTextBox18.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["REMARKS"].ToString()));

                    ExpandTreeviewNode(0);

                    UpdatePreviewStart();
                    UpdatePreviewVendor();
                    UpdatePreviewIssuedTo();

                    UpdatePreviewQuestion(RadTextBox11, 3);
                    UpdatePreviewQuestion(RadTextBox14, 4);
                    UpdatePreviewQuestion(RadTextBox15, 5);
                    UpdatePreviewQuestion(RadTextBox16, 6);
                    UpdatePreviewQuestion(RadTextBox17, 7);

                    UpdatePreviewActionTaken();
                    UpdatePreviewOriginatorUseOnly();

                    RadTabStrip1.Tabs[0].Selected = true;
                    RadTabStrip1.Tabs[0].Enabled = true;
                    RadMultiPage1.SelectedIndex = 0;

                    for (int i = 1; i <= 11; i++)
                    {
                        RadTabStrip1.Tabs[i].Enabled = false;
                    }

                }
            }

            // If the Effectiveness validation combo box is enabled, try to get the current values and
            //   check off the items
            RadComboBoxItem itm = null;
            if (RadComboBox21.Enabled)
            {
                using (DataTable dt = xCar.Exec_Get_Existing_EV_By_Oid_Datatable())
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        itm = RadComboBox21.Items.FindItem(x => x.Value == row["OID"].ToString());
                        itm.Checked = true;
                    }
                }
            }
            //catch (Exception ex)
            //{
            //    LogException(ex);

            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
        }

        protected void RadComboBox20_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (RadComboBox20.SelectedValue.ToString() == "3") //Closed
                {
                    carEV.Visible = true;
                    carEVd.Visible = true;
                }
                else
                {
                    carEV.Visible = false;
                    carEVd.Visible = false;
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            }
        }

        protected void SelectComboBoxByValue(RadComboBox comboBox, string itemValue)
        {
            RadComboBoxItem itemFound;
            itemFound = comboBox.FindItemByValue(itemValue);

            if (itemFound != null)
            {
                itemFound.Selected = true;
            }
        }

        protected void LoadApi_Iso(RadComboBox comboBox)
        {
            //try
            //{
            comboBox.ClearSelection();
            comboBox.Text = string.Empty;
            comboBox.Items.Clear();

            GetRecord getRec;
            getRec = new GetRecord();

            using (DataTable dt = getRec.Exec_GetAPI_ISO_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    RadComboBoxItem item;
                    item = new RadComboBoxItem
                    {
                        Value = row["OID"].ToString().Trim(),
                        Text = row["API_ISO_ELEM"].ToString().Trim(),
                        ToolTip = row["API_ISO_ELEM"].ToString()
                    };

                    comboBox.Items.Add(item);
                }

                //comboBox.Items.Insert(index: 0, item: new RadComboBoxItem(text: "", value: "0"));
                //comboBox.SelectedIndex = 0;
            }
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);

            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
        }

        protected void LoadStatus(RadComboBox radComboBox)
        {
            //try
            //{
            radComboBox.ClearSelection();
            radComboBox.Items.Clear();

            GetRecord getRec;
            getRec = new GetRecord();

            using (DataTable dt = getRec.Exec_GetStatus_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    RadComboBoxItem item;
                    item = new RadComboBoxItem
                    {
                        Value = row["OID"].ToString().Trim(),
                        Text = row["NM"].ToString().Trim(),
                        ToolTip = row["NM"].ToString()
                    };

                    radComboBox.Items.Add(item);
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);

            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
        }

        protected void LoadEV(RadComboBox radComboBox)
        {
            //try
            //{
            radComboBox.ClearSelection();
            radComboBox.Items.Clear();

            GetRecord getRec;
            getRec = new GetRecord();

            using (DataTable dt = getRec.Exec_GetEV_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    RadComboBoxItem item;
                    item = new RadComboBoxItem
                    {
                        Value = row["OID"].ToString().Trim(),
                        Text = row["VALIDATION_DESCRIPTION"].ToString().Trim(),
                        ToolTip = row["VALIDATION_DESCRIPTION"].ToString()
                    };

                    radComboBox.Items.Add(item);
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);

            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
        }

        protected void LoadArea(RadComboBox comboBox)
        {
            //try
            //{
            comboBox.ClearSelection();
            comboBox.Text = string.Empty;
            comboBox.Items.Clear();

            GetRecord getRec;
            getRec = new GetRecord();

            using (DataTable dt = getRec.Exec_GetArea_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    RadComboBoxItem item;
                    item = new RadComboBoxItem
                    {
                        Value = row["OID"].ToString().Trim(),
                        Text = row["AREA_DESCR"].ToString().Trim(),
                        ToolTip = row["AREA_DESCR"].ToString()
                    };
                    comboBox.Items.Add(item);
                }

                comboBox.Items.Insert(index: 0, item: new RadComboBoxItem(text: "", value: "0"));
                comboBox.SelectedIndex = 0;
            }
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);

            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
        }

        protected void LoadCategory(RadComboBox comboBox)
        {
            //try
            //{
            comboBox.ClearSelection();
            comboBox.Text = string.Empty;
            comboBox.Items.Clear();

            GetRecord getRec;
            getRec = new GetRecord();

            using (DataTable dt = getRec.Exec_GetCategory_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    RadComboBoxItem item;
                    item = new RadComboBoxItem
                    {
                        Value = row["OID"].ToString().Trim(),
                        Text = row["CATEGORY_NM"].ToString().Trim(),
                        ToolTip = row["CATEGORY_NM"].ToString()
                    };
                    comboBox.Items.Add(item);
                }

                comboBox.Items.Insert(index: 0, item: new RadComboBoxItem(text: "", value: "0"));
                comboBox.SelectedIndex = 0;
            }
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);

            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
        }

        protected void LoadFindingType(RadComboBox comboBox)
        {
            //try
            //{
            comboBox.ClearSelection();
            comboBox.Text = string.Empty;
            comboBox.Items.Clear();

            GetRecord getRec;
            getRec = new GetRecord();

            using (DataTable dt = getRec.Exec_GetFindingType_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    RadComboBoxItem item;
                    item = new RadComboBoxItem
                    {
                        Value = row["OID"].ToString().Trim(),
                        Text = row["FINDING_TYPE"].ToString().Trim(),
                        ToolTip = row["FINDING_TYPE"].ToString()
                    };
                    comboBox.Items.Add(item);
                }

                comboBox.Items.Insert(index: 0, item: new RadComboBoxItem(text: "", value: "0"));
                comboBox.SelectedIndex = 0;
            }
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);

            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
        }

        protected void LoadCountry(RadComboBox comboBox)
        {

            comboBox.ClearSelection();
            comboBox.Text = string.Empty;
            comboBox.Items.Clear();

            GetRecord getRec;
            getRec = new GetRecord();

            using (DataTable dt = getRec.Exec_GetCountry_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    RadComboBoxItem item;
                    item = new RadComboBoxItem
                    {
                        Value = row["OID"].ToString().Trim(),
                        Text = row["NM"].ToString().Trim(),
                        ToolTip = row["NM"].ToString()
                    };
                    comboBox.Items.Add(item);
                }

                comboBox.Items.Insert(index: 0, item: new RadComboBoxItem(text: "", value: "0"));
                comboBox.SelectedIndex = 0;
            }

        }

        protected void LoadDefaultRecipient(RadComboBox comboBox)
        {
            RadComboBoxItem item;
            item = new RadComboBoxItem();

            item.Font.Bold = true;

            item.Text = MySession.Current.SessionFullName.ToString();
            item.ToolTip = MySession.Current.SessionFullName.ToString();
            item.Value = MySession.Current.SessionUserID.ToString().ToUpper();

            item.Attributes.Add(key: "UserId", value: MySession.Current.SessionUserID.ToString().ToUpper());
            item.Attributes.Add(key: "FullName", value: MySession.Current.SessionFullName.ToString());
            item.Attributes.Add(key: "CITY", value: MySession.Current.SessionUserCity.ToString());
            item.Attributes.Add(key: "CNTRY_NM", value: MySession.Current.SessionUserCountry.ToString());
            item.Attributes.Add(key: "Email", value: MySession.Current.SessionEmail.ToString());
            item.Selected = true;

            comboBox.Items.Add(item);
            item.DataBind();

        }

        protected void LoadHesRecipient(RadComboBox comboBox, string uid)
        {
            if (!string.IsNullOrEmpty(uid))
            {
                comboBox.ClearSelection();
                comboBox.Items.Clear();
                comboBox.Text = string.Empty;

                if (RadioButtonList1.SelectedIndex > 0)
                {
                    GetVendors venUser;
                    venUser = new GetVendors();
                    venUser.SearchText = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(uid)));
                    venUser.VendorNumber = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(HiddenVendorNumber.Value)));

                    using (DataTable dt = venUser.GetVendorUserList())
                    {
                        //if (MySession.Current.SessionWebServer.ToString().ToUpper() == "TESTSITE")
                        //{

                        //    RadComboBoxItem item;
                        //    item = new RadComboBoxItem();

                        //    item.Font.Bold = true;

                        //    item.Text = TestVendorName;
                        //    item.ToolTip = TestVendorName;
                        //    item.Value = TestVendorUserID;

                        //    item.Attributes.Add(key: "UserId", value: TestVendorUserID);
                        //    item.Attributes.Add(key: "FullName", value: TestVendorName);
                        //    item.Attributes.Add(key: "CITY", value: TestVendorCity);
                        //    item.Attributes.Add(key: "CNTRY_NM", value: TestVendorCountry);
                        //    item.Attributes.Add(key: "Email", value: MySession.Current.SessionEmail.ToString());

                        //    item.Selected = true;
                        //    item.Checked = true;

                        //    comboBox.Items.Add(item);

                        //}
                        //else
                        //{
                        foreach (DataRow row in dt.Rows)
                        {
                            RadComboBoxItem item;
                            item = new RadComboBoxItem();

                            item.Font.Bold = true;

                            item.Text = row["Firstname"].ToString() + " " + row["Lastname"].ToString();
                            item.ToolTip = row["Firstname"].ToString() + " " + row["Lastname"].ToString();
                            item.Value = row["UserID"].ToString().ToUpper();

                            item.Attributes.Add(key: "UserId", value: row["UserID"].ToString().ToUpper());
                            item.Attributes.Add(key: "FullName", value: row["Firstname"].ToString() + " " + row["Lastname"].ToString());
                            item.Attributes.Add(key: "CITY", value: row["CITY"].ToString());
                            item.Attributes.Add(key: "CNTRY_NM", value: "");
                            item.Attributes.Add(key: "Email", value: row["EmailAddr"].ToString());

                            comboBox.Items.Add(item);
                        }
                        //}
                    }
                }
                else
                {
                    HesUsers hes;
                    hes = new HesUsers();
                    hes.UserID = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(uid)));

                    using (DataTable dt = hes.GetHesUserByUid())
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            RadComboBoxItem item;
                            item = new RadComboBoxItem();

                            item.Font.Bold = true;

                            item.Text = row["Firstname"].ToString() + " " + row["Lastname"].ToString();
                            item.ToolTip = row["Firstname"].ToString() + " " + row["Lastname"].ToString();
                            item.Value = row["UserID"].ToString().ToUpper();

                            item.Attributes.Add(key: "UserId", value: row["UserID"].ToString().ToUpper());
                            item.Attributes.Add(key: "FullName", value: row["Firstname"].ToString() + " " + row["Lastname"].ToString());
                            item.Attributes.Add(key: "CITY", value: row["CITY"].ToString());
                            item.Attributes.Add(key: "CNTRY_NM", value: row["CNTRY_NM"].ToString());
                            item.Attributes.Add(key: "Email", value: row["Email"].ToString());

                            item.Selected = true;
                            item.Checked = true;

                            comboBox.Items.Add(item);
                            item.DataBind();

                        }
                    }
                }
            }
        }

        protected void LoadVendorRecipient(RadComboBox comboBox, string uid, string vid)
        {
            if (!string.IsNullOrEmpty(uid))
            {
                comboBox.ClearSelection();
                comboBox.Items.Clear();
                comboBox.Text = string.Empty;


                //if (MySession.Current.SessionWebServer.ToString().ToUpper() == "TESTSITE")
                //{
                //    RadComboBoxItem item;
                //    item = new RadComboBoxItem();

                //    item.Font.Bold = true;

                //    item.Text = TestVendorName;
                //    item.ToolTip = TestVendorName;
                //    item.Value = TestVendorUserID;

                //    item.Attributes.Add(key: "UserId", value: TestVendorUserID);
                //    item.Attributes.Add(key: "FullName", value: TestVendorName);
                //    item.Attributes.Add(key: "CITY", value: TestVendorCity);
                //    item.Attributes.Add(key: "CNTRY_NM", value: TestVendorCountry);
                //    item.Attributes.Add(key: "Email", value: MySession.Current.SessionEmail.ToString());

                //    item.Selected = true;
                //    item.Checked = true;

                //    comboBox.Items.Add(item);
                //    item.DataBind();

                //}
                //else
                //{
                GetVendors ven;
                ven = new GetVendors
                {
                    UserId = uid,
                    VendorNumber = vid,
                    Active = ""
                };

                using (DataTable dt = ven.GetVendorUserByUid())
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        RadComboBoxItem item;
                        item = new RadComboBoxItem();

                        item.Font.Bold = true;

                        item.Text = row["Firstname"].ToString() + " " + row["Lastname"].ToString();
                        item.ToolTip = row["Firstname"].ToString() + " " + row["Lastname"].ToString();
                        item.Value = row["UserID"].ToString().ToUpper();

                        item.Attributes.Add(key: "UserId", value: row["UserID"].ToString().ToUpper());
                        item.Attributes.Add(key: "FullName", value: row["Firstname"].ToString() + " " + row["Lastname"].ToString());
                        item.Attributes.Add(key: "CITY", value: string.Empty);
                        item.Attributes.Add(key: "CNTRY_NM", value: string.Empty);
                        item.Attributes.Add(key: "Email", value: row["EmailAddr"].ToString());
                        item.Selected = true;

                        comboBox.Items.Add(item);
                        item.DataBind();

                    }
                }
                //}
            }

        }

        protected void LoadVendor(RadComboBox comboBox, string vid)
        {

            if (!string.IsNullOrEmpty(vid))
            {
                comboBox.ClearSelection();
                comboBox.Items.Clear();
                comboBox.Text = string.Empty;

                //GetVendors ven;
                //ven = new GetVendors();
                //ven.SearchText = vid;

                //using (DataTable dt = ven.GetVendorList())
                //{
                //    foreach (DataRow row in dt.Rows)
                //    {
                RadComboBoxItem item;
                item = new RadComboBoxItem();

                item.Font.Bold = true;

                //item.Text = row["VendorNumber"].ToString().Trim() + " - " + row["VendorName"].ToString().Trim();
                //item.ToolTip = row["VendorNumber"].ToString().Trim() + " - " + row["VendorName"].ToString().Trim();
                //item.Value = row["VendorNumber"].ToString().Trim();

                //item.Attributes.Add(key: "VendorNumber", value: row["VendorNumber"].ToString().Trim());
                //item.Attributes.Add(key: "VendorName", value: row["VendorName"].ToString().Trim());

                item.Text = HiddenVendorNumber.Value.ToString().Trim() + " - " + HiddenVendorName.Value.ToString().Trim();
                item.ToolTip = HiddenVendorNumber.Value.ToString().Trim() + " - " + HiddenVendorName.Value.ToString().Trim();
                item.Value = HiddenVendorNumber.Value.ToString().Trim();

                item.Attributes.Add(key: "VendorNumber", value: HiddenVendorNumber.Value.ToString().Trim());
                item.Attributes.Add(key: "VendorName", value: HiddenVendorName.Value.ToString().Trim());
                item.Selected = true;

                comboBox.Items.Add(item);
                item.DataBind();

                //    }
                //}
            }

        }

        protected void LoadFacility(RadComboBox comboBox)
        {
            comboBox.ClearSelection();
            comboBox.Text = string.Empty;
            comboBox.Items.Clear();

            GetRecord getRec;
            getRec = new GetRecord();

            using (DataTable dt = getRec.Exec_GetFacility_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    RadComboBoxItem item;
                    item = new RadComboBoxItem();
                    item.Value = row["OID"].ToString().Trim();
                    item.Text = row["FACILITY_NM"].ToString().Trim();
                    item.ToolTip = row["FACILITY_NM"].ToString();
                    comboBox.Items.Add(item);
                }

                comboBox.Items.Insert(index: 0, item: new RadComboBoxItem(text: "", value: "0"));
                comboBox.SelectedIndex = 0;
            }
        }

        protected void LoadPsl(RadComboBox comboBox)
        {
            comboBox.ClearSelection();
            comboBox.Text = string.Empty;
            comboBox.Items.Clear();

            GetRecord getRec;
            getRec = new GetRecord();

            using (DataTable dt = getRec.Exec_GetPsl_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    RadComboBoxItem item;
                    item = new RadComboBoxItem();
                    item.Value = row["OID"].ToString().Trim();
                    item.Text = row["PSL"].ToString().Trim();
                    item.ToolTip = row["PSL"].ToString();
                    comboBox.Items.Add(item);
                }

                comboBox.Items.Insert(index: 0, item: new RadComboBoxItem(text: "", value: "0"));
                comboBox.SelectedIndex = 0;
            }
        }

        protected void RadButton1_Click(object sender, EventArgs e)
        {
            try
            {
                UpdatePreviewStart();

                //////Removed lines below per David Dimerson - 2/19/2020
                /////###################

                //if (this.RadComboBox2.SelectedValue == "2") //Supplier
                //{
                //    this.RadioButtonList1.Items[1].Selected = true;
                //    this.RadioButtonList1.Items[1].Enabled = true;

                //    this.RadioButtonList2.Items[1].Selected = true;
                //    this.RadioButtonList2.Items[1].Enabled = true;

                //    GoToNextTab(1);
                //}
                //else
                //{
                //    RadTreeNode nodeStart = this.RadTreeView1.Nodes[1];
                //    if (nodeStart != null)
                //    {
                //        nodeStart.Expanded = true;

                //        RadTreeNode node1 = nodeStart.Nodes[0]; //Vendor

                //        if (node1 != null)
                //        {
                //            Literal literal10 = (Literal)node1.FindControl("Literal10");
                //            Literal literal11 = (Literal)node1.FindControl("Literal11");

                //            if (literal10 != null)
                //            {
                //                literal10.Text = string.Empty;  //Vendor Number
                //            }

                //            if (literal11 != null)
                //            {
                //                literal11.Text = string.Empty; //Vendor Name
                //            }
                //        }

                //    }

                //    this.RadioButtonList1.Items[0].Selected = true;
                //    this.RadioButtonList1.Items[1].Selected = false;
                //    this.RadioButtonList1.Items[1].Enabled = false;

                //    this.RadioButtonList2.Items[0].Selected = true;
                //    this.RadioButtonList2.Items[1].Selected = false;
                //    this.RadioButtonList2.Items[1].Enabled = false;

                //    this.HiddenVendorNumber.Value = "";
                //    this.HiddenVendorName.Value = "";

                //    if (this.RadComboBox7.SelectedItem != null)
                //    {
                //        this.HiddenIssuedToUserId.Value = this.RadComboBox7.SelectedItem.Attributes["UserID"].ToString().ToUpper();
                //        this.HiddenIssuedToUserName.Value = this.RadComboBox7.SelectedItem.Attributes["FullName"].ToString();
                //        this.HiddenIssuedToUserEmail.Value = this.RadComboBox7.SelectedItem.Attributes["Email"].ToString();
                //    }

                //    //this.HiddenIssuedToUserId.Value = "";
                //    //this.HiddenIssuedToUserName.Value = "";
                //    //this.HiddenIssuedToUserEmail.Value = "";

                //    GoToNextTab(2);
                //}

                GoToNextTab(3);

            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            }
        }

        protected void RadButton2_Click(object sender, EventArgs e)
        {
            try
            {
                UpdatePreviewVendor();
                GoToNextTab(2);

            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            }
        }

        protected void RadButton3_Click(object sender, EventArgs e)
        {
            try
            {
                UpdatePreviewIssuedTo();
                GoToNextTab(3);

                Panel1.Visible = false;


                if (RadComboBox2.SelectedValue == "2")
                {
                    RadListBox1.Height = Unit.Pixel(160);
                    RadListBox3.Items.Clear();

                    Panel1.Visible = true;

                    //if (MySession.Current.SessionWebServer.ToString().ToUpper() == "TESTSITE")
                    //{
                    //    RadListBoxItem item;
                    //    item = new RadListBoxItem();

                    //    item.Text = TestVendorName;
                    //    item.ToolTip = TestVendorName;
                    //    item.Value = TestVendorUserID;

                    //    item.Attributes.Add(key: "UserId", value: TestVendorUserID);
                    //    item.Attributes.Add(key: "UserEmail", value: MySession.Current.SessionEmail.ToString());
                    //    item.Attributes.Add(key: "FullName", value: TestVendorName);
                    //    item.Attributes.Add(key: "VendorNumber", value: this.HiddenVendorNumber.Value.ToString().Trim());

                    //    item.Checked = true;

                    //    this.RadListBox3.Items.Add(item);
                    //    this.RadListBox3.SortItems();
                    //}
                    //else
                    //{
                    GetVendors ven;
                    ven = new GetVendors();
                    ven.SearchText = string.Empty;
                    ven.VendorNumber = Server.HtmlEncode(htmlUtil.SanitizeHtml(HiddenVendorNumber.Value));

                    using (DataTable dt = ven.GetVendorUserList())
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            RadListBoxItem item;
                            item = new RadListBoxItem();

                            item.Text = row["FullName"].ToString();
                            item.Value = row["UserID"].ToString().Trim().ToUpper();
                            item.ToolTip = row["FullName"].ToString();
                            item.Checkable = true;
                            item.Attributes["UserId"] = row["UserID"].ToString().ToUpper();
                            item.Attributes["UserEmail"] = row["EmailAddr"].ToString().ToLower();
                            item.Attributes["FullName"] = row["FullName"].ToString();
                            item.Attributes["VendorNumber"] = HiddenVendorNumber.Value.ToString().Trim();

                            //////***  If vendor recipients are required, remove REM below ****
                            //if (row["UserID"].ToString().Trim().ToLower() == this.HiddenIssuedToUserId.Value.ToString().Trim().ToLower())
                            //{
                            //    item.Checked = true;
                            //}

                            RadListBox3.Items.Add(item);
                            RadListBox3.SortItems();
                        }
                    }
                    //}
                }
                else
                {
                    RadListBox1.Height = Unit.Pixel(310);
                }

            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            }
        }

        protected void AddToMailingList(string userId, string fullName, string emailAdress, bool checkedItem)
        {
            //try
            //{
            if (!string.IsNullOrEmpty(userId))
            {
                RadComboBox radComboBox8;
                radComboBox8 = (RadComboBox)RadListBox1.Header.FindControl(id: "RadComboBox8");

                if (radComboBox8 != null)
                {
                    RadListBoxItem itemFound;
                    itemFound = RadListBox1.FindItemByValue(userId.Trim().ToUpper());
                    if (itemFound == null)
                    {
                        RadListBoxItem item;
                        item = new RadListBoxItem();

                        item.Text = fullName;
                        item.Value = userId.ToUpper();
                        item.ToolTip = fullName;
                        item.Checkable = true;
                        item.Checked = checkedItem;


                        item.Attributes["UserId"] = userId.ToUpper();
                        item.Attributes["UserEmail"] = emailAdress.Trim().ToLower();
                        item.Attributes["FullName"] = fullName;

                        RadListBox1.Items.Add(item);
                        RadListBox1.SortItems();
                    }
                    else
                    {
                        itemFound.Checked = checkedItem;
                    }
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);
            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
        }

        protected void AddToSupplierMailingList(string userId, string fullName, string emailAdress, bool checkedItem, string vendorNumber)
        {
            //try
            //{
            if (!string.IsNullOrEmpty(userId))
            {
                RadListBoxItem itemFound;
                itemFound = RadListBox3.FindItemByValue(userId.Trim().ToUpper());
                if (itemFound == null)
                {
                    RadListBoxItem item;
                    item = new RadListBoxItem();

                    item.Text = fullName;
                    item.Value = userId.ToUpper();
                    item.ToolTip = fullName;
                    item.Checkable = true;
                    item.Checked = checkedItem;


                    item.Attributes["UserId"] = userId.ToUpper();
                    item.Attributes["UserEmail"] = emailAdress.Trim().ToLower();
                    item.Attributes["FullName"] = fullName;
                    item.Attributes["VendorNumber"] = vendorNumber;

                    RadListBox3.Items.Add(item);
                    RadListBox3.SortItems();
                }
                else
                {
                    itemFound.Checked = checkedItem;
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);
            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
        }

        protected void RadButton13_Click(object sender, EventArgs e)
        {
            try
            {
                RadComboBox radComboBox8;
                radComboBox8 = (RadComboBox)RadListBox1.Header.FindControl(id: "RadComboBox8");

                if (radComboBox8 != null)
                {
                    if (!string.IsNullOrEmpty(radComboBox8.Text.Trim()) && !string.IsNullOrEmpty(radComboBox8.SelectedValue.ToString().Trim()))
                    {
                        RadListBoxItem itemFound = RadListBox1.FindItemByValue(radComboBox8.SelectedValue.Trim().ToUpper());

                        if (itemFound == null)
                        {
                            RadListBoxItem item;
                            item = new RadListBoxItem();

                            item.Text = HiddenRecipientName.Value.ToString();
                            item.Value = radComboBox8.SelectedValue.Trim().ToUpper();
                            item.ToolTip = HiddenRecipientName.Value.ToString();
                            item.Checked = true;
                            item.Checkable = true;
                            item.Selected = true;
                            item.Attributes["UserId"] = HiddenRecipientId.Value.ToUpper();
                            item.Attributes["UserEmail"] = HiddenRecipientEmail.Value;
                            item.Attributes["FullName"] = HiddenRecipientName.Value;

                            RadListBox1.Items.Add(item);
                            RadListBox1.SortItems();
                        }
                        else
                        {
                            itemFound.Selected = true;
                        }
                        radComboBox8.Text = string.Empty;
                        HiddenRecipientId.Value = string.Empty;
                        HiddenRecipientEmail.Value = string.Empty;
                        HiddenRecipientName.Value = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);

                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            }
        }

        public string GetPreviewLiteralValue(int ControlNum, int parentLevel, int childLevel)
        {
            string strVal = string.Empty;

            RadTreeNode node = RadTreeView1.Nodes[parentLevel];
            if (node != null)
            {
                RadTreeNode nodeData = node.Nodes[childLevel];

                if (nodeData != null)
                {
                    Literal literal = (Literal)nodeData.FindControl("Literal" + ControlNum.ToString());
                    if (literal != null)
                    {
                        strVal = literal.Text.Trim();
                    }
                }
            }
            return strVal;
        }

        //public string GetPreviewDivValue(int ControlNum, int parentLevel, int childLevel)
        //{
        //    string strVal = string.Empty;

        //    RadTreeNode node = this.RadTreeView1.Nodes[parentLevel];
        //    if (node != null)
        //    {
        //        RadTreeNode nodeData = node.Nodes[childLevel];

        //        if (nodeData != null)
        //        {
        //            HtmlContainerControl div = (HtmlContainerControl)nodeData.FindControl("Div" + ControlNum.ToString());

        //            if (div != null)
        //            {
        //                strVal = div.InnerHtml.Trim();
        //            }
        //        }
        //    }
        //    return strVal;
        //}

        protected void LoadPreviewLiteralValue(int ControlNum, string ControlVal, int parentLevel, int childLevel)
        {
            RadTreeNode node = RadTreeView1.Nodes[parentLevel];
            if (node != null)
            {
                RadTreeNode nodeData = node.Nodes[childLevel];

                if (nodeData != null)
                {
                    Literal literal = (Literal)nodeData.FindControl("Literal" + ControlNum.ToString());
                    if (literal != null)
                    {
                        literal.Text = htmlUtil.SanitizeHtml(ControlVal);
                    }
                }
            }
        }

        public string GetPreviewRadTextBox(int ControlNum, int parentLevel, int childLevel)
        {
            string strVal = string.Empty;

            RadTreeNode node = RadTreeView1.Nodes[parentLevel];
            if (node != null)
            {
                RadTreeNode nodeData = node.Nodes[childLevel];

                if (nodeData != null)
                {
                    RadTextBox radTextBox = (RadTextBox)nodeData.FindControl("RadTextBox" + ControlNum.ToString());

                    if (radTextBox != null)
                    {
                        strVal = radTextBox.Text.Trim();
                    }
                }
            }
            return strVal;
        }

        //protected void LoadPreviewDivValue(int ControlNum, string ControlVal, int parentLevel, int childLevel)
        //{
        //    RadTreeNode node = this.RadTreeView1.Nodes[parentLevel];
        //    if (node != null)
        //    {
        //        RadTreeNode nodeData = node.Nodes[childLevel];

        //        if (nodeData != null)
        //        {
        //            HtmlContainerControl div = (HtmlContainerControl)nodeData.FindControl("Div" + ControlNum.ToString());

        //            if (div != null)
        //            {
        //                div.InnerHtml = ControlVal;
        //            }
        //        }
        //    }
        //}

        protected void LoadPreviewTextBoxValue(int ControlNum, string ControlVal, int parentLevel, int childLevel)
        {
            RadTreeNode node = RadTreeView1.Nodes[parentLevel];
            if (node != null)
            {
                RadTreeNode nodeData = node.Nodes[childLevel];

                if (nodeData != null)
                {
                    RadTextBox radTextBox = (RadTextBox)nodeData.FindControl("RadTextBox" + ControlNum.ToString());

                    if (radTextBox != null)
                    {
                        radTextBox.Text = ControlVal;
                    }
                }
            }
        }

        protected void LoadPreviewComboBoxValue(int ControlNum, string ControlVal, string ControlText, int parentLevel, int childLevel)
        {
            RadTreeNode node = RadTreeView1.Nodes[parentLevel];
            if (node != null)
            {
                RadTreeNode nodeData = node.Nodes[childLevel];

                if (nodeData != null)
                {
                    RadComboBox radComboBox = (RadComboBox)nodeData.FindControl("RadComboBox" + ControlNum.ToString());

                    if (radComboBox != null)
                    {
                        RadComboBoxItem itemFound;
                        itemFound = radComboBox.FindItemByValue(ControlVal);

                        if (itemFound == null)
                        {
                            RadComboBoxItem item;
                            item = new RadComboBoxItem();

                            item.Value = ControlVal;
                            item.Text = ControlText;

                            radComboBox.Items.Add(item);
                        }
                    }
                }
            }
        }

        protected void ClearPreviewComboBoxItems(int ControlNum, int parentLevel, int childLevel)
        {
            RadTreeNode node = RadTreeView1.Nodes[parentLevel];
            if (node != null)
            {
                RadTreeNode nodeData = node.Nodes[childLevel];

                if (nodeData != null)
                {
                    RadComboBox radComboBox = (RadComboBox)nodeData.FindControl("RadComboBox" + ControlNum.ToString());

                    if (radComboBox != null)
                    {
                        radComboBox.Items.Clear();
                    }
                }
            }
        }

        protected void RadButton25_Click(object sender, EventArgs e)
        {
            try
            {
                string uReturn;
                uReturn = SaveComleteLater();

                if (uReturn == "Successfully")
                {
                    UpdatePreviewStart();
                    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Saved');", addScriptTags: true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('Save FAILED.\\n" + removeChar.SanitizeQuoteString(uReturn.ToString()) + "\\n\\n" + ErrException + "');", addScriptTags: true);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            }
        }

        protected void RadButton26_Click(object sender, EventArgs e)
        {
            //--- NOT allowed to use this feature at this time

            ////try
            ////{
            ////    string uReturn;
            ////    uReturn = SaveComleteLater();

            ////    if (uReturn == "Successfully")
            ////    {
            ////        UpdatePreviewVendor();
            ////        ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Saved');", addScriptTags: true);
            ////    }
            ////    else
            ////    {
            ////        ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('Save FAILED.\\n" + removeChar.SanitizeQuoteString(uReturn.ToString()) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            ////    }
            ////}
            ////catch (Exception ex)
            ////{
            ////    LogException(ex);
            ////    ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            ////}
        }

        protected void RadButton27_Click(object sender, EventArgs e)
        {
            try
            {
                string uReturn;
                uReturn = SaveComleteLater();

                if (uReturn == "Successfully")
                {
                    UpdatePreviewIssuedTo();
                    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Saved');", addScriptTags: true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('Save FAILED.\\n" + removeChar.SanitizeQuoteString(uReturn.ToString()) + "\\n\\n" + ErrException + "');", addScriptTags: true);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            }
        }

        protected void RadButton28_Click(object sender, EventArgs e)
        {
            try
            {
                string uReturn;
                uReturn = SaveComleteLater();

                if (uReturn == "Successfully")
                {
                    UpdatePreviewQuestion(RadTextBox11, 3);
                    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Saved');", addScriptTags: true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('Save FAILED.\\n" + removeChar.SanitizeQuoteString(uReturn.ToString()) + "\\n\\n" + ErrException + "');", addScriptTags: true);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            }
        }

        protected void RadButton29_Click(object sender, EventArgs e)
        {
            try
            {
                string uReturn;
                uReturn = SaveComleteLater();

                if (uReturn == "Successfully")
                {
                    UpdatePreviewQuestion(RadTextBox14, 4);
                    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Saved');", addScriptTags: true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('Save FAILED.\\n" + removeChar.SanitizeQuoteString(uReturn.ToString()) + "\\n\\n" + ErrException + "');", addScriptTags: true);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            }
        }

        protected void RadButton30_Click(object sender, EventArgs e)
        {
            try
            {
                string uReturn;
                uReturn = SaveComleteLater();

                if (uReturn == "Successfully")
                {
                    UpdatePreviewQuestion(RadTextBox15, 5);
                    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Saved');", addScriptTags: true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('Save FAILED.\\n" + removeChar.SanitizeQuoteString(uReturn.ToString()) + "\\n\\n" + ErrException + "');", addScriptTags: true);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            }
        }

        protected void RadButton31_Click(object sender, EventArgs e)
        {
            try
            {
                string uReturn;
                uReturn = SaveComleteLater();

                if (uReturn == "Successfully")
                {
                    UpdatePreviewQuestion(RadTextBox16, 6);
                    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Saved');", addScriptTags: true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('Save FAILED.\\n" + removeChar.SanitizeQuoteString(uReturn.ToString()) + "\\n\\n" + ErrException + "');", addScriptTags: true);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            }
        }

        protected void RadButton32_Click(object sender, EventArgs e)
        {
            try
            {
                string uReturn;
                uReturn = SaveComleteLater();

                if (uReturn == "Successfully")
                {
                    UpdatePreviewQuestion(RadTextBox17, 7);
                    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Saved');", addScriptTags: true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('Save FAILED.\\n" + removeChar.SanitizeQuoteString(uReturn.ToString()) + "\\n\\n" + ErrException + "');", addScriptTags: true);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            }
        }

        protected void RadButton33_Click(object sender, EventArgs e)
        {
            try
            {
                string uReturn;
                uReturn = SaveComleteLater();

                if (uReturn == "Successfully")
                {
                    UpdatePreviewActionTaken();
                    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Saved');", addScriptTags: true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('Save FAILED.\\n" + removeChar.SanitizeQuoteString(uReturn.ToString()) + "\\n\\n" + ErrException + "');", addScriptTags: true);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            }
        }

        protected void RadButton34_Click(object sender, EventArgs e)
        {
            try
            {
                string uReturn;
                uReturn = SaveComleteLater();

                if (uReturn == "Successfully")
                {
                    UpdatePreviewOriginatorUseOnly();
                    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Saved');", addScriptTags: true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('Save FAILED.\\n" + removeChar.SanitizeQuoteString(uReturn.ToString()) + "\\n\\n" + ErrException + "');", addScriptTags: true);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            }
        }

        private string SaveComleteLater()
        {
            string uReturn = string.Empty;
            string uApisoReturn = string.Empty;
            string uDelApisoReturn = string.Empty;

            //// ** Load API ISO Reference
            RadTreeNode node = RadTreeView1.Nodes[0];
            if (node != null)
            {
                RadTreeNode nodeData = node.Nodes[0];

                if (nodeData != null)
                {
                    RadComboBox radComboBox = (RadComboBox)nodeData.FindControl("RadComboBox1");

                    if (radComboBox != null)
                    {
                        if (radComboBox.Items.Count > 0)
                        {
                            UpdateRecord del = new UpdateRecord
                            {
                                Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Label1.Text)))
                            };
                            uDelApisoReturn = del.ExecDeleteCarApiIso();

                            if (uDelApisoReturn == "Successfully")
                            {
                                foreach (RadComboBoxItem item in radComboBox.Items)
                                {
                                    CreateRecord newApiso;
                                    newApiso = new CreateRecord
                                    {
                                        Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Label1.Text))),
                                        Api_Iso_Oid = item.Value
                                    };
                                    uApisoReturn = newApiso.ExecCreateCar_Api_Iso();

                                    if (uApisoReturn.ToString() != "Successfully")
                                    {
                                        int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                        Exception aex = new Exception(removeChar.SanitizeQuoteString(uApisoReturn.ToString()));
                                        aex.Data.Add(key: "TargetSite", value: "newApiso.ExecCreateCar_Api_Iso()");
                                        aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                        CustomLogException(aex);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (uApisoReturn.ToString() == "Successfully" && uDelApisoReturn.ToString() == "Successfully")
            {

                UpdateRecord updtCar;
                updtCar = new UpdateRecord
                {
                    Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Label1.Text))),
                    Car_Status_Oid = RadComboBox20.SelectedItem.Value,
                    Area_Descript_Oid = RadComboBox2.SelectedItem.Value,
                    Date_Issued = RadDatePicker1.SelectedDate.Value.ToShortDateString(),
                    Due_dt = RadDatePicker2.SelectedDate.Value.ToShortDateString(),
                    Originator_Usr_Id = HiddenOriginatorUserId.Value.ToString().ToUpper(),
                    Facility_Name_Oid = RadComboBox5.SelectedItem.Value,
                    Finding_Desc = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(RadTextBox12.Text.Trim()))),
                    Finding_Type_Oid = RadComboBox9.SelectedItem.Value,
                    Psl_Oid = RadComboBox4.SelectedItem.Value,
                    Desc_Of_Improvement = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(RadTextBox13.Text.Trim()))),
                    Category_Oid = RadComboBox12.SelectedItem.Value,
                    Audit_Nbr = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(RadTextBox1.Text.Trim()))),
                    Qnote_Nbr = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(RadTextBox2.Text.Trim()))),
                    Cpi_Nbr = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(RadTextBox3.Text.Trim()))),
                    Material_Nbr = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(RadTextBox4.Text.Trim()))),
                    Purchase_Order_Nbr = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(RadTextBox5.Text.Trim()))),
                    Production_Order_Nbr = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(RadTextBox6.Text.Trim()))),
                    Api_Audit_Nbr = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(RadTextBox8.Text.Trim()))),
                    Maintenance_Order_Nbr = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(RadTextBox22.Text.Trim()))),
                    Equipment_Nbr = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(RadTextBox23.Text.Trim()))),

                    Resp_Person_Usr_Id = HiddenResponsibleUserId.Value.ToUpper(),

                    Vndr_Nbr = HiddenVendorNumber.Value.ToString().ToUpper(),
                    Vendor_Nm = HiddenVendorName.Value.ToString().ToUpper(),

                    Issued_To_Usr_Id = HiddenIssuedToUserId.Value.ToString().ToUpper(),
                    Issued_To_Usr_Name = HiddenIssuedToUserName.Value.ToString().ToUpper(),
                    Issued_To_Usr_Email = HiddenIssuedToUserEmail.Value.ToString().ToUpper(),

                    Loc_Country_Oid = RadComboBox10.SelectedItem.Value,
                    Loc_Supplier = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(RadTextBox10.Text.Trim()))),

                    Non_Conform_Rsn = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(RadTextBox11.Text.Trim()))), //Q#1
                    Root_Cause = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(RadTextBox14.Text.Trim()))), //Q#2

                    Similar_Instance_Y_N = RadComboBox14.SelectedValue
                };

                if (RadComboBox14.SelectedIndex == 1)
                {
                    updtCar.Similar_Instance = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(RadTextBox15.Text.Trim()))); //Q#3
                }
                else
                {
                    updtCar.Similar_Instance = string.Empty; //Q#3
                }

                updtCar.Corr_Action_Taken = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(RadTextBox16.Text.Trim())));  //Q#4

                if (!RadDatePicker3.IsEmpty)
                {
                    updtCar.Corr_Action_Taken_Dt = RadDatePicker3.SelectedDate.Value.ToShortDateString();
                }
                else
                {
                    updtCar.Corr_Action_Taken_Dt = string.Empty;
                }

                updtCar.Preclude_Action = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(RadTextBox17.Text.Trim())));  //Q#5

                if (!RadDatePicker4.IsEmpty)
                {
                    updtCar.Preclude_Action_Dt = RadDatePicker4.SelectedDate.Value.ToShortDateString();
                }
                else
                {
                    updtCar.Preclude_Action_Dt = string.Empty;
                }

                updtCar.Action_Taken_By_Usr_Id = HiddenActionTakenUserId.Value.ToString().ToUpper();

                if (!RadDatePicker5.IsEmpty)
                {
                    updtCar.Response_Dt = RadDatePicker5.SelectedDate.Value.ToShortDateString();
                }
                else
                {
                    updtCar.Response_Dt = string.Empty;
                }

                if (!RadDatePicker6.IsEmpty)
                {
                    updtCar.Due_Dt_Ext = RadDatePicker6.SelectedDate.Value.ToShortDateString();
                }
                else
                {
                    updtCar.Due_Dt_Ext = string.Empty;
                }

                updtCar.Reissued_To_Usr_Id = HiddenReIssuedUserId.Value.ToString().ToUpper();

                if (!RadDatePicker7.IsEmpty)
                {
                    updtCar.Reissued_Dt = RadDatePicker7.SelectedDate.Value.ToShortDateString();
                }
                else
                {
                    updtCar.Reissued_Dt = string.Empty;
                }

                updtCar.Follow_Up_Reqd = RadComboBox19.SelectedItem.Value.Trim().ToUpper();

                if (!RadDatePicker9.IsEmpty)
                {
                    updtCar.Follow_Up_Dt = RadDatePicker9.SelectedDate.Value.ToShortDateString();
                }
                else
                {
                    updtCar.Follow_Up_Dt = string.Empty;
                }

                if (!RadDatePicker8.IsEmpty)
                {
                    updtCar.Received_Dt = RadDatePicker8.SelectedDate.Value.ToShortDateString();
                }
                else
                {
                    updtCar.Received_Dt = string.Empty;
                }

                if (!RadDatePicker10.IsEmpty)
                {
                    updtCar.Verify_Dt = RadDatePicker10.SelectedDate.Value.ToShortDateString();
                }
                else
                {
                    updtCar.Verify_Dt = string.Empty;
                }

                updtCar.Verify_By_Usr_Id = HiddenVerifiedByUserId.Value.ToString().ToUpper();
                updtCar.Response_Accept_By_Usr_Id = HiddenAcceptedByUserId.Value.ToString().ToUpper();
                updtCar.Remarks = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(RadTextBox18.Text.Trim()))); //Remarks

                updtCar.Last_Updt_by = MySession.Current.SessionUserID.ToString().ToUpper();

                #region Update selected effectiveness validations

                List<int> selected = new List<int>(), allEVDOids = null;
                allEVDOids = new GetRecord() { CarOid = updtCar.Car_Oid }.Exec_GetEVD_Oids();

                foreach (RadComboBoxItem itm in RadComboBox21.Items)
                {
                    if (itm.Checked) selected.Add(Convert.ToInt32(itm.Value));
                }
                updtCar.ExecDeleteEV(allEVDOids.ToArray());
                updtCar.ExecUpdateEV(selected.ToArray());

                #endregion

                uReturn = updtCar.ExecUpdateCar();
            }

            return uReturn;
        }

        protected void RadButton5_Click(object sender, EventArgs e)
        {
            //// *** Update CAR ***
            try
            {
                string uReturn;
                string uApisoReturn = string.Empty;
                string uDelApisoReturn = string.Empty;

                //// ** Load API ISO Reference
                RadTreeNode node = RadTreeView1.Nodes[0];
                if (node != null)
                {
                    RadTreeNode nodeData = node.Nodes[0];

                    if (nodeData != null)
                    {
                        RadComboBox radComboBox = (RadComboBox)nodeData.FindControl("RadComboBox1");

                        if (radComboBox != null)
                        {
                            if (radComboBox.Items.Count > 0)
                            {
                                UpdateRecord del = new UpdateRecord();
                                del.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Label1.Text)));
                                uDelApisoReturn = del.ExecDeleteCarApiIso();

                                if (uDelApisoReturn == "Successfully")
                                {
                                    foreach (RadComboBoxItem item in radComboBox.Items)
                                    {
                                        CreateRecord newApiso;
                                        newApiso = new CreateRecord();
                                        newApiso.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Label1.Text)));
                                        newApiso.Api_Iso_Oid = item.Value;
                                        uApisoReturn = newApiso.ExecCreateCar_Api_Iso();

                                        if (uApisoReturn.ToString() != "Successfully")
                                        {
                                            int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                            Exception aex = new Exception(removeChar.SanitizeQuoteString(uApisoReturn.ToString()));
                                            aex.Data.Add(key: "TargetSite", value: "newApiso.ExecCreateCar_Api_Iso()");
                                            aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                            CustomLogException(aex);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


                if (uApisoReturn.ToString() == "Successfully" && uDelApisoReturn.ToString() == "Successfully")
                {
                    UpdateRecord updtCar;
                    updtCar = new UpdateRecord();

                    updtCar.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Label1.Text)));
                    updtCar.Car_Status_Oid = RadComboBox20.SelectedItem.Value;
                    updtCar.Area_Descript_Oid = RadComboBox2.SelectedItem.Value;
                    updtCar.Date_Issued = GetPreviewLiteralValue(3, 0, 0);
                    updtCar.Issued_To_Usr_Id = GetPreviewLiteralValue(21, 2, 0);

                    updtCar.Issued_To_Usr_Name = GetPreviewLiteralValue(12, 2, 0);
                    updtCar.Issued_To_Usr_Email = GetPreviewLiteralValue(19, 2, 0);

                    updtCar.Due_dt = GetPreviewLiteralValue(4, 0, 0);
                    updtCar.Originator_Usr_Id = GetPreviewLiteralValue(1, 0, 0);
                    updtCar.Facility_Name_Oid = RadComboBox5.SelectedItem.Value;
                    updtCar.Loc_Supplier = GetPreviewLiteralValue(23, 2, 0);
                    updtCar.Vndr_Nbr = GetPreviewLiteralValue(10, 1, 0);
                    updtCar.Vendor_Nm = GetPreviewLiteralValue(11, 1, 0);
                    updtCar.Finding_Desc = GetPreviewRadTextBox(19, 0, 0);
                    updtCar.Finding_Type_Oid = RadComboBox9.SelectedItem.Value;
                    updtCar.Psl_Oid = RadComboBox4.SelectedItem.Value;
                    updtCar.Desc_Of_Improvement = GetPreviewRadTextBox(20, 0, 0);
                    updtCar.Category_Oid = RadComboBox12.SelectedItem.Value;
                    updtCar.Loc_Country_Oid = HiddenCountryOid.Value;
                    updtCar.Audit_Nbr = GetPreviewLiteralValue(13, 0, 0);
                    updtCar.Qnote_Nbr = GetPreviewLiteralValue(14, 0, 0);
                    updtCar.Cpi_Nbr = GetPreviewLiteralValue(15, 0, 0);
                    updtCar.Material_Nbr = GetPreviewLiteralValue(16, 0, 0);
                    updtCar.Purchase_Order_Nbr = GetPreviewLiteralValue(17, 0, 0);
                    updtCar.Production_Order_Nbr = GetPreviewLiteralValue(18, 0, 0);
                    updtCar.Api_Audit_Nbr = GetPreviewLiteralValue(20, 0, 0);

                    updtCar.Maintenance_Order_Nbr = GetPreviewLiteralValue(25, 0, 0);
                    updtCar.Equipment_Nbr = GetPreviewLiteralValue(42, 0, 0);


                    updtCar.Resp_Person_Usr_Id = GetPreviewLiteralValue(24, 0, 0);

                    updtCar.Non_Conform_Rsn = GetPreviewRadTextBox(20, 3, 0); //Q#1
                    updtCar.Root_Cause = GetPreviewRadTextBox(20, 4, 0); //Q#2

                    updtCar.Similar_Instance_Y_N = RadComboBox14.SelectedValue;
                    updtCar.Similar_Instance = GetPreviewRadTextBox(20, 5, 0); //Q#3

                    updtCar.Corr_Action_Taken = GetPreviewRadTextBox(20, 6, 0); //Q#4
                    updtCar.Corr_Action_Taken_Dt = GetPreviewLiteralValue(24, 6, 0);

                    updtCar.Preclude_Action = GetPreviewRadTextBox(20, 7, 0); //Q#5
                    updtCar.Preclude_Action_Dt = GetPreviewLiteralValue(24, 7, 0);

                    updtCar.Action_Taken_By_Usr_Id = GetPreviewLiteralValue(26, 8, 0);
                    updtCar.Response_Dt = GetPreviewLiteralValue(27, 8, 0);

                    updtCar.Due_Dt_Ext = GetPreviewLiteralValue(28, 9, 0);
                    updtCar.Reissued_To_Usr_Id = GetPreviewLiteralValue(30, 9, 0);
                    updtCar.Reissued_Dt = GetPreviewLiteralValue(31, 9, 0);
                    updtCar.Received_Dt = GetPreviewLiteralValue(32, 9, 0);
                    updtCar.Follow_Up_Reqd = RadComboBox19.SelectedItem.Value.Trim().ToUpper();
                    updtCar.Follow_Up_Dt = GetPreviewLiteralValue(34, 9, 0);
                    updtCar.Verify_Dt = GetPreviewLiteralValue(35, 9, 0);
                    updtCar.Verify_By_Usr_Id = GetPreviewLiteralValue(37, 9, 0);
                    updtCar.Response_Accept_By_Usr_Id = GetPreviewLiteralValue(39, 9, 0);
                    updtCar.Remarks = GetPreviewRadTextBox(21, 9, 0); //Remarks

                    updtCar.Last_Updt_by = MySession.Current.SessionUserID.ToString().ToUpper();

                    uReturn = updtCar.ExecUpdateCar();

                    if (uReturn.ToString() == "Successfully")
                    {
                        string msgReturn = string.Empty;
                        msgReturn = SendMsgHes(Label1.Text, Label2.Text);

                        if (msgReturn == "Successfully")
                        {
                            RadListBox2.Items.Clear();

                            string[] mailReturn = { string.Empty, string.Empty };

                            CreateRecord mail;
                            mail = new CreateRecord();
                            mail.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Label1.Text)));
                            mail.Email_Subject = "Updated Corrective Action Request " + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Label2.Text.Trim())));
                            mail.Email_Message = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(RadTextBox9.Text.Trim())));
                            mail.User_Id = MySession.Current.SessionUserID.ToString().ToUpper();
                            mail.User_Name = MySession.Current.SessionFullName.ToString();

                            mailReturn = mail.ExecCreateCar_Email();

                            if (mailReturn[0].ToString() == "Successfully")
                            {
                                foreach (RadListBoxItem node1 in RadListBox1.CheckedItems)
                                {
                                    if (node1.Checked && node1.Value != "Check All")
                                    {
                                        string sReturn = string.Empty;

                                        CreateRecord sendTo;
                                        sendTo = new CreateRecord();
                                        sendTo.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Label1.Text)));
                                        sendTo.Car_Email_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(mailReturn[1].ToString())));
                                        sendTo.User_Id = node1.Attributes["UserId"].ToString().ToUpper().Trim();
                                        sendTo.User_Name = node1.Attributes["FullName"].ToString().Trim();
                                        sendTo.User_Email = node1.Attributes["UserEmail"].ToString().Trim();
                                        sendTo.Vendor_Nm = "0";

                                        sReturn = sendTo.ExecCreateCar_Email_Sent_To();

                                        if (sReturn.ToString() == "Successfully")
                                        {
                                            RadListBoxItem item;
                                            item = new RadListBoxItem();

                                            item.Text = removeChar.SanitizeQuoteString(node1.Attributes["FullName"].ToString().Trim());
                                            item.Value = node1.Attributes["UserID"].ToString().ToUpper();

                                            RadListBox2.Items.Add(node1);
                                            RadListBox2.SortItems();
                                        }
                                        else
                                        {
                                            int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                            Exception aex = new Exception(removeChar.SanitizeQuoteString(sReturn.ToString()));
                                            aex.Data.Add(key: "TargetSite", value: "sendTo.ExecCreateCar_Email_Sent_To()");
                                            aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                            CustomLogException(aex);
                                        }
                                    }
                                }

                                if (RadComboBox2.SelectedValue == "2") //Supplier
                                {
                                    msgReturn = SendMsgVendor(Label1.Text, Label2.Text);

                                    if (msgReturn == "Successfully")
                                    {

                                        foreach (RadListBoxItem node3 in RadListBox3.CheckedItems)
                                        {
                                            if (node3.Checked && node3.Value != "Check All")
                                            {
                                                string sReturn = string.Empty;

                                                CreateRecord sendTo;
                                                sendTo = new CreateRecord();
                                                sendTo.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Label1.Text)));
                                                sendTo.Car_Email_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(mailReturn[1].ToString())));
                                                sendTo.User_Id = node3.Attributes["UserId"].ToString().ToUpper().Trim();
                                                sendTo.User_Name = node3.Attributes["FullName"].ToString().Trim();
                                                sendTo.User_Email = node3.Attributes["UserEmail"].ToString().Trim();
                                                sendTo.Vendor_Nm = node3.Attributes["VendorNumber"].ToString().Trim(); ;

                                                sReturn = sendTo.ExecCreateCar_Email_Sent_To();

                                                if (sReturn.ToString() == "Successfully")
                                                {
                                                    RadListBoxItem item;
                                                    item = new RadListBoxItem();

                                                    item.Text = removeChar.SanitizeQuoteString(node3.Attributes["FullName"].ToString().Trim());
                                                    item.Value = node3.Attributes["UserID"].ToString().ToUpper();

                                                    RadListBox2.Items.Add(node3);
                                                    RadListBox2.SortItems();
                                                }
                                                else
                                                {
                                                    int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                                    Exception aex = new Exception(removeChar.SanitizeQuoteString(sReturn.ToString()));
                                                    aex.Data.Add(key: "TargetSite", value: "sendTo.ExecCreateCar_Email_Sent_To()");
                                                    aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                                                    CustomLogException(aex);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        GoToNextTab(11);

                        for (int i = 0; i <= (RadTabStrip1.Tabs.Count - 1); i++)
                        {
                            RadTabStrip1.Tabs[i].Enabled = false;
                        }

                    }
                    else
                    {
                        //this.Label1.Text = uReturn[0].ToString();
                        //this.Label2.Text = string.Empty;

                        ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('CAR Update FAILED.\\n\\n" + removeChar.SanitizeQuoteString(uReturn.ToString()) + "\\n\\n" + ErrException + "');", addScriptTags: true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('API - ISO Reference Update FAILED. \\n\\n" + ErrException + "');", addScriptTags: true);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                Label2.Text = "Error occured";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            }
        }

        protected void RadButton6_Click(object sender, EventArgs e)
        {

            try
            {
                LoadDefaultData();
                GoToNextTab(0);

            }
            catch (Exception ex)
            {
                LogException(ex);

                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            }
        }

        private void GoToNextTab(int tabIndex)
        {
            RadTabStrip1.Tabs[tabIndex].Selected = true;
            RadTabStrip1.Tabs[tabIndex].Enabled = true;

            RadMultiPage1.SelectedIndex = tabIndex;
            RadTabStrip1.CausesValidation = false;

            ExpandTreeviewNode(tabIndex);
        }

        private void ExpandTreeviewNode(int nTreeNode)
        {
            foreach (RadTreeNode node in RadTreeView1.GetAllNodes())
            {
                if (node.GetAllNodes() != null)
                {
                    node.BackColor = Color.White;
                    node.ForeColor = Color.Black;
                    node.BorderStyle = BorderStyle.None;

                    node.Font.Bold = false;
                    node.Expanded = false;

                    if (node.Index == nTreeNode)
                    {
                        node.Font.Bold = true;
                        node.Expanded = true;
                    }
                }
            }
        }

        private void UpdatePreviewStart()
        {
            LoadPreviewLiteralValue(1, HiddenOriginatorUserId.Value.ToString().ToUpper(), 0, 0);
            LoadPreviewLiteralValue(2, HiddenOriginatorUserName.Value, 0, 0);
            LoadPreviewLiteralValue(3, RadDatePicker1.SelectedDate.Value.ToShortDateString(), 0, 0); // Date Issued
            LoadPreviewLiteralValue(4, RadDatePicker2.SelectedDate.Value.ToShortDateString(), 0, 0); // Due Date
            LoadPreviewLiteralValue(5, RadComboBox9.SelectedItem.Text.Trim(), 0, 0);  // Finding Type
            LoadPreviewLiteralValue(6, RadComboBox2.SelectedItem.Text.Trim(), 0, 0); //Area 
            LoadPreviewLiteralValue(7, RadComboBox4.SelectedItem.Text.Trim(), 0, 0);  //PSL
            LoadPreviewLiteralValue(8, RadComboBox5.SelectedItem.Text.Trim(), 0, 0);  // Plant
            LoadPreviewLiteralValue(9, RadComboBox12.SelectedItem.Text.Trim(), 0, 0);  // Category

            LoadPreviewLiteralValue(19, HiddenResponsibleUserName.Value.ToString().ToUpper(), 0, 0);
            LoadPreviewLiteralValue(24, HiddenResponsibleUserId.Value.ToUpper(), 0, 0);

            //LoadPreviewLiteralValue(13, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox1.Text.Trim()))), 0, 0); // Audit Number
            //LoadPreviewLiteralValue(14, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox2.Text.Trim()))), 0, 0); // Q Note Number
            //LoadPreviewLiteralValue(15, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox3.Text.Trim()))), 0, 0); // CPI Number
            //LoadPreviewLiteralValue(16, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox4.Text.Trim()))), 0, 0); // Material Number
            //LoadPreviewLiteralValue(17, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox5.Text.Trim()))), 0, 0); // Purchase Order Number
            //LoadPreviewLiteralValue(18, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox6.Text.Trim()))), 0, 0); // Production Order Number
            //LoadPreviewLiteralValue(20, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox8.Text.Trim()))), 0, 0); // API Audit Number

            //LoadPreviewTextBoxValue(19, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox12.Text.Trim()))), 0, 0); // Description of Finding
            //LoadPreviewTextBoxValue(20, removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox13.Text.Trim()))), 0, 0); // Description of Improvement



            LoadPreviewLiteralValue(13, htmlUtil.SanitizeHtml(Server.HtmlDecode(RadTextBox1.Text.Trim())), 0, 0); // Audit Number
            LoadPreviewLiteralValue(14, htmlUtil.SanitizeHtml(Server.HtmlDecode(RadTextBox2.Text.Trim())), 0, 0); // Q Note Number
            LoadPreviewLiteralValue(15, htmlUtil.SanitizeHtml(Server.HtmlDecode(RadTextBox3.Text.Trim())), 0, 0); // CPI Number
            LoadPreviewLiteralValue(16, htmlUtil.SanitizeHtml(Server.HtmlDecode(RadTextBox4.Text.Trim())), 0, 0); // Material Number
            LoadPreviewLiteralValue(17, htmlUtil.SanitizeHtml(Server.HtmlDecode(RadTextBox5.Text.Trim())), 0, 0); // Purchase Order Number
            LoadPreviewLiteralValue(18, htmlUtil.SanitizeHtml(Server.HtmlDecode(RadTextBox6.Text.Trim())), 0, 0); // Production Order Number
            LoadPreviewLiteralValue(20, htmlUtil.SanitizeHtml(Server.HtmlDecode(RadTextBox8.Text.Trim())), 0, 0); // API Audit Number
            LoadPreviewLiteralValue(25, htmlUtil.SanitizeHtml(Server.HtmlDecode(RadTextBox22.Text.Trim())), 0, 0); // Maintenance Order Number
            LoadPreviewLiteralValue(42, htmlUtil.SanitizeHtml(Server.HtmlDecode(RadTextBox23.Text.Trim())), 0, 0); // Equipment Number

            LoadPreviewTextBoxValue(19, htmlUtil.SanitizeHtml(Server.HtmlDecode(RadTextBox12.Text.Trim())), 0, 0); // Description of Finding
            LoadPreviewTextBoxValue(20, htmlUtil.SanitizeHtml(Server.HtmlDecode(RadTextBox13.Text.Trim())), 0, 0); // Description of Improvement

            //// ** Clear API ISO Reference Items
            ClearPreviewComboBoxItems(1, 0, 0);

            //// ** Load New API ISO Reference Items
            foreach (RadComboBoxItem itemChecked in RadComboBox6.CheckedItems)
            {
                LoadPreviewComboBoxValue(1, itemChecked.Value, itemChecked.Text, 0, 0);
            }

            HiddenAreaOid.Value = RadComboBox2.SelectedItem.Value;

        }

        private void UpdatePreviewVendor()
        {
            LoadPreviewLiteralValue(10, HiddenVendorNumber.Value.ToString().ToUpper(), 1, 0);
            LoadPreviewLiteralValue(11, HiddenVendorName.Value.ToString().ToUpper(), 1, 0);

        }

        private void UpdatePreviewIssuedTo()
        {
            LoadPreviewLiteralValue(12, HiddenIssuedToUserName.Value.ToString(), 2, 0);
            LoadPreviewLiteralValue(19, HiddenIssuedToUserEmail.Value.ToString(), 2, 0);
            LoadPreviewLiteralValue(21, HiddenIssuedToUserId.Value.ToString().ToUpper(), 2, 0);

            LoadPreviewLiteralValue(22, RadComboBox10.SelectedItem.Text, 2, 0);
            LoadPreviewLiteralValue(23, htmlUtil.SanitizeHtml(Server.HtmlDecode(RadTextBox10.Text.Trim())), 2, 0);

            HiddenCountryOid.Value = RadComboBox10.SelectedItem.Value;
        }

        private void UpdatePreviewActionTaken()
        {
            LoadPreviewLiteralValue(25, HiddenActionTakenUserName.Value.ToString(), 8, 0);
            LoadPreviewLiteralValue(26, HiddenActionTakenUserId.Value.ToString().ToUpper(), 8, 0);

            if (!RadDatePicker5.IsEmpty)
            {
                LoadPreviewLiteralValue(27, RadDatePicker5.SelectedDate.Value.ToShortDateString(), 8, 0);
            }
        }

        private void UpdatePreviewOriginatorUseOnly()
        {
            if (!RadDatePicker6.IsEmpty)
            {
                LoadPreviewLiteralValue(28, RadDatePicker6.SelectedDate.Value.ToShortDateString(), 9, 0);
            }
            else
            {
                LoadPreviewLiteralValue(28, string.Empty, 9, 0);
            }

            LoadPreviewLiteralValue(29, HiddenReIssuedUserName.Value.ToString(), 9, 0);
            LoadPreviewLiteralValue(30, HiddenReIssuedUserId.Value.ToString().ToUpper(), 9, 0);

            if (!RadDatePicker7.IsEmpty)
            {
                LoadPreviewLiteralValue(31, RadDatePicker7.SelectedDate.Value.ToShortDateString(), 9, 0);
            }
            else
            {
                LoadPreviewLiteralValue(31, string.Empty, 9, 0);
            }

            if (!RadDatePicker8.IsEmpty) //Date Received
            {
                LoadPreviewLiteralValue(32, RadDatePicker8.SelectedDate.Value.ToShortDateString(), 9, 0);
            }
            else
            {
                LoadPreviewLiteralValue(32, string.Empty, 9, 0);
            }

            LoadPreviewLiteralValue(33, RadComboBox19.SelectedItem.Text, 9, 0);

            if (!RadDatePicker9.IsEmpty) //Date to Follow-up
            {
                LoadPreviewLiteralValue(34, RadDatePicker9.SelectedDate.Value.ToShortDateString(), 9, 0);
            }
            else
            {
                LoadPreviewLiteralValue(34, string.Empty, 9, 0);
            }

            if (!RadDatePicker10.IsEmpty) //Date Verified
            {
                LoadPreviewLiteralValue(35, RadDatePicker10.SelectedDate.Value.ToShortDateString(), 9, 0);
            }
            else
            {
                LoadPreviewLiteralValue(35, string.Empty, 9, 0);
            }

            LoadPreviewLiteralValue(36, HiddenVerifiedByUserName.Value.ToString(), 9, 0);
            LoadPreviewLiteralValue(37, HiddenVerifiedByUserId.Value.ToString().ToUpper(), 9, 0);

            LoadPreviewLiteralValue(38, HiddenAcceptedByUserName.Value.ToString(), 9, 0);
            LoadPreviewLiteralValue(39, HiddenAcceptedByUserId.Value.ToString().ToUpper(), 9, 0);

            LoadPreviewLiteralValue(41, RadComboBox20.SelectedItem.Text, 9, 0);

            if (RadComboBox20.SelectedValue.ToString() == "3") //Close Date
            {
                LoadPreviewLiteralValue(40, DateTime.Now.ToShortDateString(), 9, 0);
            }
            else
            {
                LoadPreviewLiteralValue(40, string.Empty, 9, 0);
            }

            LoadPreviewTextBoxValue(21, htmlUtil.SanitizeHtml(Server.HtmlDecode(RadTextBox18.Text.Trim())), 9, 0);
        }

        private void UpdatePreviewQuestion(RadTextBox radTextBox, int nodeIndex)
        {
            if (radTextBox != null)
            {
                LoadPreviewTextBoxValue(20, htmlUtil.SanitizeHtml(Server.HtmlDecode(radTextBox.Text.Trim())), nodeIndex, 0);

                if (nodeIndex == 5)
                {
                    LoadPreviewLiteralValue(24, RadComboBox14.SelectedItem.Text, nodeIndex, 0);
                }

                if (nodeIndex == 6)
                {
                    if (!RadDatePicker3.IsEmpty)
                    {
                        LoadPreviewLiteralValue(24, RadDatePicker3.SelectedDate.Value.ToShortDateString(), nodeIndex, 0);
                    }
                    else
                    {
                        LoadPreviewLiteralValue(24, string.Empty, nodeIndex, 0);
                    }
                }

                if (nodeIndex == 7)
                {
                    if (!RadDatePicker4.IsEmpty)
                    {
                        LoadPreviewLiteralValue(24, RadDatePicker4.SelectedDate.Value.ToShortDateString(), nodeIndex, 0);
                    }
                    else
                    {
                        LoadPreviewLiteralValue(24, string.Empty, nodeIndex, 0);
                    }
                }
            }
        }

        private static string GetStatusMessage(int offset, int total)
        {
            if (total <= 0)
                return "No matches";

            return String.Format(format: "<b>1</b>-<b>{0}</b> out of <b>{1}</b>", arg0: offset, arg1: total);
        }

        protected void RadComboBox3_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                RadComboBox3.Items.Clear();

                string srchText = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(e.Text.Trim())));

                HesUsers hesUser;
                hesUser = new HesUsers();
                hesUser.SearchText = srchText;

                using (DataTable dt = hesUser.GetHesUsers())
                {
                    int itemOffset = e.NumberOfItems;
                    int endOffset = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count);
                    e.EndOfItems = endOffset == dt.Rows.Count;

                    for (int i = itemOffset; i < endOffset; i++)
                    {
                        RadComboBoxItem item;
                        item = new RadComboBoxItem();

                        item.Font.Bold = true;

                        item.Text = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                        item.ToolTip = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                        item.Value = dt.Rows[i]["UserID"].ToString().Trim().ToUpper();

                        item.Attributes.Add(key: "UserId", value: dt.Rows[i]["UserID"].ToString().Trim().ToUpper());
                        item.Attributes.Add(key: "FullName", value: dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim());
                        item.Attributes.Add(key: "CITY", value: dt.Rows[i]["CITY"].ToString().Trim());
                        item.Attributes.Add(key: "CNTRY_NM", value: dt.Rows[i]["CNTRY_NM"].ToString().Trim());
                        item.Attributes.Add(key: "Email", value: dt.Rows[i]["Email"].ToString().Trim());
                        RadComboBox3.Items.Add(item);

                        item.DataBind();
                    }
                    e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                e.Message = "No matches - " + ex.Message;
            }
        }

        protected void RadComboBox7_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                RadComboBox7.Items.Clear();

                string srchText = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(e.Text.Trim())));

                //if (this.RadComboBox2.SelectedValue.ToString() == "2")
                if (RadioButtonList2.SelectedIndex > 0)
                {
                    GetVendors venUser;
                    venUser = new GetVendors();
                    venUser.SearchText = srchText;
                    venUser.VendorNumber = Server.HtmlEncode(htmlUtil.SanitizeHtml(HiddenVendorNumber.Value));

                    using (DataTable dt = venUser.GetVendorUserList())
                    {
                        //if (MySession.Current.SessionWebServer.ToString().ToUpper() == "TESTSITE")
                        //{
                        //    int itemOffset = e.NumberOfItems;
                        //    int endOffset = Math.Min(itemOffset + ItemsPerRequest, 1);
                        //    e.EndOfItems = endOffset == 1;

                        //    RadComboBoxItem item;
                        //    item = new RadComboBoxItem();

                        //    item.Font.Bold = true;

                        //    item.Text = TestVendorName;
                        //    item.ToolTip = TestVendorName;
                        //    item.Value = TestVendorUserID;

                        //    item.Attributes.Add(key: "UserId", value: TestVendorUserID);
                        //    item.Attributes.Add(key: "FullName", value: TestVendorName);
                        //    item.Attributes.Add(key: "CITY", value: TestVendorCity);
                        //    item.Attributes.Add(key: "CNTRY_NM", value: TestVendorCountry);
                        //    item.Attributes.Add(key: "Email", value: MySession.Current.SessionEmail.ToString());

                        //    item.Selected = true;
                        //    item.Checked = true;

                        //    this.RadComboBox7.Items.Add(item);
                        //    item.DataBind();
                        //    e.Message = GetStatusMessage(endOffset, 1);
                        //}
                        //else
                        //{
                        int itemOffset = e.NumberOfItems;
                        int endOffset = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count);
                        e.EndOfItems = endOffset == dt.Rows.Count;

                        for (int i = itemOffset; i < endOffset; i++)
                        {
                            RadComboBoxItem item;
                            item = new RadComboBoxItem();

                            item.Font.Bold = true;

                            item.Text = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                            item.ToolTip = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                            item.Value = dt.Rows[i]["UserID"].ToString().Trim().ToUpper();

                            item.Attributes.Add(key: "UserId", value: dt.Rows[i]["UserID"].ToString().Trim().ToUpper());
                            item.Attributes.Add(key: "FullName", value: dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim());
                            item.Attributes.Add(key: "CITY", value: "");
                            item.Attributes.Add(key: "CNTRY_NM", value: "");
                            item.Attributes.Add(key: "Email", value: dt.Rows[i]["EmailAddr"].ToString().Trim());
                            RadComboBox7.Items.Add(item);

                            item.DataBind();
                        }
                        e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
                        //}
                    }
                }
                else
                {
                    HesUsers hesUser;
                    hesUser = new HesUsers();
                    hesUser.SearchText = srchText;

                    using (DataTable dt = hesUser.GetHesUsers())
                    {
                        int itemOffset = e.NumberOfItems;
                        int endOffset = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count);
                        e.EndOfItems = endOffset == dt.Rows.Count;

                        for (int i = itemOffset; i < endOffset; i++)
                        {
                            RadComboBoxItem item;
                            item = new RadComboBoxItem();

                            item.Font.Bold = true;

                            item.Text = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                            item.ToolTip = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                            item.Value = dt.Rows[i]["UserID"].ToString().Trim().ToUpper();

                            item.Attributes.Add(key: "UserId", value: dt.Rows[i]["UserID"].ToString().Trim().ToUpper());
                            item.Attributes.Add(key: "FullName", value: dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim());
                            item.Attributes.Add(key: "CITY", value: dt.Rows[i]["CITY"].ToString().Trim());
                            item.Attributes.Add(key: "CNTRY_NM", value: dt.Rows[i]["CNTRY_NM"].ToString().Trim());
                            item.Attributes.Add(key: "Email", value: dt.Rows[i]["Email"].ToString().Trim());
                            RadComboBox7.Items.Add(item);

                            item.DataBind();
                        }
                        e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                e.Message = "No matches - " + ex.Message;
            }
        }

        protected void RadComboBox15_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                RadComboBox15.Items.Clear();

                string srchText = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(e.Text.Trim())));

                if (RadioButtonList1.SelectedIndex > 0)
                {
                    GetVendors venUser;
                    venUser = new GetVendors();
                    venUser.SearchText = srchText;
                    venUser.VendorNumber = Server.HtmlEncode(htmlUtil.SanitizeHtml(HiddenVendorNumber.Value));

                    using (DataTable dt = venUser.GetVendorUserList())
                    {
                        //if (MySession.Current.SessionWebServer.ToString().ToUpper() == "TESTSITE")
                        //{
                        //    int itemOffset = e.NumberOfItems;
                        //    int endOffset = Math.Min(itemOffset + ItemsPerRequest, 1);
                        //    e.EndOfItems = endOffset == 1;

                        //    RadComboBoxItem item;
                        //    item = new RadComboBoxItem();

                        //    item.Font.Bold = true;

                        //    item.Text = TestVendorName;
                        //    item.ToolTip = TestVendorName;
                        //    item.Value = TestVendorUserID;

                        //    item.Attributes.Add(key: "UserId", value: TestVendorUserID);
                        //    item.Attributes.Add(key: "FullName", value: TestVendorName);
                        //    item.Attributes.Add(key: "CITY", value: TestVendorCity);
                        //    item.Attributes.Add(key: "CNTRY_NM", value: TestVendorCountry);
                        //    item.Attributes.Add(key: "Email", value: MySession.Current.SessionEmail.ToString());

                        //    item.Selected = true;
                        //    item.Checked = true;

                        //    this.RadComboBox15.Items.Add(item);
                        //    item.DataBind();
                        //    e.Message = GetStatusMessage(endOffset, 1);
                        //}
                        //else
                        //{

                        int itemOffset = e.NumberOfItems;
                        int endOffset = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count);
                        e.EndOfItems = endOffset == dt.Rows.Count;

                        for (int i = itemOffset; i < endOffset; i++)
                        {
                            RadComboBoxItem item;
                            item = new RadComboBoxItem();

                            item.Font.Bold = true;

                            item.Text = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                            item.ToolTip = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                            item.Value = dt.Rows[i]["UserID"].ToString().Trim().ToUpper();

                            item.Attributes.Add(key: "UserId", value: dt.Rows[i]["UserID"].ToString().Trim().ToUpper());
                            item.Attributes.Add(key: "FullName", value: dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim());
                            item.Attributes.Add(key: "CITY", value: "");
                            item.Attributes.Add(key: "CNTRY_NM", value: "");
                            item.Attributes.Add(key: "Email", value: dt.Rows[i]["EmailAddr"].ToString().Trim());

                            RadComboBox15.Items.Add(item);

                            item.DataBind();
                        }
                        e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
                        //}
                    }
                }
                else
                {
                    HesUsers hesUser;
                    hesUser = new HesUsers();
                    hesUser.SearchText = srchText;

                    using (DataTable dt = hesUser.GetHesUsers())
                    {
                        int itemOffset = e.NumberOfItems;
                        int endOffset = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count);
                        e.EndOfItems = endOffset == dt.Rows.Count;

                        for (int i = itemOffset; i < endOffset; i++)
                        {
                            RadComboBoxItem item;
                            item = new RadComboBoxItem();

                            item.Font.Bold = true;

                            //item.Text = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                            item.Text = dt.Rows[i]["Dsply_NM"].ToString().Trim();
                            item.ToolTip = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                            item.Value = dt.Rows[i]["UserID"].ToString().Trim().ToUpper();

                            item.Attributes.Add(key: "UserId", value: dt.Rows[i]["UserID"].ToString().Trim().ToUpper());
                            //item.Attributes.Add(key: "FullName", value: dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim());
                            item.Attributes.Add(key: "FullName", value: dt.Rows[i]["Dsply_NM"].ToString().Trim());
                            item.Attributes.Add(key: "CITY", value: dt.Rows[i]["CITY"].ToString().Trim());
                            item.Attributes.Add(key: "CNTRY_NM", value: dt.Rows[i]["CNTRY_NM"].ToString().Trim());
                            item.Attributes.Add(key: "Email", value: dt.Rows[i]["Email"].ToString().Trim());

                            RadComboBox15.Items.Add(item);

                            item.DataBind();
                        }
                        e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                e.Message = "No matches - " + ex.Message;
            }
        }

        protected void RadComboBox16_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                RadComboBox16.Items.Clear();

                string srchText = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(e.Text.Trim())));

                HesUsers hesUser;
                hesUser = new HesUsers();
                hesUser.SearchText = srchText;

                using (DataTable dt = hesUser.GetHesUsers())
                {
                    int itemOffset = e.NumberOfItems;
                    int endOffset = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count);
                    e.EndOfItems = endOffset == dt.Rows.Count;

                    for (int i = itemOffset; i < endOffset; i++)
                    {
                        RadComboBoxItem item;
                        item = new RadComboBoxItem();

                        item.Font.Bold = true;

                        item.Text = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                        item.ToolTip = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                        item.Value = dt.Rows[i]["UserID"].ToString().Trim().ToUpper();

                        item.Attributes.Add(key: "UserId", value: dt.Rows[i]["UserID"].ToString().Trim().ToUpper());
                        item.Attributes.Add(key: "FullName", value: dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim());
                        item.Attributes.Add(key: "CITY", value: dt.Rows[i]["CITY"].ToString().Trim());
                        item.Attributes.Add(key: "CNTRY_NM", value: dt.Rows[i]["CNTRY_NM"].ToString().Trim());
                        item.Attributes.Add(key: "Email", value: dt.Rows[i]["Email"].ToString().Trim());

                        RadComboBox16.Items.Add(item);

                        item.DataBind();
                    }
                    e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                e.Message = "No matches - " + ex.Message;
            }
        }

        protected void RadComboBox17_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                RadComboBox17.Items.Clear();

                string srchText = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(e.Text.Trim())));

                HesUsers hesUser;
                hesUser = new HesUsers();
                hesUser.SearchText = srchText;

                using (DataTable dt = hesUser.GetHesUsers())
                {
                    int itemOffset = e.NumberOfItems;
                    int endOffset = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count);
                    e.EndOfItems = endOffset == dt.Rows.Count;

                    for (int i = itemOffset; i < endOffset; i++)
                    {
                        RadComboBoxItem item;
                        item = new RadComboBoxItem();

                        item.Font.Bold = true;

                        item.Text = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                        item.ToolTip = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                        item.Value = dt.Rows[i]["UserID"].ToString().Trim();

                        item.Attributes.Add(key: "UserId", value: dt.Rows[i]["UserID"].ToString().Trim());
                        item.Attributes.Add(key: "FullName", value: dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim());
                        item.Attributes.Add(key: "CITY", value: dt.Rows[i]["CITY"].ToString().Trim());
                        item.Attributes.Add(key: "CNTRY_NM", value: dt.Rows[i]["CNTRY_NM"].ToString().Trim());
                        item.Attributes.Add(key: "Email", value: dt.Rows[i]["Email"].ToString().Trim());

                        RadComboBox17.Items.Add(item);

                        item.DataBind();
                    }
                    e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                e.Message = "No matches - " + ex.Message;
            }
        }

        protected void RadComboBox18_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                RadComboBox18.Items.Clear();

                string srchText = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(e.Text.Trim())));

                HesUsers hesUser;
                hesUser = new HesUsers
                {
                    SearchText = srchText
                };

                using (DataTable dt = hesUser.GetHesUsers())
                {
                    int itemOffset = e.NumberOfItems;
                    int endOffset = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count);
                    e.EndOfItems = endOffset == dt.Rows.Count;

                    for (int i = itemOffset; i < endOffset; i++)
                    {
                        RadComboBoxItem item;
                        item = new RadComboBoxItem();

                        item.Font.Bold = true;

                        item.Text = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                        item.ToolTip = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                        item.Value = dt.Rows[i]["UserID"].ToString().Trim().ToUpper();

                        item.Attributes.Add(key: "UserId", value: dt.Rows[i]["UserID"].ToString().Trim().ToUpper());
                        item.Attributes.Add(key: "FullName", value: dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim());
                        item.Attributes.Add(key: "CITY", value: dt.Rows[i]["CITY"].ToString().Trim());
                        item.Attributes.Add(key: "CNTRY_NM", value: dt.Rows[i]["CNTRY_NM"].ToString().Trim());
                        item.Attributes.Add(key: "Email", value: dt.Rows[i]["Email"].ToString().Trim());

                        RadComboBox18.Items.Add(item);

                        item.DataBind();
                    }
                    e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                e.Message = "No matches - " + ex.Message;
            }
        }

        protected void RadComboBox8_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                RadComboBox radComboBox8;
                radComboBox8 = (RadComboBox)RadListBox1.Header.FindControl(id: "RadComboBox8");

                if (radComboBox8 != null)
                {
                    radComboBox8.Items.Clear();

                    string srchText = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(e.Text.Trim())));

                    //if (this.RadComboBox2.SelectedValue == "2")
                    //{
                    //    GetVendors venUser;
                    //    venUser = new GetVendors();
                    //    venUser.SearchText = srchText;
                    //    venUser.VendorNumber = this.HiddenVendorNumber.Value;

                    //    using (DataTable dt = venUser.getVendorUserList())
                    //    {
                    //        int itemOffset = e.NumberOfItems;
                    //        int endOffset = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count);
                    //        e.EndOfItems = endOffset == dt.Rows.Count;

                    //        for (int i = itemOffset; i < endOffset; i++)
                    //        {
                    //            RadComboBoxItem item;
                    //            item = new RadComboBoxItem();

                    //            item.Font.Bold = true;

                    //            item.Text = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                    //            item.ToolTip = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                    //            item.Value = dt.Rows[i]["UserID"].ToString().Trim();

                    //            item.Attributes.Add(key: "UserId", value: dt.Rows[i]["UserID"].ToString().Trim());
                    //            item.Attributes.Add(key: "FullName", value: dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim());
                    //            item.Attributes.Add(key: "CITY", value: "");
                    //            item.Attributes.Add(key: "CNTRY_NM", value: "");
                    //            item.Attributes.Add(key: "Email", value: dt.Rows[i]["EmailAddr"].ToString().Trim());
                    //            radComboBox8.Items.Add(item);

                    //            item.DataBind();
                    //        }
                    //        e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
                    //    }
                    //}
                    //else
                    //{
                    HesUsers hesUser;
                    hesUser = new HesUsers
                    {
                        SearchText = srchText
                    };

                    using (DataTable dt = hesUser.GetHesUsers())
                    {
                        int itemOffset = e.NumberOfItems;
                        int endOffset = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count);
                        e.EndOfItems = endOffset == dt.Rows.Count;

                        for (int i = itemOffset; i < endOffset; i++)
                        {
                            RadComboBoxItem item;
                            item = new RadComboBoxItem();

                            item.Font.Bold = true;

                            item.Text = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                            item.ToolTip = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                            item.Value = dt.Rows[i]["UserID"].ToString().Trim().ToUpper();

                            item.Attributes.Add(key: "UserId", value: dt.Rows[i]["UserID"].ToString().Trim().ToUpper());
                            item.Attributes.Add(key: "FullName", value: dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim());
                            item.Attributes.Add(key: "CITY", value: dt.Rows[i]["CITY"].ToString().Trim());
                            item.Attributes.Add(key: "CNTRY_NM", value: dt.Rows[i]["CNTRY_NM"].ToString().Trim());
                            item.Attributes.Add(key: "Email", value: dt.Rows[i]["Email"].ToString().Trim());
                            radComboBox8.Items.Add(item);

                            item.DataBind();
                        }
                        e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                e.Message = "No matches - " + ex.Message;
            }
        }

        protected void RadComboBox11_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                RadComboBox11.Items.Clear();

                string srchText = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(e.Text.Trim())));

                GetVendors vendor;
                vendor = new GetVendors
                {
                    SearchText = srchText
                };

                using (DataTable dt = vendor.GetVendorList())
                {
                    int itemOffset = e.NumberOfItems;
                    int endOffset = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count);
                    e.EndOfItems = endOffset == dt.Rows.Count;

                    for (int i = itemOffset; i < endOffset; i++)
                    {
                        RadComboBoxItem item;
                        item = new RadComboBoxItem();

                        item.Font.Bold = true;

                        item.Text = dt.Rows[i]["VendorNumber"].ToString().Trim() + " - " + dt.Rows[i]["VendorName"].ToString().Trim();
                        item.ToolTip = dt.Rows[i]["VendorNumber"].ToString().Trim() + " - " + dt.Rows[i]["VendorName"].ToString().Trim();
                        item.Value = dt.Rows[i]["VendorNumber"].ToString().Trim();

                        item.Attributes.Add(key: "VendorNumber", value: dt.Rows[i]["VendorNumber"].ToString().Trim());
                        item.Attributes.Add(key: "VendorName", value: dt.Rows[i]["VendorName"].ToString().Trim().ToUpper());

                        RadComboBox11.Items.Add(item);

                        item.DataBind();
                    }
                    e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                e.Message = "No matches - " + ex.Message;
            }
        }

        protected void RadComboBox13_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                RadComboBox13.Items.Clear();

                string srchText = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(e.Text.Trim())));

                HesUsers hesUser;
                hesUser = new HesUsers
                {
                    SearchText = srchText
                };

                using (DataTable dt = hesUser.GetHesUsers())
                {
                    int itemOffset = e.NumberOfItems;
                    int endOffset = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count);
                    e.EndOfItems = endOffset == dt.Rows.Count;

                    for (int i = itemOffset; i < endOffset; i++)
                    {
                        RadComboBoxItem item;
                        item = new RadComboBoxItem();

                        item.Font.Bold = true;

                        item.Text = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                        item.ToolTip = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                        item.Value = dt.Rows[i]["UserID"].ToString().Trim().ToUpper();

                        item.Attributes.Add(key: "UserId", value: dt.Rows[i]["UserID"].ToString().Trim().ToUpper());
                        item.Attributes.Add(key: "FullName", value: dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim());
                        item.Attributes.Add(key: "CITY", value: dt.Rows[i]["CITY"].ToString().Trim());
                        item.Attributes.Add(key: "CNTRY_NM", value: dt.Rows[i]["CNTRY_NM"].ToString().Trim());
                        item.Attributes.Add(key: "Email", value: dt.Rows[i]["Email"].ToString().Trim());
                        RadComboBox13.Items.Add(item);

                        item.DataBind();
                    }
                    e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                e.Message = "No matches - " + ex.Message;
            }
        }

        protected void RadComboBox14_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (e.Value.ToString().ToUpper() == "Y")
            {
                Panel3.Visible = true;
            }
            else
            {
                Panel3.Visible = false;
                RadTextBox15.Text = string.Empty;
            }
        }

        protected void RadTabStrip1_TabClick(object sender, RadTabStripEventArgs e)
        {
            try
            {
                //this.RadTabStrip1.Tabs[e.Tab.Index].Selected = true;

                int curTab = e.Tab.Index + 1;

                for (int i = curTab; i <= (RadTabStrip1.Tabs.Count - 1); i++)
                {
                    RadTabStrip1.Tabs[i].Enabled = false;
                }

                ExpandTreeviewNode(e.Tab.Index);
            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + ". " + ErrException + "');", addScriptTags: true);
            }
        }

        private string SendMsgHes(string carOid, string carNbr)
        {
            string uReturn = "Successfully";
            string HostName = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;

            try
            {
                using (MailMessage objEmail = new MailMessage())
                {
                    MailAddress frmAddr;
                    frmAddr = new MailAddress(noReplyContact.ToString().Trim(), "NoReply");

                    MailAddress frmOriginatorAddr;
                    frmOriginatorAddr = new MailAddress(MySession.Current.SessionEmail.ToString(), MySession.Current.SessionFullName.ToString());

                    objEmail.From = frmAddr;
                    objEmail.Bcc.Add(frmOriginatorAddr);

                    string carStatus = string.Empty;
                    string bodyMsg = string.Empty;
                    string bobyTestWeb = string.Empty;

                    if (MySession.Current.SessionWebServer.ToString().ToUpper() == "TESTSITE")
                    {
                        bobyTestWeb = "<font size=3 face=Arial color=red><b>***  This is a Test...Testing...Please disregard to this message  ***</b></font> <br><br>";
                    }

                    //// ** HES Recipients
                    foreach (RadListBoxItem item in RadListBox1.CheckedItems)
                    {
                        if (item.Checked)
                        {
                            if (item.Attributes["UserEmail"].ToString().Contains("@"))
                            {
                                objEmail.Bcc.Add(item.Attributes["UserEmail"].ToString());
                            }
                        }
                    }

                    GetRecord xCar;
                    xCar = new GetRecord
                    {
                        CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carOid)))
                    };

                    using (DataTable dt = xCar.Exec_Get_Existing_Car_By_Oid_Datatable())
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            carStatus = row["CAR_STATUS_NM"].ToString().ToUpper() + " - ";
                        }
                    }

                    objEmail.IsBodyHtml = true;
                    objEmail.Subject = carStatus + "Updated Corrective Action Request " + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carNbr)));
                    if (RadComboBox2.SelectedValue == "2") //Supplier
                    {
                        objEmail.Subject += " (" + htmlUtil.SanitizeHtml(HiddenVendorName.Value.ToString()) + ")";
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td align='center' style='background-color: #18B5F0'><b>***  Please do not reply to this email. This is an unmonitored address, and replies to this email cannot be responded to or read. ***</b></td></tr>");
                    sb.Append("<tr><td></td></tr>");
                    sb.Append("<tr><td>See attached Corrective Action Request <a href=" + HostName + "/default.aspx?form=car&indx=" + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carOid))) + "> " + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carNbr))) + "</a></td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br /><br />");
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td><b><u>Message from " + MySession.Current.SessionFullName.ToString() + ":</u></b></td></tr>");
                    sb.Append("<tr><td><pre>");
                    sb.Append(htmlUtil.SanitizeHtml(RadTextBox9.Text.Trim()));
                    sb.Append("</pre></td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br /><br />");
                    sb.Append("<br /><br />");

                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td><b><u>Message sent to the following recipient(s):</u></b></td></tr>");
                    foreach (RadListBoxItem item in RadListBox1.CheckedItems)
                    {
                        if (item.Checked)
                        {
                            if (item.Attributes["UserEmail"].ToString().Contains("@"))
                            {
                                sb.Append("<tr><td>");
                                sb.Append(htmlUtil.SanitizeHtml(item.Text.Trim()));
                                sb.Append("</td></tr>");
                            }
                        }
                    }

                    foreach (RadListBoxItem item in RadListBox3.CheckedItems)
                    {
                        if (item.Checked)
                        {
                            if (item.Attributes["UserEmail"].ToString().Contains("@"))
                            {
                                sb.Append("<tr><td>");
                                sb.Append(htmlUtil.SanitizeHtml(item.Text.Trim()));
                                sb.Append("</td></tr>");
                            }
                        }
                    }

                    sb.Append("</table>");
                    sb.Append("<br />");
                    sb.Append("<hr />");
                    sb.Append("This e - mail, including any attached files, may contain confidential and privileged information for the sole use of the intended recipient.  Any review, use, distribution, or disclosure by others is strictly prohibited. If you are not the intended recipient(or authorized to receive information for the intended recipient), please delete all copies of this message.");


                    objEmail.Priority = MailPriority.High;
                    objEmail.Body = bobyTestWeb + bodyMsg + sb.ToString(); ;

                    if (HiddenIssuedToUserId.Value.ToString().Trim().ToUpper() == "NON_HSN")
                    {
                        CreateDocx docx = new CreateDocx
                        {
                            Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carOid)))
                        };

                        using (MemoryStream memoryStream = docx.GetDocxFile())
                        {
                            byte[] bytes = memoryStream.ToArray();
                            memoryStream.Close();

                            Attachment attach = new Attachment(new MemoryStream(bytes), "CAR_" + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carNbr))) + ".docx");

                            objEmail.Attachments.Add(attach);

                        }
                    }
                    else
                    {
                        CreatePdf pdf = new CreatePdf
                        {
                            Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carOid)))
                        };

                        using (MemoryStream memoryStream = pdf.GetPdfFile())
                        {
                            byte[] bytes = memoryStream.ToArray();
                            memoryStream.Close();

                            Attachment attach = new Attachment(new MemoryStream(bytes), "CAR_" + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carNbr))) + ".pdf");

                            objEmail.Attachments.Add(attach);

                        }
                    }


                    using (SmtpClient smtpMailObj = new SmtpClient())
                    {
                        smtpMailObj.Host = smtpHost;
                        smtpMailObj.Send(objEmail);
                    }

                    ////GetRecord carVal;
                    ////carVal = new GetRecord();
                    ////carVal.CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.Label1.Text.Trim())));

                    ////using (DataTable dt = carVal.Exec_Get_Existing_Car_By_Oid_Datatable())
                    ////{
                    ////    foreach (DataRow row in dt.Rows)
                    ////    {


                    ////    }
                    ////}
                }
            }
            catch (Exception ex)
            {
                uReturn = ex.Message;
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Send mail failure: " + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            }
            finally
            {
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Email notification has been sent.');", addScriptTags: true);
            }

            return uReturn;
        }

        private string SendMsgVendor(string carOid, string carNbr)
        {
            string uReturn = "Successfully";
            try
            {
                using (MailMessage objEmail = new MailMessage())
                {
                    MailAddress frmAddr;
                    frmAddr = new MailAddress(noReplyContact.ToString().Trim(), "NoReply");

                    MailAddress frmOriginatorAddr;
                    frmOriginatorAddr = new MailAddress(MySession.Current.SessionEmail.ToString(), MySession.Current.SessionFullName.ToString());

                    objEmail.From = frmAddr;
                    objEmail.Bcc.Add(frmOriginatorAddr);

                    string carStatus = string.Empty;
                    string bodyMsg = string.Empty;
                    string bobyTestWeb = string.Empty;

                    if (MySession.Current.SessionWebServer.ToString().ToUpper() == "TESTSITE")
                    {
                        bobyTestWeb = "<font size=3 face=Arial color=red><b>***  This is a Test...Testing...Please disregard this message  ***</b></font> <br><br>";
                    }

                    //// ** Vendor Recipients
                    foreach (RadListBoxItem item in RadListBox3.CheckedItems)
                    {
                        if (item.Checked)
                        {
                            if (item.Attributes["UserEmail"].ToString().Contains("@"))
                            {
                                objEmail.Bcc.Add(item.Attributes["UserEmail"].ToString());
                            }
                        }
                    }

                    GetRecord xCar;
                    xCar = new GetRecord
                    {
                        CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carOid)))
                    };

                    using (DataTable dt = xCar.Exec_Get_Existing_Car_By_Oid_Datatable())
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            carStatus = row["CAR_STATUS_NM"].ToString().ToUpper() + " - ";
                        }
                    }

                    objEmail.IsBodyHtml = true;
                    objEmail.Subject = carStatus + "Updated Corrective Action Request " + htmlUtil.SanitizeHtml(Server.HtmlDecode(carNbr)) + " (" + htmlUtil.SanitizeHtml(Server.HtmlDecode(HiddenVendorName.Value.ToString())) + ")";

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td align='center' style='background-color: #18B5F0'><b>***  Please do not reply to this email. This is an unmonitored address, and replies to this email cannot be responded to or read. ***</b></td></tr>");
                    sb.Append("<tr><td></td></tr>");
                    sb.Append("<tr><td>See attached Corrective Action Request - " + htmlUtil.SanitizeHtml(Server.HtmlDecode(carNbr)) + "</td></tr>");
                    sb.Append("<tr><td>For further information, please go to the <a href=" + HsnPortal + ">  Halliburton Supplier Network</a>, then select Corrective Action Request from the menu options</td></tr>");


                    sb.Append("</table>");
                    sb.Append("<br /><br />");
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td><b><u>Message from " + MySession.Current.SessionFullName.ToString() + ":</u></b></td></tr>");
                    sb.Append("<tr><td><pre>");
                    sb.Append(htmlUtil.SanitizeHtml(RadTextBox9.Text.Trim()));
                    sb.Append("</pre></td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br /><br />");
                    sb.Append("<br /><br />");

                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td><b><u>Message sent to the following recipient(s):</u></b></td></tr>");
                    foreach (RadListBoxItem item in RadListBox1.CheckedItems)
                    {
                        if (item.Checked)
                        {
                            if (item.Attributes["UserEmail"].ToString().Contains("@"))
                            {
                                sb.Append("<tr><td>");
                                sb.Append(htmlUtil.SanitizeHtml(item.Text.Trim()));
                                sb.Append("</td></tr>");
                            }
                        }
                    }

                    foreach (RadListBoxItem item in RadListBox3.CheckedItems)
                    {
                        if (item.Checked)
                        {
                            if (item.Attributes["UserEmail"].ToString().Contains("@"))
                            {
                                sb.Append("<tr><td>");
                                sb.Append(htmlUtil.SanitizeHtml(item.Text.Trim()));
                                sb.Append("</td></tr>");
                            }
                        }
                    }

                    sb.Append("</table>");
                    sb.Append("<br />");
                    sb.Append("<hr />");
                    sb.Append("This e - mail, including any attached files, may contain confidential and privileged information for the sole use of the intended recipient.  Any review, use, distribution, or disclosure by others is strictly prohibited. If you are not the intended recipient(or authorized to receive information for the intended recipient), please delete all copies of this message.");


                    objEmail.Priority = MailPriority.High;
                    objEmail.Body = bobyTestWeb + bodyMsg + sb.ToString(); ;

                    if (HiddenIssuedToUserId.Value.ToString().Trim().ToUpper() == "NON_HSN")
                    {
                        CreateDocx docx = new CreateDocx
                        {
                            Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carOid)))
                        };

                        using (MemoryStream memoryStream = docx.GetDocxFile())
                        {
                            byte[] bytes = memoryStream.ToArray();
                            memoryStream.Close();

                            Attachment attach = new Attachment(new MemoryStream(bytes), "CAR_" + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carNbr))) + ".docx");

                            objEmail.Attachments.Add(attach);

                        }
                    }
                    else
                    {
                        CreatePdf pdf = new CreatePdf
                        {
                            Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carOid)))
                        };

                        using (MemoryStream memoryStream = pdf.GetPdfFile())
                        {
                            byte[] bytes = memoryStream.ToArray();
                            memoryStream.Close();

                            Attachment attach = new Attachment(new MemoryStream(bytes), "CAR_" + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carNbr))) + ".pdf");

                            objEmail.Attachments.Add(attach);

                        }
                    }

                    using (SmtpClient smtpMailObj = new SmtpClient())
                    {
                        smtpMailObj.Host = smtpHost;
                        smtpMailObj.Send(objEmail);
                    }
                }
            }
            catch (Exception ex)
            {
                uReturn = ex.Message;
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Send mail failure: " + removeChar.SanitizeQuoteString(ex.Message) + "\\n\\n" + ErrException + "');", addScriptTags: true);
            }
            finally
            {
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Email notification has been sent.');", addScriptTags: true);
            }

            return uReturn;
        }

        protected void RadAjaxPanel1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            //try
            //{
            //    LoadDefaultData();

            //    if (htmlUtil.IsNumeric(e.Argument.ToString()))
            //    {
            //        this.Label1.Text = e.Argument.ToString();
            //        LoadExistingCar(e.Argument.ToString());
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);

            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
        }

        protected void RadButton7_Click(object sender, EventArgs e)
        {
            UpdatePreviewQuestion(RadTextBox11, 3);
            GoToNextTab(4);
        }

        protected void RadButton8_Click(object sender, EventArgs e)
        {
            UpdatePreviewQuestion(RadTextBox14, 4);
            GoToNextTab(5);
        }

        protected void RadButton4_Click(object sender, EventArgs e)
        {
            UpdatePreviewQuestion(RadTextBox15, 5);
            GoToNextTab(6);
        }

        protected void RadButton9_Click(object sender, EventArgs e)
        {
            UpdatePreviewQuestion(RadTextBox16, 6);
            GoToNextTab(7);
        }

        protected void RadButton10_Click(object sender, EventArgs e)
        {
            UpdatePreviewQuestion(RadTextBox17, 7);
            GoToNextTab(8);
        }

        protected void RadButton11_Click(object sender, EventArgs e)
        {
            UpdatePreviewActionTaken();
            GoToNextTab(9);
        }

        protected void RadButton12_Click(object sender, EventArgs e)
        {
            UpdatePreviewOriginatorUseOnly();

            //// **HES Recipients
            RadListBox1.Items.Clear();

            foreach (RadListBoxItem item in RadListBox3.CheckedItems)
            {
                //// ** Supplier Recipients
                item.Checked = false;
            }

            ////// ** Load Recipients that email was sent to

            GetRecord hes;
            hes = new GetRecord
            {
                CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Label1.Text))),
                VendorNbr = "0"
            };

            using (DataTable dt = hes.Exec_GetUser_Sent_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    AddToMailingList(row["USER_ID"].ToString().ToUpper(), row["USER_NAME"].ToString(), row["USER_EMAIL"].ToString(), true);
                }
            }

            //if (MySession.Current.SessionWebServer.ToString().ToUpper() == "TESTSITE")
            //{
            //    AddToSupplierMailingList(TestVendorUserID.ToUpper(), TestVendorName, MySession.Current.SessionEmail.ToString(), true);
            //}
            //else
            //{
            //// *** Load recipient with the same vendor number
            GetRecord vend;
            vend = new GetRecord
            {
                CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(Label1.Text))),
                VendorNbr = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(HiddenVendorNumber.Value.ToString().Trim())))
            };
            ;

            using (DataTable dt = vend.Exec_GetUser_Sent_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    AddToSupplierMailingList(row["USER_ID"].ToString().ToUpper(), row["USER_NAME"].ToString(), row["USER_EMAIL"].ToString(), true, HiddenVendorNumber.Value.ToString().Trim());
                }
            }
            //}

            //GetRecord getRec = new GetRecord();
            //getRec.CarOid = this.Label1.Text;

            //using (DataTable dt = getRec.Exec_GetCar_Email_Sent_Datatable())
            //{
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        if (row["VENDOR_NBR"].ToString() == "0")
            //        {
            //            AddToMailingList(row["USER_ID"].ToString().ToUpper(), row["USER_NAME"].ToString(), row["USER_EMAIL"].ToString(), true);
            //        }
            //        else
            //        {
            //            if (row["VENDOR_NBR"].ToString().Trim() == this.HiddenVendorNumber.Value.ToString().Trim())
            //            {

            //                if (MySession.Current.SessionWebServer.ToString().ToUpper() == "TESTSITE")
            //                {
            //                    AddToSupplierMailingList(TestVendorUserID.ToUpper(), TestVendorName, MySession.Current.SessionEmail.ToString(), true);
            //                }
            //                else
            //                {
            //                    //// *** Load recipient with the same vendor number
            //                    AddToSupplierMailingList(row["USER_ID"].ToString().ToUpper(), row["USER_NAME"].ToString(), row["USER_EMAIL"].ToString(), true);
            //                }
            //            }
            //        }
            //    }
            //}

            AddToMailingList(MySession.Current.SessionUserID.ToString().Trim().ToUpper(), MySession.Current.SessionFullName.ToString(), MySession.Current.SessionEmail.ToString().Trim().ToLower(), true);
            AddToMailingList(HiddenReIssuedUserId.Value.ToString().Trim().ToUpper(), HiddenReIssuedUserName.Value.ToString(), HiddenReIssuedUserEmail.Value.Trim().ToLower(), true);
            AddToMailingList(HiddenVerifiedByUserId.Value.ToString().Trim().ToUpper(), HiddenVerifiedByUserName.Value.ToString(), HiddenVerifiedByUserEmail.Value.Trim().ToLower(), true);
            AddToMailingList(HiddenAcceptedByUserId.Value.ToString().Trim().ToUpper(), HiddenAcceptedByUserName.Value.ToString(), HiddenAcceptedByUserEmail.Value.Trim().ToLower(), true);

            if (RadioButtonList2.Items[0].Selected == true)
            {
                AddToMailingList(HiddenIssuedToUserId.Value.ToString().Trim().ToUpper(), HiddenIssuedToUserName.Value.ToString(), HiddenIssuedToUserEmail.Value.Trim().ToLower(), true);
                AddToMailingList(HiddenResponsibleUserId.Value.ToString().Trim().ToUpper(), HiddenResponsibleUserName.Value.ToString(), HiddenResponsibleUserEmail.Value.Trim().ToLower(), true);
            }
            else
            {
                /// ** Supplier        /// 
                AddToSupplierMailingList(HiddenIssuedToUserId.Value.ToString().Trim().ToUpper(), HiddenIssuedToUserName.Value.ToString(), HiddenIssuedToUserEmail.Value.Trim().ToLower(), true, HiddenVendorNumber.Value.ToString().Trim());
                AddToSupplierMailingList(HiddenResponsibleUserId.Value.ToString().Trim().ToUpper(), HiddenResponsibleUserName.Value.ToString(), HiddenResponsibleUserEmail.Value.Trim().ToLower(), true, HiddenVendorNumber.Value.ToString().Trim());
            }

            if (RadioButtonList1.Items[0].Selected == true)
            {
                AddToMailingList(HiddenActionTakenUserId.Value.Trim().ToUpper(), HiddenActionTakenUserName.Value.ToString(), HiddenActionTakenUserEmail.Value.Trim().ToLower(), true);
            }
            else
            {
                /// ** Supplier        /// 
                AddToSupplierMailingList(HiddenActionTakenUserId.Value.Trim().ToUpper(), HiddenActionTakenUserName.Value.ToString(), HiddenActionTakenUserEmail.Value.Trim().ToLower(), true, HiddenVendorNumber.Value.ToString().Trim());
            }

            GoToNextTab(10);
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenActionTakenUserId.Value = string.Empty;
            HiddenActionTakenUserName.Value = string.Empty;
            HiddenActionTakenUserEmail.Value = string.Empty;

            RadComboBox15.Items.Clear();
            RadComboBox15.Text = string.Empty;
        }

        protected void RadioButtonList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenIssuedToUserId.Value = string.Empty;
            HiddenIssuedToUserName.Value = string.Empty;
            HiddenIssuedToUserEmail.Value = string.Empty;

            RadComboBox7.Items.Clear();
            RadComboBox7.Text = string.Empty;

            RadComboBox10.SelectedIndex = 0;
            RadTextBox10.Text = string.Empty;

        }

        // Application event log
        private void LogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog
            {
                ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionSource = "Internal_Forms_Edit.aspx",
                ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionUrl = string.Empty,
                ExceptionTargetSite = ex.TargetSite.ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionStackTrace = ex.StackTrace.ToString().Replace(oldValue: "'", newValue: "`")
            };
            appLog.AppLogEvent();
        }

        private void CustomLogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog
            {
                ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionSource = "Internal_Forms_Edit.aspx",
                ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionUrl = "CustomLogException",
                ExceptionTargetSite = ex.Data["TargetSite"].ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionStackTrace = ex.Data["StackTrace"].ToString().Replace(oldValue: "'", newValue: "`")
            };

            appLog.AppLogEvent();
        }

    }
}