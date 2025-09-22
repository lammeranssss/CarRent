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

public class RentalServiceTests
{
    private readonly IFixture _fixture;
    private readonly IRentalRepository _repository;
    private readonly Mapper _mapper;
    private readonly RentalService _sut;

    public RentalServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _repository = Substitute.For<IRentalRepository>();

        var loggerFactory = Substitute.For<ILoggerFactory>();
        var expression = new MapperConfigurationExpression();
        expression.AddProfile<BllMappingProfile>();
        var config = new MapperConfiguration(expression, loggerFactory);
        _mapper = new Mapper(config);

        _sut = new RentalService(_repository, _mapper);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByIdAsync_ShouldReturnRentalModel_WhenRentalExists()
    {
        var rentalEntity = _fixture.Build<RentalEntity>()
            .Without(r => r.Booking)
            .Without(r => r.PickUpLocation)
            .Without(r => r.DropOffLocation)
            .Create();

        _repository.GetByIdWithNoTrackingAsync(rentalEntity.Id, default).Returns(rentalEntity);

        var result = await _sut.GetByIdAsync(rentalEntity.Id);

        result.ShouldNotBeNull();
        result!.Id.ShouldBe(rentalEntity.Id);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByIdAsync_ShouldReturnNull_WhenRentalNotFound()
    {
        _repository.GetByIdWithNoTrackingAsync(Arg.Any<Guid>(), default).Returns((RentalEntity?)null);

        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        result.ShouldBeNull();
    }

    [Fact]
    public async System.Threading.Tasks.Task AddAsync_ShouldMapAndReturnRentalModel()
    {
        var rentalModel = _fixture.Build<RentalModel>()
            .Without(r => r.Booking)
            .Without(r => r.PickUpLocation)
            .Without(r => r.DropOffLocation)
            .Create();

        var rentalEntity = _mapper.Map<RentalEntity>(rentalModel);

        _repository.AddAsync(Arg.Any<RentalEntity>(), default).Returns(rentalEntity);

        var result = await _sut.AddAsync(rentalModel);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(rentalModel.Id);
        await _repository.Received(1).AddAsync(Arg.Any<RentalEntity>(), default);
    }

    [Fact]
    public async System.Threading.Tasks.Task RemoveAsync_ShouldCallRepositoryRemove_WhenRentalExists()
    {
        var rentalEntity = _fixture.Build<RentalEntity>()
            .Without(r => r.Booking)
            .Without(r => r.PickUpLocation)
            .Without(r => r.DropOffLocation)
            .Create();

        _repository.GetByIdAsync(rentalEntity.Id, default).Returns(rentalEntity);

        await _sut.RemoveAsync(rentalEntity.Id);

        await _repository.Received(1).RemoveAsync(rentalEntity, default);
    }

    [Fact]
    public async System.Threading.Tasks.Task RemoveAsync_ShouldThrow_WhenRentalNotFound()
    {
        _repository.GetByIdAsync(Arg.Any<Guid>(), default).Returns((RentalEntity?)null);

        await Should.ThrowAsync<KeyNotFoundException>(() => _sut.RemoveAsync(Guid.NewGuid()));
    }
}
