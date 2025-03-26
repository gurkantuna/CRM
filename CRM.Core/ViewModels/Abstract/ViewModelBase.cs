namespace CRM.Core.ViewModels.Abstract {

    public abstract class ViewModelBase : IViewModel {
        protected ViewModelBase() {
            Errors = [];
            GuidId = Guid.NewGuid();
        }

        public int Id { get; set; }
        public Guid GuidId { get; set; }
        public virtual bool IsCreated { get; set; }
        public virtual bool IsAddedToRole { get; set; }
        public virtual List<string> Errors { get; set; }
    }
}
