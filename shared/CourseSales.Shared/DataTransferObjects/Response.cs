namespace CourseSales.Shared.DataTransferObjects
{
    public class Response<T>
    {
        public T Data { get; set; }

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }

        [JsonIgnore]
        public bool IsSuccessful { get; set; }

        public List<string> Errors { get; set; }

        public static Response<T> Success(T data, HttpStatusCode statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Success(HttpStatusCode statusCode)
        {
            return new Response<T> { Data = default, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Fail(List<string> errors, HttpStatusCode statusCode)
        {
            return new Response<T>
            {
                Errors = errors,
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }

        public static Response<T> Fail(string error, HttpStatusCode statusCode)
        {
            return new Response<T> { Errors = new List<string>() { error }, StatusCode = statusCode, IsSuccessful = false };
        }
    }
}
