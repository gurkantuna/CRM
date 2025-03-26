namespace CRM.Core.Entity.Abstract {
    public interface ITraceableEntity {
        public DateTime CreatedAt { get; }
        public DateTime? UpdatedAt { get; }
    }
}