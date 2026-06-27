using SignUp.Model;

namespace SignUp.Repository.Interfaces
{ 
   public interface IMemberShipRepository
    {
    Task<List<MemberShip>> GetAllAsync();
    Task<MemberShip?> GetByIdAsync(int id);
    Task<MemberShip> CreateAsync(MemberShip memberShip);
    Task UpdateAsync(MemberShip memberShip);
    Task DeleteAsync(int id);
}
}
