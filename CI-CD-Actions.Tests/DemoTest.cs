using CI_CD_Actions.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CI_CD_Actions.Tests
{
    public class DemoTest
    {
        [Fact]
        public void Test1()
        {
            Assert.True(1 == 1);
        }

        [Fact]
        public async Task CustomerIntegrationTest()
        {
            //Create a dbContext
            var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
				.Build();

            var optionsBuilder = new DbContextOptionsBuilder<CustomerContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            var context = new CustomerContext(optionsBuilder.Options);

            //Delete all existing customers
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            //Create Controller
            var controller = new CustomersController(context);

            //Add a customer
            await controller.AddCustomer(new Customer()
            {
                CustomerName = "FooBar",
            });

            //Get all customers
            var customers = await controller.GetCustomers();

            //Assert
            Assert.Single(customers.Value);
            Assert.Equal("FooBar", customers.Value.First().CustomerName);
        }
    }
}