using RestSharp;

namespace ClientConsole
{
    internal static class PerkjamApi
    {
        private const string CinemaReplaceOfferUrlEndPoint = "offers/replace-cinema-offer-url";
        private const string CinemaRefreshOfferUrlEndPoint = "offers/refresh-cinema-offer-url";
        
        internal static async Task<bool> ReplaceCinemaUrlAsync(string tokenisedUrl, string accessToken)
        {
            var config = AuthConfig.ReadFromJsonFile("appsettings.json");
            
            var queryParameters = new Dictionary<string, string>()
            {
                { "OfferUrl", tokenisedUrl }
            };
            var builtRequest = new RequestBuilder(
                baseUrl: config.BaseAddress,
                endpoint: CinemaReplaceOfferUrlEndPoint,
                queryParameters: queryParameters,
                accessToken: accessToken
            );
            try
            {
                var response = await builtRequest.Client.ExecutePutAsync(builtRequest.Request);
                return Helpers.MaybeLogError(response);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error calling {CinemaReplaceOfferUrlEndPoint} on Perkjam API. Error: {e}");
                return false;
            }
        }
        
        internal static async Task<bool> RefreshCinemaUrlAsync()
        {
            var config = AuthConfig.ReadFromJsonFile("appsettings.json");
            
            var builtRequest = new RequestBuilder(
                baseUrl: config.BaseAddress,
                endpoint: CinemaRefreshOfferUrlEndPoint,
                queryParameters: new Dictionary<string, string>()
            );
            try
            {
                var response = await builtRequest.Client.ExecutePutAsync(builtRequest.Request);
                return Helpers.MaybeLogError(response);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error calling {CinemaReplaceOfferUrlEndPoint} on Perkjam API. Error {e}");
                return false;
            }
        }
    }
}