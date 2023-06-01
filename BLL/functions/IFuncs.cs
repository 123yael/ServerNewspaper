﻿using DTO.Repository;
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

        public void AddAdFileToPdf();

        public void AddAdFileToPdf3();

        public void GeneratePDF(string filename, string imageLoc);
        public void Create(string filename);

        public void WriteToPdf(string filename);

        public List<WordAdSubCategoryDTO> GetAllWordAdSubCategories();

        public int GetIdByCustomer(CustomerDTO customer);
        public bool IsCustomerExists(string email, string pass);

        public void FinishOrder(CustomerDTO customer, List<List<DateTime>> listDates, List<OrderDetailDTO> listOrderDetails);

    }
}
