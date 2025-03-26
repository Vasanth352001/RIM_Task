
using System.Net;
using Microsoft.AspNetCore.Mvc;

public class ReturnModel
{
    public string Reason { get;set; }
    public bool? IsSuccess { get;set; }
    public object Response { get;set; }
    public HttpStatusCode httpStatusCode { get;set; }
    public FileDetail fileDetail { get;set; }
}

public class FileDetail
{
    public string fileName { get;set; }
    public string filePath { get;set; }
}