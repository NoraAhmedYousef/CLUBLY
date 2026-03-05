using Clubly.Model;
using Clubly.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using SignUp.Data;

namespace Clubly.Repository.Class
{
    public class FacilityScheduleRepository: IFacilityScheduleRepository
    {
        private readonly AppDbContext _db;
        public FacilityScheduleRepository(AppDbContext db) => _db = db;

        public async Task<List<FacilitySchedule>> GetAllAsync() =>
            await _db.FacilitySchedules
                     .Include(s => s.Facility)
                     .Include(s => s.TimeSlots)
                     .OrderBy(s => s.Date)
                     .ThenBy(s => s.Day)
                     .ToListAsync();

        public async Task<FacilitySchedule?> GetByIdAsync(int id) =>
            await _db.FacilitySchedules
                     .Include(s => s.Facility)
                     .Include(s => s.TimeSlots)
                     .FirstOrDefaultAsync(s => s.Id == id);

        public async Task<FacilitySchedule> CreateAsync(FacilitySchedule schedule)
        {
            _db.FacilitySchedules.Add(schedule);
            await _db.SaveChangesAsync();
            return schedule;
        }

        public async Task UpdateAsync(FacilitySchedule schedule)
        {
            _db.FacilitySchedules.Update(schedule);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var schedule = await _db.FacilitySchedules
                                    .Include(s => s.TimeSlots)
                                    .FirstOrDefaultAsync(s => s.Id == id);
            if (schedule is null) return;
            _db.FacilitySchedules.Remove(schedule);
            await _db.SaveChangesAsync();
        }
    }
}
