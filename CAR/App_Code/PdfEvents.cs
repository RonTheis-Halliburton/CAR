using iTextSharp.text;
using iTextSharp.text.pdf;

public class PdfEvents : PdfPageEventHelper
{

    // write on top of document
    public override void OnOpenDocument(PdfWriter writer, Document document)
    {
        base.OnOpenDocument(writer, document);

    }

    // write on start of each page
    public override void OnStartPage(PdfWriter writer, Document document)
    {
        base.OnStartPage(writer, document);
    }

    // write on end of each page
    public override void OnEndPage(PdfWriter writer, Document document)
    {

        base.OnEndPage(writer, document);

        //PdfPTable tblFooter = new PdfPTable(new float[] { 100F });
        //Font font12_Bold = FontFactory.GetFont("ARIAL", 12, 1, BaseColor.WHITE);
        //tblFooter.TotalWidth = 615F;

        //PdfPCell cell;
        //cell = new PdfPCell(new Phrase("Generated at " + DateTime.Now.ToString(), font12_Bold));
        //cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //cell.BackgroundColor = BaseColor.GRAY;
        //cell.Border = 0;
        //tblFooter.AddCell(cell);

        ////tblFooter.WriteSelectedRows(0, 10, 150, document.Bottom, writer.DirectContent);
        ////tblFooter.WriteSelectedRows(0, -1, 0, 15, writer.DirectContent);
        ////tblFooter.WriteSelectedRows(0, 1, 0, 17, writer.DirectContent);

        //tblFooter.WriteSelectedRows(0, -1, 0, tblFooter.TotalHeight, writer.DirectContent);


    }

    //write on close of document
    public override void OnCloseDocument(PdfWriter writer, Document document)
    {
        base.OnCloseDocument(writer, document);

    }
}