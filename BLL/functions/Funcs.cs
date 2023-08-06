using AutoMapper;
using DAL.Actions.Interfaces;
using DAL.Models;
using DTO.Repository;
using System.Text;
using System;
using GemBox.Document;
using SautinSoft;
// OpenXml
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using DocumentFormat.OpenXml;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;
using System.IO.Packaging;
using Microsoft.Win32.SafeHandles;
using DocumentFormat.OpenXml.Drawing.Charts;
// PdfSharp
using PdfSharp.Pdf.IO;
using PdfSharp;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PageSize = PdfSharp.PageSize;
using System.Collections.Generic;
using BLL.Exceptions;
// iText
//using iText.Kernel.Pdf;
//using iText.Layout;
//using iText.Layout.Element;
//using iText.Layout.Properties;
//using iText.IO.Image;
//using iText.Kernel.Geom;
//using iText.Kernel.Pdf.Canvas;
// Aspose.Pdf
//using Aspose.Pdf;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using Org.BouncyCastle.Asn1.Pkcs;
using Aspose.Pdf.Operators;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Spire.Pdf.Graphics;
using iText.Kernel.Pdf.Xobject;
using PdfSharp.Pdf.Advanced;
using System.Drawing;

namespace BLL.Functions
{
    public class Funcs : IFuncs
    {
        static IMapper _Mapper;
        private IConfiguration _config;

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
            INewspapersPublishedActions newspapersPublished,
            IConfiguration config)
        {
            _adSize = adSize;
            _ordersDetailActions = ordersDetailActions;
            _datesForOrderDetailActions = datesForOrderDetailActions;
            _adPlacement = adPlacement;
            _wpAdSubCategoryActions = wpAdSubCategoryActions;
            _customerActions = customerActions;
            _order = order;
            _newspapersPublished = newspapersPublished;
            _config = config;
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

        public CustomerDTO SignUp(CustomerDTO customer)
        {
            CustomerDTO custFind = GetAllCustomers().Where(x => x.CustEmail.Equals(customer.CustEmail)).FirstOrDefault();
            if (custFind != null)
                throw new UserAlreadyExistsException("Email of user already exists in the system!");
            Customer custToAdd = _Mapper.Map<CustomerDTO, Customer>(customer);
            _customerActions.AddNewCustomer(custToAdd);
            CustomerDTO newCust = _Mapper.Map<Customer, CustomerDTO>(custToAdd);
            return newCust;
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

        // מיון מהגדול לקטן
        private List<OrderDetail> SortBySizeDesc(List<OrderDetail> orderDetails)
        {
            return orderDetails.OrderByDescending(x => x.Size?.SizeHeight)
                .ThenByDescending(x => x.Size?.SizeWidth)
                .ToList();
        }

        // מיון מהקטן לגדול
        private List<OrderDetail> SortBySize(List<OrderDetail> orderDetails)
        {
            return orderDetails.OrderBy(x => x.Size?.SizeHeight)
                .ThenBy(x => x.Size?.SizeWidth)
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

        private bool IsMatFull(string[,] mat)
        {
            for (int i = 0; i < mat.GetLength(0); i++)
                for (int j = 0; j < mat.GetLength(1); j++)
                    if (mat[i, j] == null)
                        return false;
            return true;
        }

        private bool IsListOfMatsFull(string[][,] mat)
        {
            for (int i = 0; i < mat.GetLength(0); i++)
                if (!IsMatFull(mat[i]))
                    return false;
            return true;
        }

        private bool IsInsertedFileOnPage(string[,] mat, bool isInserted, OrderDetail currentDetail, int widthImage, int heightImage, PdfPage page, XGraphics gfx)
        {
            bool isEmpty;
            for (int i = 0; i < mat.GetLength(0) && !isInserted; i = i + widthImage)
            {
                for (int j = 0; j < mat.GetLength(1) && !isInserted; j = j + heightImage)
                {
                    isEmpty = true;
                    for (int q = i; q < i + widthImage && isEmpty; q++)
                        for (int l = j; l < j + heightImage && isEmpty; l++)
                            isEmpty = mat[q, l] == null;

                    if (isEmpty)
                    {
                        DrawImageOnPage(page, gfx, mat, currentDetail, i, j);
                        isInserted = true;
                    }
                }
            }
            return isInserted;
        }

        private void FillMat(string[,] mat, int sc, int sr, int col, int row, string nameOfImage)
        {
            for (int i = sc; i < sc + col; i++)
                for (int j = sr; j < sr + row; j++)
                    mat[i, j] = nameOfImage;
        }

        private void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y, int width, int height)
        {
            XImage image = XImage.FromFile("C:\\yael\\final_project\\newspaperProject\\server\\newspaper\\newspaper\\wwwroot\\Upload\\" + jpegSamplePath);
            gfx.DrawImage(image, x, y, width, height);
        }

        private void DrawImageOnPage(PdfPage page, XGraphics gfx, string[,] matPage, OrderDetail detail, int i, int j)
        {
            int widthImage = (int)detail.Size!.SizeWidth;
            int heightImage = (int)detail.Size!.SizeHeight;
            int w = ((int)page.Width - 15) / 4;
            int h = ((int)page.Height - 32) / 8;
            DrawImage(gfx, detail.AdFile!, 16 + w * i, 16 + h * j, w * widthImage - 16, h * heightImage - 16);
            FillMat(matPage, i, j, widthImage, heightImage, detail.AdFile!);
        }

        private void Inlay(string first, string regular, string words, List<OrderDetail> placeCoverFileAds, List<OrderDetail> placeBackFileAds, List<OrderDetail> placeNormalFileAds)
        {
            int rows = 8;
            int cols = 4;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            List<string[,]> pagesMats = new List<string[,]>();
            PdfDocument document = PdfReader.Open(first, PdfDocumentOpenMode.Modify);
            PdfDocument newDocument = PdfReader.Open(regular, PdfDocumentOpenMode.Import);
            PdfPage page = document.Pages[0];
            PdfPage tempPage = newDocument.Pages[0];
            pagesMats.Add(new string[cols, rows]);
            XGraphics gfx = XGraphics.FromPdfPage(page);
            List<XGraphics> xGraphicss = new List<XGraphics> { gfx };
            FillMat(pagesMats[0], 0, 0, cols, (int)(rows * 0.25), "titleImage");
            List<OrderDetail> tempListToInsert = placeCoverFileAds;
            string[,] backMat = new string[cols, rows];
            OrderDetail currentDetail;
            List<OrderDetail> anyFilesToInsertToBack = new List<OrderDetail>();
            bool isInserted = false;
            bool isEmpty;
            string[,] newPage;
            string[,] matPage;
            for (int index = 0; index < placeBackFileAds.Count; index++)
            {
                isInserted = false;
                currentDetail = placeBackFileAds[index];
                int widthImage = (int)currentDetail.Size!.SizeWidth;
                int heightImage = (int)currentDetail.Size!.SizeHeight;
                for (int i = 0; i < backMat.GetLength(0) && !isInserted; i = i + widthImage)
                {
                    for (int j = 0; j < backMat.GetLength(1) && !isInserted; j = j + heightImage)
                    {
                        isEmpty = true;
                        for (int q = i; q < i + widthImage && isEmpty; q++)
                            for (int l = j; l < j + heightImage && isEmpty; l++)
                                isEmpty = backMat[q, l] == null;
                        if (isEmpty)
                        {
                            anyFilesToInsertToBack.Add(currentDetail);
                            FillMat(backMat, i, j, widthImage, heightImage, currentDetail.AdFile!);
                            isInserted = true;
                        }
                    }
                }
                if (!isInserted)
                    placeNormalFileAds.Add(currentDetail);
            }
            for (int index = 0; index < tempListToInsert.Count; index++)
            {
                isInserted = false;
                currentDetail = tempListToInsert[index];
                int widthImage = (int)currentDetail.Size!.SizeWidth;
                int heightImage = (int)currentDetail.Size!.SizeHeight;
                for (int k = 0; k < pagesMats.Count && !isInserted; k++)
                {
                    matPage = pagesMats[k];
                    gfx = xGraphicss[k];
                    page = document.Pages[k];
                    isInserted = IsInsertedFileOnPage(matPage, isInserted, currentDetail, widthImage, heightImage, page, gfx);
                }

                if (!isInserted)
                {
                    if (page == document.Pages[0] && tempListToInsert == placeCoverFileAds)
                    {
                        for (int i = index; i < placeCoverFileAds.Count; i++)
                            placeNormalFileAds.Add(placeCoverFileAds[i]);
                        tempListToInsert = SortBySizeDesc(placeNormalFileAds);
                        index = -1;
                        continue;
                    }
                    newPage = new string[4, 8];
                    document.AddPage(tempPage);
                    page = document.Pages[document.Pages.Count - 1];
                    gfx = XGraphics.FromPdfPage(page);
                    xGraphicss.Add(gfx);
                    DrawImageOnPage(page, gfx, newPage, currentDetail, 0, 0);
                    pagesMats.Add(newPage);
                }
                else if (index == tempListToInsert.Count - 1)
                {
                    if (tempListToInsert == placeCoverFileAds)
                    {
                        for (int i = index + 1; i < placeCoverFileAds.Count; i++)
                            placeNormalFileAds.Add(placeCoverFileAds[i]);
                        index = -1;
                        tempListToInsert = SortBySizeDesc(placeNormalFileAds);
                    }
                    else if (tempListToInsert == placeNormalFileAds)
                    {
                        tempListToInsert = anyFilesToInsertToBack;
                        index = -1;
                        newPage = new string[4, 8];
                        document.AddPage(tempPage);
                        page = document.Pages[document.Pages.Count - 1];
                        gfx = XGraphics.FromPdfPage(page);
                        xGraphicss.Add(gfx);
                        pagesMats.Add(newPage);
                    }
                }
            }

            // הוספת העמוד האחורי
            newPage = new string[4, 8];
            document.AddPage(tempPage);
            page = document.Pages[document.Pages.Count - 1];
            gfx = XGraphics.FromPdfPage(page);
            xGraphicss.Add(gfx);
            pagesMats.Add(newPage);
            for (int index = 0; index < anyFilesToInsertToBack.Count; index++)
            {
                isInserted = false;
                currentDetail = anyFilesToInsertToBack[index];
                int widthImage = (int)currentDetail.Size!.SizeWidth;
                int heightImage = (int)currentDetail.Size!.SizeHeight;
                isInserted = IsInsertedFileOnPage(newPage, isInserted, currentDetail, widthImage, heightImage, page, gfx);
            }
            document.Save(first);
        }

        public void Shabets(string pathPdf)
        {
            // זה בקיצור שליפת כל פרטי ההזמנות הרלונטיות
            List<DatesForOrderDetail> allDates = _datesForOrderDetailActions.GetAllDatesForOrderDetails();
            List<OrderDetail> relevanteAds = new List<OrderDetail>();
            foreach (var date in allDates)
                if (date.Date == new DateTime(2023, 08, 08) ||
                    date.Date.AddDays(((double)date.Details.AdDuration - 1) * 7) >= new DateTime(2023, 08, 08))
                    relevanteAds.Add(date.Details);

            // הגדרת רשימות של פרסומת
            List<OrderDetail> allRelevantFileAds = new List<OrderDetail>();

            // מיון כל ההזמנות הרלונטיות לפרסומות
            foreach (OrderDetail detail in relevanteAds)
                if (detail.AdFile != null)
                    allRelevantFileAds.Add(detail);

            List<OrderDetail> placeCoverFileAds = new List<OrderDetail>();
            List<OrderDetail> placeBackFileAds = new List<OrderDetail>();
            List<OrderDetail> placeNormalFileAds = new List<OrderDetail>();
            List<OrderDetail> managerFiles = new List<OrderDetail>();
            foreach (OrderDetail detail in allRelevantFileAds)
                if (detail.Order!.Cust.CustEmail == _config["ManagerEmail"])
                    managerFiles.Add(detail);
                else if (detail.PlaceId == 1)
                    placeCoverFileAds.Add(detail);
                else if (detail.PlaceId == 2)
                    placeBackFileAds.Add(detail);
                else if (detail.PlaceId == 3)
                    placeNormalFileAds.Add(detail);

            // מציאת הניתוב הרלוונטי ושם העיתון הנוכחי
            // שליפת כל העיתונים שיצאיו עד כה
            List<NewspapersPublished> allNewpapers = _newspapersPublished.GetAllNewspapersPublished();
            // נתינת שם לעיתון עפי הקוד האחרון + 1
            int newspaperId = allNewpapers.Max(x => x.NewspaperId) + 1;
            // word ו pdf נתינת ניתוב לתיקיית עיתונים והגדרת ניתובים ל 
            string regular = myPath + "\\regularTemplate" + ".pdf";
            string regularWord = myPath + "\\regularTemplate" + ".dotx";
            string first = myPath + "\\firstTemplate" + ".pdf";
            string firstWord = myPath + "\\firstTemplate" + ".dotx";
            string words = myPath + "\\wordsTemplate" + ".pdf";
            string wordsWord = myPath + "\\wordsTemplate" + ".dotx";

            // מיון המודעות בקצרה
            allRelevantFileAds = SortBySizeDesc(allRelevantFileAds);

            ConvertFromWordToPdf(regularWord, regular);
            ConvertFromWordToPdf(firstWord, first);
            ConvertFromWordToPdf(wordsWord, words);

            Inlay(first, regular, words, SortBySize(placeCoverFileAds), SortBySizeDesc(placeBackFileAds), SortBySizeDesc(placeNormalFileAds));

            ConvertPdfToImages(first, "C:\\yael\\final_project\\newspaperProject\\server\\newspaper\\newspaper\\wwwroot\\Newspapers\\08.08.2023");
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

        public void ConvertPdfToImages(string pdfFilePath, string pathImages)
        {
            //Create a PdfDocument instance
            Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();

            //Load a sample PDF document
            pdf.LoadFromFile(pdfFilePath);

            //Loop through each page in the PDF
            for (int i = 0; i < pdf.Pages.Count; i++)
            {
                //Convert all pages to images and set the image Dpi
                System.Drawing.Image image = pdf.SaveAsImage(i, Spire.Pdf.Graphics.PdfImageType.Bitmap, 500, 500);

                //Save images as PNG format to a specified folder
                string file = String.Format(pathImages + "\\{0}.png", i);
                image.Save(file, System.Drawing.Imaging.ImageFormat.Png);

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

        // פונקציה שמקבלת רשימה של תאריכים ורשימה של קודים של פרטי הזמנות ומכניסה למסד הנתונים
        private void EnterDates(List<DateTime> listDates, List<int> orderDetailsIds)
        {
            DatesForOrderDetail datesForOrderDetail;
            for (int i = 0; i < orderDetailsIds.Count; i++)
            {
                datesForOrderDetail = new DatesForOrderDetail();
                datesForOrderDetail.Date = listDates[i];
                datesForOrderDetail.DetailsId = orderDetailsIds[i];
                datesForOrderDetail.DateStatus = false;
                _datesForOrderDetailActions.AddNewDateForOrderDetail(datesForOrderDetail);
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

        // פונקציה שמקבלת לקוח, מערך של פרטי הזמנות ומערך של תאריכים
        // הפונקציה מכניסה למסד הנתונים הזמנה וכל מה שהיא קיבלה
        public void FinishOrder(CustomerDTO customer, List<DateTime> listDates, List<OrderDetailDTO> listOrderDetails)
        {
            List<OrderDetail> orderDetails = FromListOrderDetailDTOToListOrderDetail(listOrderDetails).ToList();
            DAL.Models.Order newOrder = new DAL.Models.Order()
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

        private int CountWords(string sentence)
        {
            sentence = sentence.Trim();
            if (string.IsNullOrEmpty(sentence))
                return 0;
            string[] words = sentence.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }

        public void FinishOrderAdWords(CustomerDTO customer, List<DateTime> listDates, List<OrderDetailDTO> listOrderDetails)
        {
            List<OrderDetail> orderDetails = FromListOrderDetailDTOToListOrderDetail(listOrderDetails).ToList();
            DAL.Models.Order newOrder = new DAL.Models.Order()
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
        public void SearchAndReplaceLike(DocumentFormat.OpenXml.Wordprocessing.Document doc, Dictionary<string, string> dict)
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
            DateTime dateOfPrint = new DateTime(2023, 08, 08);

            List<DatesForOrderDetail> allDates = _datesForOrderDetailActions.GetAllDatesForOrderDetails();
            List<OrderDetail> relevanteAds = new List<OrderDetail>();
            foreach (var date in allDates)
                if (date.Date == dateOfPrint)
                    relevanteAds.Add(date.Details);

            List<OrderDetail> allRelevantWordAds = new List<OrderDetail>();

            foreach (OrderDetail detail in relevanteAds)
                if (detail.AdContent != null)
                    allRelevantWordAds.Add(detail);

            List<WordAdSubCategoryDTO> categories = GetAllWordAdSubCategories();

            List<OrderDetail> allDetailsWordAds = new List<OrderDetail>();

            List<string> wordAdToPrint = new List<string>();

            foreach (WordAdSubCategoryDTO category in categories)
            {
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

        //#region IText

        //private void DrawImageOnPage(iText.Layout.Document doc, float widthPage, float heightPage, int widthImage, int heightImage, string[,] matPage, string ImagePath, int i, int j)
        //{

        //    float w = (widthPage - 16) / 4;
        //    float h = (heightPage - 48) / 8;

        //    Image image = new Image(ImageDataFactory.Create("C:\\yael\\final_project\\newspaperProject\\server\\newspaper\\newspaper\\wwwroot\\Upload\\" + ImagePath));

        //    // הגדרת המיקום והגודל בפיקסלים (x, y, רוחב, גובה)
        //    float x = 16 + w * i;
        //    float y = 16 + h * j;
        //    float width = w * widthImage - 16;
        //    float height = h * heightImage - 16;

        //    // הגדרת המיקום והגודל בעליון-ימין של העמוד
        //    image.SetFixedPosition(x, y, width);

        //    doc.Add(image);

        //    for (int q = i; q < i + widthImage; q++)
        //        for (int l = j; l < j + heightImage; l++)
        //            matPage[q, l] = ImagePath;

        //}

        //private void Create(string filename, List<OrderDetail> relevantOrders)
        //{

        //    // הגדרה של עיתון חדש
        //    List<string[,]> pagesMats = new List<string[,]>();
        //    PdfDocument pdfDoc = new PdfDocument(new PdfWriter(new FileStream(filename, FileMode.Create, FileAccess.Write)));
        //    iText.Layout.Document doc = new iText.Layout.Document(pdfDoc);

        //    // הוספת עמוד אחד למטריצת העיתון
        //    pagesMats.Add(new string[4, 8]);
        //    int ggg = pdfDoc.GetNumberOfPages();
        //    PdfPage tempPage = pdfDoc.GetFirstPage();
        //    pdfDoc.AddPage(tempPage);
        //    pdfDoc.SetPage(1, new PdfPage());

        //    float heightPage = pdfDoc.GetDefaultPageSize().GetHeight();
        //    float widthPage = pdfDoc.GetDefaultPageSize().GetWidth();


        //    // עוברים על על המודעות הרלונטיות ומכניסים לרשימת העיתון
        //    foreach (OrderDetail detail in relevantOrders)
        //    {
        //        // משתנה שאומר האם היה מקום למודעה
        //        bool Shubatz = false;
        //        // רוחב התמונה הנוכחית
        //        if (detail.Size != null)
        //        {
        //            int Width = (int)(detail.Size.SizeWidth);
        //            int Height = (int)(detail.Size.SizeHeight);
        //            for (int k = 0; k < pagesMats.Count; k++)
        //            {
        //                // הכנסת העמוד הנוכחי למטריצת עזר חדשה
        //                string[,] matPage = pagesMats[k];

        //                bool isEmpty;

        //                for (int i = 0; i < matPage.GetLength(0) && !Shubatz; i = i + Width)
        //                {
        //                    for (int j = 0; j < matPage.GetLength(1) && !Shubatz; j = j + Height)
        //                    {
        //                        isEmpty = true;
        //                        // אם המשבצת הראשונה ריקה צריך לודא שכל המשבצות ריקות
        //                        if (matPage[i, j] == null)
        //                        {
        //                            for (int q = i; q < i + Width && isEmpty; q++)
        //                                for (int l = j; l < j + Height && isEmpty; l++)
        //                                    // אם המקום תופוס
        //                                    if (matPage[q, l] != null)
        //                                        isEmpty = false;
        //                        }
        //                        // אם המשבצת הראשונה מלאה בטוח שאין מקום במיקום זה
        //                        else
        //                            isEmpty = false;
        //                        // בדיקה אם אכן יש מקום למודעה - אז להוסיף את המודעה
        //                        if (isEmpty)
        //                        {
        //                            DrawImageOnPage(doc, widthPage, heightPage, Width, Height, matPage, detail.AdFile, i, j);
        //                            Shubatz = true;
        //                        }
        //                    }
        //                }
        //            }

        //            // במקרה שהשיבוץ באחד מהדפים הקודמים לא הצליח
        //            if (!Shubatz)
        //            {
        //                // יצירת דף חדש במטריצת העיתון
        //                string[,] newPage = new string[4, 8];
        //                pdfDoc.AddPage(tempPage);
        //                // הוספת התמונה לעמוד החדש שבטוח שיש בו מקום
        //                DrawImageOnPage(doc, widthPage, heightPage, Width, Height, newPage, detail.AdFile, 0, 0);
        //                // הוספת העמוד לעיתון ה pdf
        //                pagesMats.Add(newPage);
        //            }
        //        }
        //    }
        //    pdfDoc.Close();
        //    doc.Close();



        //    //List<string[,]> pagesMats = new List<string[,]>();
        //    //pagesMats.Add(new string[4, 8]);
        //    //using (FileStream fs = new FileStream(filename, FileMode.Create))
        //    //{

        //    //    PdfWriter writer = new PdfWriter(fs);
        //    //    PdfDocument pdfDocument = new PdfDocument(writer);
        //    //    PdfPage pdfPage = pdfDocument.AddNewPage();
        //    //    iText.Layout.Document doc = new iText.Layout.Document(pdfDocument);
        //    //    pdfDocument.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A5);
        //    //    float heightPage = pdfDocument.GetDefaultPageSize().GetHeight();
        //    //    float widthPage = pdfDocument.GetDefaultPageSize().GetWidth();

        //    //    doc.SetMargins(0, 0, 0, 0);

        //    //    foreach (OrderDetail detail in relevantOrders)
        //    //    {
        //    //        bool Shubatz = false;
        //    //        if (detail.Size != null)
        //    //        {
        //    //            int widthImage = (int)(detail.Size.SizeWidth);
        //    //            int heightImage = (int)(detail.Size.SizeHeight);
        //    //            for (int k = 0; k < pagesMats.Count; k++)
        //    //            {
        //    //                string[,] matPage = pagesMats[k];

        //    //                bool isEmpty;

        //    //                for (int i = 0; i < matPage.GetLength(0) && !Shubatz; i = i + widthImage)
        //    //                {
        //    //                    for (int j = 0; j < matPage.GetLength(1) && !Shubatz; j = j + heightImage)
        //    //                    {
        //    //                        isEmpty = true;
        //    //                        if (matPage[i, j] == null)
        //    //                        {
        //    //                            for (int q = i; q < i + widthImage && isEmpty; q++)
        //    //                                for (int l = j; l < j + heightImage && isEmpty; l++)
        //    //                                    if (matPage[q, l] != null)
        //    //                                        isEmpty = false;
        //    //                        }
        //    //                        else
        //    //                            isEmpty = false;
        //    //                        if (isEmpty)
        //    //                        {
        //    //                            DrawImageOnPage(pagesMats.Count, doc, widthPage, heightPage, widthImage, heightImage, matPage, detail.AdFile, i, j);
        //    //                            Shubatz = true;
        //    //                        }
        //    //                    }
        //    //                }
        //    //            }

        //    //            if (!Shubatz)
        //    //            {
        //    //                string[,] newPage = new string[4, 8];
        //    //                pdfPage = pdfDocument.AddNewPage();
        //    //                DrawImageOnPage(pagesMats.Count, doc, widthPage, heightPage, widthImage, heightImage, newPage, detail.AdFile, 0, 0);
        //    //                pagesMats.Add(newPage);

        //    //            }
        //    //        }
        //    //    }
        //    //    //// הוספת תמונה מהקובץ המקומי
        //    //    //ImageData imageData = ImageDataFactory.Create(imagePath);
        //    //    //Image image = new Image(imageData);

        //    //    //// גודל התמונה בדף
        //    //    //float imageWidth = 200;
        //    //    //float imageHeight = 100;

        //    //    //// מיקום התמונה בדף (פינה השמאלית התחתונה)
        //    //    //float xPosition = 100;
        //    //    //float yPosition = 100;

        //    //    //// יציב את התמונה במיקום ובגודל שצוין
        //    //    ////image.SetFixedPosition(xPosition, yPosition);
        //    //    //image.SetRelativePosition(0, 0, 0, 0);

        //    //    ////image.ScaleAbsolute(imageWidth, imageHeight);
        //    //    //// הוסף את התמונה למסמך
        //    //    //doc.Add(image);

        //    //    //// סגירת המסמך
        //    //    doc.Close();
        //    //}
        //}

        //public void Shabets(string pathPdf)
        //{
        //    // זה בקיצור שליפת כל פרטי ההזמנות הרלונטיות
        //    List<DatesForOrderDetail> allDates = _datesForOrderDetailActions.GetAllDatesForOrderDetails();
        //    List<OrderDetail> relevanteAds = new List<OrderDetail>();
        //    foreach (var date in allDates)
        //        if (date.Date == new DateTime(2023, 07, 25))
        //            relevanteAds.Add(date.Details);

        //    // הגדרת רשימות של פרסומת
        //    List<OrderDetail> allRelevantFileAds = new List<OrderDetail>();

        //    // מיון כל ההזמנות הרלונטיות לפרסומות
        //    foreach (OrderDetail detail in relevanteAds)
        //        if (detail.AdFile != null)
        //            allRelevantFileAds.Add(detail);

        //    // מציאת הניתוב הרלוונטי ושם העיתון הנוכחי
        //    // שליפת כל העיתונים שיצאיו עד כה
        //    List<NewspapersPublished> allNewpapers = _newspapersPublished.GetAllNewspapersPublished();
        //    // נתינת שם לעיתון עפי הקוד האחרון + 1
        //    int newspaperId = allNewpapers.Max(x => x.NewspaperId) + 1;
        //    // word ו pdf נתינת ניתוב לתיקיית עיתונים והגדרת ניתובים ל 
        //    string PDFpath = myPath + "\\regularTemplate";
        //    string WORDpath = myPath + "\\regularTemplate" + ".dotx";

        //    // מיון המודעות בקצרה
        //    allRelevantFileAds = SortBySize(allRelevantFileAds);

        //    ConvertFromWordToPdf(WORDpath, PDFpath + ".pdf");

        //    Create(PDFpath + ".pdf", allRelevantFileAds);

        //}

        //#endregion

        //#region Aspose.Pdf

        //private void DrawImageOnPage(Page page, int widthImage, int heightImage, string[,] matPage, string imagePath, int i, int j)
        //{
        //    int w = ((int)(page.GetPageRect(false).Width) - 16) / 4;
        //    int h = ((int)(page.GetPageRect(false).Height) - 32) / 8;

        //    string file = "C:\\yael\\final_project\\newspaperProject\\server\\newspaper\\newspaper\\wwwroot\\Upload\\" + imagePath;
        //    using (FileStream imageStream = new FileStream(file, FileMode.Open))
        //    {
        //        // הגדר קואורדינטות
        //        int lowerLeftX = 16 + w * i;
        //        int lowerLeftY = 16 + h * j + (h * heightImage - 16);
        //        int upperRightX = 16 + w * i + (w * widthImage - 16);
        //        int upperRightY = 16 + h * j;

        //        // הוסף תמונה לאוסף תמונות של משאבי דפים
        //        page.Resources.Images.Add(imageStream);

        //        // שימוש באופרטור GSave: אופרטור זה שומר את מצב הגרפיקה הנוכחי
        //        page.Contents.Add(new Aspose.Pdf.Operators.GSave());

        //        // צור אובייקטים של מלבן ומטריצה
        //        Aspose.Pdf.Rectangle rectangle = new Aspose.Pdf.Rectangle(lowerLeftX, lowerLeftY, upperRightX, upperRightY);
        //        Matrix matrix = new Matrix(new double[] { rectangle.URX - rectangle.LLX, 0, 0, rectangle.URY - rectangle.LLY, rectangle.LLX, rectangle.LLY });

        //        // שימוש באופרטור ConcatenateMatrix (מחרוזת שרשור): מגדיר כיצד יש למקם תמונה
        //        page.Contents.Add(new Aspose.Pdf.Operators.ConcatenateMatrix(matrix));
        //        Aspose.Pdf.XImage ximage = page.Resources.Images[page.Resources.Images.Count];

        //        // שימוש באופרטור Do: אופרטור זה מצייר תמונה
        //        page.Contents.Add(new Aspose.Pdf.Operators.Do(ximage.Name));

        //        // שימוש באופרטור GRestore: אופרטור זה משחזר את מצב הגרפיקה
        //        page.Contents.Add(new Aspose.Pdf.Operators.GRestore());

        //    }

        //    //Aspose.Pdf.Image image1 = new Aspose.Pdf.Image();
        //    //page.Paragraphs.Add(image1);
        //    //image1.ImageStream = imageStream;

        //    //    DrawImage(gfx, ImagePath, 16 + w * i, 16 + h * j, w * Width - 16, h * Height - 16);
        //    for (int q = i; q < i + widthImage; q++)
        //        for (int l = j; l < j + heightImage; l++)
        //            matPage[q, l] = imagePath;
        //}

        //private void Create(string filename, List<OrderDetail> relevantOrders)
        //{
        //    // צור מסמך חדש
        //    Aspose.Pdf.Document doc = new Aspose.Pdf.Document(filename + ".pdf");
        //    // הגדרה של עיתון חדש
        //    List<string[,]> pagesMats = new List<string[,]>();
        //    // הוסף עמוד לאוסף דפים של מסמך
        //    Page page = doc.Pages.Add(doc.Pages[1]);
        //    // הוספת עמוד אחד למטריצת העיתון
        //    pagesMats.Add(new string[4, 8]);
        //    // עוברים על על המודעות הרלונטיות ומכניסים לרשימת העיתון
        //    foreach (OrderDetail detail in relevantOrders)
        //    {
        //        bool Shubatz = false;
        //        if (detail.Size != null)
        //        {
        //            int widthImage = (int)(detail.Size.SizeWidth);
        //            int heightImage = (int)(detail.Size.SizeHeight);

        //            for (int k = 0; k < pagesMats.Count; k++)
        //            {
        //                // הכנסת העמוד הנוכחי למטריצת עזר חדשה
        //                string[,] matPage = pagesMats[k];

        //                bool isEmpty;

        //                for (int i = 0; i < matPage.GetLength(0) && !Shubatz; i = i + widthImage)
        //                {
        //                    for (int j = 0; j < matPage.GetLength(1) && !Shubatz; j = j + heightImage)
        //                    {
        //                        isEmpty = true;
        //                        // אם המשבצת הראשונה ריקה צריך לודא שכל המשבצות ריקות
        //                        if (matPage[i, j] == null)
        //                        {
        //                            for (int q = i; q < i + widthImage && isEmpty; q++)
        //                                for (int l = j; l < j + heightImage && isEmpty; l++)
        //                                    // אם המקום תופוס
        //                                    if (matPage[q, l] != null)
        //                                        isEmpty = false;
        //                        }
        //                        // אם המשבצת הראשונה מלאה בטוח שאין מקום במיקום זה
        //                        else
        //                            isEmpty = false;
        //                        // בדיקה אם אכן יש מקום למודעה - אז להוסיף את המודעה
        //                        if (isEmpty)
        //                        {
        //                            DrawImageOnPage(page, widthImage, heightImage, matPage, detail.AdFile, i, j);
        //                            Shubatz = true;
        //                        }
        //                    }
        //                }
        //            }

        //            // במקרה שהשיבוץ באחד מהדפים הקודמים לא הצליח
        //            if (!Shubatz)
        //            {
        //                // יצירת דף חדש במטריצת העיתון
        //                string[,] newPage = new string[4, 8];
        //                // הוסף עמוד לאוסף דפים של מסמך
        //                page = doc.Pages.Add(doc.Pages[1]);
        //                // הוספת התמונה לעמוד החדש שבטוח שיש בו מקום
        //                DrawImageOnPage(page, widthImage, heightImage, newPage, detail.AdFile, 0, 0);
        //                // הוספת העמוד לעיתון ה pdf
        //                pagesMats.Add(newPage);
        //            }
        //        }
        //    }
        //    doc.SetTitle("hello");
        //    doc.Save(filename + "my.pdf", SaveFormat.Pdf);
        //}

        //#endregion
    }
}

