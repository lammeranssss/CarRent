using AutoFixture.Xunit2;
using AutoMapper;
using CarRental.BLL.Models;
using CarRental.BLL.Services;
using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Entities;
using CarRental.Tests.BLL.AutoData;
using NSubstitute;
using Shouldly;

namespace CarRental.Tests.BLL.Services;

public class CustomerServiceTests
{
    [Theory, AutoDataCustomized]
    public async Task GetByIdAsync_ShouldReturnCustomerModel_WhenCustomerExists(
        CustomerEntity customerEntity,
        CustomerModel customerModel,
        [Frozen] ICustomerRepository repository,
        [Frozen] IMapper mapper,
        CustomerService customerService)
    {
        // Arrange
        customerModel.Id = customerEntity.Id;

        repository.GetByIdWithNoTrackingAsync(customerEntity.Id, default).Returns(customerEntity);
        mapper.Map<CustomerModel>(customerEntity).Returns(customerModel);

        // Act
        var result = await customerService.GetByIdAsync(customerEntity.Id);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBe(customerModel);
    }

    [Theory, AutoDataCustomized]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCustomerNotFound(
        Guid customerId,
        [Frozen] ICustomerRepository repository,
        [Frozen] IMapper mapper,
        CustomerService customerService)
    {
        // Arrange
        repository.GetByIdWithNoTrackingAsync(customerId, default).Returns((CustomerEntity?)null);

        mapper.Map<CustomerModel>((CustomerEntity)null!).Returns((CustomerModel)null!);

        // Act
        var result = await customerService.GetByIdAsync(customerId);

        // Assert
        result.ShouldBeNull();
    }

    [Theory, AutoDataCustomized]
    public async Task AddAsync_ShouldMapAndReturnCustomerModel(
        CustomerModel customerModel,
        CustomerEntity customerEntity,
        [Frozen] ICustomerRepository repository,
        [Frozen] IMapper mapper,
        CustomerService customerService)
    {
        // Arrange
        mapper.Map<CustomerEntity>(customerModel).Returns(customerEntity);
        repository.AddAsync(customerEntity, default).Returns(customerEntity);
        mapper.Map<CustomerModel>(customerEntity).Returns(customerModel);

        // Act
        var result = await customerService.AddAsync(customerModel);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(customerModel.Id);
        await repository.Received(1).AddAsync(customerEntity, default);
    }

    [Theory, AutoDataCustomized]
    public async Task RemoveAsync_ShouldCallRepositoryRemove_WhenCustomerExists(
        CustomerEntity customerEntity,
        [Frozen] ICustomerRepository repository,
        CustomerService customerService)
    {
        // Arrange
        repository.GetByIdAsync(customerEntity.Id, default).Returns(customerEntity);

        // Act
        await customerService.RemoveAsync(customerEntity.Id);

        // Assert
        await repository.Received(1).RemoveAsync(customerEntity, default);
    }

    [Theory, AutoDataCustomized]
    public async Task RemoveAsync_ShouldThrowKeyNotFoundException_WhenCustomerNotFound(
        Guid customerId,
        [Frozen] ICustomerRepository repository,
        CustomerService customerService)
    {
        // Arrange
        repository.GetByIdAsync(customerId, default).Returns((CustomerEntity?)null);

        // Act & Assert
        await Should.ThrowAsync<KeyNotFoundException>(() => customerService.RemoveAsync(customerId));
    }
}
