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

public class RentalServiceTests
{
    [Theory, AutoDataCustomized]
    public async Task GetByIdAsync_ShouldReturnRentalModel_WhenRentalExists(
        RentalEntity rentalEntity,
        RentalModel rentalModel,
        [Frozen] IRentalRepository repository,
        [Frozen] IMapper mapper,
        RentalService rentalService)
    {
        // Arrange
        rentalModel.Id = rentalEntity.Id;

        repository.GetByIdWithNoTrackingAsync(rentalEntity.Id, default).Returns(rentalEntity);
        mapper.Map<RentalModel>(rentalEntity).Returns(rentalModel);

        // Act
        var result = await rentalService.GetByIdAsync(rentalEntity.Id);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBe(rentalModel);
    }

    [Theory, AutoDataCustomized]
    public async Task GetByIdAsync_ShouldReturnNull_WhenRentalNotFound(
        Guid rentalId,
        [Frozen] IRentalRepository repository,
        [Frozen] IMapper mapper,
        RentalService rentalService)
    {
        // Arrange
        repository.GetByIdWithNoTrackingAsync(rentalId, default).Returns((RentalEntity?)null);

        mapper.Map<RentalModel>((RentalEntity)null!).Returns((RentalModel)null!);

        // Act
        var result = await rentalService.GetByIdAsync(rentalId);

        // Assert
        result.ShouldBeNull();
    }

    [Theory, AutoDataCustomized]
    public async Task AddAsync_ShouldMapAndReturnRentalModel(
        RentalModel rentalModel,
        RentalEntity rentalEntity,
        [Frozen] IRentalRepository repository,
        [Frozen] IMapper mapper,
        RentalService rentalService)
    {
        // Arrange
        mapper.Map<RentalEntity>(rentalModel).Returns(rentalEntity);
        repository.AddAsync(rentalEntity, default).Returns(rentalEntity);
        mapper.Map<RentalModel>(rentalEntity).Returns(rentalModel);

        // Act
        var result = await rentalService.AddAsync(rentalModel);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(rentalModel.Id);
        await repository.Received(1).AddAsync(rentalEntity, default);
    }

    [Theory, AutoDataCustomized]
    public async Task RemoveAsync_ShouldCallRepositoryRemove_WhenRentalExists(
        RentalEntity rentalEntity,
        [Frozen] IRentalRepository repository,
        RentalService rentalService)
    {
        // Arrange
        repository.GetByIdAsync(rentalEntity.Id, default).Returns(rentalEntity);

        // Act
        await rentalService.RemoveAsync(rentalEntity.Id);

        // Assert
        await repository.Received(1).RemoveAsync(rentalEntity, default);
    }

    [Theory, AutoDataCustomized]
    public async Task RemoveAsync_ShouldThrowKeyNotFoundException_WhenRentalNotFound(
        Guid rentalId,
        [Frozen] IRentalRepository repository,
        RentalService rentalService)
    {
        // Arrange
        repository.GetByIdAsync(rentalId, default).Returns((RentalEntity?)null);

        // Act & Assert
        await Should.ThrowAsync<KeyNotFoundException>(() => rentalService.RemoveAsync(rentalId));
    }
}
