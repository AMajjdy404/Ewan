using Ewan.Core.Models.Enums;

using Ewan.Core.Models.Enums;

namespace Ewan.Application.Helpers
{
    public static class PropertyBookingModeResolver
    {
        public static BookingMode ResolveFromPropertyType(PropertyType propertyType)
        {
            return propertyType switch
            {
                PropertyType.Chalet => BookingMode.ExclusiveStay,
                PropertyType.Apartment => BookingMode.ExclusiveStay,
                PropertyType.Hotel => BookingMode.RoomBased,
                PropertyType.Hall => BookingMode.TimeSlot,
                _ => throw new InvalidOperationException("Property type is not supported.")
            };
        }

    }
}
