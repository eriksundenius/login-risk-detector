using login_risk_detector.Models;

namespace login_risk_detector.Services
{
    public interface IGeoLocationService
    {
        Task<String> GetCountryCodeAsync(string ipAddress);
        //interface för att kuna ha fler geolocation metoder i andra klassen ex hårkodade locations för test

    }
}
