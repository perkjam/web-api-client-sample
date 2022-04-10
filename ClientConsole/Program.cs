using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace Perkjam.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Making the call...");
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            AuthConfig config = AuthConfig.ReadFromJsonFile("appsettings.json");

            IConfidentialClientApplication app;

            app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                .WithClientSecret(config.ClientSecret)
                .WithAuthority(new Uri(config.Authority))
                .Build();

            string[] resourceIds = new string[] { config.ResourceId };

            AuthenticationResult result = null;
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
            //Console.WriteLine("Press enter to retrieve users from the Web API");
            //Console.ReadLine();
            //Console.WriteLine();

            if (!string.IsNullOrEmpty(result?.AccessToken))
            {
                var httpClient = new HttpClient();
                var defaultRequestHeaders = httpClient.DefaultRequestHeaders;

                if (defaultRequestHeaders.Accept == null || !defaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }

                defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);

                // Test Users functionality
                // ========================
                //if (response.IsSuccessStatusCode)
                //{
                //    Console.ForegroundColor = ConsoleColor.Green;
                //    string json = await response.Content.ReadAsStringAsync();
                //    Console.WriteLine("Retrieving users from Web API:");
                //    Console.WriteLine(json);
                //}
                //else
                //{
                //    Console.ForegroundColor = ConsoleColor.Red;
                //    Console.WriteLine($"Failed to call the Web Api: {response.StatusCode}");
                //    string content = await response.Content.ReadAsStringAsync();
                //    Console.WriteLine($"Content: {content}");
                //}
                //Console.WriteLine();

                //Console.WriteLine("Post a new user to the database");
                //Console.WriteLine("Enter new user name for testing [example 'Donald Duck']:");
                //var newUserName = Console.ReadLine();
                //Console.WriteLine("Enter new user email for testing [example 'donald@gmail.com']:");
                //var newUserEmail = Console.ReadLine();
                //var newUserEmailAddress = new 
                //{ 
                //    Name = newUserName, 
                //    Email = newUserEmail, 
                //    ClientUserId = "TeslaClientId", 
                //    ClientId = config.ClientId, 
                //    VendorId = config.VendorId 
                //};
                //HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(newUserEmailAddress));
                //httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //var postResponse = await httpClient.PostAsync(config.BaseAddress + "users", httpContent);
                //if (postResponse.IsSuccessStatusCode)
                //{
                //    Console.ForegroundColor = ConsoleColor.Green;
                //    Console.WriteLine("Successfully posted new user to database:");
                //}
                //else
                //{
                //    Console.ForegroundColor = ConsoleColor.Red;
                //    Console.WriteLine($"Failed to call the Web Api and post new user to database: {postResponse.StatusCode}");
                //}

                Console.WriteLine("Post a new PowerUp to the database");
                Console.WriteLine("Enter new client id for testing [example 123]:");
                var newClientId = Console.ReadLine();
                Console.WriteLine("Enter new user email for testing [example 'donald@gmail.com']:");
                var newUserEmail = Console.ReadLine();
                var newPowerUp = new
                {
                    ClientId = newClientId,
                    Email = newUserEmail
                };
                var httpContent = new StringContent(JsonConvert.SerializeObject(newPowerUp));
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var postResponse = await httpClient.PostAsync(config.BaseAddress + "powerups", httpContent);
                if (postResponse.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Successfully posted new PowerUp to database:");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Failed to call the Web Api and post new PowerUp email to database: {postResponse.StatusCode}");
                }

                Console.ResetColor();
            }
        }
    }
}