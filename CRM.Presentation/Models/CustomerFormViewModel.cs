using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CRM.Core.Constants;
using CRM.Core.ViewModels.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace CRM.Presentation.Models {
    public class CustomerFormViewModel : ViewModelBase {

        public int Id { get; set; }

        [Required(ErrorMessage = Strings.Required), MaxLength(100, ErrorMessage = Strings.MaxLength), JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = Strings.Required), MaxLength(100, ErrorMessage = Strings.MaxLength)]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = Strings.WrongFormat), MaxLength(250, ErrorMessage = Strings.MaxLength)]
        public string? Email { get; set; }

        [MaxLength(100, ErrorMessage = Strings.MaxLength)]
        public string? Region { get; set; }

        public DateTime RegistrationDate { get; set; }
        public SelectList Regions { get; set; }

        public override string ToString() => $"{FirstName} {LastName}";
    }
}
