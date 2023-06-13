#nullable enable

using RestSharp;
using RestSharp.Authenticators;

namespace ClientConsole
{
    internal class RequestBuilder
    {
        private readonly List<RequestBuildRule> _rules = new List<RequestBuildRule>();
        private readonly Dictionary<string, string> _queryParameters;
        private readonly string _baseUrl;
        private readonly string _endpoint;
        private readonly string? _accessToken;

        internal RestClient Client { get; private set; }
        internal RestRequest Request { get; private set; }
        private string Url => Path.Combine(_baseUrl, _endpoint);
        
        internal RequestBuilder(string baseUrl, string endpoint, Dictionary<string, string> queryParameters, string accessToken)
        {
            _baseUrl = baseUrl;
            _endpoint = endpoint;
            _queryParameters = queryParameters;
            _accessToken = accessToken;
            
            BuildClient();
            BuildRequest();
            AddParameters();
        }
        
        internal RequestBuilder(string baseUrl, string endpoint, Dictionary<string, string> queryParameters)
        {
            _baseUrl = baseUrl;
            _endpoint = endpoint;
            _queryParameters = queryParameters;
            
            BuildClient();
            BuildRequest();
            AddParameters();
        }

        private void BuildClient()
        {
            if (_rules.Contains(RequestBuildRule.AuthenticatorInRequest) || _accessToken is null)
            {
                Client = new RestClient(Url);
                return;
            }
            
            var options = new RestClientOptions(Url) {
                Authenticator = new JwtAuthenticator(_accessToken),
            };
            Client = new RestClient(options);
        }

        private void BuildRequest()
        {
            if (_rules.Contains(RequestBuildRule.AuthenticatorInRequest))
            {
                Request = new RestRequest
                {
                    Authenticator = new JwtAuthenticator(_accessToken)
                };
                return;
            }

            Request = new RestRequest();
        }

        private void AddParameters()
        {
            if (_rules.Contains(RequestBuildRule.ParametersInParameters))
            {
                foreach (var queryParameter in _queryParameters)
                {
                    Request.AddParameter(queryParameter.Key, queryParameter.Value);
                }
                return;
            }
            
            Request.AddBody(_queryParameters);
        }
    }

    internal enum RequestBuildRule
    {
        None,
        ParametersInParameters,
        AuthenticatorInRequest
    }
}