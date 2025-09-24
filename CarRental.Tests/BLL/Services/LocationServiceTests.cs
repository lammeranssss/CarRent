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

public class LocationServiceTests
{
    [Theory, AutoDataCustomized]
    public async Task GetByIdAsync_ShouldReturnLocationModel_WhenLocationExists(
        LocationEntity locationEntity,
        LocationModel locationModel,
        [Frozen] ILocationRepository repository,
        [Frozen] IMapper mapper,
        LocationService locationService)
    {
        // Arrange
        locationModel.Id = locationEntity.Id;

        repository.GetByIdWithNoTrackingAsync(locationEntity.Id, default).Returns(locationEntity);
        mapper.Map<LocationModel>(locationEntity).Returns(locationModel);

        // Act
        var result = await locationService.GetByIdAsync(locationEntity.Id);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBe(locationModel);
    }

    [Theory, AutoDataCustomized]
    public async Task GetByIdAsync_ShouldReturnNull_WhenLocationNotFound(
        Guid locationId,
        [Frozen] ILocationRepository repository,
        [Frozen] IMapper mapper,
        LocationService locationService)
    {
        // Arrange
        repository.GetByIdWithNoTrackingAsync(locationId, default).Returns((LocationEntity?)null);

        mapper.Map<LocationModel>((LocationEntity)null!).Returns((LocationModel)null!);

        // Act
        var result = await locationService.GetByIdAsync(locationId);

        // Assert
        result.ShouldBeNull();
    }

    [Theory, AutoDataCustomized]
    public async Task AddAsync_ShouldMapAndReturnLocationModel(
        LocationModel locationModel,
        LocationEntity locationEntity,
        [Frozen] ILocationRepository repository,
        [Frozen] IMapper mapper,
        LocationService locationService)
    {
        // Arrange
        mapper.Map<LocationEntity>(locationModel).Returns(locationEntity);
        repository.AddAsync(locationEntity, default).Returns(locationEntity);
        mapper.Map<LocationModel>(locationEntity).Returns(locationModel);

        // Act
        var result = await locationService.AddAsync(locationModel);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(locationModel.Id);
        await repository.Received(1).AddAsync(locationEntity, default);
    }

    [Theory, AutoDataCustomized]
    public async Task RemoveAsync_ShouldCallRepositoryRemove_WhenLocationExists(
        LocationEntity locationEntity,
        [Frozen] ILocationRepository repository,
        LocationService locationService)
    {
        // Arrange
        repository.GetByIdAsync(locationEntity.Id, default).Returns(locationEntity);

        // Act
        await locationService.RemoveAsync(locationEntity.Id);

        // Assert
        await repository.Received(1).RemoveAsync(locationEntity, default);
    }

    [Theory, AutoDataCustomized]
    public async Task RemoveAsync_ShouldThrowKeyNotFoundException_WhenLocationNotFound(
        Guid locationId,
        [Frozen] ILocationRepository repository,
        LocationService locationService)
    {
        // Arrange
        repository.GetByIdAsync(locationId, default).Returns((LocationEntity?)null);

        // Act & Assert
        await Should.ThrowAsync<KeyNotFoundException>(() => locationService.RemoveAsync(locationId));
    }
}
