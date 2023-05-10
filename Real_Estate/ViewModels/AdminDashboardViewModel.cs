using Real_Estate.Models;

namespace Real_Estate.ViewModels
{
    public class AdminDashboardViewModel
    {
       public RowCountViewModel RowCount { get; set; }
       public List<EstateProperty> EstateProperties { get; set; }
    }
}
