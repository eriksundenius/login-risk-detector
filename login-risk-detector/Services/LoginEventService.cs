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
                .Where(e => e.UserId == userId && e.Succeeded)
                .OrderByDescending(e => e.Timestamp)
                .FirstOrDefaultAsync();
        }

    public async Task<List<LoginEvent?>> GetRecentSuccessfulLogins(string userId, DateTime since, int count)
        {
            return await _db.LoginEvents
                .Where(e => e.UserId == userId && e.Succeeded && e.Timestamp >= since)
                .OrderByDescending(e => e.Timestamp)
                .Take(count)
                .ToListAsync();
        }

    public async Task<List<LoginEvent?>> GetLastFailedLogins(string userId, DateTime since, int count)
        {
            return await _db.LoginEvents
                .Where(e => e.UserId == userId && !e.Succeeded && e.Timestamp >= since)
                .OrderByDescending(e => e.Timestamp)
                .Take(count)
                .ToListAsync();
        }

        public async Task<bool> HasSeenDevice(string userId, string userAgent)
        {
            return await _db.LoginEvents
                .AnyAsync(e => e.UserId == userId && e.UserAgent == userAgent);
            //Any() finns det minst en rad som matchar
        }


    }
}
