using Microsoft.EntityFrameworkCore;
using SignUp.Data;
using SignUp.Model;
using SignUp.Repository.Interfaces;

namespace SignUp.Repository.Class
{
    public class FacilityCategoryRepository : IFacilityCategoryRepository
    {
        private readonly AppDbContext _context;
        public FacilityCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<FacilityCategory>> GetAllAsync() => await _context.FacilityCategories.ToListAsync();

        public async Task<FacilityCategory?> GetByIdAsync(int id) => await _context.FacilityCategories.FindAsync(id);

        public async Task<FacilityCategory> CreateAsync(FacilityCategory entity)
        {
            _context.FacilityCategories.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(int id, FacilityCategory entity)
        {
            var existing = await _context.FacilityCategories.FindAsync(id);
            if (existing == null) return false;

            existing.Name = entity.Name;
            existing.Description = entity.Description;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.FacilityCategories.FindAsync(id);
            if (existing == null) return false;

            _context.FacilityCategories.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
