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

public class CarServiceTests
{
    [Theory, AutoDataCustomized]
    public async Task GetByIdAsync_ShouldReturnCarModel_WhenCarExists(
        CarEntity carEntity,
        CarModel carModel,
        [Frozen] ICarRepository repository,
        [Frozen] IMapper mapper,
        CarService carService) 
    {
        // Arrange
        carModel.Id = carEntity.Id;

        repository.GetByIdWithNoTrackingAsync(carEntity.Id, default).Returns(carEntity);
        mapper.Map<CarModel>(carEntity).Returns(carModel);

        // Act
        var result = await carService.GetByIdAsync(carEntity.Id);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBe(carModel);
    }

    [Theory, AutoDataCustomized]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCarNotFound(
        Guid carId,
        [Frozen] ICarRepository repository,
        [Frozen] IMapper mapper, 
        CarService carService)
    {
        // Arrange
        repository.GetByIdWithNoTrackingAsync(carId, default).Returns((CarEntity?)null);

        mapper.Map<CarModel>((CarEntity)null!).Returns((CarModel)null!);

        // Act
        var result = await carService.GetByIdAsync(carId);

        // Assert
        result.ShouldBeNull();
    }

    [Theory, AutoDataCustomized]
    public async Task AddAsync_ShouldMapAndReturnCarModel(
        CarModel carModel,
        CarEntity carEntity,
        [Frozen] ICarRepository repository,
        [Frozen] IMapper mapper,
        CarService carService)
    {
        // Arrange
        mapper.Map<CarEntity>(carModel).Returns(carEntity);
        repository.AddAsync(carEntity, default).Returns(carEntity);
        mapper.Map<CarModel>(carEntity).Returns(carModel);

        // Act
        var result = await carService.AddAsync(carModel);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(carModel.Id);
        await repository.Received(1).AddAsync(carEntity, default);
    }

    [Theory, AutoDataCustomized]
    public async Task RemoveAsync_ShouldCallRepositoryRemove_WhenCarExists(
        CarEntity carEntity,
        [Frozen] ICarRepository repository,
        CarService carService)
    {
        // Arrange
        repository.GetByIdAsync(carEntity.Id, default).Returns(carEntity);

        // Act
        await carService.RemoveAsync(carEntity.Id);

        // Assert
        await repository.Received(1).RemoveAsync(carEntity, default);
    }

    [Theory, AutoDataCustomized]
    public async Task RemoveAsync_ShouldThrowKeyNotFoundException_WhenCarNotFound(
        Guid carId,
        [Frozen] ICarRepository repository,
        CarService carService)
    {
        // Arrange
        repository.GetByIdAsync(carId, default).Returns((CarEntity?)null);

        // Act & Assert
        await Should.ThrowAsync<KeyNotFoundException>(() => carService.RemoveAsync(carId));
    }
}
