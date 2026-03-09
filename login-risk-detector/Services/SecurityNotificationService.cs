using Microsoft.AspNetCore.Identity.UI.Services;

public class SecurityNotificationService
{
    private readonly IEmailSender _emailSender;

    public SecurityNotificationService(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task SendLockoutWarningAsync(string email)
    {
        string subject = "Security alert: Account locked";

        string message =
        "We detected a suspicious login attempt and temporarily locked your account for 15 minutes.";

        await _emailSender.SendEmailAsync(email, subject, message);
    }
}