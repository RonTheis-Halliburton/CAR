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
    public partial class Active : System.Web.UI.Page
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
                    this.CheckBox1.Text = "Created by " + MySession.Current.SessionFullName.ToString();
                    LoadFacility(this.RadComboBox2);
                    LoadPsl(this.RadComboBox3);

                    this.Session["ActiveResult"] = null;

                    LoadCookie();

                    this.CurrentUserID.Value = MySession.Current.SessionUserID.ToUpper();

                }
                catch (Exception ex)
                {
                    LogException(ex);

                    ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
                }
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
                    item = new RadComboBoxItem
                    {
                        Value = row["OID"].ToString().Trim(),
                        Text = row["PLNT_NM"].ToString().Trim(),
                        ToolTip = row["FACILITY_NM"].ToString()
                    };
                    comboBox.Items.Add(item);
                }

                //comboBox.Items.Insert(index: 0, item: new RadComboBoxItem(text: "ALL", value: "0"));
                //comboBox.SelectedIndex = 0;
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
                    item = new RadComboBoxItem
                    {
                        Value = row["OID"].ToString().Trim(),
                        Text = row["PSL"].ToString().Trim(),
                        ToolTip = row["PSL"].ToString()
                    };
                    comboBox.Items.Add(item);
                }

                //comboBox.Items.Insert(index: 0, item: new RadComboBoxItem(text: "ALL", value: "0"));
                //comboBox.SelectedIndex = 0;
            }
        }

        protected void LoadCookie()
        {

            HttpCookie myCookie = Request.Cookies["CorrectiveActionActive"];
            if (myCookie != null)
            {
                SetComboByValue(this.RadComboBox2, myCookie.Values["PlantOid"].ToString());
                SetComboByValue(this.RadComboBox3, myCookie.Values["PslOid"].ToString());
            }
            else
            {
                SetComboByValue(this.RadComboBox2, "1,3,6");
                SetComboByValue(this.RadComboBox3, "1,2,3,5");
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

        protected void RadButton1_Click(object sender, EventArgs e)
        {
            try
            {

                this.Session["ActiveResult"] = null;
                this.RadGrid1.MasterTableView.CurrentPageIndex = 0;
                this.RadGrid1.MasterTableView.SortExpressions.Clear();
                this.RadGrid1.DataSource = (DataTable)this.Session["ActiveResult"];
                this.RadGrid1.DataBind();

                LoadGridSearch();
            }
            catch (Exception ex)
            {
                LogException(ex);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), key: "closeScript", script: "alert('" + removeChar.SanitizeQuoteString(ex.Message) + "');", addScriptTags: true);
            }
        }


        protected void LoadGridSearch()
        {

            HttpCookie aCookie;
            aCookie = new HttpCookie(name: "CorrectiveActionActive");
            //aCookie.Secure = true;
            //aCookie.HttpOnly = true;

            GetSearch getSearch;
            getSearch = new GetSearch();


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

            if (this.CheckBox1.Checked)
            {
                getSearch.OriginatorID = MySession.Current.SessionUserID.ToString();
            }
            else
            {
                getSearch.OriginatorID = string.Empty;
            }


            getSearch.CarAdmin = MySession.Current.SessionCarAdmin.ToString();
            getSearch.OriginatorManager = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(MySession.Current.SessionManager.ToString())));
            getSearch.OriginatorManagerID = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(MySession.Current.SessionManagerID.ToString().ToUpper())));
            getSearch.OriginatorManagerEmail = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(MySession.Current.SessionManagerEmail.ToString())));


            aCookie.Expires = DateTime.Now.AddHours(value: 1);
            Response.Cookies.Add(aCookie);

            using (DataTable dt = getSearch.SearchCarActive())
            {
                this.RadGrid1.DataSource = dt;
                this.RadGrid1.DataBind();

                this.Session["ActiveResult"] = dt;
            }

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

        protected void DeleteCar(int rowOid, int del_Flg, string menuText)
        {

            object obj = this.Session["ActiveResult"];
            if ((!(obj == null)))
            {
                DataTable objDT = (DataTable)obj;
                objDT.PrimaryKey = new DataColumn[] { objDT.Columns["OID"] };
                DataRow[] changedRows = objDT.Select("OID=" + rowOid.ToString());

                if (objDT.Rows.Find(rowOid) != null)
                {
                    string uReturn;

                    UpdateRecord delCar;
                    delCar = new UpdateRecord
                    {
                        Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(rowOid.ToString()))),
                        Del_Flg = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(del_Flg.ToString()))),
                        User_Id = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(MySession.Current.SessionUserID.ToString().ToUpper())))
                    };

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
                        this.Session["ActiveResult"] = objDT;


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

        }

        protected void OpenCar(int rowOid, string menuText)
        {

            object obj = this.Session["ActiveResult"];
            if ((!(obj == null)))
            {
                DataTable objDT = (DataTable)obj;
                objDT.PrimaryKey = new DataColumn[] { objDT.Columns["OID"] };
                DataRow[] changedRows = objDT.Select("OID=" + rowOid.ToString());

                if (objDT.Rows.Find(rowOid) != null)
                {
                    string uReturn;

                    UpdateRecord openCar;
                    openCar = new UpdateRecord
                    {
                        Car_Oid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(rowOid.ToString())))
                    };

                    uReturn = openCar.ExecOpenCar();
                    if (uReturn.ToString() == "Successfully")
                    {
                        //Update new values
                        Hashtable newValues;
                        newValues = new Hashtable
                        {
                            ["CAR_STATUS_OID"] = "2",
                            ["CAR_STATUS_NM"] = "Awaiting Closure"
                        };
                        changedRows[0].BeginEdit();

                        foreach (DictionaryEntry entry in newValues)
                        {
                            changedRows[0][(string)entry.Key] = entry.Value;
                        }
                        changedRows[0].EndEdit();

                        objDT.AcceptChanges();
                        this.Session["ActiveResult"] = objDT;


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
            object obj = this.Session["ActiveResult"];
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
            object obj = this.Session["ActiveResult"];
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
                    this.RadGrid1.ExportSettings.FileName = "Active_Corrective_Action_Requests";

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

        protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {

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

                        ImageButton btnEdit = (ImageButton)item.FindControl("ImageButton1");
                        btnEdit.OnClientClick = "ShowMenu(event, " + oidCell.Text + ", " + item.ItemIndex.ToString() + ")";

                        Label label1;
                        label1 = (Label)item.FindControl(id: "Label1");

                        Label label2;
                        label2 = (Label)item.FindControl(id: "Label2");

                        Label label3;
                        label3 = (Label)item.FindControl(id: "Label3");

                        ImageButton imageButton;
                        imageButton = e.Item.FindControl(id: "ImageButton3") as ImageButton;

                        if (label1 != null)
                        {
                            label1.Font.Bold = true;
                            label1.Text = statusCell.Text.Trim();
                        }

                        if (int.Parse(statusOidCell.Text) == 3) // CAR Closed
                        {
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

        // Application event log
        private void LogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog
            {
                ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionSource = "Active.aspx",
                ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionUrl = MySession.Current.SessionHostName.ToString().ToUpper(),
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
                ExceptionSource = "Active.aspx",
                ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionUrl = MySession.Current.SessionHostName.ToString().ToUpper(),
                ExceptionTargetSite = ex.Data["TargetSite"].ToString().Replace(oldValue: "'", newValue: "`"),
                ExceptionStackTrace = ex.Data["StackTrace"].ToString().Replace(oldValue: "'", newValue: "`")
            };

            appLog.AppLogEvent();
        }
    }
}