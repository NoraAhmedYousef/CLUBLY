using Clubly.DTO;
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

        public async Task UpdateAsync(int id, UpdateFacilityScheduleDto dto)
        {
            var existing = await _db.FacilitySchedules
                                    .Include(s => s.TimeSlots)
                                    .FirstOrDefaultAsync(s => s.Id == id);

            if (existing is null) return;

            if (dto.FacilityId.HasValue) existing.FacilityId = dto.FacilityId.Value;
            if (dto.Day is not null) existing.Day = dto.Day;
            if (dto.Date.HasValue) existing.Date = dto.Date.Value;
            if (dto.Status is not null) existing.Status = dto.Status;

            if (dto.TimeSlots is not null)
            {
                _db.FacilityTimeSlots.RemoveRange(existing.TimeSlots);
                existing.TimeSlots = dto.TimeSlots.Select(t => new FacilityTimeSlot
                {
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    Price = t.Price,        
                    FacilityScheduleId = existing.Id
                }).ToList();
            }

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
