﻿using DAL.Models;
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

        public void AddAdFileToPdf();

        public void AddAdFileToPdf3();

        public void GeneratePDF(string filename, string imageLoc);
        public void Create(string filename, List<OrderDetail> RelevantOrdersDTO);

        public void WriteToPdf(string filename);

        public List<OrderDetailDTO> GetAllOrderDetails();

        public List<WordAdSubCategoryDTO> GetAllWordAdSubCategories();

        public int GetIdByCustomer(CustomerDTO customer);
        public CustomerDTO GetCustomerByEmailAndPass(string email, string pass);

        public void FinishOrder(CustomerDTO customer, List<List<DateTime>> listDates, List<OrderDetailDTO> listOrderDetails);

        public void FirstWord(string filePath, string[] codeLines);

        public void convertWordPFD(string Input, string Output);

        //public void Create2(string filename);

        public void Shabets();

        public void ConvertPdfToWord(string pdfFilePath, string wordFilePath);


    }
}
