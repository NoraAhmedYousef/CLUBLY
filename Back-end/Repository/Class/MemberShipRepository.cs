using Microsoft.EntityFrameworkCore;
using SignUp.Data;
using SignUp.Model;
using SignUp.Repository.Interfaces;

namespace SignUp.Repository.Class
{
    public class MemberShipRepository : IMemberShipRepository
    {
        private readonly AppDbContext _context;

        public MemberShipRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MemberShip>> GetAllAsync()
        {
            return await _context.MemberShips.ToListAsync();
        }

        public async Task<MemberShip?> GetByIdAsync(int id)
        {
            return await _context.MemberShips.FindAsync(id);
        }

        public async Task<MemberShip> CreateAsync(MemberShip memberShip)
        {
            _context.MemberShips.Add(memberShip);
            await _context.SaveChangesAsync();
            return memberShip;
        }

        public async Task UpdateAsync(MemberShip memberShip)
        {
            var exists = await _context.MemberShips.AnyAsync(t => t.Id == memberShip.Id);
            if (!exists) return;
            _context.MemberShips.Update(memberShip);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var memberShip = await _context.MemberShips.FindAsync(id);
            if (memberShip == null) return;
            _context.MemberShips.Remove(memberShip);
            await _context.SaveChangesAsync();
        }

    }
}
