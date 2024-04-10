using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CAR.Forms
{
    public partial class Start : System.Web.UI.Page
    {
        string TestVendorName = ConfigurationManager.AppSettings["TestVendorName"].ToString();
        string TestVendorUserID = ConfigurationManager.AppSettings["TestVendorUserID"].ToString();
        string TestVendorCity = ConfigurationManager.AppSettings["TestVendorCity"].ToString();
        string TestVendorCountry = ConfigurationManager.AppSettings["TestVendorCountry"].ToString();

        string HsnPortal = ConfigurationManager.AppSettings["HsnPortal"].ToString();
        string SmtpHost = ConfigurationManager.AppSettings["smptHostMail"].ToString();
        string NoReplyContact = ConfigurationManager.AppSettings["NoReplyContact"].ToString();

        public string AdminContact = ConfigurationManager.AppSettings["AdminContact"].ToString();
        public string ErrException = "If problem persists, please contact the administrator: " + ConfigurationManager.AppSettings["AdminContact"];
        SanitizeString removeChar = new SanitizeString();

        MyUtil.HtmlUtility htmlUtil = MyUtil.HtmlUtility.Instance;

        private const int ItemsPerRequest = 100;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (MySession.Current.SessionUserID == null || string.IsNullOrEmpty(MySession.Current.SessionUserID))
            {
                this.Session.Abandon();
                string script = "GetParentPage();";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", script, true);
            }

            if (!Page.IsPostBack)
            {
                try
                {
                    this.RadTabStrip1.Tabs[0].Visible = true;
                    this.RadMultiPage1.PageViews[0].Visible = true;

                    LoadDefaultData();
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
            this.Label1.Text = "0";
            this.Label2.Text = "0";

            this.Div1.InnerHtml = string.Empty;
            this.Div2.InnerHtml = string.Empty;

            this.Literal2.Text = string.Empty;
            this.Literal3.Text = string.Empty;
            this.Literal4.Text = string.Empty;
            this.Literal5.Text = string.Empty;
            this.Literal6.Text = string.Empty;
            this.Literal7.Text = string.Empty;
            this.Literal8.Text = string.Empty;
            this.Literal9.Text = string.Empty;
            this.Literal10.Text = string.Empty;
            this.Literal11.Text = string.Empty;
            this.Literal12.Text = string.Empty;
            this.Literal13.Text = string.Empty;
            this.Literal14.Text = string.Empty;
            this.Literal15.Text = string.Empty;
            this.Literal16.Text = string.Empty;
            this.Literal17.Text = string.Empty;
            this.Literal18.Text = string.Empty;
            //this.Literal19.Text = string.Empty;
            this.Literal20.Text = string.Empty;
            this.Literal21.Text = string.Empty;
            this.Literal22.Text = string.Empty;
            this.Literal23.Text = string.Empty;
            this.Literal24.Text = string.Empty;

            this.RadTextBox1.Text = string.Empty;
            this.RadTextBox2.Text = string.Empty;
            this.RadTextBox3.Text = string.Empty;
            this.RadTextBox4.Text = string.Empty;
            this.RadTextBox5.Text = string.Empty;
            this.RadTextBox6.Text = string.Empty;
            this.RadTextBox8.Text = string.Empty;
            this.RadTextBox9.Text = string.Empty;
            this.RadTextBox10.Text = string.Empty;
            this.RadTextBox12.Text = string.Empty;
            this.RadTextBox13.Text = string.Empty;

            this.RadTextBox11.Text = string.Empty;
            this.RadTextBox14.Text = string.Empty;

            this.RadComboBox1.Items.Clear();
            this.RadComboBox3.Items.Clear();
            this.RadComboBox3.ClearSelection();
            this.RadComboBox3.Text = string.Empty;

            this.RadComboBox10.Text = string.Empty;
            this.RadComboBox10.Items.Clear();
            this.RadComboBox10.ClearSelection();

            LoadDefaultRecipient(this.RadComboBox3);
            LoadFindingType(this.RadComboBox9);

            LoadArea(this.RadComboBox2);
            LoadFacility(this.RadComboBox5);
            LoadPsl(this.RadComboBox4);
            LoadApi_Iso(this.RadComboBox6);
            LoadCountry(this.RadComboBox10);
            LoadCategory(this.RadComboBox12);

            this.HiddenOriginatorUserId.Value = MySession.Current.SessionUserID.ToString();
            this.HiddenOriginatorUserName.Value = MySession.Current.SessionFullName.ToString();
            this.HiddenOriginatorUserEmail.Value = MySession.Current.SessionEmail.ToString();

            this.HiddenIssuedToUserId.Value = string.Empty;
            this.HiddenIssuedToUserName.Value = string.Empty;
            this.HiddenIssuedToUserEmail.Value = string.Empty;

            this.HiddenRecipientId.Value = string.Empty;
            this.HiddenRecipientName.Value = string.Empty;
            this.HiddenRecipientEmail.Value = string.Empty;

            this.HiddenVendorNumber.Value = string.Empty;
            this.HiddenVendorName.Value = string.Empty;

            this.HiddenCountryOid.Value = "0";
            this.HiddenAreaOid.Value = "0";

            this.RadComboBox11.Text = string.Empty;
            this.RadComboBox11.Items.Clear();
            this.RadComboBox11.ClearSelection();

            this.RadComboBox7.Text = string.Empty;
            this.RadComboBox7.Items.Clear();
            this.RadComboBox7.ClearSelection();

            this.HiddenRecipientId.Value = string.Empty;
            this.HiddenRecipientName.Value = string.Empty;
            this.HiddenRecipientEmail.Value = string.Empty;

            this.RadListBox1.Items.Clear();
            this.RadListBox2.Items.Clear();

            this.Label3.Text = string.Empty;

            this.RadDatePicker1.MinDate = DateTime.Now.AddMonths(-12);
            this.RadDatePicker1.SelectedDate = DateTime.Today;

            this.RadDatePicker2.MinDate = DateTime.Today.AddDays(-1);
            this.RadDatePicker2.SelectedDate = DateTime.Today.AddDays(30);

            this.Panel2.Visible = false;


            RadTab tabVendor = this.RadTabStrip1.FindTabByText("Supplier");
            RadTab tabIssuedTo = this.RadTabStrip1.FindTabByText("Issued To");
            RadTab tabNotification = this.RadTabStrip1.FindTabByText("Notification");
            RadTab tabFinish = this.RadTabStrip1.FindTabByText("Finish");
            tabVendor.Enabled = tabIssuedTo.Enabled = tabNotification.Enabled = tabFinish.Enabled = false;
        }

        protected void LoadExistingCar(string CarOid)
        {

            GetRecord xCar;
            xCar = new GetRecord();
            xCar.CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(CarOid)));

            using (DataTable dt = xCar.Exec_Get_Existing_Car_By_Oid_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    this.Label2.Text = htmlUtil.SanitizeHtml(Server.HtmlEncode(row["CAR_NBR"].ToString()));
                    SelectComboBoxByValue(this.RadComboBox9, row["FINDING_TYPE_OID"].ToString());
                    SelectComboBoxByValue(this.RadComboBox2, row["AREA_DESCRIPT_OID"].ToString());
                    SelectComboBoxByValue(this.RadComboBox5, row["FACILITY_NAME_OID"].ToString());
                    SelectComboBoxByValue(this.RadComboBox4, row["PSL_OID"].ToString());
                    SelectComboBoxByValue(this.RadComboBox10, row["LOC_COUNTRY_OID"].ToString());
                    SelectComboBoxByValue(this.RadComboBox12, row["CATEGORY_OID"].ToString());

                    GetRecord api;
                    api = new GetRecord();
                    api.CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(CarOid)));

                    using (DataTable dtApi = api.Exec_GetCar_Api_Iso_Datatable())
                    {
                        foreach (DataRow rowApi in dtApi.Rows)
                        {
                            RadComboBoxItem itemFound;
                            itemFound = this.RadComboBox6.FindItemByValue(rowApi["API_ISO_ELEMENTS_OID"].ToString());

                            if (itemFound != null)
                            {
                                itemFound.Checked = true;

                                RadComboBoxItem item;
                                item = new RadComboBoxItem();

                                item.Value = itemFound.Value;
                                item.Text = itemFound.Text;

                                this.RadComboBox1.Items.Add(item);

                            }
                        }
                    }

                    this.RadTextBox1.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["AUDIT_NBR"].ToString()));
                    this.RadTextBox2.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["QNOTE_NBR"].ToString()));
                    this.RadTextBox3.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["CPI_NBR"].ToString()));
                    this.RadTextBox4.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["MATERIAL_NBR"].ToString()));
                    this.RadTextBox5.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["PURCHASE_ORDER_NBR"].ToString()));
                    this.RadTextBox6.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["PRODUCTION_ORDER_NBR"].ToString()));
                    this.RadTextBox8.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["API_AUDIT_NBR"].ToString()));
                    this.RadTextBox10.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["LOC_SUPPLIER"].ToString()));

                    this.RadTextBox12.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["FINDING_DESC"].ToString()));

                    this.RadTextBox11.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["MAINTENANCE_ORDER_NBR"].ToString()));
                    this.RadTextBox14.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(row["EQUIPMENT_NBR"].ToString()));

                    this.RadDatePicker1.MinDate = DateTime.Now.AddMonths(-12);
                    this.RadDatePicker1.SelectedDate = DateTime.Today;

                    this.RadDatePicker2.MinDate = DateTime.Today.AddDays(-1);
                    this.RadDatePicker2.SelectedDate = DateTime.Today.AddDays(30);

                    ////this.RadDatePicker1.MinDate = DateTime.Parse(row["DATE_ISSUED"].ToString());
                    ////this.RadDatePicker1.SelectedDate = DateTime.Parse(row["DATE_ISSUED"].ToString());

                    ////this.RadDatePicker2.MinDate = DateTime.Parse(row["DUE_DT"].ToString());
                    ////this.RadDatePicker2.SelectedDate = DateTime.Parse(row["DUE_DT"].ToString());

                    LoadDefaultRecipient(this.RadComboBox3);
                    this.HiddenOriginatorUserId.Value = MySession.Current.SessionUserID.ToString();
                    this.HiddenOriginatorUserName.Value = MySession.Current.SessionFullName.ToString();
                    this.HiddenOriginatorUserEmail.Value = MySession.Current.SessionEmail.ToString();

                    ////if(!string.IsNullOrEmpty(row["ORIGINATOR_USR_ID"].ToString()))
                    ////{
                    ////    LoadHesRecipient(this.RadComboBox3, row["ORIGINATOR_USR_ID"].ToString());

                    ////    this.HiddenOriginatorUserId.Value = this.RadComboBox3.SelectedItem.Attributes["UserID"].ToString();
                    ////    this.HiddenOriginatorUserName.Value = this.RadComboBox3.SelectedItem.Attributes["FullName"].ToString();
                    ////    this.HiddenOriginatorUserEmail.Value = this.RadComboBox3.SelectedItem.Attributes["Email"].ToString();
                    ////}

                    if (int.Parse(row["AREA_DESCRIPT_OID"].ToString()) == 2) //Supplier, Vendor
                    {
                        LoadVendor(this.RadComboBox11, row["VNDR_NBR"].ToString());
                        this.HiddenVendorNumber.Value = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["VNDR_NBR"].ToString())));
                        this.HiddenVendorName.Value = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(row["VENDOR_NM"].ToString())));

                        LoadVendorRecipient(this.RadComboBox7, Server.HtmlEncode(htmlUtil.SanitizeHtml(row["ISSUED_TO_USR_ID"].ToString())), Server.HtmlEncode(htmlUtil.SanitizeHtml(row["VNDR_NBR"].ToString())));
                    }
                    else
                    {
                        LoadHesRecipient(this.RadComboBox7, row["ISSUED_TO_USR_ID"].ToString());
                    }

                    if (this.RadComboBox7.Items.Count > 0)
                    {
                        this.HiddenIssuedToUserId.Value = this.RadComboBox7.SelectedItem.Attributes["UserID"].ToString().Trim().ToUpper();
                        this.HiddenIssuedToUserName.Value = this.RadComboBox7.SelectedItem.Attributes["FullName"].ToString();
                        this.HiddenIssuedToUserEmail.Value = this.RadComboBox7.SelectedItem.Attributes["Email"].ToString();
                        this.RadTextBox10.Text = this.RadComboBox7.SelectedItem.Attributes["CITY"].ToString();

                        RadComboBoxItem itemFound;
                        itemFound = this.RadComboBox10.FindItemByText(this.RadComboBox7.SelectedItem.Attributes["CNTRY_NM"].ToString());

                        if (itemFound != null)
                        {
                            itemFound.Selected = true;
                        }

                    }

                    UpdatePreviewStart();
                    UpdatePreviewVendor();
                    UpdatePreviewIssuedTo();

                }
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
                    item = new RadComboBoxItem();
                    item.Value = row["OID"].ToString().Trim();
                    item.Text = row["API_ISO_ELEM"].ToString().Trim();
                    item.ToolTip = row["API_ISO_ELEM"].ToString();

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
                    item = new RadComboBoxItem();
                    item.Value = row["OID"].ToString().Trim();
                    item.Text = row["AREA_DESCR"].ToString().Trim();
                    item.ToolTip = row["AREA_DESCR"].ToString();
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
                    item = new RadComboBoxItem();
                    item.Value = row["OID"].ToString().Trim();
                    item.Text = row["CATEGORY_NM"].ToString().Trim();
                    item.ToolTip = row["CATEGORY_NM"].ToString();
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
                    item = new RadComboBoxItem();
                    item.Value = row["OID"].ToString().Trim();
                    item.Text = row["FINDING_TYPE"].ToString().Trim();
                    item.ToolTip = row["FINDING_TYPE"].ToString();
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
            //try
            //{
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
                    item = new RadComboBoxItem();
                    item.Value = row["OID"].ToString().Trim();
                    item.Text = row["NM"].ToString().Trim();
                    item.ToolTip = row["NM"].ToString();
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


        protected void LoadDefaultRecipient(RadComboBox comboBox)
        {
            //try
            //{

            RadComboBoxItem itemFound;
            itemFound = comboBox.FindItemByValue(MySession.Current.SessionUserID.ToString().ToUpper());
            if (itemFound == null)
            {
                RadComboBoxItem item;
                item = new RadComboBoxItem();

                item.Font.Bold = true;

                item.Text = MySession.Current.SessionFullName.ToString();
                item.ToolTip = MySession.Current.SessionFullName.ToString();
                item.Value = MySession.Current.SessionUserID.ToString().Trim().ToUpper();

                item.Attributes.Add(key: "UserId", value: MySession.Current.SessionUserID.ToString().Trim().ToUpper());
                item.Attributes.Add(key: "FullName", value: MySession.Current.SessionFullName.ToString());
                item.Attributes.Add(key: "CITY", value: MySession.Current.SessionUserCity.ToString());
                item.Attributes.Add(key: "CNTRY_NM", value: MySession.Current.SessionUserCountry.ToString());
                item.Attributes.Add(key: "Email", value: MySession.Current.SessionEmail.ToString());
                item.Selected = true;

                comboBox.Items.Add(item);
                item.DataBind();
            }


            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);

            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
        }

        protected void LoadHesRecipient(RadComboBox comboBox, string uid)
        {
            //try
            //{
            if (!string.IsNullOrEmpty(uid))
            {
                comboBox.ClearSelection();
                comboBox.Items.Clear();
                comboBox.Text = string.Empty;

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
                        item.Value = row["UserID"].ToString().Trim().ToUpper();

                        item.Attributes.Add(key: "UserId", value: row["UserID"].ToString().Trim().ToUpper());
                        item.Attributes.Add(key: "FullName", value: row["Firstname"].ToString() + " " + row["Lastname"].ToString());
                        item.Attributes.Add(key: "CITY", value: row["CITY"].ToString());
                        item.Attributes.Add(key: "CNTRY_NM", value: row["CNTRY_NM"].ToString());
                        item.Attributes.Add(key: "Email", value: row["Email"].ToString());
                        item.Selected = true;

                        comboBox.Items.Add(item);
                        item.DataBind();

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

        protected void LoadVendorRecipient(RadComboBox comboBox, string uid, string vid)
        {
            //try
            //{
            if (!string.IsNullOrEmpty(uid))
            {
                comboBox.ClearSelection();
                comboBox.Items.Clear();
                comboBox.Text = string.Empty;

                GetVendors ven;
                ven = new GetVendors();
                ven.UserId = uid;
                ven.VendorNumber = vid;
                ven.Active = "";

                using (DataTable dt = ven.GetVendorUserByUid())
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        RadComboBoxItem item;
                        item = new RadComboBoxItem();

                        item.Font.Bold = true;

                        item.Text = row["Firstname"].ToString() + " " + row["Lastname"].ToString();
                        item.ToolTip = row["Firstname"].ToString() + " " + row["Lastname"].ToString();
                        item.Value = row["UserID"].ToString().Trim().ToUpper();

                        item.Attributes.Add(key: "UserId", value: row["UserID"].ToString().Trim().ToUpper());
                        item.Attributes.Add(key: "FullName", value: row["Firstname"].ToString() + " " + row["Lastname"].ToString());
                        item.Attributes.Add(key: "CITY", value: string.Empty);
                        item.Attributes.Add(key: "CNTRY_NM", value: string.Empty);
                        item.Attributes.Add(key: "Email", value: row["EmailAddr"].ToString());
                        item.Selected = true;

                        comboBox.Items.Add(item);
                        item.DataBind();

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

        protected void LoadVendor(RadComboBox comboBox, string vid)
        {
            //try
            //{
            if (!string.IsNullOrEmpty(vid))
            {
                comboBox.ClearSelection();
                comboBox.Items.Clear();
                comboBox.Text = string.Empty;

                GetVendors ven;
                ven = new GetVendors();
                ven.SearchText = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(vid)));

                using (DataTable dt = ven.GetVendorList())
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        RadComboBoxItem item;
                        item = new RadComboBoxItem();

                        item.Font.Bold = true;

                        item.Text = row["VendorNumber"].ToString().Trim() + " - " + row["VendorName"].ToString().Trim();
                        item.ToolTip = row["VendorNumber"].ToString().Trim() + " - " + row["VendorName"].ToString().Trim();
                        item.Value = row["VendorNumber"].ToString().Trim();

                        item.Attributes.Add(key: "VendorNumber", value: row["VendorNumber"].ToString().Trim());
                        item.Attributes.Add(key: "VendorName", value: row["VendorName"].ToString().Trim());
                        item.Selected = true;

                        comboBox.Items.Add(item);
                        item.DataBind();

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

        protected void LoadFacility(RadComboBox comboBox)
        {
            //try
            //{
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
                    item.Text = row["PLNT_NM"].ToString().Trim();
                    item.ToolTip = row["FACILITY_NM"].ToString();
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

        protected void LoadPsl(RadComboBox comboBox)
        {
            //try
            //{
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
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);

            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
        }

        protected void RadButton1_Click(object sender, EventArgs e)
        {
            //try
            //{
            UpdatePreviewStart();

            if (this.RadComboBox2.SelectedValue == "2") //Supplier
            {
                this.RadioButtonList2.Items[1].Selected = true;
                this.RadioButtonList2.Items[1].Enabled = true;

                GoToNextTab(1);
            }
            else
            {

                //this.RadTextBox10.Text = string.Empty; //Location City State

                //this.Literal10.Text = string.Empty; //Vendor Number
                //this.Literal11.Text = string.Empty; //Vendor Name
                //this.Literal12.Text = string.Empty;//Issued To name
                //this.Literal21.Text = string.Empty; //Issued To User ID
                //this.Literal22.Text = string.Empty; //Location Country
                //this.Literal23.Text = string.Empty; //Location City State

                //this.HiddenVendorNumber.Value = "";
                //this.HiddenVendorName.Value = "";
                //this.HiddenIssuedToUserId.Value = "";
                //this.HiddenIssuedToUserName.Value = "";
                //this.HiddenIssuedToUserEmail.Value = "";

                //this.RadComboBox7.Text = "";
                //this.RadComboBox7.ClearSelection();
                //this.RadComboBox7.Items.Clear();

                //this.RadComboBox10.Text = "";
                //this.RadComboBox10.ClearSelection();

                //this.RadComboBox11.Text = "";
                //this.RadComboBox11.ClearSelection();
                //this.RadComboBox11.Items.Clear();

                this.RadioButtonList2.Items[1].Selected = false;
                this.RadioButtonList2.Items[0].Selected = true;
                this.RadioButtonList2.Items[1].Enabled = false;

                GoToNextTab(2);
            }
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);
            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
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

                this.RadTextBox9.Text = "Attention,  Having 30 days for you to reply, this notification is intended to confirm attached Corrective Action Request has been assigned to you, in order to describe and submit your pertinent corrective and preventive actions response with all associated evidences as, 5Why's, training records, purchase orders associated (as needed), etc.";

                UpdatePreviewIssuedTo();

                this.Panel1.Visible = false;

                this.RadListBox1.Items.Clear();
                RadComboBox radComboBox8;
                radComboBox8 = (RadComboBox)this.RadListBox1.Header.FindControl(id: "RadComboBox8");

                if (this.RadComboBox2.SelectedValue == "2") //Supplier
                {
                    this.RadListBox1.Height = Unit.Pixel(160);
                    this.RadListBox3.Items.Clear();

                    this.Panel1.Visible = true;

                    //if (MySession.Current.SessionWebServer.ToString().ToUpper() == "TESTSITE")
                    //{
                    //    RadListBoxItem item;
                    //    item = new RadListBoxItem();

                    //    item.Text = TestVendorName;
                    //    item.ToolTip = TestVendorName;
                    //    item.Value = TestVendorUserID;

                    //    item.Attributes.Add(key: "UserId", value: TestVendorUserID);
                    //    item.Attributes.Add(key: "FullName", value: TestVendorName);
                    //    item.Attributes.Add(key: "CITY", value: TestVendorCity);
                    //    item.Attributes.Add(key: "CNTRY_NM", value: TestVendorCountry);
                    //    item.Attributes.Add(key: "UserEmail", value: MySession.Current.SessionEmail.ToString());
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
                    ven.VendorNumber = Server.HtmlEncode(htmlUtil.SanitizeHtml(this.HiddenVendorNumber.Value));

                    using (DataTable dt = ven.GetVendorUserList())
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            RadListBoxItem item;
                            item = new RadListBoxItem();

                            item.Text = row["FullName"].ToString();
                            item.Value = row["UserID"].ToString().ToUpper();
                            item.ToolTip = row["FullName"].ToString();
                            item.Checkable = true;
                            item.Attributes["UserId"] = row["UserID"].ToString().ToUpper();
                            item.Attributes["UserEmail"] = row["EmailAddr"].ToString().ToLower();
                            item.Attributes["FullName"] = row["FullName"].ToString();
                            item.Attributes["VendorNumber"] = this.HiddenVendorNumber.Value.ToString().Trim();

                            if (row["UserID"].ToString().Trim().ToUpper() == this.HiddenIssuedToUserId.Value.ToString().Trim().ToUpper())
                            {
                                item.Checked = true;
                                item.Selected = true;
                            }

                            this.RadListBox3.Items.Add(item);
                            this.RadListBox3.SortItems();
                        }
                    }

                    ////Manually added recipient
                    ///
                    if (this.HiddenIssuedToUserId.Value.ToString().Trim().ToUpper() == "NON_HSN")
                    {
                        RadListBoxItem item;
                        item = new RadListBoxItem();

                        item.Text = this.HiddenIssuedToUserName.Value.Trim();
                        item.Value = this.HiddenIssuedToUserId.Value.Trim();
                        item.ToolTip = this.HiddenIssuedToUserName.Value.Trim();
                        item.Checked = true;
                        item.Checkable = true;
                        item.Attributes["UserId"] = this.HiddenIssuedToUserId.Value.Trim().Trim().ToUpper();
                        item.Attributes["UserEmail"] = this.HiddenIssuedToUserEmail.Value.Trim().ToUpper();
                        item.Attributes["FullName"] = this.HiddenIssuedToUserName.Value.Trim();
                        item.Attributes["VendorNumber"] = this.HiddenVendorNumber.Value.ToString().Trim();

                        this.RadListBox3.Items.Add(item);
                        this.RadListBox3.SortItems();
                    }

                    //}

                    if (radComboBox8 != null)
                    {
                        RadListBoxItem itemFound;
                        itemFound = this.RadListBox1.FindItemByValue(MySession.Current.SessionUserID.ToString().Trim().ToUpper());
                        if (itemFound == null)
                        {
                            RadListBoxItem item;
                            item = new RadListBoxItem();

                            item.Text = MySession.Current.SessionFullName.ToString();
                            item.Value = MySession.Current.SessionUserID.ToString().ToUpper();
                            item.ToolTip = MySession.Current.SessionFullName.ToString();
                            item.Checked = true;
                            item.Checkable = true;
                            //item.Selected = true;
                            item.Attributes["UserId"] = MySession.Current.SessionUserID.ToString().ToUpper();
                            item.Attributes["UserEmail"] = MySession.Current.SessionEmail.ToString().Trim().ToLower();
                            item.Attributes["FullName"] = MySession.Current.SessionFullName.ToString();

                            this.RadListBox1.Items.Add(item);
                            this.RadListBox1.SortItems();
                        }
                    }
                }
                else
                {
                    this.RadListBox1.Height = Unit.Pixel(310);

                    if (radComboBox8 != null)
                    {
                        RadListBoxItem itemFound;
                        itemFound = this.RadListBox1.FindItemByValue(this.HiddenIssuedToUserId.Value.ToString().Trim().ToUpper());
                        if (itemFound == null)
                        {
                            RadListBoxItem item;
                            item = new RadListBoxItem();

                            item.Text = this.HiddenIssuedToUserName.Value.ToString();
                            item.Value = this.HiddenIssuedToUserId.Value.ToString().ToUpper();
                            item.ToolTip = this.HiddenIssuedToUserName.Value.ToString();
                            item.Checked = true;
                            item.Checkable = true;
                            //item.Selected = true;
                            item.Attributes["UserId"] = this.HiddenIssuedToUserId.Value.ToString().ToUpper();
                            item.Attributes["UserEmail"] = this.HiddenIssuedToUserEmail.Value.ToString().Trim().ToLower();
                            item.Attributes["FullName"] = this.HiddenIssuedToUserName.Value.ToString();

                            this.RadListBox1.Items.Add(item);
                            this.RadListBox1.SortItems();
                        }

                        itemFound = this.RadListBox1.FindItemByValue(MySession.Current.SessionUserID.ToString().ToUpper());
                        if (itemFound == null)
                        {
                            RadListBoxItem item;
                            item = new RadListBoxItem();

                            item.Text = MySession.Current.SessionFullName.ToString();
                            item.Value = MySession.Current.SessionUserID.ToString().ToUpper();
                            item.ToolTip = MySession.Current.SessionFullName.ToString();
                            item.Checked = true;
                            item.Checkable = true;
                            //item.Selected = true;
                            item.Attributes["UserId"] = MySession.Current.SessionUserID.ToString().ToUpper();
                            item.Attributes["UserEmail"] = MySession.Current.SessionEmail.ToString().Trim().ToLower();
                            item.Attributes["FullName"] = MySession.Current.SessionFullName.ToString();

                            this.RadListBox1.Items.Add(item);
                            this.RadListBox1.SortItems();
                        }

                    }
                }

                GoToNextTab(3);
            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            }
        }
        protected void RadButton4_Click(object sender, EventArgs e)
        {
            try
            {
                RadComboBox radComboBox8;
                radComboBox8 = (RadComboBox)this.RadListBox1.Header.FindControl(id: "RadComboBox8");

                if (radComboBox8 != null)
                {
                    if (!string.IsNullOrEmpty(radComboBox8.Text.Trim()) && !string.IsNullOrEmpty(radComboBox8.SelectedValue.ToString().Trim()))
                    {
                        RadListBoxItem itemFound = this.RadListBox1.FindItemByValue(radComboBox8.SelectedValue.Trim().ToUpper());

                        if (itemFound == null)
                        {
                            RadListBoxItem item;
                            item = new RadListBoxItem();

                            item.Text = this.HiddenRecipientName.Value.ToString();
                            item.Value = radComboBox8.SelectedValue.Trim().Trim().ToUpper();
                            item.ToolTip = this.HiddenRecipientName.Value.ToString();
                            item.Checked = true;
                            item.Checkable = true;
                            item.Selected = true;
                            item.Attributes["UserId"] = this.HiddenRecipientId.Value.ToString().Trim().ToUpper();
                            item.Attributes["UserEmail"] = this.HiddenRecipientEmail.Value;
                            item.Attributes["FullName"] = this.HiddenRecipientName.Value;

                            this.RadListBox1.Items.Add(item);
                            this.RadListBox1.SortItems();
                        }
                        else
                        {
                            itemFound.Selected = true;
                        }
                        radComboBox8.Text = string.Empty;
                        this.HiddenRecipientId.Value = string.Empty;
                        this.HiddenRecipientEmail.Value = string.Empty;
                        this.HiddenRecipientName.Value = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);

                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            }
        }


        protected void RadButton5_Click(object sender, EventArgs e)
        {
            try
            {
                string[] uReturn;
                string newCarOid = "0";
                string car_Nbr = "0";
                string newCar_Nbr = "0";

                CreateRecord newCar;
                newCar = new CreateRecord
                {
                    Created_By = MySession.Current.SessionUserID.ToString(),
                    Originator_Usr_Id = this.HiddenOriginatorUserId.Value.ToString().ToUpper(),
                    Date_Issued = this.Literal3.Text,
                    Due_dt = this.Literal4.Text,
                    Finding_Type_Oid = this.RadComboBox9.SelectedItem.Value,
                    Area_Descript_Oid = this.RadComboBox2.SelectedItem.Value,
                    Psl_Oid = this.RadComboBox4.SelectedItem.Value,
                    Facility_Name_Oid = this.RadComboBox5.SelectedItem.Value,
                    Finding_Desc = Server.HtmlEncode(htmlUtil.SanitizeHtml(this.Div1.InnerHtml.ToString().Trim())),
                    Desc_Of_Improvement = Server.HtmlEncode(htmlUtil.SanitizeHtml(this.Div2.InnerHtml.ToString().Trim())),
                    Category_Oid = this.RadComboBox12.SelectedItem.Value,
                    Audit_Nbr = this.Literal13.Text,
                    Qnote_Nbr = this.Literal14.Text,
                    Cpi_Nbr = this.Literal15.Text,
                    Material_Nbr = this.Literal16.Text,
                    Purchase_Order_Nbr = this.Literal17.Text,
                    Production_Order_Nbr = this.Literal18.Text,
                    Api_Audit_Nbr = this.Literal20.Text,
                    Vndr_Nbr = this.Literal10.Text,
                    Vendor_Nm = this.Literal11.Text,
                    Issued_To_Usr_Id = this.Literal21.Text.Trim().ToUpper(),
                    Loc_Country_Oid = this.HiddenCountryOid.Value,
                    Loc_Supplier = this.Literal23.Text,
                    Issued_To_Usr_Email = this.Literal19.Text,
                    Issued_To_Usr_Name = Server.HtmlEncode(htmlUtil.SanitizeHtml(this.Literal12.Text.Trim())),

                    Maintenance_Order_Nbr = this.Literal24.Text,
                    Equipment_Nbr = this.Literal25.Text
                };

                uReturn = newCar.ExecCreateCar();

                if (uReturn[0].ToString() == "Successfully")
                {

                    car_Nbr = "0000" + uReturn[3].ToString();
                    newCar_Nbr = uReturn[2].ToString() + "-" + car_Nbr.Substring(car_Nbr.Length - 5);

                    this.Label1.Text = uReturn[1].ToString();
                    this.Label2.Text = newCar_Nbr;

                    this.Label3.Text = newCar_Nbr;
                    RadButton7.Text = "Edit - " + newCar_Nbr;
                    this.RadButton7.Value = newCar_Nbr;

                    newCarOid = uReturn[1].ToString();

                    foreach (RadComboBoxItem itemChecked in this.RadComboBox6.CheckedItems)
                    {
                        string uApisoReturn;

                        CreateRecord newApiso;
                        newApiso = new CreateRecord();
                        newApiso.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(newCarOid)));
                        newApiso.Api_Iso_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(itemChecked.Value)));
                        uApisoReturn = newApiso.ExecCreateCar_Api_Iso();

                        if (uApisoReturn.ToString() != "Successfully")
                        {
                            int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                            Exception aex = new Exception(removeChar.SanitizeQuoteString(uApisoReturn.ToString()));
                            aex.Data.Add(key: "TargetSite", value: "newApiso.ExecCreateCar_Api_Iso()");
                            aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                            CustomLogException(aex);
                        }
                    }

                    string msgReturn = string.Empty;
                    msgReturn = SendMsgHes(newCarOid, newCar_Nbr);

                    if (msgReturn == "Successfully")
                    {
                        this.RadListBox2.Items.Clear();

                        string[] mailReturn = { string.Empty, string.Empty };

                        CreateRecord mail;
                        mail = new CreateRecord();
                        mail.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(newCarOid)));
                        mail.Email_Subject = "Corrective Action Request - " + newCar_Nbr;
                        mail.Email_Message = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox9.Text.Trim())));
                        mail.User_Id = MySession.Current.SessionUserID.ToString();
                        mail.User_Name = MySession.Current.SessionFullName.ToString();

                        mailReturn = mail.ExecCreateCar_Email();

                        if (mailReturn[0].ToString() == "Successfully")
                        {
                            foreach (RadListBoxItem node1 in this.RadListBox1.CheckedItems)
                            {
                                if (node1.Checked && node1.Value != "Check All")
                                {
                                    string sReturn = string.Empty;

                                    CreateRecord sendTo;
                                    sendTo = new CreateRecord();
                                    sendTo.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(newCarOid)));
                                    sendTo.Car_Email_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(mailReturn[1].ToString())));
                                    sendTo.User_Id = node1.Attributes["UserId"].ToString().Trim().ToUpper();
                                    sendTo.User_Name = node1.Attributes["FullName"].ToString().Trim();
                                    sendTo.User_Email = node1.Attributes["UserEmail"].ToString().Trim();
                                    sendTo.Vendor_Nm = "0";

                                    sReturn = sendTo.ExecCreateCar_Email_Sent_To();

                                    if (sReturn.ToString() == "Successfully")
                                    {
                                        RadListBoxItem item;
                                        item = new RadListBoxItem();

                                        item.Text = removeChar.SanitizeQuoteString(node1.Attributes["FullName"].ToString().Trim());
                                        item.Value = node1.Attributes["UserID"].ToString().Trim().ToUpper();

                                        this.RadListBox2.Items.Add(node1);
                                        this.RadListBox2.SortItems();
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

                            if (this.RadComboBox2.SelectedValue == "2") //Supplier
                            {
                                msgReturn = SendMsgVendor(newCarOid, newCar_Nbr);

                                if (msgReturn == "Successfully")
                                {
                                    foreach (RadListBoxItem node3 in this.RadListBox3.CheckedItems)
                                    {
                                        if (node3.Checked && node3.Value != "Check All")
                                        {
                                            string sReturn = string.Empty;

                                            CreateRecord sendTo;
                                            sendTo = new CreateRecord();
                                            sendTo.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(newCarOid)));
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
                                                item.Value = node3.Attributes["UserID"].ToString().Trim().ToUpper();

                                                this.RadListBox2.Items.Add(node3);
                                                this.RadListBox2.SortItems();
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

                    for (int i = 0; i <= (this.RadTabStrip1.Tabs.Count - 1); i++)
                    {
                        this.RadTabStrip1.Tabs[i].Enabled = false;
                    }

                    GoToNextTab(4);
                }
                else
                {
                    this.Label1.Text = Server.HtmlEncode(htmlUtil.SanitizeHtml(uReturn[0].ToString()));
                    this.Label2.Text = string.Empty;

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(uReturn[0].ToString()) + "');", addScriptTags: true);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                this.Label2.Text = Server.HtmlEncode(htmlUtil.SanitizeHtml(ex.Message));
                //ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
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
            this.RadTabStrip1.Tabs[tabIndex].Selected = true;
            this.RadTabStrip1.Tabs[tabIndex].Enabled = true;

            this.RadMultiPage1.SelectedIndex = tabIndex;
            this.RadTabStrip1.CausesValidation = false;
        }

        private void UpdatePreviewStart()
        {
            this.Literal1.Text = this.HiddenOriginatorUserId.Value.ToString().ToUpper();
            this.Literal2.Text = this.HiddenOriginatorUserName.Value;
            this.Literal3.Text = this.RadDatePicker1.SelectedDate.Value.ToShortDateString();
            this.Literal4.Text = this.RadDatePicker2.SelectedDate.Value.ToShortDateString();
            this.Literal5.Text = this.RadComboBox9.SelectedItem.Text.Trim();
            this.Literal6.Text = this.RadComboBox2.SelectedItem.Text.Trim();
            this.Literal7.Text = this.RadComboBox4.SelectedItem.Text.Trim();
            this.Literal8.Text = this.RadComboBox5.SelectedItem.Text.Trim();
            this.Literal9.Text = this.RadComboBox12.SelectedItem.Text.Trim();

            this.RadComboBox1.Items.Clear();

            foreach (RadComboBoxItem itemChecked in this.RadComboBox6.CheckedItems)
            {
                RadComboBoxItem item;
                item = new RadComboBoxItem();

                item.Value = itemChecked.Value;
                item.Text = itemChecked.Text;

                this.RadComboBox1.Items.Add(item);
            }

            this.Literal13.Text = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox1.Text.Trim())));
            this.Literal14.Text = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox2.Text.Trim())));
            this.Literal15.Text = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox3.Text.Trim())));
            this.Literal16.Text = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox4.Text.Trim())));
            this.Literal17.Text = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox5.Text.Trim())));
            this.Literal18.Text = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox6.Text.Trim())));

            this.Literal20.Text = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox8.Text.Trim())));
            this.Literal24.Text = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox11.Text.Trim())));
            this.Literal25.Text = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox14.Text.Trim())));

            this.Div1.InnerHtml = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox12.Text.Trim())));
            this.Div2.InnerHtml = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox13.Text.Trim())));

            this.HiddenAreaOid.Value = this.RadComboBox2.SelectedItem.Value;

            this.Panel2.Visible = true;
        }

        private void UpdatePreviewVendor()
        {
            this.Literal10.Text = htmlUtil.SanitizeHtml(this.HiddenVendorNumber.Value.ToString());
            this.Literal11.Text = htmlUtil.SanitizeHtml(this.HiddenVendorName.Value.ToString());
        }

        private void UpdatePreviewIssuedTo()
        {
            if (!string.IsNullOrEmpty(this.HiddenIssuedToUserId.Value.ToString().Trim()))
            {
                ////User selected from known list
                ///                
                this.Literal21.Text = this.HiddenIssuedToUserId.Value.ToString();  //User ID                                                          

            }
            else
            {
                //// Manually typed and unknown user
                ///
                this.Literal21.Text = "NON_HSN";  //User ID
                this.HiddenIssuedToUserId.Value = "NON_HSN";  //User ID
                this.HiddenIssuedToUserEmail.Value = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox7.Text.Trim())));  //Email
            }

            this.Literal12.Text = this.HiddenIssuedToUserName.Value.ToString();  //User Name
            this.Literal19.Text = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox7.Text.Trim())));  //Email
            this.Literal22.Text = this.RadComboBox10.SelectedItem.Text;  // Country
            this.HiddenCountryOid.Value = this.RadComboBox10.SelectedItem.Value;  // Country

            this.Literal23.Text = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox10.Text.Trim()))); //City State
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
                this.RadComboBox3.Items.Clear();

                string srchText = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(e.Text.Trim())));

                HesUsers hesUser;
                hesUser = new HesUsers();
                hesUser.SearchText = srchText;

                using (DataTable dt = hesUser.GetHesUsers())
                {

                    if (MySession.Current.SessionWebServer.ToString().ToUpper() == "TESTSITE")
                    {
                        int itemOffset = e.NumberOfItems;
                        int endOffset = Math.Min(itemOffset + ItemsPerRequest, 1);
                        e.EndOfItems = endOffset == 1;

                        RadComboBoxItem item;
                        item = new RadComboBoxItem();

                        item.Font.Bold = true;

                        item.Text = TestVendorName;
                        item.ToolTip = TestVendorName;
                        item.Value = TestVendorUserID;

                        item.Attributes.Add(key: "UserId", value: TestVendorUserID);
                        item.Attributes.Add(key: "FullName", value: TestVendorName);
                        item.Attributes.Add(key: "CITY", value: TestVendorCity);
                        item.Attributes.Add(key: "CNTRY_NM", value: TestVendorCountry);
                        item.Attributes.Add(key: "Email", value: MySession.Current.SessionEmail.ToString());

                        item.Selected = true;
                        item.Checked = true;

                        this.RadComboBox3.Items.Add(item);
                        item.DataBind();
                        e.Message = GetStatusMessage(endOffset, 1);
                    }
                    else
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
                            this.RadComboBox3.Items.Add(item);

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

        protected void RadComboBox7_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                this.RadComboBox7.Items.Clear();

                string srchText = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(e.Text.Trim())));

                ////if (this.RadComboBox2.SelectedValue == "2")
                if (this.RadioButtonList2.SelectedIndex > 0)
                {
                    GetVendors venUser;
                    venUser = new GetVendors();
                    venUser.SearchText = srchText;
                    venUser.VendorNumber = Server.HtmlEncode(htmlUtil.SanitizeHtml(this.HiddenVendorNumber.Value));

                    using (DataTable dt = venUser.GetVendorUserList())
                    {
                        if (dt.Rows.Count > 0)
                        {
                            //// HSN user list
                            ///
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
                                this.RadComboBox7.Items.Add(item);

                                item.DataBind();
                            }

                            e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
                        }
                        else
                        {
                            ////Not found in HSN user list



                        }

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
                        //    int itemOffset = e.NumberOfItems;
                        //    int endOffset = Math.Min(itemOffset + ItemsPerRequest, dt.Rows.Count);
                        //    e.EndOfItems = endOffset == dt.Rows.Count;

                        //    for (int i = itemOffset; i < endOffset; i++)
                        //    {
                        //        RadComboBoxItem item;
                        //        item = new RadComboBoxItem();

                        //        item.Font.Bold = true;

                        //        item.Text = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                        //        item.ToolTip = dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim();
                        //        item.Value = dt.Rows[i]["UserID"].ToString().Trim().ToUpper();

                        //        item.Attributes.Add(key: "UserId", value: dt.Rows[i]["UserID"].ToString().Trim().ToUpper());
                        //        item.Attributes.Add(key: "FullName", value: dt.Rows[i]["FirstName"].ToString().Trim() + " " + dt.Rows[i]["LastName"].ToString().Trim());
                        //        item.Attributes.Add(key: "CITY", value: "");
                        //        item.Attributes.Add(key: "CNTRY_NM", value: "");
                        //        item.Attributes.Add(key: "Email", value: dt.Rows[i]["EmailAddr"].ToString().Trim());
                        //        this.RadComboBox7.Items.Add(item);

                        //        item.DataBind();
                        //    }

                        //    e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
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
                            this.RadComboBox7.Items.Add(item);

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

        protected void RadComboBox8_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                RadComboBox radComboBox8;
                radComboBox8 = (RadComboBox)this.RadListBox1.Header.FindControl(id: "RadComboBox8");

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
                string srchText = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(e.Text.Trim())));

                GetVendors vendor;
                vendor = new GetVendors();
                vendor.SearchText = srchText;

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

                        this.RadComboBox11.Items.Add(item);

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

        protected void RadTabStrip1_TabClick(object sender, RadTabStripEventArgs e)
        {
            try
            {
                //this.RadTabStrip1.Tabs[e.Tab.Index].Selected = true;

                int curTab = e.Tab.Index + 1;

                for (int i = curTab; i <= (this.RadTabStrip1.Tabs.Count - 1); i++)
                {
                    this.RadTabStrip1.Tabs[i].Enabled = false;
                }

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
                    frmAddr = new MailAddress(NoReplyContact.ToString().Trim(), "NoReply");

                    MailAddress frmOriginatorAddr;
                    frmOriginatorAddr = new MailAddress(MySession.Current.SessionEmail.ToString(), MySession.Current.SessionFullName.ToString());

                    objEmail.From = frmAddr;
                    objEmail.Bcc.Add(frmOriginatorAddr);

                    string carStatus = string.Empty;
                    string bodyMsg = string.Empty;
                    string bobyTestWeb = string.Empty;

                    if (MySession.Current.SessionWebServer.ToString().ToUpper() == "TESTSITE")
                    {
                        bobyTestWeb = "<font size=3 face=Arial color=red><b>***  This is a Test...Testing...Please do not reply to this message  ***</b></font> <br><br>";
                    }

                    ////HES
                    foreach (RadListBoxItem item in this.RadListBox1.CheckedItems)
                    {
                        if (item.Checked)
                        {
                            if (item.Attributes["UserEmail"].ToString().Contains("@"))
                            {
                                objEmail.To.Add(item.Attributes["UserEmail"].ToString());
                            }
                        }
                    }

                    GetRecord xCar;
                    xCar = new GetRecord();
                    xCar.CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carOid)));

                    using (DataTable dt = xCar.Exec_Get_Existing_Car_By_Oid_Datatable())
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            carStatus = row["CAR_STATUS_NM"].ToString().ToUpper() + " - ";
                        }
                    }

                    objEmail.IsBodyHtml = true;
                    objEmail.Subject = carStatus + "Corrective Action Request - " + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carNbr)));

                    if (this.RadComboBox2.SelectedValue == "2") //Supplier
                    {
                        objEmail.Subject += " (" + htmlUtil.SanitizeHtml(this.HiddenVendorName.Value.ToString()) + ")";
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td align='center' style='background-color: #18B5F0'><b>***  Please do not reply to this email. This is an unmonitored address, and replies to this email cannot be responded to or read. ***</b></td></tr>");
                    sb.Append("<tr><td></td></tr>");
                    sb.Append("<tr><td>See attached Corrective Action Request <a href=" + HostName + "/default.aspx?form=car&indx=" + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carOid))) + "> #" + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carNbr))) + "</a></td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br /><br />");
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td><b><u>Message from " + MySession.Current.SessionFullName.ToString() + ":</u></b></td></tr>");
                    sb.Append("<tr><td><pre>");
                    sb.Append(htmlUtil.SanitizeHtml(this.RadTextBox9.Text.Trim()));
                    sb.Append("</pre></td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br /><br />");
                    sb.Append("<br /><br />");

                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td><b><u>Message sent to the following recipient(s):</u></b></td></tr>");
                    foreach (RadListBoxItem item in this.RadListBox1.CheckedItems)
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

                    foreach (RadListBoxItem item in this.RadListBox3.CheckedItems)
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


                    if (this.HiddenIssuedToUserId.Value.ToString().Trim().ToUpper() == "NON_HSN")
                    {
                        CreateNewDocx docx = new CreateNewDocx();
                        docx.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carOid)));

                        using (MemoryStream memoryStream = docx.GetNewDocxFile())
                        {
                            byte[] bytes = memoryStream.ToArray();
                            memoryStream.Close();

                            Attachment attach = new Attachment(new MemoryStream(bytes), "CAR_" + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carNbr))) + ".docx");

                            objEmail.Attachments.Add(attach);

                        }

                    }
                    else
                    {
                        CreateNewPdf pdf = new CreateNewPdf();
                        pdf.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carOid)));

                        using (MemoryStream memoryStream = pdf.GetNewPdfFile())
                        {
                            byte[] bytes = memoryStream.ToArray();
                            memoryStream.Close();

                            Attachment attach = new Attachment(new MemoryStream(bytes), "CAR_" + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carNbr))) + ".pdf");

                            objEmail.Attachments.Add(attach);

                        }
                    }


                    using (SmtpClient smtpMailObj = new SmtpClient())
                    {
                        smtpMailObj.Host = SmtpHost;
                        smtpMailObj.Send(objEmail);
                    }
                }
            }
            catch (Exception ex)
            {
                uReturn = ex.Message;
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Send mail failure: " + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
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
            string HostName = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;

            try
            {
                using (MailMessage objEmail = new MailMessage())
                {
                    MailAddress frmAddr;
                    frmAddr = new MailAddress(NoReplyContact.ToString().Trim(), "NoReply");

                    MailAddress frmOriginatorAddr;
                    frmOriginatorAddr = new MailAddress(MySession.Current.SessionEmail.ToString(), MySession.Current.SessionFullName.ToString());

                    objEmail.From = frmAddr;
                    objEmail.Bcc.Add(frmOriginatorAddr);

                    string carStatus = string.Empty;
                    string bodyMsg = string.Empty;
                    string bobyTestWeb = string.Empty;

                    if (MySession.Current.SessionWebServer.ToString().ToUpper() == "TESTSITE")
                    {
                        bobyTestWeb = "<font size=3 face=Arial color=red><b>***  This is a Test...Testing...Please do not reply to this message  ***</b></font> <br><br>";
                    }

                    //// ** Vendor Recipients
                    foreach (RadListBoxItem item in this.RadListBox3.CheckedItems)
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
                    xCar = new GetRecord();
                    xCar.CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carOid)));

                    using (DataTable dt = xCar.Exec_Get_Existing_Car_By_Oid_Datatable())
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            carStatus = row["CAR_STATUS_NM"].ToString().ToUpper() + " - ";
                        }
                    }

                    objEmail.IsBodyHtml = true;
                    objEmail.Subject = carStatus + "Corrective Action Request - " + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carNbr))) + ")";

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td align='center' style='background-color: #18B5F0'><b>***  Please do not reply to this email. This is an unmonitored address, and replies to this email cannot be responded to or read. ***</b></td></tr>");
                    sb.Append("<tr><td></td></tr>");
                    sb.Append("<tr><td>See attached Corrective Action Request " + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carNbr))) + "</td></tr>");
                    sb.Append("<tr><td>For further information, please go to the <a href=" + HsnPortal + ">  Halliburton Supplier Network</a>, then select Corrective Action Request from the menu options.</td></tr>");
                    sb.Append("</table>");

                    sb.Append("<br /><br />");
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td><b><u>Message from " + MySession.Current.SessionFullName.ToString() + ":</u></b></td></tr>");
                    sb.Append("<tr><td><pre>");
                    sb.Append(htmlUtil.SanitizeHtml(this.RadTextBox9.Text.Trim()));
                    sb.Append("</pre></td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br /><br />");
                    sb.Append("<br /><br />");

                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td><b><u>Message sent to the following recipient(s):</u></b></td></tr>");
                    foreach (RadListBoxItem item in this.RadListBox1.CheckedItems)
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

                    foreach (RadListBoxItem item in this.RadListBox3.CheckedItems)
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


                    if (this.HiddenIssuedToUserId.Value.ToString().Trim().ToUpper() == "NON_HSN")
                    {
                        CreateNewDocx docx = new CreateNewDocx();
                        docx.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carOid)));

                        using (MemoryStream memoryStream = docx.GetNewDocxFile())
                        {
                            byte[] bytes = memoryStream.ToArray();
                            memoryStream.Close();

                            Attachment attach = new Attachment(new MemoryStream(bytes), "CAR_" + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carNbr))) + ".docx");

                            objEmail.Attachments.Add(attach);

                        }

                    }
                    else
                    {

                        CreateNewPdf pdf = new CreateNewPdf();
                        pdf.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carOid)));

                        using (MemoryStream memoryStream = pdf.GetNewPdfFile())
                        {
                            byte[] bytes = memoryStream.ToArray();
                            memoryStream.Close();

                            Attachment attach = new Attachment(new MemoryStream(bytes), "CAR_" + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(carNbr))) + ".pdf");

                            objEmail.Attachments.Add(attach);

                        }
                    }

                    using (SmtpClient smtpMailObj = new SmtpClient())
                    {
                        smtpMailObj.Host = SmtpHost;
                        smtpMailObj.Send(objEmail);
                    }
                }
            }
            catch (Exception ex)
            {
                uReturn = ex.Message;
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Send mail failure: " + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            }
            finally
            {
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Email notification has been sent.');", addScriptTags: true);
            }

            return uReturn;
        }

        //private string SendMsg(string newOId)
        //{
        //    string uReturn = "Successfully";
        //    string HostName = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;

        //    try
        //    {
        //        using (MailMessage objEmail = new MailMessage())
        //        {
        //            MailAddress frmAddr;
        //            frmAddr = new MailAddress(NoReplyContact.ToString().Trim(), "NoReply");

        //            MailAddress frmOriginatorAddr;
        //            frmOriginatorAddr = new MailAddress(MySession.Current.SessionEmail.ToString(), MySession.Current.SessionFullName.ToString());

        //            objEmail.From = frmAddr;
        //            objEmail.Bcc.Add(frmOriginatorAddr);

        //            string bodyMsg = string.Empty;
        //            string bobyTestWeb = string.Empty;

        //            if (MySession.Current.SessionWebServer.ToString().ToUpper() == "TESTSITE")
        //            {
        //                bobyTestWeb = "<font size=3 face=Arial color=red><b>***  This is a Test...Testing...Please do not reply to this message  ***</b></font> <br><br>";
        //            }

        //            ////HES
        //            foreach (RadListBoxItem item in this.RadListBox1.CheckedItems)
        //            {
        //                if (item.Checked)
        //                {
        //                    if (item.Attributes["UserEmail"].ToString().Contains("@"))
        //                    {
        //                        objEmail.To.Add(item.Attributes["UserEmail"].ToString());
        //                    }
        //                }
        //            }

        //            ////Supplier
        //            foreach (RadListBoxItem item in this.RadListBox3.CheckedItems)
        //            {
        //                if (item.Checked)
        //                {
        //                    if (item.Attributes["UserEmail"].ToString().Contains("@"))
        //                    {
        //                        objEmail.To.Add(item.Attributes["UserEmail"].ToString());
        //                    }
        //                }
        //            }

        //            objEmail.IsBodyHtml = true;

        //            GetRecord carVal;
        //            carVal = new GetRecord();
        //            carVal.CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(newOId)));

        //            using (DataTable dt = carVal.Exec_Get_Existing_Car_By_Oid_Datatable())
        //            {
        //                foreach (DataRow row in dt.Rows)
        //                {
        //                    objEmail.Subject = "CAR No. " + row["CAR_NBR"].ToString() + ";  " + removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(this.RadTextBox7.Text.Trim())));

        //                    bodyMsg += "<font size=2 face=Arial color=red><b>***  Please do not reply to this email. This is an unmonitored address, and replies to this email cannot be responded to or read. ***</b></font><br><br>";
        //                    bodyMsg += "<font size=2 face=Arial><a href=" + HostName + "/default.aspx?form=car&indx=" + newOId + ">Halliburton Employee Click Here To View CAR No. " + row["CAR_NBR"].ToString() + "</a></font><br>";

        //                    if (this.RadListBox3.CheckedItems.Count > 0)
        //                    {
        //                        bodyMsg += "<br><font size=2 face=Arial><a href=" + HsnPortal + ">Halliburton Supplier Must go to the HSN Portal to  View CAR No. " + row["CAR_NBR"].ToString() + "</a></font><br>";
        //                    }

        //                    bodyMsg += "<hr><table><br>";
        //                    bodyMsg += "<table>";
        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>Message:</font></th></tr>";
        //                    bodyMsg += "<tr><td><font size=2 face=Arial><pre   style=font - size: 15px;  font-family: Arial;>" + Server.HtmlDecode(this.RadTextBox9.Text.Trim()) + "</pre></font></td></tr>";
        //                    bodyMsg += "</table>";
        //                    bodyMsg += "<hr><table><br>";
        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>Originator:</font></th><td><font size=2 face=Arial>" + row["ORIGINATOR_USR_NM"].ToString() + "</font></td></tr>";
        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>Date Issued:</font></th><td><font size=2 face=Arial>" + row["DATE_ISSUED"].ToString() + "</font></td></tr>";
        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>Due Date:</font></th><td><font size=2 face=Arial>" + row["DUE_DT"].ToString() + "</font></td></tr>";
        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>Finding Type:</font></th><td><font size=2 face=Arial>" + row["FINDING_TYPE_NM"].ToString() + "</font></td></tr>";
        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>Area:</font></th><td><font size=2 face=Arial>" + Server.HtmlDecode(row["AREA_DESCRIPT_NM"].ToString()) + "</font></td></tr>";
        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>PSL:</font></th><td><font size=2 face=Arial>" + row["PSL_NM"].ToString() + "</font></td></tr>";
        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>Plant:</font></th><td><font size=2 face=Arial>" + row["FACILITY_NAME"].ToString() + "</font></td></tr>";

        //                    bodyMsg += "<tr><th align=left valign=top><font size=2 face=Arial>Description of Finding:</font></th><td><font size=2 face=Arial><pre>" + Server.HtmlDecode(row["FINDING_DESC"].ToString()) + "</pre></font></td></tr>";
        //                    bodyMsg += "<tr><th align=left valign=top><font size=2 face=Arial>Description of Improvement:</font></th><td><font size=2 face=Arial><pre>" + Server.HtmlDecode(row["DESC_OF_IMPROVEMENT"].ToString()) + "</pre></font></td></tr>";

        //                    bodyMsg += "<tr><th align=left valign=top><font size=2 face=Arial>API / ISO Reference:</font></th><td valign=top><table>";

        //                    GetRecord apiso;
        //                    apiso = new GetRecord();
        //                    apiso.CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(newOId)));

        //                    using (DataTable dtApiso = apiso.Exec_GetCar_Api_Iso_Datatable())
        //                    {
        //                        foreach (DataRow rowApiso in dtApiso.Rows)
        //                        {
        //                            bodyMsg += "<tr><td><font size=2 face=Arial>" + rowApiso["API_ISO_ELEM"].ToString() + "</font></td></tr>";
        //                        }
        //                    }
        //                    bodyMsg += "</table></td></tr>";

        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>Audit Number:</font></th><td><font size=2 face=Arial>" + row["AUDIT_NBR"].ToString() + "</font></td></tr>";
        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>Q Note Number:</font></th><td><font size=2 face=Arial>" + row["QNOTE_NBR"].ToString() + "</font></td></tr>";
        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>CPI Number:</font></th><td><font size=2 face=Arial>" + row["CPI_NBR"].ToString() + "</font></td></tr>";
        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>Material Number:</font></th><td><font size=2 face=Arial>" + row["MATERIAL_NBR"].ToString() + "</font></td></tr>";
        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>Purchase Order Number:</font></th><td><font size=2 face=Arial>" + row["PURCHASE_ORDER_NBR"].ToString() + "</font></td></tr>";
        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>Production Order Number:</font></th><td><font size=2 face=Arial>" + row["PRODUCTION_ORDER_NBR"].ToString() + "</font></td></tr>";
        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>Category:</font></th><td><font size=2 face=Arial>" + row["CATEGORY_NM"].ToString() + "</font></td></tr>";
        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>Vendor Number:</font></th><td><font size=2 face=Arial>" + row["VNDR_NBR"].ToString() + "</font></td></tr>";
        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>Vendor Name:</font></th><td><font size=2 face=Arial>" + row["VENDOR_NM"].ToString() + "</font></td></tr>";
        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>Issued To:</font></th><td><font size=2 face=Arial>" + row["ISSUED_TO_USR_NM"].ToString() + "</font></td></tr>";
        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>Location/Country:</font></th><td><font size=2 face=Arial>" + row["LOC_COUNTRY_NM"].ToString() + "</font></td></tr>";
        //                    bodyMsg += "<tr><th align=left><font size=2 face=Arial>Location/City State:</font></th><td><font size=2 face=Arial>" + row["LOC_SUPPLIER"].ToString() + "</font></td></tr>";
        //                    bodyMsg += "</table>";
        //                }

        //                if (!string.IsNullOrEmpty(bodyMsg))
        //                {
        //                    objEmail.Priority = MailPriority.High;
        //                    objEmail.Body = bobyTestWeb + bodyMsg;

        //                    using (SmtpClient smtpMailObj = new SmtpClient())
        //                    {
        //                        smtpMailObj.Host = SmtpHost;
        //                        smtpMailObj.Send(objEmail);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        uReturn = ex.Message;
        //        LogException(ex);
        //        ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Send mail failure: " + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
        //    }
        //    finally
        //    {
        //        ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Email notification has been sent.');", addScriptTags: true);
        //    }

        //    return uReturn;
        //}


        protected void RadAjaxPanel1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            try
            {
                LoadDefaultData();

                if (htmlUtil.IsNumeric(e.Argument.ToString()))
                {
                    this.Label1.Text = e.Argument.ToString();
                    LoadExistingCar(e.Argument.ToString());
                }
            }
            catch (Exception ex)
            {
                LogException(ex);

                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            }
        }

        protected void RadioButtonList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.HiddenIssuedToUserId.Value = string.Empty;
            this.HiddenIssuedToUserName.Value = string.Empty;
            this.HiddenIssuedToUserEmail.Value = string.Empty;

            this.RadComboBox7.Items.Clear();
            this.RadComboBox7.Text = string.Empty;

            this.RadComboBox10.SelectedIndex = 0;
            this.RadTextBox7.Text = string.Empty;
            this.RadTextBox10.Text = string.Empty;
        }

        // Application event log
        private void LogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog();

            appLog.ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionSource = "Internal_Forms_Start.aspx";
            appLog.ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionUrl = string.Empty;
            appLog.ExceptionTargetSite = ex.TargetSite.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionStackTrace = ex.StackTrace.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.AppLogEvent();
        }

        private void CustomLogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog();

            appLog.ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionSource = "Internal_Forms_Start.aspx";
            appLog.ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionUrl = "CustomLogException";
            appLog.ExceptionTargetSite = ex.Data["TargetSite"].ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionStackTrace = ex.Data["StackTrace"].ToString().Replace(oldValue: "'", newValue: "`");

            appLog.AppLogEvent();
        }
    }
}