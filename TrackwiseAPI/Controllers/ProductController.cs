using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.ViewModels;
using TrackwiseAPI.Models.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        [Route("GetAllProducts")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Customer")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var results = await _productRepository.GetAllProductsAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetProduct/{productId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Customer")]
        public async Task<IActionResult> GetProductAsync(string productId)
        {
            try
            {
                var result = await _productRepository.GetProductAsync(productId);

                if (result == null) return NotFound("Product does not exist. You need to create it first");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }

        [HttpPost]
        [Route("AddProduct")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> AddProduct(ProductVM prodvm)
        {
            var product = new Product { Product_Name = prodvm.Product_Name, Product_Description = prodvm.Product_Description, Product_Price = prodvm.Product_Price,Quantity = prodvm.Product_Quantity ,Product_Category_ID = prodvm.Product_Category_ID, Product_Type_ID = prodvm.Product_Type_ID };

            try
            {
                _productRepository.Add(product);
                await _productRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(product);
        }


        [HttpPut]
        [Route("EditProduct/{productId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<ProductVM>> EditProduct(string productId, ProductVM productModel)
        {
            try
            {
                var existingProduct = await _productRepository.GetProductAsync(productId);
                if (existingProduct == null) return NotFound($"The product does not exist");

                if (existingProduct.Product_Name == productModel.Product_Name &&
                    existingProduct.Product_Description == productModel.Product_Description &&
                    existingProduct.Product_Price == productModel.Product_Price)
                {
                    // No changes made, return the existing driver without updating
                    return Ok(existingProduct);
                }

                existingProduct.Product_Name = productModel.Product_Name;
                existingProduct.Product_Description = productModel.Product_Description;
                existingProduct.Product_Price = productModel.Product_Price;
                



                if (await _productRepository.SaveChangesAsync())
                {
                    return Ok(existingProduct);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }


        [HttpDelete]
        [Route("DeleteProduct/{productId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(string productId)
        {
            try
            {
                var existingProduct = await _productRepository.GetProductAsync(productId);

                if (existingProduct == null) return NotFound($"The product does not exist");

                _productRepository.Delete(existingProduct);

                if (await _productRepository.SaveChangesAsync()) return Ok(existingProduct);

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }


    }
}

