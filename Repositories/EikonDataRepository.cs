//using EIKON.Data.Models;
//using EIKON.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;

using System.Net.Http.Headers;
using EIKON.UI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using EIKON.UI.Services;
using Microsoft.IdentityModel.Tokens;
using DevExpress.Office.Utils;
using DevExpress.CodeParser;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
namespace EIKON.UI.Repositories
{
    public class EikonDataRepository : IEikonDataRepository
        {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationService _authenticationService;
       // private EikonDataServices _eikonDataService;
        private string ApiBaseUrl = "";
        //EikonDataServices _services = new();
        private readonly string _apiBaseUrl;
       // private readonly AppApi _appSettings;
        
        public EikonDataRepository(HttpClient httpClient, AuthenticationService authenticationService, IConfiguration config)
        {
            _httpClient = httpClient;
            _authenticationService = authenticationService;
            //getToken();
            // _services.h ;
            //ApiBaseUrl =_services.GetApiBaseUrl();


            _apiBaseUrl = config.GetSection("AppApi").GetSection("ApiBaseUrl").Value;
           // _appSettings = appSettings.Value;
           // _apiBaseUrl = appSettings.Value.ApiBaseUrl;


        }
        //public async void OnAfterRenderAsync()
        //{
        //    var token = await _authenticationService.GetTokenAsync();
        //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.ToString());
        //}
        public  string getToken()
        {
           // EikonDataServices eds = new EikonDataServices(_httpClient,_htt);

            var token =  _authenticationService.tkn;
            if (string.IsNullOrEmpty(token))
            {
                token= null;
            }
            return token;
        }
        //trhis000
        async Task<bool> IEikonDataRepository.DeleteById(int id)
        {
           // ApiBaseUrl=_services.GetApiBaseUrl();   
            HttpResponseMessage response = await _httpClient.DeleteAsync(_apiBaseUrl + "Trhis000/" + id.ToString());

            if (response.IsSuccessStatusCode)
            { return true;  }
            else
            { return false; }
        }

        async Task<List<Trhis000Dto>>  IEikonDataRepository.GetAll()
        {
            
           return await _httpClient.GetFromJsonAsync<List<Trhis000Dto>>(_apiBaseUrl + "Trhis000");
            //throw new NotImplementedException();
        }

        async Task<Trhis000Dto> IEikonDataRepository.GetByEkCodigo(string IsCodigo)
        {
            return await _httpClient.GetFromJsonAsync<Trhis000Dto>(_apiBaseUrl + "Trhis000/" + IsCodigo);

            //throw new NotImplementedException();
        }

        async Task<Trhis000Dto> IEikonDataRepository.GetById(int id)
        {
            return await _httpClient.GetFromJsonAsync<Trhis000Dto>(_apiBaseUrl + "Trhis000/" + id.ToString());
            //throw new NotImplementedException();
        }

        async Task<bool> IEikonDataRepository.Insert(Trhis000Dto trhis)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Trhis000Dto>(_apiBaseUrl + "Trhis000/", trhis);
            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }  
            //throw new NotImplementedException();
        }

        async Task<bool> IEikonDataRepository.Update(Trhis000Dto trhis)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_apiBaseUrl + "Trhis000/" + trhis.IsCodigo, trhis);
            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }

        }

        // trhpa000
        async Task<bool> IEikonDataRepository.paDeleteById(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_apiBaseUrl + "Trhpa000/" + id.ToString());

            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }
        }

        async Task<List<Trhpa000Dto>> IEikonDataRepository.paGetAll()
        {
            return await _httpClient.GetFromJsonAsync<List<Trhpa000Dto>>(_apiBaseUrl + "Trhpa000");
        }

        async Task<Trhpa000Dto> IEikonDataRepository.paGetByEkCodigo(string PaCodigo)
        {
            return await _httpClient.GetFromJsonAsync<Trhpa000Dto>(_apiBaseUrl + "Trhpa000/" + PaCodigo);
           // throw new NotImplementedException();
        }

        async  Task<Trhpa000Dto> IEikonDataRepository.paGetById(int id)
        {
            return await _httpClient.GetFromJsonAsync<Trhpa000Dto>(_apiBaseUrl + "Trhpa000/" + id.ToString());
        }

        async Task<bool> IEikonDataRepository.paInsert(Trhpa000Dto trhpa)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Trhpa000Dto>(_apiBaseUrl + "Trhpa000/", trhpa);
            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }
        }

       async  Task<bool> IEikonDataRepository.paUpdate(Trhpa000Dto trhpa)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_apiBaseUrl + "Trhpa000/" + trhpa.PaCodigo, trhpa);
            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }
        }

        // trhnz000
        async Task<bool> IEikonDataRepository.nzDeleteById(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_apiBaseUrl + "Trhnz000/" + id.ToString());

            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }
        }

        async Task<List<Trhnz000Dto>> IEikonDataRepository.nzGetAll()
        {
            return await _httpClient.GetFromJsonAsync<List<Trhnz000Dto>>(_apiBaseUrl + "Trhnz000");
        }

        async Task<Trhnz000Dto> IEikonDataRepository.nzGetByEkCodigo(string nzCodigo)
        {
            return await _httpClient.GetFromJsonAsync<Trhnz000Dto>(_apiBaseUrl + "Trhnz000/" + nzCodigo);
            // throw new NotImplementedException();
        }

        async Task<Trhnz000Dto> IEikonDataRepository.nzGetById(int id)
        {
            return await _httpClient.GetFromJsonAsync<Trhnz000Dto>(_apiBaseUrl + "Trhnz000/" + id.ToString());
        }

        async Task<bool> IEikonDataRepository.nzInsert(Trhnz000Dto trhnz)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Trhnz000Dto>(_apiBaseUrl + "Trhnz000/", trhnz);
            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }
        }

        async Task<bool> IEikonDataRepository.nzUpdate(Trhnz000Dto trhnz)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_apiBaseUrl + "Trhnz000/" + trhnz.NzCodigo, trhnz);
            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }
        }

        // Trhps000 ini
       public async Task<List<Trhps000Dto>> psGetAll(string token)
        {
            //var token =  _authenticationService.tkn;
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            return await _httpClient.GetFromJsonAsync<List<Trhps000Dto>>(_apiBaseUrl + "Trhps000");
            }
            List <Trhps000Dto>  lista = new List<Trhps000Dto>();    
            return lista;
            
        }
        public async Task<Trhps000Dto> psGetById(int id) {
            var token = _authenticationService.tkn;
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return await _httpClient.GetFromJsonAsync<Trhps000Dto>(_apiBaseUrl + "Trhps000/" + id.ToString()); }
        public async Task<bool> psInsert(Trhps000Dto trhps) {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Trhps000Dto>(_apiBaseUrl + "Trhps000/", trhps);
            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }
        }
        public async Task<bool> psUpdate(Trhps000Dto trhps)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_apiBaseUrl + "Trhps000/" + trhps.IdentityColumn, trhps);
            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }
        }
        public async Task<bool> psDeleteById(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_apiBaseUrl + "Trhps000/" + id.ToString());

            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }
        }
        // Trhps000 fin

        // Frhue000 ini
        public async Task<List<Frhue000Dto>> ueGetAll()
        {
            return await _httpClient.GetFromJsonAsync<List<Frhue000Dto>>(_apiBaseUrl + "Frhue000");
        }
        public async Task<Frhue000Dto> ueGetById(int id) 
        {
            return await _httpClient.GetFromJsonAsync<Frhue000Dto>(_apiBaseUrl + "Frhue000/" + id.ToString());
        }
        public async Task<List<Frhue000Dto>> ueGetFrhue000sByUserId(string PasUserId) 
        { 
            return await _httpClient.GetFromJsonAsync<List<Frhue000Dto>>(_apiBaseUrl + "Frhue000/GetFrhue000sByUserId/" + PasUserId); }
        public async Task<bool> ueInsert(Frhue000Dto Frhue) {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Frhue000Dto>(_apiBaseUrl + "Frhue000/", Frhue);
            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }
        }
        public async Task<bool> ueUpdate(Frhue000Dto Frhue)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_apiBaseUrl + "Frhue000/" + Frhue.IdentityColumn, Frhue);
            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }
        }

        public  async Task<bool> ueDeleteById(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_apiBaseUrl + "Frhue000/" + id.ToString());

            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }
        }
        // Frhue000 fin

        // Trhfm000 ini
        public async Task<List<Trhfm000Dto>> fmGetAll()
        {
            return await _httpClient.GetFromJsonAsync<List<Trhfm000Dto>>(_apiBaseUrl + "Trhfm000");
        }
        public async Task<Trhfm000Dto> fmGetById(int id)
        {
            return await _httpClient.GetFromJsonAsync<Trhfm000Dto>(_apiBaseUrl + "Trhfm000/" + id.ToString());
        }
        public async Task<Trhfm000Dto> fmGetTrhfm000sByPaIs(string IsCodigo)
        {
            return await _httpClient.GetFromJsonAsync<Trhfm000Dto>(_apiBaseUrl + "Trhfm000/PorIsCodigo/" + IsCodigo);
        }
        public async Task<bool> fmInsert(Trhfm000Dto Trhfm)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Trhfm000Dto>(_apiBaseUrl + "Trhfm000/", Trhfm);
            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }
        }
        public async Task<bool> fmUpdate(Trhfm000Dto Trhfm)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_apiBaseUrl + "Trhfm000/" + Trhfm.IdentityColumn, Trhfm);
            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }
        }

        public async Task<bool> fmDeleteById(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_apiBaseUrl + "Trhfm000/" + id.ToString());

            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }
        }
        // Trhfm000 fin

        // Frhrq010 ini
        public async Task<List<Frhrq010Dto>> rqGetAll(string token)
        {
            //var token =  _authenticationService.tkn;
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _httpClient.GetFromJsonAsync<List<Frhrq010Dto>>(_apiBaseUrl + "Frhrq010");
            }
            List<Frhrq010Dto> lista = new List<Frhrq010Dto>();
            return lista;
        }
        public async Task<Frhrq010Dto> rqGetById(int id, string token)
        {
           // var token = _authenticationService.tkn;
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<Frhrq010Dto>(_apiBaseUrl + "Frhrq010/" + id.ToString());
            }
            Frhrq010Dto frhrq010Dto = new Frhrq010Dto();
            return frhrq010Dto;
        }

        public async Task<Frhrq010Dto> rqFrhrq010ByReqnume(string reqnume, string token)
        {
            //var token = _authenticationService.tkn;
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<Frhrq010Dto>(_apiBaseUrl + "Frhrq010ByReqnume/" + reqnume);

            }
            Frhrq010Dto frhrq010Dto = new Frhrq010Dto();
            return frhrq010Dto;
        }
        public async Task<bool> rqInsert(Frhrq010Dto Frhrq, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Frhrq010Dto>(_apiBaseUrl + "Frhrq010/", Frhrq);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }
        public async Task<bool> rqUpdate(Frhrq010Dto Frhrq, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_apiBaseUrl + "Frhrq010/" + Frhrq.IdentityColumn, Frhrq);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }
        public async Task<bool> rqDeleteById(int id, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.DeleteAsync(_apiBaseUrl + "Frhrq010/" + id.ToString());

            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }
            }
            return false;
        }
        // Frhrq010 fin

        async Task<SendEmailRequest> IEikonDataRepository.sendEmailRequest(SendEmailRequest sendEmailRequest)
        {
            
            //http://localhost:8081/api/Authentication/SendMail
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<SendEmailRequest>(_apiBaseUrl + "Authentication/SendMail/", sendEmailRequest);
            if (response.IsSuccessStatusCode)
            { return sendEmailRequest; }
            else
            { return null; }
        }

        async Task<List<Frhpu010Dto>> IEikonDataRepository.puGetAllAsync(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _httpClient.GetFromJsonAsync<List<Frhpu010Dto>>(_apiBaseUrl + "Frhpu010");
            }
            List<Frhpu010Dto> lista = new List<Frhpu010Dto>();
            return lista;
           // return await _httpClient.GetFromJsonAsync<List<Frhpu010Dto>>(_apiBaseUrl + "Frhpu010");
        }

       async Task<Frhpu010Dto> IEikonDataRepository.puGetByReqnumeAsync(string reqnume, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<Frhpu010Dto>(_apiBaseUrl + "Frhpu010ByReqnume/" + reqnume);

            }
            Frhpu010Dto frhpu010Dto = new Frhpu010Dto();
            return frhpu010Dto;
        }

       async Task<Frhpu010Dto> IEikonDataRepository.puGetByIdAsync(int id, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<Frhpu010Dto>(_apiBaseUrl + "Frhpu010/Frhpu010ById/" + id.ToString());
            }
            Frhpu010Dto frhpu010Dto = new Frhpu010Dto();
            return frhpu010Dto;
        }

       async Task<Frhpu010Dto> IEikonDataRepository.puGetByJoNumberAsync(string jonumber, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<Frhpu010Dto>(_apiBaseUrl + "Frhpu010ByJoNumber/" + jonumber);

            }
            Frhpu010Dto frhpu010Dto = new Frhpu010Dto();
            return frhpu010Dto;

        }

        async Task<Frhpu010Dto> IEikonDataRepository.puGetByPuenumeAsync(string puenume, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<Frhpu010Dto>(_apiBaseUrl + "Frhpu010ByPuenume/" + puenume);

            }
            Frhpu010Dto frhpu010Dto = new Frhpu010Dto();
            return frhpu010Dto;
        }

       async Task<IEnumerable<Interfaces.SelectListItem>> IEikonDataRepository.puGetSelectListAsync(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<IEnumerable<Interfaces.SelectListItem>>(_apiBaseUrl + "Frhpu010/SelectList");

            }
            IEnumerable<Interfaces.SelectListItem> lista = null;
            return lista;
        }

       async Task<bool> IEikonDataRepository.puAddAsync(Frhpu010Dto dto, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Frhpu010Dto>(_apiBaseUrl + "Frhpu010/", dto);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        async Task<bool> IEikonDataRepository.puUpdateAsync(int id, Frhpu010Dto dto, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_apiBaseUrl + "Frhpu010/" + dto.IdentityColumn, dto);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        async Task<bool> IEikonDataRepository.puDeleteAsync(int id, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.DeleteAsync(_apiBaseUrl + "Frhpu010/" + id.ToString());

                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        //joRepo Ini
        public async Task<List<Frhjo010Dto>> joGetAllAsync(string token)
        {
          //  return await _context.Frhjo010s.ToListAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _httpClient.GetFromJsonAsync<List<Frhjo010Dto>>(_apiBaseUrl + "Frhjo010");
            }
            List<Frhjo010Dto> lista = new List<Frhjo010Dto>();
            return lista;
        }

        public async Task<Frhjo010Dto> joGetByCodigoAsync(string codigo, string token)
        {
            // return await _context.Frhjo010s.FirstOrDefaultAsync(x => x.JoNumber.Contains(codigo));
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<Frhjo010Dto>(_apiBaseUrl + "Frhjo010/Frhjo010ByCodigo/" + codigo);

            }
            Frhjo010Dto frhjo010Dto = new Frhjo010Dto();
            return frhjo010Dto;
        }

        public async Task<List<Frhjo010Dto>> joGetByDescriAsync(string descri, string token)
        {
           // return await _context.Frhjo010s.Where(rg => rg.JoTitulo.Contains(descri))
           //                                .OrderBy(o => o.JoTitulo)
             //                              .ToListAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _httpClient.GetFromJsonAsync<List<Frhjo010Dto>>(_apiBaseUrl + "Frhjo010/" + descri);
            }
            List<Frhjo010Dto> lista = new List<Frhjo010Dto>();
            return lista;
        }

         async Task<bool> IEikonDataRepository.joAddAsync(Frhjo010Dto entity, string token)
        {
            //await _context.Frhjo010s.AddAsync(entity);
            //await _context.SaveChangesAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Frhjo010Dto>(_apiBaseUrl + "Frhjo010/", entity);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        public async Task<bool> joUpdateAsync(Frhjo010Dto dto, string token)
        {
            // _context.Entry(entity).State = EntityState.Modified;
            // await _context.SaveChangesAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_apiBaseUrl + "Frhjo010/" + dto.IdentityColumn, dto);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        public async Task<bool> joDeleteAsync(int id, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.DeleteAsync(_apiBaseUrl + "Frhjo010/" + id.ToString());

                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
            //var entity = await _context.Frhjo010s.FindAsync(id);
            //if (entity != null)
            //{
            //    _context.Frhjo010s.Remove(entity);
            //    await _context.SaveChangesAsync();
            //}
        }

        public async Task<bool> joExistsAsync(string codigo, string token)
        {
            //return await _context.Frhjo010s.AnyAsync(e => e.JoNumber == codigo);
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Frhjo010Dto frhjo010Dto = new Frhjo010Dto();
                frhjo010Dto = await _httpClient.GetFromJsonAsync<Frhjo010Dto>(_apiBaseUrl + "Frhjo010ByCodigo/" + codigo);
               // return await 
                if (frhjo010Dto != null)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        /**
         // GET: api/Trhof010
        
         
         **/
       async Task<List<Trhof010Dto>> IEikonDataRepository.ofGetAllAsync(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _httpClient.GetFromJsonAsync<List<Trhof010Dto>>(_apiBaseUrl + "Trhof010");
            }
            List<Trhof010Dto> lista = new List<Trhof010Dto>();
            return lista;
        }

       async Task<Trhof010Dto> IEikonDataRepository.ofGetByCodigoAsync(string codigo, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<Trhof010Dto>(_apiBaseUrl + "Trhof010/PorCodigo/" + codigo);

            }
            Trhof010Dto trhof010Dto = new Trhof010Dto();
            return trhof010Dto;
        }

       async Task<List<Trhof010Dto>> IEikonDataRepository.ofGetByDescriAsync(string emcodigo, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _httpClient.GetFromJsonAsync<List<Trhof010Dto>>(_apiBaseUrl + "Trhof010/ByEmCodigo/"+emcodigo);
            }
            List<Trhof010Dto> lista = new List<Trhof010Dto>();
            return lista;
        }

       async  Task<bool> IEikonDataRepository.ofAddAsync(Trhof010Dto entity, string token)
        {
            if (!string.IsNullOrEmpty(token))
            { //https://localhost:8082/api/Trhof010
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Trhof010Dto>(_apiBaseUrl + "Trhof010/", entity);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        async Task<bool> IEikonDataRepository.ofUpdateAsync(Trhof010Dto dto, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_apiBaseUrl + "Trhof010/" + dto.OfCodigo, dto);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        async Task<bool> IEikonDataRepository.ofDeleteAsync(int id, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.DeleteAsync(_apiBaseUrl + "Trhof010/" + id.ToString());

                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;

        }



        // Trhub010 inicio
        async Task<List<Trhub010Dto>> IEikonDataRepository.ubGetAllAsync(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _httpClient.GetFromJsonAsync<List<Trhub010Dto>>(_apiBaseUrl + "Trhub010");
            }
            List<Trhub010Dto> lista = new List<Trhub010Dto>();
            return lista;
        }

        async Task<Trhub010Dto> IEikonDataRepository.ubGetByCodigoAsync(string codigo, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<Trhub010Dto>(_apiBaseUrl + "Trhub010/PorCodigo/" + codigo);

            }
            Trhub010Dto trhub010Dto = new Trhub010Dto();
            return trhub010Dto;
        }

        async Task<List<Trhub010Dto>> IEikonDataRepository.ubGetByDescriAsync(string emcodigo, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _httpClient.GetFromJsonAsync<List<Trhub010Dto>>(_apiBaseUrl + "Trhub010/ByEmCodigo/" + emcodigo);
            }
            List<Trhub010Dto> lista = new List<Trhub010Dto>();
            return lista;
        }

        async Task<bool> IEikonDataRepository.ubAddAsync(Trhub010Dto entity, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Trhub010Dto>(_apiBaseUrl + "Trhub010/", entity);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        async Task<bool> IEikonDataRepository.ubUpdateAsync(Trhub010Dto dto, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_apiBaseUrl + "Trhub010/" + dto.UbCodigo, dto);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        async Task<bool> IEikonDataRepository.ubDeleteAsync(int id, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.DeleteAsync(_apiBaseUrl + "Trhub010/" + id.ToString());

                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;

        }

        // Trhnp010 inicio

       async Task<List<Trhnp010Dto>> IEikonDataRepository.npGetAllAsync(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _httpClient.GetFromJsonAsync<List<Trhnp010Dto>>(_apiBaseUrl + "Trhnp010");
            }
            List<Trhnp010Dto> lista = new List<Trhnp010Dto>();
            return lista;

        }

        async Task<Trhnp010Dto> IEikonDataRepository.npGetByCodigoAsync(string codigo, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<Trhnp010Dto>(_apiBaseUrl + "Trhnp010/PorCodigo/" + codigo);

            }
            Trhnp010Dto trhnp010Dto = new Trhnp010Dto();
            return trhnp010Dto;
        }

      async   Task<List<Trhnp010Dto>> IEikonDataRepository.npGetByEmCodigoAsync(string emcodigo, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _httpClient.GetFromJsonAsync<List<Trhnp010Dto>>(_apiBaseUrl + "Trhnp010/ByEmCodigo/" + emcodigo);
            }
            List<Trhnp010Dto> lista = new List<Trhnp010Dto>();
            return lista;

        }

        async Task<bool> IEikonDataRepository.npAddAsync(Trhnp010Dto entity, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Trhnp010Dto>(_apiBaseUrl + "Trhnp010/", entity);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        async Task<bool> IEikonDataRepository.npUpdateAsync(Trhnp010Dto entity, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_apiBaseUrl + "Trhnp010/" + entity.NpCodigo, entity);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        async Task<bool> IEikonDataRepository.npDeleteAsync(int id, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.DeleteAsync(_apiBaseUrl + "Trhnp010/" + id.ToString());

                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }


        // Trhct010 inicio

        async Task<List<Trhct010Dto>> IEikonDataRepository.ctGetAllAsync(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _httpClient.GetFromJsonAsync<List<Trhct010Dto>>(_apiBaseUrl + "Trhct010");
            }
            List<Trhct010Dto> lista = new List<Trhct010Dto>();
            return lista;

        }

        async Task<Trhct010Dto> IEikonDataRepository.ctGetByCodigoAsync(string codigo, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<Trhct010Dto>(_apiBaseUrl + "Trhct010/PorCodigo/" + codigo);

            }
            Trhct010Dto trhct010Dto = new Trhct010Dto();
            return trhct010Dto;
        }

        async Task<List<Trhct010Dto>> IEikonDataRepository.ctGetByEmCodigoAsync(string emcodigo, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _httpClient.GetFromJsonAsync<List<Trhct010Dto>>(_apiBaseUrl + "Trhct010/ByEmCodigo/" + emcodigo);
            }
            List<Trhct010Dto> lista = new List<Trhct010Dto>();
            return lista;

        }

        async Task<bool> IEikonDataRepository.ctAddAsync(Trhct010Dto entity, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Trhct010Dto>(_apiBaseUrl + "Trhct010/", entity);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        async Task<bool> IEikonDataRepository.ctUpdateAsync(Trhct010Dto entity, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_apiBaseUrl + "Trhct010/" + entity.CtCodigo, entity);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        async Task<bool> IEikonDataRepository.ctDeleteAsync(int id, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.DeleteAsync(_apiBaseUrl + "Trhct010/" + id.ToString());

                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        // Trhgr010 inicio

        async Task<List<Trhgr010Dto>> IEikonDataRepository.grGetAllAsync(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _httpClient.GetFromJsonAsync<List<Trhgr010Dto>>(_apiBaseUrl + "Trhgr010");
            }
            List<Trhgr010Dto> lista = new List<Trhgr010Dto>();
            return lista;

        }

        async Task<Trhgr010Dto> IEikonDataRepository.grGetByCodigoAsync(string codigo, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<Trhgr010Dto>(_apiBaseUrl + "Trhgr010/PorCodigo/" + codigo);

            }
            Trhgr010Dto trhgr010Dto = new Trhgr010Dto();
            return trhgr010Dto;
        }

        async Task<List<Trhgr010Dto>> IEikonDataRepository.grGetByEmCodigoAsync(string emcodigo, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _httpClient.GetFromJsonAsync<List<Trhgr010Dto>>(_apiBaseUrl + "Trhgr010/ByEmCodigo/" + emcodigo);
            }
            List<Trhgr010Dto> lista = new List<Trhgr010Dto>();
            return lista;

        }

        async Task<bool> IEikonDataRepository.grAddAsync(Trhgr010Dto entity, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Trhgr010Dto>(_apiBaseUrl + "Trhgr010/", entity);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        async Task<bool> IEikonDataRepository.grUpdateAsync(Trhgr010Dto entity, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_apiBaseUrl + "Trhgr010/" + entity.GrCodigo, entity);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        async Task<bool> IEikonDataRepository.grDeleteAsync(int id, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.DeleteAsync(_apiBaseUrl + "Trhgr010/" + id.ToString());

                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }
    

    // Trhcc010 inicio

    async Task<List<Trhcc010Dto>> IEikonDataRepository.ccGetAllAsync(string token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await _httpClient.GetFromJsonAsync<List<Trhcc010Dto>>(_apiBaseUrl + "Trhcc010");
        }
        List<Trhcc010Dto> lista = new List<Trhcc010Dto>();
        return lista;

    }

    async Task<Trhcc010Dto> IEikonDataRepository.ccGetByCodigoAsync(string codigo, string token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetFromJsonAsync<Trhcc010Dto>(_apiBaseUrl + "Trhcc010/PorCodigo/" + codigo);

        }
        Trhcc010Dto trhcc010Dto = new Trhcc010Dto();
        return trhcc010Dto;
    }

    async Task<List<Trhcc010Dto>> IEikonDataRepository.ccGetByEmCodigoAsync(string emcodigo, string token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await _httpClient.GetFromJsonAsync<List<Trhcc010Dto>>(_apiBaseUrl + "Trhcc010/ByEmCodigo/" + emcodigo);
        }
        List<Trhcc010Dto> lista = new List<Trhcc010Dto>();
        return lista;

    }

    async Task<bool> IEikonDataRepository.ccAddAsync(Trhcc010Dto entity, string token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Trhcc010Dto>(_apiBaseUrl + "Trhcc010/", entity);
            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }
        }
        return false;
    }

    async Task<bool> IEikonDataRepository.ccUpdateAsync(Trhcc010Dto entity, string token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_apiBaseUrl + "Trhcc010/" + entity.CcCodigo, entity);
            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }
        }
        return false;
    }

    async Task<bool> IEikonDataRepository.ccDeleteAsync(int id, string token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.DeleteAsync(_apiBaseUrl + "Trhcc010/" + id.ToString());

            if (response.IsSuccessStatusCode)
            { return true; }
            else
            { return false; }
        }
        return false;
    }

        // Trhrw000 inicio

        async Task<List<Trhrw000Dto>> IEikonDataRepository.rwGetAllAsync(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _httpClient.GetFromJsonAsync<List<Trhrw000Dto>>(_apiBaseUrl + "Trhrw000");
            }
            List<Trhrw000Dto> lista = new List<Trhrw000Dto>();
            return lista;

        }

        async Task<Trhrw000Dto> IEikonDataRepository.rwGetByCodigoAsync(string codigo, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<Trhrw000Dto>(_apiBaseUrl + "Trhrw000/PorCodigo/" + codigo);

            }
            Trhrw000Dto trhrw010Dto = new Trhrw000Dto();
            return trhrw010Dto;
        }

        async Task<List<Trhrw000Dto>> IEikonDataRepository.rwGetByEmCodigoAsync(string emcodigo, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _httpClient.GetFromJsonAsync<List<Trhrw000Dto>>(_apiBaseUrl + "Trhrw000/ByEmCodigo/" + emcodigo);
            }
            List<Trhrw000Dto> lista = new List<Trhrw000Dto>();
            return lista;

        }

        async Task<bool> IEikonDataRepository.rwAddAsync(Trhrw000Dto entity, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Trhrw000Dto>(_apiBaseUrl + "Trhcc010/", entity);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        async Task<bool> IEikonDataRepository.rwUpdateAsync(Trhrw000Dto entity, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_apiBaseUrl + "Trhrw000/" + entity.RwCodigo, entity);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        async Task<bool> IEikonDataRepository.rwDeleteAsync(int id, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.DeleteAsync(_apiBaseUrl + "Trhrw000/" + id.ToString());

                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }
        // Trhrw000 fin

        // Trhpr000 inicio
        

        async Task<List<Trhpr000Dto>> IEikonDataRepository.prGetAllAsync(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _httpClient.GetFromJsonAsync<List<Trhpr000Dto>>(_apiBaseUrl + "Trhpr000");
            }
            List<Trhpr000Dto> lista = new List<Trhpr000Dto>();
            return lista;

        }

        async Task<Trhpr000Dto> IEikonDataRepository.prGetByCodigoAsync(string codigo, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await _httpClient.GetFromJsonAsync<Trhpr000Dto>(_apiBaseUrl + "Trhpr000/PorCodigo/" + codigo);

            }
            Trhpr000Dto trhpr010Dto = new Trhpr000Dto();
            return trhpr010Dto;
        }


        async Task<bool> IEikonDataRepository.prAddAsync(Trhpr000Dto entity, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Trhpr000Dto>(_apiBaseUrl + "Trhcc010/", entity);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        async Task<bool> IEikonDataRepository.prUpdateAsync(Trhpr000Dto entity, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_apiBaseUrl + "Trhpr000/" + entity.PrCodigo, entity);
                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        async Task<bool> IEikonDataRepository.prDeleteAsync(int id, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.DeleteAsync(_apiBaseUrl + "Trhpr000/" + id.ToString());

                if (response.IsSuccessStatusCode)
                { return true; }
                else
                { return false; }
            }
            return false;
        }

        // Trhpr000 fin



        
        //***//
        public async Task<List<Frhwd010>> PayrollProcessAsync(
        IEnumerable<CsrPreGenDto> transacciones,
        string emCodigo = "00",
        string ofCodigo = "000",
        string ubCodigo = "00000",
        string tipoEmpl = "F",
        string wbCodigo = "00001",
        int wgAnoRef = 0,
        int wgSecuen = 0,
        string maenomi= "00000000",
        string token="")
        {
            if (!string.IsNullOrEmpty(token))
            {
                if (ofCodigo == null) ofCodigo = "000";
                if (ubCodigo == null) ubCodigo = "00000";
                if (ofCodigo == "0") ofCodigo = "000";
                if (ubCodigo == "0") ubCodigo = "00000";
                if (maenomi == "0") maenomi = "00000000";
                if (maenomi == null) maenomi = "00000000";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var url = $"PayrollProcess/{emCodigo}/{ofCodigo}/{ubCodigo}/{tipoEmpl}/{wbCodigo}/{wgAnoRef}/{wgSecuen}/{maenomi}";
                // https://localhost:44377/api/Frhma010/PayrollProcess/01/000/00000/F/00001/2025/3
                //url = "PayrollProcess/01/000/00000/F/00001/2025/3";
                var response = await _httpClient.PostAsJsonAsync(_apiBaseUrl + "Frhma010/" + url, transacciones);

               // var rt = _httpClient.PostAsJsonAsync(_apiBaseUrl + "Frhma010/" + url, transacciones);
               // rt.Wait();
               // var nomina = rt.Result.Content.ReadFromJsonAsync<List<Frhwd010>>();

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<Frhwd010>>() ?? new List<Frhwd010>();
                    //return await nomina;
                }
                else
                {
                    // Manejo de error
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error en PayrollProcess: {error}");
                }
            }
            return null;
        }

        public Task<IEnumerable<Interfaces.SelectListItem>> puGetSelectListAsync(string token)
        {
            throw new NotImplementedException();
        }
    }
}

