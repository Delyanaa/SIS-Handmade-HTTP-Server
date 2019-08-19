using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;
using SIS.HTTP.Enums;
using SIS.HTTP.Cookies;
using System.IO;
using System.Runtime.CompilerServices;
using SIS.HTTP.Requests.Contracts;

namespace Demo.App.Controllers
{
    public abstract class BaseController
    {
        protected IHttpRequest HttpRequest { get; set; }
        private bool IsLoggedIn()
        {
            return HttpRequest.Session.ContainsParameter(parameterName: "username");
        }

        private string ParseTemplate(string viewContent)
        {
            if (this.IsLoggedIn())
            {
                return viewContent.Replace(oldValue: "@Model.HelloMessage",
                    newValue: $"Hello, " +
                    $"{this.HttpRequest.Session.GetParameter(parameterName: "username")}"); 
            }
            else
            {
                return viewContent.Replace(oldValue: "@Model.HelloMessage",
                    newValue: "Hello World From SIS");
            }
        }

        public IHttpResponse View([CallerMemberName] string view = null)
        {
            string controllerName = this.GetType().Name.Replace("Controller", string.Empty);
            string viewName = view;

            string viewContent = File.ReadAllText("Views/" + controllerName + "/" + viewName + ".html");

            viewContent = this.ParseTemplate(viewContent);
            HtmlResult htmlResult = new HtmlResult(viewContent, HttpResponseStatusCode.Ok); ;
            htmlResult.Cookies.AddCookie(new HttpCookie(key: "lang", value: "en"));

            return htmlResult;
        }

        public IHttpResponse Redirect(string url)
        {
            return new RedirectResult(url);
        } 
    }
}
