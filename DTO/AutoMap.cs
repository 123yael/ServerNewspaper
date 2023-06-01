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

            CreateMap<AdvertisementCategoryDTO, AdvertisementCategory>();
            CreateMap<AdvertisementCategory, AdvertisementCategoryDTO>();

            CreateMap<CustomerDTO, Customer>();
            CreateMap<Customer, CustomerDTO>();

            CreateMap<DatesForOrderDetailDTO, DatesForOrderDetail>();
            CreateMap<DatesForOrderDetail, DatesForOrderDetailDTO>().ForMember(dest =>
            dest.DetailsId, opt =>
            opt.MapFrom(src => src.Details.DetailsId)); ;

            CreateMap<NewspapersPublishedDTO, NewspapersPublished>();
            CreateMap<NewspapersPublished, NewspapersPublishedDTO>();

            CreateMap<OrderDTO, Order>();
            CreateMap<Order, OrderDTO>();

            CreateMap<OrderDetailDTO, OrderDetail>();
            //    .ForMember(dest =>
            //dest.Size, opt =>
            //opt.MapFrom(src => src.SizeId != null ? new AdSize { SizeId = src.SizeId.Value } : null));
            CreateMap<OrderDetail, OrderDetailDTO>().ForMember(dest =>
            dest.SizeId, opt =>
            opt.MapFrom(src => src.Size.SizeId));

            CreateMap<PagesInNewspaperDTO, PagesInNewspaper>();
            CreateMap<PagesInNewspaper, PagesInNewspaperDTO>();

            CreateMap<PlacingAdsInPageDTO, PlacingAdsInPage>();
            CreateMap<PlacingAdsInPage, PlacingAdsInPageDTO>();

            CreateMap<WordAdSubCategoryDTO, WordAdSubCategory>();
            CreateMap<WordAdSubCategory, WordAdSubCategoryDTO>();
        }

    }
}
