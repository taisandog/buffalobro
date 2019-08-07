namespace OBS
{
    using System;
    using System.Net;
    using System.Runtime.CompilerServices;

    public abstract class ServiceException : Exception
    {
        public ServiceException()
        {
        }

        public ServiceException(Exception innerException) : base(innerException.Message, innerException)
        {
        }

        public ServiceException(string message) : base(message)
        {
        }

        public ServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ServiceException(string message, OBS.ErrorType errorType, string errorCode) : base(message)
        {
            this.ErrorCode = errorCode;
            this.ErrorType = errorType;
        }

        public ServiceException(string message, Exception innerException, HttpStatusCode statusCode) : base(message, innerException)
        {
            this.StatusCode = statusCode;
        }

        public ServiceException(string message, OBS.ErrorType errorType, string errorCode, string requestId) : base(message)
        {
            this.ErrorCode = errorCode;
            this.ErrorType = errorType;
            this.RequestId = requestId;
        }

        public ServiceException(string message, OBS.ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode) : base(message)
        {
            this.ErrorCode = errorCode;
            this.ErrorType = errorType;
            this.RequestId = requestId;
            this.StatusCode = statusCode;
        }

        public ServiceException(string message, Exception innerException, OBS.ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode) : base(message, innerException)
        {
            this.ErrorCode = errorCode;
            this.ErrorType = errorType;
            this.RequestId = requestId;
            this.StatusCode = statusCode;
        }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public OBS.ErrorType ErrorType { get; set; }

        public string RequestId { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}

