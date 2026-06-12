using Clubly.DTO;
using Clubly.Model;
using Clubly.Repository.Interfaces;
using Clubly.Service.Interfaces;
using SignUp.Repository.Interfaces;

namespace Clubly.Service.Class
{
    public class MembershipRenewalRequestService : IMembershipRenewalRequestService
    {
        private readonly IMembershipRenewalRequestRepository _repo;
        private readonly IMemberShipRepository _membershipRepo; // عدّل الاسم لو مختلف عندك
        private readonly IWebHostEnvironment _env;

        public MembershipRenewalRequestService(
            IMembershipRenewalRequestRepository repo,
            IMemberShipRepository membershipRepo,
            IWebHostEnvironment env)
        {
            _repo = repo;
            _membershipRepo = membershipRepo;
            _env = env;
        }

        public async Task<List<MembershipRenewalRequestDto>> GetAllAsync() =>
            (await _repo.GetAllAsync()).Select(Map).ToList();

        public async Task<MembershipRenewalRequestDto?> GetByIdAsync(int id)
        {
            var r = await _repo.GetByIdAsync(id);
            return r is null ? null : Map(r);
        }

        public async Task<List<MembershipRenewalRequestDto>> GetByMemberAsync(int memberId) =>
            (await _repo.GetByMemberAsync(memberId)).Select(Map).ToList();

        public async Task<MembershipRenewalRequestDto> CreateAsync(CreateMembershipRenewalRequestDto dto)
        {
            // هات سعر الخطة المطلوبة من قاعدة البيانات (أأمن من إرسال السعر من العميل)
            var plan = await _membershipRepo.GetByIdAsync(dto.MemberShipId)
                       ?? throw new KeyNotFoundException("Membership plan not found.");

            string receiptUrl = "";
            if (dto.ReceiptImage is not null && dto.ReceiptImage.Length > 0)
                receiptUrl = await SaveReceiptAsync(dto.ReceiptImage);

            var entity = new MembershipRenewalRequest
            {
                MemberId = dto.MemberId,
                MemberShipId = dto.MemberShipId,
                PaymentMethod = dto.PaymentMethod,
                TransactionId = dto.TransactionId,
                Price = plan.Price,
                ReceiptImageUrl = receiptUrl,
                Status = "Pending",
                RequestDate = DateTime.UtcNow
            };

            var created = await _repo.CreateAsync(entity);
            var full = await _repo.GetByIdAsync(created.Id);
            return Map(full!);
        }

        public async Task<bool> UpdateStatusAsync(int id, UpdateMembershipRenewalStatusDto dto) =>
            await _repo.UpdateStatusAsync(id, dto.Status);

        public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);

        // ── Helpers ──────────────────────────────────────
        private async Task<string> SaveReceiptAsync(Microsoft.AspNetCore.Http.IFormFile file)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "receipts");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/receipts/{fileName}";
        }

        private static MembershipRenewalRequestDto Map(MembershipRenewalRequest r) => new()
        {
            Id = r.Id,
            MemberId = r.MemberId,
            MemberName = r.Member?.FullName ?? "",
            MemberShipNumber = r.Member?.MemberShipNumber ?? 0,
            MemberShipId = r.MemberShipId,
            MemberShipName = r.MemberShip?.Name ?? "",
            MemberShipDuration = r.MemberShip?.Duration ?? 0,
            Status = r.Status,
            PaymentMethod = r.PaymentMethod,
            TransactionId = r.TransactionId,
            Price = r.Price,
            ReceiptImageUrl = r.ReceiptImageUrl,
            RequestDate = r.RequestDate,
            ProcessedDate = r.ProcessedDate
        };
    }
}