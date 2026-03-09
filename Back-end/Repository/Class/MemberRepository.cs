using Microsoft.EntityFrameworkCore;
using SignUp.Data;
using SignUp.Model;
using SignUp.Repository.Interfaces;


namespace SignUp.Repository.Classes
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDbContext _db;

        public MemberRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Member>> GetAllAsync()
        {
            return await _db.Members
                .Include(m => m.MemberShip)
                .ToListAsync();
        }

        public async Task<Member?> GetByIdAsync(int id)
        {
            return await _db.Members
                .Include(m => m.MemberShip)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Member member)
        {
            _db.Members.Add(member);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Member member)
        {
            _db.Members.Update(member);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Member member)
        {
            _db.Members.Remove(member);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Members.AnyAsync(m => m.Id == id);
        }
    }
}