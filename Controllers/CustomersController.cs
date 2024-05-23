using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTrade.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ProjectTrade.Models.CommonModel;


namespace ProjectTrade.Controllers
{
    [ApiController]
    [Route("api/customers/")]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerContext _context;

        public CustomersController(CustomerContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Gets the count of customers.
        /// </summary>
        /// <returns>The count of customers as an integer.</returns>
        [HttpGet("count")]
        public async Task<ActionResult<CountResponse>> GetCustomerCount()
        {
            var count = await _context.CustomersDetail.CountAsync();
            return Ok(new CountResponse { Count = count });
        }
        [HttpGet("customerdetails")]
        public async Task<ActionResult<IEnumerable<CustomerDetail>>> GetCustomerDetails()
        {
            return await _context.CustomersDetail.ToListAsync();
        }
    }

}
