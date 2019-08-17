using System;

namespace SIS.HTTP.Exceptions
{
    /*Exception will be thrown whenever there is an error 
     * that the Server was not supposed to encounter.*/
    public class InternalServerErrorException : Exception
    {
        private const string InternalServerErrorExceptionDefaultMessage = "The Server has encountered an error.";

        public InternalServerErrorException() : this(InternalServerErrorExceptionDefaultMessage) { }

        public InternalServerErrorException(string name) : base(name) { }
    }
}
