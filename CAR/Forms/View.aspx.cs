using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace CAR.Forms
{
    public partial class View : System.Web.UI.Page
    {
        public string AdminContact = ConfigurationManager.AppSettings["AdminContact"].ToString();
        public string ErrMessage = ConfigurationManager.AppSettings["ErrorMessage"].ToString();
        public string ErrException = "If problem persists, please contact the administrator: " + ConfigurationManager.AppSettings["AdminContact"];

        SanitizeString removeChar = new SanitizeString();
        MyUtil.HtmlUtility htmlUtil = MyUtil.HtmlUtility.Instance;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["OID"] != null && Request.QueryString["Car_Nbr"] != null)
            {
                if (Int32.Parse(Request.QueryString["OID"].ToString()) > 0)
                {
                    Print2Pdf(Request.QueryString["OID"].ToString(), Request.QueryString["Car_Nbr"].ToString());
                    //PrintToPdf(Request.QueryString["OID"].ToString());
                }
                else
                {
                    Response.Write("Invalid Record ID.  Please verify and try again.");
                }
            }
        }

        private void Print2Pdf(string CarOid, string CarNbr)
        {
            try
            {
                string fileName = CarNbr + ".pdf";


                CreatePdf pdf = new CreatePdf();
                pdf.Car_Oid = CarOid;

                using (MemoryStream mStream = pdf.GetPdfFile())
                {
                    Response.AppendHeader("Content-Disposition", Convert.ToString("attachment; filename=") + fileName);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/pdf";
                    Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name.ToString() != "ThreadAbortException")
                {
                    LogException(ex);
                    throw new Exception("Error Report To PDF..." + ex.Message);
                }
            }
        }


        //private void PrintToPdf(string CarOid)
        //{
        //    try
        //    {
        //        string fileName;
        //        float[] Width2 = new float[] { 3f, 12f };
        //        float[] Width21 = new float[] { 15f, 30f };
        //        float[] Width3 = new float[] { 1f, 6f, 1f };
        //        float[] Width4 = new float[] { 2f, 5f, 2f, 5f };
        //        float[] Width41 = new float[] { 3f, 5f, 4f, 5f };
        //        float[] Width42 = new float[] { 3f, 5f, 2f, 2f };
        //        float[] Width43 = new float[] { 1f, 5f, 1f, 1f };
        //        float[] Width5 = new float[] { 1f, 1f, 1f, 4f, 1f };
        //        float[] Width51 = new float[] { 1f, 5f, 1f, 1f, 1f };
        //        float[] Width6 = new float[] { 1f, 1f, 1f, 6f, 1f, 1f };

        //        Font font11Bold = FontFactory.GetFont("ARIAL", 11, 1, iTextSharp.text.BaseColor.WHITE);
        //        Font font10Bold = FontFactory.GetFont("ARIAL", 10, 1);
        //        Font font10 = FontFactory.GetFont("ARIAL", 10);

        //        MemoryStream myMemoryStream = new MemoryStream();

        //        iTextSharp.text.Document doc;
        //        doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, 10, 10, 20, 20);

        //        PdfWriter wri = PdfWriter.GetInstance(doc, Response.OutputStream);
        //        doc.Open();

        //        GetRecord xCar;
        //        xCar = new GetRecord();
        //        xCar.CarOid = removeChar.SanitizeQuoteString(Server.HtmlEncode(htmlUtil.SanitizeHtml(CarOid)));

        //        using (DataTable dt = xCar.Exec_Get_Existing_Car_By_Oid_Datatable())
        //        {
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                fileName = "CAR_" + row["CAR_NBR"].ToString() + ".pdf";

        //                //********* Create General Info Header *********//
        //                PdfPTable tableInfo = new PdfPTable(2);
        //                tableInfo.TotalWidth = 580f;
        //                tableInfo.LockedWidth = true;
        //                tableInfo.SetWidths(Width21);
        //                tableInfo.SpacingAfter = 7f;

        //                PdfPCell headerInfo = new PdfPCell(new Phrase("Halliburton - Corrective Action Request #" + row["CAR_NBR"].ToString(), font11Bold));
        //                headerInfo.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

        //                headerInfo.Colspan = 2;
        //                tableInfo.AddCell(headerInfo);

        //                //*** Col1 Header
        //                PdfPTable tblCol1 = new PdfPTable(1);
        //                tblCol1.AddCell(new Phrase("Originator", font10Bold));
        //                tblCol1.AddCell(new Phrase("Date Issued", font10Bold));
        //                tblCol1.AddCell(new Phrase("Due Date", font10Bold));
        //                tblCol1.AddCell(new Phrase("Finding Type", font10Bold));
        //                tblCol1.AddCell(new Phrase("Area", font10Bold));
        //                tblCol1.AddCell(new Phrase("PSL", font10Bold));
        //                tblCol1.AddCell(new Phrase("Plant", font10Bold));

        //                string[] strApiHeader = row["API_ISO_ELEM"].ToString().Split('|');
        //                for (int i = 0; i < (strApiHeader.Length); i++)
        //                {
        //                    if(i==0)
        //                    {
        //                        tblCol1.AddCell(new Phrase("API/ISO Reference", font10Bold));
        //                    }
        //                    else
        //                    {
        //                        tblCol1.AddCell(new Phrase(" ", font10Bold));
        //                    }
        //                }

        //                tblCol1.AddCell(new Phrase("Audit Number", font10Bold));
        //                tblCol1.AddCell(new Phrase("Q Note Number", font10Bold));
        //                tblCol1.AddCell(new Phrase("CPI Number", font10Bold));
        //                tblCol1.AddCell(new Phrase("Material Number", font10Bold));
        //                tblCol1.AddCell(new Phrase("Purchase Order Number", font10Bold));
        //                tblCol1.AddCell(new Phrase("Production Order Number", font10Bold));
        //                tblCol1.AddCell(new Phrase("API Audit Number", font10Bold));
        //                tblCol1.AddCell(new Phrase("Category", font10Bold));
        //                tblCol1.AddCell(new Phrase("Responsible Person", font10Bold));
        //                tblCol1.AddCell(new Phrase("Vendor Number", font10Bold));
        //                tblCol1.AddCell(new Phrase("Vendor Name", font10Bold));
        //                tblCol1.AddCell(new Phrase("Issued To", font10Bold));
        //                tblCol1.AddCell(new Phrase("Location/Country", font10Bold));
        //                tblCol1.AddCell(new Phrase("Location/City State", font10Bold));

        //                tblCol1.AddCell(new Phrase("Action Taken By", font10Bold));
        //                tblCol1.AddCell(new Phrase("Reponse Date", font10Bold));

        //                PdfPCell nestCol1 = new PdfPCell(tblCol1);
        //                nestCol1.Padding = 0f;
        //                tableInfo.AddCell(nestCol1);

        //                //*** Col2 Data
        //                PdfPTable tblCol2 = new PdfPTable(1);
        //                tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["ORIGINATOR_USR_NM"].ToString())))), font10));
        //                tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["DATE_ISSUED"].ToString())))), font10));
        //                tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["DUE_DT"].ToString())))), font10));
        //                tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["FINDING_TYPE_NM"].ToString())))), font10));
        //                tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["AREA_DESCRIPT_NM"].ToString())))), font10));
        //                tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["PSL_NM"].ToString())))), font10));
        //                tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["FACILITY_NAME"].ToString())))), font10));

        //                string[] strApiiso = row["API_ISO_ELEM"].ToString().Split('|');

        //                if(strApiiso.Length == 0)
        //                {
        //                    tblCol2.AddCell(new Phrase(row["API_ISO_ELEM"].ToString(), font10));
        //                }
        //                else
        //                {
        //                    foreach (string api in strApiiso)
        //                    {
        //                        if (string.IsNullOrEmpty(api.ToString().Trim()))
        //                        {
        //                            tblCol2.AddCell(new Phrase(" ", font10));
        //                        }
        //                        else
        //                        {
        //                            tblCol2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(api.ToString())))), font10));
        //                        }
        //                    }
        //                }

        //                tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["AUDIT_NBR"].ToString()))), font10));
        //                tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["QNOTE_NBR"].ToString()))), font10));
        //                tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["CPI_NBR"].ToString()))), font10));
        //                tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["MATERIAL_NBR"].ToString()))), font10));
        //                tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["PURCHASE_ORDER_NBR"].ToString()))), font10));
        //                tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["PRODUCTION_ORDER_NBR"].ToString()))), font10));
        //                tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["API_AUDIT_NBR"].ToString()))), font10));
        //                tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["CATEGORY_NM"].ToString()))), font10));

        //                if(row["RESP_PERSON_USR_NM"].ToString().Trim().Length >0)
        //                {
        //                    tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["RESP_PERSON_USR_NM"].ToString()))), font10));
        //                }
        //                else
        //                {
        //                    tblCol2.AddCell(new Phrase(" ", font10));
        //                }

        //                if (row["VNDR_NBR"].ToString().Length > 0)
        //                {
        //                    tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["VNDR_NBR"].ToString()))), font10));
        //                }
        //                else
        //                {
        //                    tblCol2.AddCell(new Phrase(" ", font10));
        //                }

        //                if (row["VENDOR_NM"].ToString().Length > 0)
        //                {
        //                    tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["VENDOR_NM"].ToString()))), font10));
        //                }
        //                else
        //                {
        //                    tblCol2.AddCell(new Phrase(" ", font10));
        //                }

        //                if (row["ISSUED_TO_USR_NM"].ToString().Length > 0)
        //                {
        //                    tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["ISSUED_TO_USR_NM"].ToString()))), font10));
        //                }
        //                else
        //                {
        //                    tblCol2.AddCell(new Phrase(" ", font10));
        //                }

        //                if (row["LOC_COUNTRY_NM"].ToString().Length > 0)
        //                {
        //                    tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["LOC_COUNTRY_NM"].ToString()))), font10));
        //                }
        //                else
        //                {
        //                    tblCol2.AddCell(new Phrase(" ", font10));
        //                }

        //                if (row["LOC_SUPPLIER"].ToString().Length > 0)
        //                {
        //                    tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["LOC_SUPPLIER"].ToString()))), font10));
        //                }
        //                else
        //                {
        //                    tblCol2.AddCell(new Phrase(" ", font10));
        //                }

        //                if (row["ACTION_TAKEN_BY_USR_NM"].ToString().Length > 0)
        //                {
        //                    tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["ACTION_TAKEN_BY_USR_NM"].ToString()))), font10));
        //                }
        //                else
        //                {
        //                    tblCol2.AddCell(new Phrase(" ", font10));
        //                }

        //                if (row["RESPONSE_DT"].ToString().Length > 0)
        //                {
        //                    tblCol2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["RESPONSE_DT"].ToString()))), font10));
        //                }
        //                else
        //                {
        //                    tblCol2.AddCell(new Phrase(" ", font10));
        //                }

        //                PdfPCell nestCol2 = new PdfPCell(tblCol2);
        //                nestCol2.Padding = 0f;
        //                tableInfo.AddCell(nestCol2);

        //                ////*** Col3 Header
        //                //PdfPTable tblCol3 = new PdfPTable(1);
        //                //tblCol3.AddCell(new Phrase("Audit Number", font10Bold));
        //                //tblCol3.AddCell(new Phrase("Q Note Number", font10Bold));
        //                //tblCol3.AddCell(new Phrase("CPI Number", font10Bold));
        //                //tblCol3.AddCell(new Phrase("Material Number", font10Bold));
        //                //tblCol3.AddCell(new Phrase("Purchase Order Number", font10Bold));
        //                //tblCol3.AddCell(new Phrase("Production Order Number", font10Bold));
        //                //tblCol3.AddCell(new Phrase("API Audit Number", font10Bold));
        //                //tblCol3.AddCell(new Phrase("Category", font10Bold));
        //                //tblCol3.AddCell(new Phrase("Responsible Person", font10Bold));

        //                //PdfPCell nestCol3 = new PdfPCell(tblCol3);
        //                //nestCol3.Padding = 0f;
        //                //tableInfo.AddCell(nestCol3);

        //                ////*** Col4
        //                //PdfPTable tblCol4 = new PdfPTable(1);
        //                //tblCol4.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["AUDIT_NBR"].ToString()))), font10));
        //                //tblCol4.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["QNOTE_NBR"].ToString()))), font10));
        //                //tblCol4.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["CPI_NBR"].ToString()))), font10));
        //                //tblCol4.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["MATERIAL_NBR"].ToString()))), font10));
        //                //tblCol4.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["PURCHASE_ORDER_NBR"].ToString()))), font10));
        //                //tblCol4.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["PRODUCTION_ORDER_NBR"].ToString()))), font10));
        //                //tblCol4.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["API_AUDIT_NBR"].ToString()))), font10));
        //                //tblCol4.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["CATEGORY_NM"].ToString()))), font10));
        //                //tblCol4.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["RESP_PERSON_USR_NM"].ToString()))), font10));

        //                //PdfPCell nestCol4 = new PdfPCell(tblCol4);
        //                //nestCol3.Padding = 0f;
        //                //tableInfo.AddCell(nestCol4);

        //                tableInfo.SpacingAfter = 7f;
        //                doc.Add(tableInfo);

        //                //********* Create Description of Finding Header *********//
        //                PdfPTable tableDof = new PdfPTable(1);
        //                tableDof.TotalWidth = 580f;
        //                tableDof.LockedWidth = true;
        //                //tableInfo.SetWidths(Width41);
        //                tableDof.SpacingAfter = 7f;

        //                PdfPCell headerDof = new PdfPCell(new Phrase("Description of Finding", font11Bold));
        //                headerDof.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

        //                tableDof.AddCell(headerDof);

        //                //*** Col Dof
        //                PdfPTable tblCol_Dof = new PdfPTable(1);
        //                tblCol_Dof.AddCell(new Phrase(row["FINDING_DESC"].ToString(), font10));

        //                PdfPCell nestDof = new PdfPCell(tblCol_Dof);
        //                nestDof.Padding = 0f;

        //                tableDof.AddCell(nestDof);

        //                tableDof.SpacingAfter = 7f;
        //                doc.Add(tableDof);


        //                //********* Create Description of Improvement Header *********//
        //                PdfPTable tableDoi = new PdfPTable(1);
        //                tableDoi.TotalWidth = 580f;
        //                tableDoi.LockedWidth = true;
        //                tableDoi.SpacingAfter = 7f;

        //                PdfPCell headerDoi = new PdfPCell(new Phrase("Description of Improvement", font11Bold));
        //                headerDoi.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

        //                tableDoi.AddCell(headerDoi);

        //                //*** Col Dof
        //                PdfPTable tblCol_Doi = new PdfPTable(1);
        //                tblCol_Doi.AddCell(new Phrase(row["DESC_OF_IMPROVEMENT"].ToString(), font10));

        //                PdfPCell nestDoi = new PdfPCell(tblCol_Doi);
        //                nestDoi.Padding = 0f;

        //                tableDoi.AddCell(nestDoi);

        //                tableDoi.SpacingAfter = 7f;
        //                doc.Add(tableDoi);

        //                doc.NewPage();

        //                //********* Question 1  *********//
        //                PdfPTable tableQ1 = new PdfPTable(1);
        //                tableQ1.TotalWidth = 580f;
        //                tableQ1.LockedWidth = true;
        //                tableQ1.SpacingAfter = 7f;

        //                PdfPCell headerQ1 = new PdfPCell(new Phrase("Why did this nonconformance occur?", font11Bold));
        //                headerQ1.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

        //                tableQ1.AddCell(headerQ1);

        //                PdfPTable tblCol_Q1 = new PdfPTable(1);

        //                if(row["NON_CONFORM_RSN"].ToString().Length >0)
        //                {
        //                    tblCol_Q1.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["NON_CONFORM_RSN"].ToString()))), font10));
        //                }
        //                else
        //                {
        //                    tblCol_Q1.AddCell(new Phrase(" ", font10));
        //                }

        //                PdfPCell nestQ1 = new PdfPCell(tblCol_Q1);
        //                nestQ1.Padding = 0f;

        //                tableQ1.AddCell(nestQ1);

        //                tableQ1.SpacingAfter = 7f;
        //                doc.Add(tableQ1);

        //                //********* Question 2  *********//
        //                PdfPTable tableQ2 = new PdfPTable(1);
        //                tableQ2.TotalWidth = 580f;
        //                tableQ2.LockedWidth = true;
        //                tableQ2.SpacingAfter = 7f;

        //                PdfPCell headerQ2 = new PdfPCell(new Phrase("What is the root cause / potential root cause of the non-conformance?", font11Bold));
        //                headerQ2.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

        //                tableQ2.AddCell(headerQ2);

        //                PdfPTable tblCol_Q2 = new PdfPTable(1);

        //                if(row["ROOT_CAUSE"].ToString().Length >0)
        //                {
        //                    tblCol_Q2.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["ROOT_CAUSE"].ToString()))), font10));
        //                }
        //                else
        //                {
        //                    tblCol_Q2.AddCell(new Phrase(" ", font10));
        //                }

        //                PdfPCell nestQ2 = new PdfPCell(tblCol_Q2);
        //                nestQ2.Padding = 0f;

        //                tableQ2.AddCell(nestQ2);

        //                tableQ2.SpacingAfter = 7f;
        //                doc.Add(tableQ2);

        //                //********* Question 3  *********//
        //                PdfPTable tableQ3 = new PdfPTable(1);
        //                tableQ3.TotalWidth = 580f;
        //                tableQ3.LockedWidth = true;
        //                tableQ3.SpacingAfter = 7f;

        //                PdfPCell headerQ3 = new PdfPCell(new Phrase("Are there similar instances of this nonconformance in your area of responsibility? (Y/N)", font11Bold));
        //                headerQ3.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

        //                tableQ3.AddCell(headerQ3);

        //                PdfPTable tblCol_Q3 = new PdfPTable(1);
        //                tblCol_Q3.AddCell(new Phrase(row["SIMILAR_INSTANCE_Y_N_TEXT"].ToString(), font10));

        //                PdfPCell nestQ3 = new PdfPCell(tblCol_Q3);
        //                nestQ3.Padding = 0f;

        //                tableQ3.AddCell(nestQ3);

        //                if (row["SIMILAR_INSTANCE_Y_N_TEXT"].ToString().ToUpper() == "YES")
        //                {
        //                    PdfPTable tblCol_Q3Y = new PdfPTable(1);
        //                    tblCol_Q3Y.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["SIMILAR_INSTANCE"].ToString()))), font10));

        //                    PdfPCell nestQ3Y = new PdfPCell(tblCol_Q3Y);
        //                    nestQ3Y.Padding = 0f;

        //                    tableQ3.AddCell(nestQ3Y);
        //                }
        //                tableQ3.SpacingAfter = 7f;
        //                doc.Add(tableQ3);


        //                //********* Question 4  *********//
        //                PdfPTable tableQ4 = new PdfPTable(1);
        //                tableQ4.TotalWidth = 580f;
        //                tableQ4.LockedWidth = true;
        //                tableQ4.SpacingAfter = 7f;

        //                PdfPCell headerQ4 = new PdfPCell(new Phrase("What action was taken (or is planned) to correct this nonconformance?", font11Bold));
        //                headerQ4.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

        //                tableQ4.AddCell(headerQ4);

        //                PdfPTable tblCol_Q4 = new PdfPTable(1);

        //                if(row["CORR_ACTION_TAKEN"].ToString().Length>0)
        //                {
        //                    tblCol_Q4.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["CORR_ACTION_TAKEN"].ToString()))), font10));
        //                }
        //                else
        //                {
        //                    tblCol_Q4.AddCell(new Phrase(" ", font10));
        //                }

        //                PdfPCell nestQ4 = new PdfPCell(tblCol_Q4);
        //                nestQ4.Padding = 0f;

        //                tableQ4.AddCell(nestQ4);

        //                tableQ4.SpacingAfter = 7f;
        //                doc.Add(tableQ4);

        //                PdfPTable tableQ4D = new PdfPTable(1);
        //                tableQ4D.TotalWidth = 580f;
        //                tableQ4D.LockedWidth = true;
        //                tableQ4D.SpacingAfter = 7f;

        //                PdfPCell headerQ4D = new PdfPCell(new Phrase("What is the scheduled implementation date?", font11Bold));
        //                headerQ4D.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

        //                tableQ4D.AddCell(headerQ4D);

        //                PdfPTable tblCol_Q4D = new PdfPTable(1);

        //                if(row["CORR_ACTION_TAKEN_DT"].ToString().Length>0)
        //                {
        //                    tblCol_Q4D.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["CORR_ACTION_TAKEN_DT"].ToString()))), font10));
        //                }
        //                else
        //                {
        //                    tblCol_Q4D.AddCell(new Phrase(" ", font10));
        //                }

        //                PdfPCell nestQ4D = new PdfPCell(tblCol_Q4D);
        //                nestQ4D.Padding = 0f;

        //                tableQ4D.AddCell(nestQ4D);

        //                tableQ4D.SpacingAfter = 7f;
        //                doc.Add(tableQ4D);

        //                //********* Question 5  *********//
        //                PdfPTable tableQ5 = new PdfPTable(1);
        //                tableQ5.TotalWidth = 580f;
        //                tableQ5.LockedWidth = true;
        //                tableQ5.SpacingAfter = 7f;

        //                PdfPCell headerQ5 = new PdfPCell(new Phrase("What action was taken (or is planned) to preclude this and similar non-conformances?", font11Bold));
        //                headerQ5.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

        //                tableQ5.AddCell(headerQ5);

        //                PdfPTable tblCol_Q5 = new PdfPTable(1);

        //                if(row["PRECLUDE_ACTION"].ToString().Length>0)
        //                {
        //                    tblCol_Q5.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["PRECLUDE_ACTION"].ToString()))), font10));
        //                }
        //                else
        //                {
        //                    tblCol_Q5.AddCell(new Phrase(" ", font10));
        //                }

        //                PdfPCell nestQ5 = new PdfPCell(tblCol_Q5);
        //                nestQ5.Padding = 0f;

        //                tableQ5.AddCell(nestQ5);

        //                tableQ5.SpacingAfter = 7f;
        //                doc.Add(tableQ5);

        //                PdfPTable tableQ5D = new PdfPTable(1);
        //                tableQ5D.TotalWidth = 580f;
        //                tableQ5D.LockedWidth = true;
        //                tableQ5D.SpacingAfter = 7f;

        //                PdfPCell headerQ5D = new PdfPCell(new Phrase("What is the scheduled implementation date? ", font11Bold));
        //                headerQ5D.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

        //                tableQ5D.AddCell(headerQ5D);

        //                PdfPTable tblCol_Q5D = new PdfPTable(1);

        //                if(row["PRECLUDE_ACTION_DT"].ToString().Length>0)
        //                {
        //                    tblCol_Q5D.AddCell(new Phrase(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["PRECLUDE_ACTION_DT"].ToString()))), font10));
        //                }
        //                else
        //                {
        //                    tblCol_Q5D.AddCell(new Phrase(" ", font10));
        //                }

        //                PdfPCell nestQ5D = new PdfPCell(tblCol_Q5D);
        //                nestQ5D.Padding = 0f;

        //                tableQ5D.AddCell(nestQ5D);

        //                tableQ5D.SpacingAfter = 7f;
        //                doc.Add(tableQ5D);

        //                doc.NewPage();

        //                //********* Create Originator Header *********//
        //                PdfPTable tableOrig = new PdfPTable(2);
        //                tableOrig.TotalWidth = 580f;
        //                tableOrig.LockedWidth = true;
        //                tableOrig.SetWidths(Width21);
        //                tableOrig.SpacingAfter = 7f;

        //                PdfPCell headerOrig = new PdfPCell(new Phrase("Originator Use Only", font11Bold));
        //                headerOrig.BackgroundColor = iTextSharp.text.BaseColor.BLACK;

        //                headerOrig.Colspan = 2;
        //                tableOrig.AddCell(headerOrig);

        //                //*** Col1 Header
        //                PdfPTable tblCol = new PdfPTable(1);
        //                tblCol.AddCell(new Phrase("Due Date Extension", font10Bold));
        //                tblCol.AddCell(new Phrase("Re-Issued To", font10Bold));
        //                tblCol.AddCell(new Phrase("Date Re-Issued", font10Bold));
        //                tblCol.AddCell(new Phrase("Follow-up Required", font10Bold));
        //                tblCol.AddCell(new Phrase("Date to Follow-up", font10Bold));
        //                tblCol.AddCell(new Phrase("Date Recieved", font10Bold));
        //                tblCol.AddCell(new Phrase("Date Verified", font10Bold));
        //                tblCol.AddCell(new Phrase("Verified By", font10Bold));
        //                tblCol.AddCell(new Phrase("Response Accepted By", font10Bold));
        //                tblCol.AddCell(new Phrase("Status", font10Bold));
        //                tblCol.AddCell(new Phrase("Close Date", font10Bold));
        //                tblCol.AddCell(new Phrase("How was effectiveness validated?", font10Bold));

        //                PdfPCell nestCol = new PdfPCell(tblCol);
        //                nestCol.Padding = 0f;
        //                tableOrig.AddCell(nestCol);

        //                //*** Col2 Data
        //                PdfPTable tblCol_2 = new PdfPTable(1);

        //                if(row["DUE_DT_EXT"].ToString().Length >0)
        //                {
        //                    tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["DUE_DT_EXT"].ToString())))), font10));
        //                }
        //                else
        //                {
        //                    tblCol_2.AddCell(new Phrase(" ", font10));
        //                }

        //                if (row["REISSUED_TO_USR_NM"].ToString().Length > 0)
        //                {
        //                    tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["REISSUED_TO_USR_NM"].ToString())))), font10));
        //                }
        //                else
        //                {
        //                    tblCol_2.AddCell(new Phrase(" ", font10));
        //                }

        //                if (row["REISSUED_DT_TEXT"].ToString().Length > 0)
        //                {
        //                    tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["REISSUED_DT_TEXT"].ToString())))), font10));
        //                }
        //                else
        //                {
        //                    tblCol_2.AddCell(new Phrase(" ", font10));
        //                }

        //                if (row["FOLLOW_UP_REQD_TEXT"].ToString().Length > 0)
        //                {
        //                    tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["FOLLOW_UP_REQD_TEXT"].ToString())))), font10));
        //                }
        //                else
        //                {
        //                    tblCol_2.AddCell(new Phrase(" ", font10));
        //                }

        //                if (row["FOLLOW_UP_DT"].ToString().Length > 0)
        //                {
        //                    tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["FOLLOW_UP_DT"].ToString())))), font10));
        //                }
        //                else
        //                {
        //                    tblCol_2.AddCell(new Phrase(" ", font10));
        //                }

        //                if (row["RECEIVED_DT"].ToString().Length > 0)
        //                {
        //                    tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["RECEIVED_DT"].ToString())))), font10));
        //                }
        //                else
        //                {
        //                    tblCol_2.AddCell(new Phrase(" ", font10));
        //                }

        //                if (row["VERIFY_DT"].ToString().Length > 0)
        //                {
        //                    tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["VERIFY_DT"].ToString())))), font10));
        //                }
        //                else
        //                {
        //                    tblCol_2.AddCell(new Phrase(" ", font10));
        //                }

        //                if (row["VERIFIED_BY_USR_NM"].ToString().Length > 0)
        //                {
        //                    tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["VERIFIED_BY_USR_NM"].ToString())))), font10));
        //                }
        //                else
        //                {
        //                    tblCol_2.AddCell(new Phrase(" ", font10));
        //                }

        //                if (row["RESPONSE_ACCEPT_BY_USR_NM"].ToString().Length > 0)
        //                {
        //                    tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["RESPONSE_ACCEPT_BY_USR_NM"].ToString())))), font10));
        //                }
        //                else
        //                {
        //                    tblCol_2.AddCell(new Phrase(" ", font10));
        //                }

        //                if (row["CAR_STATUS_NM"].ToString().Length > 0)
        //                {
        //                    tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["CAR_STATUS_NM"].ToString())))), font10));
        //                }
        //                else
        //                {
        //                    tblCol_2.AddCell(new Phrase(" ", font10));
        //                }

        //                if (row["CLOSE_DT"].ToString().Length > 0)
        //                {
        //                    tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["CLOSE_DT"].ToString())))), font10));
        //                }
        //                else
        //                {
        //                    tblCol_2.AddCell(new Phrase(" ", font10));
        //                }

        //                if (row["REMARKS"].ToString().Length > 0)
        //                {
        //                    tblCol_2.AddCell(new Phrase(removeChar.SanitizeField(removeChar.SanitizeQuoteString(Server.HtmlDecode(htmlUtil.SanitizeHtml(row["REMARKS"].ToString())))), font10));
        //                }
        //                else
        //                {
        //                    tblCol_2.AddCell(new Phrase(" ", font10));
        //                }

        //                PdfPCell nestCol_2 = new PdfPCell(tblCol_2);
        //                nestCol_2.Padding = 0f;
        //                tableOrig.AddCell(nestCol_2);

        //                doc.Add(tableOrig);

        //                doc.Close();

        //                Response.AppendHeader("Content-Disposition", Convert.ToString("attachment; filename=") + fileName);
        //                Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //                Response.ContentType = "application/pdf";
        //                Response.OutputStream.Write(myMemoryStream.GetBuffer(), 0, myMemoryStream.GetBuffer().Length);
        //                Response.Flush();
        //                Response.End();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.GetType().Name.ToString() != "ThreadAbortException")
        //        {
        //            LogException(ex);
        //            throw new Exception("Error Report To PDF..." + ex.Message);
        //        }
        //    }
        //}

        private void LogException(Exception ex)
        {
            Applog appLog;
            appLog = new Applog();

            appLog.ExceptionMsg = ex.Message.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionSource = "View.aspx";
            appLog.ExceptionType = ex.GetType().Name.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionUrl = MySession.Current.SessionHostName.ToString().ToUpper();
            appLog.ExceptionTargetSite = ex.TargetSite.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.ExceptionStackTrace = ex.StackTrace.ToString().Replace(oldValue: "'", newValue: "`");
            appLog.AppLogEvent();
        }
    }
}