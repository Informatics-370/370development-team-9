using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.ViewModels;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        [HttpGet]
        [Route("GetAllSuppliers")]
        public async Task<IActionResult> GetAllSuppliers()
        {
            try
            {
                var results = await _supplierRepository.GetAllSuppliersAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetSupplier/{supplierId}")]
        public async Task<IActionResult> GetSupplierAsync(string supplierId)
        {
            try
            {
                var result = await _supplierRepository.GetSupplierAsync(supplierId);

                if (result == null) return NotFound("Supplier does not exist. You need to create it first");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }

        [HttpPost]
        [Route("AddSupplier")]
        public async Task<IActionResult> AddSupplier(SupplierVM supvm)
        {
            var supplier = new Supplier { Name = supvm.Name, Email = supvm.Email, Contact_Number = supvm.Contact_Number};

            try
            {
                _supplierRepository.Add(supplier);
                await _supplierRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(supplier);
        }


        [HttpPut]
        [Route("EditSupplier/{supplierId}")]
        public async Task<ActionResult<SupplierVM>> EditSupplier(string supplierId, SupplierVM supplierModel)
        {
            try
            {
                var existingSupplier = await _supplierRepository.GetSupplierAsync(supplierId);
                if (existingSupplier == null) return NotFound($"The supplier does not exist");

                if (existingSupplier.Name == supplierModel.Name &&
                    existingSupplier.Email == supplierModel.Email &&
                    existingSupplier.Contact_Number == supplierModel.Contact_Number)
                {
                    // No changes made, return the existing driver without updating
                    return Ok(existingSupplier);
                }

                existingSupplier.Name = supplierModel.Name;
                existingSupplier.Email = supplierModel.Email;
                existingSupplier.Contact_Number = supplierModel.Contact_Number;



                if (await _supplierRepository.SaveChangesAsync())
                {
                    return Ok(existingSupplier);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }


        [HttpDelete]
        [Route("DeleteSupplier/{supplierId}")]
        public async Task<IActionResult> DeleteSupplier(string supplierId)
        {
            try
            {
                var existingSupplier = await _supplierRepository.GetSupplierAsync(supplierId);

                if (existingSupplier == null) return NotFound($"The supplier does not exist");

                _supplierRepository.Delete(existingSupplier);

                if (await _supplierRepository.SaveChangesAsync()) return Ok(existingSupplier);

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }


    }
}
