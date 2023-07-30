using AutoMapper;
using DAL.Actions.Interfaces;
using DAL.Models;
using DTO.Repository;
using PdfSharp.Drawing;
using System.Text;
using PdfSharp.Pdf;
using PdfSharp;
using PdfSharp.Drawing.Layout;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using Document = DocumentFormat.OpenXml.Wordprocessing.Document;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using PageSize = PdfSharp.PageSize;
using DocumentFormat.OpenXml;
using GemBox.Document;
using SautinSoft;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;
using System;
using PdfSharp.Pdf.IO;

namespace BLL.functions
{
    public class Funcs : IFuncs
    {
        static IMapper _Mapper;

        private string myPath = "C:\\yael\\final_project\\newspaperProject\\server\\newspaper\\newspaper\\wwwroot\\TempWord";

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

        #region OrderDetail
        
        private List<OrderDetail> FromListOrderDetailDTOToListOrderDetail(List<OrderDetailDTO> listOrderDetails)
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (OrderDetailDTO orderD in listOrderDetails)
                orderDetails.Add(_Mapper.Map<OrderDetailDTO, OrderDetail>(orderD));
            return orderDetails;
        }

        public List<OrderDetailDTO> GetAllOrderDetails()
        {
            List<OrderDetailDTO> orderDetailsDTO = new List<OrderDetailDTO>();
            List<OrderDetail> details = _ordersDetailActions.GetAllOrderDetails();
            foreach (var detail in details)
                orderDetailsDTO.Add(_Mapper.Map<OrderDetail, OrderDetailDTO>(detail));
            return orderDetailsDTO;
        }

        //פונקציה שממינת את את רשימת הפרסומות
        private List<OrderDetail> SortBySize(List<OrderDetail> orderDetails)
        {
            return orderDetails.OrderByDescending(x => x.Size?.SizeHeight)
                .ThenByDescending(x => x.Size?.SizeWidth)
                .ToList();
        }

        #endregion

        #region DatesForOrderDetail

        private List<DatesForOrderDetailDTO> GetAllDatesForDetails()
        {
            var datesForDetailsDTO = new List<DatesForOrderDetailDTO>();
            List<DatesForOrderDetail> datesForOrderDetails = _datesForOrderDetailActions.GetAllDatesForOrderDetails();
            foreach (var date in datesForOrderDetails)
                datesForDetailsDTO.Add(_Mapper.Map<DatesForOrderDetail, DatesForOrderDetailDTO>(date));
            return datesForDetailsDTO;
        }

        #endregion

        #region PdfSharp

        private void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y, int width, int height)
        {
            XImage image = XImage.FromFile("C:\\yael\\final_project\\newspaperProject\\server\\newspaper\\newspaper\\wwwroot\\Upload\\" + jpegSamplePath);
            gfx.DrawImage(image, x, y, width, height);
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

        public void Create(string filename, List<OrderDetail> relevantOrders)
        {
            PdfDocument pdfDocument = PdfReader.Open(filename, PdfDocumentOpenMode.Modify);

            // הגדרה של עיתון חדש
            List<string[,]> pagesMats = new List<string[,]>();
            // שורת קסם להרצת קוד שכותב לתוך קובץ pdf
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //// יצירת עמוד חגש בעיתון pdf
            //PdfDocument document = new PdfDocument();
            // הגדתרת font שאיתו נכתוב לתוך העיתון
            //XFont font = new XFont("Ink Free", 20, XFontStyle.BoldItalic);
            // הוספת עמוד אחד למטריצת העיתון
            pagesMats.Add(new string[4, 8]);
            // הגדרת רשימה של עמודים בעיתון pdf
            //List<PdfPage> pages = new List<PdfPage>();
            // הוספת עמוד ה pdf לרשימת עמודי ה pdf
            //pages.Add(pdfDocument.Pages.Add());
            pdfDocument.Pages.Add();
            // העמוד pdf שאיתו כרגע אנחנו מתעסקים
            PdfPage page = pdfDocument.Pages[0];// pages[0];
            // הגדרת גודל העמוד בעיתון ה pdf
            //page.Size = PageSize.Quarto;
            // הגדרת משתנה שדרכו אנו כותבים לתוך עיתון pdf
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // עוברים על על המודעות הרלונטיות ומכניסים לרשימת העיתון
            foreach (OrderDetail detail in relevantOrders)
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
                        pdfDocument.Pages.Add();
                        //pages.Add(pdfDocument.AddPage());
                        // עדכון מטריצת העמוד הנוכחי לעמוד האחרון ברשימת המטריצות
                        page = pdfDocument.Pages[pdfDocument.Pages.Count - 1];
                        // הגדרת גודלו של העמוד
                        //page.Size = PageSize.Quarto;
                        //gfx = XGraphics.FromPdfPage(page);
                        // הוספת התמונה לעמוד החדש שבטוח שיש בו מקום
                        DrawImageOnPage(page, gfx, Width, Height, newPage, detail.AdFile, 0, 0);
                        // הוספת העמוד לעיתון ה pdf
                        pagesMats.Add(newPage);
                    }
                }
            }
            pdfDocument.Save(filename);
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

        #endregion

        #region Converts

        public void ConvertFromWordToPdf(string Input, string Output)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            var doc = DocumentModel.Load(Input);
            doc.Save(Output);
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

        #endregion

        #region FinishOrder

        // פונקציה שמכניסה פרטי הזמנות למסד הנתונים ומחזירה רשימה של קודים של פרטי הזמנות
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

        #region FinishOrderFiles

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

        // פונקציה שמקבלת לקוח, מערך של פרטי הזמנות ומערך של מערכים לתאריכים
        // הפונקציה מכניסה למסד הנתונים הזמנה וכל מה שהיא קיבלה
        public void FinishOrder(CustomerDTO customer, List<List<DateTime>> listDates, List<OrderDetailDTO> listOrderDetails)
        {
            List<OrderDetail> orderDetails = FromListOrderDetailDTOToListOrderDetail(listOrderDetails).ToList();
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

        #endregion

        #region FinishOrderWords

        public static int CountWords(string sentence)
        {
            sentence = sentence.Trim();
            if (string.IsNullOrEmpty(sentence))
                return 0;
            string[] words = sentence.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }

        public void FinishOrderAdWords(CustomerDTO customer, List<List<DateTime>> listDates, List<OrderDetailDTO> listOrderDetails)
        {
            List<OrderDetail> orderDetails = FromListOrderDetailDTOToListOrderDetail(listOrderDetails).ToList();
            Order newOrder = new Order()
            {
                CustId = GetIdByCustomer(customer),
                OrderDate = DateTime.Now,
                OrderFinalPrice = CountWords(listOrderDetails[0].AdContent)
            };
            _order.AddNewOrder(newOrder);
            List<int> orderDetailsIds = EnterOrderDetails(orderDetails, newOrder.OrderId);
            EnterDates(listDates, orderDetailsIds);
        }

        #endregion

        #endregion

        #region FunctionsByOpenXml

        public void CompleteWordTemplate(string fullname, string path)
        {
            string tempPath = path + @"\temp";
            if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);
            string tempFileFullName = path + @"\temp\temp.dotx";
            File.Copy(fullname, tempFileFullName, true);
            using (WordprocessingDocument myDoc = WordprocessingDocument.Open(tempFileFullName, true))
            {
                //ReplaceUserWordTemplates(myDoc);
                myDoc.MainDocumentPart.Document.Save();
                myDoc.Close();
            }
            File.Copy(tempFileFullName, path + @"\temp\stam.docx", true);
            CreateOneDocAndCopyToDest(tempFileFullName, path + @"\temp\stam.docx");
        }

        private void ReplaceUserWordTemplates(WordprocessingDocument myDoc)
        {
            var mainPart = myDoc.MainDocumentPart;
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            DateTime dateOfPrint = new DateTime(2023, 07, 25);

            ////////////////////////////////////////////
            List<DatesForOrderDetail> allDates = _datesForOrderDetailActions.GetAllDatesForOrderDetails();//GetAllDatesForDetails();
            List<OrderDetail> relevanteAds = new List<OrderDetail>();
            foreach (var date in allDates)
                if (date.Date == dateOfPrint)
                    relevanteAds.Add(date.Details);

            List<OrderDetail> allRelevantWordAds = new List<OrderDetail>();

            foreach (OrderDetail detail in relevanteAds)
                if (detail.AdContent != null)
                    allRelevantWordAds.Add(detail);

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
            string res = "";
            Break lineBreak = new Break();
            //string[] WordAdToPrint = wordAdToPrint.ToArray();
            foreach (string item in wordAdToPrint)
            {
                if (item.IndexOf(" ") == -1)
                    res += item + (char)13 + (char)10;// "\r\n";
                else
                    res += item + "\n";

            }

            /////////////////////////////////////////
            string t = res;//AllAdWordsByOneString(new DateTime(2023, 07, 25));

            //keyValues.Add("Title", t);
            keyValues.Add("myAdsWords", "");
            SearchAndReplaceLike(mainPart!.Document, keyValues);
        }

        public void CreateOneDocAndCopyToDest(string sourceCopy, string dest)
        {
            using (WordprocessingDocument myDoc = WordprocessingDocument.Open(sourceCopy, true))
            {

                myDoc.MainDocumentPart.Document.Save();
                myDoc.Close();
                using (WordprocessingDocument myDestDoc = WordprocessingDocument.Open(dest, true))
                {
                    var mainDestPart = myDestDoc.MainDocumentPart;

                    Paragraph PagemainDestPartBreakParagraph = new Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Break() { Type = BreakValues.Page }));
                    mainDestPart.Document.Body.Append(PagemainDestPartBreakParagraph);

                    string altChunkId = "AltChunkId1"; ;
                    AlternativeFormatImportPart chunk = mainDestPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.WordprocessingML, altChunkId);
                    using (FileStream fileStream = File.Open(sourceCopy, FileMode.Open))
                    {
                        chunk.FeedData(fileStream);
                    }
                    AltChunk altChunk = new AltChunk();
                    altChunk.Id = altChunkId;

                    mainDestPart.Document.Body.InsertAfter(altChunk, mainDestPart.Document.Body.Elements<Paragraph>().Last());

                    myDestDoc.MainDocumentPart.Document.Save();
                    myDestDoc.Close();
                }

            }
        }

        // פונקציה שמקבלת קובץ ומילון של בוקמרקים ומחליפה אותם
        public void SearchAndReplaceLike(Document doc, Dictionary<string, string> dict)
        {
            var allParas = doc.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>();

            RunProperties runProperties = new RunProperties();
            Color color = new Color() { Val = "FF0000" }; // Replace "FF0000" with your desired color code
            runProperties.Append(color);

            // Create a new Break element to represent the line break
            Break lineBreak = new Break();

            // Add the line break after the replaced text
            foreach (Text item in allParas)
            {
                foreach (KeyValuePair<string, string> itm in dict)
                {
                    if (item.Text.Trim().Contains(itm.Key.Trim()))
                    {
                        string rText = item.Text.Replace(itm.Key.Trim(), itm.Value.Trim());
                        Run run = new Run();
                        run.Append(runProperties);
                        run.Append(new Text(rText));

                        run.Append(lineBreak);
                        item.Parent.ReplaceChild(run, item);
                        //item.Space = SpaceProcessingModeValues.Preserve;
                        item.Text = rText + "\r\n";// item.Text.Replace(itm.Key.Trim(), itm.Value.Trim());

                    }
                }
            }

        }

        #endregion

        #region CreateWordsAdByOpenXml

        /// <summary>
        /// CreateWordAd - מקבלת שם של קובץ dotx ואותו היא מעתיקה
        /// ומכניסה לקובץץ ההמועתק את המודעות מילים בצורה מעוצבת
        /// </summary>
        /// <param name="fullname">שם של הקובץ טמפליט</param>
        /// <param name="path">נייוט לתקיה בה תיווצר תקיה חדשה בשם temp</param>

        public void CreateWordAd(string fullname, string path)
        {
            string tempPath = path + @"\temp";
            if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);
            string tempFileFullName = path + @"\temp\temp.dotx";
            File.Copy(fullname, tempFileFullName, true);
            WriteToWordAd(tempFileFullName);
        }

        private void WriteToWordAd(string tempFileFullName)
        {
            DateTime dateOfPrint = new DateTime(2023, 07, 25);

            List<DatesForOrderDetail> allDates = _datesForOrderDetailActions.GetAllDatesForOrderDetails();
            List<OrderDetail> relevanteAds = new List<OrderDetail>();
            foreach (var date in allDates)
                if (date.Date == dateOfPrint)
                    relevanteAds.Add(date.Details);

            List<OrderDetail> allRelevantWordAds = new List<OrderDetail>();

            foreach (OrderDetail detail in relevanteAds)
                if (detail.AdContent != null)
                    allRelevantWordAds.Add(detail);

            //רשימה של כל תתי הקטגוריות
            List<WordAdSubCategoryDTO> categories = GetAllWordAdSubCategories();
            //רשימה של כל פרטי ההזמנה ממוינים לפי קטגוריות
            List<OrderDetail> allDetailsWordAds = new List<OrderDetail>();
            //רשימה שתכיל תת קטגוריה ומיד אח"כ את כל המודעות שלה
            List<string> wordAdToPrint = new List<string>();
            foreach (WordAdSubCategoryDTO category in categories)
            {
                //wordAdToPrint.Add(category.WordCategoryName);
                using (WordprocessingDocument myDestDoc = WordprocessingDocument.Open(tempFileFullName, true))
                {
                    MainDocumentPart mainPart = myDestDoc.MainDocumentPart;

                    Body body = mainPart.Document.Body;

                    Paragraph newParagraph = new Paragraph();
                    ParagraphProperties paragraphProperties = new ParagraphProperties();

                    Justification justification = new Justification() { Val = JustificationValues.Center };
                    paragraphProperties.Append(justification);

                    Shading shading = new Shading() { Val = ShadingPatternValues.Solid, Color = "C435FF" };
                    paragraphProperties.Append(shading);

                    RunProperties runProperties = new RunProperties();

                    Bold bold = new Bold();
                    runProperties.Append(bold);

                    Color color = new Color() { Val = "FFFFFF" }; // For example, red color
                    runProperties.Append(color);

                    Text t = new Text(category.WordCategoryName);

                    Run run = new Run();
                    run.Append(runProperties);
                    run.Append(t);

                    newParagraph.Append(paragraphProperties);
                    newParagraph.Append(run);
                    body.Append(newParagraph);

                    mainPart.Document.Save();

                }
                foreach (OrderDetail detail in allRelevantWordAds)
                    if (detail.WordCategoryId == category.WordCategoryId)
                    {
                        using (WordprocessingDocument myDestDoc = WordprocessingDocument.Open(tempFileFullName, true))
                        {
                            MainDocumentPart mainPart = myDestDoc.MainDocumentPart;

                            Body body = mainPart.Document.Body;

                            Paragraph newParagraph = new Paragraph();
                            ParagraphProperties paragraphProperties = new ParagraphProperties();

                            Justification justification = new Justification() { Val = JustificationValues.Both };
                            paragraphProperties.Append(justification);

                            Run run = new Run(new Text("• " + detail.AdContent));
                            newParagraph.Append(paragraphProperties);
                            newParagraph.Append(run);

                            body.Append(newParagraph);

                            mainPart.Document.Save();
                        }
                    }
            }
        }

        #endregion

    }
}

