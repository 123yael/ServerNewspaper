﻿using AutoMapper;
using DAL.Actions.Interfaces;
using DAL.Models;
using DTO.Repository;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection.PortableExecutable;
using PdfSharp.Pdf;
using System.Diagnostics;
using PdfSharp;
using Azure;
using PdfSharp.Charting;
using System.Drawing.Printing;
using System.Text.Encodings.Web;
using PdfSharp.Drawing.Layout;
using static System.Net.Mime.MediaTypeNames;
using DAL.Actions.Classes;
using System.Data.SqlTypes;
using System.Reflection.Metadata;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using Document = DocumentFormat.OpenXml.Wordprocessing.Document;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using PageSize = PdfSharp.PageSize;
using DocumentFormat.OpenXml;
using GemBox.Document;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using SkiaSharp;

// עבור תמונה
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Vml;
using SautinSoft;

namespace BLL.functions
{
    public class Funcs : IFuncs
    {
        static IMapper _Mapper;

        private string myPath = "C:\\Users\\YAEL\\OneDrive\\שולחן העבודה\\temps";

        static Funcs()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DTO.AutoMap>();
            });
            _Mapper = config.CreateMapper();
        }

        IAdSizeActions _adSize;
        IOrderDetailActions _ordersDetailActions;
        IDatesForOrderDetailActions _datesForOrderDetailActions;
        IAdPlacementActions _adPlacement;
        IWordAdSubCategoryActions _wpAdSubCategoryActions;
        ICustomerActions _customerActions;
        IOrderActions _order;
        INewspapersPublishedActions _newspapersPublished;
        private object codeLines;

        public Funcs(IAdSizeActions adSize,
            IOrderDetailActions ordersDetailActions,
            IDatesForOrderDetailActions datesForOrderDetailActions,
            IAdPlacementActions adPlacement,
            IWordAdSubCategoryActions wpAdSubCategoryActions,
            ICustomerActions customerActions,
            IOrderActions order,
            INewspapersPublishedActions newspapersPublished)
        {
            _adSize = adSize;
            _ordersDetailActions = ordersDetailActions;
            _datesForOrderDetailActions = datesForOrderDetailActions;
            _adPlacement = adPlacement;
            _wpAdSubCategoryActions = wpAdSubCategoryActions;
            _customerActions = customerActions;
            _order = order;
            _newspapersPublished = newspapersPublished;
        }

        #region WpAdSubCategory

        // פונקציה שמחזירה את כל תתי הקטגוריות של מודעות מילים בפורמט DTO
        public List<WordAdSubCategoryDTO> GetAllWordAdSubCategories()
        {
            var wordAdSubCategoryDTO = new List<WordAdSubCategoryDTO>();
            List<WordAdSubCategory> listwordAdSubCategories = _wpAdSubCategoryActions.GetAllWordAdSubCategories();
            foreach (var wordAdSubCategory in listwordAdSubCategories)
                wordAdSubCategoryDTO.Add(_Mapper.Map<WordAdSubCategory, WordAdSubCategoryDTO>(wordAdSubCategory));
            return wordAdSubCategoryDTO;
        }
        #endregion

        #region AdSize

        // פונקציה שמחזירה מערך של כל גדלי הפרסומות בפורמט של DTO
        public List<AdSizeDTO> GetAllAdSize()
        {
            var adSizeDTO = new List<AdSizeDTO>();
            List<AdSize> listAdSize = _adSize.GetAllAdSizes();
            foreach (var adSize in listAdSize)
                adSizeDTO.Add(_Mapper.Map<AdSize, AdSizeDTO>(adSize));
            return adSizeDTO;
        }

        #endregion

        #region AdPlacement

        // פונקציה שמחזירה את כל מיקומי הפרסומות בפורמט של DTO
        public List<AdPlacementDTO> GetAllAdPlacement()
        {
            var adPlacementDTO = new List<AdPlacementDTO>();
            List<AdPlacement> listAdPlacement = _adPlacement.GetAllAdPlacements();
            foreach (var adPlacement in listAdPlacement)
                adPlacementDTO.Add(_Mapper.Map<AdPlacement, AdPlacementDTO>(adPlacement));
            return adPlacementDTO;
        }

        
        #endregion

        #region Customer

        // פונקציה שמקבלת לקוח ומחזירה את הקוד שלו
        // ואם הוא לא קיים היא מוסיפה אותו ומחזירה את הקוד שלו
        public int GetIdByCustomer(CustomerDTO customer)
        {
            CustomerDTO newCust = GetAllCustomers().Where(x => x.CustEmail.Equals(customer.CustEmail)).FirstOrDefault(c => c.CustPassword.Equals(customer.CustPassword));
            if (newCust != null)
                return newCust.CustId;
            Customer custToAdd = _Mapper.Map<CustomerDTO, Customer>(customer);
            _customerActions.AddNewCustomer(custToAdd);
            return custToAdd.CustId;
        }

        // פונקציה שמחזירה את כל הלקוחות
        private List<CustomerDTO> GetAllCustomers()
        {
            List<CustomerDTO> customers = new List<CustomerDTO>();
            List<Customer> customers2 = _customerActions.GetAllCustomers();
            foreach (Customer cust in customers2)
                customers.Add(_Mapper.Map<Customer, CustomerDTO>(cust));
            return customers;
        }

        // פונקציה שמקבלת מייל וסיסמה של לקוח
        // ומזירה אותו אם הוא קיים וכלום אם לא קיים
        public CustomerDTO GetCustomerByEmailAndPass(string email, string pass)
        {
            var newCust = GetAllCustomers().FirstOrDefault(x => x.CustEmail.Equals(email) && x.CustPassword.Equals(pass));
            if (newCust != null)
                return newCust;
            return null!;
        }

        #endregion

        #region NewspapersPublished

        public List<NewspapersPublishedDTO> GetAllNewspapersPublished()
        {
            List<NewspapersPublished> NewspapersPublished = _newspapersPublished.GetAllNewspapersPublished();
            List<NewspapersPublishedDTO> NewspapersPublishedDTO = new List<NewspapersPublishedDTO>();
            foreach (NewspapersPublished np in NewspapersPublished)
                NewspapersPublishedDTO.Add(_Mapper.Map<NewspapersPublished, NewspapersPublishedDTO>(np));
            return NewspapersPublishedDTO;
        }

        #endregion

        // פונקציה שמוסיפה תמונה לקובץ pdf
        public void AddAdFileToPdf()
        {
            // שורה נחוצה עבור הרצת הקוד הבא
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // שלום עולם
            // Create a new PDF document -- יצירת קובץ חדש
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Created with PDFsharp";
            // Create an empty page - יצירת עמוד ריק חדש
            PdfPage page = document.AddPage();
            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);
            // Create a font - יצירת פונט חדש
            XFont font = new XFont("Snap ITC", 20, XFontStyle.BoldItalic);
            // Draw the text
            gfx.DrawString("Hello, World!", font, XBrushes.Black,
            new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            // Save the document...
            const string filename = "C:\\Users\\שירה בוריה\\Desktop\\Output.pdf";
            document.Save(filename);
            // ...and start a viewer.
            //Process.Start(new ProcessStartInfo { FileName = filename, UseShellExecute = true });
            //Process.Start(filename);
        }
        public void GeneratePDF(string filename, string imageLoc)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            PdfDocument document = new PdfDocument();

            // Create an empty page or load existing
            // קביעת גודל עמוד ב pdf

            // Get an XGraphics object for drawing
            int counter = 0;

            PdfPage page1 = document.AddPage();
            page1.Size = PageSize.Quarto;
            int w = (int)page1.Width - 32;
            int h = (int)page1.Height - 64;
            XGraphics gfx1 = XGraphics.FromPdfPage(page1);
            DrawImage(gfx1, imageLoc, 16, 16, w, h);

            PdfPage page2 = document.AddPage();
            page2.Size = PageSize.Quarto;
            XGraphics gfx2 = XGraphics.FromPdfPage(page2);
            w = (w / 2) - 8;
            h = (h / 2) - 8;
            DrawImage(gfx2, imageLoc, 16, 16, w, h);
            DrawImage(gfx2, imageLoc, 32 + w, 16, w, h);
            DrawImage(gfx2, imageLoc, 32 + w, 32 + h, w, h);
            DrawImage(gfx2, imageLoc, 16, 32 + h, w, h);

            PdfPage page3 = document.AddPage();
            page3.Size = PageSize.Quarto;
            XGraphics gfx3 = XGraphics.FromPdfPage(page3);
            DrawImage(gfx2, imageLoc, 16, 32 + h, w, h);

            // Save and start View
            document.Save(filename);
            //Process.Start(filename);
        }
        private void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y, int width, int height)
        {
            XImage image = XImage.FromFile("C:\\yael\\programming\\final_project\\newspaperProject\\server\\newspaper\\newspaper\\wwwroot\\Upload\\" + jpegSamplePath);
            gfx.DrawImage(image, x, y, width, height);
        }


        public void AddAdFileToPdf3()
        {
            // שורה נחוצה עבור הרצת הקוד הבא
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // שלום עולם
            // Create a new PDF document -- יצירת קובץ חדש
            PdfDocument document = new PdfDocument();
            XFont font = new XFont("Times", 20, XFontStyle.BoldItalic);
            PageSize[] pageSizes = (PageSize[])Enum.GetValues(typeof(PageSize));
            foreach (PageSize pageSize in pageSizes)
            {
                if (pageSize == PageSize.Undefined)
                    continue;
                PdfPage page = document.AddPage();
                page.Size = pageSize;
                XGraphics gfx = XGraphics.FromPdfPage(page);
                gfx.DrawString(pageSize.ToString(), font, XBrushes.DarkCyan,
                    new XRect(0, 0, page.Width, page.Height),
                    XStringFormats.Center);
                page = document.AddPage();
                page.Size = pageSize;
                page.Orientation = PageOrientation.Landscape;
                gfx = XGraphics.FromPdfPage(page);
                gfx.DrawString(pageSize + "(landscape)", font,
                    XBrushes.DarkCyan, new XRect(0, 0, page.Width, page.Height),
                    XStringFormats.Center);
            }
            const string filename = "C:\\Users\\YAEL\\OneDrive\\שולחן העבודה\\PageSizes_tempfile.pdf";
            document.Save(filename);
            // ...and start a viewer.
            //Process.Start(new ProcessStartInfo { FileName = filename, UseShellExecute = true });
            //Process.Start(filename);
        }
        private void DrawImageOnPage(PdfPage page, XGraphics gfx, int Width, int Height, string[,] matPage, String ImagePath, int i, int j)
        {
            int w = ((int)(page.Width) - 16) / 4;
            int h = ((int)(page.Height) - 48) / 8;
            DrawImage(gfx, ImagePath, 16 + w * i, 16 + h * j, w * Width - 16, h * Height - 16);
            for (int q = i; q < i + Width; q++)
                for (int l = j; l < j + Height; l++)
                    matPage[q, l] = ImagePath;
        }

        // פונקצהי כתיבה לתוך דף pdf
        public void WriteToPdf(string filename)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PdfDocument document = new PdfDocument();
            XFont font = new XFont("Arial", 11, XFontStyle.Regular);
            List<PdfPage> pages = new List<PdfPage>();
            pages.Add(document.AddPage());
            PdfPage page = pages[0];// pages.Count - 1];
            page.Size = PageSize.Quarto;
            XGraphics gfx = XGraphics.FromPdfPage(page);

            string myText = "פרוטוקול תקשורת הוא אוסף של חוקים המגדירים" +
                " את אופן בקשת וקבלת נתונים במערכת תקשורת מסוימת וכולל כללים לייצוג " +
                "המידע , איתות ,אימות , ותיקון שגיאות" +
                " לצורך העברת המידע בערוץ תקשורת .פרוטוקול מוכר ופשוט " +
                "הוא שיחת טלפון הכוללת כללים מוסכמים: הרמת " +
                "השפופרת, קריאת \"הלו\", הצד מנגד עונה ב\"שלום\" )זהו שלב האימות( ולאחר מכן יסביר " +
                "את מהות ההתקשרות ותתחיל" +
                " העברת המידע. לפני ניתוק השיחה ייפרדו האנשים ב\"שלום\" או \"להתראות\". אולם" +
                " ישנה גמישות, ואין בהכרח צורך בפרוטוקול קשיח " +
                "ומוחלט, " +
                "ולכן לא כל שיחת טלפון מתנהלת על-פי הפרוטוקול המדויק הנ\"ל. " +
                "אך כאשר מדובר ברשת תקשורת בין מחשבים, שימוש בפרוטוקולים מדויקים" +
                " הכרחי על-מנת שהצדדים יבינו זה את זה ויוכלו לספק שירותים זה לזה.";

            string t = "Hello friends, my name is Yael Malkin I live in " +
                "Beit Shemesh in the Rama B. I want to tell you about " +
                "one of my hobbies: I love to bake. Before every Sabbath " +
                "before I started studying programming, I would sit down " +
                "with cake and dessert recipes and pick out some recipes " +
                "I hadn’t made in the last 3 months so as not to bore my " +
                "family and also identify new types of cakes.\r\nSix " +
                "years ago, when I was in elementary school at the age " +
                "of thirteen, one day a friend from eighth grade called " +
                "me and asked me if I could join a group in a class that " +
                "is learning cake design because one girl is missing to " +
                "close the group, I thought and in the end I decided to " +
                "join.\r\nToday I can say that since then I have been " +
                "Always busy designing cakes and 3 years ago I even " +
                "started selling them.\r\nThink I would not join the " +
                "class,\r\nJust look at how worth it is to identify " +
                "opportunities and take advantage of them.\r\n";


            int w = ((int)(page.Width) - 16) / 4;
            int h = ((int)(page.Height) - 48) / 8;

            XTextFormatter tf = new XTextFormatter(gfx);
            //XRect rect = new XRect(8, 8, page.Width/4, page.Height);
            XRect rect = new XRect(16 + w * 0, 16 + h * 0, w * 1 - 16, h * 4 - 16);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(t, font, XBrushes.Black, rect, XStringFormats.TopLeft);
            document.Save(filename);
        }

        // פונקציה שממינת את את רשימת הפרסומות
        private List<OrderDetail> SortBySize(List<OrderDetail> orderDetails)
        {
            return orderDetails.OrderByDescending(x => x.Size?.SizeHeight)
                .ThenByDescending(x => x.Size?.SizeWidth)
                .ToList();
        }

        public void Create(string filename, List<OrderDetail> RelevantOrders)
        {
            /*
            // מערך של הזמנות רלונטיות לתאריך של יציאת העיתון הנוכחי - יתמלא בהמשך
            List<OrderDetail> RelevantOrders = new List<OrderDetail>();
            // מערך של כל ההזמנות עם התאריכים
            List<DatesForOrderDetail> DatesList = _datesForOrderDetailActions.GetAllDatesForOrderDetails();
            // מילוי מערך של הזמנות רלונטיות מתוך מערך ההזמנות המלא
            foreach (DatesForOrderDetail date in DatesList)
                if (date.Date == new DateTime(2023, 02, 07))
                    RelevantOrders.Add(date.Details);*/
            // הגדרה של עיתון חדש
            List<string[,]> pagesMats = new List<string[,]>();
            // שורת קסם להרצת קוד שכותב לתוך קובץ pdf
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // יצירת עמוד חגש בעיתון pdf
            PdfDocument document = new PdfDocument();
            // הגדתרת font שאיתו נכתוב לתוך העיתון
            XFont font = new XFont("Ink Free", 20, XFontStyle.BoldItalic);
            // הוספת עמוד אחד למטריצת העיתון
            pagesMats.Add(new string[4, 8]);
            // הגדרת רשימה של עמודים בעיתון pdf
            List<PdfPage> pages = new List<PdfPage>();
            // הוספת עמוד ה pdf לרשימת עמודי ה pdf
            pages.Add(document.AddPage());
            // העמוד pdf שאיתו כרגע אנחנו מתעסקים
            PdfPage page = pages[0];
            // הגדרת גודל העמוד בעיתון ה pdf
            page.Size = PageSize.Quarto;
            // הגדרת משתנה שדרכו אנו כותבים לתוך עיתון pdf
            XGraphics gfx = XGraphics.FromPdfPage(page);
            // כתובת כותרת תחתונה לעיתון pdf
            gfx.DrawString("   ----------------------------------------------------- Yael advertising", font,
                    XBrushes.Black, new XRect(0, -10, page.Width, page.Height),
                    XStringFormats.BottomLeft);
            //// שליחה לפונקציה של מיון הרשימה שיהיה מהפרסומת הגדולה לפרסומת הקטנה
            ////אין צורך במיון הרשימה כבר ממוינת
            ////RelevantOrders = SortBySize(RelevantOrders);
            //List<OrderDetail> RelevantOrders = new List<OrderDetail>();
            ////של קשרי הגומלין לא מתמלא מאליו? size למה ה 
            //OrderDetail temp = new OrderDetail();
            //foreach (OrderDetailDTO detail in RelevantOrdersDTO)
            //{
            //    temp = _Mapper.Map<OrderDetailDTO, OrderDetail>(detail);
            //    temp.Size = _adSize.getSizeById((int)temp.SizeId);
            //    RelevantOrders.Add(temp);
            //}

            // עוברים על על המודעות הרלונטיות ומכניסים לרשימת העיתון
            foreach (OrderDetail detail in RelevantOrders)
            {
                // משתנה שאומר האם היה מקום למודעה
                bool Shubatz = false;
                // רוחב התמונה הנוכחית
                if (detail.Size != null)
                {
                    int Width = (int)(detail.Size.SizeWidth);
                    // גובה התמונה הנוכחית
                    int Height = (int)(detail.Size.SizeHeight);
                    // צריכה לעבור עבור כל פרטי הזמנה האם הם נכנסות בעמוד או לא
                    // בכוונה הלולאה היא רגילה כדי שבשביל ה'דאבל' נוכל לראות האם העמוד הבא ג"כ פנוי
                    for (int k = 0; k < pagesMats.Count; k++)
                    {
                        // הכנסת העמוד הנוכחי למטריצת עזר חדשה
                        string[,] matPage = pagesMats[k];

                        bool isEmpty;

                        for (int i = 0; i < matPage.GetLength(0) && !Shubatz; i = i + Width)
                        {
                            for (int j = 0; j < matPage.GetLength(1) && !Shubatz; j = j + Height)
                            {
                                isEmpty = true;
                                // אם המשבצת הראשונה ריקה צריך לודא שכל המשבצות ריקות
                                if (matPage[i, j] == null)
                                {
                                    for (int q = i; q < i + Width && isEmpty; q++)
                                        for (int l = j; l < j + Height && isEmpty; l++)
                                            // אם המקום תופוס
                                            if (matPage[q, l] != null)
                                                isEmpty = false;
                                }
                                // אם המשבצת הראשונה מלאה בטוח שאין מקום במיקום זה
                                else
                                    isEmpty = false;
                                // בדיקה אם אכן יש מקום למודעה - אז להוסיף את המודעה
                                if (isEmpty)
                                {
                                    DrawImageOnPage(page, gfx, Width, Height, matPage, detail.AdFile, i, j);
                                    Shubatz = true;
                                }
                            }
                        }
                    }

                    // במקרה שהשיבוץ באחד מהדפים הקודמים לא הצליח
                    if (!Shubatz)
                    {
                        // יצירת דף חדש במטריצת העיתון
                        string[,] newPage = new string[4, 8];
                        // הוספת עמוד חדש לעיתון ה pdf
                        pages.Add(document.AddPage());
                        // עדכון מטריצת העמוד הנוכחי לעמוד האחרון ברשימת המטריצות
                        page = pages[pages.Count - 1];
                        // הגדרת גודלו של העמוד
                        page.Size = PageSize.Quarto;
                        gfx = XGraphics.FromPdfPage(page);
                        // הוספת כותרת תחתונה לעמוד
                        gfx.DrawString("   ----------------------------------------------------- Yael advertising", font,
                            XBrushes.Black, new XRect(0, -10, page.Width, page.Height),
                            XStringFormats.BottomLeft);
                        // הוספת התמונה לעמוד החדש שבטוח שיש בו מקום
                        DrawImageOnPage(page, gfx, Width, Height, newPage, detail.AdFile, 0, 0);
                        // הוספת העמוד לעיתון ה pdf
                        pagesMats.Add(newPage);
                    }
                }
            }
            document.Save(filename);
        }

        //פונקציה שכותבת לתוך קובץ word
        public void FirstWord(string filePath, string[] codeLines)
        {
            // Create a new Word document
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                // Add a new main document part
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                // Create a new document tree
                Document document = new Document();
                Body body = new Body();

                // Create a new table
                Table table = new Table();
                TableProperties tableProperties = new TableProperties(
                    new TableBorders(
                        new TopBorder(),
                        new BottomBorder(),
                        new LeftBorder(),
                        new RightBorder(),
                        new InsideHorizontalBorder(),
                        new InsideVerticalBorder()
                    )
                );
                table.AppendChild(tableProperties);

                // Set the number of columns
                int numColumns = 4;

                // Calculate the number of rows needed based on the number of code lines and the number of columns
                int numRows = (int)Math.Ceiling((double)codeLines.Length / numColumns);

                // Loop through each row
                for (int i = 0; i < numRows; i++)
                {
                    TableRow row = new TableRow();

                    // Loop through each column
                    for (int j = 0; j < numColumns; j++)
                    {
                        // Calculate the index of the current code line
                        int index = i + j * numRows;

                        // Create a new cell
                        TableCell cell = new TableCell();

                        //כתיבת כותרת
                        if (index < codeLines.Length && codeLines[index].Contains("Title"))
                        {
                            Paragraph titleParagraph = new Paragraph();
                            Run titleRun = new Run();
                            Text titleText = new Text(codeLines[index]);
                            titleRun.Append(titleText);
                            titleRun.RunProperties = new RunProperties(new Bold(), new FontSize() { Val = "24" }, new Justification() { Val = JustificationValues.Right });
                            titleParagraph.Append(titleRun);
                            cell.Append(titleParagraph);
                        }
                        // If there is a code line for this cell, add it to the cell
                        else
                        if (index < codeLines.Length)
                        {
                            Paragraph paragraph = new Paragraph();
                            Run run = new Run();
                            Text text = new Text(codeLines[index]);
                            run.Append(text);
                            paragraph.Append(run);
                            cell.Append(paragraph);
                        }

                        // Add the cell to the row
                        row.Append(cell);
                    }

                    // Add the row to the table
                    table.Append(row);
                }

                // Add the table to the body
                body.Append(table);

                // Add the body to the document
                document.Append(body);

                // Add the document to the main document part
                mainPart.Document = document;

                // Save the changes
                mainPart.Document.Save();
            }
        }

        //convert from Word to PDF
        public void convertWordPFD(string Input, string Output)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            var doc = DocumentModel.Load(Input);
            doc.Save(Output);
        }

        //--------------------------------------------------------------------------------------
        //----------------------- כל מה שקשור להזמנת פרסומת -----------------------------------
        //--------------------------------------------------------------------------------------

        //פונקציה שממירה מערך dto של פרטי הזמנה ומערך רגיל של פרטי הזמנה
        private List<OrderDetail> ListOrderDetailDTOToListOrderDetail(List<OrderDetailDTO> listOrderDetails)
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (OrderDetailDTO orderD in listOrderDetails)
                orderDetails.Add(_Mapper.Map<OrderDetailDTO, OrderDetail>(orderD));
            return orderDetails;
        }

        // פונקציה שמקבלת מערך של פרטי הזמנות ומחזירה את הסכום הכללי שלהם
        private decimal FinalPrice(List<OrderDetail> orderDetails)
        {
            decimal sum = 0;
            List<AdSize> listAdSize = _adSize.GetAllAdSizes();
            foreach (OrderDetail od in orderDetails)
            {
                sum += listAdSize.FirstOrDefault(s => s.SizeId == od.SizeId).SizePrice;
            }
            return sum;
        }

        // פונקציה שמכניסה פרטי הזמנות למסד הנתונים
        // ומחזירה רשימה של קודים של פרטי הזמנות
        private List<int> EnterOrderDetails(List<OrderDetail> orderDetails, int orderId)
        {
            List<int> ordersIds = new List<int>();
            foreach (var orderDetail in orderDetails)
            {
                orderDetail.OrderId = orderId;
                ordersIds.Add(_ordersDetailActions.AddNewOrderDetail(orderDetail));
            }
            return ordersIds;
        }

        // פונקציה שמקבלת רשימה של רשימות של תאריכים ורשימה של תז של פרטי המנות ומכניסה למסד הנתונים
        private void EnterDates(List<List<DateTime>> listDates, List<int> orderDetailsIds)
        {
            DatesForOrderDetail datesForOrderDetail;
            for (int i = 0; i < orderDetailsIds.Count; i++)
            {
                foreach (var date in listDates[i])
                {
                    datesForOrderDetail = new DatesForOrderDetail();
                    datesForOrderDetail.Date = date;
                    datesForOrderDetail.DetailsId = orderDetailsIds[i];
                    datesForOrderDetail.DateStatus = false;
                    _datesForOrderDetailActions.AddNewDateForOrderDetail(datesForOrderDetail);
                }
            }
        }

        public void ConvertPdfToWord(string pdfFilePath, string wordFilePath)
        {

            // Create a new instance of the PDF Focus .Net library
            PdfFocus pdfFocus = new PdfFocus();

            // Load the PDF file using PDF Focus .Net
            pdfFocus.OpenPdf(pdfFilePath);

            // Save the PDF file as a Word file using PDF Focus .Net
            if (pdfFocus.PageCount > 0)
            {
                pdfFocus.WordOptions.Format = PdfFocus.CWordOptions.eWordDocument.Docx;
                pdfFocus.ToWord(wordFilePath);
            }
        }


        // פונקציה שמקבלת לקוח, מערך של פרטי הזמנות ומערך של מערכים לתאריכים
        // הפונקציה מכניסה למסד הנתונים הזמנה וכל מה שהיא קיבלה
        public void FinishOrder(CustomerDTO customer, List<List<DateTime>> listDates, List<OrderDetailDTO> listOrderDetails)
        {
            List<OrderDetail> orderDetails = ListOrderDetailDTOToListOrderDetail(listOrderDetails).ToList();
            Order newOrder = new Order()
            {
                CustId = GetIdByCustomer(customer),
                OrderDate = DateTime.Now,
                OrderFinalPrice = FinalPrice(orderDetails)
            };
            _order.AddNewOrder(newOrder);
            List<int> orderDetailsIds = EnterOrderDetails(orderDetails, newOrder.OrderId);
            EnterDates(listDates, orderDetailsIds);
        }

        public List<OrderDetailDTO> GetAllOrderDetails()
        {
            List<OrderDetailDTO> orderDetailsDTO = new List<OrderDetailDTO>();
            List<OrderDetail> details = _ordersDetailActions.GetAllOrderDetails();
            foreach (var detail in details)
                orderDetailsDTO.Add(_Mapper.Map<OrderDetail, OrderDetailDTO>(detail));
            return orderDetailsDTO;
        }

        public List<DatesForOrderDetailDTO> GetAllDatesForDetails()
        {
            var datesForDetailsDTO = new List<DatesForOrderDetailDTO>();
            List<DatesForOrderDetail> datesForOrderDetails = _datesForOrderDetailActions.GetAllDatesForOrderDetails();
            foreach (var date in datesForOrderDetails)
                datesForDetailsDTO.Add(_Mapper.Map<DatesForOrderDetail, DatesForOrderDetailDTO>(date));
            return datesForDetailsDTO;
        }


        //מכאן התחילו השינויים ליום 16/06/2023
        //שיבוץ העיתון

        public void Shabets()
        {
            //// שליפת כל פרטי ההזמנות וכן את כל פרטי ההזמנות של התאריך הקרוב.
            //List<OrderDetailDTO> orderDetailDTOs = GetAllOrderDetails();
            //List<DatesForOrderDetailDTO> allDates = GetAllDatesForDetails();
            //List<int> detailsIds = new List<int>();
            //// מעבר על כל התאריכים ושליפת הפרטי ההזמנה הרלוונטיים לתאריך הקרוב
            //foreach (var date in allDates)
            //{
            //    if (date.Date == DateTime.Today)
            //    {
            //        detailsIds.Add((int)date.DetailsId);
            //    }
            //}
            //// רשימה עם פרטי הזמנה רלוונטיים
            //List<OrderDetailDTO> relevanteAds = new List<OrderDetailDTO>();
            //foreach (int id in detailsIds)
            //    relevanteAds.Add(_Mapper.Map<OrderDetail, OrderDetailDTO>(_ordersDetailActions.GetOrderDetailsById(id)));
            
            // זה בקיצור שליפת כל פרטי ההזמנות הרלונטיות
            List<DatesForOrderDetail> allDates = _datesForOrderDetailActions.GetAllDatesForOrderDetails();//GetAllDatesForDetails();
            List<OrderDetail> relevanteAds = new List<OrderDetail>();
            foreach (var date in allDates)
                if (date.Date == new DateTime(2023, 07, 04))
                    relevanteAds.Add(date.Details);

            // הגדרת רשימות של מודעות מילים ומודעות פרסומת
            List<OrderDetail> allRelevantFileAds = new List<OrderDetail>();
            List<OrderDetail> allRelevantWordAds = new List<OrderDetail>();

            // מיון כל ההזמנות הרלונטיות למילים ופרסומות
            foreach (OrderDetail detail in relevanteAds)
                if (detail.AdFile != null)
                    allRelevantFileAds.Add(detail);
                else if (detail.AdContent != null)
                    allRelevantWordAds.Add(detail);

            //מציאת הניתוב הרלוונטי ושם העיתון הנוכחי
            //שליפת כל העיתונים שיצאיו עד כה
            List<NewspapersPublished> allNewpapers = _newspapersPublished.GetAllNewspapersPublished();
            //נתינת שם לעיתון עפי הקוד האחרון + 1
            int NewspaperId = (allNewpapers.Max(x => x.NewspaperId)) + 1;
            // word ו pdf נתינת ניתוב לתיקיית עיתונים והגדרת ניתובים ל 
            string PDFpath = myPath + "\\" + NewspaperId + ".pdf",
                WORDpath = myPath + "\\" + NewspaperId + ".docx";


            //מיון הפרסומות לפי גודל
            //List<AdSizeDTO> adSizeDTOs = GetAllAdSize();
            //List<OrderDetailDTO> allDetailsFileAds = new List<OrderDetailDTO>();
            //foreach (AdSizeDTO size in adSizeDTOs)
            //    foreach (OrderDetailDTO detail in allRelevantFileAds)
            //        if (detail.SizeId == size.SizeId)
            //            allDetailsFileAds.Add(detail);
            // מיון המודעות בקצרה
            allRelevantFileAds = SortBySize(allRelevantFileAds);

            //עכשיו הרשימה ממוינת לפי גודל הפרסומות מהגדול לקטן
            //PDF אחרי שמיינו את כל הגדלים אפשר פשוט להכניס את כל המודעות לעיתון 
            //הבעיה היא שהכנסת המודעות היא אינה רציפה וכל מודעת יכולה לדרוס את השניה
            //נכניס את המודעות לעיתון
            List<OrderDetailDTO> od = new List<OrderDetailDTO>();
            foreach (OrderDetail detail in allRelevantFileAds)
                od.Add(_Mapper.Map<OrderDetail, OrderDetailDTO>(detail));
            Create(myPath + "\\Pictures.pdf", allRelevantFileAds);


            //רשימה של כל תתי הקטגוריות
            List<WordAdSubCategoryDTO> categories = GetAllWordAdSubCategories();
            //רשימה של כל פרטי ההזמנה ממוינים לפי קטגוריות
            List<OrderDetail> allDetailsWordAds = new List<OrderDetail>();
            //רשימה שתכיל תת קטגוריה ומיד אח"כ את כל המודעות שלה
            List<string> wordAdToPrint = new List<string>();
            foreach (WordAdSubCategoryDTO category in categories)
            {
                wordAdToPrint.Add(category.WordCategoryName);
                foreach (OrderDetail detail in allRelevantWordAds)
                    if (detail.WordCategoryId == category.WordCategoryId)
                    {
                        allDetailsWordAds.Add(detail);
                        wordAdToPrint.Add(detail.AdContent);
                    }
            }
            string[] WordAdToPrint = wordAdToPrint.ToArray();

            //הכנסת מודעות מילים לעיתון

            //חדש word יצירת קובץ 
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(WORDpath, WordprocessingDocumentType.Document))
            {
                // Add a new main document part
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                // Create a new document tree
                Document document = new Document();
                Body body = new Body();

                // Create a new table
                Table table = new Table();
                TableProperties tableProperties = new TableProperties(
                    new TableBorders(
                        new TopBorder(),
                        new BottomBorder(),
                        new LeftBorder(),
                        new RightBorder(),
                        new InsideHorizontalBorder(),
                        new InsideVerticalBorder()
                    )
                );
                table.AppendChild(tableProperties);

                // Set the number of columns
                int numColumns = 4;

                // Calculate the number of rows needed based on the number of code lines and the number of columns
                int numRows = (int)Math.Ceiling((double)WordAdToPrint.Length / numColumns);

                // Loop through each row
                for (int i = 0; i < numRows; i++)
                {
                    TableRow row = new TableRow();

                    // Loop through each column
                    for (int j = 0; j < numColumns; j++)
                    {
                        // Calculate the index of the current code line
                        int index = i + j * numRows;

                        // Create a new cell
                        TableCell cell = new TableCell();

                        //כתיבת כותרת
                        if (index < WordAdToPrint.Length && WordAdToPrint[index].Contains("Title"))
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Paragraph titleParagraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                            DocumentFormat.OpenXml.Wordprocessing.Run titleRun = new DocumentFormat.OpenXml.Wordprocessing.Run();
                            Text titleText = new Text(WordAdToPrint[index]);
                            titleRun.Append(titleText);
                            titleRun.RunProperties = new RunProperties(new Bold(), new FontSize() { Val = "24" }, new Justification() { Val = JustificationValues.Right });
                            titleParagraph.Append(titleRun);
                            cell.Append(titleParagraph);
                        }
                        // If there is a code line for this cell, add it to the cell
                        else
                        if (index < WordAdToPrint.Length)
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                            DocumentFormat.OpenXml.Wordprocessing.Run run = new DocumentFormat.OpenXml.Wordprocessing.Run();
                            Text text = new Text(WordAdToPrint[index]);
                            run.Append(text);
                            paragraph.Append(run);
                            cell.Append(paragraph);
                        }

                        // Add the cell to the row
                        row.Append(cell);
                    }

                    // Add the row to the table
                    table.Append(row);
                }

                // Add the table to the body
                body.Append(table);

                // Add the body to the document
                document.Append(body);

                // Add the document to the main document part
                mainPart.Document = document;

                // Save the changes
                mainPart.Document.Save();
            }

            convertWordPFD(WORDpath, PDFpath);

        }

    }
}




