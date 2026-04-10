using Clubly.Model;
using Clubly.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using SignUp.Data;

namespace Clubly.Repository.Class
{
    public class GuestRepository : IGuestRepository
    {
        private readonly AppDbContext _db;

        public GuestRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Guest>> GetAllAsync()
        {
            return await _db.Guests
                .Include(g => g.ActivityBookings)
                .ToListAsync();
        }

        public async Task<Guest?> GetByIdAsync(int id)
        {
            return await _db.Guests
                .Include(g => g.ActivityBookings)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task AddAsync(Guest guest)
        {
            _db.Guests.Add(guest);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guest guest)
        {
            _db.Guests.Update(guest);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guest guest)
        {
            _db.Guests.Remove(guest);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Guests.AnyAsync(g => g.Id == id);
        }

        public async Task<Guest?> GetByEmailAsync(string email)
        {
            return await _db.Guests
                .FirstOrDefaultAsync(g => g.Email == email);
        }
    }
}