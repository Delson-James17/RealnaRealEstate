namespace RealEstate.API.Models
{
    public class PropertyCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public  List<EstateProperty>? EstateProperties { get; set; }
        public List<PropertyListViewModel>? PropertyListViews { get; set; }
    }
}
