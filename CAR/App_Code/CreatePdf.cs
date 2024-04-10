using CAR;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

public class CreatePdf
{
    public string Car_Oid { set; get; }


    public SqlDataAdapter Sda = new SqlDataAdapter();
    public string ConStr = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
    public string ErrException = ConfigurationManager.AppSettings["ErrorMessage"] + ConfigurationManager.AppSettings["AdminContact"];

    SanitizeString removeChar = new SanitizeString();
    MyUtil.HtmlUtility htmlUtil = MyUtil.HtmlUtility.Instance;

    public MemoryStream GetPdfFile()
    {
        MemoryStream myMemoryStream = new MemoryStream();

        try
        {
            float[] Width2 = new float[] { 3f, 12f };
            float[] Width21 = new float[] { 15f, 30f };
            float[] Width3 = new float[] { 1f, 6f, 1f };
            float[] Width4 = new float[] { 2f, 5f, 2f, 5f };
            float[] Width41 = new float[] { 3f, 5f, 4f, 5f };
            float[] Width42 = new float[] { 3f, 5f, 2f, 2f };
            float[] Width43 = new float[] { 1f, 5f, 1f, 1f };
            float[] Width5 = new float[] { 1f, 1f, 1f, 4f, 1f };
            float[] Width51 = new float[] { 1f, 5f, 1f, 1f, 1f };
            float[] Width6 = new float[] { 1f, 1f, 1f, 6f, 1f, 1f };


            iTextSharp.text.Font font11BoldBlack = FontFactory.GetFont("ARIAL", 11, 1, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font font11Bold = FontFactory.GetFont("ARIAL", 11, 1, iTextSharp.text.BaseColor.WHITE);
            iTextSharp.text.Font font10Bold = FontFactory.GetFont("ARIAL", 10, 1);
            iTextSharp.text.Font font10 = FontFactory.GetFont("ARIAL", 10);
            iTextSharp.text.Font font7 = FontFactory.GetFont("ARIAL", 7);
            //MemoryStream myMemoryStream = new MemoryStream();

            iTextSharp.text.Document doc;
            doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, 10, 10, 20, 20);

            PdfWriter wri = PdfWriter.GetInstance(doc, myMemoryStream);

            wri.CloseStream = false;
            doc.Open();

            GetRecord xCar;
            xCar = new GetRecord();
            xCar.CarOid = Car_Oid;

            using (DataTable dt = xCar.Exec_Get_Existing_Car_By_Oid_Datatable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    doc.AddTitle("Halliburton Corrective Action");
                    doc.AddSubject("CAR " + row["CAR_NBR"].ToString());
                    doc.AddCreator("Corrective Action");
                    doc.AddAuthor(removeChar.SanitizeQuoteString(MySession.Current.SessionFullName));


                    //********* Create General Info Header *********//
                    PdfPTable tableInfo = new PdfPTable(2);
                    tableInfo.TotalWidth = 580f;
                    tableInfo.LockedWidth = true;
                    tableInfo.SetWidths(Width21);
                    tableInfo.SpacingAfter = 7f;

                    PdfPCell headerInfo = new PdfPCell(new Phrase("Halliburton - Corrective Action Request #" + row["CAR_NBR"].ToString(), font11Bold));
                    headerInfo.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

                    headerInfo.Colspan = 2;
                    tableInfo.AddCell(headerInfo);

                    //*** Col1 Header
                    PdfPTable tblCol1 = new PdfPTable(1);
                    tblCol1.AddCell(new Phrase("Originator", font10Bold));
                    tblCol1.AddCell(new Phrase("Date Issued", font10Bold));
                    tblCol1.AddCell(new Phrase("Due Date", font10Bold));
                    tblCol1.AddCell(new Phrase("Finding Type", font10Bold));
                    tblCol1.AddCell(new Phrase("Area", font10Bold));
                    tblCol1.AddCell(new Phrase("PSL", font10Bold));
                    tblCol1.AddCell(new Phrase("Plant", font10Bold));

                    string[] strApiHeader = row["API_ISO_ELEM"].ToString().Split('|');
                    for (int i = 0; i < (strApiHeader.Length); i++)
                    {
                        if (i == 0)
                        {
                            tblCol1.AddCell(new Phrase("API/ISO Reference", font10Bold));
                        }
                        else
                        {
                            tblCol1.AddCell(new Phrase(" ", font10Bold));
                        }
                    }

                    tblCol1.AddCell(new Phrase("Audit Number", font10Bold));
                    tblCol1.AddCell(new Phrase("Q Note Number", font10Bold));
                    tblCol1.AddCell(new Phrase("CPI Number", font10Bold));
                    tblCol1.AddCell(new Phrase("Material Number", font10Bold));
                    tblCol1.AddCell(new Phrase("Purchase Order Number", font10Bold));
                    tblCol1.AddCell(new Phrase("Production Order Number", font10Bold));
                    tblCol1.AddCell(new Phrase("API Audit Number", font10Bold));
                    tblCol1.AddCell(new Phrase("Maintenance Order Number", font10Bold));
                    tblCol1.AddCell(new Phrase("Equipment Number", font10Bold));
                    tblCol1.AddCell(new Phrase("Category", font10Bold));
                    tblCol1.AddCell(new Phrase("Responsible Person", font10Bold));
                    tblCol1.AddCell(new Phrase("Supplier Number", font10Bold));
                    tblCol1.AddCell(new Phrase("Supplier Name", font10Bold));

                    tblCol1.AddCell(new Phrase("Location/Country", font10Bold));
                    tblCol1.AddCell(new Phrase("Location/City State", font10Bold));

                    tblCol1.AddCell(new Phrase("Issued To", font10Bold));
                    tblCol1.AddCell(new Phrase("Issued To E-Mail", font10Bold));

                    tblCol1.AddCell(new Phrase("Action Taken By", font10Bold));
                    tblCol1.AddCell(new Phrase("Reponse Date", font10Bold));

                    PdfPCell nestCol1 = new PdfPCell(tblCol1);
                    nestCol1.Padding = 0f;
                    tableInfo.AddCell(nestCol1);

                    //*** Col2 Data
                    PdfPTable tblCol2 = new PdfPTable(1);
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["ORIGINATOR_USR_NM"].ToString())))), font10));
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["DATE_ISSUED"].ToString())))), font10));
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["DUE_DT"].ToString())))), font10));
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["FINDING_TYPE_NM"].ToString())))), font10));
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["AREA_DESCRIPT_NM"].ToString())))), font10));
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["PSL_NM"].ToString())))), font10));
                    tblCol2.AddCell(new Phrase(removeChar.SanitizeField(row["PLNT_CD"].ToString() + " - " + removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["FACILITY_NAME"].ToString())))), font10));

                    string[] strApiiso = row["API_ISO_ELEM"].ToString().Split('|');

                    if (strApiiso.Length == 0)
                    {
                        tblCol2.AddCell(new Phrase(row["API_ISO_ELEM"].ToString(), font10));
                    }
                    else
                    {
                        foreach (string api in strApiiso)
                        {
                            if (string.IsNullOrEmpty(api.ToString().Trim()))
                            {
                                tblCol2.AddCell(new Phrase(" ", font10));
                            }
                            else
                            {
                                tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(HttpUtility.HtmlDecode(htmlUtil.SanitizeHtml(api.ToString())))), font10));
                            }
                        }
                    }

                    if (row["AUDIT_NBR"].ToString().Trim().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["AUDIT_NBR"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["QNOTE_NBR"].ToString().Trim().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["QNOTE_NBR"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["CPI_NBR"].ToString().Trim().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["CPI_NBR"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["MATERIAL_NBR"].ToString().Trim().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["MATERIAL_NBR"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["PURCHASE_ORDER_NBR"].ToString().Trim().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["PURCHASE_ORDER_NBR"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["PRODUCTION_ORDER_NBR"].ToString().Trim().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["PRODUCTION_ORDER_NBR"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["API_AUDIT_NBR"].ToString().Trim().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["API_AUDIT_NBR"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["MAINTENANCE_ORDER_NBR"].ToString().Trim().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["MAINTENANCE_ORDER_NBR"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["EQUIPMENT_NBR"].ToString().Trim().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["EQUIPMENT_NBR"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["CATEGORY_NM"].ToString().Trim().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["CATEGORY_NM"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["RESP_PERSON_USR_NM"].ToString().Trim().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["RESP_PERSON_USR_NM"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["VNDR_NBR"].ToString().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["VNDR_NBR"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["VENDOR_NM"].ToString().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["VENDOR_NM"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["LOC_COUNTRY_NM"].ToString().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["LOC_COUNTRY_NM"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["LOC_SUPPLIER"].ToString().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["LOC_SUPPLIER"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["ISSUED_TO_USR_NM"].ToString().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["ISSUED_TO_USR_NM"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["ISSUED_TO_USR_EMAIL"].ToString().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["ISSUED_TO_USR_EMAIL"].ToString().Trim()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }


                    if (row["ACTION_TAKEN_BY_USR_NM"].ToString().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["ACTION_TAKEN_BY_USR_NM"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    if (row["RESPONSE_DT"].ToString().Length > 0)
                    {
                        tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["RESPONSE_DT"].ToString()))), font10));
                    }
                    else
                    {
                        tblCol2.AddCell(new Phrase(" ", font10));
                    }

                    PdfPCell nestCol2 = new PdfPCell(tblCol2);
                    nestCol2.Padding = 0f;
                    tableInfo.AddCell(nestCol2);

                    tableInfo.SpacingAfter = 7f;
                    doc.Add(tableInfo);


                    //********* Create Description of Finding Header *********//
                    PdfPTable tableDof = new PdfPTable(1);
                    tableDof.TotalWidth = 580f;
                    tableDof.LockedWidth = true;
                    //tableInfo.SetWidths(Width41);
                    tableDof.SpacingAfter = 7f;

                    PdfPCell headerDof = new PdfPCell(new Phrase("Description of Finding", font11Bold));
                    headerDof.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

                    tableDof.AddCell(headerDof);

                    //*** Col Dof
                    PdfPTable tblCol_Dof = new PdfPTable(1);
                    tblCol_Dof.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["FINDING_DESC"].ToString())))), font10));

                    PdfPCell nestDof = new PdfPCell(tblCol_Dof);
                    nestDof.Padding = 0f;

                    tableDof.AddCell(nestDof);

                    tableDof.SpacingAfter = 7f;
                    doc.Add(tableDof);


                    //********* Create Description of Improvement Header *********//
                    PdfPTable tableDoi = new PdfPTable(1);
                    tableDoi.TotalWidth = 580f;
                    tableDoi.LockedWidth = true;
                    tableDoi.SpacingAfter = 7f;

                    PdfPCell headerDoi = new PdfPCell(new Phrase("Description of Improvement", font11Bold));
                    headerDoi.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

                    tableDoi.AddCell(headerDoi);

                    //*** Col Dof
                    PdfPTable tblCol_Doi = new PdfPTable(1);
                    tblCol_Doi.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["DESC_OF_IMPROVEMENT"].ToString())))), font10));

                    PdfPCell nestDoi = new PdfPCell(tblCol_Doi);
                    nestDoi.Padding = 0f;

                    tableDoi.AddCell(nestDoi);

                    tableDoi.SpacingAfter = 7f;
                    doc.Add(tableDoi);

                    doc.NewPage();

                    PdfPTable tableResp = new PdfPTable(1);
                    tableResp.TotalWidth = 580f;
                    tableResp.LockedWidth = true;
                    tableResp.SpacingAfter = 7f;

                    PdfPCell headerResp = new PdfPCell(new Phrase("Respondent Section", font11BoldBlack));
                    headerResp.Border = 0;
                    headerResp.HorizontalAlignment = 1;
                    tableResp.AddCell(headerResp);

                    PdfPTable tblCol_Resp = new PdfPTable(1);

                    PdfPCell colResp = new PdfPCell(new Phrase("Please investigate and respond to the following. All questions must be answered. Any documentation that will substantiate the corrective action and/or action completed to minimize recurrence should be submitted with your response.", font7));
                    colResp.Border = 0;
                    colResp.HorizontalAlignment = 1;
                    tblCol_Resp.AddCell(colResp);

                    PdfPCell nestResp = new PdfPCell(tblCol_Resp);
                    nestResp.Padding = 0f;
                    nestResp.Border = 0;
                    nestResp.HorizontalAlignment = 1;
                    tableResp.AddCell(nestResp);
                    tableResp.SpacingAfter = 7f;

                    doc.Add(tableResp);


                    //********* Question 1  *********//
                    PdfPTable tableQ1 = new PdfPTable(1);
                    tableQ1.TotalWidth = 580f;
                    tableQ1.LockedWidth = true;
                    tableQ1.SpacingAfter = 7f;

                    PdfPCell headerQ1 = new PdfPCell(new Phrase("1.  Why did this nonconformance occur?", font11Bold));
                    headerQ1.BackgroundColor = iTextSharp.text.BaseColor.BLACK;
                    tableQ1.AddCell(headerQ1);

                    PdfPCell colQ = new PdfPCell(new Phrase("(Provide a detailed explanation.  A simple re-wording of the nonconformance will not be accepted. Terms such as oversight or human error require further explanation.)", font7));
                    colQ.Border = 0;
                    tableQ1.AddCell(colQ);

                    PdfPTable tblCol_Q1 = new PdfPTable(1);
                    PdfPCell colA1 = new PdfPCell();
                    colA1.Border = 0;
                    if (row["NON_CONFORM_RSN"].ToString().Length > 0)
                    {
                        colA1.Phrase = new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["NON_CONFORM_RSN"].ToString()))), font10);
                    }
                    else
                    {
                        colA1.Phrase = new Phrase(" ", font10);
                    }

                    tableQ1.AddCell(colA1);

                    tableQ1.SpacingAfter = 7f;
                    doc.Add(tableQ1);

                    //********* Question 2  *********//
                    PdfPTable tableQ2 = new PdfPTable(1);
                    tableQ2.TotalWidth = 580f;
                    tableQ2.LockedWidth = true;
                    tableQ2.SpacingAfter = 7f;

                    PdfPCell headerQ2 = new PdfPCell(new Phrase("2.  What is the root cause / potential root cause of the non-conformance?", font11Bold));
                    headerQ2.BackgroundColor = iTextSharp.text.BaseColor.BLACK;
                    tableQ2.AddCell(headerQ2);

                    PdfPCell colQ2 = new PdfPCell(new Phrase("(Use the final ?why? if the 5y methodology is utilized.)", font7));
                    colQ2.Border = 0;
                    tableQ2.AddCell(colQ2);

                    PdfPTable tblCol_Q2 = new PdfPTable(1);
                    PdfPCell colA2 = new PdfPCell();
                    colA2.Border = 0;
                    if (row["ROOT_CAUSE"].ToString().Length > 0)
                    {
                        colA2.Phrase = new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["ROOT_CAUSE"].ToString()))), font10);
                    }
                    else
                    {
                        colA2.Phrase = new Phrase(" ", font10);
                    }

                    tableQ2.AddCell(colA2);
                    tableQ2.SpacingAfter = 7f;
                    doc.Add(tableQ2);

                    //********* Question 3  *********//
                    PdfPTable tableQ3 = new PdfPTable(1);
                    tableQ3.TotalWidth = 580f;
                    tableQ3.LockedWidth = true;
                    tableQ3.SpacingAfter = 7f;

                    PdfPCell headerQ3 = new PdfPCell(new Phrase("3.  Are there similar instances of this nonconformance in your area of responsibility? (Y/N)", font11Bold));
                    headerQ3.BackgroundColor = iTextSharp.text.BaseColor.BLACK;
                    tableQ3.AddCell(headerQ3);

                    PdfPTable tblCol_Q3 = new PdfPTable(1);
                    PdfPCell colA3 = new PdfPCell();
                    colA3.Border = 0;
                    colA3.Phrase = new Phrase(row["SIMILAR_INSTANCE_Y_N_TEXT"].ToString(), font10);

                    tableQ3.AddCell(colA3);

                    PdfPCell colQ3 = new PdfPCell(new Phrase("If Yes, describe similar instance below.", font7));
                    colQ3.Border = 0;
                    tableQ3.AddCell(colQ3);

                    if (row["SIMILAR_INSTANCE_Y_N_TEXT"].ToString().ToUpper() == "YES")
                    {
                        PdfPTable tblCol_Q3Y = new PdfPTable(1);
                        PdfPCell colA3Y = new PdfPCell();
                        colA3Y.Border = 0;

                        colA3Y.Phrase = new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["SIMILAR_INSTANCE"].ToString()))), font10);

                        tableQ3.AddCell(colA3Y);
                    }
                    tableQ3.SpacingAfter = 7f;
                    doc.Add(tableQ3);


                    //********* Question 4  *********//
                    PdfPTable tableQ4 = new PdfPTable(1);
                    tableQ4.TotalWidth = 580f;
                    tableQ4.LockedWidth = true;
                    tableQ4.SpacingAfter = 7f;

                    PdfPCell headerQ4 = new PdfPCell(new Phrase("4.  What action was taken (or is planned) to correct this nonconformance?", font11Bold));
                    headerQ4.BackgroundColor = iTextSharp.text.BaseColor.BLACK;
                    tableQ4.AddCell(headerQ4);

                    PdfPCell colQ4 = new PdfPCell(new Phrase("(Only actions completed are considered fully acceptable.  Future system revisions, training sessions, reviews etc.though indicating intent do not substantiate completed action.)", font7));
                    colQ4.Border = 0;
                    tableQ4.AddCell(colQ4);

                    PdfPTable tblCol_Q4 = new PdfPTable(1);
                    PdfPCell colA4 = new PdfPCell();
                    colA4.Border = 0;

                    if (row["CORR_ACTION_TAKEN"].ToString().Length > 0)
                    {
                        colA4.Phrase = new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["CORR_ACTION_TAKEN"].ToString()))), font10);
                    }
                    else
                    {
                        colA4.Phrase = new Phrase(" ", font10);
                    }

                    tableQ4.AddCell(colA4);
                    tableQ4.SpacingAfter = 7f;
                    doc.Add(tableQ4);

                    PdfPTable tableQ4D = new PdfPTable(1);
                    tableQ4D.TotalWidth = 580f;
                    tableQ4D.LockedWidth = true;
                    tableQ4D.SpacingAfter = 7f;

                    PdfPCell headerQ4D = new PdfPCell(new Phrase("What is the scheduled implementation date?", font10));
                    headerQ4D.Border = 0;
                    //headerQ4D.BackgroundColor = iTextSharp.text.BaseColor.BLACK;
                    tableQ4D.AddCell(headerQ4D);

                    PdfPTable tblCol_Q4D = new PdfPTable(1);
                    PdfPCell colA4D = new PdfPCell();
                    colA4D.Border = 0;

                    if (row["CORR_ACTION_TAKEN_DT"].ToString().Length > 0)
                    {
                        colA4D.Phrase = new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["CORR_ACTION_TAKEN_DT"].ToString()))), font10);
                    }
                    else
                    {
                        colA4D.Phrase = new Phrase(" ", font10);
                    }

                    tableQ4D.AddCell(colA4D);

                    tableQ4D.SpacingAfter = 7f;
                    doc.Add(tableQ4D);

                    //********* Question 5  *********//
                    PdfPTable tableQ5 = new PdfPTable(1);
                    tableQ5.TotalWidth = 580f;
                    tableQ5.LockedWidth = true;
                    tableQ5.SpacingAfter = 7f;

                    PdfPCell headerQ5 = new PdfPCell(new Phrase("5.  What action was taken (or is planned) to preclude this and similar non-conformances?", font11Bold));
                    headerQ5.BackgroundColor = iTextSharp.text.BaseColor.BLACK;
                    tableQ5.AddCell(headerQ5);

                    PdfPCell colQ5 = new PdfPCell(new Phrase("(Only actions completed are considered fully acceptable.  Future system revisions, training sessions, reviews etc.though indicating intent do not substantiate completed action.)", font7));
                    colQ5.Border = 0;
                    tableQ5.AddCell(colQ5);

                    PdfPTable tblCol_Q5 = new PdfPTable(1);
                    PdfPCell colA5 = new PdfPCell();
                    colA5.Border = 0;

                    if (row["PRECLUDE_ACTION"].ToString().Length > 0)
                    {
                        colA5.Phrase = new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["PRECLUDE_ACTION"].ToString()))), font10);
                    }
                    else
                    {
                        colA5.Phrase = new Phrase(" ", font10);
                    }

                    tableQ5.AddCell(colA5);

                    tableQ5.SpacingAfter = 7f;
                    doc.Add(tableQ5);

                    PdfPTable tableQ5D = new PdfPTable(1);
                    tableQ5D.TotalWidth = 580f;
                    tableQ5D.LockedWidth = true;
                    tableQ5D.SpacingAfter = 7f;

                    PdfPCell headerQ5D = new PdfPCell(new Phrase("What is the scheduled implementation date? ", font10));
                    //headerQ5D.BackgroundColor = iTextSharp.text.BaseColor.BLACK;
                    headerQ5D.Border = 0;
                    tableQ5D.AddCell(headerQ5D);

                    PdfPTable tblCol_Q5D = new PdfPTable(1);
                    PdfPCell colA5D = new PdfPCell();
                    colA5D.Border = 0;

                    if (row["PRECLUDE_ACTION_DT"].ToString().Length > 0)
                    {
                        colA5D.Phrase = new Phrase(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["PRECLUDE_ACTION_DT"].ToString()))), font10);
                    }
                    else
                    {
                        colA5D.Phrase = new Phrase(" ", font10);
                    }

                    tableQ5D.AddCell(colA5D);
                    tableQ5D.SpacingAfter = 7f;
                    doc.Add(tableQ5D);

                    if (htmlUtil.IsNumeric(row["CAR_STATUS_OID"].ToString()))
                    {
                        if (int.Parse(row["CAR_STATUS_OID"].ToString()) == 3) //Closed
                        {
                            doc.NewPage();

                            //********* Create Originator Header *********//
                            PdfPTable tableOrig = new PdfPTable(2);
                            tableOrig.TotalWidth = 580f;
                            tableOrig.LockedWidth = true;
                            tableOrig.SetWidths(Width21);
                            tableOrig.SpacingAfter = 7f;

                            PdfPCell headerOrig = new PdfPCell(new Phrase("Originator Use Only", font11Bold));
                            headerOrig.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

                            headerOrig.Colspan = 2;
                            tableOrig.AddCell(headerOrig);

                            //*** Col1 Header
                            PdfPTable tblCol = new PdfPTable(1);
                            tblCol.AddCell(new Phrase("Due Date Extension", font10Bold));
                            tblCol.AddCell(new Phrase("Re-Issued To", font10Bold));
                            tblCol.AddCell(new Phrase("Date Re-Issued", font10Bold));
                            tblCol.AddCell(new Phrase("Follow-up Required", font10Bold));
                            tblCol.AddCell(new Phrase("Date to Follow-up", font10Bold));
                            tblCol.AddCell(new Phrase("Date Recieved", font10Bold));
                            tblCol.AddCell(new Phrase("Date Verified", font10Bold));
                            tblCol.AddCell(new Phrase("Verified By", font10Bold));
                            tblCol.AddCell(new Phrase("Response Accepted By", font10Bold));
                            tblCol.AddCell(new Phrase("Status", font10Bold));
                            tblCol.AddCell(new Phrase("Close Date", font10Bold));
                            tblCol.AddCell(new Phrase("How was effectiveness validated?", font10Bold));

                            PdfPCell nestCol = new PdfPCell(tblCol);
                            nestCol.Padding = 0f;
                            tableOrig.AddCell(nestCol);

                            //*** Col2 Data
                            PdfPTable tblCol_2 = new PdfPTable(1);

                            if (row["DUE_DT_EXT"].ToString().Length > 0)
                            {
                                tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["DUE_DT_EXT"].ToString())))), font10));
                            }
                            else
                            {
                                tblCol_2.AddCell(new Phrase(" ", font10));
                            }

                            if (row["REISSUED_TO_USR_NM"].ToString().Length > 0)
                            {
                                tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["REISSUED_TO_USR_NM"].ToString())))), font10));
                            }
                            else
                            {
                                tblCol_2.AddCell(new Phrase(" ", font10));
                            }

                            if (row["REISSUED_DT_TEXT"].ToString().Length > 0)
                            {
                                tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["REISSUED_DT_TEXT"].ToString())))), font10));
                            }
                            else
                            {
                                tblCol_2.AddCell(new Phrase(" ", font10));
                            }

                            if (row["FOLLOW_UP_REQD_TEXT"].ToString().Length > 0)
                            {
                                tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["FOLLOW_UP_REQD_TEXT"].ToString())))), font10));
                            }
                            else
                            {
                                tblCol_2.AddCell(new Phrase(" ", font10));
                            }

                            if (row["FOLLOW_UP_DT"].ToString().Length > 0)
                            {
                                tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["FOLLOW_UP_DT"].ToString())))), font10));
                            }
                            else
                            {
                                tblCol_2.AddCell(new Phrase(" ", font10));
                            }

                            if (row["RECEIVED_DT"].ToString().Length > 0)
                            {
                                tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["RECEIVED_DT"].ToString())))), font10));
                            }
                            else
                            {
                                tblCol_2.AddCell(new Phrase(" ", font10));
                            }

                            if (row["VERIFY_DT"].ToString().Length > 0)
                            {
                                tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["VERIFY_DT"].ToString())))), font10));
                            }
                            else
                            {
                                tblCol_2.AddCell(new Phrase(" ", font10));
                            }

                            if (row["VERIFIED_BY_USR_NM"].ToString().Length > 0)
                            {
                                tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["VERIFIED_BY_USR_NM"].ToString())))), font10));
                            }
                            else
                            {
                                tblCol_2.AddCell(new Phrase(" ", font10));
                            }

                            if (row["RESPONSE_ACCEPT_BY_USR_NM"].ToString().Length > 0)
                            {
                                tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["RESPONSE_ACCEPT_BY_USR_NM"].ToString())))), font10));
                            }
                            else
                            {
                                tblCol_2.AddCell(new Phrase(" ", font10));
                            }

                            if (row["CAR_STATUS_NM"].ToString().Length > 0)
                            {
                                tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["CAR_STATUS_NM"].ToString())))), font10));
                            }
                            else
                            {
                                tblCol_2.AddCell(new Phrase(" ", font10));
                            }

                            if (row["CLOSE_DT"].ToString().Length > 0)
                            {
                                tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["CLOSE_DT"].ToString())))), font10));
                            }
                            else
                            {
                                tblCol_2.AddCell(new Phrase(" ", font10));
                            }

                            if (row["REMARKS"].ToString().Length > 0)
                            {
                                tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(htmlUtil.SanitizeHtml(HttpUtility.HtmlDecode(row["REMARKS"].ToString())))), font10));
                            }
                            else
                            {
                                tblCol_2.AddCell(new Phrase(" ", font10));
                            }

                            PdfPCell nestCol_2 = new PdfPCell(tblCol_2);
                            nestCol_2.Padding = 0f;
                            tableOrig.AddCell(nestCol_2);

                            doc.Add(tableOrig);
                        }
                    }

                    doc.Close();

                    myMemoryStream.Flush(); //Always caches me out  
                    myMemoryStream.Position = 0;
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
            myMemoryStream.Close();
        }

        return myMemoryStream;
    }


    public byte[] GetBlankPage()
    {
        byte[] retVal = null;

        Document doc;
        doc = new Document();

        MemoryStream memoryStream;
        memoryStream = new MemoryStream();
        PdfWriter writer;
        writer = PdfWriter.GetInstance(doc, memoryStream);

        PdfEvents pdfPageEventHelper = new PdfEvents();
        writer.PageEvent = pdfPageEventHelper;

        doc.Open();
        doc.Add(new Paragraph(str: "This page intentionally left blank"));


        writer.CloseStream = false;
        doc.Close();

        memoryStream.Position = 0;
        retVal = memoryStream.ToArray();

        return retVal;
    }



    private void LogException(Exception ex)
    {
        Applog appLog;
        appLog = new Applog();

        appLog.ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`");
        appLog.ExceptionSource = "External_Class_CreatePdf.cs";
        appLog.ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`");
        appLog.ExceptionUrl = MySession.Current.SessionHostName.ToString().ToUpper();
        appLog.ExceptionTargetSite = ex.TargetSite.ToString().Replace(oldValue: "'", newValue: "`");
        appLog.ExceptionStackTrace = ex.StackTrace.ToString().Replace(oldValue: "'", newValue: "`");
        appLog.AppLogEvent();
    }

}