using AutoMapper;
using Models.Dtos.Categories;
using Models.Dtos.Manufacture;
using Models.Dtos.Parameters;
using Models.Dtos.Pircing;
using Models.Dtos.Products;
using Models.Entities.Categories;
using Models.Entities.Manufacture;
using Models.Entities.Parameters;
using Models.Entities.Pricing;
using Models.Entities.Products;
using System;

namespace Repository.Mappings
{
    public class OvipProductProfile : Profile
    {
        public OvipProductProfile()
        {
            // =========================
            // CREATE DTO -> ENTITY
            // =========================
            CreateMap<OvipProductCreateDto, OvipProduct>()
                // ID: NE ignore -> kézzel adjuk
                .ForMember(x => x.OvipProductId, opt => opt.MapFrom(x => x.OvipProductId))

                // audit
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))

                // navigation ignore
                .ForMember(x => x.Categories, opt => opt.Ignore())
                .ForMember(x => x.Images, opt => opt.Ignore())
                .ForMember(x => x.Parameters, opt => opt.Ignore())
                .ForMember(x => x.Stock, opt => opt.Ignore())
                .ForMember(x => x.QuantityDiscounts, opt => opt.Ignore())
                .ForMember(x => x.PricelistPrices, opt => opt.Ignore())
                .ForMember(x => x.Manufacture, opt => opt.Ignore())
                .ForMember(x => x.MainCategory, opt => opt.Ignore())
                .ForMember(x => x.VariantProduct, opt => opt.Ignore());

            // =========================
            // UPDATE DTO -> ENTITY
            // =========================
            CreateMap<OvipProductUpdateDto, OvipProduct>()
                .ForMember(x => x.OvipProductId, opt => opt.Ignore())
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))

                // navigation ignore
                .ForMember(x => x.Categories, opt => opt.Ignore())
                .ForMember(x => x.Images, opt => opt.Ignore())
                .ForMember(x => x.Parameters, opt => opt.Ignore())
                .ForMember(x => x.Stock, opt => opt.Ignore())
                .ForMember(x => x.QuantityDiscounts, opt => opt.Ignore())
                .ForMember(x => x.PricelistPrices, opt => opt.Ignore())
                .ForMember(x => x.Manufacture, opt => opt.Ignore())
                .ForMember(x => x.MainCategory, opt => opt.Ignore())
                .ForMember(x => x.VariantProduct, opt => opt.Ignore());

            // =========================
            // ENTITY -> DTO
            // =========================
            CreateMap<OvipProduct, OvipProductCreateDto>();
            CreateMap<OvipProduct, OvipProductUpdateDto>();

            // =====================================================
            // PRODUCT IMAGE
            // =====================================================

            // CREATE DTO -> ENTITY
            CreateMap<OvipProductImageCreateDto, OvipProductImage>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Product, opt => opt.Ignore());

            // UPDATE DTO -> ENTITY
            CreateMap<OvipProductImageUpdateDto, OvipProductImage>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Product, opt => opt.Ignore());

            // ENTITY -> DTO
            CreateMap<OvipProductImage, OvipProductImageCreateDto>();
            CreateMap<OvipProductImage, OvipProductImageUpdateDto>();

            // =====================================================
            // PRODUCT PARAMETER
            // =====================================================

            // CREATE DTO -> ENTITY
            CreateMap<OvipProductParameterCreateDto, OvipProductParameter>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Product, opt => opt.Ignore())
                .ForMember(x => x.Parameter, opt => opt.Ignore());

            // UPDATE DTO -> ENTITY
            CreateMap<OvipProductParameterUpdateDto, OvipProductParameter>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Product, opt => opt.Ignore())
                .ForMember(x => x.Parameter, opt => opt.Ignore());

            // ENTITY -> DTO
            CreateMap<OvipProductParameter, OvipProductParameterCreateDto>();
            CreateMap<OvipProductParameter, OvipProductParameterUpdateDto>();

            // =====================================================
            // STOCK
            // =====================================================

            // CREATE DTO -> ENTITY
            CreateMap<OvipStockCreateDto, OvipStock>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Product, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // UPDATE DTO -> ENTITY
            CreateMap<OvipStockUpdateDto, OvipStock>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Product, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // ENTITY -> DTO
            CreateMap<OvipStock, OvipStockCreateDto>();
            CreateMap<OvipStock, OvipStockUpdateDto>();

            // =====================================================
            // PRICE LIST
            // =====================================================

            // CREATE DTO -> ENTITY
            CreateMap<OvipPriceListCreateDto, OvipPriceList>()
                .ForMember(x => x.OvipPriceListId,
                    opt => opt.MapFrom(x => x.OvipPriceListId))
                .ForMember(x => x.Prices,
                    opt => opt.Ignore());

            // UPDATE DTO -> ENTITY
            CreateMap<OvipPriceListUpdateDto, OvipPriceList>()
                .ForMember(x => x.OvipPriceListId,
                    opt => opt.MapFrom(x => x.OvipPriceListId))
                .ForMember(x => x.Prices,
                    opt => opt.Ignore());

            // ENTITY -> DTO
            CreateMap<OvipPriceList, OvipPriceListCreateDto>();
            CreateMap<OvipPriceList, OvipPriceListUpdateDto>();

            // =====================================================
            // PRICE LIST PRICE
            // =====================================================

            // CREATE DTO -> ENTITY
            CreateMap<OvipPriceListPriceCreateDto, OvipPriceListPrice>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.PriceList, opt => opt.Ignore())
                .ForMember(x => x.Product, opt => opt.Ignore());

            // UPDATE DTO -> ENTITY
            CreateMap<OvipPriceListPriceUpdateDto, OvipPriceListPrice>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.PriceList, opt => opt.Ignore())
                .ForMember(x => x.Product, opt => opt.Ignore());

            // ENTITY -> DTO
            CreateMap<OvipPriceListPrice, OvipPriceListPriceCreateDto>();
            CreateMap<OvipPriceListPrice, OvipPriceListPriceUpdateDto>();

            // =====================================================
            // QUANTITY DISCOUNT
            // =====================================================

            // CREATE DTO -> ENTITY
            CreateMap<OvipQuantityDiscountCreateDto, OvipQuantityDiscount>()
                .ForMember(x => x.OvipQuantityId, opt => opt.MapFrom(x => x.OvipQuantityId));

            // UPDATE DTO -> ENTITY
            CreateMap<OvipQuantityDiscountUpdateDto, OvipQuantityDiscount>()
                .ForMember(x => x.OvipQuantityId, opt => opt.MapFrom(x => x.OvipQuantityId));

            // ENTITY -> DTO
            CreateMap<OvipQuantityDiscount, OvipQuantityDiscountCreateDto>();
            CreateMap<OvipQuantityDiscount, OvipQuantityDiscountUpdateDto>();

            // =====================================================
            // PARAMETER
            // =====================================================

            // CREATE DTO -> ENTITY
            CreateMap<OvipParameterCreateDto, OvipParameter>()
                .ForMember(x => x.OvipParameterId, opt => opt.MapFrom(x => x.OvipParameterId));

            // UPDATE DTO -> ENTITY
            CreateMap<OvipParameterUpdateDto, OvipParameter>()
                .ForMember(x => x.OvipParameterId, opt => opt.MapFrom(x => x.OvipParameterId));

            // ENTITY -> DTO
            CreateMap<OvipParameter, OvipParameterCreateDto>();
            CreateMap<OvipParameter, OvipParameterUpdateDto>();

            // =====================================================
            // MANUFACTURE
            // =====================================================

            // CREATE DTO -> ENTITY
            CreateMap<OvipManufactureCreateDto, OvipManufacture>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Product, opt => opt.Ignore())
                .ForMember(x => x.Parts, opt => opt.Ignore());

            // UPDATE DTO -> ENTITY
            CreateMap<OvipManufactureUpdateDto, OvipManufacture>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Product, opt => opt.Ignore())
                .ForMember(x => x.Parts, opt => opt.Ignore());

            // ENTITY -> DTO
            CreateMap<OvipManufacture, OvipManufactureCreateDto>();
            CreateMap<OvipManufacture, OvipManufactureUpdateDto>();

            // =====================================================
            // MANUFACTURE PART
            // =====================================================

            // CREATE DTO -> ENTITY
            CreateMap<OvipManufacturePartCreateDto, OvipManufacturePart>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Manufacture, opt => opt.Ignore())
                .ForMember(x => x.PartProduct, opt => opt.Ignore());

            // UPDATE DTO -> ENTITY
            CreateMap<OvipManufacturePartUpdateDto, OvipManufacturePart>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Manufacture, opt => opt.Ignore())
                .ForMember(x => x.PartProduct, opt => opt.Ignore());

            // ENTITY -> DTO
            CreateMap<OvipManufacturePart, OvipManufacturePartCreateDto>();
            CreateMap<OvipManufacturePart, OvipManufacturePartUpdateDto>();


            // =========================
            // CATEGORY
            // =========================

            // CREATE DTO -> ENTITY
            CreateMap<OvipCategoryCreateDto, OvipCategory>()
                .ForMember(x => x.OvipCategoryId, opt => opt.MapFrom(x => x.OvipCategoryId))
                .ForMember(x => x.ParentCategory, opt => opt.Ignore())
                .ForMember(x => x.Children, opt => opt.Ignore())
                .ForMember(x => x.Products, opt => opt.Ignore());

            // UPDATE DTO -> ENTITY
            CreateMap<OvipCategoryUpdateDto, OvipCategory>()
                .ForMember(x => x.OvipCategoryId, opt => opt.Ignore())
                .ForMember(x => x.ParentCategory, opt => opt.Ignore())
                .ForMember(x => x.Children, opt => opt.Ignore())
                .ForMember(x => x.Products, opt => opt.Ignore());

            // ENTITY -> DTO (ha kell)
            CreateMap<OvipCategory, OvipCategoryCreateDto>();
            CreateMap<OvipCategory, OvipCategoryUpdateDto>();

            // =========================
            // CATEGORY CONNECTION
            // =========================

            // CREATE DTO -> ENTITY
            CreateMap<OvipCategoryConnectionCreateDto, OvipCategoryConnection>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Product, opt => opt.Ignore())
                .ForMember(x => x.Category, opt => opt.Ignore());

            // UPDATE DTO -> ENTITY
            CreateMap<OvipCategoryConnectionUpdateDto, OvipCategoryConnection>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Product, opt => opt.Ignore())
                .ForMember(x => x.Category, opt => opt.Ignore());

            // ENTITY -> DTO
            CreateMap<OvipCategoryConnection, OvipCategoryConnectionCreateDto>();
            CreateMap<OvipCategoryConnection, OvipCategoryConnectionUpdateDto>();
        }
    }
}