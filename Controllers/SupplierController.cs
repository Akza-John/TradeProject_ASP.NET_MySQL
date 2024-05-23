using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTrade.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ProjectTrade.Models.CommonModel;

namespace ProjectTrade.Controllers
{
    [ApiController]
    [Route("api/supplier/")]
    public class SupplierController : ControllerBase
    {
        private readonly CustomerContext _context; 

        public SupplierController(CustomerContext context)
        {
            _context = context;
        }


        [HttpGet("count")]
        public async Task<ActionResult<CountResponse>> GetSupplierCount()
        {
            var count = await _context.SupplierDetail.CountAsync();
            return Ok(new CountResponse { Count = count });
        }
        [HttpGet("supplierdetails")]
        public async Task<ActionResult<IEnumerable<SupplierDetail>>> GetSupplierDetails()
        {
            return await _context.SupplierDetail.ToListAsync();
        }
    }
}

