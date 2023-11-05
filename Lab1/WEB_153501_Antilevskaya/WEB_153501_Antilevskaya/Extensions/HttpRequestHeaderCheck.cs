using Azure.Core;

namespace WEB_153501_Antilevskaya.Extensions
{
    public static class HttpRequestHeaderCheck
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException(nameof(request));
            }
            if (request.Headers != null)
            {
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            }
            return false;
        }
    }
}
