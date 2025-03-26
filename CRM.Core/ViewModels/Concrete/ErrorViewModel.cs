using CRM.Core.ViewModels.Abstract;

namespace CRM.Core.ViewModels.Concrete {
    public class ErrorViewModel : ViewModelBase {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public int? StatusCode { get; set; }
        public string? Path { get; set; }
        public string? Message { get; set; }
        public string? InnerException { get; set; }
        public string? StackTrace { get; set; }
        public string? Header { get; set; }
        public string Info { get; set; } = "Unknown Error";
    }
}