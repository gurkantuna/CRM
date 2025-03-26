using System.ComponentModel.DataAnnotations;
using CRM.Core.Constants;
using CRM.Core.ViewModels.Abstract;

namespace CRM.Core.ViewModels.Concrete {
    public class UserLoginViewModel : ViewModelBase {

        [MaxLength(50)]
        [Required(AllowEmptyStrings = false, ErrorMessage = Strings.Required)]
        public string? UserName { get; set; }    

        [MaxLength(50)]
        [Required(AllowEmptyStrings = false, ErrorMessage = Strings.Required)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }

        public bool RememberMe { get; set; } = true;
    }
}