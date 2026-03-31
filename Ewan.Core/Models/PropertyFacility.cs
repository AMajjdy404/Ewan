namespace Ewan.Core.Models
{
    public class PropertyFacility
    {
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;

        public int FacilityId { get; set; }
        public Facility Facility { get; set; } = null!;
    }
}
