using AutoMapper;
using DAL.Actions.Interfaces;
using DAL.Models;
using DTO.Repository;
using System.Text;
using System;
using GemBox.Document;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using BLL.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.Net.Mail;
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using DocumentFormat.OpenXml.Bibliography;
using System.Globalization;
using Azure;
using System.Text.Json;
using BLL.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using MimeKit.Utils;
using MimeKit;
using System.Net.Mime;
using BLL.Jwt;

namespace BLL.Functions
{
    public class Funcs : IFuncs
    {
        static IMapper _Mapper;
        private IConfiguration _config;

        private string pathWwwroot, myPath, upload;
        private string GHOSTSCRIPT_DLL_PATH = "C:\\Program Files\\gs\\gs10.02.0\\bin\\gsdll64.dll";

        static Funcs()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DTO.AutoMap>();
            });
            _Mapper = config.CreateMapper();
        }

        private readonly IAdSizeActions _adSize;
        private readonly IOrderDetailActions _ordersDetailActions;
        private readonly IDatesForOrderDetailActions _datesForOrderDetailActions;
        private readonly IAdPlacementActions _adPlacement;
        private readonly IWordAdSubCategoryActions _wpAdSubCategoryActions;
        private readonly ICustomerActions _customerActions;
        private readonly IOrderActions _order;
        private readonly INewspapersPublishedActions _newspapersPublished;
        private readonly ICacheRedis _cacheRedis;
        private readonly IJwtService _jwtService;
        private readonly IWebHostEnvironment _environment;


        public Funcs(IAdSizeActions adSize,
            IOrderDetailActions ordersDetailActions,
            IDatesForOrderDetailActions datesForOrderDetailActions,
            IAdPlacementActions adPlacement,
            IWordAdSubCategoryActions wpAdSubCategoryActions,
            ICustomerActions customerActions,
            IOrderActions order,
            INewspapersPublishedActions newspapersPublished,
            ICacheRedis cacheRedis,
            IConfiguration config,
            IJwtService jwtService,
            IWebHostEnvironment environment)
        {
            _adSize = adSize;
            _ordersDetailActions = ordersDetailActions;
            _datesForOrderDetailActions = datesForOrderDetailActions;
            _adPlacement = adPlacement;
            _wpAdSubCategoryActions = wpAdSubCategoryActions;
            _customerActions = customerActions;
            _order = order;
            _newspapersPublished = newspapersPublished;
            _cacheRedis = cacheRedis;
            _config = config;
            _jwtService = jwtService;
            _environment = environment;
            pathWwwroot = environment.WebRootPath;
            myPath = pathWwwroot + "\\TempWord";
            GHOSTSCRIPT_DLL_PATH = "C:\\Program Files\\gs\\gs10.02.0\\bin\\gsdll64.dll";
            upload = pathWwwroot + "\\Upload\\";
        }

        #region WpAdSubCategory

        public List<WordAdSubCategoryDTO> GetAllWordAdSubCategories()
        {
            List<WordAdSubCategory> listwordAdSubCategories = _wpAdSubCategoryActions.GetAllWordAdSubCategories();
            List<WordAdSubCategoryDTO> wordAdSubCategoryDTO = _Mapper.Map<List<WordAdSubCategoryDTO>>(listwordAdSubCategories);
            return wordAdSubCategoryDTO;
        }

        #endregion

        #region AdSize

        public List<AdSizeDTO> GetAllAdSize()
        {
            List<AdSize> listAdSize = _adSize.GetAllAdSizes();
            List<AdSizeDTO> adSizeDTO = _Mapper.Map<List<AdSizeDTO>>(listAdSize);
            return adSizeDTO;
        }

        private decimal GetPriceBySizeId(int id)
        {
            return _adSize.GetSizeById(id).SizePrice;
        }
      
        #endregion

        #region AdPlacement

        public List<AdPlacementDTO> GetAllAdPlacement()
        {
            List<AdPlacement> listAdPlacement = _adPlacement.GetAllAdPlacements();
            List<AdPlacementDTO> adPlacementDTO = _Mapper.Map<List<AdPlacementDTO>>(listAdPlacement);
            return adPlacementDTO;
        }

        private decimal GetPriceByPlaceId(int id)
        {
            return _adPlacement.GetPlacementById(id).PlacePrice;
        }

        #endregion

        #region Customer

        // פונקציה שמקבלת לקוח ומחזירה את הקוד שלו
        // ואם הוא לא קיים היא מוסיפה אותו ומחזירה את הקוד שלו
        public int GetIdByCustomer(CustomerDTO customer)
        {
            var newCust = GetAllCustomers().Where(x => x.CustEmail.Equals(customer.CustEmail)).FirstOrDefault(c => c.CustPassword.Equals(customer.CustPassword));
            if (newCust != null)
                return newCust.CustId;
            Customer custToAdd = _Mapper.Map<CustomerDTO, Customer>(customer);
            _customerActions.AddNewCustomer(custToAdd);
            return custToAdd.CustId;
        }

        public string SignUp(CustomerDTO customer, bool isRegistered)
        {
            CustomerDTO custFind = GetAllCustomers().Where(x => x.CustEmail.Equals(customer.CustEmail)).FirstOrDefault()!;
            if (custFind != null)
                throw new UserAlreadyExistsException("Email of user already exists in the system!");
            Customer custToAdd = _Mapper.Map<CustomerDTO, Customer>(customer);
            _customerActions.AddNewCustomer(custToAdd);
            _cacheRedis.SetItem(new Item()
            {
                Email = customer.CustEmail,
                IsRegistered = isRegistered,
            });
            CustomerDTO newCust = _Mapper.Map<Customer, CustomerDTO>(custToAdd);
            string token = _jwtService.CreateToken(newCust);
            return token;
        }

        private List<CustomerDTO> GetAllCustomers()
        {
            List<Customer> customers = _customerActions.GetAllCustomers();
            List<CustomerDTO> customersDTO = _Mapper.Map<List<CustomerDTO>>(customers);
            return customersDTO;
        }

        public string LogIn(string email, string pass)
        {
            var cust = GetAllCustomers().FirstOrDefault(x => x.CustEmail.Equals(email) && x.CustPassword.Equals(pass));
            if (cust == null)
                throw new UserNotFoundException();
            string token = _jwtService.CreateToken(cust);
            return token;
        }

        public bool IsAdmin(string token)
        {
            string email = _jwtService.GetEmailFromToken(token);
            string password = _jwtService.GetPasswordFromToken(token);
            Console.WriteLine(_config["ManagerEmail"]);
            return email.Equals(_config["ManagerEmail"]) && password.Equals(_config["ManagerPassword"]);
        }
        #endregion

        #region NewspapersPublished

        public List<NewspapersPublishedDTO> GetAllNewspapersPublished()
        {
            List<NewspapersPublished> newspapersPublished = _newspapersPublished.GetAllNewspapersPublished();
            List<NewspapersPublishedDTO> NewspapersPublishedDTO = _Mapper.Map<List<NewspapersPublishedDTO>>(newspapersPublished);
            return NewspapersPublishedDTO.OrderByDescending(x => x.NewspaperId).ToList();
        }

        #endregion

        #region OrderDetail

        private List<OrderDetail> FromListOrderDetailDTOToListOrderDetail(List<OrderDetailDTO> listOrderDetails)
        {
            List<OrderDetail> orderDetails = _Mapper.Map<List<OrderDetail>>(listOrderDetails);
            return orderDetails;
        }

        public List<OrderDetailDTO> GetAllOrderDetails()
        {
            List<OrderDetail> details = _ordersDetailActions.GetAllOrderDetails();
            List<OrderDetailDTO> orderDetailsDTO = _Mapper.Map<List<OrderDetailDTO>>(details);
            return orderDetailsDTO;
        }

        private List<OrderDetail> SortBySizeDesc(List<OrderDetail> orderDetails)
        {
            return orderDetails.OrderByDescending(x => x.Size?.SizeHeight)
                .ThenByDescending(x => x.Size?.SizeWidth)
                .ToList();
        }

        private List<OrderDetail> SortBySize(List<OrderDetail> orderDetails)
        {
            return orderDetails.OrderBy(x => x.Size?.SizeHeight)
                .ThenBy(x => x.Size?.SizeWidth)
                .ToList();
        }

        private List<OrderDetail> GetAllReleventOrders(DateTime dateForPrint)
        {
            List<DatesForOrderDetail> allDates = _datesForOrderDetailActions.GetAllDatesForOrderDetails();
            var relevanteAds = allDates
                .Where(d => d.Date == dateForPrint || d.Details!.Order!.Cust.CustEmail == _config["ManagerEmail"]
                || (d.Date < dateForPrint && d.Date.AddDays(((double)(d.Details.AdDuration!) - 1) * 7) >= dateForPrint))
                .Where(d => d.ApprovalStatus == true)
                .Select(x => x.Details);
            return relevanteAds.ToList()!;
        }

        public decimal CalculationOfOrderPrice(List<OrderDetailDTO> listOrderDetails)
        {
            decimal sum = listOrderDetails.Sum(x => GetPriceBySizeId(x.SizeId ?? 0) * (decimal)x.AdDuration);
            return sum;
        }

        public decimal CalculationOfOrderWordsPrice(List<OrderDetailDTO> listOrderDetails)
        {
            decimal sum = listOrderDetails.Sum(x => CountWords(x.AdContent) * (decimal)x.AdDuration);
            return sum;
        }

        #endregion

        #region Table

        public Object GetAllOrderDetailsTableByDate(DateTime dateForPrint, int page, int itemsPerPage)
        {
            List<DatesForOrderDetail> relevanteAds = GetAllDatesForOrderDetailByDate(dateForPrint)
                .Where(d => d.Details!.AdContent == null).ToList();
            List<OrderDetailsTable> orderDetailDTOs = _Mapper.Map<List<OrderDetailsTable>>(relevanteAds);
            for (int i = 0; i < orderDetailDTOs.Count; i++)
                orderDetailDTOs[i].WeekNumber = (dateForPrint.Subtract(orderDetailDTOs[i].Date).Days / 7) + 1;

            var paginationMetadata = new PaginationMetadata(orderDetailDTOs.Count(), page, itemsPerPage);

            var items = orderDetailDTOs.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);

            return new { List = items, PaginationMetadata = paginationMetadata };
        }

        public Object GetAllDetailsWordsTableByDate(DateTime dateForPrint, int page, int itemsPerPage)
        {
            List<DatesForOrderDetail> relevanteAds = GetAllDatesForOrderDetailByDate(dateForPrint)
                .Where(d => d.Details!.AdContent != null).ToList();
            List<DetailsWordsTable> orderDetailDTOs = _Mapper.Map<List<DetailsWordsTable>>(relevanteAds);
            for (int i = 0; i < orderDetailDTOs.Count; i++)
                orderDetailDTOs[i].WeekNumber = (dateForPrint.Subtract(orderDetailDTOs[i].Date).Days / 7) + 1;

            var paginationMetadata = new PaginationMetadata(orderDetailDTOs.Count(), page, itemsPerPage);

            var items = orderDetailDTOs.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);

            return new { List = items, PaginationMetadata = paginationMetadata };
        }

        #endregion

        #region DatesForOrderDetail

        private List<DatesForOrderDetail> GetAllDatesForOrderDetailByDate(DateTime dateForPrint)
        {
            List<DatesForOrderDetail> allDates = _datesForOrderDetailActions.GetAllDatesForOrderDetails();
            var relevanteAds = allDates
                .Where(d => d.Date == dateForPrint || (d.Date < dateForPrint && d.Date.AddDays(((double)(d.Details!.AdDuration!) - 1) * 7) >= dateForPrint));
            return relevanteAds.ToList();
        }

        public void UpdateStatus(int id, bool status)
        {
            _datesForOrderDetailActions.UpdateStatus(id, status);
        }

        #endregion

        #region PdfSharp

        private int PageNumbering(string oldPdfFile, string newPdfFile)
        {
            string existingPdfPath = oldPdfFile;
            string newPdfPath = newPdfFile;
            PdfDocument existingPdf = PdfReader.Open(existingPdfPath, PdfDocumentOpenMode.Import);
            PdfDocument newPdf = new PdfDocument();
            XGraphics gfx;
            for (int pageIndex = 0; pageIndex < existingPdf.PageCount; pageIndex++)
            {
                PdfPage existingPage = existingPdf.Pages[pageIndex];
                PdfPage newPage = newPdf.AddPage(existingPage);
                gfx = XGraphics.FromPdfPage(newPage);
                if (pageIndex % 2 == 1)
                    gfx.DrawString((pageIndex + 1) + "", new XFont("MV Boli", 16), XBrushes.White,
                                   new XRect(23, -2, newPage.Width, newPage.Height), XStringFormats.BottomLeft);
                else
                    gfx.DrawString((pageIndex + 1) + "", new XFont("MV Boli", 16), XBrushes.White,
                                   new XRect(-23, -2, newPage.Width, newPage.Height), XStringFormats.BottomRight);
            }
            newPdf.Save(newPdfPath);
            return existingPdf.PageCount;
        }

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

        private bool IsInsertedFileOnPage(string[,] mat, OrderDetail currentDetail, int widthImage, int heightImage, PdfPage page, XGraphics gfx)
        {
            bool isInserted = false;
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
                        DrawImageOnPageAndFillMat(page, gfx, mat, currentDetail, i, j);
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
            XImage image = XImage.FromFile(upload + jpegSamplePath);
            gfx.DrawImage(image, x, y, width, height);
        }

        private void DrawImageOnPageAndFillMat(PdfPage page, XGraphics gfx, string[,] matPage, OrderDetail detail, int i, int j)
        {
            int widthImage = (int)detail.Size!.SizeWidth;
            int heightImage = (int)detail.Size!.SizeHeight;
            int w = ((int)page.Width - 15) / 4;
            int h = ((int)page.Height - 32) / 8;
            DrawImage(gfx, detail.AdFile!, 16 + w * i, 16 + h * j, w * widthImage - 16, h * heightImage - 16);
            FillMat(matPage, i, j, widthImage, heightImage, detail.AdFile!);
        }

        private void placement(List<OrderDetail> list, string[,] mat, List<OrderDetail> listToAdd1, List<OrderDetail> listToAdd2, PdfPage page, XGraphics gfx, bool isDrow)
        {
            bool isInserted, isEmpty;
            OrderDetail currentDetail;
            for (int index = 0; index < list.Count; index++)
            {
                isInserted = false;
                currentDetail = list[index];
                int widthImage = (int)currentDetail.Size!.SizeWidth;
                int heightImage = (int)currentDetail.Size!.SizeHeight;
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
                            if (isDrow)
                                DrawImageOnPageAndFillMat(page, gfx, mat, currentDetail, i, j);
                            else
                            {
                                listToAdd1.Add(currentDetail);
                                FillMat(mat, i, j, widthImage, heightImage, currentDetail.AdFile!);
                            }
                            isInserted = true;
                        }
                    }
                }
                if (!isInserted)
                    listToAdd2.Add(currentDetail);
            }
        }

        private void Inlay(string final, string regular, string words, List<OrderDetail> placeCoverFileAds, List<OrderDetail> placeBackFileAds, List<OrderDetail> placeNormalFileAds, List<OrderDetail> managerFiles, int countLetter)
        {
            int countBox = (countLetter / 100) + 1;
            int num = 0;
            int rows = 8;
            int cols = 4;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            List<string[,]> pagesMats = new List<string[,]>();

            PdfDocument finalDocument = PdfReader.Open(final, PdfDocumentOpenMode.Modify);
            PdfDocument regularDocument = PdfReader.Open(regular, PdfDocumentOpenMode.Import);
            PdfDocument wordsDocument = PdfReader.Open(words, PdfDocumentOpenMode.Import);
            PdfPage page = finalDocument.Pages[0];
            PdfPage regularPage = regularDocument.Pages[0];
            PdfPage wordsPage = wordsDocument.Pages[0];

            string[,] firstMat = new string[cols, rows];
            pagesMats.Add(firstMat);
            XGraphics gfx = XGraphics.FromPdfPage(page);
            List<XGraphics> xGraphicss = new List<XGraphics> { gfx };
            FillMat(firstMat, 0, 0, cols, (int)(rows * 0.25), "titleImage");

            string[,] backMat = new string[cols, rows];
            string[,] wordsMat = new string[cols, rows];
            int temp = countBox;
            for (int i = 0; temp > 0; i++)
            {
                if (temp > 8)
                {
                    temp -= 8;
                    num = 8;
                }
                else
                {
                    num = temp;
                    temp = 0;
                }
                FillMat(wordsMat, i, 0, 1, num, "words");
            }
            List<OrderDetail> anyFilesToInsertToBack = new List<OrderDetail>();
            List<OrderDetail> anyFilesToInsertToWords = new List<OrderDetail>();
            List<OrderDetail> anyFilesToInsert1 = new List<OrderDetail>();
            List<OrderDetail> anyFilesToInsert2 = new List<OrderDetail>();

            OrderDetail currentDetail;
            bool isInserted;
            bool isEmpty;
            string[,] newPage;

            placeBackFileAds = SortBySizeDesc(placeBackFileAds);

            // רישום המודעות לעמוד האחורי, מודעה שאין לה מקום נכנסת למודעות שאין להן עדיפות
            placement(placeBackFileAds, backMat, anyFilesToInsertToBack, placeNormalFileAds, page, gfx, false);

            if(!IsMatFull(backMat))
            {
                placeBackFileAds = placeNormalFileAds;
                placeNormalFileAds = new List<OrderDetail>();
                placement(placeBackFileAds, backMat, anyFilesToInsertToBack, placeNormalFileAds, page, gfx, false);
            }

            placeCoverFileAds = SortBySizeDesc(placeCoverFileAds);

            // הכנסת המודעות לעמוד הקדמי אם אין להן מקום הם נכנסות לרשימת המודעות ללא עדיפות
            placement(placeCoverFileAds, firstMat, new List<OrderDetail>(), placeNormalFileAds, page, gfx, true);

            placeNormalFileAds = SortBySizeDesc(placeNormalFileAds);

            // הכנסת המודעות ללא עדיפות לעמוד הקדמי כל מי שלא נכנסה נכנסת למערך של שאר המודעות
            placement(placeNormalFileAds, firstMat, new List<OrderDetail>(), anyFilesToInsert1, page, gfx, true);

            anyFilesToInsert1 = SortBySizeDesc(anyFilesToInsert1);

            // רישום המועדות שנשארו לעמוד של המודעות מילים
            placement(anyFilesToInsert1, wordsMat, anyFilesToInsertToWords, anyFilesToInsert2, page, gfx, false);

            anyFilesToInsert2 = SortBySizeDesc(anyFilesToInsert2);

            // הכנסת כל שאר המודעות
            for (int index = 0; index < anyFilesToInsert2.Count; index++)
            {
                isInserted = false;
                currentDetail = anyFilesToInsert2[index];
                int widthImage = (int)currentDetail.Size!.SizeWidth;
                int heightImage = (int)currentDetail.Size!.SizeHeight;

                for (int p = 0; p < pagesMats.Count && !isInserted; p++)
                {
                    newPage = pagesMats[p];
                    gfx = xGraphicss[p];
                    for (int i = 0; i < newPage.GetLength(0) && !isInserted; i = i + widthImage)
                    {
                        for (int j = 0; j < newPage.GetLength(1) && !isInserted; j = j + heightImage)
                        {
                            isEmpty = true;
                            for (int q = i; q < i + widthImage && isEmpty; q++)
                                for (int l = j; l < j + heightImage && isEmpty; l++)
                                    isEmpty = newPage[q, l] == null;
                            if (isEmpty)
                            {
                                anyFilesToInsertToBack.Add(currentDetail);
                                DrawImageOnPageAndFillMat(page, gfx, newPage, currentDetail, i, j);
                                isInserted = true;
                            }
                        }
                    }
                }
                // אם לא נכנס באף אחד מהעמודים הקודמים אז פתיחת עמוד חדש
                if (!isInserted)
                {
                    newPage = new string[4, 8];
                    finalDocument.AddPage(regularPage);
                    page = finalDocument.Pages[finalDocument.Pages.Count - 1];
                    gfx = XGraphics.FromPdfPage(page);
                    xGraphicss.Add(gfx);
                    DrawImageOnPageAndFillMat(page, gfx, newPage, currentDetail, 0, 0);
                    pagesMats.Add(newPage);
                }

            }

            // הוספת עמוד מילים
            newPage = new string[cols, rows];
            temp = countBox;
            for (int i = 0; temp > 0; i++)
            {
                if (temp > 8)
                {
                    temp -= 8;
                    num = 8;
                }
                else
                {
                    num = temp;
                    temp = 0;
                }
                FillMat(newPage, i, 0, 1, num, "words");
            }
            finalDocument.AddPage(wordsPage);
            page = finalDocument.Pages[finalDocument.Pages.Count - 1];
            gfx = XGraphics.FromPdfPage(page);
            xGraphicss.Add(gfx);
            pagesMats.Add(newPage);
            for (int index = 0; index < anyFilesToInsertToWords.Count; index++)
            {
                currentDetail = anyFilesToInsertToWords[index];
                int widthImage = (int)currentDetail.Size!.SizeWidth;
                int heightImage = (int)currentDetail.Size!.SizeHeight;
                isInserted = IsInsertedFileOnPage(newPage, currentDetail, widthImage, heightImage, page, gfx);
            }

            // לפני הוספת העמוד האחורי צריך לבדוק כמה עמודים יש ואם יש זוגי צריך להוסיף עמוד
            if (pagesMats.Count % 2 == 0)
            {
                newPage = new string[cols, rows];
                finalDocument.AddPage(regularPage);
                page = finalDocument.Pages[finalDocument.Pages.Count - 1];
                gfx = XGraphics.FromPdfPage(page);
                xGraphicss.Add(gfx);
                pagesMats.Add(newPage);
            }

            // הוספת העמוד האחורי
            newPage = new string[cols, rows];
            finalDocument.AddPage(regularPage);
            page = finalDocument.Pages[finalDocument.Pages.Count - 1];
            gfx = XGraphics.FromPdfPage(page);
            xGraphicss.Add(gfx);
            pagesMats.Add(newPage);
            for (int index = 0; index < anyFilesToInsertToBack.Count; index++)
            {
                currentDetail = anyFilesToInsertToBack[index];
                int widthImage = (int)currentDetail.Size!.SizeWidth;
                int heightImage = (int)currentDetail.Size!.SizeHeight;
                isInserted = IsInsertedFileOnPage(newPage, currentDetail, widthImage, heightImage, page, gfx);
            }

            managerFiles = SortBySizeDesc(managerFiles);

            for (int index = 0; index < managerFiles.Count; index++)
            {
                isInserted = false;
                currentDetail = managerFiles[index];
                int widthImage = (int)currentDetail.Size!.SizeWidth;
                int heightImage = (int)currentDetail.Size!.SizeHeight;

                for (int p = 0; p < pagesMats.Count && !isInserted; p++)
                {
                    newPage = pagesMats[p];
                    gfx = xGraphicss[p];
                    for (int i = 0; i < newPage.GetLength(0) && !isInserted; i = i + widthImage)
                    {
                        for (int j = 0; j < newPage.GetLength(1) && !isInserted; j = j + heightImage)
                        {
                            isEmpty = true;
                            for (int q = i; q < i + widthImage && isEmpty; q++)
                                for (int l = j; l < j + heightImage && isEmpty; l++)
                                    isEmpty = newPage[q, l] == null;
                            if (isEmpty)
                            {
                                anyFilesToInsertToBack.Add(currentDetail);
                                DrawImageOnPageAndFillMat(page, gfx, newPage, currentDetail, i, j);
                                isInserted = true;
                            }
                        }
                    }
                }
            }
            finalDocument.Save(final);
        }

        public NewspapersPublishedDTO Shabets(DateTime datePublished)
        {
            List<OrderDetail> relevanteAds = GetAllReleventOrders(datePublished);

            List<OrderDetail> allRelevantFileAds = new List<OrderDetail>();
            List<OrderDetail> allRelevantWordsAds = new List<OrderDetail>();

            // מיון כל ההזמנות הרלונטיות לפרסומות ולמילים
            foreach (OrderDetail detail in relevanteAds)
                if (detail.AdFile != null)
                    allRelevantFileAds.Add(detail);
                else
                    allRelevantWordsAds.Add(detail);

            List<OrderDetail> placeCoverFileAds = new List<OrderDetail>();
            List<OrderDetail> placeBackFileAds = new List<OrderDetail>();
            List<OrderDetail> placeNormalFileAds = new List<OrderDetail>();
            List<OrderDetail> managerFiles = new List<OrderDetail>();
            allRelevantFileAds = SortBySizeDesc(allRelevantFileAds);
            foreach (OrderDetail detail in allRelevantFileAds)
                if (detail.Order!.Cust.CustEmail == _config["ManagerEmail"])
                    managerFiles.Add(detail);
                else if (detail.PlaceId == 2)
                    placeCoverFileAds.Add(detail);
                else if (detail.PlaceId == 1)
                    placeBackFileAds.Add(detail);
                else if (detail.PlaceId == 3)
                    placeNormalFileAds.Add(detail);

            List<NewspapersPublished> allNewpapers = _newspapersPublished.GetAllNewspapersPublished();
            int newspaperId = allNewpapers.Count() > 0 ? allNewpapers.Max(x => x.NewspaperId) + 1 : 1;

            string regular = myPath + "\\regularTemplate" + ".pdf";
            string regularWord = myPath + "\\regularTemplate" + ".dotx";
            string first = myPath + "\\firstTemplate" + ".pdf";
            string firstWord = myPath + "\\firstTemplate" + ".dotx";
            string words = myPath + "\\wordsTemplate" + ".pdf";
            string wordsWord = myPath + "\\wordsTemplate" + ".dotx";
            string final = myPath + "\\finalNewspaper" + ".pdf";
            string final2 = myPath + "\\finalNewspaper2" + ".pdf";
            string finalWord = myPath + "\\finalNewspaper" + ".dotx";
            string tempFileFullName = myPath + "\\wordsTemplateFull.dotx";

            int countLetter = CreateWordAd(wordsWord, words, tempFileFullName, allRelevantWordsAds);

            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            keyValues.Add("num", newspaperId.ToString());
            keyValues.Add("date", datePublished.ToString("d"));
            File.Copy(firstWord, finalWord, true);
            using (WordprocessingDocument myDoc = WordprocessingDocument.Open(finalWord, true))
            {
                SearchAndReplaceLike(myDoc.MainDocumentPart!.Document, keyValues);
            }

            ConvertFromWordToPdf(regularWord, regular);
            ConvertFromWordToPdf(finalWord, final);

            Inlay(final, regular, words, placeCoverFileAds, placeBackFileAds, placeNormalFileAds, managerFiles, countLetter);

            int countPages = PageNumbering(final, final2);

            string dir = pathWwwroot + "\\Newspapers\\" + datePublished.ToString("dd.MM.yyyy");
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            ConvertPdfToImages(final2, dir);
            File.Copy(final2, dir + "\\newspaper", true);

            File.Delete(regular);
            File.Delete(first);
            File.Delete(words);
            File.Delete(final);
            File.Delete(final2);
            File.Delete(finalWord);
            File.Delete(tempFileFullName);

            NewspapersPublishedDTO newspapersPublishedDTO = new NewspapersPublishedDTO()
            {
                CountPages = countPages,
                PublicationDate = datePublished.ToString("dd.MM.yyyy")
            };
            return newspapersPublishedDTO;
        }

        public void ClosingNewspaper(DateTime date, int countPages)
        {
            string dateString = date.ToString("dd.MM.yyyy");
            int index = GetAllNewspapersPublished().FindIndex(n => n.PublicationDate == dateString);
            if (index != -1)
                throw new DateAlreadyExistsException("Date of newspaper already exists in the system!");
            string placeForNespaper = pathWwwroot + "\\Newspapers\\" + dateString;
            if (!Directory.Exists(placeForNespaper))
                throw new NewspaperNotGeneratedException("Newspaper not generated in the files!");
            NewspapersPublished newspapersPublished = new NewspapersPublished();
            newspapersPublished.PublicationDate = date;
            newspapersPublished.CountPages = countPages;
            _newspapersPublished.AddNewNewspaperPublished(newspapersPublished);
            SentNewspaperForRecords(newspapersPublished.NewspaperId, placeForNespaper);
        }

        #endregion

        #region Converts

        public void ConvertFromWordToPdf(string dotxFilePath, string pdfFilePath)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            DocumentModel document = DocumentModel.Load(dotxFilePath, LoadOptions.DocxDefault);
            document.Save(pdfFilePath);
        }

        public void ConvertPdfToImages(string pdfFilePath, string pathImages)
        {

            if (!Directory.Exists(pathImages))
                Directory.CreateDirectory(pathImages);

            try
            {
                using (GhostscriptRasterizer rasterizer = new GhostscriptRasterizer())
                {
                    GhostscriptVersionInfo gvi = new GhostscriptVersionInfo(GHOSTSCRIPT_DLL_PATH);

                    rasterizer.Open(pdfFilePath, gvi, false);

                    for (int pageNumber = 1; pageNumber <= rasterizer.PageCount; pageNumber++)
                    {
                        int dpi = 300;
                        System.Drawing.Image img = rasterizer.GetPage(dpi, pageNumber);

                        string outputFilePath = Path.Combine(pathImages, $"{pageNumber - 1}.png");
                        img.Save(outputFilePath, System.Drawing.Imaging.ImageFormat.Png);

                        img.Dispose();
                    }
                }

                Console.WriteLine("PDF conversion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while converting PDF to images: " + ex.Message);
            }

        }

        #endregion

        #region FinishOrder

        private DateTime GetDateNow()
        {
            DateTime date = new DateTime(2024, 2, 28);//DateTime.Now
            return date;
        }
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
        private void EnterDates(List<string> listDates, List<int> orderDetailsIds)
        {
            DatesForOrderDetail datesForOrderDetail;
            DateTime dateTime;
            for (int i = 0; i < orderDetailsIds.Count; i++)
            {
                datesForOrderDetail = new DatesForOrderDetail();
                DateTime.TryParseExact(listDates[i], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                datesForOrderDetail.Date = dateTime;
                datesForOrderDetail.DetailsId = orderDetailsIds[i];
                datesForOrderDetail.ApprovalStatus = true;
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
                sum += listAdSize.FirstOrDefault(s => s.SizeId == od.SizeId)!.SizePrice * (decimal)od.AdDuration!;
            }
            return sum;
        }

        // פונקציה שמקבלת לקוח, מערך של פרטי הזמנות ומערך של תאריכים
        // הפונקציה מכניסה למסד הנתונים הזמנה וכל מה שהיא קיבלה
        public void FinishOrder(string token, List<string> listDates, List<OrderDetailDTO> listOrderDetails)
        {
            List<OrderDetail> orderDetails = FromListOrderDetailDTOToListOrderDetail(listOrderDetails).ToList();
            Order newOrder = new Order()
            {
                CustId = _jwtService.GetIdFromToken(token),
                OrderDate = GetDateNow(),
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

        public void FinishOrderAdWords(string token, List<string> listDates, List<OrderDetailDTO> listOrderDetails)
        {
            List<OrderDetail> orderDetails = FromListOrderDetailDTOToListOrderDetail(listOrderDetails).ToList();
            Order newOrder = new Order()
            {
                CustId = _jwtService.GetIdFromToken(token),
                OrderDate = GetDateNow(),
                OrderFinalPrice = CountWords(listOrderDetails[0].AdContent) * (decimal)listOrderDetails[0].AdDuration
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
                ReplaceUserWordTemplates(myDoc);
                myDoc.MainDocumentPart!.Document.Save();
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

            List<DatesForOrderDetail> allDates = _datesForOrderDetailActions.GetAllDatesForOrderDetails();//GetAllDatesForDetails();
            List<OrderDetail> relevanteAds = new List<OrderDetail>();
            foreach (var date in allDates)
                if (date.Date == dateOfPrint)
                    relevanteAds.Add(date.Details!);

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
                        wordAdToPrint.Add(detail.AdContent!);
                    }
            }
            string res = "";
            Break lineBreak = new Break();
            foreach (string item in wordAdToPrint)
            {
                if (item.IndexOf(" ") == -1)
                    res += item + (char)13 + (char)10;
                else
                    res += item + "\n";

            }

            string t = res;
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

            // Add the line break after the replaced text
            foreach (Text item in allParas)
            {
                foreach (KeyValuePair<string, string> itm in dict)
                {
                    if (item.Text.Trim().Contains(itm.Key.Trim()))
                    {
                        string rText = item.Text.Replace(itm.Key.Trim(), itm.Value.Trim());
                        Run run = new Run();
                        run.Append(new Text(rText));
                        run.Append(new Color());

                        item.Parent.ReplaceChild(run, item);
                        item.Text = rText;
                    }
                }
            }
        }

        #endregion

        #region CreateWordsAdByOpenXml

        private int CreateWordAd(string fullname, string words, string tempFileFullName, List<OrderDetail> allRelevantWordsAds)
        {
            File.Copy(fullname, tempFileFullName, true);
            return WriteToWordAd(tempFileFullName, words, allRelevantWordsAds);
        }

        private int WriteToWordAd(string tempFileFullName, string tempFileFullNamepdf, List<OrderDetail> allRelevantWordsAds)
        {
            int countLetter = 100;

            List<WordAdSubCategoryDTO> categories = GetAllWordAdSubCategories();

            List<OrderDetail> allDetailsWordAds = new List<OrderDetail>();

            List<string> wordAdToPrint = new List<string>();

            foreach (WordAdSubCategoryDTO category in categories)
            {
                if (allRelevantWordsAds.FirstOrDefault(w => w.WordCategory!.WordCategoryName.Equals(category.WordCategoryName)) != null)
                    using (WordprocessingDocument myDestDoc = WordprocessingDocument.Open(tempFileFullName, true))
                    {
                        MainDocumentPart mainPart = myDestDoc.MainDocumentPart!;

                        Body body = mainPart.Document.Body!;

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
                        countLetter += 24;
                        Text t = new Text(category.WordCategoryName);

                        Run run = new Run();
                        run.Append(runProperties);
                        run.Append(t);

                        newParagraph.Append(paragraphProperties);
                        newParagraph.Append(run);
                        body.Append(newParagraph);

                        mainPart.Document.Save();

                    }
                foreach (OrderDetail detail in allRelevantWordsAds)
                    if (detail.WordCategoryId == category.WordCategoryId)
                    {
                        using (WordprocessingDocument myDestDoc = WordprocessingDocument.Open(tempFileFullName, true))
                        {
                            MainDocumentPart mainPart = myDestDoc.MainDocumentPart!;

                            Body body = mainPart.Document.Body!;

                            Paragraph newParagraph = new Paragraph();
                            ParagraphProperties paragraphProperties = new ParagraphProperties();

                            Justification justification = new Justification() { Val = JustificationValues.Both };
                            paragraphProperties.Append(justification);

                            countLetter += detail.AdContent!.Trim().Length + 26;
                            Run run = new Run(new Text("• " + detail.AdContent.Trim()));
                            newParagraph.Append(paragraphProperties);
                            newParagraph.Append(run);

                            body.Append(newParagraph);

                            mainPart.Document.Save();
                        }
                    }
                ConvertFromWordToPdf(tempFileFullName, tempFileFullNamepdf);
            }
            return countLetter;
        }

        #endregion

        #region Email

        private void Sending(MailMessage mail)
        {
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Credentials = new System.Net.NetworkCredential("yads10000@gmail.com", "hzudmbsxcymmpsie");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        public string SentEmail(string name, string email, string message, string subject, string phone)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add("malkin.yaeli@gmail.com");
            mail.From = new MailAddress("yads10000@gmail.com", "Yads");
            mail.Subject = subject;
            if (phone == "number")
                phone = "";
            else
                phone = "Phone: " + phone + "<br /><br />";
            mail.Body = "From: " + email + "<br /><br />" + phone + "Message: " + message;
            mail.IsBodyHtml = true;

            MailMessage mailToClient = new MailMessage();
            mailToClient.To.Add(email);
            mailToClient.From = new MailAddress("yads10000@gmail.com", "Yads");
            mailToClient.Subject = "Thank you for your inquiry";
            mailToClient.Body = $"<div><h4>Hi {name},</h4>" +
                "<p>Thanks for contacting us</p>" +
                "<p>We will be happy to be in touch with you regarding your application</p>" +
                "<p>Thank you very much for your cooperation,</p>" +
                "<p>Company managers</p></div>";
            mailToClient.IsBodyHtml = true;

            try
            {
                Sending(mail);
                Sending(mailToClient);
                return "Mail Sent Successfully...";
            }
            catch
            {
                return "Error while Sending Mail";
            }
        }

        private void SentNewspaperForRecords(int num, string placeForNespaper)
        {
            var message = new MailMessage();
            message.From = new MailAddress("yads10000@gmail.com", "Yads");
            message.Subject = "Issue " + num;

            var html = "";

            LinkedResource linkedResource = new LinkedResource(pathWwwroot + @$"\Newspapers\logo.jpg");
            linkedResource.ContentId = Guid.NewGuid().ToString();
            html += $"<center><img width=\"600px\" hight=\"218px\" src=\"cid:" + linkedResource.ContentId + "\"/></center>";
            html += $"<center><h2>Attached to the message is this week's magazine, enjoy</h2></center>";

            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(linkedResource);

            message.AlternateViews.Add(alternateView);

            string filePath = placeForNespaper + @"\newspaper";
            Attachment attachment = new Attachment(filePath, MediaTypeNames.Application.Pdf);
            message.Attachments.Add(attachment);


            var list = _cacheRedis.GetAllItems();
            foreach (var item in list)
            {
                if (item.IsRegistered)
                {
                    message.To.Add(new MailAddress(item.Email));
                    Sending(message);
                    message.To.Clear();
                }
            }

        }

        #endregion
    }
}

