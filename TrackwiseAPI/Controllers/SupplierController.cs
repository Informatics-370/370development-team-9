using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
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
        private readonly IAuditRepository _auditRepository;

        public SupplierController(ISupplierRepository supplierRepository, IAuditRepository auditRepository)
        {
            _supplierRepository = supplierRepository;
            _auditRepository = auditRepository;
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
            var supplierId = Guid.NewGuid().ToString();
            var supplier = new Supplier {Supplier_ID = supplierId, Name = supvm.Name, Email = supvm.Email, Contact_Number = supvm.Contact_Number};

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var auditId = Guid.NewGuid().ToString();
            var audit = new Audit { Audit_ID = auditId, Action = "Add Supplier", CreatedDate = DateTime.Now, User = userEmail };

            try
            {
                _supplierRepository.Add(supplier);
                await _supplierRepository.SaveChangesAsync();
                _auditRepository.Add(audit);
                await _auditRepository.SaveChangesAsync();
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
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var auditId = Guid.NewGuid().ToString();
            var audit = new Audit { Audit_ID = auditId, Action = "Update Supplier", CreatedDate = DateTime.Now, User = userEmail };
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
                    _auditRepository.Add(audit);
                    await _auditRepository.SaveChangesAsync();
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
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var auditId = Guid.NewGuid().ToString();
                var audit = new Audit { Audit_ID = auditId, Action = "Delete Supplier", CreatedDate = DateTime.Now, User = userEmail };
                var existingSupplier = await _supplierRepository.GetSupplierAsync(supplierId);

                if (existingSupplier == null) return NotFound($"The supplier does not exist");

                _supplierRepository.Delete(existingSupplier);

                if (await _supplierRepository.SaveChangesAsync())
                {
                    _auditRepository.Add(audit);
                    await _auditRepository.SaveChangesAsync();
                    return Ok(existingSupplier);
                }
                    

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }


    }
}
