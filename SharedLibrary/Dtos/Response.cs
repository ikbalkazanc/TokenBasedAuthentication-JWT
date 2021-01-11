using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SharedLibrary.Dtos
{
    public class Response<T> where T : class
    {
        public T Data { get; set; }
        public int StatusCode { get; private set; }
        [JsonIgnore]
        public bool isSuccessful { get; set; }
        public ErrorDto Error { get; set; }
        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode };
        }
        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Data = default, StatusCode = statusCode };
        }
        public static Response<T> Fail(ErrorDto errorDto, int statusCode) 
        {
            return new Response<T>
            {
                Error = errorDto,
                StatusCode = statusCode
            };
        }
        public static Response<T> Fail(string errorMessage, int statusCode , bool isShow)
        {
            var errorDto = new ErrorDto(errorMessage ,isShow);
            return new Response<T>
            {
                Error = errorDto,
                StatusCode = statusCode
            };
        }
    }
}
