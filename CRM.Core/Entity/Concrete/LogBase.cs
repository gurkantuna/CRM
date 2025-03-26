using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.Core.Entity.Abstract;
using Microsoft.Extensions.Logging;

namespace CRM.Core.Entity.Concrete {
    [Table("Logs")]
    public class LogBase : EntityBase {

        public LogBase() {
            if(CreatedDate == default) {
                CreatedDate = DateTime.Now;
            }
        }

        [Required]
        public DateTime CreatedDate { get; private set; }

        [Required]
        public string Message { get; set; }

        public LogLevel Type { get; set; }
    }
}