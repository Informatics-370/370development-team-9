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
using System.Security.Claims;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IAuditRepository _auditRepository;
        public ProductController(IProductRepository productRepository, IAuditRepository auditRepository)
        {
            _productRepository = productRepository;
            _auditRepository = auditRepository;
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
                    ListStatus = p.ListStatus,
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
                    ListStatus = product.ListStatus,
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
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var auditId = Guid.NewGuid().ToString();
            var audit = new Audit { Audit_ID = auditId, Action = "Add Product", CreatedDate = DateTime.Now, User = userEmail };

            var newProduct = new Product
            {
                Product_ID = productId,
                Product_Name = product.Product_Name,
                Product_Description = product.Product_Description,
                Product_Price = product.Product_Price,
                Quantity = product.Quantity,
                ListStatus = true,
                Image = product.Image,
                Product_Category_ID = product.Product_Category.Product_Category_ID,
                Product_Type_ID = product.Product_Type.Product_Type_ID
                // Map other properties as needed
            };

            try
            {
                _productRepository.Add(newProduct);
                await _productRepository.SaveChangesAsync();
                _auditRepository.Add(audit);
                await _auditRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(product);
        }

        [HttpPost]
        [Route("UploadProducts")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UploadProducts(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file.");
            }

            // Read and parse the CSV file content here
            using (var streamReader = new StreamReader(file.OpenReadStream()))
            using (var csvReader = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var products = csvReader.GetRecords<ProductCsvModel>().ToList();

                // Process each product and add it to the database
                foreach (var product in products)
                {
                    var productId = Guid.NewGuid().ToString();
                    var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    var auditId = Guid.NewGuid().ToString();
                    var audit = new Audit { Audit_ID = auditId, Action = "Add Product", CreatedDate = DateTime.Now, User = userEmail };

                    var productCategory = _productRepository.FindByName(product.Product_Category_ID);
                    if (productCategory == null)
                    {
                        // Handle the case where the category is not found
                        return BadRequest($"Category not found: {product.Product_Category_ID}");
                    }

                    var productType = _productRepository.FindTypeByName(product.Product_Type_ID);
                    if (productType == null)
                    {
                        // Handle the case where the category is not found
                        return BadRequest($"Type not found: {product.Product_Type_ID}");
                    }

                    var newProduct = new Product
                    {
                        Product_ID = productId,
                        Product_Name = product.Product_Name,
                        Product_Description = product.Product_Description,
                        Product_Price = product.Product_Price,
                        Quantity = product.Quantity,
                        ListStatus = product.ListStatus,
                        Image = product.Image,
                        Product_Category_ID = productCategory.Product_Category_ID,
                        Product_Type_ID = productType.Product_Type_ID
                        // Map other properties as needed
                    };

                    try
                    {
                        _productRepository.Add(newProduct);
                        await _productRepository.SaveChangesAsync();
                        _auditRepository.Add(audit);
                        await _auditRepository.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        // Handle exceptions appropriately
                        return BadRequest("Invalid transaction");
                    }
                }
            }

            return Ok("Products uploaded successfully");
        }

        public class ProductCsvModel
        {
            [Name("product_name")]
            public string Product_Name { get; set; }

            [Name("product_description")]
            public string Product_Description { get; set; }

            [Name("product_price")]
            public double Product_Price { get; set; }

            [Name("quantity")]
            public int Quantity { get; set; }

            [Name("list_status")]
            [Optional]
            public bool ListStatus { get; set; }

            [Name("image")]
            [Optional]
            public string? Image { get; set; }
            [Name("product_category_id")]
            public string Product_Category_ID { get; set; }
            [Name("product_type_id")]
            public string Product_Type_ID { get; set; }
        }

        [HttpGet]
        [Route("Getcsv")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetProductsCsv()
        {

            var csvPath = Path.Combine(Environment.CurrentDirectory, $"products-{DateTime.Now.ToFileTime()}.csv");
            using (var streamWriter = new StreamWriter(csvPath))
            {
                using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    var products = await _productRepository.GetAllProductsAsync();
                    csvWriter.WriteRecords(products);
                }
            }
            return Ok("Addded");
        }

        [HttpGet]
        [Route("Getjson")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetProductsJson()
        {

            var products = _productRepository.GetAllProductsAsync();
            
            var json1 = JsonSerializer.Serialize(products);
            string filePath = "C:/Users/Renier/Downloads/products.json";
            System.IO.File.WriteAllText(filePath, json1);
            /*
            // Retrieve and serialize your product data
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(products);

            // Set the response content type to JSON
            System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();
            response.Content = new System.Net.Http.StringContent(json);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            // Set the file name for download
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = "products.json"
            };
            */
            return Ok();
        }


        [HttpPut]
        [Route("EditProduct/{productId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<ProductVM>> EditProduct(string productId, ProductDTO productModel)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var auditId = Guid.NewGuid().ToString();
            var audit = new Audit { Audit_ID = auditId, Action = "Update Product", CreatedDate = DateTime.Now, User = userEmail };
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
                    _auditRepository.Add(audit);
                    await _auditRepository.SaveChangesAsync();
                    return Ok(existingProduct);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

        [HttpPut]
        [Route("UnlistProduct/{productId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UnlistProduct(string productId)
        {
            try
            {
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var auditId = Guid.NewGuid().ToString();
                var audit = new Audit { Audit_ID = auditId, Action = "Unlist Product", CreatedDate = DateTime.Now, User = userEmail };
                var product = await _productRepository.GetProductAsync(productId);

                product.ListStatus = false;

                if (await _productRepository.SaveChangesAsync())
                {
                    return Ok(product);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

        [HttpPut]
        [Route("RelistProduct/{productId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> RelistProduct(string productId)
        {
            try
            {
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var auditId = Guid.NewGuid().ToString();
                var audit = new Audit { Audit_ID = auditId, Action = "Relist Product", CreatedDate = DateTime.Now, User = userEmail };
                var product = await _productRepository.GetProductAsync(productId);

                product.ListStatus = true;

                if (await _productRepository.SaveChangesAsync())
                {
                    return Ok(product);
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
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var auditId = Guid.NewGuid().ToString();
                var audit = new Audit { Audit_ID = auditId, Action = "Delete Product", CreatedDate = DateTime.Now, User = userEmail };
                var existingProduct = await _productRepository.GetProductAsync(productId);

                if (existingProduct == null) return NotFound($"The product does not exist");

                _productRepository.Delete(existingProduct);

                if (await _productRepository.SaveChangesAsync()) 
                {
                    _auditRepository.Add(audit);
                    await _auditRepository.SaveChangesAsync();
                    return Ok(existingProduct);
                }

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

        [HttpGet]
        [Route("GetSpesificProductType/{product_Type_ID}")]
        public async Task<IActionResult> GetSpesificProductTypeAsync(string product_Type_ID)
        {
            try
            {
                var result = await _productRepository.GetSpesificProductTypeAsync(product_Type_ID);

                var productType = result.Select(type => new ProductSpesificTypeDTO
                {
                    Product_Type_ID = type.Product_Type_ID,
                    Name = type.ProductType.Name,
                    Description = type.ProductType.Description,
                    Product_Category = new ProductSpesificCategoryDTO
                    {
                        Product_Category_ID = type.ProductCategory.Product_Category_ID,
                        Name = type.ProductCategory.Name,
                        Description = type.ProductCategory.Description
                    }
                }).ToList();

                return Ok(productType);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }

        [HttpGet]
        [Route("GetSpesificProductCategory/{product_Category_ID}")]
        public async Task<IActionResult> GetSpesificProductCatgegoryAsync(string product_Category_ID)
        {
            try
            {
                var result = await _productRepository.GetSpesificProductCategoryAsync(product_Category_ID);

                var productCategory = result.Select(type => new ProductSpesificCategoryDTO
                {
                    Product_Category_ID = type.Product_Category_ID,
                    Name = type.ProductCategory.Name,
                    Description = type.ProductCategory.Description,
                    Product_Type = new ProductSpesificTypeDTO
                    {
                        Product_Type_ID = type.ProductType.Product_Type_ID,
                        Name = type.ProductType.Name,
                        Description = type.ProductType.Description
                    }
                }).ToList();

                return Ok(productCategory);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }

    }
}

