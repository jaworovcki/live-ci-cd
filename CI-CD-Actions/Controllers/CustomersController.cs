using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CI_CD_Actions.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomersController : ControllerBase
	{
        private readonly CustomerContext _context;

		public CustomersController(CustomerContext context)
		{
			_context = context;
		}

		[HttpGet(Name = nameof(GetCustomers))]
		public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers() 
			=> await _context.Customers
				.AsNoTracking()
				.ToListAsync();

		[HttpGet("{id}", Name = nameof(GetCustomer))]
		public async Task<ActionResult<Customer>> GetCustomer(int id)
		{
			var customer = await _context.Customers.FindAsync(id);

			if (customer == null)
			{
				return NotFound();
			}

			return Ok(customer);
		}

		[HttpPost]
		public async Task<ActionResult<Customer>> AddCustomer([FromBody] Customer customer)
		{
			_context.Customers.Add(customer);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
		}

	}
}
