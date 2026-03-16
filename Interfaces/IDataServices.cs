using Data.Eikon.DTOs;

namespace EIKON.UI.Interfaces
{
    public interface IDataServices
    {
        Task<List<Trhis000Dto>> GetAll();
        Task<Trhis000Dto> GetById(int id);
        Task<Trhis000Dto> GetByEkCodigo(string IsCodigo);
        Task<bool> Trhis000Save(Trhis000Dto trhis);
       

        Task<bool> DeleteById(int id);
    }
}
