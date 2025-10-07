using System.Net;
using System.Text;
using System.Text.Json;
using CarRental.API.Models.Requests.Customers;
using CarRental.API.Models.Responses.Customers;
using CarRental.DAL.DataContext;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace CarRental.IntegrationTests.Controllers;

public class CustomersControllerTests(CustomWebApplicationFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task GetAll_WhenCustomersExist_ReturnsCustomersList()
    {
        // Arrange
        var customers = new[]
        {
            TestDataHelper.CreateCustomerEntity(),
            TestDataHelper.CreateCustomerEntity()
        };
        await AddEntitiesAsync(customers);

        // Act
        var response = await Client.GetAsync("/api/customers");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = Deserialize<List<CustomerResponse>>(await response.Content.ReadAsStringAsync());

        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetById_WhenCustomerExists_ReturnsCustomer()
    {
        // Arrange
        var customer = await AddEntityAsync(TestDataHelper.CreateCustomerEntity());

        // Act
        var response = await Client.GetAsync($"/api/customers/{customer.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = Deserialize<CustomerResponse>(await response.Content.ReadAsStringAsync());

        result.ShouldNotBeNull();
        result.Id.ShouldBe(customer.Id);
    }

    [Fact]
    public async Task GetById_WhenCustomerNotExists_ReturnsNotFound()
    {
        // Act
        var response = await Client.GetAsync($"/api/customers/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WithValidRequest_ReturnsCreatedCustomer()
    {
        // Arrange
        var customerData = TestDataHelper.CreateCustomerEntity();
        var request = new CreateCustomerRequest(
            FirstName: customerData.FirstName,
            LastName: customerData.LastName,
            Email: customerData.Email,
            Phone: customerData.Phone,
            Address: customerData.Address,
            LicenseNumber: customerData.LicenseNumber
        );
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("/api/customers", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = Deserialize<CustomerResponse>(await response.Content.ReadAsStringAsync());

        result.ShouldNotBeNull();
        result.FirstName.ShouldBe(request.FirstName);
        result.Email.ShouldBe(request.Email);
    }

    [Fact]
    public async Task Update_WithValidRequest_ReturnsUpdatedCustomer()
    {
        // Arrange
        var customer = await AddEntityAsync(TestDataHelper.CreateCustomerEntity());
        var updatedCustomerData = TestDataHelper.CreateCustomerEntity(firstName: "UpdatedFirstName", lastName: "UpdatedLastName");

        var request = new CreateCustomerRequest(
            FirstName: updatedCustomerData.FirstName,
            LastName: updatedCustomerData.LastName,
            Email: customer.Email,
            Phone: customer.Phone,
            Address: customer.Address,
            LicenseNumber: customer.LicenseNumber
        );
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PutAsync($"/api/customers/{customer.Id}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = Deserialize<CustomerResponse>(await response.Content.ReadAsStringAsync());

        result.ShouldNotBeNull();
        result.FirstName.ShouldBe(request.FirstName);
        result.LastName.ShouldBe(request.LastName);
    }

    [Fact]
    public async Task Delete_WhenCustomerExists_RemovesCustomer()
    {
        // Arrange
        var customer = await AddEntityAsync(TestDataHelper.CreateCustomerEntity());

        // Act
        var response = await Client.DeleteAsync($"/api/customers/{customer.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var scope = CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();
        var deletedCustomer = await dbContext.Customers.FindAsync(customer.Id);
        deletedCustomer.ShouldBeNull();
    }
}
