using iText.IO.Image;
using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using iText.Kernel.Geom;
using DAL.Models;
using DAL.Actions.Interfaces;
using GemBox.Document;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;

namespace BLL.functions
{

    public class Fff
    {

        private string myPath = "C:\\yael\\final_project\\newspaperProject\\server\\newspaper\\newspaper\\wwwroot\\TempWord";

        IAdSizeActions _adSize;
        IOrderDetailActions _ordersDetailActions;
        IDatesForOrderDetailActions _datesForOrderDetailActions;
        IAdPlacementActions _adPlacement;
        IWordAdSubCategoryActions _wpAdSubCategoryActions;
        ICustomerActions _customerActions;
        IOrderActions _order;
        INewspapersPublishedActions _newspapersPublished;


        public Fff(IAdSizeActions adSize,
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

        public Fff()
        {
        }

        public void aaa()
        {

            string imagePath = @"C:\Users\eliah\OneDrive\תמונות\board.png";
            string outputPath = @"c:\eSupport\stam.pdf";
            // יצירת קובץ PDF חדש
            using (FileStream fs = new FileStream(outputPath, FileMode.Create))
            {
                // יצירת מסמך PDF חדש
                PdfWriter writer = new PdfWriter(fs);
                PdfDocument pdfDocument = new PdfDocument(writer);
                Document doc = new Document(pdfDocument);
                pdfDocument.SetDefaultPageSize(PageSize.A5);
                doc.SetMargins(0, 0, 0, 0);

                // הוספת תמונה מהקובץ המקומי
                ImageData imageData = ImageDataFactory.Create(imagePath);
                Image image = new Image(imageData);

                // גודל התמונה בדף
                float imageWidth = 200;
                float imageHeight = 100;

                // מיקום התמונה בדף (פינה השמאלית התחתונה)
                float xPosition = 100;
                float yPosition = 100;

                // יציב את התמונה במיקום ובגודל שצוין
                //image.SetFixedPosition(xPosition, yPosition);
                image.SetRelativePosition(0, 0, 0, 0);

                //image.ScaleAbsolute(imageWidth, imageHeight);
                // הוסף את התמונה למסמך
                doc.Add(image);

                // סגירת המסמך
                doc.Close();
            }


            // הדפסת הודעה לאחר הצלחת התהליך
            Console.WriteLine("התמונה הוספה בהצלחה לקובץ PDF!");
        }

        public void Shabets(string pathPdf)
        {
            // זה בקיצור שליפת כל פרטי ההזמנות הרלונטיות
            List<DatesForOrderDetail> allDates = _datesForOrderDetailActions.GetAllDatesForOrderDetails();
            List<OrderDetail> relevanteAds = new List<OrderDetail>();
            foreach (var date in allDates)
                if (date.Date == new DateTime(2023, 07, 25))
                    relevanteAds.Add(date.Details);

            // הגדרת רשימות של פרסומת
            List<OrderDetail> allRelevantFileAds = new List<OrderDetail>();

            // מיון כל ההזמנות הרלונטיות לפרסומות
            foreach (OrderDetail detail in relevanteAds)
                if (detail.AdFile != null)
                    allRelevantFileAds.Add(detail);

            // מציאת הניתוב הרלוונטי ושם העיתון הנוכחי
            // שליפת כל העיתונים שיצאיו עד כה
            List<NewspapersPublished> allNewpapers = _newspapersPublished.GetAllNewspapersPublished();
            // נתינת שם לעיתון עפי הקוד האחרון + 1
            int newspaperId = allNewpapers.Max(x => x.NewspaperId) + 1;
            // word ו pdf נתינת ניתוב לתיקיית עיתונים והגדרת ניתובים ל 
            string PDFpath = myPath + "\\regularTemplate" + ".pdf";
            string WORDpath = myPath + "\\regularTemplate" + ".dotx";

            // מיון המודעות בקצרה
            allRelevantFileAds = SortBySize(allRelevantFileAds);

            ConvertFromWordToPdf(WORDpath, PDFpath);

            Create(PDFpath, allRelevantFileAds);

        }

        private List<OrderDetail> SortBySize(List<OrderDetail> orderDetails)
        {
            return orderDetails.OrderByDescending(x => x.Size?.SizeHeight)
                .ThenByDescending(x => x.Size?.SizeWidth)
                .ToList();
        }

        public void ConvertFromWordToPdf(string Input, string Output)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            var doc = DocumentModel.Load(Input);
            doc.Save(Output);
        }


        public void Create(string filename, List<OrderDetail> relevantOrders)
        {

            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                List<string[,]> pagesMats = new List<string[,]>();
                pagesMats.Add(new string[4, 8]);

                PdfWriter writer = new PdfWriter(fs);
                PdfDocument pdfDocument = new PdfDocument(writer);
                Document doc = new Document(pdfDocument);
                pdfDocument.SetDefaultPageSize(PageSize.A5);
                float height = pdfDocument.GetDefaultPageSize().GetHeight();
                float width = pdfDocument.GetDefaultPageSize().GetWidth();

                doc.SetMargins(0, 0, 0, 0);

                foreach (OrderDetail detail in relevantOrders)
                {
                    bool Shubatz = false;
                    if (detail.Size != null)
                    {
                        int Width = (int)(detail.Size.SizeWidth);
                        int Height = (int)(detail.Size.SizeHeight);
                        for (int k = 0; k < pagesMats.Count; k++)
                        {
                            string[,] matPage = pagesMats[k];

                            bool isEmpty;

                            for (int i = 0; i < matPage.GetLength(0) && !Shubatz; i = i + Width)
                            {
                                for (int j = 0; j < matPage.GetLength(1) && !Shubatz; j = j + Height)
                                {
                                    isEmpty = true;
                                    if (matPage[i, j] == null)
                                    {
                                        for (int q = i; q < i + Width && isEmpty; q++)
                                            for (int l = j; l < j + Height && isEmpty; l++)
                                                if (matPage[q, l] != null)
                                                    isEmpty = false;
                                    }
                                    else
                                        isEmpty = false;
                                    if (isEmpty)
                                    {
                                        DrawImageOnPage(doc, height, width, Width, Height, matPage, detail.AdFile, i, j);
                                        Shubatz = true;
                                    }
                                }
                            }
                        }

                        if (!Shubatz)
                        {
                            string[,] newPage = new string[4, 8];
                            pdfDocument.AddNewPage();
                            DrawImageOnPage(doc, height, width, Width, Height, newPage, detail.AdFile, 0, 0);
                            pagesMats.Add(newPage);
                        }
                    }
                }
                //// הוספת תמונה מהקובץ המקומי
                //ImageData imageData = ImageDataFactory.Create(imagePath);
                //Image image = new Image(imageData);

                //// גודל התמונה בדף
                //float imageWidth = 200;
                //float imageHeight = 100;

                //// מיקום התמונה בדף (פינה השמאלית התחתונה)
                //float xPosition = 100;
                //float yPosition = 100;

                //// יציב את התמונה במיקום ובגודל שצוין
                ////image.SetFixedPosition(xPosition, yPosition);
                //image.SetRelativePosition(0, 0, 0, 0);

                ////image.ScaleAbsolute(imageWidth, imageHeight);
                //// הוסף את התמונה למסמך
                //doc.Add(image);

                //// סגירת המסמך
                doc.Close();
            }
        }


        private void DrawImageOnPage(Document doc, float height, float width, int Width, int Height, string[,] matPage, string ImagePath, int i, int j)
        {
            float w = (width - 16) / 4;
            float h = (height - 48) / 8;

            // הוספת תמונה מהקובץ המקומי
            ImageData imageData = ImageDataFactory.Create("C:\\yael\\final_project\\newspaperProject\\server\\newspaper\\newspaper\\wwwroot\\Upload\\" + ImagePath);
            Image image = new Image(imageData);
            image.SetRelativePosition(16 + w * i, 16 + h * j, w * Width - 16, h * Height - 16);
            doc.Add(image);

            for (int q = i; q < i + Width; q++)
                for (int l = j; l < j + Height; l++)
                    matPage[q, l] = ImagePath;

        }




        /* 
         










        public void Shabets(string pathPdf)
        {
            // זה בקיצור שליפת כל פרטי ההזמנות הרלונטיות
            List<DatesForOrderDetail> allDates = _datesForOrderDetailActions.GetAllDatesForOrderDetails();
            List<OrderDetail> relevanteAds = new List<OrderDetail>();
            foreach (var date in allDates)
                if (date.Date == new DateTime(2023, 07, 25))
                    relevanteAds.Add(date.Details);

            // הגדרת רשימות של פרסומת
            List<OrderDetail> allRelevantFileAds = new List<OrderDetail>();

            // מיון כל ההזמנות הרלונטיות לפרסומות
            foreach (OrderDetail detail in relevanteAds)
                if (detail.AdFile != null)
                    allRelevantFileAds.Add(detail);

            // מציאת הניתוב הרלוונטי ושם העיתון הנוכחי
            // שליפת כל העיתונים שיצאיו עד כה
            List<NewspapersPublished> allNewpapers = _newspapersPublished.GetAllNewspapersPublished();
            // נתינת שם לעיתון עפי הקוד האחרון + 1
            int newspaperId = allNewpapers.Max(x => x.NewspaperId) + 1;
            // word ו pdf נתינת ניתוב לתיקיית עיתונים והגדרת ניתובים ל 
            string PDFpath = myPath + "\\regularTemplate" + ".pdf";
            string WORDpath = myPath + "\\regularTemplate" + ".dotx";

            // מיון המודעות בקצרה
            allRelevantFileAds = SortBySize(allRelevantFileAds);

            ConvertFromWordToPdf(WORDpath, PDFpath);

            Create(PDFpath, allRelevantFileAds);

        }



        private void Create(string filename, List<OrderDetail> relevantOrders)
        {

            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                List<string[,]> pagesMats = new List<string[,]>();
                pagesMats.Add(new string[4, 8]);

                PdfWriter writer = new PdfWriter(fs);
                PdfDocument pdfDocument = new PdfDocument(writer);
                iText.Layout.Document doc = new iText.Layout.Document(pdfDocument);
                pdfDocument.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A5);
                float height = pdfDocument.GetDefaultPageSize().GetHeight();
                float width = pdfDocument.GetDefaultPageSize().GetWidth();

                doc.SetMargins(0, 0, 0, 0);

                foreach (OrderDetail detail in relevantOrders)
                {
                    bool Shubatz = false;
                    if (detail.Size != null)
                    {
                        int Width = (int)(detail.Size.SizeWidth);
                        int Height = (int)(detail.Size.SizeHeight);
                        for (int k = 0; k < pagesMats.Count; k++)
                        {
                            string[,] matPage = pagesMats[k];

                            bool isEmpty;

                            for (int i = 0; i < matPage.GetLength(0) && !Shubatz; i = i + Width)
                            {
                                for (int j = 0; j < matPage.GetLength(1) && !Shubatz; j = j + Height)
                                {
                                    isEmpty = true;
                                    if (matPage[i, j] == null)
                                    {
                                        for (int q = i; q < i + Width && isEmpty; q++)
                                            for (int l = j; l < j + Height && isEmpty; l++)
                                                if (matPage[q, l] != null)
                                                    isEmpty = false;
                                    }
                                    else
                                        isEmpty = false;
                                    if (isEmpty)
                                    {
                                        DrawImageOnPage(doc, height, width, Width, Height, matPage, detail.AdFile, i, j);
                                        Shubatz = true;
                                    }
                                }
                            }
                        }

                        if (!Shubatz)
                        {
                            string[,] newPage = new string[4, 8];
                            pdfDocument.AddNewPage();
                            DrawImageOnPage(doc, height, width, Width, Height, newPage, detail.AdFile, 0, 0);
                            pagesMats.Add(newPage);
                        }
                    }
                }
                doc.Close();
            }
        }


        private void DrawImageOnPage(iText.Layout.Document doc, float height, float width, int Width, int Height, string[,] matPage, string ImagePath, int i, int j)
        {
            float w = (width - 16) / 4;
            float h = (height - 48) / 8;

            // הוספת תמונה מהקובץ המקומי
            ImageData imageData = ImageDataFactory.Create("C:\\yael\\final_project\\newspaperProject\\server\\newspaper\\newspaper\\wwwroot\\Upload\\" + ImagePath);
            Image image = new Image(imageData);
            image.SetRelativePosition(16 + w * i, 16 + h * j, w * Width - 16, h * Height - 16);
            doc.Add(image);

            for (int q = i; q < i + Width; q++)
                for (int l = j; l < j + Height; l++)
                    matPage[q, l] = ImagePath;

        }
         */
    }
}

