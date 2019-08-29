namespace OBS
{
    using System;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class ObsException : ServiceException
    {
        public ObsException(Exception innerException) : base(innerException.Message, innerException)
        {
        }

        public ObsException(string message) : base(message)
        {
        }

        public ObsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ObsException(string message, ErrorType errorType, string errorCode) : base(message, errorType, errorCode)
        {
        }

        public ObsException(string message, ErrorType errorType, string errorCode, string requestId) : base(message, errorType, errorCode, requestId)
        {
        }

        public ObsException(string message, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode) : base(message, errorType, errorCode, requestId, statusCode)
        {
        }

        public ObsException(string message, ErrorType errorType, string errorCode, string errorMessage, string requestId, HttpStatusCode statusCode) : base(message, errorType, errorCode, requestId, statusCode)
        {
            base.ErrorMessage = errorMessage;
        }

        public ObsException(string message, Exception innerException, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode) : base(message, innerException, errorType, errorCode, requestId, statusCode)
        {
        }

        public ObsException(string message, Exception innerException, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode, string obsId2) : base(message, innerException, errorType, errorCode, requestId, statusCode)
        {
            this.ObsId2 = obsId2;
        }

        public ObsException(string message, Exception innerException, ErrorType errorType, string errorCode, string errorMessage, string requestId, HttpStatusCode statusCode) : base(message, innerException, errorType, errorCode, requestId, statusCode)
        {
            base.ErrorMessage = errorMessage;
        }

        public override string ToString()
        {
            StringBuilder builder1 = new StringBuilder();
            builder1.Append(this.Message).Append(", StatusCode:").Append(Convert.ToInt32(base.StatusCode)).Append(", ErrorCode:").Append(base.ErrorCode).Append(", ErrorMessage:").Append(base.ErrorMessage).Append(", RequestId:").Append(base.RequestId).Append(", HostId:").Append(this.HostId);
            return builder1.ToString();
        }

        public string HostId { get; set; }

        [Obsolete]
        public string ObsId2 { get; set; }
    }
}

