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
            await ImpossibleTravel(currentLogin, result);
            await BruteForce(currentLogin, result);
            await SuspiciusTime(currentLogin, result);

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
        
        //Anledningen till att vi jämför med lastsucessful och inte lastloginattempt är för att
        //bottar eller andra attacker kan trigga denna och ge falska alarm hela tiden
        private async Task ImpossibleTravel(LoginEvent currentLogin, RiskResult result) 
        {
            var lastLogin = await _loginEventService.GetLastSuccessfulLogin(currentLogin.UserId);
            if (lastLogin == null) return; //Första inlogget.
            if (lastLogin.CountryCode == currentLogin.CountryCode)
            {
                return;
            }
            else 
            {
                var hours = (currentLogin.Timestamp - lastLogin.Timestamp).TotalHours;
                if (hours >= 0 && hours <=2)//nytt land inom 2 timmar 
                {
                    result.RiskScore += 50;
                    result.Reasons.Add("Impossible Travel");
                }
            }
        }

        private async Task BruteForce(LoginEvent currentLogin, RiskResult result)
        {
            DateTime since = DateTime.UtcNow.AddMinutes(-15);
            int count = 5;

            var failedLogins = await _loginEventService.GetLastFailedLogins(currentLogin.UserId, since, count);
            if(failedLogins.Count == 0) { return; }
            if(failedLogins.Count >= count) 
            {  
                result.RiskScore += 50;
                result.Reasons.Add("Brute force detected");
            }
        }

        private async Task SuspiciusTime(LoginEvent currentLogin, RiskResult result) 
        {
            int count = 20;
            var logins = await _loginEventService.GetAllLoginsTime(currentLogin.UserId, count);
            if (logins.Count == 0) return;

            double averageHour = logins.Average(t => t.Hour);
            double thisHour = currentLogin.Timestamp.Hour;
            double difference = thisHour - averageHour;

                if(difference > 10)
                {
                    result.RiskScore += 20;
                    result.Reasons.Add("Suspicius Time");
                }
            
        }

    }
}
