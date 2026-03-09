using Microsoft.EntityFrameworkCore;
using SignUp.Data;
using SignUp.DTO;
using SignUp.Model;
using SignUp.Repository.Interfaces;
using SignUp.Service.Interfaces;

namespace SignUp.Service.Class
{
    public class ActivityGroupService : IActivityGroupService
    {
        private readonly IActivityGroupRepository _repo;
        private readonly IActivityRepository _activityRepo;

        public ActivityGroupService(IActivityGroupRepository repo, IActivityRepository activityRepo)
        {
            _repo = repo;
            _activityRepo = activityRepo;
        }

        public async Task<List<ActivityGroupDto>> GetAllAsync()
        {
            var groups = await _repo.GetAllAsync();
            return groups.Select(g => new ActivityGroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Capacity = g.Capacity,
                Code = g.Code,
                ActivityId = g.ActivityId,
                ActivityName = g.Activity?.Name,
                FacilityName = g.Activity?.Facility?.Name,
                TrainerId = g.TrainerId,
                TrainerName = g.Trainer?.FullName,
                Duration =g.Duration,
                Day = g.Day,
                StartTime = g.StartTime.ToString(@"hh\:mm"),
                EndTime = g.EndTime.ToString(@"hh\:mm"),
                Status = g.Status
            }).ToList();
        }

        public async Task<ActivityGroupDto?> GetByIdAsync(int id)
        {
            var g = await _repo.GetByIdAsync(id);
            if (g == null) return null;
            return new ActivityGroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Capacity = g.Capacity,
                Code = g.Code,
                ActivityId = g.ActivityId,
                ActivityName = g.Activity?.Name,
                FacilityName = g.Activity?.Facility?.Name,
                TrainerId = g.TrainerId,
                TrainerName = g.Trainer?.FullName,
                Duration = g.Duration,
                Day = g.Day,
                StartTime = g.StartTime.ToString(@"hh\:mm"),
                EndTime = g.EndTime.ToString(@"hh\:mm"),
                Status = g.Status
            };
        }

        public async Task<ActivityGroupDto> CreateAsync(CreateActivityGroupDto dto)
        {
            var group = new ActivityGroup
            {
                Name = dto.Name,
                Capacity = dto.Capacity,
                Code = dto.Code,
                ActivityId = dto.ActivityId,
                TrainerId = dto.TrainerId,
                Duration = dto.Duration,
                Day = dto.Day,
                StartTime = TimeSpan.Parse(dto.StartTime),
                EndTime = TimeSpan.Parse(dto.EndTime),
                Status = dto.Status

            };
            await _repo.CreateAsync(group);
            return await GetByIdAsync(group.Id) ?? throw new Exception("Failed to create group");
        }

        public async Task<bool> UpdateAsync(int id, UpdateActivityGroupDto dto)
        {
            var group = await _repo.GetByIdAsync(id);
            if (group == null) return false;
            if (!string.IsNullOrEmpty(dto.Name)) group.Name = dto.Name;
            if (dto.Capacity.HasValue) group.Capacity = dto.Capacity.Value;
            if (!string.IsNullOrEmpty(dto.Code)) group.Code = dto.Code;
            if (dto.ActivityId.HasValue) group.ActivityId = dto.ActivityId.Value;
            if (dto.TrainerId.HasValue) group.TrainerId = dto.TrainerId.Value;
            if (dto.TrainerId == 0) group.TrainerId = null;
            if (!string.IsNullOrEmpty(dto.Duration)) group.Duration = dto.Duration;
            if (!string.IsNullOrEmpty(dto.Day)) group.Day = dto.Day;
            if (!string.IsNullOrEmpty(dto.StartTime)) group.StartTime = TimeSpan.Parse(dto.StartTime);
            if (!string.IsNullOrEmpty(dto.EndTime)) group.EndTime = TimeSpan.Parse(dto.EndTime);
            if (dto.Status != null) group.Status = dto.Status;

            await _repo.UpdateAsync(group);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
        public async Task<List<ActivityGroupDto>> GetByActivityIdAsync(int activityId)
        {
            var groups = await _repo.GetByActivityIdAsync(activityId);

            return groups.Select(g => new ActivityGroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Capacity = g.Capacity,
                Code = g.Code,
                ActivityId = g.ActivityId,
                ActivityName = g.Activity?.Name,
                FacilityName = g.Activity?.Facility?.Name,
                TrainerId = g.TrainerId,
                TrainerName = g.Trainer?.FullName,
                Duration = g.Duration,
                Day = g.Day,
                StartTime = g.StartTime.ToString(@"hh\:mm"),
                EndTime = g.EndTime.ToString(@"hh\:mm"),
                Status = g.Status,
            }).ToList();
        }


    }
}