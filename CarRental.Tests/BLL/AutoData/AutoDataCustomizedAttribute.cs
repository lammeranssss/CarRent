using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using CarRental.BLL.Models;

namespace CarRental.Tests.BLL.AutoData;

public class AutoDataCustomizedAttribute : AutoDataAttribute
{
    public AutoDataCustomizedAttribute() : base(() =>
    {
        var fixture = new Fixture();

        fixture.Customize(new AutoNSubstituteCustomization
        {
            ConfigureMembers = true
        });

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        fixture.Customize<BookingModel>(c => c.Without(p => p.Customer).Without(p => p.Car).Without(p => p.Rental));
        fixture.Customize<CarModel>(c => c.Without(p => p.Location).Without(p => p.Bookings));
        fixture.Customize<CustomerModel>(c => c.Without(p => p.Bookings));
        fixture.Customize<LocationModel>(c => c.Without(p => p.Cars).Without(p => p.PickUpRentals).Without(p => p.DropOffRentals));
        fixture.Customize<RentalModel>(c => c.Without(p => p.Booking).Without(p => p.PickUpLocation).Without(p => p.DropOffLocation));
        return fixture;
    })
    {
    }
}