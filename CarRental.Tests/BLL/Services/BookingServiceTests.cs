using AutoFixture.Xunit2;
using AutoMapper;
using CarRental.BLL.Models;
using CarRental.BLL.Services;
using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Entities;
using NSubstitute;
using Shouldly;
using CarRental.Tests.BLL.AutoData;

namespace CarRental.Tests.BLL.Services; 

public class BookingServiceTests
{
    [Theory, AutoDataCustomized]
    public async Task GetByIdAsync_ShouldReturnBookingModel_WhenBookingExists(
        BookingEntity bookingEntity,
        BookingModel bookingModel,
        [Frozen] IBookingRepository repository,
        [Frozen] IMapper mapper,
        BookingService bookingService) 
    {
        // Arrange
        bookingModel.Id = bookingEntity.Id;

        repository.GetByIdWithNoTrackingAsync(bookingEntity.Id, default).Returns(bookingEntity);
        mapper.Map<BookingModel>(bookingEntity).Returns(bookingModel);

        // Act
        var result = await bookingService.GetByIdAsync(bookingEntity.Id);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBe(bookingModel);
    }

    [Theory, AutoDataCustomized]
    public async Task GetByIdAsync_ShouldReturnNull_WhenBookingNotFound(
        Guid bookingId,
        [Frozen] IBookingRepository repository,
        [Frozen] IMapper mapper,
        BookingService bookingService)
    {
        // Arrange
        repository.GetByIdWithNoTrackingAsync(bookingId, default).Returns((BookingEntity?)null);

        mapper.Map<BookingModel>((BookingEntity)null!).Returns((BookingModel)null!);

        // Act
        var result = await bookingService.GetByIdAsync(bookingId);

        // Assert
        result.ShouldBeNull();
    }

    [Theory, AutoDataCustomized]
    public async Task AddAsync_ShouldMapAndReturnBookingModel_WhenCalledWithModel(
        BookingModel bookingModel,
        BookingEntity bookingEntity,
        [Frozen] IBookingRepository repository,
        [Frozen] IMapper mapper,
        BookingService bookingService)
    {
        // Arrange
        mapper.Map<BookingEntity>(bookingModel).Returns(bookingEntity);
        repository.AddAsync(bookingEntity, default).Returns(bookingEntity);
        mapper.Map<BookingModel>(bookingEntity).Returns(bookingModel);

        // Act
        var result = await bookingService.AddAsync(bookingModel);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(bookingModel.Id);
        await repository.Received(1).AddAsync(bookingEntity, default);
    }

    [Theory, AutoDataCustomized]
    public async Task RemoveAsync_ShouldCallRepositoryRemove_WhenBookingExists(
        BookingEntity bookingEntity,
        [Frozen] IBookingRepository repository,
        BookingService bookingService)
    {
        // Arrange
        repository.GetByIdAsync(bookingEntity.Id, default).Returns(bookingEntity);

        // Act
        await bookingService.RemoveAsync(bookingEntity.Id);

        // Assert
        await repository.Received(1).RemoveAsync(bookingEntity, default);
    }

    [Theory, AutoDataCustomized]
    public async Task RemoveAsync_ShouldThrowKeyNotFoundException_WhenBookingNotFound(
        Guid bookingId,
        [Frozen] IBookingRepository repository,
        BookingService bookingService)
    {
        // Arrange
        repository.GetByIdAsync(bookingId, default).Returns((BookingEntity?)null);

        // Act & Assert
        await Should.ThrowAsync<KeyNotFoundException>(() => bookingService.RemoveAsync(bookingId));
    }
}
