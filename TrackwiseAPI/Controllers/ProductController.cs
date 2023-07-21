using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.ViewModels;
using TrackwiseAPI.Models.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using TrackwiseAPI.Models.DataTransferObjects;

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
                var products = await _productRepository.GetAllProductsAsync();
                var productDtos = products.Select(p => new ProductDTO
                {
                    Product_ID = p.Product_ID,
                    Product_Name = p.Product_Name,
                    Product_Description = p.Product_Description,
                    Product_Price = p.Product_Price,
                    Quantity = p.Quantity,
                    Product_Category = new ProductCategoryDTO
                    {
                        Product_Category_ID = p.ProductCategory.Product_Category_ID,
                        Name = p.ProductCategory.Name,
                        Description = p.ProductCategory.Description,
                    },
                    Product_Type = new ProductTypeDTO
                    {
                        Product_Type_ID = p.ProductType.Product_Type_ID,
                        Name = p.ProductType.Name,
                        Description = p.ProductType.Description,
                    }
                    // Map other properties as needed
                }).ToList();

                return Ok(productDtos);
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
                var product = await _productRepository.GetProductAsync(productId);

                if (product == null)
                    return NotFound("Product does not exist. You need to create it first");

                var productDto = new ProductDTO
                {
                    Product_ID = product.Product_ID,
                    Product_Name = product.Product_Name,
                    Product_Description = product.Product_Description,
                    Product_Price = product.Product_Price,
                    Quantity = product.Quantity,
                    Product_Category = new ProductCategoryDTO
                    {
                        Product_Category_ID = product.ProductCategory.Product_Category_ID,
                        Name = product.ProductCategory.Name,
                        Description = product.ProductCategory.Description,
                    },
                    Product_Type = new ProductTypeDTO
                    {
                        Product_Type_ID = product.ProductType.Product_Type_ID,
                        Name = product.ProductType.Name,
                        Description = product.ProductType.Description,
                    }
                    // Map other properties as needed
                };

                return Ok(productDto);
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
            var productId = Guid.NewGuid().ToString();

            var product = new Product 
            {
                Product_ID = productId, 
                Product_Name = prodvm.Product_Name, 
                Product_Description = prodvm.Product_Description, 
                Product_Price = prodvm.Product_Price, 
                Quantity = prodvm.Product_Quantity ,
                Product_Category_ID = prodvm.Product_Category_ID, 
                Product_Type_ID = prodvm.Product_Type_ID };

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
                    existingProduct.Product_Price == productModel.Product_Price &&
                    existingProduct.Quantity == productModel.Product_Quantity)
                {
                    // No changes made, return the existing driver without updating
                    return Ok(existingProduct);
                }

                existingProduct.Product_Name = productModel.Product_Name;
                existingProduct.Product_Description = productModel.Product_Description;
                existingProduct.Product_Price = productModel.Product_Price;
                existingProduct.Quantity = productModel.Product_Quantity;



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

