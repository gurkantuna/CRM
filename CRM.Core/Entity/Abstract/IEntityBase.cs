using System.ComponentModel.DataAnnotations;

namespace CRM.Core.Entity.Abstract {
    public interface IEntityBase {
        [Key]
        public int Id { get; }
    }
}