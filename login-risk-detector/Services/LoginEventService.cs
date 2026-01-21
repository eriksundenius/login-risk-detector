using Microsoft.EntityFrameworkCore;
using login_risk_detector.Data;
using login_risk_detector.Models;
using System.Linq;

namespace login_risk_detector.Services
{ 
    //Historik och lagring
    public class LoginEventService
    {
        private readonly ApplicationDbContext _db;

    public LoginEventService(ApplicationDbContext db)
        {
            _db = db;
        }

    public async Task<LoginEvent?> GetLastSuccessfulLogin(string userId)
        {
            return await _db.LoginEvents
                .Where(e => e.UserId == userId && e.Suceeded)
                .OrderByDescending(e => e.Timestamp)
                .FirstOrDefaultAsync();
        }

    public async Task<List<LoginEvent>> GetRecentSuccessfulLogins(string userId, DateTime since, int count)
        {
            return await _db.LoginEvents
                .Where(e => e.UserId == userId && e.Suceeded && e.Timestamp >= since)
                .OrderByDescending(e => e.Timestamp)
                .Take(count)
                .ToListAsync();
        }

    public async Task<List<LoginEvent?>> GetLastUnSuccessfulLogins(string userId, DateTime since, int count)
        {
            return await _db.LoginEvents
                .Where(e => e.UserId == userId && e.Suceeded == false && e.Timestamp >= since)
                .OrderByDescending(e => e.Timestamp)
                .Take(count)
                .ToListAsync();
        }
    }
}
