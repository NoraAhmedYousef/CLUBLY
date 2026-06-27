using Microsoft.EntityFrameworkCore;
using SignUp.Data;
using SignUp.Model;
using SignUp.Repository.Interfaces;

namespace SignUp.Repository.Class
{
    public class FacilityRepository : IFacilityRepository
    {
        private readonly AppDbContext _context;

        public FacilityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Facility>> GetAllAsync() =>
            await _context.Facilities
                          .Include(f => f.FacilityCategory)
                          .ToListAsync();

        public async Task<Facility?> GetByIdAsync(int id) =>
            await _context.Facilities
                          .Include(f => f.FacilityCategory)
                          .FirstOrDefaultAsync(f => f.Id == id);

        public async Task<Facility> CreateAsync(Facility facility)
        {
            _context.Facilities.Add(facility);
            await _context.SaveChangesAsync();
            return facility;
        }

        public async Task UpdateAsync(Facility facility)
        {
            _context.Facilities.Update(facility);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility != null)
            {
                _context.Facilities.Remove(facility);
                await _context.SaveChangesAsync();
            }

        }
    }
}
