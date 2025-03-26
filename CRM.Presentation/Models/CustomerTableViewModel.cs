using CRM.Entity.Concrete;

namespace CRM.Presentation.Models {
    public class CustomerTableViewModel : CustomerFormViewModel {
        public IEnumerable<Customer> Customers { get; set; }
    }
}