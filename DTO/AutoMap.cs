using AutoMapper;
using DTO.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;


namespace DTO
{
    public class AutoMap : Profile
    {
        public AutoMap()
        {
            CreateMap<AdPlacementDTO, AdPlacement>();
            CreateMap<AdPlacement, AdPlacementDTO>();

            CreateMap<AdSizeDTO, AdSize>();
            CreateMap<AdSize, AdSizeDTO>();

            CreateMap<CustomerDTO, Customer>();
            CreateMap<Customer, CustomerDTO>();

            CreateMap<DatesForOrderDetailDTO, DatesForOrderDetail>();
            CreateMap<DatesForOrderDetail, DatesForOrderDetailDTO>().ForMember(dest =>
            dest.DetailsId, opt =>
            opt.MapFrom(src => src.Details.DetailsId));

            CreateMap<NewspapersPublishedDTO, NewspapersPublished>();
            CreateMap<NewspapersPublished, NewspapersPublishedDTO>()
                .ForMember(dest =>
            dest.PublicationDate, opt =>
            opt.MapFrom(src => src.PublicationDate.ToString("dd.MM.yyyy")));

            CreateMap<OrderDTO, Order>();
            CreateMap<Order, OrderDTO>();

            CreateMap<OrderDetailDTO, OrderDetail>();
            CreateMap<OrderDetail, OrderDetailDTO>().ForMember(dest =>
            dest.SizeId, opt =>
            opt.MapFrom(src => src.Size.SizeId));

            CreateMap<WordAdSubCategoryDTO, WordAdSubCategory>();
            CreateMap<WordAdSubCategory, WordAdSubCategoryDTO>();

            CreateMap<OrderDetail, OrderDetailsTable>()
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Size.SizeName))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.CustFullName, opt => opt.MapFrom(src => src.Order.Cust.CustFirstName + " " + src.Order.Cust.CustLastName))
                .ForMember(dest => dest.CustEmail, opt => opt.MapFrom(src => src.Order.Cust.CustEmail))
                .ForMember(dest => dest.CustPhone, opt => opt.MapFrom(src => src.Order.Cust.CustPhone))
                .ForMember(dest => dest.OrderFinalPrice, opt => opt.MapFrom(src => src.Order.OrderFinalPrice));
        }

    }
}
