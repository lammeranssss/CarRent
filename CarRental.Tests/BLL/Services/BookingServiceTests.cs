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

public class BookingServiceTests
{
    private readonly IFixture _fixture;
    private readonly IBookingRepository _repository;
    private readonly Mapper _mapper;
    private readonly BookingService _sut;

    public BookingServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _repository = Substitute.For<IBookingRepository>();

        var loggerFactory = Substitute.For<ILoggerFactory>();
        var expression = new MapperConfigurationExpression();
        expression.AddProfile<BllMappingProfile>();
        var config = new MapperConfiguration(expression, loggerFactory);
        _mapper = new Mapper(config);

        _sut = new BookingService(_repository, _mapper);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByIdAsync_ShouldReturnBookingModel_WhenBookingExists()
    {
        var bookingEntity = _fixture.Build<BookingEntity>()
            .Without(b => b.Customer)
            .Without(b => b.Car)
            .Without(b => b.Rental)
            .Create();

        _repository.GetByIdWithNoTrackingAsync(bookingEntity.Id, default).Returns(bookingEntity);

        var result = await _sut.GetByIdAsync(bookingEntity.Id);

        result.ShouldNotBeNull();
        result!.Id.ShouldBe(bookingEntity.Id);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByIdAsync_ShouldReturnNull_WhenBookingNotFound()
    {
        _repository.GetByIdWithNoTrackingAsync(Arg.Any<Guid>(), default).Returns((BookingEntity?)null);

        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        result.ShouldBeNull();
    }

    [Fact]
    public async System.Threading.Tasks.Task AddAsync_ShouldMapAndReturnBookingModel()
    {
        var bookingModel = _fixture.Build<BookingModel>()
            .Without(b => b.Customer)
            .Without(b => b.Car)
            .Without(b => b.Rental)
            .Create();

        var bookingEntity = _mapper.Map<BookingEntity>(bookingModel);

        _repository.AddAsync(Arg.Any<BookingEntity>(), default).Returns(bookingEntity);

        var result = await _sut.AddAsync(bookingModel);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(bookingModel.Id);
        await _repository.Received(1).AddAsync(Arg.Any<BookingEntity>(), default);
    }

    [Fact]
    public async System.Threading.Tasks.Task RemoveAsync_ShouldCallRepositoryRemove_WhenBookingExists()
    {
        var bookingEntity = _fixture.Build<BookingEntity>()
            .Without(b => b.Customer)
            .Without(b => b.Car)
            .Without(b => b.Rental)
            .Create();

        _repository.GetByIdAsync(bookingEntity.Id, default).Returns(bookingEntity);

        await _sut.RemoveAsync(bookingEntity.Id);

        await _repository.Received(1).RemoveAsync(bookingEntity, default);
    }

    [Fact]
    public async System.Threading.Tasks.Task RemoveAsync_ShouldThrow_WhenBookingNotFound()
    {
        _repository.GetByIdAsync(Arg.Any<Guid>(), default).Returns((BookingEntity?)null);

        await Should.ThrowAsync<KeyNotFoundException>(() => _sut.RemoveAsync(Guid.NewGuid()));
    }
}
