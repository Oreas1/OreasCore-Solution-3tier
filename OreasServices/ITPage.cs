using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OreasServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OreasModel
{
    public class ITPage
    {
  

        private PdfWriter _pdfWriter;
        public PdfDocument _doc;
        public Document _document;

        public PdfFont _font;

        private PageSize _pagesize;

        private Table _pdfpTable;
        private MemoryStream _memoryStream;

        private bool IsHeader;
        private bool IsFooter;
        private bool IsPortrait;

        public ITPage()
        { 

        }
        public ITPage(PageSize pagesize, float marginLeft, float marginRight, float marginTop, float marginBottom, string reportName, bool _IsPortrait, bool _IsHeader = true, bool _IsFooter = true, bool _IsLetterHead = false)
        {
            _memoryStream = new MemoryStream();
            _pdfWriter = new PdfWriter(_memoryStream);
            _doc = new PdfDocument(_pdfWriter);
            _pagesize = pagesize;
            IsHeader = _IsHeader;
            IsFooter = _IsFooter;
            IsPortrait = _IsPortrait;

            if (IsHeader)
                marginTop = marginTop + 35f;
            if (IsFooter)
                marginBottom = marginBottom - 15f;

            if (_IsLetterHead)
            {
                IsHeader = false;
                IsFooter = false;
                IsPortrait = true;
                marginTop = (float)Rpt_Shared.LetterHead_HeaderHeight;
                marginBottom = (float)Rpt_Shared.LetterHead_FooterHeight;
                if (Rpt_Shared.LetterHead_PaperSize == "Letter")
                    pagesize = PageSize.LETTER;
                else if (Rpt_Shared.LetterHead_PaperSize == "A4")
                    pagesize = PageSize.A4;
            }

            _document = new Document(_doc, pagesize, false);

            _document.SetMargins(marginTop, marginRight, marginBottom, marginLeft);
          
           

            _font = PdfFontFactory.CreateFont(StandardFonts.COURIER);
            try
            {
                string fullFilePath = new System.Uri(Assembly.GetExecutingAssembly().CodeBase).ToString();
                fullFilePath = fullFilePath.Split(new string[] { "/OreasServices.dll" }, StringSplitOptions.None)[0];
                var filePath = fullFilePath + @"/Font/verdana.ttf";



                _font = PdfFontFactory.CreateFont(filePath);
            }
            catch (Exception)
            {

            }
            finally
            {
                _document.SetFont(_font);
            }


            if (IsPortrait)
            {
                _doc.SetDefaultPageSize(pagesize);
            }
            else
            {
                _doc.SetDefaultPageSize(pagesize.Rotate());
            }

            if (!string.IsNullOrEmpty(reportName))
                _document.Add(new Cell().Add(new Paragraph().Add(reportName)).SetBorder(Border.NO_BORDER).SetFontSize(10).SetBold().SetUnderline().SetTextAlignment(TextAlignment.CENTER));

            _pdfpTable = new Table(1);
            _pdfpTable.SetBorder(Border.NO_BORDER);

            _pdfpTable.SetWidth(UnitValue.CreatePercentValue(100));



        }

    

        public void InsertContent(IBlockElement _pdfpCell)
        {
            _document.Add(_pdfpCell);
        }

        public void InsertNewPage()
        {
            _document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));            
        }
   
        public Table GetHearderTable() 
        {            
            Table h = new Table(new float[] {
                        (float)(_pagesize.GetWidth() * 0.15), //logo
                        (float)(_pagesize.GetWidth() * 0.85), //detail
                }
                ).SetFixedLayout().SetBorder(Border.NO_BORDER);


            Cell cell;

            Image img = new Image(ImageDataFactory.Create(Rpt_Shared.LicenseToLogo ?? new byte[] { }));
            img.ScaleAbsolute(35, 35);

            cell = new Cell().Add(new Paragraph().Add(img).SetTextAlignment(TextAlignment.CENTER)).SetBorder(Border.NO_BORDER);
            h.AddCell(cell);

            cell = new Cell()
                .Add(new Paragraph(Rpt_Shared.LicenseTo).SetFontSize(12))
                .Add(new Paragraph().Add(Rpt_Shared.LicenseToAddress).Add(Rpt_Shared.LicenseToContactNo).SetFontSize(8))
                .SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.BOTTOM);

            h.AddCell(cell);

            return h;
        }

        public byte[] FinishToGetBytes()
        {

            //-------------------------------setting header-----------------------------------------------//
            if (IsHeader)
            {
                float[] columnWidths = { 2, 8 };
                Table h = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth().SetFixedLayout();

                h.SetWidth(_doc.GetDefaultPageSize().GetWidth() - 10);
                Cell cell;



                Image img = new Image(ImageDataFactory.Create(Rpt_Shared.LicenseToLogo ?? new byte[] { }));
                img.ScaleAbsolute(35, 35);

                cell = new Cell().Add(new Paragraph().Add(img).SetTextAlignment(TextAlignment.CENTER)).SetBorderBottom(new SolidBorder(0.5f)).SetBorder(Border.NO_BORDER);
                h.AddCell(cell);

                cell = new Cell()
                    .Add(new Paragraph(Rpt_Shared.LicenseTo).SetFontSize(12))
                    .Add(new Paragraph().Add(Rpt_Shared.LicenseToAddress).Add(Rpt_Shared.LicenseToContactNo).SetFontSize(8))
                    .SetBorderBottom(new SolidBorder(0.5f)).SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.BOTTOM);

                h.AddCell(cell);

                Paragraph header = new Paragraph().Add(h);
                int n = _doc.GetNumberOfPages();
                for (int i = 1; i <= n; i++)
                {
                    var a = _doc.GetPage(i);

                    Rectangle pageSize = _doc.GetPage(i).GetPageSize();
                    _document.ShowTextAligned(header, 5, pageSize.GetTop() - 45, i, TextAlignment.LEFT, VerticalAlignment.BOTTOM, 0);
                    _document.ShowTextAligned(new Paragraph(String.Format("Page " + i + " of " + n)), pageSize.GetWidth() - 10, pageSize.GetTop() - 28, i, TextAlignment.RIGHT, VerticalAlignment.TOP, 0).SetFontSize(8);
                }


            }

            //-------------------------------setting footer-----------------------------------------------//
            if (IsFooter)
            { 
                float[] columnWidths = { 2, 8 };
                Table t = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth().SetFixedLayout();

                t.SetWidth(_doc.GetDefaultPageSize().GetWidth() - 10);

                Image img = new Image(ImageDataFactory.Create(Rpt_Shared.LicenseByLogo ?? new byte[] { }));
                img.ScaleAbsolute(10, 10);

                Cell cell = new Cell(1, 2).Add(
                    new Paragraph().Add(img).Add(" " + Rpt_Shared.LicenseBy + " " + Rpt_Shared.LicenseByAddress + " " + Rpt_Shared.LicenseByCellNo).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER)
                    )
                    .SetBorderTop(new SolidBorder(0.2f)).SetBorder(Border.NO_BORDER);

                t.AddCell(cell);
   

                Paragraph footer = new Paragraph().Add(t);

                for (int i = 1; i <= _doc.GetNumberOfPages(); i++)
                {
                    Rectangle pageSize = _doc.GetPage(i).GetPageSize();
                    float x = 5;//pageSize.GetWidth() / 2;
                    float y = pageSize.GetBottom();
                    _document.ShowTextAligned(footer, x, y, i, TextAlignment.LEFT, VerticalAlignment.BOTTOM, 0);
                }

            }




            _document.Close();

            return _memoryStream.ToArray();

        }
    }

    //--------------for standard color scheme--------------//
    public enum MyColor
    {
        White = 0, SteelBlue = 1, LightSteelBlue = 2, Gray = 3, LightGray = 4, Pink = 5, LightPink = 6, Maroon = 7, LightGreen = 8
    }
    public class MyDeviceRgb
    {
        public DeviceRgb color;
        public MyDeviceRgb(MyColor c)
        {
            if (c == MyColor.SteelBlue)
            {
                color = new DeviceRgb(System.Drawing.Color.SteelBlue);
            }
            else if (c == MyColor.LightSteelBlue)
            {
                color = new DeviceRgb(System.Drawing.Color.LightSteelBlue);
            }
            else if (c == MyColor.Gray)
            {
                color = new DeviceRgb(System.Drawing.Color.Gray);
            }
            else if (c == MyColor.LightGray)
            {
                color = new DeviceRgb(System.Drawing.Color.LightGray);
            }
            else if (c == MyColor.Pink)
            {
                color = new DeviceRgb(System.Drawing.Color.Pink);
            }
            else if (c == MyColor.LightPink)
            {
                color = new DeviceRgb(System.Drawing.Color.LightPink);
            }
            else if (c == MyColor.Maroon)
            {
                color = new DeviceRgb(System.Drawing.Color.Maroon);
            }
            else if (c == MyColor.LightGreen)
            {
                color = new DeviceRgb(System.Drawing.Color.LightGreen);
            }
            else
            {
                color = new DeviceRgb(System.Drawing.Color.White);
            }
            
        }
    }

    //------------convert amount into words----------------//
    public static class AmountIntoWords
    {
        private static readonly string[] Ones = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
        private static readonly string[] Teens = { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        private static readonly string[] Tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        private static readonly string[] Thousands = { "", "Thousand", "Million", "Billion", "Trillion" };

        public static string ConvertToWords(long number)
        {
            if (number == 0)
                return "Zero";

            string words = "";

            for (int i = 0; number > 0; i++)
            {
                if (number % 1000 != 0)
                    words = ConvertHundreds(number % 1000) + Thousands[i] + " " + words;

                number /= 1000;
            }

            return words.Trim();
        }

        private static string ConvertHundreds(long number)
        {
            string words = "";

            if (number >= 100)
            {
                words += Ones[number / 100] + " Hundred ";
                number %= 100;
            }

            if (number >= 10 && number <= 19)
            {
                words += Teens[number % 10] + " ";
            }
            else
            {
                words += Tens[number / 10] + " ";
                words += Ones[number % 10] + " ";
            }

            return words;
        }

        public static double ExecuteMathExpression(string expression)
        {
            DataTable table = new DataTable();

            // Compute the expression
            try
            {
                object result = table.Compute(expression, "");
                if (result is IConvertible)
                {
                    return Math.Round(Convert.ToDouble(result),4);
                    
                }
                else
                {
                    return 0;
                }
            }
            catch 
            {
                return 0;
            }

        }
    }
}
