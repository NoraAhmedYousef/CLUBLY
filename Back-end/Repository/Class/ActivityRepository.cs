using Microsoft.EntityFrameworkCore;
using SignUp.Data;
using SignUp.DTO;
using SignUp.Model;
using SignUp.Repository.Interfaces;
namespace SignUp.Repository.Class
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly AppDbContext _context;

        public ActivityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Activity>> GetAllAsync() =>
            await _context.Activities
                          .Include(a => a.Facility)
                          .ToListAsync();

        public async Task<Activity?> GetByIdAsync(int id) =>
            await _context.Activities
                          .Include(a => a.Facility)
                          .FirstOrDefaultAsync(a => a.Id == id);

        public async Task<Activity> CreateAsync(Activity activity)
        {
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task UpdateAsync(Activity activity)
        {
            _context.Activities.Update(activity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity != null)
            {
                _context.Remove(activity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
