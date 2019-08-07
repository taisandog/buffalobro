namespace OBS.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    internal static class Constants
    {
        private static volatile IList<string> _AllowedRequestHttpHeaders;
        private static volatile IList<string> _AllowedResourceParameters;
        private static volatile IList<string> _AllowedResponseHttpHeaders;
        private static readonly object _lock = new object();
        public static volatile IDictionary<string, string> _MimeTypes;
        public const string AllowedInUrl = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
        public static readonly System.Globalization.CultureInfo CultureInfo = System.Globalization.CultureInfo.GetCultureInfo("en-US");
        public const int DefaultBufferSize = 0x2000;
        public const int DefaultConnectionLimit = 0x3e8;
        public const int DefaultConnectTimeout = 0xea60;
        public const string DefaultEncoding = "utf-8";
        public const int DefaultMaxErrorRetry = 3;
        public const int DefaultMaxIdleTime = 0x7530;
        public const long DefaultProgressUpdateInterval = 0x19000L;
        public const int DefaultReadWriteTimeout = 0xea60;
        public const string InvalidBucketName = "InvalidBucketName";
        public const string InvalidBucketNameMessage = "bucket name is not valid";
        public const string InvalidEndpoint = "InvalidEndpoint";
        public const string InvalidEndpointMessage = "endpoint is not valid";
        public const string InvalidObjectKey = "InvalidObjectKey";
        public const string InvalidObjectKeyMessage = "object key is null";
        public const string InvalidPartNumber = "InvalidPartNumber";
        public const string InvalidPartNumberMessage = "part number is not valid";
        public const string InvalidSourceBucketNameMessage = "source object key is null";
        public const string InvalidSourceObjectKeyMessage = "source bucket name is null";
        public const string InvalidUploadId = "InvalidUploadId";
        public const string InvalidUploadIdMessage = "upload id is not valid";
        public const string ISO8601DateFormat = @"yyyy-MM-dd\THH:mm:ss.fff\Z";
        public const string ISO8601DateFormatMidNight = @"yyyy-MM-dd\T00:00:00\Z";
        public const string ISO8601DateFormatNoMS = @"yyyy-MM-dd\THH:mm:ss\Z";
        public const string LongDateFormat = "yyyyMMddTHHmmssZ";
        public const string NullRequest = "NullRequest";
        public const string NullRequestMessage = "request is null";
        public const string ObsHeaderMetaPrefix = "x-obs-meta-";
        public const string ObsHeaderPrefix = "x-obs-";
        public const string ObsSdkVersion = "3.0.0";
        public const string RequestTimeout = "RequestTimeout";
        public const string RFC822DateFormat = @"ddd, dd MMM yyyy HH:mm:ss \G\M\T";
        public const string SdkUserAgent = "obs-sdk-.net/3.0.0";
        public const string ShortDateFormat = "yyyyMMdd";
        public const string UrlEncodedContent = "application/x-www-form-urlencoded; charset=utf-8";
        public const string V2HeaderMetaPrefix = "x-amz-meta-";
        public const string V2HeaderPrefix = "x-amz-";

        public static IList<string> AllowedRequestHttpHeaders
        {
            get
            {
                if (_AllowedRequestHttpHeaders == null)
                {
                    object obj2 = _lock;
                    lock (obj2)
                    {
                        if (_AllowedRequestHttpHeaders == null)
                        {
                            _AllowedRequestHttpHeaders = new List<string>();
                            _AllowedRequestHttpHeaders.Add("content-type");
                            _AllowedRequestHttpHeaders.Add("content-md5");
                            _AllowedRequestHttpHeaders.Add("content-length");
                            _AllowedRequestHttpHeaders.Add("content-language");
                            _AllowedRequestHttpHeaders.Add("expires");
                            _AllowedRequestHttpHeaders.Add("origin");
                            _AllowedRequestHttpHeaders.Add("cache-control");
                            _AllowedRequestHttpHeaders.Add("content-disposition");
                            _AllowedRequestHttpHeaders.Add("content-encoding");
                            _AllowedRequestHttpHeaders.Add("access-control-request-method");
                            _AllowedRequestHttpHeaders.Add("access-control-request-headers");
                            _AllowedRequestHttpHeaders.Add("success-action-redirect");
                            _AllowedRequestHttpHeaders.Add("x-default-storage-class");
                            _AllowedRequestHttpHeaders.Add("location");
                            _AllowedRequestHttpHeaders.Add("date");
                            _AllowedRequestHttpHeaders.Add("etag");
                            _AllowedRequestHttpHeaders.Add("range");
                            _AllowedRequestHttpHeaders.Add("host");
                            _AllowedRequestHttpHeaders.Add("if-modified-since");
                            _AllowedRequestHttpHeaders.Add("if-unmodified-since");
                            _AllowedRequestHttpHeaders.Add("if-match");
                            _AllowedRequestHttpHeaders.Add("if-none-match");
                            _AllowedRequestHttpHeaders.Add("last-modified");
                            _AllowedRequestHttpHeaders.Add("content-range");
                        }
                    }
                }
                return _AllowedRequestHttpHeaders;
            }
        }

        public static IList<string> AllowedResourceParameters
        {
            get
            {
                if (_AllowedResourceParameters == null)
                {
                    object obj2 = _lock;
                    lock (obj2)
                    {
                        if (_AllowedResourceParameters == null)
                        {
                            _AllowedResourceParameters = new List<string>();
                            _AllowedResourceParameters.Add("acl");
                            _AllowedResourceParameters.Add("policy");
                            _AllowedResourceParameters.Add("torrent");
                            _AllowedResourceParameters.Add("logging");
                            _AllowedResourceParameters.Add("location");
                            _AllowedResourceParameters.Add("storageinfo");
                            _AllowedResourceParameters.Add("quota");
                            _AllowedResourceParameters.Add("storagepolicy");
                            _AllowedResourceParameters.Add("storageclass");
                            _AllowedResourceParameters.Add("requestpayment");
                            _AllowedResourceParameters.Add("versions");
                            _AllowedResourceParameters.Add("versioning");
                            _AllowedResourceParameters.Add("versionid");
                            _AllowedResourceParameters.Add("uploads");
                            _AllowedResourceParameters.Add("uploadid");
                            _AllowedResourceParameters.Add("partnumber");
                            _AllowedResourceParameters.Add("website");
                            _AllowedResourceParameters.Add("notification");
                            _AllowedResourceParameters.Add("lifecycle");
                            _AllowedResourceParameters.Add("delete");
                            _AllowedResourceParameters.Add("cors");
                            _AllowedResourceParameters.Add("restore");
                            _AllowedResourceParameters.Add("tagging");
                            _AllowedResourceParameters.Add("append");
                            _AllowedResourceParameters.Add("position");
                            _AllowedResourceParameters.Add("replication");
                            _AllowedResourceParameters.Add("response-content-type");
                            _AllowedResourceParameters.Add("response-content-language");
                            _AllowedResourceParameters.Add("response-expires");
                            _AllowedResourceParameters.Add("response-cache-control");
                            _AllowedResourceParameters.Add("response-content-disposition");
                            _AllowedResourceParameters.Add("response-content-encoding");
                            _AllowedResourceParameters.Add("x-image-process");
                        }
                    }
                }
                return _AllowedResourceParameters;
            }
        }

        public static IList<string> AllowedResponseHttpHeaders
        {
            get
            {
                if (_AllowedResponseHttpHeaders == null)
                {
                    object obj2 = _lock;
                    lock (obj2)
                    {
                        if (_AllowedResponseHttpHeaders == null)
                        {
                            _AllowedResponseHttpHeaders = new List<string>();
                            _AllowedResponseHttpHeaders.Add("content-type");
                            _AllowedResponseHttpHeaders.Add("content-md5");
                            _AllowedResponseHttpHeaders.Add("content-length");
                            _AllowedResponseHttpHeaders.Add("content-language");
                            _AllowedResponseHttpHeaders.Add("expires");
                            _AllowedResponseHttpHeaders.Add("origin");
                            _AllowedResponseHttpHeaders.Add("cache-control");
                            _AllowedResponseHttpHeaders.Add("content-disposition");
                            _AllowedResponseHttpHeaders.Add("content-encoding");
                            _AllowedResponseHttpHeaders.Add("x-default-storage-class");
                            _AllowedResponseHttpHeaders.Add("location");
                            _AllowedResponseHttpHeaders.Add("date");
                            _AllowedResponseHttpHeaders.Add("etag");
                            _AllowedResponseHttpHeaders.Add("host");
                            _AllowedResponseHttpHeaders.Add("last-modified");
                            _AllowedResponseHttpHeaders.Add("content-range");
                            _AllowedResponseHttpHeaders.Add("x-reserved");
                            _AllowedResponseHttpHeaders.Add("access-control-allow-origin");
                            _AllowedResponseHttpHeaders.Add("access-control-allow-headers");
                            _AllowedResponseHttpHeaders.Add("access-control-max-age");
                            _AllowedResponseHttpHeaders.Add("access-control-allow-methods");
                            _AllowedResponseHttpHeaders.Add("access-control-expose-headers");
                            _AllowedResponseHttpHeaders.Add("connection");
                        }
                    }
                }
                return _AllowedResponseHttpHeaders;
            }
        }

        public static IDictionary<string, string> MimeTypes
        {
            get
            {
                if (_MimeTypes == null)
                {
                    object obj2 = _lock;
                    lock (obj2)
                    {
                        if (_MimeTypes == null)
                        {
                            _MimeTypes = new Dictionary<string, string>();
                            _MimeTypes.Add("7z", "application/x-7z-compressed");
                            _MimeTypes.Add("aac", "audio/x-aac");
                            _MimeTypes.Add("ai", "application/postscript");
                            _MimeTypes.Add("aif", "audio/x-aiff");
                            _MimeTypes.Add("asc", "text/plain");
                            _MimeTypes.Add("asf", "video/x-ms-asf");
                            _MimeTypes.Add("atom", "application/atom+xml");
                            _MimeTypes.Add("avi", "video/x-msvideo");
                            _MimeTypes.Add("bmp", "image/bmp");
                            _MimeTypes.Add("bz2", "application/x-bzip2");
                            _MimeTypes.Add("cer", "application/pkix-cert");
                            _MimeTypes.Add("crl", "application/pkix-crl");
                            _MimeTypes.Add("crt", "application/x-x509-ca-cert");
                            _MimeTypes.Add("css", "text/css");
                            _MimeTypes.Add("csv", "text/csv");
                            _MimeTypes.Add("cu", "application/cu-seeme");
                            _MimeTypes.Add("deb", "application/x-debian-package");
                            _MimeTypes.Add("doc", "application/msword");
                            _MimeTypes.Add("docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                            _MimeTypes.Add("dvi", "application/x-dvi");
                            _MimeTypes.Add("eot", "application/vnd.ms-fontobject");
                            _MimeTypes.Add("eps", "application/postscript");
                            _MimeTypes.Add("epub", "application/epub+zip");
                            _MimeTypes.Add("etx", "text/x-setext");
                            _MimeTypes.Add("flac", "audio/flac");
                            _MimeTypes.Add("flv", "video/x-flv");
                            _MimeTypes.Add("gif", "image/gif");
                            _MimeTypes.Add("gz", "application/gzip");
                            _MimeTypes.Add("htm", "text/html");
                            _MimeTypes.Add("html", "text/html");
                            _MimeTypes.Add("ico", "image/x-icon");
                            _MimeTypes.Add("ics", "text/calendar");
                            _MimeTypes.Add("ini", "text/plain");
                            _MimeTypes.Add("iso", "application/x-iso9660-image");
                            _MimeTypes.Add("jar", "application/java-archive");
                            _MimeTypes.Add("jpe", "image/jpeg");
                            _MimeTypes.Add("jpeg", "image/jpeg");
                            _MimeTypes.Add("jpg", "image/jpeg");
                            _MimeTypes.Add("js", "text/javascript");
                            _MimeTypes.Add("json", "application/json");
                            _MimeTypes.Add("latex", "application/x-latex");
                            _MimeTypes.Add("log", "text/plain");
                            _MimeTypes.Add("m4a", "audio/mp4");
                            _MimeTypes.Add("m4v", "video/mp4");
                            _MimeTypes.Add("mid", "audio/midi");
                            _MimeTypes.Add("midi", "audio/midi");
                            _MimeTypes.Add("mov", "video/quicktime");
                            _MimeTypes.Add("mp3", "audio/mpeg");
                            _MimeTypes.Add("mp4", "video/mp4");
                            _MimeTypes.Add("mp4a", "audio/mp4");
                            _MimeTypes.Add("mp4v", "video/mp4");
                            _MimeTypes.Add("mpe", "video/mpeg");
                            _MimeTypes.Add("mpeg", "video/mpeg");
                            _MimeTypes.Add("mpg", "video/mpeg");
                            _MimeTypes.Add("mpg4", "video/mp4");
                            _MimeTypes.Add("oga", "audio/ogg");
                            _MimeTypes.Add("ogg", "audio/ogg");
                            _MimeTypes.Add("ogv", "video/ogg");
                            _MimeTypes.Add("ogx", "application/ogg");
                            _MimeTypes.Add("pbm", "image/x-portable-bitmap");
                            _MimeTypes.Add("pdf", "application/pdf");
                            _MimeTypes.Add("pgm", "image/x-portable-graymap");
                            _MimeTypes.Add("png", "image/png");
                            _MimeTypes.Add("pnm", "image/x-portable-anymap");
                            _MimeTypes.Add("ppm", "image/x-portable-pixmap");
                            _MimeTypes.Add("ppt", "application/vnd.ms-powerpoint");
                            _MimeTypes.Add("pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
                            _MimeTypes.Add("ps", "application/postscript");
                            _MimeTypes.Add("qt", "video/quicktime");
                            _MimeTypes.Add("rar", "application/x-rar-compressed");
                            _MimeTypes.Add("ras", "image/x-cmu-raster");
                            _MimeTypes.Add("rss", "application/rss+xml");
                            _MimeTypes.Add("rtf", "application/rtf");
                            _MimeTypes.Add("sgm", "text/sgml");
                            _MimeTypes.Add("sgml", "text/sgml");
                            _MimeTypes.Add("svg", "image/svg+xml");
                            _MimeTypes.Add("swf", "application/x-shockwave-flash");
                            _MimeTypes.Add("tar", "application/x-tar");
                            _MimeTypes.Add("tif", "image/tiff");
                            _MimeTypes.Add("tiff", "image/tiff");
                            _MimeTypes.Add("torrent", "application/x-bittorrent");
                            _MimeTypes.Add("ttf", "application/x-font-ttf");
                            _MimeTypes.Add("txt", "text/plain");
                            _MimeTypes.Add("wav", "audio/x-wav");
                            _MimeTypes.Add("webm", "video/webm");
                            _MimeTypes.Add("wma", "audio/x-ms-wma");
                            _MimeTypes.Add("wmv", "video/x-ms-wmv");
                            _MimeTypes.Add("woff", "application/x-font-woff");
                            _MimeTypes.Add("wsdl", "application/wsdl+xml");
                            _MimeTypes.Add("xbm", "image/x-xbitmap");
                            _MimeTypes.Add("xls", "application/vnd.ms-excel");
                            _MimeTypes.Add("xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                            _MimeTypes.Add("xml", "application/xml");
                            _MimeTypes.Add("xpm", "image/x-xpixmap");
                            _MimeTypes.Add("xwd", "image/x-xwindowdump");
                            _MimeTypes.Add("yaml", "text/yaml");
                            _MimeTypes.Add("yml", "text/yaml");
                            _MimeTypes.Add("zip", "application/zip");
                        }
                    }
                }
                return _MimeTypes;
            }
        }

        internal static class CommonHeaders
        {
            public const string AccessControlRequestHeader = "Access-Control-Request-Headers";
            public const string Authorization = "Authorization";
            public const string CacheControl = "Cache-Control";
            public const string ContentDisposition = "Content-Disposition";
            public const string ContentEncoding = "Content-Encoding";
            public const string ContentLength = "Content-Length";
            public const string ContentMd5 = "Content-MD5";
            public const string ContentType = "Content-Type";
            public const string Date = "Date";
            public const string ETag = "ETag";
            public const string Expires = "Expires";
            public const string Host = "Host";
            public const string IfMatch = "If-Match";
            public const string IfModifiedSince = "If-Modified-Since";
            public const string IfNoneMatch = "If-None-Match";
            public const string IfUnmodifiedSince = "If-Unmodified-Since";
            public const string LastModified = "Last-Modified";
            public const string Location = "Location";
            public const string OriginHeader = "Origin";
            public const string Range = "Range";
            public const string UserAgent = "User-Agent";
        }

        internal static class ObsRequestParams
        {
            public const string Delimiter = "delimiter";
            public const string ImageProcess = "x-image-process";
            public const string KeyMarker = "key-marker";
            public const string Marker = "marker";
            public const string MaxKeys = "max-keys";
            public const string MaxParts = "max-parts";
            public const string MaxUploads = "max-upload";
            public const string PartNumber = "partNumber";
            public const string PartNumberMarker = "part-number-marker";
            public const string Position = "position";
            public const string Prefix = "prefix";
            public const string ResponseCacheControl = "response-cache-control";
            public const string ResponseContentDisposition = "response-content-disposition";
            public const string ResponseContentEncoding = "response-content-encoding";
            public const string ResponseContentLanguage = "response-content-language";
            public const string ResponseContentType = "response-content-type";
            public const string ResponseExpires = "response-expires";
            public const string UploadId = "uploadId";
            public const string UploadIdMarker = "upload-id-marker";
            public const string VersionId = "versionId";
            public const string VersionIdMarker = "version-id-marker";
        }
    }
}

