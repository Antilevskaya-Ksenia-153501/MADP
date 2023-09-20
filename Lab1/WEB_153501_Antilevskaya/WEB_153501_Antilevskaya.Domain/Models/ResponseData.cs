namespace WEB_153501_Antilevskaya.Domain.Models;

/* Represent the data that is returned as a response from an operation, method, or API call
   Encapsulates the data that needs to be returned as part of the response, 
   along with additional information such as status codes, error messages, or metadata.*/
public class ResponseData<T>
{
    public T Data { get; set; }
    public bool Success { get; set; } = true;
    public string? ErrorMessage { get; set; }
}
