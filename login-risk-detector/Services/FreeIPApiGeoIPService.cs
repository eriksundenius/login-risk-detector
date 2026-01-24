using login_risk_detector.Models;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Threading;

namespace login_risk_detector.Services
{
    public class FreeIPApiGeoIPService : IGeoLocationService
    {
        //Detta är med hjälp av https://weblogs.asp.net/ricardoperes/getting-location-and-weather-from-an-ip-address/ väldigt bra
        
        //Det är options för hur JSON ska tolkas. Gör CamelCase så det blir C# property
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        private readonly HttpClient _httpClient;

        public FreeIPApiGeoIPService(HttpClient httpClient)
        {
            //Exception om säg sidan skulle ligga nere 
            ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://freeipapi.com/api/json/");
            _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, MediaTypeNames.Application.Json);
            //Länken och att det ska vara Json
            _httpClient.DefaultRequestHeaders.Add(HeaderNames.CacheControl, "public, max-age=3600");
            //sparas 1 timme i cachen. Inget krav men microsoft hade det, de är bättre prestanda
        }


        public async Task<string> GetCountryCodeAsync(string ipAddress)
        {
            //en if sats direkt om IPn inte finns eller är tom slipper göra API anrop snabbare
            if (string.IsNullOrEmpty(ipAddress))
                return "UNKNOWN";


            var geoInfo = await _httpClient.GetFromJsonAsync<FreeAPIGeoInfo>(ipAddress, _options);//Här gör faktiskt anropet
            //Gör om Json filen till freeapiGeoinfo objektet

            //tar bara country code. Blir unknown om det inte gick att hämta pga fler anledningar
            var code = geoInfo?.CountryCode;
            return string.IsNullOrWhiteSpace(code) ? "UNKNOWN" : code;
        }

       
    }
}
