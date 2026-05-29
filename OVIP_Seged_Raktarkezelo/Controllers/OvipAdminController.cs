using Logic.Logic.CategoriesLogic.Interfaces;
using Logic.Logic.ManufactureLogic.Interfaces;
using Logic.Logic.ParametersLogic.Interfaces;
using Logic.Logic.PricingLogic.Interfaces;
using Logic.Logic.ProductsLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Categories;
using Models.Dtos.Pircing;
using Models.Dtos.Products;
using Models.Entities.Categories;
using Models.Entities.Pricing;
using Models.Entities.Products;

namespace OVIP_Seged_Raktarkezelo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OvipAdminController : ControllerBase
    {
        // =========================
        // LOGICS
        // =========================

        private readonly IOvipProductLogic _productLogic;
        private readonly IOvipProductImageLogic _imageLogic;
        private readonly IOvipProductParameterLogic _productParameterLogic;
        private readonly IOvipStockLogic _stockLogic;

        private readonly IOvipCategoryLogic _categoryLogic;
        private readonly IOvipCategoryConnectionLogic _categoryConnectionLogic;

        private readonly IOvipParameterLogic _parameterLogic;

        private readonly IOvipPriceListLogic _priceListLogic;
        private readonly IOvipPriceListPriceLogic _priceListPriceLogic;
        private readonly IOvipQuantityDiscountLogic _quantityDiscountLogic;

        private readonly IOvipManufactureLogic _manufactureLogic;
        private readonly IOvipManufacturePartLogic _manufacturePartLogic;

        public OvipAdminController(
            IOvipProductLogic productLogic,
            IOvipProductImageLogic imageLogic,
            IOvipProductParameterLogic productParameterLogic,
            IOvipStockLogic stockLogic,
            IOvipCategoryLogic categoryLogic,
            IOvipCategoryConnectionLogic categoryConnectionLogic,
            IOvipParameterLogic parameterLogic,
            IOvipPriceListLogic priceListLogic,
            IOvipPriceListPriceLogic priceListPriceLogic,
            IOvipQuantityDiscountLogic quantityDiscountLogic,
            IOvipManufactureLogic manufactureLogic,
            IOvipManufacturePartLogic manufacturePartLogic)
        {
            _productLogic = productLogic;
            _imageLogic = imageLogic;
            _productParameterLogic = productParameterLogic;
            _stockLogic = stockLogic;

            _categoryLogic = categoryLogic;
            _categoryConnectionLogic = categoryConnectionLogic;

            _parameterLogic = parameterLogic;

            _priceListLogic = priceListLogic;
            _priceListPriceLogic = priceListPriceLogic;
            _quantityDiscountLogic = quantityDiscountLogic;

            _manufactureLogic = manufactureLogic;
            _manufacturePartLogic = manufacturePartLogic;
        }

        // =========================================================
        // PRODUCTS
        // =========================================================

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(await _productLogic.GetAllAsync());
        }

        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            return Ok(await _productLogic.GetByIdAsync(id));
        }

        [HttpPost("products")]
        public async Task<IActionResult> CreateProduct(OvipProductCreateDto product)
        {
            return Ok(await _productLogic.CreateAsync(product));
        }

        [HttpPut("products")]
        public async Task<IActionResult> UpdateProduct(OvipProductUpdateDto product)
        {
            return Ok(await _productLogic.UpdateAsync(product));
        }

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return Ok(await _productLogic.DeleteAsync(id));
        }

        // =========================================================
        // IMAGES
        // =========================================================

        [HttpGet("images")]
        public async Task<IActionResult> GetImages()
        {
            return Ok(await _imageLogic.GetAllAsync());
        }

        [HttpGet("images/product/{productId}")]
        public async Task<IActionResult> GetImagesByProduct(int productId)
        {
            return Ok(await _imageLogic.GetByProductIdAsync(productId));
        }

        [HttpPost("images")]
        public async Task<IActionResult> CreateImage(OvipProductImageCreateDto image)
        {
            return Ok(await _imageLogic.CreateAsync(image));
        }

        [HttpPut("images")]
        public async Task<IActionResult> UpdateImage(OvipProductImageUpdateDto image)
        {
            return Ok(await _imageLogic.UpdateAsync(image));
        }

        [HttpDelete("images/{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            return Ok(await _imageLogic.DeleteAsync(id));
        }

        // =========================================================
        // STOCK
        // =========================================================

        [HttpGet("stock")]
        public async Task<IActionResult> GetStock()
        {
            return Ok(await _stockLogic.GetAllAsync());
        }

        [HttpGet("stock/product/{productId}")]
        public async Task<IActionResult> GetStockByProduct(int productId)
        {
            return Ok(await _stockLogic.GetByProductIdAsync(productId));
        }

        [HttpPost("stock")]
        public async Task<IActionResult> CreateStock(OvipStockCreateDto stock)
        {
            return Ok(await _stockLogic.CreateAsync(stock));
        }

        [HttpPut("stock")]
        public async Task<IActionResult> UpdateStock(OvipStockUpdateDto stock)
        {
            return Ok(await _stockLogic.UpdateAsync(stock));
        }

        // =========================================================
        // CATEGORIES
        // =========================================================

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _categoryLogic.GetAllAsync());
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory(OvipCategoryCreateDto category)
        {
            return Ok(await _categoryLogic.CreateAsync(category));
        }

        // =========================================================
        // PRICE LIST
        // =========================================================

        [HttpGet("pricelists")]
        public async Task<IActionResult> GetPriceLists()
        {
            return Ok(await _priceListLogic.GetAllAsync());
        }

        [HttpGet("pricelists/{id}")]
        public async Task<IActionResult> GetPriceList(int id)
        {
            return Ok(await _priceListLogic.GetByIdAsync(id));
        }

        [HttpPost("pricelists")]
        public async Task<IActionResult> CreatePriceList(OvipPriceListCreateDto entity)
        {
            return Ok(await _priceListLogic.CreateAsync(entity));
        }

        // =========================================================
        // PRICE LIST PRICES
        // =========================================================

        [HttpGet("prices")]
        public async Task<IActionResult> GetPrices()
        {
            return Ok(await _priceListPriceLogic.GetAllAsync());
        }

        [HttpGet("prices/product/{productId}")]
        public async Task<IActionResult> GetPricesByProduct(int productId)
        {
            return Ok(await _priceListPriceLogic.GetByProductIdAsync(productId));
        }

        // =========================================================
        // DISCOUNTS
        // =========================================================

        [HttpGet("discounts")]
        public async Task<IActionResult> GetDiscounts()
        {
            return Ok(await _quantityDiscountLogic.GetAllAsync());
        }

        [HttpGet("discounts/product/{productId}")]
        public async Task<IActionResult> GetDiscountsByProduct(int productId)
        {
            return Ok(await _quantityDiscountLogic.GetByProductIdAsync(productId));
        }

        // =========================================================
        // PARAMETERS
        // =========================================================

        [HttpGet("parameters")]
        public async Task<IActionResult> GetParameters()
        {
            return Ok(await _parameterLogic.GetAllAsync());
        }

        // =========================================================
        // MANUFACTURE
        // =========================================================

        [HttpGet("manufacture")]
        public async Task<IActionResult> GetManufactures()
        {
            return Ok(await _manufactureLogic.GetAllAsync());
        }

        [HttpGet("manufacture/product/{productId}")]
        public async Task<IActionResult> GetManufactureByProduct(int productId)
        {
            return Ok(await _manufactureLogic.GetByProductIdAsync(productId));
        }

        [HttpGet("manufactureparts")]
        public async Task<IActionResult> GetManufactureParts()
        {
            return Ok(await _manufacturePartLogic.GetAllAsync());
        }
    }
}
