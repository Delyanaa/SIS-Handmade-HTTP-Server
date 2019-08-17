namespace SIS.HTTP.Headers.Contracts
{
    /*describe the behavior of a Repository-like
    object for the HttpHeaders.*/

    /*Create an interface, called IHttpHeaderCollection, 
     * which will describe the behavior of a Repository-like
     * object for the HttpHeaders.*/
    public interface IHttpHeaderCollection
    {
        void AddHeader(HttpHeader header);
        bool ContainsHeader(string key);
        HttpHeader GetHeader(string key);
    }
}
