using System.ComponentModel.DataAnnotations;
using CRM.Core.Constants;
using CRM.Core.Entity.Abstract;

namespace CRM.Core.Entity.Concrete {
    public class RegionBase : EntityBase {

        [Required(ErrorMessage = Strings.Required), MaxLength(20, ErrorMessage = Strings.MaxLength)]
        public string Name { get; set; }
    }
}
