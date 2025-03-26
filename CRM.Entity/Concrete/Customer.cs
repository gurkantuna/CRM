using System.ComponentModel.DataAnnotations;
using CRM.Core.Constants;
using CRM.Core.Entity.Abstract;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace CRM.Entity.Concrete {
    /*
     ○ Customer:
        ■ Fields: Id, FirstName, LastName, Email, Region,
        RegistrationDate
        ■ This table stores customer data that is managed by the system. 
     */
    [Index(nameof(Email), IsUnique = true)]
    public class Customer : EntityBase, IPersonEntity {
        public Customer() {
            RegistrationDate = DateTime.Now;
        }

        [Required(ErrorMessage = Strings.Required), MaxLength(100, ErrorMessage = Strings.MaxLength)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = Strings.Required), MaxLength(100, ErrorMessage = Strings.MaxLength)]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = Strings.WrongFormat), MaxLength(250, ErrorMessage = Strings.MaxLength)]
        public string? Email { get; set; }

        [MaxLength(100, ErrorMessage = Strings.MaxLength)]
        public string? Region { get; set; }

        [BindNever]
        public DateTime RegistrationDate { get; set; }

        public override string ToString() => $"{FirstName} {LastName}";
    }
}
