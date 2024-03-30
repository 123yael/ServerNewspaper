using DAL.Models;
using DTO.Repository;
using Microsoft.AspNetCore.Http;

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

        public string SignUp(CustomerDTO customer, bool isRegistered);

        public string LogIn(string email, string pass);

        public int GetIdByCustomer(CustomerDTO customer);

        public bool IsAdmin(string token);

        #endregion

        #region NewspapersPublished

        public List<NewspapersPublishedDTO> GetAllNewspapersPublished();

        #endregion

        #region OrderDetail

        public List<OrderDetailDTO> GetAllOrderDetails();

        public decimal CalculationOfOrderPrice(List<OrderDetailDTO> listOrderDetails);

        public decimal CalculationOfOrderWordsPrice(List<OrderDetailDTO> listOrderDetails);

        #endregion

        #region Tabel

        public Object GetAllOrderDetailsTableByDate(DateTime dateForPrint, int page, int itemsPerPage);

        public Object GetAllDetailsWordsTableByDate(DateTime dateForPrint, int page, int itemsPerPage);

        public Object GetAllOrderDetailsTableManager(int page, int itemsPerPage);

        public Object GetAllOrderDetailsTableManagerWords(int page, int itemsPerPage);

        #endregion

        #region DatesForOrderDetail

        public void UpdateStatus(int id, bool status);


        #endregion

        #region PdfSharp

        public NewspapersPublishedDTO Shabets(DateTime date);

        public void ClosingNewspaper(DateTime date, int countPages);

        #endregion

        #region Converts

        public void ConvertFromWordToPdf(string Input, string Output);

        #endregion

        #region FinishOrder

        public void FinishOrder(string token, List<string> listDates, List<OrderDetailDTO> listOrderDetails);

        public void FinishOrderAdWords(string token, List<string> listDates, List<OrderDetailDTO> listOrderDetails);

        #endregion

        #region FunctionsByOpenXml

        public void CompleteWordTemplate(string fullname, string path);

        #endregion

        #region Email

        public string SentEmail(string name, string email, string message, string subject, string phone);

        #endregion
    }
}
