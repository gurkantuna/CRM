using System.Diagnostics;
using System.Text;
using CRM.Business.Abstract;
using CRM.Core.ViewModels.Concrete;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Presentation.Controllers {

    public class ErrorController : Controller {

        public ErrorController(IDbLogger dbLogger) {
            _dbLogger = dbLogger;
        }

        private readonly IDbLogger _dbLogger;


        public async Task<IActionResult> Index(int? status) {
            var errorViewModel = new ErrorViewModel {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = status
            };

            var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var sb = new StringBuilder();

            if(statusCodeReExecuteFeature != null) {
                sb.Append(statusCodeReExecuteFeature.OriginalQueryString != null
                                    ? $"{statusCodeReExecuteFeature.OriginalPath}?{statusCodeReExecuteFeature.OriginalQueryString}"
                                    : $"{statusCodeReExecuteFeature.OriginalPath}");
            }
            else if(exceptionHandlerPathFeature != null) {
                errorViewModel.Message = exceptionHandlerPathFeature.Error.Message;
                errorViewModel.InnerException = exceptionHandlerPathFeature.Error?.InnerException?.Message;
                errorViewModel.StackTrace = exceptionHandlerPathFeature.Error?.StackTrace;
                sb.Append(exceptionHandlerPathFeature.Path);
                if(errorViewModel.Message != null) { sb.Append("!!! Exception = Message:").Append(errorViewModel.Message).Append(" - "); }
                if(errorViewModel.InnerException != null) { sb.Append("Inner Exception:").Append(errorViewModel.InnerException).Append(" - "); }
                if(errorViewModel.StackTrace != null) { sb.Append("StackTrace: ").Append(errorViewModel.StackTrace); }
            }

            if(status.HasValue) {

                switch(errorViewModel.StatusCode) {
                    case StatusCodes.Status404NotFound:
                        errorViewModel.Header = "Not Found";
                        errorViewModel.Info = $"{statusCodeReExecuteFeature?.OriginalPath}-{statusCodeReExecuteFeature?.OriginalQueryString} not found";
                        break;
                    case StatusCodes.Status400BadRequest:
                        errorViewModel.Header = "Bad Request";
                        errorViewModel.Info = "Bad request";
                        break;
                    case StatusCodes.Status500InternalServerError:
                        errorViewModel.Header = "Internal Server Error";
                        break;
                    case 522:
                        errorViewModel.Header = "Timeout";
                        errorViewModel.Info = "Timeout";
                        break;
                    default:
                        errorViewModel.Header = "Error";
                        errorViewModel.Info = "Unknown Error";
                        break;
                }
            }

            sb.Insert(0, $"StatusCode: {errorViewModel.StatusCode} - ");
            errorViewModel.Message = sb.ToString();

            if(errorViewModel.StatusCode.HasValue) {
                if(errorViewModel.StatusCode.Value == 404) {
                    ViewData["Title"] = $"Not found - {errorViewModel.StatusCode}";
                }
                else {
                    ViewData["Title"] = $"Error - {errorViewModel.StatusCode}";
                }
            }

            _dbLogger.LogToDbByMessage($"Hata Id:{errorViewModel.RequestId}-{errorViewModel.Message}", LogLevel.Trace);

            return View(errorViewModel);
        }

        [HttpPost("/error/js-catch")]
        public async Task<IActionResult> JsCatch(string error) {
            var errorVievModel = new ErrorViewModel { StatusCode = StatusCodes.Status500InternalServerError };
            errorVievModel.Message = $"StatusCode : {errorVievModel.StatusCode} - {error}";
            //await _errorBusiness.CatchErrorAsync(errorVievModel, true);
            return BadRequest(errorVievModel);
        }
    }
}