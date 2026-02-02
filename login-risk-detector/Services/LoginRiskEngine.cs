using login_risk_detector.Services;
using login_risk_detector.Models;
using NuGet.Packaging.Signing;


namespace login_risk_detector.Services
{
    //Beräkna vilka siglans som triggas
    public class LoginRiskEngine
    {
        private readonly LoginEventService _loginEventService;
        public LoginRiskEngine(LoginEventService loginEventService)
        {
            _loginEventService = loginEventService;
        }

        public async Task<RiskResult> AssessRisk(LoginEvent currentLogin)
        {
            var result = new RiskResult();
            await CheckDevice(currentLogin, result);
            await CheckCountry(currentLogin, result);
            return result;

        }

        private async Task CheckDevice(LoginEvent currentLogin, RiskResult result)
        {
            bool hasSeenDevice = await _loginEventService.HasSeenDevice(currentLogin.UserId, currentLogin.UserAgent);
            if (hasSeenDevice == true)
            {
                return;
            } else
            {
                result.RiskScore += 10;
                result.Reasons.Add("New device detected");
            }
            return;
        }

        private async Task CheckCountry(LoginEvent currentLogin, RiskResult result)
        {
            bool hasSeenCountry = await _loginEventService.HasSeenCountry(currentLogin.UserId, currentLogin.CountryCode);
                if (hasSeenCountry == true) {
                return;
            } else
            {
                result.RiskScore += 20;
                result.Reasons.Add("New Country detected");
            } 
        }
    }
}
