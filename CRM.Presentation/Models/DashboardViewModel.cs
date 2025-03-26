using CRM.Entity.Concrete;

namespace CRM.Presentation.Models {
    public class DashboardViewModel {
        public IEnumerable<Customer> Customers { get; set; }
    }
}
