using System.ComponentModel.DataAnnotations;
using CRM.Core.Constants;
using CRM.Core.ViewModels.Abstract;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CRM.Core.ViewModels.Concrete {
    public class UserRegisterViewModel : ViewModelBase {

        [MaxLength(50)]
        [Required(AllowEmptyStrings = false, ErrorMessage = Strings.Required)]
        public string UserName { get; set; }

        [MaxLength(256)]
        [EmailAddress(ErrorMessage = Strings.WrongFormat)]
        [Required(AllowEmptyStrings = false, ErrorMessage = Strings.Required)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(20)]
        [Required(AllowEmptyStrings = false, ErrorMessage = Strings.Required)]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [MaxLength(20)]
        [Compare(nameof(Password), ErrorMessage = Strings.WrongComparePasswords)]
        [Required(AllowEmptyStrings = false, ErrorMessage = Strings.Required)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public bool RememberMe { get; set; } = true;

        [MaxLength(20), BindNever]
        public string Role { get; set; } = Strings.Moderator;
    }
}
