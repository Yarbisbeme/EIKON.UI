using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using EIKON.Data.Models;
//using EIKON.Data.DTOs;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace EIKON.UI.Interfaces
{
    public interface IEikonDataRepository : IEikonApiService
    {
        // Trhis000 ini
        Task<List<Trhis000Dto>> GetAll();   
        Task<Trhis000Dto> GetById(int id);
        Task<Trhis000Dto> GetByEkCodigo(string IsCodigo);
        Task<bool> Insert(Trhis000Dto trhis);
        Task<bool> Update(Trhis000Dto trhis);

        Task<bool> DeleteById(int id);
        // Trhis000 fin
        // Trhpa000 ini
        Task<List<Trhpa000Dto>> paGetAll();
        Task<Trhpa000Dto> paGetById(int id);
        Task<Trhpa000Dto> paGetByEkCodigo(string IsCodigo);
        Task<bool> paInsert(Trhpa000Dto trhis);
        Task<bool> paUpdate(Trhpa000Dto trhis);

        Task<bool> paDeleteById(int id);
        // Trhpa000 fin

        // Trhnz000 ini
        Task<List<Trhnz000Dto>> nzGetAll();
        Task<Trhnz000Dto> nzGetById(int id);
        Task<Trhnz000Dto> nzGetByEkCodigo(string NzCodigo);
        Task<bool> nzInsert(Trhnz000Dto trhnz);
        Task<bool> nzUpdate(Trhnz000Dto trhnz);

        Task<bool> nzDeleteById(int id);
        // Trhnz000 fin

        // Trhps000 ini
        Task<List<Trhps000Dto>> psGetAll(string token);
        Task<Trhps000Dto> psGetById(int id);
        Task<bool> psInsert(Trhps000Dto trhps);
        Task<bool> psUpdate(Trhps000Dto trhps);
        Task<bool> psDeleteById(int id);
        // Trhps000 fin

        // Frhue000 ini
        Task<List<Frhue000Dto>> ueGetAll();
        Task<Frhue000Dto> ueGetById(int id);
        Task<List<Frhue000Dto>> ueGetFrhue000sByUserId(string PasUserId);
        Task<bool> ueInsert(Frhue000Dto Frhue);
        Task<bool> ueUpdate(Frhue000Dto Frhue);

        Task<bool> ueDeleteById(int id);
        // Frhue000 fin

        // Trhfm000 ini
        Task<List<Trhfm000Dto>> fmGetAll();
        Task<Trhfm000Dto> fmGetById(int id);
        Task<Trhfm000Dto> fmGetTrhfm000sByPaIs(string IsCodigo);
        Task<bool> fmInsert(Trhfm000Dto Trhfm);
        Task<bool> fmUpdate(Trhfm000Dto Trhfm);

        Task<bool> fmDeleteById(int id);
        // Trhfm000 fin

        // Frhrq010 ini
        Task<List<Frhrq010Dto>> rqGetAll(string token);
        Task<Frhrq010Dto> rqGetById(int id, string token);
        Task<Frhrq010Dto> rqFrhrq010ByReqnume(string reqnume, string token);
        Task<bool> rqInsert(Frhrq010Dto Frhrq, string token);
        Task<bool> rqUpdate(Frhrq010Dto Frhrq, string token);
        Task<bool> rqDeleteById(int id, string token);
        // Frhrq010 fin

        // Frhpu010 ini
        //Task<List<Frhpu010Dto>> puGetAll(string token);
        //Task<Frhpu010Dto> puGetById(int id, string token);
        //Task<Frhpu010Dto> puFrhpu010ByReqnume(string reqnume, string token);
        //Task<bool> puInsert(Frhpu010Dto Frhpu, string token);
        //Task<bool> puUpdate(Frhpu010Dto Frhpu, string token);
        //Task<bool> puDeleteById(int id, string token);

        Task<List<Frhpu010Dto>> puGetAllAsync(string token);
        Task<Frhpu010Dto> puGetByReqnumeAsync(string reqnume, string token);
        Task<Frhpu010Dto> puGetByIdAsync(int id, string token);
        Task<Frhpu010Dto> puGetByJoNumberAsync(string jonumber, string token);
        Task<Frhpu010Dto> puGetByPuenumeAsync(string puenume, string token);
        Task<IEnumerable<SelectListItem>> puGetSelectListAsync(string token);
        Task<bool> puAddAsync(Frhpu010Dto dto, string token);
        Task<bool> puUpdateAsync(int id, Frhpu010Dto dto, string token);
        Task<bool> puDeleteAsync(int id, string token);
        // Frhpu010 fin

        //frhjo010 inicio

        Task<List<Frhjo010Dto>> joGetAllAsync(string token);
        Task<Frhjo010Dto> joGetByCodigoAsync(string codigo, string token);
        Task<List<Frhjo010Dto>> joGetByDescriAsync(string descri, string token);
        Task<bool> joAddAsync(Frhjo010Dto entity, string token);
        Task<bool> joUpdateAsync(Frhjo010Dto entity, string token);
        Task<bool> joDeleteAsync(int id, string token);
        Task<bool> joExistsAsync(string codigo, string token);

        //frhjo010 fin

        //Trhof010 inicio
        
        Task<List<Trhof010Dto>> ofGetAllAsync(string token);
        Task<Trhof010Dto> ofGetByCodigoAsync(string codigo, string token);
        Task<List<Trhof010Dto>> ofGetByDescriAsync(string emcodigo, string token);
        Task<bool> ofAddAsync(Trhof010Dto entity, string token);
        Task<bool> ofUpdateAsync(Trhof010Dto entity, string token);
        Task<bool> ofDeleteAsync(int id, string token);


        //Trhof010 fin

        //Trhub010 inicio

        Task<List<Trhub010Dto>> ubGetAllAsync(string token);
        Task<Trhub010Dto> ubGetByCodigoAsync(string codigo, string token);
        Task<List<Trhub010Dto>> ubGetByDescriAsync(string emcodigo, string token);
        Task<bool> ubAddAsync(Trhub010Dto entity, string token);
        Task<bool> ubUpdateAsync(Trhub010Dto entity, string token);
        Task<bool> ubDeleteAsync(int id, string token);


        //Trhub010 fin

        //Trhnp010 inicio

        Task<List<Trhnp010Dto>> npGetAllAsync(string token);
        Task<Trhnp010Dto> npGetByCodigoAsync(string codigo, string token);
        Task<List<Trhnp010Dto>> npGetByEmCodigoAsync(string emcodigo, string token);
        Task<bool> npAddAsync(Trhnp010Dto entity, string token);
        Task<bool> npUpdateAsync(Trhnp010Dto entity, string token);
        Task<bool> npDeleteAsync(int id, string token);


        //Trhnp010 fin

        //Trhct010 inicio

        Task<List<Trhct010Dto>> ctGetAllAsync(string token);
        Task<Trhct010Dto> ctGetByCodigoAsync(string codigo, string token);
        Task<List<Trhct010Dto>> ctGetByEmCodigoAsync(string emcodigo, string token);
        Task<bool> ctAddAsync(Trhct010Dto entity, string token);
        Task<bool> ctUpdateAsync(Trhct010Dto entity, string token);
        Task<bool> ctDeleteAsync(int id, string token);


        //Trhct010 fin

        //Trhgr010 inicio

        Task<List<Trhgr010Dto>> grGetAllAsync(string token);
        Task<Trhgr010Dto> grGetByCodigoAsync(string codigo, string token);
        Task<List<Trhgr010Dto>> grGetByEmCodigoAsync(string emcodigo, string token);
        Task<bool> grAddAsync(Trhgr010Dto entity, string token);
        Task<bool> grUpdateAsync(Trhgr010Dto entity, string token);
        Task<bool> grDeleteAsync(int id, string token);


        //Trhgr010 fin

        //Trhcc010 inicio

        Task<List<Trhcc010Dto>> ccGetAllAsync(string token);
        Task<Trhcc010Dto> ccGetByCodigoAsync(string codigo, string token);
        Task<List<Trhcc010Dto>> ccGetByEmCodigoAsync(string emcodigo, string token);
        Task<bool> ccAddAsync(Trhcc010Dto entity, string token);
        Task<bool> ccUpdateAsync(Trhcc010Dto entity, string token);
        Task<bool> ccDeleteAsync(int id, string token);
        //Trhcc010 fin

        //Trhrw000 inicio

        Task<List<Trhrw000Dto>> rwGetAllAsync(string token);
        Task<Trhrw000Dto> rwGetByCodigoAsync(string codigo, string token);
        Task<List<Trhrw000Dto>> rwGetByEmCodigoAsync(string emcodigo, string token);
        Task<bool> rwAddAsync(Trhrw000Dto entity, string token);
        Task<bool> rwUpdateAsync(Trhrw000Dto entity, string token);
        Task<bool> rwDeleteAsync(int id, string token);


        //Trhrw000 fin

        //Trhpr000 inicio

        Task<List<Trhpr000Dto>> prGetAllAsync(string token);
        Task<Trhpr000Dto> prGetByCodigoAsync(string codigo, string token);
        
        Task<bool> prAddAsync(Trhpr000Dto entity, string token);
        Task<bool> prUpdateAsync(Trhpr000Dto entity, string token);
        Task<bool> prDeleteAsync(int id, string token);


        //Trhpr000 fin


        Task<SendEmailRequest> sendEmailRequest(SendEmailRequest sendEmailRequest);

            Task<List<Frhwd010>> PayrollProcessAsync(
                IEnumerable<CsrPreGenDto> transacciones,
                string emCodigo = "00",
                string ofCodigo = "000",
                string ubCodigo = "00000",
                string tipoEmpl = "F",
                string wbCodigo = "00001",
                int wgAnoRef = 0,
                int wgSecuen = 0,
                string maenomi="00000000",
                string token = ""
            );
        

    }
}
