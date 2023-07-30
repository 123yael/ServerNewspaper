using DAL.Models;
using DTO.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.functions
{
    public interface IFuncs
    {
        public List<AdSizeDTO> GetAllAdSize();

        public List<AdPlacementDTO> GetAllAdPlacement();

        public List<NewspapersPublishedDTO> GetAllNewspapersPublished();

        public void Create(string filename, List<OrderDetail> RelevantOrdersDTO);

        public List<OrderDetailDTO> GetAllOrderDetails();

        public List<WordAdSubCategoryDTO> GetAllWordAdSubCategories();

        public int GetIdByCustomer(CustomerDTO customer);
        public CustomerDTO GetCustomerByEmailAndPass(string email, string pass);

        public void FinishOrder(CustomerDTO customer, List<List<DateTime>> listDates, List<OrderDetailDTO> listOrderDetails);

        public void FinishOrderAdWords(CustomerDTO customer, List<List<DateTime>> listDates, List<OrderDetailDTO> listOrderDetails);

        public void ConvertFromWordToPdf(string Input, string Output);

        public void Shabets(string pathPdf);

        public void ConvertPdfToWord(string pdfFilePath, string wordFilePath);

        public void CompleteWordTemplate(string fullname, string path);

        public void CreateWordAd(string fullname, string path);

    }
}
