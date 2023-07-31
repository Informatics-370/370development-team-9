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
                    Image = p.Image,
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
                    Image = product.Image,
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
        public async Task<IActionResult> AddProduct(ProductDTO product)
        {
            var productId = Guid.NewGuid().ToString();


            var newProduct = new Product
            {
                Product_ID = productId,
                Product_Name = product.Product_Name,
                Product_Description = product.Product_Description,
                Product_Price = product.Product_Price,
                Quantity = product.Quantity,
                Image = product.Image,
                Product_Category_ID = product.Product_Category.Product_Category_ID,
                Product_Type_ID = product.Product_Type.Product_Type_ID
                // Map other properties as needed
            };

            try
            {
                _productRepository.Add(newProduct);
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
        public async Task<ActionResult<ProductVM>> EditProduct(string productId, ProductDTO productModel)
        {
            try
            {
                var existingProduct = await _productRepository.GetProductAsync(productId);
                if (existingProduct == null) return NotFound($"The product does not exist");

                if (existingProduct.Product_Name == productModel.Product_Name &&
                    existingProduct.Product_Description == productModel.Product_Description &&
                    existingProduct.Product_Price == productModel.Product_Price &&
                    existingProduct.Quantity == productModel.Quantity &&
                    existingProduct.Image == productModel.Image &&
                    existingProduct.Product_Category_ID == productModel.Product_Category.Product_Category_ID &&
                    existingProduct.Product_Type_ID == productModel.Product_Type.Product_Type_ID)
                {
                    // No changes made, return the existing driver without updating
                    return Ok(existingProduct);
                }

                existingProduct.Product_Name = productModel.Product_Name;
                existingProduct.Product_Description = productModel.Product_Description;
                existingProduct.Product_Price = productModel.Product_Price;
                existingProduct.Quantity = productModel.Quantity;
                existingProduct.Image = productModel.Image;
                existingProduct.Product_Category_ID = productModel.Product_Category.Product_Category_ID;
                existingProduct.Product_Type_ID = productModel.Product_Type.Product_Type_ID;



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

        [HttpGet]
        [Route("GetProductCategory")]
        public async Task<IActionResult> GetProductCategory()
        {
            try
            {
                var results = await _productRepository.GetProductCategoryAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetProductType")]
        public async Task<IActionResult> GetProductType()
        {
            try
            {
                var results = await _productRepository.GetProductTypeAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

    }
}

