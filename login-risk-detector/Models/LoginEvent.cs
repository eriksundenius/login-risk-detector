namespace login_risk_detector.Models
{
    public class LoginEvent
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; } = null!;
        public string UserAgent { get; set; } = null!; // Detta är för webbläsare/enhet
        public bool Succeeded { get; set; } // om för många false ska det flaggas
        public string CountryCode { get; set; } = null!;

    }
}
// Vid inlogg skapar ett LoginEventObjekt och spara i databasen så vi kan jämföra med framtida inlogg
