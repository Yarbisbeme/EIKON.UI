using Data.Eikon.DTOs;

using EIKON.UI.Interfaces;
using EIKON.UI.Repositories;
namespace EIKON.UI.Services
{
    public class DataServices : IDataServices
    {
        private  IEikonDataRepository _trhis000Repository;
        public DataServices(IEikonDataRepository trhis000Repository)
        {
            trhis000Repository=_trhis000Repository;
        }
        Task<bool> IDataServices.DeleteById(int id)
        {
         var ret=   _trhis000Repository.DeleteById(id);
            return ret; 
            //throw new NotImplementedException();
        }

        Task<List<Trhis000Dto>> IDataServices.GetAll()
        {
          return  _trhis000Repository.GetAll();
            //throw new NotImplementedException();
        }

        Task<Trhis000Dto> IDataServices.GetByEkCodigo(string IsCodigo)
        {
            return _trhis000Repository.GetByEkCodigo(IsCodigo);
            //throw new NotImplementedException();
        }

        Task<Trhis000Dto> IDataServices.GetById(int id)
        {
            return _trhis000Repository.GetById(id);
            //throw new NotImplementedException();
        }

        Task<bool> IDataServices.Trhis000Save(Trhis000Dto trhis)
        {
            if(trhis.IdentityColumn==0)
            return    _trhis000Repository.Insert(trhis);
            else
            return _trhis000Repository.Update(trhis);
            //throw new NotImplementedException();
        }
    }
}

