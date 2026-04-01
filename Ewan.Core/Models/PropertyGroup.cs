namespace Ewan.Core.Models
{
    public class PropertyGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<Property> Properties { get; set; } = new HashSet<Property>();
    }
}
