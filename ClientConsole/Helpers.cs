using System.Net;
using RestSharp;
using static System.Console;

namespace ClientConsole
{
    internal static class Helpers
    {
        internal static bool MaybeLogError(RestResponse restResponse)
        {
            if (restResponse.IsSuccessful)
                return true;
            
            switch (restResponse.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    WriteLine("Call to {Uri} failed with a {StatusCode}: {ErrorMessage}",
                        restResponse.ResponseUri,
                        restResponse.StatusCode,
                        restResponse.ErrorMessage);
                    break;
                default:
                    WriteLine("Call to {Uri} failed with a {StatusCode}: {ErrorMessage}", restResponse.ResponseUri, restResponse.StatusCode, restResponse.ErrorMessage);
                    break;
            }

            return false;
        }
    }
}