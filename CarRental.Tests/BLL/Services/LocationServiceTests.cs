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

public class LocationServiceTests
{
    private readonly IFixture _fixture;
    private readonly ILocationRepository _repository;
    private readonly Mapper _mapper;
    private readonly LocationService _sut;

    public LocationServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _repository = Substitute.For<ILocationRepository>();

        var loggerFactory = Substitute.For<ILoggerFactory>();
        var expression = new MapperConfigurationExpression();
        expression.AddProfile<BllMappingProfile>();
        var config = new MapperConfiguration(expression, loggerFactory);
        _mapper = new Mapper(config);

        _sut = new LocationService(_repository, _mapper);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByIdAsync_ShouldReturnLocationModel_WhenLocationExists()
    {
        var locationEntity = _fixture.Build<LocationEntity>()
            .Without(l => l.Cars)
            .Without(l => l.PickUpRentals)
            .Without(l => l.DropOffRentals)
            .Create();

        _repository.GetByIdWithNoTrackingAsync(locationEntity.Id, default).Returns(locationEntity);

        var result = await _sut.GetByIdAsync(locationEntity.Id);

        result.ShouldNotBeNull();
        result!.Id.ShouldBe(locationEntity.Id);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByIdAsync_ShouldReturnNull_WhenLocationNotFound()
    {
        _repository.GetByIdWithNoTrackingAsync(Arg.Any<Guid>(), default).Returns((LocationEntity?)null);

        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        result.ShouldBeNull();
    }

    [Fact]
    public async System.Threading.Tasks.Task AddAsync_ShouldMapAndReturnLocationModel()
    {
        var model = _fixture.Build<LocationModel>()
            .Without(l => l.Cars)
            .Without(l => l.PickUpRentals)
            .Without(l => l.DropOffRentals)
            .Create();

        var entity = _mapper.Map<LocationEntity>(model);

        _repository.AddAsync(Arg.Any<LocationEntity>(), default).Returns(entity);

        var result = await _sut.AddAsync(model);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(model.Id);
        await _repository.Received(1).AddAsync(Arg.Any<LocationEntity>(), default);
    }

    [Fact]
    public async System.Threading.Tasks.Task RemoveAsync_ShouldCallRepositoryRemove_WhenLocationExists()
    {
        var locationEntity = _fixture.Build<LocationEntity>()
            .Without(l => l.Cars)
            .Without(l => l.PickUpRentals)
            .Without(l => l.DropOffRentals)
            .Create();

        _repository.GetByIdAsync(locationEntity.Id, default).Returns(locationEntity);

        await _sut.RemoveAsync(locationEntity.Id);

        await _repository.Received(1).RemoveAsync(locationEntity, default);
    }

    [Fact]
    public async System.Threading.Tasks.Task RemoveAsync_ShouldThrow_WhenLocationNotFound()
    {
        _repository.GetByIdAsync(Arg.Any<Guid>(), default).Returns((LocationEntity?)null);

        await Should.ThrowAsync<KeyNotFoundException>(() => _sut.RemoveAsync(Guid.NewGuid()));
    }
}
