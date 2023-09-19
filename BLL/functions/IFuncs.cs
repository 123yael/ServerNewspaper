using DAL.Models;
using DTO.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Functions
{
    public interface IFuncs
    {
        #region WpAdSubCategory

        public List<WordAdSubCategoryDTO> GetAllWordAdSubCategories();

        #endregion

        #region AdSize

        public List<AdSizeDTO> GetAllAdSize();

        #endregion

        #region AdPlacement

        public List<AdPlacementDTO> GetAllAdPlacement();

        #endregion

        #region Customer

        public CustomerDTO SignUp(CustomerDTO customer);

        public CustomerDTO GetCustomerByEmailAndPass(string email, string pass);

        public int GetIdByCustomer(CustomerDTO customer);

        #endregion

        #region NewspapersPublished

        public List<NewspapersPublishedDTO> GetAllNewspapersPublished();

        #endregion

        #region OrderDetail

        public List<OrderDetailDTO> GetAllOrderDetails();

        #endregion

        #region PdfSharp

        public void Shabets(string pathPdf);

        #endregion

        #region Converts

        public void ConvertPdfToWord(string pdfFilePath, string wordFilePath);

        public void ConvertFromWordToPdf(string Input, string Output);

        #endregion

        #region FinishOrder

        public void FinishOrder(CustomerDTO customer, List<DateTime> listDates, List<OrderDetailDTO> listOrderDetails);

        public void FinishOrderAdWords(CustomerDTO customer, List<DateTime> listDates, List<OrderDetailDTO> listOrderDetails);

        #endregion

        #region FunctionsByOpenXml

        public void CompleteWordTemplate(string fullname, string path);

        #endregion

        //#region IText

        //public void Shabets(string pathPdf);

        //#endregion

        #region Email

        public int SentEmail(string name, string email, string message);

        #endregion
    }
}
