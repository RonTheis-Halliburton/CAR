using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;

namespace CAR.Forms
{
    public partial class Search : System.Web.UI.Page
    {
        public string AdminContact = ConfigurationManager.AppSettings["AdminContact"].ToString();
        public string ErrException = "If problem persists, please contact the administrator: " + ConfigurationManager.AppSettings["AdminContact"];
        SanitizeString removeChar = new SanitizeString();
        MyUtil.HtmlUtility htmlUtil = MyUtil.HtmlUtility.Instance;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (MySession.Current.SessionUserID == null || string.IsNullOrEmpty(MySession.Current.SessionUserID))
            {
                this.Session.Abandon();
                string script = "GetParentPage();";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", script, true);
            }

            this.RadGrid1.HeaderContextMenu.ItemCreated += new RadMenuEventHandler(HeaderContextMenu_ItemCreated);

            if (!Page.IsPostBack)
            {
                try
                {
                    LoadDefaultData();

                    object obj = this.Session["SearchResult"];
                    if ((!(obj == null)))
                    {
                        LoadCookie();
                    }

                    if (!string.IsNullOrEmpty(MySession.Current.SessionUrlNdex.ToString()))
                    {
                        LoadGridSearchByOid(MySession.Current.SessionUrlNdex.ToString());

                        MySession.Current.SessionUrlNdex = string.Empty;
                        MySession.Current.SessionUrlForm = string.Empty;

                    }
                    else
                    {
                        //this.RadTextBox1.Text = "2018-00803";
                        //this.RadTextBox1.Text = "2019-000";

                        this.RadTextBox1.Text = DateTime.Today.Year.ToString() + "-0000";
                    }

                    this.CurrentUserID.Value = MySession.Current.SessionUserID.ToUpper();
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
            LoadSearchBy(this.RadListBox1);
            LoadStatus(this.RadComboBox7);
            LoadArea(this.RadComboBox6);
            LoadFindingType(this.RadComboBox1);
            LoadFacility(this.RadComboBox2);
            LoadPsl(this.RadComboBox3);
            LoadCategory(this.RadComboBox4);
            LoadApi_Iso(this.RadComboBox5);
            LoadSearchDate(this.RadComboBox8);

            if (!MySession.Current.SessionCarAdmin)
            {
                RadMenuItem itemAdmin;
                itemAdmin = this.RadContextMenu1.FindItemByValue("Admin");
                if (itemAdmin != null)
                {
                    itemAdmin.Visible = false;
                }

                RadMenuItem itemOpen;
                itemOpen = this.RadContextMenu1.FindItemByValue("Open");
                if (itemOpen != null)
                {
                    itemOpen.Visible = false;
                }

            }
        }

        protected void LoadSearchBy(RadListBox radListBox)
        {
            //try
            //{
            radListBox.ClearSelection();
            radListBox.Items.Clear();

            GetRecord getRec;
            getRec = new GetRecord();

            using (DataTable dt = getRec.Exec_GetSarchBy_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    RadListBoxItem item;
                    item = new RadListBoxItem();
                    item.Value = row["SEARCH_FIELD"].ToString().Trim();
                    item.Text = row["SEARCH_TEXT"].ToString().Trim();
                    item.ToolTip = row["SEARCH_TEXT"].ToString();

                    if (int.Parse(row["FIELD_SELECTED"].ToString()) == 1)
                    {
                        item.Checked = true;
                    }

                    radListBox.Items.Add(item);
                }
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
                    item = new RadComboBoxItem();
                    item.Value = row["OID"].ToString().Trim();
                    item.Text = row["NM"].ToString().Trim();
                    item.ToolTip = row["NM"].ToString();

                    if (int.Parse(row["OID"].ToString()) != 3)
                    {
                        item.Checked = true;
                    }

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

        protected void LoadArea(RadComboBox radComboBox)
        {
            //try
            //{
            radComboBox.ClearSelection();
            radComboBox.Items.Clear();

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

                    //if (int.Parse(row["FIELD_SELECTED"].ToString()) == 1)
                    //{
                    //    item.Checked = true;
                    //}

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

                //comboBox.Items.Insert(index: 0, item: new RadComboBoxItem(text: "ALL", value: "0"));
                //comboBox.SelectedIndex = 0;
            }
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);

            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
        }

        protected void LoadSearchDate(RadComboBox comboBox)
        {
            //try
            //{
            comboBox.ClearSelection();
            comboBox.Text = string.Empty;
            comboBox.Items.Clear();

            GetRecord getRec;
            getRec = new GetRecord();

            using (DataTable dt = getRec.Exec_GetSearchDate_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    RadComboBoxItem item;
                    item = new RadComboBoxItem();
                    item.Value = row["SEARCH_FIELD"].ToString().Trim();
                    item.Text = row["SEARCH_TEXT"].ToString().Trim();
                    item.ToolTip = row["SEARCH_TEXT"].ToString();
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

                //comboBox.Items.Insert(index: 0, item: new RadComboBoxItem(text: "ALL", value: "0"));
                //comboBox.SelectedIndex = 0;
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

                //comboBox.Items.Insert(index: 0, item: new RadComboBoxItem(text: "ALL", value: "0"));
                //comboBox.SelectedIndex = 0;
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

                //comboBox.Items.Insert(index: 0, item: new RadComboBoxItem(text: "ALL", value: "0"));
                //comboBox.SelectedIndex = 0;
            }
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);

            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
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

        protected void RadButton1_Click(object sender, EventArgs e)
        {
            this.Session["SearchResult"] = null;
            this.RadGrid1.MasterTableView.CurrentPageIndex = 0;
            this.RadGrid1.MasterTableView.SortExpressions.Clear();
            this.RadGrid1.DataSource = (DataTable)this.Session["SearchResult"];
            this.RadGrid1.DataBind();

            if (!string.IsNullOrEmpty(this.RadTextBox1.Text.Trim()))
            {
                LoadGridSearch();
            }
        }

        protected void LoadCookie()
        {
            //try
            //{
            HttpCookie myCookie = Request.Cookies["CorrectiveActionSearch"];
            if (myCookie != null)
            {
                this.RadTextBox1.Text = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(myCookie.Values["SearchText"].ToString())));

                SetListBoxByValue(this.RadListBox1, myCookie.Values["SearchBy"].ToString());
                SetComboByValue(this.RadComboBox7, myCookie.Values["StatusOid"].ToString());
                SetComboByValue(this.RadComboBox6, myCookie.Values["AreaOid"].ToString());
                SetComboByValue(this.RadComboBox1, myCookie.Values["FindingTypeOid"].ToString());
                SetComboByValue(this.RadComboBox2, myCookie.Values["PlantOid"].ToString());
                SetComboByValue(this.RadComboBox3, myCookie.Values["PslOid"].ToString());
                SetComboByValue(this.RadComboBox4, myCookie.Values["CategoryOid"].ToString());
                SetComboByValue(this.RadComboBox5, myCookie.Values["ApiIsoOid"].ToString());

                SelectComboByValue(this.RadComboBox8, myCookie.Values["SearchDate"].ToString());
                SelectComboByValue(this.RadComboBox9, myCookie.Values["Deleted"].ToString());
                SelectComboByValue(this.RadComboBox10, myCookie.Values["Indexed"].ToString());

                if (htmlUtil.IsDate(myCookie.Values["DateFrom"].ToString()))
                {
                    this.RadDatePicker1.SelectedDate = DateTime.Parse(myCookie.Values["DateFrom"].ToString());
                }

                if (htmlUtil.IsDate(myCookie.Values["DateTo"].ToString()))
                {
                    this.RadDatePicker2.SelectedDate = DateTime.Parse(myCookie.Values["DateTo"].ToString());
                }

            }
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);

            //    ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
        }

        protected void SelectComboByValue(RadComboBox comboBox, string comboValue)
        {
            string[] strValue = comboValue.Split(separator: ',');

            foreach (string val in strValue)
            {
                RadComboBoxItem item;
                item = comboBox.FindItemByValue(val);

                if (item != null)
                {
                    item.Selected = true;
                }
            }
        }

        protected void SetComboByValue(RadComboBox comboBox, string comboValue)
        {
            string[] strValue = comboValue.Split(separator: ',');

            foreach (string val in strValue)
            {
                RadComboBoxItem item;
                item = comboBox.FindItemByValue(val);

                if (item != null)
                {
                    item.Checked = true;
                }
            }
        }

        protected void SetListBoxByValue(RadListBox listBox, string comboValue)
        {
            string[] strValue = comboValue.Split(separator: '|');

            foreach (string val in strValue)
            {
                RadListBoxItem item;
                item = listBox.FindItemByValue(val);

                if (item != null)
                {
                    item.Checked = true;
                }
            }
        }

        protected void LoadGridSearchByOid(string Car_oid)
        {
            GetSearch getSearch;
            getSearch = new GetSearch();
            getSearch.CarOid = Car_oid;
            getSearch.CarAdmin = MySession.Current.SessionCarAdmin.ToString();

            using (DataTable dt = getSearch.SearchCarByOid())
            {
                if (dt.Rows.Count > 0)
                {
                    this.RadTextBox1.Text = removeChar.SanitizeQuoteString(Server.HtmlEncode(dt.Rows[0]["CAR_NBR"].ToString()));
                    this.RadGrid1.DataSource = dt;
                    this.RadGrid1.DataBind();

                    this.Session["SearchResult"] = dt;
                }
            }
        }

        protected void LoadGridSearch()
        {
            //try
            //{
            HttpCookie aCookie;
            aCookie = new HttpCookie(name: "CorrectiveActionSearch");
            //aCookie.Secure = true;
            //aCookie.HttpOnly = true;

            GetSearch getSearch;
            getSearch = new GetSearch();

            getSearch.SrchText = removeChar.SanitizeQuoteString(Server.HtmlEncode(this.RadTextBox1.Text.Trim()));
            aCookie.Values.Add(name: "SearchText", value: Server.HtmlDecode(this.RadTextBox1.Text.Trim()));

            StringBuilder searchFieldValue = GetListByValue(this.RadListBox1);
            if (searchFieldValue.Length > 0)
            {
                getSearch.SrchField = searchFieldValue.ToString();
            }
            else
            {
                getSearch.SrchField = string.Empty;
            }

            aCookie.Values.Add(name: "SearchBy", value: searchFieldValue.ToString());

            StringBuilder statusValue = GetComboByValue(this.RadComboBox7);
            if (statusValue.Length > 0)
            {
                getSearch.StatusOid = statusValue.ToString();
            }
            else
            {
                getSearch.StatusOid = string.Empty;
            }

            aCookie.Values.Add(name: "StatusOid", value: statusValue.ToString());

            StringBuilder areaValue = GetComboByValue(this.RadComboBox6);
            if (areaValue.Length > 0)
            {
                getSearch.AreaOid = areaValue.ToString();
            }
            else
            {
                getSearch.AreaOid = string.Empty;
            }

            aCookie.Values.Add(name: "AreaOid", value: areaValue.ToString());

            StringBuilder findingValue = GetComboByValue(this.RadComboBox1);
            if (findingValue.Length > 0)
            {
                getSearch.FindingTypeOid = findingValue.ToString();
            }
            else
            {
                getSearch.FindingTypeOid = string.Empty;
            }

            aCookie.Values.Add(name: "FindingTypeOid", value: findingValue.ToString());

            StringBuilder plantValue = GetComboByValue(this.RadComboBox2);
            if (plantValue.Length > 0)
            {
                getSearch.PlantOid = plantValue.ToString();
            }
            else
            {
                getSearch.PlantOid = string.Empty;
            }

            aCookie.Values.Add(name: "PlantOid", value: plantValue.ToString());

            StringBuilder pslValue = GetComboByValue(this.RadComboBox3);
            if (pslValue.Length > 0)
            {
                getSearch.PslOid = pslValue.ToString();
            }
            else
            {
                getSearch.PslOid = string.Empty;
            }

            aCookie.Values.Add(name: "PslOid", value: pslValue.ToString());

            StringBuilder categoryValue = GetComboByValue(this.RadComboBox4);
            if (categoryValue.Length > 0)
            {
                getSearch.CategoryOid = categoryValue.ToString();
            }
            else
            {
                getSearch.CategoryOid = string.Empty;
            }

            aCookie.Values.Add(name: "CategoryOid", value: categoryValue.ToString());

            StringBuilder apiIsoValue = GetComboByValue(this.RadComboBox5);
            if (apiIsoValue.Length > 0)
            {
                getSearch.ApiIsoOid = apiIsoValue.ToString();
            }
            else
            {
                getSearch.ApiIsoOid = string.Empty;
            }

            aCookie.Values.Add(name: "ApiIsoOid", value: apiIsoValue.ToString());

            if (this.RadComboBox8.SelectedIndex > 0)
            {
                getSearch.SearchDate = this.RadComboBox8.SelectedValue;
            }
            else
            {
                getSearch.SearchDate = string.Empty;
            }

            aCookie.Values.Add(name: "SearchDate", value: this.RadComboBox8.SelectedValue);

            if (this.RadDatePicker1.IsEmpty == false)
            {
                getSearch.DateFrom = this.RadDatePicker1.SelectedDate.Value.ToShortDateString();
                aCookie.Values.Add(name: "DateFrom", value: this.RadDatePicker1.SelectedDate.Value.ToShortDateString());
            }
            else
            {
                getSearch.DateFrom = string.Empty;
                aCookie.Values.Add(name: "DateFrom", value: string.Empty);
            }

            if (this.RadDatePicker2.IsEmpty == false)
            {
                getSearch.DateTo = this.RadDatePicker2.SelectedDate.Value.ToShortDateString();
                aCookie.Values.Add(name: "DateTo", value: this.RadDatePicker2.SelectedDate.Value.ToShortDateString());
            }
            else
            {
                getSearch.DateTo = string.Empty;
                aCookie.Values.Add(name: "DateTo", value: string.Empty);
            }

            getSearch.CarAdmin = MySession.Current.SessionCarAdmin.ToString();
            getSearch.OriginatorManager = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(MySession.Current.SessionManager.ToString())));
            getSearch.OriginatorManagerID = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(MySession.Current.SessionManagerID.ToString().ToUpper())));
            getSearch.OriginatorManagerEmail = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(MySession.Current.SessionManagerEmail.ToString())));

            getSearch.Deleted = this.RadComboBox9.SelectedValue;
            aCookie.Values.Add(name: "Deleted", value: this.RadComboBox9.SelectedValue);

            getSearch.Indexed = this.RadComboBox10.SelectedValue;
            aCookie.Values.Add(name: "Indexed", value: this.RadComboBox10.SelectedValue);

            aCookie.Expires = DateTime.Now.AddHours(value: 1);
            Response.Cookies.Add(aCookie);

            using (DataTable dt = getSearch.SearchCar())
            {
                this.RadGrid1.DataSource = dt;
                this.RadGrid1.DataBind();

                this.Session["SearchResult"] = dt;
            }
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);
            //    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + ErrException + "<br>" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
        }

        public StringBuilder GetComboByValue(RadComboBox comboBox)
        {
            var sb = new StringBuilder();
            int i = 0;
            foreach (RadComboBoxItem checkeditem in comboBox.CheckedItems)
            {
                if (i == 0)
                {
                    sb.Append(checkeditem.Value.ToString());
                }
                else
                {
                    sb.Append("," + checkeditem.Value.ToString());
                }

                i++;
            }

            return sb;
        }

        public StringBuilder GetListByValue(RadListBox listBox)
        {
            var sb = new StringBuilder();
            int i = 0;

            foreach (RadListBoxItem checkeditem in listBox.CheckedItems)
            {
                if (i == 0)
                {
                    sb.Append(checkeditem.Value.ToString());
                }
                else
                {
                    sb.Append("|" + checkeditem.Value.ToString());
                }
                i++;
            }

            return sb;
        }

        protected void RadAjaxPanel1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            try
            {
                switch (e.Argument)
                {
                    case "oIndexDialog":
                        LoadGridSearch();
                        break;

                    case "oEditDialog":
                        LoadGridSearch();
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogException(ex);

                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            }
        }

        void HeaderContextMenu_ItemCreated(object sender, RadMenuEventArgs e)
        {
            RadContextMenu ctxMenu = sender as RadContextMenu;
            if (ctxMenu != null)
            {
                if (e.Item.Text == "Group By")
                {
                    e.Item.Style.Add("display", "none");
                    ctxMenu.Items[e.Item.Index - 1].Remove();
                }

                if (e.Item.Text == "Ungroup")
                {
                    e.Item.Style.Add("display", "none");
                    ctxMenu.Items[e.Item.Index - 1].Remove();
                }

                if (e.Item.Text == "Columns")
                {
                    e.Item.Style.Add("display", "none");
                    ctxMenu.Items[e.Item.Index - 1].Remove();
                }

                if (e.Item.Text == "Best Fit")
                {
                    e.Item.Style.Add("display", "none");
                    ctxMenu.Items[e.Item.Index - 1].Remove();
                }

            }


            foreach (Control c in e.Item.Controls)
            {
                RadComboBox combo = c as RadComboBox;
                if (combo != null)
                {
                    while (combo.Items.Count > 11)
                    {
                        combo.Items.Remove(combo.Items.Count - 1);
                    }
                }
            }
        }

        protected void RadGrid1_PreRender(object sender, EventArgs e)
        {

            RadGrid radgrid = (RadGrid)sender;

            if (radgrid != null)
            {
                if (radgrid.MasterTableView != null)
                {
                    foreach (GridColumn col in radgrid.MasterTableView.RenderColumns)
                    {
                        col.HeaderStyle.CssClass = (!string.IsNullOrEmpty(col.CurrentFilterValue) || col.ListOfFilterValues != null && col.ListOfFilterValues.Length > 0) ? "filteredColumn" : string.Empty;
                    }
                }
            }
        }

        protected void RadGrid1_ExcelMLWorkBookCreated(object sender, Telerik.Web.UI.GridExcelBuilder.GridExcelMLWorkBookCreatedEventArgs e)
        {
            try
            {
                BorderStyles border;
                border = new BorderStyles
                {
                    Color = System.Drawing.Color.LightGray,
                    Weight = 1,
                    LineStyle = LineStyle.Continuous,
                    PositionType = PositionType.Bottom
                };

                StyleElement styleBold;
                styleBold = new StyleElement(id: "StyleBold");
                styleBold.InteriorStyle.Pattern = InteriorPatternType.Solid;
                styleBold.Borders.Add(border);
                styleBold.FontStyle.Bold = true;
                e.WorkBook.Styles.Add(styleBold);

                StyleElement style;
                style = new StyleElement(id: "Style");
                style.InteriorStyle.Pattern = InteriorPatternType.Solid;
                style.Borders.Add(border);
                e.WorkBook.Styles.Add(style);
            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(ex.Message)) + "');", addScriptTags: true);
            }
        }

        protected void RadGrid1_FilterCheckListItemsRequested(object sender, GridFilterCheckListItemsRequestedEventArgs e)
        {
            object obj = this.Session["SearchResult"];
            if ((!(obj == null)))
            {
                IGridDataColumn gridCol;
                gridCol = e.Column as IGridDataColumn;

                if (gridCol != null)
                {
                    string DataField = gridCol.GetActiveDataField();

                    e.ListBox.DataSource = (DataTable)(obj);
                    e.ListBox.DataKeyField = DataField;
                    e.ListBox.DataTextField = DataField;
                    e.ListBox.DataValueField = DataField;
                    e.ListBox.DataBind();
                }
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            object obj = this.Session["SearchResult"];
            if ((!(obj == null)))
            {
                this.RadGrid1.DataSource = (DataTable)(obj);
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.ExportToExcelCommandName)
                {
                    this.RadGrid1.ExportSettings.FileName = "Corrective_Action_Requests";

                    for (int i = 15; i < 52; i++)
                    {
                        this.RadGrid1.MasterTableView.Columns[i].Display = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + ".\\n\\n" + ErrException + "  " + AdminContact + "');", addScriptTags: true);
            }
        }


        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
                {
                    GridEditableItem item;
                    item = e.Item as GridEditableItem;

                    if (item != null)
                    {
                        TableCell oidCell = item["OID"];
                        TableCell statusEvCell = item["Car_Ev"];
                        TableCell statusOidCell = item["CAR_STATUS_OID"];
                        TableCell statusCell = item["CAR_STATUS_NM"];
                        TableCell awaitingDaysCell = item["AWAITING_DAYS"];
                        TableCell closedDaysCell = item["CLOSED_DAYS"];

                        TableCell dateIssuedCell = item["DATE_ISSUED_NM"];
                        TableCell dateClosedCell = item["CLOSE_DT"];
                        TableCell deleteFlagCell = item["DEL_FLG"];

                        TableCell qdmsCell = item["QDMS_INDEXED"];
                        TableCell qdmsDateCell = item["QDMS_INDEXED_DT"];
                        TableCell qdmsByCell = item["QDMS_INDEXED_BY_NM"];

                        ImageButton btnEdit = (ImageButton)item.FindControl("ImageButton1");
                        btnEdit.OnClientClick = "ShowMenu(event, " + oidCell.Text + ", " + item.ItemIndex.ToString() + ")";

                        Label label1;
                        label1 = (Label)item.FindControl(id: "Label1");

                        Label label2;
                        label2 = (Label)item.FindControl(id: "Label2");

                        Label label3;
                        label3 = (Label)item.FindControl(id: "Label3");

                        Label label4;
                        label4 = (Label)item.FindControl(id: "Label4");

                        Label label5;
                        label5 = (Label)item.FindControl(id: "Label5");

                        Label label6;
                        label6 = (Label)item.FindControl(id: "Label6");

                        Label label7;
                        label7 = (Label)item.FindControl(id: "Label7");

                        ImageButton imageButton;
                        imageButton = e.Item.FindControl(id: "ImageButton3") as ImageButton;

                        if (label1 != null)
                        {
                            label1.Font.Bold = true;
                            label1.Text = statusCell.Text.Trim();
                        }

                        if (int.Parse(statusOidCell.Text) == 3) // CAR Closed
                        {
                            imageButton.Enabled = false;

                            if (label2 != null)
                            {
                                label2.Text = dateClosedCell.Text;
                                label2.Font.Bold = true;
                            }

                            if (label3 != null)
                            {
                                label3.Text = closedDaysCell.Text + " Day(s)";
                                label3.Font.Bold = true;
                            }
                        }
                        else
                        {
                            if (imageButton != null)
                            {
                                imageButton.ImageUrl = "~/Img/clock_red32.png";
                                imageButton.Enabled = false;
                                imageButton.ToolTip = statusCell.Text.Trim();
                            }

                            if (label2 != null)
                            {
                                label2.Text = dateIssuedCell.Text;
                                label2.Font.Bold = true;
                            }

                            if (label3 != null)
                            {
                                label3.Text = awaitingDaysCell.Text + " Day(s)";
                                label3.Font.Bold = true;
                            }
                        }

                        if (bool.Parse(deleteFlagCell.Text))
                        {
                            if (imageButton != null)
                            {
                                imageButton.ImageUrl = "~/Img/lock_delete32.png";
                            }
                        }

                        if (htmlUtil.IsNumeric(qdmsCell.Text.Trim()))
                        {
                            if (int.Parse(qdmsCell.Text.Trim()) == 1)
                            {
                                label4.Text = "QDMS by:  ";
                                label5.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(qdmsByCell.Text.Trim()));

                                label6.Text = "QMDS Date:  ";
                                label7.Text = htmlUtil.SanitizeHtml(Server.HtmlDecode(qdmsDateCell.Text.Trim()));
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                LogException(ex);
                e.Canceled = true;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            }
        }

        // Application event log
        private void LogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog();

            appLog.ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionSource = "Search.aspx";
            appLog.ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionUrl = MySession.Current.SessionHostName.ToString().ToUpper();
            appLog.ExceptionTargetSite = ex.TargetSite.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionStackTrace = ex.StackTrace.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.AppLogEvent();
        }

        private void CustomLogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog();

            appLog.ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionSource = "Search.aspx";
            appLog.ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionUrl = MySession.Current.SessionHostName.ToString().ToUpper();
            appLog.ExceptionTargetSite = ex.Data["TargetSite"].ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionStackTrace = ex.Data["StackTrace"].ToString().Replace(oldValue: "'", newValue: "`");

            appLog.AppLogEvent();
        }


        protected void RadComboBox8_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (this.RadComboBox8.SelectedIndex == 0)
            {
                this.RadDatePicker1.Clear();
                this.RadDatePicker1.SelectedDate = DateTime.Now;

                this.RadDatePicker2.Clear();
                this.RadDatePicker2.SelectedDate = DateTime.Now;
            }
        }

        protected void DeleteCar(int rowOid, int del_Flg, string menuText)
        {
            //try
            //{
            object obj = this.Session["SearchResult"];
            if ((!(obj == null)))
            {
                DataTable objDT = (DataTable)obj;
                objDT.PrimaryKey = new DataColumn[] { objDT.Columns["OID"] };
                DataRow[] changedRows = objDT.Select("OID=" + rowOid.ToString());

                if (objDT.Rows.Find(rowOid) != null)
                {
                    string uReturn;

                    UpdateRecord delCar;
                    delCar = new UpdateRecord();
                    delCar.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(rowOid.ToString())));
                    delCar.Del_Flg = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(del_Flg.ToString())));
                    delCar.User_Id = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(MySession.Current.SessionUserID.ToString().ToUpper())));

                    uReturn = delCar.ExecDeleteCar();
                    if (uReturn.ToString() == "Successfully")
                    {
                        //Update new values
                        Hashtable newValues;
                        newValues = new Hashtable();
                        newValues["DEL_FLG"] = Convert.ToBoolean(del_Flg);
                        changedRows[0].BeginEdit();

                        foreach (DictionaryEntry entry in newValues)
                        {
                            changedRows[0][(string)entry.Key] = entry.Value;
                        }
                        changedRows[0].EndEdit();

                        objDT.AcceptChanges();
                        this.Session["SearchResult"] = objDT;


                        ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(uReturn.ToString()) + "');", addScriptTags: true);
                    }
                    else
                    {
                        int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                        Exception aex = new Exception(removeChar.SanitizeQuoteString(uReturn.ToString()));
                        aex.Data.Add(key: "TargetSite", value: "RadContextMenu1_ItemClick; " + menuText);
                        aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                        CustomLogException(aex);
                        ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Error: " + removeChar.SanitizeQuoteString(uReturn.ToString()) + "\\n\\n" + ErrException + "');", addScriptTags: true);

                    }
                }
                this.RadGrid1.DataSource = objDT;
                this.RadGrid1.DataBind();
            }
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);
            //    ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}            
        }

        protected void OpenCar(int rowOid, string menuText)
        {
            //try
            //{
            object obj = this.Session["SearchResult"];
            if ((!(obj == null)))
            {
                DataTable objDT = (DataTable)obj;
                objDT.PrimaryKey = new DataColumn[] { objDT.Columns["OID"] };
                DataRow[] changedRows = objDT.Select("OID=" + rowOid.ToString());

                if (objDT.Rows.Find(rowOid) != null)
                {
                    string uReturn;

                    UpdateRecord openCar;
                    openCar = new UpdateRecord();
                    openCar.Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(rowOid.ToString())));

                    uReturn = openCar.ExecOpenCar();
                    if (uReturn.ToString() == "Successfully")
                    {
                        //Update new values
                        Hashtable newValues;
                        newValues = new Hashtable();
                        newValues["CAR_STATUS_OID"] = "2";
                        newValues["CAR_STATUS_NM"] = "Awaiting Closure";
                        changedRows[0].BeginEdit();

                        foreach (DictionaryEntry entry in newValues)
                        {
                            changedRows[0][(string)entry.Key] = entry.Value;
                        }
                        changedRows[0].EndEdit();

                        objDT.AcceptChanges();
                        this.Session["SearchResult"] = objDT;


                        ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(uReturn.ToString()) + "');", addScriptTags: true);
                    }
                    else
                    {
                        int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                        Exception aex = new Exception(removeChar.SanitizeQuoteString(uReturn.ToString()));
                        aex.Data.Add(key: "TargetSite", value: "RadContextMenu1_ItemClick; " + menuText);
                        aex.Data.Add(key: "StackTrace", value: "Error line " + lineNumber.ToString());

                        CustomLogException(aex);
                        ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('Error: " + removeChar.SanitizeQuoteString(uReturn.ToString()) + "\\n\\n" + ErrException + "');", addScriptTags: true);

                    }
                }
                this.RadGrid1.DataSource = objDT;
                this.RadGrid1.DataBind();
            }
            //}
            //catch (Exception ex)
            //{
            //    LogException(ex);
            //    ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            //}
        }

        protected void RadContextMenu1_ItemClick(object sender, RadMenuEventArgs e)
        {
            try
            {

                if (htmlUtil.IsNumeric(this.RowIndex.Value))
                {
                    int radGridClickedRowOid;
                    radGridClickedRowOid = Convert.ToInt32(this.RowOid.Value);

                    switch (e.Item.Text)
                    {
                        case "Undelete":
                            DeleteCar(radGridClickedRowOid, 0, e.Item.Text);
                            break;
                        case "Re-Open":
                            OpenCar(radGridClickedRowOid, e.Item.Text);
                            break;
                        case "Delete":
                            DeleteCar(radGridClickedRowOid, 1, e.Item.Text);
                            break;
                    }

                    LoadGridSearch();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('Invalid selected index...  Delete process has been cancelled.');", addScriptTags: true);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            }
        }
    }
}