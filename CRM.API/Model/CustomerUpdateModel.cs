using System.ComponentModel.DataAnnotations;
using CRM.Core.Constants;

namespace CRM.API.Model {
    public class CustomerUpdateModel {

        [Required(ErrorMessage = Strings.Required)]
        public int Id { get; set; }

        [Required(ErrorMessage = Strings.Required), MaxLength(100, ErrorMessage = Strings.MaxLength)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = Strings.Required), MaxLength(100, ErrorMessage = Strings.MaxLength)]
        public string LastName { get; set; }

        [Required, EmailAddress(ErrorMessage = Strings.WrongFormat), MaxLength(250, ErrorMessage = Strings.MaxLength)]
        public string? Email { get; set; }

        [Required, MaxLength(100, ErrorMessage = Strings.MaxLength)]
        public string? Region { get; set; }

        public override string ToString() => $"{FirstName} {LastName}";
    }
}
