using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CRM.Core.Entity.Abstract {
    /*
      * Entities:
         ○ User:
         ■ Fields: Id, Username, Password, Role, CreatedAt, UpdatedAt
         ■ Used for authentication and role-based access. 
      */
    public class UserBase : IdentityUser<int>, IEntityBase, ITraceableEntity {
        public UserBase() {
            CreatedAt = DateTime.Now;
        }

        [BindNever]
        public DateTime CreatedAt { get; protected internal set; }

        public DateTime? UpdatedAt { get; protected internal set; }
    }
}
