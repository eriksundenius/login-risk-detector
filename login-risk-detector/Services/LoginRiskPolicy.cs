using login_risk_detector.Models;


namespace login_risk_detector.Services
{
    //Ta in scoret och ta beslut om actions
    public class LoginRiskPolicy
    {
        public RiskLevel GetRiskLevel(int riskScore)
        {
            if (riskScore >= 45)
                return RiskLevel.High;
            if(riskScore >= 20)
                return RiskLevel.Medium;
            return RiskLevel.Low;
        }

        public async Task DecideAction(RiskLevel riskLevel)
        {
            if (riskLevel == RiskLevel.High)
                await denyLogin();
        }

        public async Task denyLogin() //Inte dela ut en en token.
        {
            
        }

    }
}
