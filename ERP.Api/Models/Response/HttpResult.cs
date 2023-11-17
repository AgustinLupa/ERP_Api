namespace ERP.Api.Models.Response;

public class HttpResult
{
    public int StatusCode { get; set; } = 200;
    public string Message { get; set; } = string.Empty;
    public object Request { get; set; } = new();
    public object Response { get; set; } = new();
}
