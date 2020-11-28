using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing;
using System.Drawing.Design;


namespace LoginPage
{
    class PdfCreator
    {
        public PdfCreator(string filename, string nameapplicant, string applicantEmail, string staffMember, string staffMemberEmail, List<string> comments, List<string> sections)
        {
            Document mydocu = new Document(PageSize.A4.Rotate());

            try
            {
                #region Common Part
                PdfPTable pdfTableBlank = new PdfPTable(1);
                PdfPTable pdfTableBlank1 = new PdfPTable(1);

                //footer 
                PdfPTable pdfTableFooter = new PdfPTable(1);
                pdfTableFooter.DefaultCell.BorderWidth = 0;
                pdfTableFooter.WidthPercentage = 100;
                pdfTableFooter.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                Chunk cnkFooter = new Chunk("Application feedback", FontFactory.GetFont("Garamond"));

                cnkFooter.Font.Size = 30;
                pdfTableFooter.AddCell(new Phrase(cnkFooter));
                //End of footer section
                BaseFont bfi = BaseFont.CreateFont(
                        BaseFont.TIMES_ROMAN,
                        BaseFont.CP1252,
                        BaseFont.EMBEDDED);
                iTextSharp.text.Font fonti = new iTextSharp.text.Font(bfi, 12);


                pdfTableBlank.AddCell(new Phrase("Reviewer details", fonti));
                pdfTableBlank.DefaultCell.Border = 0;



                pdfTableBlank1.AddCell(new Phrase("Applicant details", fonti));
                pdfTableBlank1.DefaultCell.Border = 0;
                #endregion

                #region Paage
                #region Section-1 <Header Form>

                PdfPTable pdfTable2 = new PdfPTable(2);
                PdfPTable pdfTable2prim = new PdfPTable(2);

                PdfPTable pdfTable3 = new PdfPTable(2);
                PdfPTable pdfTable3prim = new PdfPTable(2);

                //Font Style
                System.Drawing.Font fontH1 = new System.Drawing.Font("Calibri", 12);

                //pdfTable1.DefaultCell.Padding = 5;


                pdfTable2.DefaultCell.Padding = 5;
                pdfTable2.WidthPercentage = 80;
                pdfTable2.DefaultCell.BorderWidth = 0;


                pdfTable2prim.DefaultCell.Padding = 5;
                pdfTable2prim.WidthPercentage = 80;
                pdfTable2prim.DefaultCell.BorderWidth = 0;


                pdfTable3.DefaultCell.Padding = 5;
                pdfTable3.WidthPercentage = 80;
                pdfTable3.DefaultCell.BorderWidth = 0;

                pdfTable3prim.DefaultCell.Padding = 5;
                pdfTable3prim.WidthPercentage = 80;
                pdfTable3prim.DefaultCell.BorderWidth = 0;


                Chunk c2 = new Chunk("Name of the applicant", FontFactory.GetFont("Calibri"));
                c2.Font.Color = new iTextSharp.text.BaseColor(0, 0, 0);
                c2.Font.SetStyle(0); //0 For normal font
                c2.Font.Size = 9;
                Phrase p2 = new Phrase();
                p2.Add(c2);
                pdfTable2.AddCell(p2);


                Chunk c2bis = new Chunk(nameapplicant, FontFactory.GetFont("Calibri"));
                c2bis.Font.Color = new iTextSharp.text.BaseColor(0, 0, 200);
                c2bis.Font.SetStyle(0); //0 For normal font
                c2bis.Font.Size = 9;
                Phrase p2bis = new Phrase();
                p2bis.Add(c2bis);
                pdfTable2.AddCell(p2bis);

                Chunk c2prim = new Chunk("E-mail of the applicant", FontFactory.GetFont("Calibri"));
                c2prim.Font.Color = new iTextSharp.text.BaseColor(0, 0, 0);
                c2prim.Font.SetStyle(0); //0 For normal font
                c2prim.Font.Size = 9;
                Phrase p2prim = new Phrase();
                p2prim.Add(c2prim);
                pdfTable2prim.AddCell(p2prim);


                Chunk c2primbis = new Chunk(applicantEmail, FontFactory.GetFont("Calibri"));
                c2primbis.Font.Color = new iTextSharp.text.BaseColor(0, 0, 200);
                c2primbis.Font.SetStyle(0); //0 For normal font
                c2primbis.Font.Size = 9;
                Phrase p2primbis = new Phrase();
                p2primbis.Add(c2primbis);
                pdfTable2prim.AddCell(p2primbis);

                Chunk c3 = new Chunk("Review written by ", FontFactory.GetFont("Calibri"));
                c3.Font.Color = new iTextSharp.text.BaseColor(0, 0, 0);
                c3.Font.SetStyle(0); //0 For normal font
                c3.Font.Size = 9;
                Phrase p3 = new Phrase();
                p3.Add(c3);
                pdfTable3.AddCell(p3);

                Chunk c4 = new Chunk(staffMember, FontFactory.GetFont("Calibri"));
                c4.Font.Color = new iTextSharp.text.BaseColor(0, 0, 200);
                c4.Font.SetStyle(0); //0 For normal font
                c4.Font.Size = 9;
                Phrase p4 = new Phrase();
                p4.Add(c4);
                pdfTable3.AddCell(p4);

                Chunk c3prim = new Chunk("E-mail of " + staffMember, FontFactory.GetFont("Calibri"));
                c3prim.Font.Color = new iTextSharp.text.BaseColor(0, 0, 0);
                c3prim.Font.SetStyle(0); //0 For normal font
                c3prim.Font.Size = 9;
                Phrase p3prim = new Phrase();
                p3prim.Add(c3prim);
                pdfTable3prim.AddCell(p3prim);

                Chunk c4prim = new Chunk(staffMemberEmail, FontFactory.GetFont("Calibri"));
                c4prim.Font.Color = new iTextSharp.text.BaseColor(0, 0, 200);
                c4prim.Font.SetStyle(0); //0 For normal font
                c4prim.Font.Size = 9;
                Phrase p4prim = new Phrase();
                p4.Add(c4);
                pdfTable3prim.AddCell(p4);


                #endregion


                #endregion
                string cfname = (filename + ".pdf");
                PdfWriter.GetInstance(mydocu, new FileStream(cfname, FileMode.Create));
                mydocu.Open();

                mydocu.Add(pdfTableBlank1);
                mydocu.Add(pdfTable2);
                mydocu.Add(pdfTable2prim);
                mydocu.Add(pdfTableBlank);

                mydocu.Add(pdfTable3);
                mydocu.Add(pdfTable3prim);

                mydocu.Add(new iTextSharp.text.Paragraph("  "));
                mydocu.Add(new iTextSharp.text.Paragraph("  "));

                mydocu.Add(pdfTableFooter);
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 15, iTextSharp.text.Font.NORMAL);
                PdfCreator.commentsections(mydocu, comments, sections);
                // mydocu.Add(new iTextSharp.text.Paragraph(pargr,font));

            }
            catch (DocumentException de)
            {
                Console.Error.WriteLine(de.Message);
            }

            catch (IOException ioe)
            {
                Console.Error.WriteLine(ioe.Message);
            }

            mydocu.Close();
        }

        public static Document commentsections(Document mydoc, List<string> comments, List<string> sections)
        {
            for (int i = 0; i < sections.Count; i++)
            {
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 15, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font fontbold = new iTextSharp.text.Font(bf, 15, iTextSharp.text.Font.BOLD);
                mydoc.Add(new iTextSharp.text.Paragraph("  ", font));
                Paragraph sectionpar = new Paragraph(sections[i], FontFactory.GetFont("Garamond", 20, iTextSharp.text.Font.BOLD));
                sectionpar.Font.Color = new iTextSharp.text.BaseColor(00, 168, 243);
                Paragraph commentpar = new Paragraph(comments[i], FontFactory.GetFont("Arial", 15, iTextSharp.text.Font.NORMAL));
                sectionpar.Alignment = Element.ALIGN_CENTER;


                commentpar.Alignment = Element.ALIGN_CENTER;

                mydoc.Add(new iTextSharp.text.Paragraph(sectionpar));
                mydoc.Add(new iTextSharp.text.Paragraph(commentpar));
            }

            return mydoc;
        }
    }
}
