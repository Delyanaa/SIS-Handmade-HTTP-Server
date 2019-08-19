using SIS.HTTP.Common;
using SIS.HTTP.Sessions.Contracts;
using System.Collections.Generic;

namespace SIS.HTTP.Sessions
{
    public class HttpSession : IHttpSession
    {
        private readonly Dictionary<string, object> sessionParameters;

        public HttpSession(string id)
        {
            this.Id = id;
            this.sessionParameters = new Dictionary<string, object>();
            this.IsNew = true;
    }
        public string Id { get; }
        public bool IsNew { get; set; }
        /* Extracts, 
       * form the parameter collection,
       * the parameter with the given name,
       * and returns it.*/
        public object GetParameter(string parameterName)
        {
            CoreValidator.ThrowIfNullOrEmpty(parameterName, nameof(parameterName));

            //TODO: Validation for existing parameter (throw exception?)
            return this.sessionParameters[parameterName];
        }


        /* Adds the given parameter 
         * with the given name to a 
         * key-value-pair collection of parameters.*/
        public void AddParameter(string parameterName, object parameter)
        {
            CoreValidator.ThrowIfNullOrEmpty(parameterName, nameof(parameterName));
            CoreValidator.ThrowIfNull(parameter, nameof(parameter));

            this.sessionParameters[parameterName] = parameter;
        }

        /* Returns 
         * a boolean result, on whether a parameter 
         * with given name is contained in the collection.*/
        public bool ContainsParameter(string parameterName)
        {
            CoreValidator.ThrowIfNullOrEmpty(parameterName, nameof(parameterName));

            return this.sessionParameters.ContainsKey(parameterName);
        }

        /*clears 
         * the collection, emptying it*/
        public void ClearParameters()
        {
            this.sessionParameters.Clear();
        }

    }
}
