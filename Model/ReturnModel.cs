
using System.Net;

public class ReturnModel
{
    public string Reason { get;set; }
    public bool? IsSuccess { get;set; }
    public object Response { get;set; }
    public HttpStatusCode httpStatusCode { get;set; }
}