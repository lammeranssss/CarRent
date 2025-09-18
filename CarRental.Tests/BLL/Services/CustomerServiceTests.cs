using AutoFixture;
using AutoMapper;
using CarRental.BLL.Mapping;
using CarRental.BLL.Models;
using CarRental.BLL.Services;
using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;

namespace CarRental.Tests.BLL.Services;

public class CustomerServiceTests
{
    private readonly IFixture _fixture;
    private readonly ICustomerRepository _repository;
    private readonly Mapper _mapper;
    private readonly CustomerService _sut;

    public CustomerServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _repository = Substitute.For<ICustomerRepository>();

        var loggerFactory = Substitute.For<ILoggerFactory>();
        var expression = new MapperConfigurationExpression();
        expression.AddProfile<BllMappingProfile>();
        var config = new MapperConfiguration(expression, loggerFactory);
        _mapper = new Mapper(config);

        _sut = new CustomerService(_repository, _mapper);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByIdAsync_ShouldReturnCustomerModel_WhenCustomerExists()
    {
        var customerEntity = _fixture.Build<CustomerEntity>()
            .Without(c => c.Bookings)
            .Create();

        _repository.GetByIdWithNoTrackingAsync(customerEntity.Id, default).Returns(customerEntity);

        var result = await _sut.GetByIdAsync(customerEntity.Id);

        result.ShouldNotBeNull();
        result!.Id.ShouldBe(customerEntity.Id);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByIdAsync_ShouldReturnNull_WhenCustomerNotFound()
    {
        _repository.GetByIdWithNoTrackingAsync(Arg.Any<Guid>(), default).Returns((CustomerEntity?)null);

        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        result.ShouldBeNull();
    }

    [Fact]
    public async System.Threading.Tasks.Task AddAsync_ShouldMapAndReturnCustomerModel()
    {
        var customerModel = _fixture.Build<CustomerModel>()
            .Without(c => c.Bookings)
            .Create();

        var customerEntity = _mapper.Map<CustomerEntity>(customerModel);

        _repository.AddAsync(Arg.Any<CustomerEntity>(), default).Returns(customerEntity);

        var result = await _sut.AddAsync(customerModel);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(customerModel.Id);
        await _repository.Received(1).AddAsync(Arg.Any<CustomerEntity>(), default);
    }

    [Fact]
    public async System.Threading.Tasks.Task RemoveAsync_ShouldCallRepositoryRemove_WhenCustomerExists()
    {
        var customerEntity = _fixture.Build<CustomerEntity>()
            .Without(c => c.Bookings)
            .Create();

        _repository.GetByIdAsync(customerEntity.Id, default).Returns(customerEntity);

        await _sut.RemoveAsync(customerEntity.Id);

        await _repository.Received(1).RemoveAsync(customerEntity, default);
    }

    [Fact]
    public async System.Threading.Tasks.Task RemoveAsync_ShouldThrow_WhenCustomerNotFound()
    {
        _repository.GetByIdAsync(Arg.Any<Guid>(), default).Returns((CustomerEntity?)null);

        await Should.ThrowAsync<KeyNotFoundException>(() => _sut.RemoveAsync(Guid.NewGuid()));
    }
}
