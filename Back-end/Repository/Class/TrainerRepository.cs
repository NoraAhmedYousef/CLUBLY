using Microsoft.EntityFrameworkCore;
using SignUp.Data;
using SignUp.Model;
using SignUp.Repository.Interfaces;

namespace SignUp.Repository.Class
{
    public class TrainerRepository : ITrainerRepository
    {
        private readonly AppDbContext _context;
        public TrainerRepository(AppDbContext context) => _context = context;

        public async Task<List<Trainer>> GetAllAsync() =>
            await _context.Trainers.ToListAsync();

        public async Task<Trainer?> GetByIdAsync(int id) =>
            await _context.Trainers.FindAsync(id);

        public async Task<Trainer> CreateAsync(Trainer trainer)
        {
            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();
            return trainer;
        }

        public async Task UpdateAsync(Trainer trainer)
        {
            _context.Trainers.Update(trainer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Trainer trainer)
        {
            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Trainer>> GetByActivityIdAsync(int activityId)
        {
            return await _context.Trainers
                .Include(t => t.Activities)
                .Where(t => t.Activities.Any(a => a.Id == activityId))
                .ToListAsync();
        }
    }
}
