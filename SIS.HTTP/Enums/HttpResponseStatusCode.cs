namespace SIS.HTTP.Enums
{
    /*Create an enumeration, called HttpResponseStatusCode, 
     * which will be used to define the status of the response 
     * our Server will be sending. This enumeration should hold 
     * values which are the statuses and integer values which 
     * will be the codes.*/
    public enum HttpResponseStatusCode
    {
        Ok = 200,
        Create = 201, 
        Found = 302,
        SeeOther = 303,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 500
    }
}
