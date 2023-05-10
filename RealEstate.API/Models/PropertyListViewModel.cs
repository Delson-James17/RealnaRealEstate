using RealEstate.API.Models;

namespace RealEstate.API.Models;


public class PropertyListViewModel
{
      public int Id { get; set; }
    public int? EstateId { get; set; }   
    public List<EstateProperty>? Properties { get; set; }
    public int? CategoryId { get; set; }
    public List<PropertyCategory>? Categories { get; set; }
  
}
