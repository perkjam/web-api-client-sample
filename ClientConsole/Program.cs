using System.Net.Http.Headers;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace ClientConsole
{
    public static class Program
    {
        static void Main()
        {
            Console.WriteLine("Making the call...");
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            var config = AuthConfig.ReadFromJsonFile("appsettings.json");

            // var accessToken = await CallApiToGetToken();
            // if (string.IsNullOrEmpty(accessToken))
            //     return;
            //
            // var httpClient = ConfigureClient(accessToken);
            
            // Call Replace Offer Endpoint
            // ===========================
            // Console.WriteLine("Call Replace Offer Endpoint");
            // var postData = new 
            // { 
            //     OfferUrl = "Test URL"
            // };
            // var httpContent = new StringContent(JsonConvert.SerializeObject(postData));
            // httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            // var response = await httpClient.PutAsync(config.BaseAddress + "offers/replace-cinema-offer-url", httpContent);
            // if (response.IsSuccessStatusCode)
            // {
            //     Console.ForegroundColor = ConsoleColor.Green;
            //     Console.WriteLine("Successfully updated offer URL");
            // }
            // else
            // {
            //     Console.ForegroundColor = ConsoleColor.Red;
            //     Console.WriteLine($"Failed to call the Web Api and update the offer URL: {response.StatusCode}");
            //     var content = await response.Content.ReadAsStringAsync();
            //     Console.WriteLine($"Content: {content}");
            // }
            
            // Call Replace Offer Endpoint
            // ===========================
            var success = PerkjamApi.RefreshCinemaUrlAsync().Result;


            // Test API Access
            // ===============
            // Console.WriteLine("Test API access");
            // var testResponse = await httpClient.GetAsync(config.BaseAddress + "test/unauthorised");
            // //var testResponse = await httpClient.GetAsync(config.BaseAddress + "test/authorised");
            // //var testResponse = await httpClient.GetAsync(config.BaseAddress + "test/authorised-check-database-connection");
            // if (testResponse.IsSuccessStatusCode)
            // {
            //     Console.ForegroundColor = ConsoleColor.Green;
            //     Console.WriteLine("Successfully accessed PerkjamWebAPI");
            // }
            // else
            // {
            //     Console.ForegroundColor = ConsoleColor.Red;
            //     Console.WriteLine($"Failed to call the Web Api: {testResponse.StatusCode}");
            //     var content = await testResponse.Content.ReadAsStringAsync();
            //     Console.WriteLine($"Content: {content}");
            // }

            // Post a new user
            // ===============
            // Console.WriteLine("Post a new user to the database");
            // Console.WriteLine("Enter new user name for testing [example 'Donald Duck']:");
            // var newUserName = Console.ReadLine();
            // Console.WriteLine("Enter new user email for testing [example 'donald@gmail.com']:");
            // var newUserEmail = Console.ReadLine();
            // var newUserEmailAddress = new 
            // { 
            //     Name = newUserName, 
            //     Email = newUserEmail, 
            //     ClientUserId = "TeslaClientId",
            //     config.ClientId,
            //     config.VendorId 
            // };
            // HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(newUserEmailAddress));
            // httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //
            // var postResponse = await httpClient.PostAsync(config.BaseAddress + "users", httpContent);
            // if (postResponse.IsSuccessStatusCode)
            // {
            //     Console.ForegroundColor = ConsoleColor.Green;
            //     Console.WriteLine("Successfully posted new user to database:");
            // }
            // else
            // {
            //     Console.ForegroundColor = ConsoleColor.Red;
            //     Console.WriteLine($"Failed to call the Web Api and post new user to database: {postResponse.StatusCode}");
            // }
            // ==================
            
            
            
            // Post a new PowerUp
            // ==================
            // Console.WriteLine("Post a new PowerUp to the database");
            // Console.WriteLine("Enter new client id for testing [example 123]:");
            // var newClientId = Console.ReadLine();
            // Console.WriteLine("Enter new user email for testing [example 'donald@gmail.com']:");
            // var newUserEmail = Console.ReadLine();
            // var newPowerUp = new
            // {
            //     ClientId = newClientId,
            //     Email = newUserEmail
            // };
            // var httpContent = new StringContent(JsonConvert.SerializeObject(newPowerUp));
            // httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //
            // var postResponse = await httpClient.PostAsync(config.BaseAddress + "powerups", httpContent);
            // if (postResponse.IsSuccessStatusCode)
            // {
            //     Console.ForegroundColor = ConsoleColor.Green;
            //     Console.WriteLine("Successfully posted new PowerUp to database:");
            // }
            // else
            // {
            //     Console.ForegroundColor = ConsoleColor.Red;
            //     Console.WriteLine($"Failed to call the Web Api and post new PowerUp email to database: {postResponse.StatusCode}");
            // }
            // ==================

            Console.ResetColor();
        }

        private static HttpClient ConfigureClient(string accessToken)
        {
            var result = new HttpClient();
            var defaultRequestHeaders = result.DefaultRequestHeaders;

            if (defaultRequestHeaders.Accept.All(m => m.MediaType != "application/json"))
            {
                result.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

            return result;
        }

        private static async Task<string> CallApiToGetToken()
        {
            var config = AuthConfig.ReadFromJsonFile("appsettings.json");
            
            var app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                .WithClientSecret(config.ClientSecret)
                .WithAuthority(new Uri(config.Authority))
                .Build();

            var resourceIds = new[] { config.ResourceId };

            AuthenticationResult? result = null;
            try
            {
                result = await app.AcquireTokenForClient(resourceIds).ExecuteAsync();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Token acquired:");
                Console.WriteLine(result.AccessToken);
                Console.ResetColor();
            }
            catch (MsalClientException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            Console.WriteLine();

            return result == null
                ? string.Empty
                : result.AccessToken;
        }
    }
}