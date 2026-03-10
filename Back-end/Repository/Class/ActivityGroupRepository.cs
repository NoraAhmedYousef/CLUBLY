using Microsoft.EntityFrameworkCore;
using SignUp.Data;
using SignUp.Model;
using SignUp.Repository.Interfaces;

namespace SignUp.Repository.Class
{
    public class ActivityGroupRepository : IActivityGroupRepository
    {
        private readonly AppDbContext _context;
        public ActivityGroupRepository(AppDbContext context) => _context = context;

        public async Task<List<ActivityGroup>> GetAllAsync()
            => await _context.ActivityGroups
                .Include(g => g.Trainer)
                .Include(g => g.Activity).ThenInclude(a => a.Facility)
                .Include(g => g.TimeSlots)
                .ToListAsync();

        public async Task<ActivityGroup?> GetByIdAsync(int id)
            => await _context.ActivityGroups
                .Include(g => g.Trainer)
                .Include(g => g.Activity).ThenInclude(a => a.Facility)
                .Include(g => g.TimeSlots)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<ActivityGroup> CreateAsync(ActivityGroup group)
        {
            _context.ActivityGroups.Add(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<bool> UpdateAsync(ActivityGroup group)
        {
            if (!await _context.ActivityGroups.AnyAsync(x => x.Id == group.Id)) return false;
            _context.ActivityGroups.Update(group);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var g = await _context.ActivityGroups.FindAsync(id);
            if (g == null) return false;
            _context.ActivityGroups.Remove(g);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ActivityGroup>> GetByActivityIdAsync(int activityId)
            => await _context.ActivityGroups
                .Where(g => g.ActivityId == activityId)
                .Include(g => g.Trainer)
                .Include(g => g.Activity).ThenInclude(a => a.Facility)
                .Include(g => g.TimeSlots)
                .ToListAsync();
    }
}