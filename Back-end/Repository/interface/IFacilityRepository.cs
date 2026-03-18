using SignUp.Model;

namespace SignUp.Repository.Interfaces
{ 
   public interface IFacilityRepository
    {
        Task<List<Facility>> GetAllAsync();
        Task<Facility?> GetByIdAsync(int id);
        Task<Facility> CreateAsync(Facility facility);
        Task UpdateAsync(Facility facility);
        Task DeleteAsync(int id);
    }
}
