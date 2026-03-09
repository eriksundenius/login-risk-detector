using login_risk_detector.Models;
using login_risk_detector.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using NuGet.Protocol.Core.Types;
using System;
using System.Threading.Tasks;


namespace login_risk_detector.Services
{
    //Ta in scoret och ta beslut om actions
    public class LoginRiskPolicy
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SecurityNotificationService _securityNotificationService;

        public LoginRiskPolicy(UserManager<IdentityUser> userManager, SecurityNotificationService emailSender)
        {
            _userManager = userManager;
            _securityNotificationService = emailSender;
        }


        public RiskLevel GetRiskLevel(int riskScore)
        {
            if (riskScore >= 45)
                return RiskLevel.High;
            if(riskScore >= 20)
                return RiskLevel.Medium;
            return RiskLevel.Low;
        }

        public async Task DecideAction(RiskLevel riskLevel, IdentityUser user)
        {
            if (riskLevel == RiskLevel.High)
                await denyLoginTimeOut(user);
        }

        public async Task denyLoginTimeOut(IdentityUser user) 
        {
            var lockoutEnd = DateTimeOffset.UtcNow.AddMinutes(15);

            await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);
            await _securityNotificationService.SendLockoutWarningAsync(user.Email);
        }

    }
}
