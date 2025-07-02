namespace DAL.ViewModels;

public enum AjaxError
{
    Success = 1,
    Error = 2,
    ValidationError = 3,
    NotFound = 4,
    UpSertError = 5,
    UnAuthorized = 6,
}
public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
