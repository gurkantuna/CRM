using CRM.Core.Entity.Abstract;

namespace CRM.Entity.Concrete {

    public class User : UserBase {
        public override bool EmailConfirmed { get; set; } = false;
    }
}