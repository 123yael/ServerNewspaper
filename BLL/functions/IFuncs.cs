using DAL.Models;
using DTO.Repository;

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

        public CustomerDTO LogIn(string email, string pass);

        public int GetIdByCustomer(CustomerDTO customer);

        #endregion

        #region NewspapersPublished

        public List<NewspapersPublishedDTO> GetAllNewspapersPublished();

        #endregion

        #region OrderDetail

        public List<OrderDetailDTO> GetAllOrderDetails();

        #endregion

        #region Tabel

        public Object GetAllOrderDetailsTableByDate(DateTime dateForPrint, int page, int itemsPerPage);

        public Object GetAllDetailsWordsTableByDate(DateTime dateForPrint, int page, int itemsPerPage);

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

        public void FinishOrder(CustomerDTO customer, List<string> listDates, List<OrderDetailDTO> listOrderDetails);

        public void FinishOrderAdWords(CustomerDTO customer, List<string> listDates, List<OrderDetailDTO> listOrderDetails);

        #endregion

        #region FunctionsByOpenXml

        public void CompleteWordTemplate(string fullname, string path);

        #endregion

        #region Email

        public string SentEmail(string name, string email, string message, string subject, string phone);

        #endregion
    }
}
