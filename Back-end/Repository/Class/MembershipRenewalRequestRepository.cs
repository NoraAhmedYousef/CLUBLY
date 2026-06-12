using Clubly.Model;
using Clubly.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using SignUp.Data;

namespace Clubly.Repository.Class
{
    public class MembershipRenewalRequestRepository : IMembershipRenewalRequestRepository
    {
        private readonly AppDbContext _db;
        public MembershipRenewalRequestRepository(AppDbContext db) => _db = db;

        public async Task<List<MembershipRenewalRequest>> GetAllAsync() =>
            await _db.MembershipRenewalRequests
                     .Include(r => r.Member)
                     .Include(r => r.MemberShip)
                     .OrderByDescending(r => r.RequestDate)
                     .ToListAsync();

        public async Task<MembershipRenewalRequest?> GetByIdAsync(int id) =>
            await _db.MembershipRenewalRequests
                     .Include(r => r.Member)
                     .Include(r => r.MemberShip)
                     .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<List<MembershipRenewalRequest>> GetByMemberAsync(int memberId) =>
            await _db.MembershipRenewalRequests
                     .Include(r => r.MemberShip)
                     .Where(r => r.MemberId == memberId)
                     .OrderByDescending(r => r.RequestDate)
                     .ToListAsync();

        public async Task<MembershipRenewalRequest> CreateAsync(MembershipRenewalRequest request)
        {
            _db.MembershipRenewalRequests.Add(request);
            await _db.SaveChangesAsync();
            return request;
        }

        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            var r = await _db.MembershipRenewalRequests
                              .Include(r => r.Member)
                              .FirstOrDefaultAsync(r => r.Id == id);
            if (r is null) return false;

            r.Status = status;
            r.ProcessedDate = DateTime.UtcNow;

            // لو تم القبول: حدّث خطة العضو وتاريخ الانضمام (لإعادة حساب تاريخ الانتهاء)
            if (status == "Approved" && r.Member is not null)
            {
                r.Member.MemberShipId = r.MemberShipId;
                r.Member.JoinDate = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync(int id)
        {
            var r = await _db.MembershipRenewalRequests.FindAsync(id);
            if (r is null) return;
            _db.MembershipRenewalRequests.Remove(r);
            await _db.SaveChangesAsync();
        }
    }
}