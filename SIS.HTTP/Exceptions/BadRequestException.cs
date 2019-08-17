using System;

namespace SIS.HTTP.Exceptions
{
    /*The Server will catch errors of BadRequestException type, first. 
     * If it catches an error of this type, a 400 Bad Request Response 
     * will be returned, with the Exception’s message as content.*/

    /*This exception will be thrown when there is an error 
     * with the parsing of the HttpRequest, e.g. Unsupported 
     * HTTP Protocol, Unsupported HTTP Method, Malformed Request etc.*/

    public class BadRequestException : Exception
    {
        private const string BadExceptionDefaultMessage = "The Request was malformed or contains unsupported elements.";

        public BadRequestException() : this(BadExceptionDefaultMessage) { }

        public BadRequestException(string name) { }
    }
}

