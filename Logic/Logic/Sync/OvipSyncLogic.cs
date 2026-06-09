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
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Logic.Logic.Sync
{
    public class OvipSyncLogic : IOvipSyncLogic
    {
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

        // PHP backend URL (proxy to the OVIP SOAP API)
        // Update if your PHP service runs on a different host/port
        private const string PhpBackendUrl = "http://72.60.176.243:5000/";

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

        public async Task<string> SyncCategoriesAsync()
{
    var json = await CallPhpProxyAsync("getCategories");
    var items = Deserialize<List<OvipCategoryRemoteDto>>(json);

    var existingCategories = await _categoryLogic.GetAllAsync();

    // 1. kör: mentés ParentCategoryId nélkül
    foreach (var item in items)
    {
        var existing = existingCategories
            .FirstOrDefault(x => x.OvipCategoryId == item.ovip_category_id);

        if (existing == null)
        {
            await _categoryLogic.CreateAsync(new OvipCategoryCreateDto
            {
                OvipCategoryId = item.ovip_category_id,
                ParentCategoryId = null,
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
                ParentCategoryId = null,
                Name = item.name ?? string.Empty,
                Description = item.description,
                SeoTitle = item.seo_title,
                SeoDescription = item.seo_description,
                Image = item.image,
                Order = item.order
            });
        }
    }

    // 2. kör: szülők beállítása az items alapján
    foreach (var item in items)
    {
        var parentId = item.parent_category_id == 0
            ? null
            : item.parent_category_id;

        var parentExists = parentId == null ||
            items.Any(x => x.ovip_category_id == parentId);

        if (!parentExists)
        {
            Console.WriteLine(
                $"Hiányzó parent category: Category={item.ovip_category_id}, Parent={parentId}"
            );

            continue;
        }

        await _categoryLogic.UpdateAsync(new OvipCategoryUpdateDto
        {
            OvipCategoryId = item.ovip_category_id,
            ParentCategoryId = parentId,
            Name = item.name ?? string.Empty,
            Description = item.description,
            SeoTitle = item.seo_title,
            SeoDescription = item.seo_description,
            Image = item.image,
            Order = item.order
        });
    }

    return json;
}

        public async Task<string> SyncParametersAsync()
        {
            var json = await CallPhpProxyAsync("getParams");
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
            return json;
        }

        public async Task<string> SyncPriceListsAsync()
        {
            var json = await CallPhpProxyAsync("getPricelist");
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
            return json;
        }

        public async Task<string> SyncProductsAsync(string? extraData = null, int? limitFrom = null, int? limitTo = null)
{
    var result = new ProductSyncResult();

    var logs = new List<string>();

    void Log(string message)
    {
        var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        logs.Add(line);
        Console.WriteLine(line);
    }

    Log("===== PRODUCT SYNC START =====");
    Log($"Input extraData: '{extraData ?? "NULL"}'");
    Log($"Input limitFrom: {(limitFrom.HasValue ? limitFrom.Value.ToString() : "NULL")}");
    Log($"Input limitTo: {(limitTo.HasValue ? limitTo.Value.ToString() : "NULL")}");

    var existingCategoryIds = new HashSet<int>(
        (await _categoryLogic.GetAllAsync()).Select(x => x.OvipCategoryId));

    Log($"Existing categories count: {existingCategoryIds.Count}");

    async Task EnsureCategoryAsync(int categoryId)
    {
        Log($"Checking category: {categoryId}");

        if (existingCategoryIds.Contains(categoryId))
        {
            Log($"Category exists in cache: {categoryId}");
            return;
        }

        var existing = await _categoryLogic.GetByIdAsync(categoryId);

        if (existing != null)
        {
            existingCategoryIds.Add(categoryId);
            Log($"Category exists in database: {categoryId}");
            return;
        }

        try
        {
            Log($"Creating placeholder category: {categoryId}");

            await _categoryLogic.CreateAsync(new OvipCategoryCreateDto
            {
                OvipCategoryId = categoryId,
                ParentCategoryId = null,
                Name = $"Placeholder category {categoryId}",
                Description = $"Automatically created because a product referenced missing category {categoryId}.",
                Order = 0
            });

            existingCategoryIds.Add(categoryId);

            var warning = $"Placeholder category created for missing category id {categoryId}.";
            result.Warnings.Add(warning);
            Log($"WARNING: {warning}");
        }
        catch (Exception ex)
        {
            var error = $"Failed to create placeholder category {categoryId}: {ex.GetBaseException().Message}";
            result.Errors.Add(error);
            Log($"ERROR: {error}");
        }
    }

    async Task ProcessItemAsync(
        OvipProductRemoteDto item,
        Dictionary<int, Models.Entities.Products.OvipProduct> existingProducts)
    {
        result.Processed++;

        Log("");
        Log("--------------------------------------------------");
        Log($"Processing product #{result.Processed}");
        Log($"OvipProductId: {item.ovip_product_id}");
        Log($"Name: {item.name ?? "NULL"}");
        Log($"Sku: {item.sku ?? "NULL"}");
        Log($"CategoryId raw: {(item.ovip_category_id.HasValue ? item.ovip_category_id.Value.ToString() : "NULL")}");
        Log($"VariantId raw: {item.product_variant_id ?? "NULL"}");
        Log($"Manufacturer: {item.manufacturer ?? "NULL"}");
        Log($"ManufactureSku: {item.manufacture_sku ?? "NULL"}");
        Log($"Barcode: {item.bar_code ?? "NULL"}");
        Log($"WebshopVisible: {(item.webshop_visible.HasValue ? item.webshop_visible.Value.ToString() : "NULL")}");
        Log($"Deleted: {(item.deleted.HasValue ? item.deleted.Value.ToString() : "NULL")}");
        Log($"Orderable: {(item.orderable.HasValue ? item.orderable.Value.ToString() : "NULL")}");
        Log($"NetPrice: {(item.net_price.HasValue ? item.net_price.Value.ToString() : "NULL")}");
        Log($"GrossPrice: {(item.gross_price.HasValue ? item.gross_price.Value.ToString() : "NULL")}");
        Log($"Tax: {(item.tax.HasValue ? item.tax.Value.ToString() : "NULL")}");
        Log($"NetSalePrice: {(item.net_sale_price.HasValue ? item.net_sale_price.Value.ToString() : "NULL")}");
        Log($"GrossSalePrice: {(item.gross_sale_price.HasValue ? item.gross_sale_price.Value.ToString() : "NULL")}");
        Log($"SaleStart raw: {item.sale_start ?? "NULL"}");
        Log($"SaleEnd raw: {item.sale_end ?? "NULL"}");

        var categoryId = item.ovip_category_id ?? 0;

        if (categoryId == 0)
        {
            var error = $"Product {item.ovip_product_id}: missing or zero category id.";
            result.Errors.Add(error);
            Log($"ERROR: {error}");
            return;
        }

        await EnsureCategoryAsync(categoryId);

        var saleStart = ParseNullableDate(item.sale_start);
        var saleEnd = ParseNullableDate(item.sale_end);

        Log($"Parsed SaleStart: {(saleStart.HasValue ? saleStart.Value.ToString("yyyy-MM-dd HH:mm:ss") : "NULL")}");
        Log($"Parsed SaleEnd: {(saleEnd.HasValue ? saleEnd.Value.ToString("yyyy-MM-dd HH:mm:ss") : "NULL")}");

        try
        {
            var exists = existingProducts.TryGetValue(item.ovip_product_id, out var existing);

            Log($"Product exists: {exists}");

            if (!exists)
            {
                Log("Action: CREATE");

                await _productLogic.CreateAsync(new OvipProductCreateDto
                {
                    OvipProductId = item.ovip_product_id,
                    Name = item.name ?? string.Empty,
                    Sku = item.sku ?? string.Empty,
                    ManufactureSku = item.manufacture_sku,
                    Barcode = item.bar_code,
                    Manufacturer = item.manufacturer,

                    // Deleted = item.deleted ?? false,
                    WebshopVisible = item.webshop_visible ?? false,
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
                    SaleStart = saleStart,
                    SaleEnd = saleEnd,

                    OvipCategoryId = categoryId,
                    ProductVariantId = item.product_variant_id
                });

                result.Created++;
                Log($"SUCCESS CREATE ProductId={item.ovip_product_id}");
            }
            else
            {
                Log("Action: UPDATE");

                await _productLogic.UpdateAsync(new OvipProductUpdateDto
                {
                    OvipProductId = item.ovip_product_id,
                    Name = item.name ?? string.Empty,
                    Sku = item.sku ?? string.Empty,
                    ManufactureSku = item.manufacture_sku,
                    Barcode = item.bar_code,
                    Manufacturer = item.manufacturer,

                    Deleted = item.deleted ?? false,
                    WebshopVisible = item.webshop_visible ?? false,
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
                    SaleStart = saleStart,
                    SaleEnd = saleEnd,

                    OvipCategoryId = categoryId,
                    ProductVariantId = item.product_variant_id
                });

                result.Updated++;
                Log($"SUCCESS UPDATE ProductId={item.ovip_product_id}");
            }
        }
        catch (Exception ex)
        {
            var baseError = ex.GetBaseException().Message;

            Log($"ERROR ProductId={item.ovip_product_id}");
            Log($"ERROR Message: {ex.Message}");
            Log($"ERROR BaseException: {baseError}");
            Log($"ERROR StackTrace: {ex.StackTrace}");

            result.Errors.Add(
                $"Product {item.ovip_product_id}: {baseError}"
            );
        }
    }

    var json = await CallPhpProxyAsync(
        request: "getProducts",
        extraData: extraData,
        limitFrom: limitFrom,
        limitTo: limitTo);

    Log($"Raw JSON length: {json.Length}");

    var products = Deserialize<List<OvipProductRemoteDto>>(json);

    Log($"Products received from OVIP: {products.Count}");

    var existingProducts = (await _productLogic.GetAllAsync())
        .ToDictionary(x => x.OvipProductId);

    Log($"Existing products count: {existingProducts.Count}");

    foreach (var item in products)
        await ProcessItemAsync(item, existingProducts);

    Log("===== PRODUCT SYNC END =====");
    Log($"Processed: {result.Processed}");
    Log($"Created: {result.Created}");
    Log($"Updated: {result.Updated}");
    Log($"Errors: {result.Errors.Count}");
    Log($"Warnings: {result.Warnings.Count}");

    result.Logs = logs;

    return JsonSerializer.Serialize(result, new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    });
}

        public async Task<string> SyncCategoryConnectionsAsync()
        {
            var json = await CallPhpProxyAsync("getCategoriesPlus");
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
            return json;
        }

        public async Task<string> SyncPriceListPricesAsync()
        {
            var json = await CallPhpProxyAsync("getPricelist");
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
                            SaleStart = ParseNullableDate(price.sale_start),
                            SaleEnd = ParseNullableDate(price.sale_end)
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
                            SaleStart = ParseNullableDate(price.sale_start),
                            SaleEnd = ParseNullableDate(price.sale_end)
                        });
                    }
                }
            }
            return json;
        }

        public async Task<string> SyncQuantityDiscountsAsync()
        {
            var json = await CallPhpProxyAsync("GetQtyDiscount");
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
            return json;
        }

        public async Task<string> SyncManufacturesAsync()
        {
            var json = await CallPhpProxyAsync("getManufacture");
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
            return json;
        }

        private static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            }) ?? throw new Exception("Az OVIP válasz nem feldolgozható.");
        }

        private static DateTime? ParseNullableDate(string? dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return null;

            return DateTime.TryParse(dateString, out var parsed)
                ? parsed
                : null;
        }

        private sealed class ProductSyncResult
{
    public int Processed { get; set; }
    public int Created { get; set; }
    public int Updated { get; set; }

    public List<string> Errors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
    public List<string> Logs { get; set; } = new();
}

        public async Task<string> CallPhpProxyAsync(
            string request,
            string? extraData = null,
            int? limitFrom = null,
            int? limitTo = null)
        {
            var url = PhpBackendUrl + "?request=" + Uri.EscapeDataString(request);

            if (!string.IsNullOrEmpty(extraData))
                url += "&extra_data=" + Uri.EscapeDataString(extraData);

            if (limitFrom.HasValue)
                url += "&limit_from=" + limitFrom.Value;

            if (limitTo.HasValue)
                url += "&limit_to=" + limitTo.Value;

            using var client = new System.Net.Http.HttpClient();
            var resp = await client.GetAsync(url);
            var body = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
            {
                throw new Exception($"PHP backend call failed ({(int)resp.StatusCode} {resp.ReasonPhrase}): {body}");
            }

            try
            {
                using var doc = JsonDocument.Parse(body);

                if (doc.RootElement.TryGetProperty("success", out var successEl) && successEl.ValueKind == JsonValueKind.False)
                {
                    throw new Exception("PHP backend returned success=false: " + body);
                }

                if (doc.RootElement.TryGetProperty("data", out var dataEl))
                {
                    return dataEl.GetRawText();
                }

                // Fallback: return entire body
                return body;
            }
            catch (JsonException)
            {
                // Not JSON — return raw body
                return body;
            }
        }
        
    }
    
}
