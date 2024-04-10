using CAR;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Common.FormatProviders;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Editing;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;

public class CreateNewDocx
{
    public string Car_Oid { set; get; }

    private static readonly ThemableColor BlueColor = new ThemableColor(color: Colors.Blue);
    private static readonly ThemableColor GrayColor = new ThemableColor(color: Colors.Gray);

    public SqlDataAdapter Sda = new SqlDataAdapter();
    public string ConStr = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
    public string ErrException = ConfigurationManager.AppSettings["ErrorMessage"] + ConfigurationManager.AppSettings["AdminContact"];

    SanitizeString removeChar = new SanitizeString();
    MyUtil.HtmlUtility htmlUtil = MyUtil.HtmlUtility.Instance;
    public MemoryStream GetNewDocxFile()
    {
        ThemeColorScheme colorScheme = new ThemeColorScheme("Mine",
                                                                                                                Colors.Black,     // background 1 
                                                                                                                Colors.Blue,      // text 1 
                                                                                                                Colors.Brown,     // background 2 
                                                                                                                Colors.Cyan,      // text 2 
                                                                                                                Colors.DarkGray,  // accent 1 
                                                                                                                Colors.Gray,      // accent 2 
                                                                                                                Colors.Green,     // accent 3 
                                                                                                                Colors.LightGray, // accent 4 
                                                                                                                Colors.Magenta,   // accent 5 
                                                                                                                Colors.Orange,    // accent 6 
                                                                                                                Colors.Purple,    // hyperlink 
                                                                                                                Colors.Red);      // followedHyperlink

        ThemeFontScheme fontScheme = new ThemeFontScheme("Mine",
                                                                                                    "Times New Roman",   // Major 
                                                                                                    "Arial");          // Minor 

        ThemableFontFamily themableFont = new ThemableFontFamily(ThemeFontType.Minor);

        DocumentTheme theme = new DocumentTheme("Mine", colorScheme, fontScheme);

        MemoryStream ms = new MemoryStream();

        int cellWidth = 200;
        int cellWidth2 = 400;
        int cellWidth3 = 650;
        int fontSizeHeader = 13;
        int fontSizeBody = 11;

        try
        {
            GetRecord xCar;
            xCar = new GetRecord();
            xCar.CarOid = Car_Oid;

            using (DataTable dt = xCar.Exec_Get_Existing_Car_By_Oid_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    RadFlowDocument document = new RadFlowDocument();
                    document.Theme = theme;

                    RadFlowDocumentEditor editor = new RadFlowDocumentEditor(document);
                    editor.ParagraphFormatting.TextAlignment.LocalValue = Alignment.Center;

                    IFormatProvider<RadFlowDocument> formatProvider = new DocxFormatProvider();

                    Run headerHal = editor.InsertText("HALLIBURTON ENERGY SERVICES");
                    headerHal.FontSize = fontSizeHeader;
                    headerHal.FontWeight = FontWeights.Bold;

                    editor.InsertBreak(BreakType.LineBreak);

                    Run headerCar = editor.InsertText("Corrective Action Request");
                    headerCar.FontSize = fontSizeHeader;
                    headerCar.FontWeight = FontWeights.Bold;

                    Table table = editor.InsertTable();

                    //Run header = editor.InsertText("Halliburton - Corrective Action Request " + row["CAR_NBR"].ToString());
                    ////header.Underline.Pattern = UnderlinePattern.Single;
                    ////header.Underline.Color = new ThemableColor(Colors.Black);                    
                    //header.FontSize = 15;
                    //header.FontWeight = FontWeights.Bold;
                    //Table table = editor.InsertTable();



                    ///Row 0
                    TableRow row0 = table.Rows.AddTableRow();
                    TableCell cell0 = row0.Cells.AddTableCell();
                    Paragraph cellPar0 = cell0.Blocks.AddParagraph();

                    Run runCellText0 = cellPar0.Inlines.AddRun(string.Format("CAR Nbr:", 0, 0));
                    runCellText0.FontWeight = FontWeights.Bold;
                    runCellText0.FontSize = fontSizeBody;
                    cell0.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cellCar = row0.Cells.AddTableCell();
                    Paragraph cellParCar = cellCar.Blocks.AddParagraph();

                    Run runCellTextCar = cellParCar.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["CAR_NBR"].ToString())))), 0, 1));
                    runCellTextCar.ForegroundColor = BlueColor;
                    runCellTextCar.FontSize = fontSizeBody;
                    cellCar.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 1
                    TableRow row1 = table.Rows.AddTableRow();
                    TableCell cell1 = row1.Cells.AddTableCell();
                    Paragraph cellPar1 = cell1.Blocks.AddParagraph();

                    Run runCellText1 = cellPar1.Inlines.AddRun(string.Format("Originator:", 0, 0));
                    runCellText1.FontWeight = FontWeights.Bold;
                    runCellText1.FontSize = fontSizeBody;
                    cell1.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell2 = row1.Cells.AddTableCell();
                    Paragraph cellPar2 = cell2.Blocks.AddParagraph();

                    Run runCellText2 = cellPar2.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["ORIGINATOR_USR_NM"].ToString())))), 0, 1));
                    runCellText2.ForegroundColor = BlueColor;
                    runCellText2.FontSize = fontSizeBody;
                    cell2.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 2
                    TableRow row2 = table.Rows.AddTableRow();
                    TableCell cell3 = row2.Cells.AddTableCell();
                    Paragraph cellPar3 = cell3.Blocks.AddParagraph();

                    Run runCellText3 = cellPar3.Inlines.AddRun(string.Format("Date Issued:", 0, 0));
                    runCellText3.FontWeight = FontWeights.Bold;
                    runCellText3.FontSize = fontSizeBody;
                    cell3.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell4 = row2.Cells.AddTableCell();
                    Paragraph cellPar4 = cell4.Blocks.AddParagraph();

                    Run runCellText4 = cellPar4.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["DATE_ISSUED"].ToString())))), 0, 1));
                    runCellText4.ForegroundColor = BlueColor;
                    runCellText4.FontSize = fontSizeBody;
                    cell4.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 3
                    TableRow row3 = table.Rows.AddTableRow();
                    TableCell cell5 = row3.Cells.AddTableCell();
                    Paragraph cellPar5 = cell5.Blocks.AddParagraph();

                    Run runCellText5 = cellPar5.Inlines.AddRun(string.Format("Due Date:", 0, 0));
                    runCellText5.FontWeight = FontWeights.Bold;
                    runCellText5.FontSize = fontSizeBody;
                    cell5.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell6 = row3.Cells.AddTableCell();
                    Paragraph cellPar6 = cell6.Blocks.AddParagraph();

                    Run runCellText6 = cellPar6.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["DUE_DT"].ToString())))), 0, 1));
                    runCellText6.ForegroundColor = BlueColor;
                    runCellText6.FontSize = fontSizeBody;
                    cell6.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 4
                    TableRow row4 = table.Rows.AddTableRow();
                    TableCell cell7 = row4.Cells.AddTableCell();
                    Paragraph cellPar7 = cell7.Blocks.AddParagraph();

                    Run runCellText7 = cellPar7.Inlines.AddRun(string.Format("Finding Type:", 0, 0));
                    runCellText7.FontWeight = FontWeights.Bold;
                    runCellText7.FontSize = fontSizeBody;
                    cell7.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell8 = row4.Cells.AddTableCell();
                    Paragraph cellPar8 = cell8.Blocks.AddParagraph();

                    Run runCellText8 = cellPar8.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["FINDING_TYPE_NM"].ToString())))), 0, 1));
                    runCellText8.ForegroundColor = BlueColor;
                    runCellText8.FontSize = fontSizeBody;
                    cell8.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 5
                    TableRow row5 = table.Rows.AddTableRow();
                    TableCell cell9 = row5.Cells.AddTableCell();
                    Paragraph cellPar9 = cell9.Blocks.AddParagraph();

                    Run runCellText9 = cellPar9.Inlines.AddRun(string.Format("Area:", 0, 0));
                    runCellText9.FontWeight = FontWeights.Bold;
                    runCellText9.FontSize = fontSizeBody;
                    cell9.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell10 = row5.Cells.AddTableCell();
                    Paragraph cellPar10 = cell10.Blocks.AddParagraph();

                    Run runCellText10 = cellPar10.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["AREA_DESCRIPT_NM"].ToString())))), 0, 1));
                    runCellText10.ForegroundColor = BlueColor;
                    runCellText10.FontSize = fontSizeBody;
                    cell10.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 6
                    TableRow row6 = table.Rows.AddTableRow();
                    TableCell cell11 = row6.Cells.AddTableCell();
                    Paragraph cellPar11 = cell11.Blocks.AddParagraph();

                    Run runCellText11 = cellPar11.Inlines.AddRun(string.Format("PSL:", 0, 0));
                    runCellText11.FontWeight = FontWeights.Bold;
                    runCellText11.FontSize = fontSizeBody;
                    cell11.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell12 = row6.Cells.AddTableCell();
                    Paragraph cellPar12 = cell12.Blocks.AddParagraph();

                    Run runCellText12 = cellPar12.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["PSL_NM"].ToString())))), 0, 1));
                    runCellText12.ForegroundColor = BlueColor;
                    runCellText12.FontSize = fontSizeBody;
                    cell12.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 7
                    TableRow row7 = table.Rows.AddTableRow();
                    TableCell cell13 = row7.Cells.AddTableCell();
                    Paragraph cellPar13 = cell13.Blocks.AddParagraph();

                    Run runCellText13 = cellPar13.Inlines.AddRun(string.Format("Plant:", 0, 0));
                    runCellText13.FontWeight = FontWeights.Bold;
                    runCellText13.FontSize = fontSizeBody;
                    cell13.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell14 = row7.Cells.AddTableCell();
                    Paragraph cellPar14 = cell14.Blocks.AddParagraph();

                    Run runCellText14 = cellPar14.Inlines.AddRun(string.Format(row["PLNT_CD"].ToString() + " - " + removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["FACILITY_NAME"].ToString())))), 0, 1));
                    runCellText14.ForegroundColor = BlueColor;
                    runCellText14.FontSize = fontSizeBody;
                    cell14.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 8
                    TableRow row8 = table.Rows.AddTableRow();
                    TableCell cell15 = row8.Cells.AddTableCell();
                    Paragraph cellPar15 = cell15.Blocks.AddParagraph();

                    Run runCellText15 = cellPar15.Inlines.AddRun(string.Format("API/ISO Reference:", 0, 0));
                    runCellText15.FontWeight = FontWeights.Bold;
                    runCellText15.FontSize = fontSizeBody;
                    cell15.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell16 = row8.Cells.AddTableCell();
                    cell16.PreferredWidth = new TableWidthUnit(cellWidth2);

                    Table tableRef = cell16.Blocks.AddTable();
                    string[] strApiiso = row["API_ISO_ELEM"].ToString().Split('|');
                    foreach (string api in strApiiso)
                    {
                        TableRow rowT = tableRef.Rows.AddTableRow();

                        TableCell cell = rowT.Cells.AddTableCell();
                        Paragraph cellPar = cell.Blocks.AddParagraph();

                        Run runCellText = cellPar.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(HttpUtility.HtmlDecode(htmlUtil.SanitizeHtml(api.ToString())))), 0, 1));
                        runCellText.ForegroundColor = BlueColor;
                        runCellText.FontSize = fontSizeBody;
                        cell.PreferredWidth = new TableWidthUnit(cellWidth2);

                    }

                    ///Row 9
                    TableRow row9 = table.Rows.AddTableRow();
                    TableCell cell17 = row9.Cells.AddTableCell();
                    Paragraph cellPar17 = cell17.Blocks.AddParagraph();

                    Run runCellText17 = cellPar17.Inlines.AddRun(string.Format("Audit Number:", 0, 0));
                    runCellText17.FontWeight = FontWeights.Bold;
                    runCellText17.FontSize = fontSizeBody;
                    cell17.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell18 = row9.Cells.AddTableCell();
                    Paragraph cellPar18 = cell18.Blocks.AddParagraph();

                    Run runCellText18 = cellPar18.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["AUDIT_NBR"].ToString())))), 0, 1));
                    runCellText18.ForegroundColor = BlueColor;
                    runCellText18.FontSize = fontSizeBody;
                    cell18.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 10
                    TableRow row10 = table.Rows.AddTableRow();
                    TableCell cell19 = row10.Cells.AddTableCell();
                    Paragraph cellPar19 = cell19.Blocks.AddParagraph();

                    Run runCellText19 = cellPar19.Inlines.AddRun(string.Format("Q Note Number:", 0, 0));
                    runCellText19.FontWeight = FontWeights.Bold;
                    runCellText19.FontSize = fontSizeBody;
                    cell19.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell20 = row10.Cells.AddTableCell();
                    Paragraph cellPar20 = cell20.Blocks.AddParagraph();

                    Run runCellText20 = cellPar20.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["QNOTE_NBR"].ToString())))), 0, 1));
                    runCellText20.ForegroundColor = BlueColor;
                    runCellText20.FontSize = fontSizeBody;
                    cell20.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 11
                    TableRow row11 = table.Rows.AddTableRow();
                    TableCell cell21 = row11.Cells.AddTableCell();
                    Paragraph cellPar21 = cell21.Blocks.AddParagraph();

                    Run runCellText21 = cellPar21.Inlines.AddRun(string.Format("CPI Number:", 0, 0));
                    runCellText21.FontWeight = FontWeights.Bold;
                    runCellText21.FontSize = fontSizeBody;
                    cell21.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell22 = row11.Cells.AddTableCell();
                    Paragraph cellPar22 = cell22.Blocks.AddParagraph();

                    Run runCellText22 = cellPar22.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["CPI_NBR"].ToString())))), 0, 1));
                    runCellText22.ForegroundColor = BlueColor;
                    runCellText22.FontSize = fontSizeBody;
                    cell22.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 12
                    TableRow row12 = table.Rows.AddTableRow();
                    TableCell cell23 = row12.Cells.AddTableCell();
                    Paragraph cellPar23 = cell23.Blocks.AddParagraph();

                    Run runCellText23 = cellPar23.Inlines.AddRun(string.Format("Material Number:", 0, 0));
                    runCellText23.FontWeight = FontWeights.Bold;
                    runCellText23.FontSize = fontSizeBody;
                    cell23.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell24 = row12.Cells.AddTableCell();
                    Paragraph cellPar24 = cell24.Blocks.AddParagraph();

                    Run runCellText24 = cellPar24.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["MATERIAL_NBR"].ToString())))), 0, 1));
                    runCellText24.ForegroundColor = BlueColor;
                    runCellText24.FontSize = fontSizeBody;
                    cell24.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 13
                    TableRow row13 = table.Rows.AddTableRow();
                    TableCell cell25 = row13.Cells.AddTableCell();
                    Paragraph cellPar25 = cell25.Blocks.AddParagraph();

                    Run runCellText25 = cellPar25.Inlines.AddRun(string.Format("Purchase Order Number:", 0, 0));
                    runCellText25.FontWeight = FontWeights.Bold;
                    runCellText25.FontSize = fontSizeBody;
                    cell25.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell26 = row13.Cells.AddTableCell();
                    Paragraph cellPar26 = cell26.Blocks.AddParagraph();

                    Run runCellText26 = cellPar26.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["PURCHASE_ORDER_NBR"].ToString())))), 0, 1));
                    runCellText26.ForegroundColor = BlueColor;
                    runCellText26.FontSize = fontSizeBody;
                    cell26.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 14
                    TableRow row14 = table.Rows.AddTableRow();
                    TableCell cell27 = row14.Cells.AddTableCell();
                    Paragraph cellPar27 = cell27.Blocks.AddParagraph();

                    Run runCellText27 = cellPar27.Inlines.AddRun(string.Format("Production Order Number:", 0, 0));
                    runCellText27.FontWeight = FontWeights.Bold;
                    runCellText27.FontSize = fontSizeBody;
                    cell27.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell28 = row14.Cells.AddTableCell();
                    Paragraph cellPar28 = cell28.Blocks.AddParagraph();

                    Run runCellText28 = cellPar28.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["PRODUCTION_ORDER_NBR"].ToString())))), 0, 1));
                    runCellText28.ForegroundColor = BlueColor;
                    runCellText28.FontSize = fontSizeBody;
                    cell28.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 15
                    TableRow row15 = table.Rows.AddTableRow();
                    TableCell cell29 = row15.Cells.AddTableCell();
                    Paragraph cellPar29 = cell29.Blocks.AddParagraph();

                    Run runCellText29 = cellPar29.Inlines.AddRun(string.Format("API Audit Number:", 0, 0));
                    runCellText29.FontWeight = FontWeights.Bold;
                    runCellText29.FontSize = fontSizeBody;
                    cell29.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell30 = row15.Cells.AddTableCell();
                    Paragraph cellPar30 = cell30.Blocks.AddParagraph();

                    Run runCellText30 = cellPar30.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["API_AUDIT_NBR"].ToString())))), 0, 1));
                    runCellText30.ForegroundColor = BlueColor;
                    runCellText30.FontSize = fontSizeBody;
                    cell30.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 16a
                    TableRow row16a = table.Rows.AddTableRow();
                    TableCell cell16a = row16a.Cells.AddTableCell();
                    Paragraph cellPar16a = cell16a.Blocks.AddParagraph();

                    Run runCellText16a = cellPar16a.Inlines.AddRun(string.Format("Maintenance Order Number:", 0, 0));
                    runCellText16a.FontWeight = FontWeights.Bold;
                    runCellText16a.FontSize = fontSizeBody;
                    cell16a.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell16b = row16a.Cells.AddTableCell();
                    Paragraph cellPar16b = cell16b.Blocks.AddParagraph();

                    Run runCellTextcell16b = cellPar16b.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["MAINTENANCE_ORDER_NBR"].ToString())))), 0, 1));
                    runCellTextcell16b.ForegroundColor = BlueColor;
                    runCellTextcell16b.FontSize = fontSizeBody;
                    cell16b.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 16c
                    TableRow row16c = table.Rows.AddTableRow();
                    TableCell cell16c = row16c.Cells.AddTableCell();
                    Paragraph cellPar16c = cell16c.Blocks.AddParagraph();

                    Run runCellText16c = cellPar16c.Inlines.AddRun(string.Format("Equipment Number:", 0, 0));
                    runCellText16c.FontWeight = FontWeights.Bold;
                    runCellText16c.FontSize = fontSizeBody;
                    cell16c.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell16d = row16c.Cells.AddTableCell();
                    Paragraph cellPar16d = cell16d.Blocks.AddParagraph();

                    Run runCellTextcell16d = cellPar16d.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["EQUIPMENT_NBR"].ToString())))), 0, 1));
                    runCellTextcell16d.ForegroundColor = BlueColor;
                    runCellTextcell16d.FontSize = fontSizeBody;
                    cell16d.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 16
                    TableRow row16 = table.Rows.AddTableRow();
                    TableCell cell31 = row16.Cells.AddTableCell();
                    Paragraph cellPar31 = cell31.Blocks.AddParagraph();

                    Run runCellText31 = cellPar31.Inlines.AddRun(string.Format("Category:", 0, 0));
                    runCellText31.FontWeight = FontWeights.Bold;
                    runCellText31.FontSize = fontSizeBody;
                    cell31.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell32 = row16.Cells.AddTableCell();
                    Paragraph cellPar32 = cell32.Blocks.AddParagraph();

                    Run runCellText32 = cellPar32.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["CATEGORY_NM"].ToString())))), 0, 1));
                    runCellText32.ForegroundColor = BlueColor;
                    runCellText32.FontSize = fontSizeBody;
                    cell32.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 18
                    TableRow row18 = table.Rows.AddTableRow();
                    TableCell cell35 = row18.Cells.AddTableCell();
                    Paragraph cellPar35 = cell35.Blocks.AddParagraph();

                    Run runCellText35 = cellPar35.Inlines.AddRun(string.Format("Vendor Number:", 0, 0));
                    runCellText35.FontWeight = FontWeights.Bold;
                    runCellText35.FontSize = fontSizeBody;
                    cell35.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell36 = row18.Cells.AddTableCell();
                    Paragraph cellPar36 = cell36.Blocks.AddParagraph();

                    Run runCellText36 = cellPar36.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["VNDR_NBR"].ToString())))), 0, 1));
                    runCellText36.ForegroundColor = BlueColor;
                    runCellText36.FontSize = fontSizeBody;
                    cell36.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 19
                    TableRow row19 = table.Rows.AddTableRow();
                    TableCell cell37 = row19.Cells.AddTableCell();
                    Paragraph cellPar37 = cell37.Blocks.AddParagraph();

                    Run runCellText37 = cellPar37.Inlines.AddRun(string.Format("Vendor Name:", 0, 0));
                    runCellText37.FontWeight = FontWeights.Bold;
                    runCellText37.FontSize = fontSizeBody;
                    cell37.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell38 = row19.Cells.AddTableCell();
                    Paragraph cellPar38 = cell38.Blocks.AddParagraph();

                    Run runCellText38 = cellPar38.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["VENDOR_NM"].ToString())))), 0, 1));
                    runCellText38.ForegroundColor = BlueColor;
                    runCellText38.FontSize = fontSizeBody;
                    cell38.PreferredWidth = new TableWidthUnit(cellWidth2);

                    /////Row 20
                    //TableRow row20 = table.Rows.AddTableRow();
                    //TableCell cell39 = row20.Cells.AddTableCell();
                    //Paragraph cellPar39 = cell39.Blocks.AddParagraph();

                    //Run runCellText39 = cellPar39.Inlines.AddRun(string.Format("Issued To:", 0, 0));
                    //runCellText39.FontWeight = FontWeights.Bold;
                    //runCellText39.FontSize = fontSizeBody;
                    //cell39.PreferredWidth = new TableWidthUnit(cellWidth);

                    //TableCell cell40 = row20.Cells.AddTableCell();
                    //Paragraph cellPar40 = cell40.Blocks.AddParagraph();

                    //Run runCellText40 = cellPar40.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["ISSUED_TO_USR_NM"].ToString())))), 0, 1));
                    //runCellText40.ForegroundColor = BlueColor;
                    //runCellText40.FontSize = fontSizeBody;
                    //cell40.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 21
                    TableRow row21 = table.Rows.AddTableRow();
                    TableCell cell41 = row21.Cells.AddTableCell();
                    Paragraph cellPar41 = cell41.Blocks.AddParagraph();

                    Run runCellText41 = cellPar41.Inlines.AddRun(string.Format("Location/Country:", 0, 0));
                    runCellText41.FontWeight = FontWeights.Bold;
                    runCellText41.FontSize = fontSizeBody;
                    cell41.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell42 = row21.Cells.AddTableCell();
                    Paragraph cellPar42 = cell42.Blocks.AddParagraph();

                    Run runCellText42 = cellPar42.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["LOC_COUNTRY_NM"].ToString())))), 0, 1));
                    runCellText42.ForegroundColor = BlueColor;
                    runCellText42.FontSize = fontSizeBody;
                    cell42.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 22
                    TableRow row22 = table.Rows.AddTableRow();
                    TableCell cell43 = row22.Cells.AddTableCell();
                    Paragraph cellPar43 = cell43.Blocks.AddParagraph();

                    Run runCellText43 = cellPar43.Inlines.AddRun(string.Format("Location/City State:", 0, 0));
                    runCellText43.FontWeight = FontWeights.Bold;
                    runCellText43.FontSize = fontSizeBody;
                    cell43.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell44 = row22.Cells.AddTableCell();
                    Paragraph cellPar44 = cell44.Blocks.AddParagraph();

                    Run runCellText44 = cellPar44.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["LOC_SUPPLIER"].ToString())))), 0, 1));
                    runCellText44.ForegroundColor = BlueColor;
                    runCellText44.FontSize = fontSizeBody;
                    cell44.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 23
                    TableRow row23 = table.Rows.AddTableRow();
                    TableCell cell45 = row23.Cells.AddTableCell();
                    Paragraph cellPar45 = cell45.Blocks.AddParagraph();

                    Run runCellText45 = cellPar45.Inlines.AddRun(string.Format("Issued To Name:", 0, 0));
                    runCellText45.FontWeight = FontWeights.Bold;
                    runCellText45.FontSize = fontSizeBody;
                    cell45.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell46 = row23.Cells.AddTableCell();
                    Paragraph cellPar46 = cell46.Blocks.AddParagraph();

                    Run runCellText46 = cellPar46.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["ISSUED_TO_USR_NM"].ToString().Trim())))), 0, 1));
                    runCellText46.ForegroundColor = BlueColor;
                    runCellText46.FontSize = fontSizeBody;
                    cell46.PreferredWidth = new TableWidthUnit(cellWidth2);

                    ///Row 24
                    TableRow row24 = table.Rows.AddTableRow();
                    TableCell cell47 = row24.Cells.AddTableCell();
                    Paragraph cellPar47 = cell47.Blocks.AddParagraph();

                    Run runCellText47 = cellPar47.Inlines.AddRun(string.Format("Issued To E-mail:", 0, 0));
                    runCellText47.FontWeight = FontWeights.Bold;
                    runCellText47.FontSize = fontSizeBody;
                    cell47.PreferredWidth = new TableWidthUnit(cellWidth);

                    TableCell cell48 = row24.Cells.AddTableCell();
                    Paragraph cellPar48 = cell48.Blocks.AddParagraph();

                    Run runCellText48 = cellPar48.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["ISSUED_TO_USR_EMAIL"].ToString())))), 0, 1));
                    runCellText48.ForegroundColor = BlueColor;
                    runCellText48.FontSize = fontSizeBody;
                    cell48.PreferredWidth = new TableWidthUnit(cellWidth2);


                    //editor.InsertBreak(BreakType.PageBreak);

                    ///Row 25                    
                    Table table25 = editor.InsertTable();
                    table25.StyleId = BuiltInStyleNames.TableGridStyleId;

                    TableRow row25 = table25.Rows.AddTableRow();
                    TableCell cell49 = row25.Cells.AddTableCell();
                    cell49.ColumnSpan = 2;

                    //cell49.Properties.BackgroundColor.LocalValue = new ThemableColor(themeColorType: ThemeColorType.Accent3);
                    Paragraph cellPar49 = cell49.Blocks.AddParagraph();

                    Run runCellText49 = cellPar49.Inlines.AddRun(string.Format("Description of Finding:", 0, 0));
                    runCellText49.FontWeight = FontWeights.Bold;
                    runCellText49.FontSize = fontSizeBody;
                    cell49.PreferredWidth = new TableWidthUnit(cellWidth3);

                    ///Row 26
                    TableRow row26 = table25.Rows.AddTableRow();
                    TableCell cell50 = row26.Cells.AddTableCell();
                    cell50.ColumnSpan = 2;

                    Paragraph cellPar50 = cell50.Blocks.AddParagraph();

                    Run runCellText50 = cellPar50.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["FINDING_DESC"].ToString())))), 0, 0));
                    runCellText50.ForegroundColor = BlueColor;
                    runCellText50.FontSize = fontSizeBody;
                    cell50.PreferredWidth = new TableWidthUnit(cellWidth3);

                    ///Row 27
                    Table table27 = editor.InsertTable();
                    table27.StyleId = BuiltInStyleNames.TableGridStyleId;

                    TableRow row27 = table27.Rows.AddTableRow();
                    TableCell cell51 = row27.Cells.AddTableCell();
                    cell51.ColumnSpan = 2;
                    //cell51.Properties.BackgroundColor.LocalValue = new ThemableColor(themeColorType: ThemeColorType.Accent3);
                    Paragraph cellPar51 = cell51.Blocks.AddParagraph();

                    Run runCellText51 = cellPar51.Inlines.AddRun(string.Format("Description of Improvement:", 0, 0));
                    runCellText51.FontWeight = FontWeights.Bold;
                    runCellText51.FontSize = fontSizeBody;
                    cell51.PreferredWidth = new TableWidthUnit(cellWidth3);

                    ///Row 28
                    TableRow row28 = table27.Rows.AddTableRow();
                    TableCell cell52 = row28.Cells.AddTableCell();
                    Paragraph cellPar52 = cell52.Blocks.AddParagraph();

                    Run runCellText52 = cellPar52.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["DESC_OF_IMPROVEMENT"].ToString())))), 0, 0));
                    runCellText52.ForegroundColor = BlueColor;
                    runCellText52.FontSize = fontSizeBody;
                    cell52.PreferredWidth = new TableWidthUnit(cellWidth3);

                    editor.InsertBreak(BreakType.PageBreak);

                    Run headerResp = editor.InsertText("Respondent Section");
                    headerResp.FontWeight = FontWeights.Bold;
                    headerResp.FontSize = fontSizeHeader;
                    headerResp.ForegroundColor = GrayColor;

                    editor.InsertBreak(BreakType.LineBreak);

                    Run headerDesc = editor.InsertText("Please investigate and respond to the following. All questions must be answered. Any documentation that will substantiate the corrective action and/or action completed to minimize recurrence should be submitted with your response.");
                    headerDesc.FontSize = 10;

                    editor.InsertBreak(BreakType.LineBreak);

                    ///Row 17
                    Table table17 = editor.InsertTable();
                    table17.StyleId = BuiltInStyleNames.TableGridStyleId;

                    TableRow row17 = table17.Rows.AddTableRow();
                    TableCell cell33 = row17.Cells.AddTableCell();
                    Paragraph cellPar33 = cell33.Blocks.AddParagraph();

                    Run runCellText33 = cellPar33.Inlines.AddRun(string.Format("Responsible Person:", 0, 0));
                    runCellText33.FontWeight = FontWeights.Bold;
                    runCellText33.FontSize = fontSizeBody;
                    cell33.PreferredWidth = new TableWidthUnit(cellWidth);

                    Table table17a = editor.InsertTable();
                    table17a.StyleId = BuiltInStyleNames.TableGridStyleId;

                    TableRow row17a = table17a.Rows.AddTableRow();

                    TableCell cell34 = row17a.Cells.AddTableCell();
                    Paragraph cellPar34 = cell34.Blocks.AddParagraph();

                    Run runCellText34 = cellPar34.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["RESP_PERSON_USR_NM"].ToString())))), 0, 1));
                    runCellText34.ForegroundColor = BlueColor;
                    runCellText34.FontSize = fontSizeBody;
                    cell34.PreferredWidth = new TableWidthUnit(cellWidth2);

                    //editor.InsertBreak(BreakType.LineBreak);

                    //********* Question 1  *********//
                    ///Row 29
                    Table table29 = editor.InsertTable();
                    table29.StyleId = BuiltInStyleNames.TableGridStyleId;

                    TableRow row29 = table29.Rows.AddTableRow();
                    TableCell cell53 = row29.Cells.AddTableCell();
                    //cell53.Properties.BackgroundColor.LocalValue = new ThemableColor(themeColorType: ThemeColorType.Accent3);
                    Paragraph cellPar53 = cell53.Blocks.AddParagraph();

                    Run runCellText53 = cellPar53.Inlines.AddRun(string.Format("1.  Why did this nonconformance occur?", 0, 0));
                    runCellText53.FontWeight = FontWeights.Bold;
                    runCellText53.FontSize = fontSizeBody;
                    cell53.PreferredWidth = new TableWidthUnit(cellWidth3);

                    TableRow row29a = table29.Rows.AddTableRow();
                    TableCell cell53a = row29a.Cells.AddTableCell();
                    Paragraph cellPar53a = cell53a.Blocks.AddParagraph();

                    Run runCellText53a = cellPar53a.Inlines.AddRun(string.Format("(Provide a detailed explanation.  A simple re-wording of the nonconformance will not be accepted. Terms such as oversight or human error require further explanation.)", 0, 0));
                    runCellText53a.FontSize = 10;
                    cell53a.PreferredWidth = new TableWidthUnit(cellWidth3);


                    ///Row 30
                    TableRow row30 = table29.Rows.AddTableRow();
                    TableCell cell54 = row30.Cells.AddTableCell();
                    Paragraph cellPar54 = cell54.Blocks.AddParagraph();

                    Run runCellText54 = cellPar54.Inlines.AddRun(string.Format(""));
                    runCellText54.ForegroundColor = BlueColor;
                    runCellText54.FontSize = fontSizeBody;
                    cell54.PreferredWidth = new TableWidthUnit(cellWidth3);

                    //editor.InsertBreak(BreakType.LineBreak);

                    //********* Question 2  *********//
                    ///Row 31
                    Table table31 = editor.InsertTable();
                    table31.StyleId = BuiltInStyleNames.TableGridStyleId;

                    TableRow row31 = table31.Rows.AddTableRow();
                    TableCell cell55 = row31.Cells.AddTableCell();
                    //cell55.Properties.BackgroundColor.LocalValue = new ThemableColor(themeColorType: ThemeColorType.Accent3);
                    Paragraph cellPar55 = cell55.Blocks.AddParagraph();

                    Run runCellText55 = cellPar55.Inlines.AddRun(string.Format("2.  What is the root cause / potential root cause of the non-conformance?", 0, 0));
                    runCellText55.FontWeight = FontWeights.Bold;
                    runCellText55.FontSize = fontSizeBody;
                    cell55.PreferredWidth = new TableWidthUnit(cellWidth3);

                    TableRow row31a = table31.Rows.AddTableRow();
                    TableCell cell55a = row31a.Cells.AddTableCell();
                    Paragraph cellPar55a = cell55a.Blocks.AddParagraph();

                    Run runCellText55a = cellPar55a.Inlines.AddRun(string.Format("(Use the final ?why? if the 5y methodology is utilized.)", 0, 0));
                    runCellText55a.FontSize = 10;
                    cell55a.PreferredWidth = new TableWidthUnit(cellWidth3);


                    ///Row 32
                    TableRow row32 = table31.Rows.AddTableRow();
                    TableCell cell56 = row32.Cells.AddTableCell();
                    Paragraph cellPar56 = cell56.Blocks.AddParagraph();

                    Run runCellText56 = cellPar56.Inlines.AddRun(string.Format(""));
                    runCellText56.ForegroundColor = BlueColor;
                    runCellText56.FontSize = fontSizeBody;
                    cell56.PreferredWidth = new TableWidthUnit(cellWidth3);

                    //editor.InsertBreak(BreakType.LineBreak);

                    //********* Question 3  *********//
                    ///Row 33
                    Table table33 = editor.InsertTable();
                    table33.StyleId = BuiltInStyleNames.TableGridStyleId;

                    TableRow row33 = table33.Rows.AddTableRow();
                    TableCell cell57 = row33.Cells.AddTableCell();
                    //cell57.Properties.BackgroundColor.LocalValue = new ThemableColor(themeColorType: ThemeColorType.Accent3);
                    Paragraph cellPar57 = cell57.Blocks.AddParagraph();

                    Run runCellText57 = cellPar57.Inlines.AddRun(string.Format("3.  Are there similar instances of this nonconformance in your area of responsibility? (Y/N) ", 0, 0));
                    runCellText57.FontWeight = FontWeights.Bold;
                    runCellText57.FontSize = fontSizeBody;
                    cell57.PreferredWidth = new TableWidthUnit(cellWidth3);

                    ///Row 34
                    TableRow row34 = table33.Rows.AddTableRow();
                    TableCell cell58 = row34.Cells.AddTableCell();
                    Paragraph cellPar58 = cell58.Blocks.AddParagraph();

                    Run runCellText58 = cellPar58.Inlines.AddRun(string.Format(""));
                    runCellText58.ForegroundColor = BlueColor;
                    runCellText58.FontSize = fontSizeBody;
                    cell58.PreferredWidth = new TableWidthUnit(cellWidth3);

                    TableRow row34a = table33.Rows.AddTableRow();
                    TableCell cell58a = row34a.Cells.AddTableCell();
                    Paragraph cellPar58a = cell58a.Blocks.AddParagraph();
                    //cell58.Properties.BackgroundColor.LocalValue = new ThemableColor(themeColorType: ThemeColorType.Accent3);
                    Run runCellText58a = cellPar58a.Inlines.AddRun(string.Format("If Yes, describe similar instance below.", 0, 0));
                    runCellText58a.FontSize = fontSizeBody;
                    cell58a.PreferredWidth = new TableWidthUnit(cellWidth3);

                    TableRow row34b = table33.Rows.AddTableRow();
                    TableCell cell58b = row34b.Cells.AddTableCell();
                    Paragraph cellPar58b = cell58b.Blocks.AddParagraph();

                    Run runCellText58b = cellPar58b.Inlines.AddRun(string.Format(""));
                    runCellText58b.ForegroundColor = BlueColor;
                    runCellText58b.FontSize = fontSizeBody;
                    cell58b.PreferredWidth = new TableWidthUnit(cellWidth3);

                    //editor.InsertBreak(BreakType.LineBreak);

                    //********* Question 4  *********//
                    ///Row 35
                    Table table35 = editor.InsertTable();
                    table35.StyleId = BuiltInStyleNames.TableGridStyleId;

                    TableRow row35 = table35.Rows.AddTableRow();
                    TableCell cell59 = row35.Cells.AddTableCell();
                    //cell59.Properties.BackgroundColor.LocalValue = new ThemableColor(themeColorType: ThemeColorType.Accent3);
                    Paragraph cellPar59 = cell59.Blocks.AddParagraph();

                    Run runCellText59 = cellPar59.Inlines.AddRun(string.Format("4.  What action was taken (or is planned) to correct this nonconformance", 0, 0));
                    runCellText59.FontWeight = FontWeights.Bold;
                    runCellText59.FontSize = fontSizeBody;
                    cell59.PreferredWidth = new TableWidthUnit(cellWidth3);

                    TableRow row35a = table35.Rows.AddTableRow();
                    TableCell cell59a = row35a.Cells.AddTableCell();
                    Paragraph cellPar59a = cell59a.Blocks.AddParagraph();

                    Run runCellText59a = cellPar59a.Inlines.AddRun(string.Format("(Only actions completed are considered fully acceptable.  Future system revisions, training sessions, reviews etc.though indicating intent do not substantiate completed action.)", 0, 0));
                    runCellText59a.FontSize = 10;
                    cell59a.PreferredWidth = new TableWidthUnit(cellWidth3);

                    ///Row 36
                    TableRow row36 = table35.Rows.AddTableRow();
                    TableCell cell60 = row36.Cells.AddTableCell();
                    Paragraph cellPar60 = cell60.Blocks.AddParagraph();

                    Run runCellText60 = cellPar60.Inlines.AddRun(string.Format(""));
                    runCellText60.ForegroundColor = BlueColor;
                    runCellText60.FontSize = fontSizeBody;
                    cell60.PreferredWidth = new TableWidthUnit(cellWidth3);

                    ///Row 37
                    TableRow row37 = table35.Rows.AddTableRow();
                    TableCell cell61 = row37.Cells.AddTableCell();
                    //cell61.Properties.BackgroundColor.LocalValue = new ThemableColor(themeColorType: ThemeColorType.Accent3);
                    Paragraph cellPar61 = cell61.Blocks.AddParagraph();

                    Run runCellText61 = cellPar61.Inlines.AddRun(string.Format("What is the scheduled implementation date? ", 0, 0));
                    runCellText61.FontSize = fontSizeBody;
                    cell61.PreferredWidth = new TableWidthUnit(cellWidth3);

                    ///Row 38
                    TableRow row38 = table35.Rows.AddTableRow();
                    TableCell cell62 = row38.Cells.AddTableCell();
                    Paragraph cellPar62 = cell62.Blocks.AddParagraph();

                    Run runCellText62 = cellPar62.Inlines.AddRun(string.Format(""));
                    runCellText62.ForegroundColor = BlueColor;
                    runCellText62.FontSize = fontSizeBody;
                    cell62.PreferredWidth = new TableWidthUnit(cellWidth3);

                    //editor.InsertBreak(BreakType.LineBreak);

                    //********* Question 5  *********//
                    ///Row 39
                    Table table39 = editor.InsertTable();
                    table39.StyleId = BuiltInStyleNames.TableGridStyleId;

                    TableRow row39 = table39.Rows.AddTableRow();
                    TableCell cell63 = row39.Cells.AddTableCell();
                    //cell63.Properties.BackgroundColor.LocalValue = new ThemableColor(themeColorType: ThemeColorType.Accent3);
                    Paragraph cellPar63 = cell63.Blocks.AddParagraph();

                    Run runCellText63 = cellPar63.Inlines.AddRun(string.Format("5.  What action was taken (or is planned) to preclude this and similar non-conformances?", 0, 0));
                    runCellText63.FontWeight = FontWeights.Bold;
                    runCellText63.FontSize = fontSizeBody;
                    cell63.PreferredWidth = new TableWidthUnit(cellWidth3);

                    TableRow row39a = table39.Rows.AddTableRow();
                    TableCell cell63a = row39a.Cells.AddTableCell();
                    Paragraph cellPar63a = cell63a.Blocks.AddParagraph();

                    Run runCellText63a = cellPar63a.Inlines.AddRun(string.Format("(Only actions completed are considered fully acceptable.  Future system revisions, training sessions, reviews etc.though indicating intent do not substantiate completed action.)", 0, 0));
                    runCellText63a.FontSize = 10;
                    cell63a.PreferredWidth = new TableWidthUnit(cellWidth3);

                    ///Row 40
                    TableRow row40 = table39.Rows.AddTableRow();
                    TableCell cell64 = row40.Cells.AddTableCell();
                    Paragraph cellPar64 = cell64.Blocks.AddParagraph();

                    Run runCellText64 = cellPar64.Inlines.AddRun(string.Format(""));
                    runCellText64.ForegroundColor = BlueColor;
                    runCellText64.FontSize = fontSizeBody;
                    cell64.PreferredWidth = new TableWidthUnit(cellWidth3);

                    ///Row 41
                    TableRow row41 = table39.Rows.AddTableRow();
                    TableCell cell65 = row41.Cells.AddTableCell();
                    //cell65.Properties.BackgroundColor.LocalValue = new ThemableColor(themeColorType: ThemeColorType.Accent3);
                    Paragraph cellPar65 = cell65.Blocks.AddParagraph();

                    Run runCellText65 = cellPar65.Inlines.AddRun(string.Format("What is the scheduled implementation date? ", 0, 0));
                    runCellText65.FontSize = fontSizeBody;
                    cell65.PreferredWidth = new TableWidthUnit(cellWidth3);

                    ///Row 42
                    TableRow row42 = table39.Rows.AddTableRow();
                    TableCell cell66 = row42.Cells.AddTableCell();
                    Paragraph cellPar66 = cell66.Blocks.AddParagraph();

                    Run runCellText66 = cellPar66.Inlines.AddRun(string.Format(""));
                    runCellText66.ForegroundColor = BlueColor;
                    runCellText66.FontSize = fontSizeBody;
                    cell66.PreferredWidth = new TableWidthUnit(cellWidth3);

                    ///Row 43                    
                    TableRow row43 = table39.Rows.AddTableRow();
                    TableCell cell67 = row43.Cells.AddTableCell();
                    Paragraph cellPar67 = cell67.Blocks.AddParagraph();

                    Run runCellText67 = cellPar67.Inlines.AddRun(string.Format("Action Taken By:", 0, 0));
                    runCellText67.FontWeight = FontWeights.Bold;
                    runCellText67.FontSize = fontSizeBody;
                    cell67.PreferredWidth = new TableWidthUnit(cellWidth);

                    ///Row 44     
                    TableRow row44 = table39.Rows.AddTableRow();
                    TableCell cell68 = row44.Cells.AddTableCell();
                    Paragraph cellPar68 = cell68.Blocks.AddParagraph();

                    Run runCellText68 = cellPar68.Inlines.AddRun(string.Format(""));
                    runCellText68.ForegroundColor = BlueColor;
                    runCellText68.FontSize = fontSizeBody;
                    cell68.PreferredWidth = new TableWidthUnit(cellWidth2);


                    ///Row 45
                    TableRow row45 = table39.Rows.AddTableRow();
                    TableCell cell69 = row45.Cells.AddTableCell();
                    Paragraph cellPar69 = cell69.Blocks.AddParagraph();

                    Run runCellText69 = cellPar69.Inlines.AddRun(string.Format("Response Date:", 0, 0));
                    runCellText69.FontWeight = FontWeights.Bold;
                    runCellText69.FontSize = fontSizeBody;
                    cell69.PreferredWidth = new TableWidthUnit(cellWidth);

                    ///Row 46
                    TableRow row46 = table39.Rows.AddTableRow();
                    TableCell cell70 = row46.Cells.AddTableCell();
                    Paragraph cellPar70 = cell70.Blocks.AddParagraph();

                    Run runCellText70 = cellPar70.Inlines.AddRun(string.Format(""));
                    runCellText70.ForegroundColor = BlueColor;
                    runCellText70.FontSize = fontSizeBody;
                    cell70.PreferredWidth = new TableWidthUnit(cellWidth2);

                    //if (htmlUtil.IsNumeric(row["CAR_STATUS_OID"].ToString()))
                    //{
                    //    if (int.Parse(row["CAR_STATUS_OID"].ToString()) == 3) //Closed
                    //    {

                    //        editor.InsertBreak(BreakType.PageBreak);

                    //        Run headerOrg = editor.InsertText("Originator Section");
                    //        headerResp.FontWeight = FontWeights.Bold;
                    //        headerResp.FontSize = fontSizeHeader;
                    //        headerResp.ForegroundColor = GrayColor;


                    //        ///Row 47
                    //        Table table47 = editor.InsertTable();
                    //        TableRow row47 = table47.Rows.AddTableRow();
                    //        TableCell cell71 = row47.Cells.AddTableCell();
                    //        Paragraph cellPar71 = cell71.Blocks.AddParagraph();

                    //        Run runCellText71 = cellPar71.Inlines.AddRun(string.Format("Due Date Extension:", 0, 0));
                    //        runCellText71.FontWeight = FontWeights.Bold;
                    //        runCellText71.FontSize = fontSizeBody;
                    //        cell71.PreferredWidth = new TableWidthUnit(cellWidth);

                    //        TableCell cell72 = row47.Cells.AddTableCell();
                    //        Paragraph cellPar72 = cell72.Blocks.AddParagraph();

                    //        Run runCellText72 = cellPar72.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["DUE_DT_EXT"].ToString())))), 0, 1));
                    //        runCellText72.ForegroundColor = BlueColor;
                    //        runCellText72.FontSize = fontSizeBody;
                    //        cell72.PreferredWidth = new TableWidthUnit(cellWidth2);


                    //        ///Row 48
                    //        TableRow row48 = table47.Rows.AddTableRow();
                    //        TableCell cell73 = row48.Cells.AddTableCell();
                    //        Paragraph cellPar73 = cell73.Blocks.AddParagraph();

                    //        Run runCellText73 = cellPar73.Inlines.AddRun(string.Format("Reissued To:", 0, 0));
                    //        runCellText73.FontWeight = FontWeights.Bold;
                    //        runCellText73.FontSize = fontSizeBody;
                    //        cell73.PreferredWidth = new TableWidthUnit(cellWidth);

                    //        TableCell cell74 = row48.Cells.AddTableCell();
                    //        Paragraph cellPar74 = cell74.Blocks.AddParagraph();

                    //        Run runCellText74 = cellPar74.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["REISSUED_TO_USR_NM"].ToString())))), 0, 1));
                    //        runCellText74.ForegroundColor = BlueColor;
                    //        runCellText74.FontSize = fontSizeBody;
                    //        cell74.PreferredWidth = new TableWidthUnit(cellWidth2);

                    //        ///Row 49
                    //        TableRow row49 = table47.Rows.AddTableRow();
                    //        TableCell cell75 = row49.Cells.AddTableCell();
                    //        Paragraph cellPar75 = cell75.Blocks.AddParagraph();

                    //        Run runCellText75 = cellPar75.Inlines.AddRun(string.Format("Date Reissued:", 0, 0));
                    //        runCellText75.FontWeight = FontWeights.Bold;
                    //        runCellText75.FontSize = fontSizeBody;
                    //        cell75.PreferredWidth = new TableWidthUnit(cellWidth);

                    //        TableCell cell76 = row49.Cells.AddTableCell();
                    //        Paragraph cellPar76 = cell76.Blocks.AddParagraph();

                    //        Run runCellText76 = cellPar76.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["REISSUED_DT_TEXT"].ToString())))), 0, 1));
                    //        runCellText76.ForegroundColor = BlueColor;
                    //        runCellText76.FontSize = fontSizeBody;
                    //        cell76.PreferredWidth = new TableWidthUnit(cellWidth2);

                    //        ///Row 50
                    //        TableRow row50 = table47.Rows.AddTableRow();
                    //        TableCell cell77 = row50.Cells.AddTableCell();
                    //        Paragraph cellPar77 = cell77.Blocks.AddParagraph();

                    //        Run runCellText77 = cellPar77.Inlines.AddRun(string.Format("Date Received:", 0, 0));
                    //        runCellText77.FontWeight = FontWeights.Bold;
                    //        runCellText77.FontSize = fontSizeBody;
                    //        cell77.PreferredWidth = new TableWidthUnit(cellWidth);

                    //        TableCell cell78 = row50.Cells.AddTableCell();
                    //        Paragraph cellPar78 = cell78.Blocks.AddParagraph();

                    //        Run runCellText78 = cellPar78.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["RECEIVED_DT"].ToString())))), 0, 1));
                    //        runCellText78.ForegroundColor = BlueColor;
                    //        runCellText78.FontSize = fontSizeBody;
                    //        cell78.PreferredWidth = new TableWidthUnit(cellWidth2);

                    //        ///Row 51
                    //        TableRow row51 = table47.Rows.AddTableRow();
                    //        TableCell cell79 = row51.Cells.AddTableCell();
                    //        Paragraph cellPar79 = cell79.Blocks.AddParagraph();

                    //        Run runCellText79 = cellPar79.Inlines.AddRun(string.Format("Follow-up Required:", 0, 0));
                    //        runCellText79.FontWeight = FontWeights.Bold;
                    //        runCellText79.FontSize = fontSizeBody;
                    //        cell79.PreferredWidth = new TableWidthUnit(cellWidth);

                    //        TableCell cell80 = row51.Cells.AddTableCell();
                    //        Paragraph cellPar80 = cell80.Blocks.AddParagraph();

                    //        Run runCellText80 = cellPar80.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["FOLLOW_UP_REQD_TEXT"].ToString())))), 0, 1));
                    //        runCellText80.ForegroundColor = BlueColor;
                    //        runCellText80.FontSize = fontSizeBody;
                    //        cell80.PreferredWidth = new TableWidthUnit(cellWidth2);

                    //        ///Row 52
                    //        TableRow row52 = table47.Rows.AddTableRow();
                    //        TableCell cell81 = row52.Cells.AddTableCell();
                    //        Paragraph cellPar81 = cell81.Blocks.AddParagraph();

                    //        Run runCellText81 = cellPar81.Inlines.AddRun(string.Format("Date To Follow-up:", 0, 0));
                    //        runCellText81.FontWeight = FontWeights.Bold;
                    //        runCellText81.FontSize = fontSizeBody;
                    //        cell81.PreferredWidth = new TableWidthUnit(cellWidth);

                    //        TableCell cell82 = row52.Cells.AddTableCell();
                    //        Paragraph cellPar82 = cell82.Blocks.AddParagraph();

                    //        Run runCellText82 = cellPar82.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["FOLLOW_UP_DT"].ToString())))), 0, 1));
                    //        runCellText82.ForegroundColor = BlueColor;
                    //        runCellText82.FontSize = fontSizeBody;
                    //        cell82.PreferredWidth = new TableWidthUnit(cellWidth2);

                    //        ///Row 53
                    //        TableRow row53 = table47.Rows.AddTableRow();
                    //        TableCell cell83 = row53.Cells.AddTableCell();
                    //        Paragraph cellPar83 = cell83.Blocks.AddParagraph();

                    //        Run runCellText83 = cellPar83.Inlines.AddRun(string.Format("Date Verified:", 0, 0));
                    //        runCellText83.FontWeight = FontWeights.Bold;
                    //        runCellText83.FontSize = fontSizeBody;
                    //        cell83.PreferredWidth = new TableWidthUnit(cellWidth);

                    //        TableCell cell84 = row53.Cells.AddTableCell();
                    //        Paragraph cellPar84 = cell84.Blocks.AddParagraph();

                    //        Run runCellText84 = cellPar84.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["VERIFY_DT"].ToString())))), 0, 1));
                    //        runCellText84.ForegroundColor = BlueColor;
                    //        runCellText84.FontSize = fontSizeBody;
                    //        cell84.PreferredWidth = new TableWidthUnit(cellWidth2);

                    //        ///Row 54
                    //        TableRow row54 = table47.Rows.AddTableRow();
                    //        TableCell cell85 = row54.Cells.AddTableCell();
                    //        Paragraph cellPar85 = cell85.Blocks.AddParagraph();

                    //        Run runCellText85 = cellPar85.Inlines.AddRun(string.Format("Verified by:", 0, 0));
                    //        runCellText85.FontWeight = FontWeights.Bold;
                    //        runCellText85.FontSize = fontSizeBody;
                    //        cell85.PreferredWidth = new TableWidthUnit(cellWidth);

                    //        TableCell cell86 = row54.Cells.AddTableCell();
                    //        Paragraph cellPar86 = cell86.Blocks.AddParagraph();

                    //        Run runCellText86 = cellPar86.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["VERIFIED_BY_USR_NM"].ToString())))), 0, 1));
                    //        runCellText86.ForegroundColor = BlueColor;
                    //        runCellText86.FontSize = fontSizeBody;
                    //        cell86.PreferredWidth = new TableWidthUnit(cellWidth2);


                    //        ///Row 55
                    //        TableRow row55 = table47.Rows.AddTableRow();
                    //        TableCell cell87 = row55.Cells.AddTableCell();
                    //        Paragraph cellPar87 = cell87.Blocks.AddParagraph();

                    //        Run runCellText87 = cellPar87.Inlines.AddRun(string.Format("Response Accepted By:", 0, 0));
                    //        runCellText87.FontWeight = FontWeights.Bold;
                    //        runCellText87.FontSize = fontSizeBody;
                    //        cell87.PreferredWidth = new TableWidthUnit(cellWidth);

                    //        TableCell cell88 = row55.Cells.AddTableCell();
                    //        Paragraph cellPar88 = cell88.Blocks.AddParagraph();

                    //        Run runCellText88 = cellPar88.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["RESPONSE_ACCEPT_BY_USR_NM"].ToString())))), 0, 1));
                    //        runCellText88.ForegroundColor = BlueColor;
                    //        runCellText88.FontSize = fontSizeBody;
                    //        cell88.PreferredWidth = new TableWidthUnit(cellWidth2);

                    //        ///Row 56
                    //        TableRow row56 = table47.Rows.AddTableRow();
                    //        TableCell cell89 = row56.Cells.AddTableCell();
                    //        Paragraph cellPar89 = cell89.Blocks.AddParagraph();

                    //        Run runCellText89 = cellPar89.Inlines.AddRun(string.Format("Status:", 0, 0));
                    //        runCellText89.FontWeight = FontWeights.Bold;
                    //        runCellText89.FontSize = fontSizeBody;
                    //        cell89.PreferredWidth = new TableWidthUnit(cellWidth);

                    //        TableCell cell90 = row56.Cells.AddTableCell();
                    //        Paragraph cellPar90 = cell90.Blocks.AddParagraph();

                    //        Run runCellText90 = cellPar90.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["CAR_STATUS_NM"].ToString())))), 0, 1));
                    //        runCellText90.ForegroundColor = BlueColor;
                    //        runCellText90.FontSize = fontSizeBody;
                    //        cell90.PreferredWidth = new TableWidthUnit(cellWidth2);

                    //        ///Row 57
                    //        TableRow row57 = table47.Rows.AddTableRow();
                    //        TableCell cell91 = row57.Cells.AddTableCell();
                    //        Paragraph cellPar91 = cell91.Blocks.AddParagraph();

                    //        Run runCellText91 = cellPar91.Inlines.AddRun(string.Format("Close Date:", 0, 0));
                    //        runCellText91.FontWeight = FontWeights.Bold;
                    //        runCellText91.FontSize = fontSizeBody;
                    //        cell91.PreferredWidth = new TableWidthUnit(cellWidth);

                    //        TableCell cell92 = row57.Cells.AddTableCell();
                    //        Paragraph cellPar92 = cell92.Blocks.AddParagraph();

                    //        Run runCellText92 = cellPar92.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["CLOSE_DT"].ToString())))), 0, 1));
                    //        runCellText92.ForegroundColor = BlueColor;
                    //        runCellText92.FontSize = fontSizeBody;
                    //        cell92.PreferredWidth = new TableWidthUnit(cellWidth2);

                    //        editor.InsertBreak(BreakType.LineBreak);

                    //        ///Row 58
                    //        Table table58 = editor.InsertTable();
                    //        TableRow row58 = table58.Rows.AddTableRow();
                    //        TableCell cell93 = row58.Cells.AddTableCell();
                    //        Paragraph cellPar93 = cell93.Blocks.AddParagraph();

                    //        Run runCellText93 = cellPar93.Inlines.AddRun(string.Format("How was effectiveness validated? ", 0, 0));
                    //        runCellText93.FontWeight = FontWeights.Bold;
                    //        runCellText93.FontSize = fontSizeBody;
                    //        cell93.PreferredWidth = new TableWidthUnit(cellWidth3);

                    //        ///Row 69
                    //        TableRow row59 = table58.Rows.AddTableRow();
                    //        TableCell cell94 = row59.Cells.AddTableCell();
                    //        Paragraph cellPar94 = cell94.Blocks.AddParagraph();

                    //        Run runCellText94 = cellPar94.Inlines.AddRun(string.Format(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["REMARKS"].ToString())))), 0, 1));
                    //        runCellText94.ForegroundColor = BlueColor;
                    //        runCellText94.FontSize = fontSizeBody;
                    //        cell94.PreferredWidth = new TableWidthUnit(cellWidth3);
                    //    }
                    //}



                    formatProvider.Export(document, ms);

                }
            }

        }
        catch (Exception ex)
        {
            if (ex.GetType().Name.ToString() != "ThreadAbortException")
            {
                LogException(ex);
            }
        }
        finally
        {
            ms.Close();
        }

        return ms;
    }

    private void LogException(Exception ex)
    {
        Applog appLog;
        appLog = new Applog();

        appLog.ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`");
        appLog.ExceptionSource = "External_Class_CreateNewDocx.cs";
        appLog.ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`");
        appLog.ExceptionUrl = MySession.Current.SessionHostName.ToString().ToUpper();
        appLog.ExceptionTargetSite = ex.TargetSite.ToString().Replace(oldValue: "'", newValue: "`");
        appLog.ExceptionStackTrace = ex.StackTrace.ToString().Replace(oldValue: "'", newValue: "`");
        appLog.AppLogEvent();
    }
}