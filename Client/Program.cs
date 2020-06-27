using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Console Client!");
            new AuthenticationRunner().Run();
            Console.ReadKey();
        }
    }

    public abstract class AuthenticationRunnerBase
    {
        protected string _apiClientUrl { get; }
        protected string _idServerUrl { get; }
        protected string _tokenScope { get; }
        public AuthenticationRunnerBase(string apiClientUrl, string idServerUrl, string tokenScope)
        {
            _apiClientUrl = apiClientUrl;
            _idServerUrl = idServerUrl;
            _tokenScope = tokenScope;
        }

        public void Run()
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync(_idServerUrl).Result;
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // request token
            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = _tokenScope
            }).Result;

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            // call api
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = apiClient.GetAsync(_apiClientUrl).Result;
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }

    public class AuthenticationRunner : AuthenticationRunnerBase
    {
        public AuthenticationRunner() : base(apiClientUrl:"https://localhost:44379/identity", idServerUrl:"https://localhost:5001", "CheckoutApi") { }
    }
}
