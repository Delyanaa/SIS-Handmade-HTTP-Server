using SIS.HTTP.Sessions.Contracts;
using System.Collections.Concurrent;
using SIS.HTTP.Sessions;


namespace SIS.HTTP.Sessions
{
    class HttpSessionStorage
    {
        public const string SessionCookieKey = "SIS_ID";

        private static readonly ConcurrentDictionary<string, IHttpSession> sessions =
            new ConcurrentDictionary<string, IHttpSession>();

        /* Retrieves 
         * a Session from the Session Storage collection 
         * if it exists, or adds it and then retrieves it, 
         * if it does NOT exist.*/
        public static IHttpSession GetSessions(string id)
        {
            return sessions.GetOrAdd(id, _ => new HttpSession(id));
        }
    }
}
