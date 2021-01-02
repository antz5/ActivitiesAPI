using System;
using System.Net;
namespace Application.Exceptions {
    public class RestException : Exception {
        public HttpStatusCode StatusCode { get; }
        public object Errors { get; }
        
        public RestException (HttpStatusCode statusCode, object errors = null) {
            Errors = errors;
            StatusCode = statusCode;
        }
    }
}