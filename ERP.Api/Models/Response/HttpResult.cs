namespace ERP.Api.Models.Response;

public class HttpResult
{
    public int Status { get; set; } = 200;
    public string Message { get; set; } = string.Empty;
    public object? Request { get; set; } = new();
    public object? Response { get; set; } = new(); 

    public HttpResult(int status=200, string message="", object? response=null, object? request=null)
    {
        Status = status;
        Message = message;
        Response = response ?? new();
        Request = request ?? new();
    }
}
