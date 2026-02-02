using System.ComponentModel.DataAnnotations;

namespace login_risk_detector.Models
{
    public class RiskResult
    {
        [Range(0, 100)]
        public int RiskScore { get; set; } = 0;
        public RiskLevel RiskLevel { get; set; }
        public List<string> Reasons { get; set; } = new();

    }

}
