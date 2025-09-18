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

public class CarServiceTests
{
    private readonly IFixture _fixture;
    private readonly ICarRepository _repository;
    private readonly Mapper _mapper;
    private readonly CarService _sut;

    public CarServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _repository = Substitute.For<ICarRepository>();

        var loggerFactory = Substitute.For<ILoggerFactory>();
        var expression = new MapperConfigurationExpression();
        expression.AddProfile<BllMappingProfile>();
        var config = new MapperConfiguration(expression, loggerFactory);
        _mapper = new Mapper(config);

        _sut = new CarService(_repository, _mapper);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByIdAsync_ShouldReturnCarModel_WhenCarExists()
    {
        var carEntity = _fixture.Build<CarEntity>()
            .Without(c => c.Location)
            .Without(c => c.Bookings)
            .Create();

        _repository.GetByIdWithNoTrackingAsync(carEntity.Id, default).Returns(carEntity);

        var result = await _sut.GetByIdAsync(carEntity.Id);

        result.ShouldNotBeNull();
        result!.Id.ShouldBe(carEntity.Id);
        result.Brand.ShouldBe(carEntity.Brand);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByIdAsync_ShouldReturnNull_WhenCarNotFound()
    {
        _repository.GetByIdWithNoTrackingAsync(Arg.Any<Guid>(), default).Returns((CarEntity?)null);

        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        result.ShouldBeNull();
    }

    [Fact]
    public async System.Threading.Tasks.Task AddAsync_ShouldMapAndReturnCarModel()
    {
        var carModel = _fixture.Build<CarModel>()
            .Without(c => c.Location)
            .Create();

        var carEntity = _mapper.Map<CarEntity>(carModel);

        _repository.AddAsync(Arg.Any<CarEntity>(), default).Returns(carEntity);

        var result = await _sut.AddAsync(carModel);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(carModel.Id);
        await _repository.Received(1).AddAsync(Arg.Any<CarEntity>(), default);
    }

    [Fact]
    public async System.Threading.Tasks.Task RemoveAsync_ShouldCallRepositoryRemove_WhenCarExists()
    {
        var carEntity = _fixture.Build<CarEntity>()
            .Without(c => c.Location)
            .Without(c => c.Bookings)
            .Create();

        _repository.GetByIdAsync(carEntity.Id, default).Returns(carEntity);

        await _sut.RemoveAsync(carEntity.Id);

        await _repository.Received(1).RemoveAsync(carEntity, default);
    }

    [Fact]
    public async System.Threading.Tasks.Task RemoveAsync_ShouldThrow_WhenCarNotFound()
    {
        _repository.GetByIdAsync(Arg.Any<Guid>(), default).Returns((CarEntity?)null);

        await Should.ThrowAsync<KeyNotFoundException>(() => _sut.RemoveAsync(Guid.NewGuid()));
    }
}
