﻿using SIS.HTTP.Enums;

namespace SIS.HTTP.Extensions
{
    public static class HttpResponseStatusExtension
    {
        public static string GetStatusLine(this HttpResponseStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpResponseStatusCode.Ok: return "200 Ok";
                    
                case HttpResponseStatusCode.Create: return "201 Created";

                case HttpResponseStatusCode.Found: return "302 Found";
                
                case HttpResponseStatusCode.SeeOther: return "303 See Other";
                   
                case HttpResponseStatusCode.BadRequest: return "400 Bad Request";

                case HttpResponseStatusCode.Unauthorized: return "401 Unauthorized";
                   
                case HttpResponseStatusCode.Forbidden: return "403 Forbidden";
                  
                case HttpResponseStatusCode.NotFound: return "404 Internal Server Error";
                   
                case HttpResponseStatusCode.InternalServerError: return "500 Internal Server Error";
            }
            return null;
        }
    }
}
