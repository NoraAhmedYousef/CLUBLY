using Clubly.DTO;
using Clubly.Model;
using Clubly.Repository.Interfaces;
using Clubly.Service.Interfaces;

namespace Clubly.Service.Class
{
    public class FacilityScheduleService : IFacilityScheduleService
    {
        private readonly IFacilityScheduleRepository _repo;
        public FacilityScheduleService(IFacilityScheduleRepository repo) => _repo = repo;

        private static FacilityScheduleDto ToDto(FacilitySchedule s) => new()
        {
            Id = s.Id,
            FacilityId = s.FacilityId,
            FacilityName = s.Facility?.Name ?? "",
            Day = s.Day,
            Date = s.Date,
            Status = s.Status,
            TimeSlots = s.TimeSlots
                            .OrderBy(t => t.StartTime)
                            .Select(t => new TimeSlotDto
                            {
                                Id = t.Id,
                                StartTime = t.StartTime,
                                EndTime = t.EndTime,
                                Price = t.Price
                            }).ToList()
        };

        public async Task<List<FacilityScheduleDto>> GetAllAsync() =>
            (await _repo.GetAllAsync()).Select(ToDto).ToList();

        public async Task<FacilityScheduleDto?> GetByIdAsync(int id)
        {
            var s = await _repo.GetByIdAsync(id);
            return s is null ? null : ToDto(s);
        }

        public async Task<FacilityScheduleDto> CreateAsync(CreateFacilityScheduleDto dto)
        {
            var schedule = new FacilitySchedule
            {
                FacilityId = dto.FacilityId,
                Day = dto.Day,
                Date = dto.Date,
                Status = dto.Status,
                TimeSlots = dto.TimeSlots.Select(t => new FacilityTimeSlot
                {
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    Price = t.Price
                }).ToList()
            };

            var created = await _repo.CreateAsync(schedule);
            var result = await _repo.GetByIdAsync(created.Id);
            return ToDto(result!);
        }

        public async Task<bool> UpdateAsync(int id, UpdateFacilityScheduleDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing is null) return false;

            await _repo.UpdateAsync(id, dto);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (await _repo.GetByIdAsync(id) is null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }

    }
}
