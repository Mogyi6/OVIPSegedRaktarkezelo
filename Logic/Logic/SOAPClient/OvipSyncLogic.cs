using Logic.Logic.CategoriesLogic.Interfaces;
using Logic.Logic.ManufactureLogic.Interfaces;
using Logic.Logic.ParametersLogic.Interfaces;
using Logic.Logic.PricingLogic.Interfaces;
using Logic.Logic.ProductsLogic.Interfaces;
using Models.Dtos.Categories;
using Models.Dtos.Manufacture;
using Models.Dtos.Parameters;
using Models.Dtos.Pircing;
using Models.Dtos.Products;
using Models.Dtos.RemoteDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Logic.Logic.SOAPClient
{
    public class OvipSyncLogic : IOvipSyncLogic
    {
        private readonly IOvipSoapClient _client;

        private readonly IOvipCategoryLogic _categoryLogic;
        private readonly IOvipCategoryConnectionLogic _categoryConnectionLogic;
        private readonly IOvipProductLogic _productLogic;
        private readonly IOvipPriceListLogic _priceListLogic;
        private readonly IOvipPriceListPriceLogic _priceListPriceLogic;
        private readonly IOvipQuantityDiscountLogic _quantityDiscountLogic;
        private readonly IOvipParameterLogic _parameterLogic;
        private readonly IOvipManufactureLogic _manufactureLogic;
        private readonly IOvipManufacturePartLogic _manufacturePartLogic;

        public OvipSyncLogic(
            IOvipSoapClient client,
            IOvipCategoryLogic categoryLogic,
            IOvipCategoryConnectionLogic categoryConnectionLogic,
            IOvipProductLogic productLogic,
            IOvipPriceListLogic priceListLogic,
            IOvipPriceListPriceLogic priceListPriceLogic,
            IOvipQuantityDiscountLogic quantityDiscountLogic,
            IOvipParameterLogic parameterLogic,
            IOvipManufactureLogic manufactureLogic,
            IOvipManufacturePartLogic manufacturePartLogic)
        {
            _client = client;
            _categoryLogic = categoryLogic;
            _categoryConnectionLogic = categoryConnectionLogic;
            _productLogic = productLogic;
            _priceListLogic = priceListLogic;
            _priceListPriceLogic = priceListPriceLogic;
            _quantityDiscountLogic = quantityDiscountLogic;
            _parameterLogic = parameterLogic;
            _manufactureLogic = manufactureLogic;
            _manufacturePartLogic = manufacturePartLogic;
        }

        public async Task SyncAllAsync()
        {
            await SyncCategoriesAsync();
            await SyncParametersAsync();
            await SyncPriceListsAsync();

            await SyncProductsAsync();

            await SyncCategoryConnectionsAsync();
            await SyncPriceListPricesAsync();
            await SyncQuantityDiscountsAsync();

            await SyncManufacturesAsync();
        }

        private async Task SyncCategoriesAsync()
        {
            var json = await _client.GetRequestAsync("getCategories");
            var items = Deserialize<List<OvipCategoryRemoteDto>>(json);

            var existingCategories = await _categoryLogic.GetAllAsync();

            foreach (var item in items)
            {
                var existing = existingCategories
                    .FirstOrDefault(x => x.OvipCategoryId == item.ovip_category_id);

                if (existing == null)
                {
                    await _categoryLogic.CreateAsync(new OvipCategoryCreateDto
                    {
                        OvipCategoryId = item.ovip_category_id,
                        ParentCategoryId = item.parent_category_id,
                        Name = item.name ?? string.Empty,
                        Description = item.description,
                        SeoTitle = item.seo_title,
                        SeoDescription = item.seo_description,
                        Image = item.image,
                        Order = item.order
                    });
                }
                else
                {
                    await _categoryLogic.UpdateAsync(new OvipCategoryUpdateDto
                    {
                        OvipCategoryId = item.ovip_category_id,
                        ParentCategoryId = item.parent_category_id,
                        Name = item.name ?? string.Empty,
                        Description = item.description,
                        SeoTitle = item.seo_title,
                        SeoDescription = item.seo_description,
                        Image = item.image,
                        Order = item.order
                    });
                }
            }
        }

        private async Task SyncParametersAsync()
        {
            var json = await _client.GetRequestAsync("getParams");
            var items = Deserialize<List<OvipParameterRemoteDto>>(json);

            var existingParameters = await _parameterLogic.GetAllAsync();

            foreach (var item in items)
            {
                var existing = existingParameters
                    .FirstOrDefault(x => x.OvipParameterId == item.ovip_parameter_id);

                if (existing == null)
                {
                    await _parameterLogic.CreateAsync(new OvipParameterCreateDto
                    {
                        OvipParameterId = item.ovip_parameter_id,
                        ParameterName = item.parameter_name ?? string.Empty
                    });
                }
                else
                {
                    await _parameterLogic.UpdateAsync(new OvipParameterUpdateDto
                    {
                        OvipParameterId = item.ovip_parameter_id,
                        ParameterName = item.parameter_name ?? string.Empty
                    });
                }
            }
        }

        private async Task SyncPriceListsAsync()
        {
            var json = await _client.GetRequestAsync("getPricelist");
            var items = Deserialize<List<OvipPriceListRemoteDto>>(json);

            var existingPriceLists = await _priceListLogic.GetAllAsync();

            foreach (var item in items)
            {
                var existing = existingPriceLists
                    .FirstOrDefault(x => x.OvipPriceListId == item.price_list_ovip_id);

                if (existing == null)
                {
                    await _priceListLogic.CreateAsync(new OvipPriceListCreateDto
                    {
                        OvipPriceListId = item.price_list_ovip_id,
                        Name = item.price_list_name ?? string.Empty
                    });
                }
                else
                {
                    await _priceListLogic.UpdateAsync(new OvipPriceListUpdateDto
                    {
                        OvipPriceListId = item.price_list_ovip_id,
                        Name = item.price_list_name ?? string.Empty
                    });
                }
            }
        }

        private async Task SyncProductsAsync()
        {
            var limitFrom = 0;
            var limitTo = 10000;

            while (true)
            {
                var json = await _client.GetRequestAsync(
                    request: "getProducts",
                    extraData: null,
                    limitFrom: limitFrom,
                    limitTo: limitTo);

                var products = Deserialize<List<OvipProductRemoteDto>>(json);

                if (products.Count == 0)
                    break;

                var existingProducts = await _productLogic.GetAllAsync();

                foreach (var item in products)
                {
                    var existing = existingProducts
                        .FirstOrDefault(x => x.OvipProductId == item.ovip_product_id);

                    if (existing == null)
                    {
                        await _productLogic.CreateAsync(new OvipProductCreateDto
                        {
                            OvipProductId = item.ovip_product_id,
                            Name = item.name ?? string.Empty,
                            Sku = item.sku ?? string.Empty,
                            ManufactureSku = item.manufacture_sku,
                            Barcode = item.bar_code,
                            Manufacturer = item.manufacturer,

                            WebshopVisible = item.webshop_visible == 1,
                            Orderable = item.orderable ?? 0,

                            ShortDescription = item.short_description,
                            LongDescription = item.long_description,
                            SeoTitle = item.seo_title,
                            SeoDescription = item.seo_description,

                            NetWeight = item.net_weight,
                            GrossWeight = item.gross_weight,
                            Width = item.width,
                            Height = item.height,
                            Length = item.length,

                            Unit = item.unit,
                            AltUnit = item.alt_unit,
                            AltUnitQuantity = item.alt_unit_quantity,
                            ProductUnitQuantity = item.product_unit_quantity,

                            NetPrice = item.net_price ?? 0,
                            GrossPrice = item.gross_price ?? 0,
                            Tax = item.tax ?? 0,

                            NetSalePrice = item.net_sale_price,
                            GrossSalePrice = item.gross_sale_price,
                            SaleStart = item.sale_start,
                            SaleEnd = item.sale_end,

                            OvipCategoryId = item.ovip_category_id ?? 0
                        });
                    }
                    else
                    {
                        await _productLogic.UpdateAsync(new OvipProductUpdateDto
                        {
                            OvipProductId = item.ovip_product_id,
                            Name = item.name ?? string.Empty,
                            Sku = item.sku ?? string.Empty,
                            ManufactureSku = item.manufacture_sku,
                            Barcode = item.bar_code,
                            Manufacturer = item.manufacturer,

                            Deleted = item.deleted == 1,
                            WebshopVisible = item.webshop_visible == 1,
                            Orderable = item.orderable ?? 0,

                            ShortDescription = item.short_description,
                            LongDescription = item.long_description,
                            SeoTitle = item.seo_title,
                            SeoDescription = item.seo_description,

                            NetWeight = item.net_weight,
                            GrossWeight = item.gross_weight,
                            Width = item.width,
                            Height = item.height,
                            Length = item.length,

                            Unit = item.unit,
                            AltUnit = item.alt_unit,
                            AltUnitQuantity = item.alt_unit_quantity,
                            ProductUnitQuantity = item.product_unit_quantity,

                            NetPrice = item.net_price ?? 0,
                            GrossPrice = item.gross_price ?? 0,
                            Tax = item.tax ?? 0,

                            NetSalePrice = item.net_sale_price,
                            GrossSalePrice = item.gross_sale_price,
                            SaleStart = item.sale_start,
                            SaleEnd = item.sale_end,

                            OvipCategoryId = item.ovip_category_id ?? 0
                        });
                    }
                }

                if (products.Count < limitTo)
                    break;

                limitFrom += limitTo;
            }
        }

        private async Task SyncCategoryConnectionsAsync()
        {
            var json = await _client.GetRequestAsync("getCategoriesPlus");
            var items = Deserialize<List<OvipCategoryConnectionRemoteDto>>(json);

            var existingConnections = await _categoryConnectionLogic.GetAllAsync();

            foreach (var item in items)
            {
                var existing = existingConnections.FirstOrDefault(x =>
                    x.OvipProductId == item.ovip_product_id &&
                    x.OvipCategoryId == item.ovip_category_id);

                if (existing == null)
                {
                    await _categoryConnectionLogic.CreateAsync(new OvipCategoryConnectionCreateDto
                    {
                        OvipProductId = item.ovip_product_id,
                        OvipCategoryId = item.ovip_category_id
                    });
                }
                else
                {
                    await _categoryConnectionLogic.UpdateAsync(new OvipCategoryConnectionUpdateDto
                    {
                        Id = existing.Id,
                        OvipProductId = item.ovip_product_id,
                        OvipCategoryId = item.ovip_category_id
                    });
                }
            }
        }

        private async Task SyncPriceListPricesAsync()
        {
            var json = await _client.GetRequestAsync("getPricelist");
            var priceLists = Deserialize<List<OvipPriceListRemoteDto>>(json);

            var existingPrices = await _priceListPriceLogic.GetAllAsync();

            foreach (var priceList in priceLists)
            {
                foreach (var price in priceList.price_list_prices)
                {
                    var existing = existingPrices.FirstOrDefault(x =>
                        x.OvipPriceListId == priceList.price_list_ovip_id &&
                        x.OvipProductId == price.ovip_product_id);

                    if (existing == null)
                    {
                        await _priceListPriceLogic.CreateAsync(new OvipPriceListPriceCreateDto
                        {
                            OvipPriceListId = priceList.price_list_ovip_id,
                            OvipProductId = price.ovip_product_id,
                            NetPrice = price.net_price,
                            GrossPrice = price.gross_price,
                            NetSalePrice = price.net_sale_price,
                            GrossSalePrice = price.gross_sale_price,
                            Tax = price.tax,
                            SaleStart = price.sale_start,
                            SaleEnd = price.sale_end
                        });
                    }
                    else
                    {
                        await _priceListPriceLogic.UpdateAsync(new OvipPriceListPriceUpdateDto
                        {
                            Id = existing.Id,
                            OvipPriceListId = priceList.price_list_ovip_id,
                            OvipProductId = price.ovip_product_id,
                            NetPrice = price.net_price,
                            GrossPrice = price.gross_price,
                            NetSalePrice = price.net_sale_price,
                            GrossSalePrice = price.gross_sale_price,
                            Tax = price.tax,
                            SaleStart = price.sale_start,
                            SaleEnd = price.sale_end
                        });
                    }
                }
            }
        }

        private async Task SyncQuantityDiscountsAsync()
        {
            var json = await _client.GetRequestAsync("GetQtyDiscount");
            var items = Deserialize<List<OvipQuantityDiscountRemoteDto>>(json);

            var existingDiscounts = await _quantityDiscountLogic.GetAllAsync();

            foreach (var item in items)
            {
                var existing = existingDiscounts
                    .FirstOrDefault(x => x.OvipQuantityId == item.ovip_quantity_id);

                if (existing == null)
                {
                    await _quantityDiscountLogic.CreateAsync(new OvipQuantityDiscountCreateDto
                    {
                        OvipQuantityId = item.ovip_quantity_id,
                        ProductId = item.product_id,
                        PriceListId = item.price_list_id,
                        DiscountFromQuantity = item.discount_from_quantity,
                        DiscountUntilQuantity = item.discount_until_quantity,
                        DiscountType = item.discount_type ?? string.Empty,
                        DiscountValue = item.discount_value
                    });
                }
                else
                {
                    await _quantityDiscountLogic.UpdateAsync(new OvipQuantityDiscountUpdateDto
                    {
                        OvipQuantityId = item.ovip_quantity_id,
                        ProductId = item.product_id,
                        PriceListId = item.price_list_id,
                        DiscountFromQuantity = item.discount_from_quantity,
                        DiscountUntilQuantity = item.discount_until_quantity,
                        DiscountType = item.discount_type ?? string.Empty,
                        DiscountValue = item.discount_value
                    });
                }
            }
        }

        private async Task SyncManufacturesAsync()
        {
            var json = await _client.GetRequestAsync("getManufacture");
            var items = Deserialize<List<OvipManufactureRemoteDto>>(json);

            var existingManufactures = await _manufactureLogic.GetAllAsync();

            foreach (var item in items)
            {
                var existing = existingManufactures
                    .FirstOrDefault(x => x.OvipProductId == item.ovip_product_id);

                var manufacture = existing == null
                    ? await _manufactureLogic.CreateAsync(new OvipManufactureCreateDto
                    {
                        OvipProductId = item.ovip_product_id,
                        AutoComplete = item.auto_complete == 1
                    })
                    : await _manufactureLogic.UpdateAsync(new OvipManufactureUpdateDto
                    {
                        Id = existing.Id,
                        OvipProductId = item.ovip_product_id,
                        AutoComplete = item.auto_complete == 1
                    });

                if (manufacture == null)
                    continue;

                var existingParts = await _manufacturePartLogic.GetByManufactureIdAsync(manufacture.Id);

                foreach (var part in item.parts)
                {
                    var existingPart = existingParts.FirstOrDefault(x =>
                        x.ManufactureId == manufacture.Id &&
                        x.PartProductId == part.ovip_product_id);

                    if (existingPart == null)
                    {
                        await _manufacturePartLogic.CreateAsync(new OvipManufacturePartCreateDto
                        {
                            ManufactureId = manufacture.Id,
                            PartProductId = part.ovip_product_id,
                            Quantity = part.quantity
                        });
                    }
                    else
                    {
                        await _manufacturePartLogic.UpdateAsync(new OvipManufacturePartUpdateDto
                        {
                            Id = existingPart.Id,
                            ManufactureId = manufacture.Id,
                            PartProductId = part.ovip_product_id,
                            Quantity = part.quantity
                        });
                    }
                }
            }
        }

        private static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new Exception("Az OVIP válasz nem feldolgozható.");
        }
    }
}
