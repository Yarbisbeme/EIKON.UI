using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using EIKON.UI.Models;
//using EIKON.Models;
//using EIKON.Data.Models;    
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using EIKON.Data.DTOs;
using System.Net.Http.Headers;
using DevExpress.Xpo.DB;
using EIKON.UI.Interfaces;
using EIKON.UI.Repositories;
using System.Net;
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.XtraPrinting.Native;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using DevExpress.XtraPrinting.Native.Extensions;
using System.Collections;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using DevExpress.Web.Internal;
using EIKON.UI.Data.Mock;
using DevExpress.XtraRichEdit.Import.OpenXml;
using DevExpress.XtraCharts;

namespace EIKON.UI.Services
{

    public class EikonDataServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        
        private readonly HttpClient _httpClient;
        private IEikonDataRepository _eikonDataRepository;
        //private IEikonApiService _eikonApiService;
        private AuthenticationService _authenticationService;
        private string _apiBaseUrl="";
        public EikonDataServices(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, AuthenticationService authenticationService, IConfiguration conf)
        {
            _httpClient = httpClient;
         
            _httpContextAccessor = httpContextAccessor;
            _authenticationService = authenticationService;
           
            _apiBaseUrl = conf.GetSection("AppApi").GetSection("ApiBaseUrl").Value;
            //_apiBaseUrl = "https://localhost:44377/"; 


            _eikonDataRepository = new EikonDataRepository(_httpClient,_authenticationService, conf);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());


        }

        private string _token;
        private string _pa;
        private string _user;
        private string _tipo;

        //private string _apiBaseUrl = _apiBaseUrl ;
        //private string _apiBaseUrl = "https://localhost:44377/api/";
        
        public void SetToken(string token)
        {
            // Almacenar el token de forma segura, por ejemplo, en el LocalStorage o en una cookie
            //_httpContextAccessor.HttpContext.Session.SetString("token", token);
            _token = token;
            
        }

        public string GetToken()
        {
            return _token;
            //return _httpContextAccessor.HttpContext.Session.GetString("token");
        }

        public void SetUser(string user)
        {
            //_httpContextAccessor.HttpContext.Session.SetString("User", user);
            _user = user;
        }

        public string GetUser()
        {
           // return _httpContextAccessor.HttpContext.Session.GetString("User");
            return _user;
        }
        public void SetPa(string pa)
        {
            
            _pa = pa;
        }

        public string GetPa()
        {
            //return _httpContextAccessor.HttpContext.Session.GetString("Pa");
            return _pa;
        }
        public void SetTipo(string tipo)
        {

            _tipo = tipo;
        }

        public string GetTipo()
        {
            //return _httpContextAccessor.HttpContext.Session.GetString("Pa");
            return _tipo;
        }
        public string GetApiBaseUrl()
        {
            return _apiBaseUrl;
        }
        public async Task<string> GetDataEmpAsync(string emCodigo)
        {
            Trhem000Dto emdto = new Trhem000Dto();
            var api = new Trhem000Client(_apiBaseUrl, _httpClient);
            var emp = (Trhem000Dto) await api.Trhem000PorCodigoAsync(emCodigo);
            return emp.EmDescri; //return await _httpClient.GetFromJsonAsync<List<Trhem000>>(_apiBaseUrl + "Trhem000");
        }
        public async void ObtenerTokenAsync()
        {
            LoginDto user = new LoginDto();
            user.UsrEmail = "eikon@eikon.com.do";
            user.UsrSuper = "Eikon";
            user.UsrTk = " ";
            string str = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(str, Encoding.UTF8, "application/json");

            var rt = _httpClient.GetFromJsonAsync<LoginDto>(_apiBaseUrl + "Authentication/GenerarToken/eikon%40eikon.com.do/Eikon");
            rt.Wait();
            var usr = rt.Result;

            SetToken(usr.UsrTk);
            SetUser(user.UsrEmail);

        }

        public IEnumerable<LoginDto> Frhma010Login(string maenomi, string clave, string empresa="01")
        {

            using (var client = new HttpClient())
            {
                IEnumerable<LoginDto> loginDto = Enumerable.Empty<LoginDto>();
                client.BaseAddress = new Uri(_apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", " ");
                // var responseTask = client.GetAsync("Frhma010/login/" + maenomi + "/" + clave);
                try
                {
                    var url = string.Format("{0}Api/Frhma010/login/{1}/{2}/{3}", _apiBaseUrl, maenomi.Trim(), clave.Trim(),empresa);
                    var rt = client.GetFromJsonAsync<IEnumerable<LoginDto>>(url);
                    rt.Wait();
                    var usr = rt.Result;
                    if (maenomi != "xx")
                    // var responseTask = client.GetAsync("EikonHired/" + maenomi); //
                    // responseTask.Wait();
                    {
                        SetToken(usr.FirstOrDefault().UsrTk);
                        SetPa(usr.FirstOrDefault().UsrPa);
                        SetUser(maenomi);
                    }
                    return usr;
                }
                catch (Exception ex)
                {
                    LoginDto lg = new LoginDto();
                    lg.UsrPa = "0000";
                    lg.UsrName = "Usuario o Clave incorrecta";
                    lg.UsrEmail = " ";
                    lg.UsrEm = " ";
                    lg.UsrSuper = " ";
                    lg.EsAnalista = false;
                    lg.UsrTk = " ";
                    lg.FecExpira = DateTime.Today;
                    lg.EsSupervisor = true;
                    lg.LastVisit = DateTime.Today;
                    lg.Tipo = " ";
                    lg.UsrId = " ";
                    lg.UsrPa = " ";
                    lg.UsrLang = " ";
                    SetToken(" ");
                    loginDto.Append(lg);
                    return loginDto;

                }

                //if (result.IsSuccessStatusCode)
                //{

                //    var responseString = result.Content.ReadAsStringAsync();
                //    try
                //    {
                //        var loginDto = JsonConvert.DeserializeObject<IEnumerable<LoginDto>>(responseString.Result);
                //        SetToken(loginDto.FirstOrDefault().usrTk);
                //        return loginDto;
                //    }
                //    catch (JsonException ex)
                //    {
                //        // Maneja la excepción si hay un problema al deserializar
                //        // Console.WriteLine($"JsonException: {ex.Message}");
                //        // return new LoginDto(); // Devuelve un objeto vacío en caso de fallo
                //        return null;
                //    }
                //    //var customersString = result.Content.ReadAsStringAsync();
                //    // var readTask = result.Content.ReadAsAsync<LoginDto>();
                //    // readTask.Wait();
                //    // return readTask.Result;
                //}
                //else
                //{
                //    IEnumerable<LoginDto> loginDto = Enumerable.Empty<LoginDto>();
                //    return loginDto;
                //}
            }
        }

        public IEnumerable<LoginDto> Frhma010Recuperar(string usuario, string cedula)
        {

            using (var client = new HttpClient())
            {
                IEnumerable<LoginDto> loginDto = Enumerable.Empty<LoginDto>();
                client.BaseAddress = new Uri(_apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", " ");
                // var responseTask = client.GetAsync("Frhma010/login/" + maenomi + "/" + clave);
                try
                {
                    var url = string.Format("{0}Api/Frhma010/Recuperar/{1}/{2}", _apiBaseUrl, usuario.Trim(), cedula.Trim());
                    var rt = client.GetFromJsonAsync<IEnumerable<LoginDto>>(url);
                    rt.Wait();
                    var usr = rt.Result;
                    // var responseTask = client.GetAsync("EikonHired/" + maenomi); //
                    // responseTask.Wait();
                    //SetToken(usr.FirstOrDefault().usrTk);
                    //SetPa(usr.FirstOrDefault().usrPa);
                    //SetUser(usuario);
                    return usr;
                }
                catch (Exception)
                {
                    LoginDto lg = new LoginDto();
                    lg.UsrPa = "0000";
                    lg.UsrName = "Error de datos";
                    lg.UsrEmail = " ";
                    lg.UsrEm = " ";
                    lg.UsrSuper = " ";
                    lg.EsAnalista = false;
                    lg.UsrTk = " ";
                    lg.FecExpira = DateTime.Today;
                    lg.EsSupervisor = true;
                    lg.LastVisit = DateTime.Today;
                    lg.Tipo = " ";
                    lg.UsrId = " ";
                    lg.UsrPId = " ";
                    lg.UsrLang = " ";
                    SetToken(" ");
                    loginDto.Append(lg);
                    return loginDto;

                }

            
            }
        }

        public async Task<List<Trhem000Dto>> Trhem000PorUsurio(string usuario)
        {

            using (var client = new HttpClient())
            {
                List<Trhem000Dto> empresas = new List<Trhem000Dto>();
                //client.BaseAddress = new Uri(_apiBaseUrl);
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", " ");
                //// var responseTask = client.GetAsync("Frhma010/login/" + maenomi + "/" + clave);
                try
                {
                    var api = new Trhem000Client(_apiBaseUrl, client);
                    return (List<Trhem000Dto>) await api.Trhem000PorUsuarioAsync(usuario);
                    
                }
                catch (Exception ex)
                {
                    
                    return empresas;

                }

            }
        }

        public IEnumerable<Trhwb010Dto> Trhwb010DtoPorEmpresa(string em_empresa, string token)
        {

            using (var client = new HttpClient())
            {
                IEnumerable<Trhwb010Dto> nominas = Enumerable.Empty<Trhwb010Dto>();
                client.BaseAddress = new Uri(_apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    //Trhwb010Dto/PorEmpresa/01
                    var url = string.Format("{0}Api/Trhwb010/Trhwb010PorEmpresa/{1}", _apiBaseUrl, em_empresa);
                    var rt = client.GetFromJsonAsync<IEnumerable<Trhwb010Dto>>(url);
                    rt.Wait();
                    var emp = rt.Result;
                    // var responseTask = client.GetAsync("EikonHired/" + maenomi); //
                    // responseTask.Wait();

                    return emp;
                }
                catch (Exception ex)
                {

                    return nominas;

                }

            }
        }
        public IEnumerable<Vfrhma000> VFrhma000PorEmpresaTipo(string em_empresa,string of_codigo, string ub_codigo, string tipos, string token)
        {
            if (em_empresa == null) { em_empresa = "01"; };
            if (em_empresa == "") { em_empresa = "01"; }
            if (of_codigo == null) { of_codigo = "000"; }
            if (of_codigo == "") { of_codigo = "000"; }
            if (ub_codigo == null) { ub_codigo = "00000"; }
            if (of_codigo == "0") { of_codigo = "000"; }
            if (ub_codigo == "0") { ub_codigo = "00000"; }

            ;
            ;
            using (var client = new HttpClient())
            {
                IEnumerable<Vfrhma000> empleados = Enumerable.Empty<Vfrhma000>();
                client.BaseAddress = new Uri(_apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                   
                    var url = string.Format("{0}Api/Frhma010/Vfrhma000EmCodigo/{1}/{2}/{3}/{4}", _apiBaseUrl, em_empresa,of_codigo,ub_codigo,tipos);
                    var rt = client.GetFromJsonAsync<IEnumerable<Vfrhma000>>(url);
                    rt.Wait();
                    var emp = rt.Result;
                    // var responseTask = client.GetAsync("EikonHired/" + maenomi); //
                    // responseTask.Wait();

                    return emp;
                }
                catch (Exception ex)
                {

                    return empleados;

                }

            }
        }
        public async Task<IEnumerable<Trhwg010Dto>> Trhwg010PorEmpresa(string emEmpresa, string wbCodigo, int wgAnoRef, string token)
        {

            using (var client = new HttpClient())
            {
                IEnumerable<Trhwg010Dto> periodos = Enumerable.Empty<Trhwg010Dto>();
                client.BaseAddress = new Uri(_apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    var api = new Trhwg010Client(_apiBaseUrl, _httpClient);
                   var rt= (IEnumerable<Trhwg010Dto>) await api.PorNominaAsync(emEmpresa, wbCodigo, wgAnoRef);
                    //var url = string.Format("{0}Api/Trhwg010/PorNomina/{1}/{2}/{3}", _apiBaseUrl, emEmpresa,wbCodigo,wgAnoRef);
                   // var rt = client.GetFromJsonAsync<IEnumerable<Trhwg010Dto>>(url);
                   // rt.Wait();
                    var Periodos = rt;
                    // var responseTask = client.GetAsync("EikonHired/" + maenomi); //
                    // responseTask.Wait();

                    return Periodos;
                }
                catch (Exception ex)
                {

                    return periodos;

                }

            }
        }

        public IEnumerable<CsrPreGenDto> Trhwc010PorEmpresa(string emEmpresa, string wbCodigo, string token)
        {

            using (var client = new HttpClient())
            {
                IEnumerable<CsrPreGenDto> transacciones = Enumerable.Empty<CsrPreGenDto>();
                client.BaseAddress = new Uri(_apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {//https://localhost:44377/api/Trhwc010/Trhwc010PorEmpresaNomina/01/00001
                    //https://localhost:44377/Api/Trhwc010/PorEmpresaNomina/01/00007
                    var url = string.Format("{0}Api/Trhwc010/Trhwc010PorEmpresaNomina/{1}/{2}", _apiBaseUrl, emEmpresa, wbCodigo);
                    var rt = client.GetFromJsonAsync<IEnumerable<CsrPreGenDto>>(url);
                    rt.Wait();
                    var trans = rt.Result;
                    // var responseTask = client.GetAsync("EikonHired/" + maenomi); //
                    // responseTask.Wait();

                    return trans;
                }
                catch (Exception ex)
                {

                    return transacciones;

                }

            }
        }
        public async Task<List<MenuItemDto>> GetMenuForRole(string rol)
        {   var api = new AuthenticationClient(_apiBaseUrl, _httpClient);
            return (List<MenuItemDto>)await api.GetMenuFromRolesAsync(rol);

            //return _allMenuItems.Where(m => m.Role == role).ToList();
            //using (var client = new HttpClient())
            //{
            //    List<MenuItemDto> loginDto = new List<MenuItemDto>();
            //    client.BaseAddress = new Uri(_apiBaseUrl);
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            //    // https://localhost:44377/api/Authentication/GetMenuFromRoles/0015
            //    try
            //    {
            //        var url = string.Format("{0}Api/Authentication/GetMenuFromRoles/{1}", _apiBaseUrl, "0015");
            //        var rt = client.GetFromJsonAsync<List<MenuItemDto>>(url);
            //        rt.Wait();
            //        var usr = rt.Result;
            //        // var responseTask = client.GetAsync("EikonHired/" + maenomi); //
            //        // responseTask.Wait();
            //        // SetToken(usr.FirstOrDefault().usrTk);
            //        return rt.Result;
            //    }
            //    catch (Exception)
            //    {
            //        MenuItemDto lg = new MenuItemDto();
            //        lg.Title = "No Data";
            //        lg.Url = "/";
            //        lg.Children = null;

            //        loginDto.Add(lg);
            //        return loginDto;

            //    }


            //}
        }
        public async Task<List<MenuItemDto>> GetMenuModulos()
        {
            var api = new AuthenticationClient(_apiBaseUrl, _httpClient);
            return (List<MenuItemDto>)await api.GetMenuModulosAsync();

            
        }
        public async Task<List<MenuItemDto>> GetMenuRoles(string pa_codigo)
        {
            var api = new AuthenticationClient(_apiBaseUrl, _httpClient);
            return (List<MenuItemDto>)await api.GetMenuFromRolesAsync(pa_codigo);


        }

        public async Task<List<DxMnItemDto>> GetDxMenuForRole(string maenume, string Token)
        {
            //return _allMenuItems.Where(m => m.Role == role).ToList();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            List<DxMnItemDto> loginDto = new List<DxMnItemDto>();
            var api = new AuthenticationClient(_apiBaseUrl, _httpClient);
            var lg= (List<DxMnItemDto>)await api.GetDxMenuFromRolesAsync(maenume);
            if (lg.Count == 0)
            {
                DxMnItemDto lg1 = new DxMnItemDto();
                lg1.Name = "No Data";
                lg1.Id = "/";
                lg1.Items = null;
                loginDto.Add(lg1);
                return loginDto;
            }
            else
            {
                return lg;
            }
            //using (var client = new HttpClient())
            //{
            //    _apiBaseUrl = GetApiBaseUrl();

            //    List<DxMnItemDto> loginDto = new List<DxMnItemDto>();
            //    client.BaseAddress = new Uri(_apiBaseUrl);
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            //    // https://localhost:44377/api/Authentication/GetMenuFromRoles/0015
            //    try
            //    {
            //        var url = string.Format("{0}Authentication/GetDxMenuFromRoles/{1}", _apiBaseUrl, maenume);
            //        var rt = client.GetFromJsonAsync<List<DxMnItemDto>>(url);
            //        rt.Wait();
            //        var usr = rt.Result;
            //        // var responseTask = client.GetAsync("EikonHired/" + maenomi); //
            //        // responseTask.Wait();
            //        // SetToken(usr.FirstOrDefault().usrTk);
            //        return rt.Result;
            //    }
            //    catch (Exception)
            //    {
            //        DxMnItemDto lg = new DxMnItemDto();
            //        lg.Name = "No Data";
            //        lg.Id = "/";
            //        lg.Items = null;

            //        loginDto.Add(lg);
            //        return loginDto;

            //    }





            //}

        }
        public async Task<List<DxMnItemDto>> GetDxMenuForEmpresaUsuario(string em_empresa, string usuario, string Token)
        {   
            List<DxMnItemDto> loginDto = new List<DxMnItemDto>();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var api = new AuthenticationClient(_apiBaseUrl, _httpClient);
            var lg = (List<DxMnItemDto>)await api.GetDxMenuFromEmpresaUsuarioAsync(em_empresa,usuario);
            if (lg.Count == 0)
            {
                DxMnItemDto lg1 = new DxMnItemDto();
                lg1.Name = "No Data";
                lg1.Id = "/";
                lg1.Items = null;
                loginDto.Add(lg1);
                return loginDto;
            }
            else
            {
                return lg;
            }
        
        }

        public async Task<List<Frhrq010Dto>> FRHrq010DtoPorEmpresa(string em_empresa)
        {
            List<Frhrq010Dto> loginDto = new List<Frhrq010Dto>();
            var api = new Frhrq010Client(_apiBaseUrl, _httpClient);
            var lg = (List<Frhrq010Dto>)await api.Frhrq010AllAsync();
            if (lg.Count == 0)
            {
                Frhrq010Dto lg1 = new Frhrq010Dto();
                lg1.Reqtitu = "No hay Datos";
                lg1.Camnume = "";
                lg1.AnCodigo = null;
                loginDto.Add(lg1);
                return loginDto;
            }
            else
            {
                return (List<Frhrq010Dto>) lg.Where(e=>e.EmEmpresa==em_empresa).ToList();
            }


        }

        public async Task<List<Frhrq010Dto>> FRHrq010DtoInsert(Frhrq010Dto rq)
        {
            var api = new Frhrq010Client(_apiBaseUrl, _httpClient);
            await api.Frhrq010POSTAsync(rq);
            return (List<Frhrq010Dto>)await api.Frhrq010AllAsync();


        }

        public async Task<List<Frhjl010Dto>> Frhjl010DtoInsert(IEnumerable<Frhjl010Dto> jl)
        {
            var api = new Frhjl010Client(_apiBaseUrl, _httpClient);
            await api.Frhjl010AllPOSTAsync(jl);
            return (List<Frhjl010Dto>)await api.Frhjl010AllGETAsync();
        }
        public async Task<bool> Frhjl010DtoDelete(string jonumber)
        {
            var api = new Frhjl010Client(_apiBaseUrl, _httpClient);
            await api.Frhjl010DELETEAsync(jonumber);
            return true;


        }
        public async Task<List<Frhrq010Dto>> FRHrq010DtoUpdate(int reqid, Frhrq010Dto rqDto)
        {
            var api = new Frhrq010Client(_apiBaseUrl, _httpClient);
            try
            {
                await api.Frhrq010PUTAsync(reqid, rqDto);
                Console.WriteLine("Actualizado correctamente");
                return (List<Frhrq010Dto>)await api.Frhrq010AllAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error 1 Update Req: " +ex.ToString());
                return (List<Frhrq010Dto>)await api.Frhrq010AllAsync();
            }
            //await api.Frhrq010PUTAsync(reqid,rqDto);
            


        }
        public async Task<List<Frhma010Dto>> Frhma010PorEmpresa(string em_empresa)
        {
           // List<Frhma010Dto> data = new List<Frhma010Dto>();
            var api = new Frhma010Client(_apiBaseUrl, _httpClient);
            var lg = (List<Frhma010Dto>)await api.Frhma010Async();
            if (lg.Count == 0)
            {
                Frhma010Dto lg1 = new Frhma010Dto();
                lg1.Maenomb = "No hay Datos";
                lg1.Maeapel = "";
                lg1.Maenume = null;
                lg.Add(lg1);
                return lg;
            }
            else
            {
                return (List<Frhma010Dto>)lg.Where(e => e.EmEmpresa == em_empresa).ToList();
            }


        }
        public async Task<List<RptPuestosEstatus>> GetDataRrhel006(string Estatus)
        {
            //return _allMenuItems.Where(m => m.Role == role).ToList();
            var api = new ReportDataClient(_apiBaseUrl, _httpClient);
            return (List<RptPuestosEstatus>)await api.PuestosAsync(Estatus); 
            //using (var client = new HttpClient())
            //{
            //    List<RptPuestosEstatus> reportData = new List<RptPuestosEstatus>();
            //    client.BaseAddress = new Uri(_apiBaseUrl);
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            //    // https://localhost:44377/api/Authentication/GetMenuFromRoles/0015
            //    try
            //    { //https://localhost:8082/api/ReportData/puestos/A
            //        var url = string.Format("{0}ReportData/puestos/{1}", _apiBaseUrl, Estatus);
            //        var rt = client.GetFromJsonAsync<List<RptPuestosEstatus>>(url);
            //        rt.Wait();
            //        var usr = rt.Result;
            //        // var responseTask = client.GetAsync("EikonHired/" + maenomi); //
            //        // responseTask.Wait();
            //        // SetToken(usr.FirstOrDefault().usrTk);
            //        return rt.Result;
            //    }
            //    catch (Exception)
            //    {
            //        RptPuestosEstatus lg = new RptPuestosEstatus();
            //        lg.Jo_titulo = "No Data";


            //        reportData.Add(lg);
            //        return reportData;

            //    }


            //}
        }
        ///api/ReportData/puestosPorAreas/{of_codigo}/{ub_codigo}/{np_codigo}/{jo_number}/{m_empresa}
        public async Task<List<Rrhel006aDto>> GetDataRrhel006a(string of_codigo, string ub_codigo, string np_codigo, string jo_number, string m_empresa)
        {
            var api = new ReportDataClient(_apiBaseUrl, _httpClient);
            return (List<Rrhel006aDto>)await api.PuestosPorAreasAsync(of_codigo, ub_codigo, np_codigo, jo_number, m_empresa);
        }
            //return _allMenuItems.Where(m => m.Role == role).ToList();
        //    using (var client = new HttpClient())
        //    {
        //        List<Rrhel006aDto> reportData = new List<Rrhel006aDto>();
        //        client.BaseAddress = new Uri(_apiBaseUrl);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
        //        // https://localhost:44377/api/Authentication/GetMenuFromRoles/0015
        //        try
        //        { //https://localhost:8082/api/ReportData/puestos/A
        //            var url = string.Format("{0}ReportData/puestosPorAreas/{1}/{2}/{3}/{4}/{5}", _apiBaseUrl, of_codigo, ub_codigo, np_codigo, jo_number, m_empresa);
        //            var rt = client.GetFromJsonAsync<List<Rrhel006aDto>>(url);
        //            rt.Wait();
        //            var usr = rt.Result;
        //            // var responseTask = client.GetAsync("EikonHired/" + maenomi); //
        //            // responseTask.Wait();
        //            // SetToken(usr.FirstOrDefault().usrTk);
        //            return rt.Result;
        //        }
        //        catch (Exception)
        //        {
        //            Rrhel006aDto lg = new Rrhel006aDto();
        //            lg.Np_descri = "No Data";


        //            reportData.Add(lg);
        //            return reportData;

        //        }


        //    }
        //}
        //// Acceso a datos trhis000 Inicio
        public async Task<List<Trhis000Dto>> GetTrhis000Dtos()
        { var api = new Trhis000Client(_apiBaseUrl, _httpClient);
               return (List<Trhis000Dto>)await api.Trhis000AllAsync();
        }

        public async Task<List<Trhta000Dto>> GetTrhta000Dtos()
        {
            var api = new Trhta000Client(_apiBaseUrl, _httpClient);
            return (List<Trhta000Dto>)await api.Trhta000AllAsync();
        }
        public async Task<List<Trhis000Dto>> GetTrhis000DtosByIsCodigo(string IsCodigo)
        {
            var api = new Trhis000Client(_apiBaseUrl, _httpClient);
            var ret= (List<Trhis000Dto>)await api.Trhis000AllAsync();
            return ret.Where(isc=>isc.IsForm.Trim()==IsCodigo).ToList();
        }
        public async Task<List<Trhmp000Dto>> GetTrhmp000DtosByIsCodigo(string PaCodigo)
        {
            var api = new Trhmp000Client(_apiBaseUrl, _httpClient);
            var ret = (List<Trhmp000Dto>)await api.Trhmp000AllAsync();
            return ret.Where(isc => isc.PaCodigo.Contains(PaCodigo)).ToList();
        }
        public async Task<Trhis000Dto> GetByIdTrhis000Dtos(int id)
        {
            var api = new Trhis000Client(_apiBaseUrl, _httpClient);
            return (Trhis000Dto)await api.Trhis000PorCodigoAsync(id.ToString());
            // return await _eikonDataRepository.GetById(id);
        }
        public async Task<Trhis000Dto> GetByEkCodigoTrhis000Dtos(string IsCodigo)
        {
            var api = new Trhis000Client(_apiBaseUrl, _httpClient);
            return (Trhis000Dto)await api.Trhis000PorCodigoAsync(IsCodigo);
            //return await _eikonDataRepository.GetByEkCodigo(IsCodigo);
        }

        public async Task<bool> InsertTrhis000Dto(Trhis000Dto trhis)
        {
            //return await _eikonDataRepository.Insert(trhis);
            var api = new Trhis000Client(_apiBaseUrl, _httpClient);
            await api.Trhis000POSTAsync(trhis);
            return true;
        }
        public async Task<bool> UpdateTrhis000Dto(Trhis000Dto trhis)
        {
            //return await _eikonDataRepository.Update(trhis);
            var api = new Trhis000Client(_apiBaseUrl, _httpClient);
            await api.Trhis000PUTAsync(trhis.IdentityColumn,trhis);
            return true;
        }
        public async Task<bool> DeleteTrhis000Dto(int id)
        {
            //return await _eikonDataRepository.DeleteById(id);
            var api = new Trhis000Client(_apiBaseUrl, _httpClient);
            await api.Trhis000DELETEAsync(id);
            return true;    
        }
    // Acceso a datos trhis000 Fin

    // Acceso a datos trhpa000 Inicio
    public async Task<List<Trhpa000Dto>> GetTrhpa000Dtos()
    {
            //return await _eikonDataRepository.paGetAll();
            var api = new Trhpa000Client(_apiBaseUrl, _httpClient);
            return (List<Trhpa000Dto>)await api.Trhpa000AllAsync();

        }
    public async Task<Trhpa000Dto> GetByIdTrhpa000Dtos(int id)
    {
            //return await _eikonDataRepository.paGetById(id);
            var api = new Trhpa000Client(_apiBaseUrl, _httpClient);
            return (Trhpa000Dto)await api.Trhpa000GETAsync(id);
        }
    public async Task<Trhpa000Dto> GetByEkCodigoTrhpa000Dtos(string PaCodigo)
    {   //**revisar se creo metodo nuevo en el servidor
        return await _eikonDataRepository.paGetByEkCodigo(PaCodigo);
    }

    public async Task<bool> InsertTrhpa000Dto(Trhpa000Dto trhpa)
    {
            //return await _eikonDataRepository.paInsert(trhpa);
            var api = new Trhpa000Client(_apiBaseUrl, _httpClient);
            await api.Trhpa000POSTAsync(trhpa);
            return true;
        }
    public async Task<bool> UpdateTrhpa000Dto(Trhpa000Dto trhpa)
    {
            //return await _eikonDataRepository.paUpdate(trhpa);
            var api = new Trhpa000Client(_apiBaseUrl, _httpClient);
            await api.Trhpa000PUTAsync(trhpa.IdentityColumn, trhpa);
            return true;
        }
    public async Task<bool> DeleteTrhpa000Dto(int id)
    {
            //return await _eikonDataRepository.paDeleteById(id);
            var api = new Trhpa000Client(_apiBaseUrl, _httpClient);
            try
            {
                await api.Trhpa000DELETEAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                // Loguear el error 
                return false; // fallo el delete;
            }
        }
        // Acceso a datos trhpa000 Fin

        // Acceso a datos trhnz000 Inicio
        public async Task<List<Trhnz000Dto>> GetTrhnz000Dtos()
        {
            //return await _eikonDataRepository.nzGetAll();
            var api = new Trhnz000Client(_apiBaseUrl, _httpClient);
            return (List<Trhnz000Dto>)await api.Trhnz000AllAsync();

        }
        public async Task<Trhnz000Dto> GetByIdTrhnz000Dtos(int id)
        {
             //**revisar controlador metodo ById
            return await _eikonDataRepository.nzGetById(id);
            //var api = new Trhnz000Client(_apiBaseUrl, _httpClient);
            //return (Trhnz000Dto)await api.PorCodigo129Async(id);
        }
        public async Task<Trhnz000Dto> GetByEkCodigoTrhnz000Dtos(string nzCodigo)
        {
             //**revisar controlador tipo Dto
            //return await _eikonDataRepository.nzGetByEkCodigo(nzCodigo);
            var api = new Trhnz000Client(_apiBaseUrl, _httpClient);
            return (Trhnz000Dto)await api.Trhnz000PorCodigoAsync(nzCodigo);
        }

        public async Task<bool> InsertTrhnz000Dto(Trhnz000Dto trhnz)
        {
            //return await _eikonDataRepository.nzInsert(trhnz);
            var api = new Trhnz000Client(_apiBaseUrl, _httpClient);
            try
            {
                await api.Trhnz000POSTAsync(trhnz);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
            
        }
        public async Task<bool> UpdateTrhnz000Dto(Trhnz000Dto trhnz)
        {
            // return await _eikonDataRepository.nzUpdate(trhnz);
            var api = new Trhnz000Client(_apiBaseUrl, _httpClient);
            try
            {
                await api.Trhnz000PUTAsync(trhnz.IdentityColumn, trhnz);
                return true;
            }
            catch (Exception)
            {
                return false;
            }   
        }
        public async Task<bool> DeleteTrhnz000Dto(int id)
        {
            // return await _eikonDataRepository.nzDeleteById(id);
            var api = new Trhnz000Client(_apiBaseUrl, _httpClient);
            try
            {
                await api.Trhnz000DELETEAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                // Loguear el error 
                return false; // fallo el delete;
            }   
        }
        // Acceso a datos trhnz000 Fin


        // Acceso a datos trhps000 Inicio
        public async Task<List<Trhps000Dto>> GetTrhps000Dtos()
        {   //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhps000Client(_apiBaseUrl, _httpClient);
            return (List<Trhps000Dto>)await api.Trhps000AllAsync();
        }
        //GetTrhem000Dtos
        public async Task<List<Trhem000Dto>> GetTrhem000Dtos()
        {   //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhem000Client(_apiBaseUrl, _httpClient);
            return (List<Trhem000Dto>)await api.Trhem000AllAsync();
        }
        //GetTrhhq000Dtos
        public async Task<List<Trhhq000Dto>> GetTrhhq000Dtos()
        {   //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhhq000Client(_apiBaseUrl, _httpClient);
            return (List<Trhhq000Dto>)await api.Trhhq000AllAsync();
        }
        //GetTrhnc000Dtos
        public async Task<List<Trhnc000Dto>> GetTrhnc000Dtos()
        {   //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhnc000Client(_apiBaseUrl, _httpClient);
            return (List<Trhnc000Dto>)await api.Trhnc000AllAsync();
        }
        //GetTrhpv000Dtos
        public async Task<List<Trhpv000Dto>> GetTrhpv000Dtos()
        {   //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhpv000Client(_apiBaseUrl, _httpClient);
            return (List<Trhpv000Dto>)await api.Trhpv000AllAsync();
        }
        public async Task<List<Trhwc010Dto>> GetTrhwc010Dtos()
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhwc010Client(_apiBaseUrl, _httpClient);
            return (List<Trhwc010Dto>)await api.Trhwc010AllAsync();

        }

        //GetTrhlw010Dtos
        public async Task<List<Trhlw010Dto>> GetTrhlw010Dtos()
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhlw010Client(_apiBaseUrl, _httpClient);
            return (List<Trhlw010Dto>)await api.Trhlw010AllAsync();

        }

        public async Task<List<Trhlw010Dto>> PostTrhlw010Dtos(Trhlw010Dto trhwi)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhlw010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhlw010POSTAsync(trhwi);
            return (List<Trhlw010Dto>)await api.Trhlw010AllAsync();

        }

        public async Task<List<Trhlw010Dto>> PutTrhlw010Dtos(Trhlw010Dto trhwi)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhlw010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhlw010PUTAsync(trhwi.IdentityColumn, trhwi);
            return (List<Trhlw010Dto>)await api.Trhlw010AllAsync();

        }
        public async Task<List<Trhlw010Dto>> DelTrhlw010Dtos(int id)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhlw010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhlw010DELETEAsync(id);
            return (List<Trhlw010Dto>)await api.Trhlw010AllAsync();

        }


        //GetTrhwx010Dtos
        public async Task<List<Trhwx010Dto>> GetTrhwx010Dtos()
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhwx010Client(_apiBaseUrl, _httpClient);
            return (List<Trhwx010Dto>)await api.Trhwx010AllAsync();

        }

        public async Task<List<Trhwx010Dto>> PostTrhwx010Dtos(Trhwx010Dto trhwi)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhwx010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhwx010POSTAsync(trhwi);
            return (List<Trhwx010Dto>)await api.Trhwx010AllAsync();

        }

        public async Task<List<Trhwx010Dto>> PutTrhwx010Dtos(Trhwx010Dto trhwi)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhwx010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhwx010PUTAsync(trhwi.IdentityColumn, trhwi);
            return (List<Trhwx010Dto>)await api.Trhwx010AllAsync();

        }
        public async Task<List<Trhwx010Dto>> DelTrhwx010Dtos(int id)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhwx010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhwx010DELETEAsync(id);
            return (List<Trhwx010Dto>)await api.Trhwx010AllAsync();

        }


        //GetTrhwt010Dtos
        public async Task<List<Trhwt010Dto>> GetTrhwt010Dtos()
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhwt010Client(_apiBaseUrl, _httpClient);
            return (List<Trhwt010Dto>)await api.Trhwt010AllAsync();

        }

        public async Task<List<Trhwt010Dto>> PostTrhwt010Dtos(Trhwt010Dto trhwi)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhwt010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhwt010POSTAsync(trhwi);
            return (List<Trhwt010Dto>)await api.Trhwt010AllAsync();

        }

        public async Task<List<Trhwt010Dto>> PutTrhwt010Dtos(Trhwt010Dto trhwi)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhwt010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhwt010PUTAsync(trhwi.IdentityColumn, trhwi);
            return (List<Trhwt010Dto>)await api.Trhwt010AllAsync();

        }
        public async Task<List<Trhwt010Dto>> DelTrhwt010Dtos(int id)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhwt010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhwt010DELETEAsync(id);
            return (List<Trhwt010Dto>)await api.Trhwt010AllAsync();

        }


        //GetTrhwi010Dtos
        public async Task<List<Trhwi010Dto>> GetTrhwi010Dtos()
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhwi010Client(_apiBaseUrl, _httpClient);
            return (List<Trhwi010Dto>)await api.Trhwi010AllAsync();

        }
        public async Task<List<Trhwi010Dto>> PostTrhwi010Dtos(Trhwi010Dto trhwi)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhwi010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhwi010POSTAsync(trhwi); 
            return (List<Trhwi010Dto>)await api.Trhwi010AllAsync();

        }

        public async Task<List<Trhwi010Dto>> PutTrhwi010Dtos(Trhwi010Dto trhwi)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhwi010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhwi010PUTAsync(trhwi.IdentityColumn, trhwi);
            return (List<Trhwi010Dto>)await api.Trhwi010AllAsync();

        }
        public async Task<List<Trhwi010Dto>> DelTrhwi010Dtos(int id)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhwi010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhwi010DELETEAsync(id);
            return (List<Trhwi010Dto>)await api.Trhwi010AllAsync();

        }

        //GetTrhca010Dtos
        public async Task<List<Trhca010Dto>> GetTrhca010Dtos()
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhca010Client(_apiBaseUrl, _httpClient);
            return (List<Trhca010Dto>)await api.Trhca010AllAsync();

        }
        public async Task<List<Trhca010Dto>> PostTrhca010Dtos(Trhca010Dto trhwi)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhca010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhca010POSTAsync(trhwi);
            return (List<Trhca010Dto>)await api.Trhca010AllAsync();

        }

        public async Task<List<Trhca010Dto>> PutTrhca010Dtos(Trhca010Dto trhwi)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhca010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhca010PUTAsync(trhwi.IdentityColumn, trhwi);
            return (List<Trhca010Dto>)await api.Trhca010AllAsync();

        }
        public async Task<List<Trhca010Dto>> DelTrhca010Dtos(int id)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhca010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhca010DELETEAsync(id);
            return (List<Trhca010Dto>)await api.Trhca010AllAsync();

        }
        //GetTrhvt000Dtos
        public async Task<List<Trhvt000Dto>> GetTrhvt000Dtos()
        {
            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhvt000Client(_apiBaseUrl, _httpClient);
            return (List<Trhvt000Dto>)await api.Trhvt000AllAsync();

        }

        //GetTrhpe000Dtos
        public async Task<List<Trhpe000Dto>> GetTrhpe000Dtos()
        {
             //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhpe000Client(_apiBaseUrl, _httpClient);
            return (List<Trhpe000Dto>)await api.Trhpe000AllAsync();

        }

        // GetTrhtp010Dtos
        public async Task<List<Trhtp010Dto>> GetTrhtp010Dtos()
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhtp010Client(_apiBaseUrl, _httpClient);
            return (List<Trhtp010Dto>)await api.Trhtp010AllAsync();

        }

        public async Task<List<Trhtp010Dto>> PostTrhtp010Dtos(Trhtp010Dto trhwi)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhtp010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhtp010POSTAsync(trhwi);
            return (List<Trhtp010Dto>)await api.Trhtp010AllAsync();

        }

        public async Task<List<Trhtp010Dto>> PutTrhtp010Dtos(Trhtp010Dto trhwi)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhtp010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhtp010PUTAsync(trhwi.IdentityColumn, trhwi);
            return (List<Trhtp010Dto>)await api.Trhtp010AllAsync();

        }
        public async Task<List<Trhtp010Dto>> DelTrhtp010Dtos(int id)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhtp010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhtp010DELETEAsync(id);
            return (List<Trhtp010Dto>)await api.Trhtp010AllAsync();

        }

        // GetTrhtc010Dtos
        public async Task<List<Trhtc010Dto>> GetTrhtc010Dtos()
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhtc010Client(_apiBaseUrl, _httpClient);
            return (List<Trhtc010Dto>)await api.Trhtc010AllAsync();

        }

        public async Task<List<Trhtc010Dto>> PostTrhtc010Dtos(Trhtc010Dto trhwi)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhtc010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhtc010POSTAsync(trhwi);
            return (List<Trhtc010Dto>)await api.Trhtc010AllAsync();

        }

        public async Task<List<Trhtc010Dto>> PutTrhtc010Dtos(Trhtc010Dto trhwi)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhtc010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhtc010PUTAsync(trhwi.IdentityColumn, trhwi);
            return (List<Trhtc010Dto>)await api.Trhtc010AllAsync();

        }
        public async Task<List<Trhtc010Dto>> DelTrhtc010Dtos(int id)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhtc010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhtc010DELETEAsync(id);
            return (List<Trhtc010Dto>)await api.Trhtc010AllAsync();

        }


        //GetTrhta010Dtos
        public async Task<List<Trhta010Dto>> GetTrhta010Dtos()
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhta010Client(_apiBaseUrl, _httpClient);
            return (List<Trhta010Dto>)await api.Trhta010AllAsync();

        }
        public async Task<List<Trhta010Dto>> PostTrhta010Dtos(Trhta010Dto trhwi)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhta010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhta010POSTAsync(trhwi);
            return (List<Trhta010Dto>)await api.Trhta010AllAsync();

        }

        public async Task<List<Trhta010Dto>> PutTrhta010Dtos(Trhta010Dto trhwi)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhta010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhta010PUTAsync(trhwi.IdentityColumn, trhwi);
            return (List<Trhta010Dto>)await api.Trhta010AllAsync();

        }
        public async Task<List<Trhta010Dto>> DelTrhta010Dtos(int id)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhta010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhta010DELETEAsync(id);
            return (List<Trhta010Dto>)await api.Trhta010AllAsync();

        }


        //GetTrhwy010Dtos
        public async Task<List<Trhwy010Dto>> GetTrhwy010Dtos()
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhwy010Client(_apiBaseUrl, _httpClient);
            return (List<Trhwy010Dto>)await api.Trhwy010AllAsync();

        }

        public async Task<List<Trhwy010Dto>> PostTrhwy010Dtos(Trhwy010Dto trhwi)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhwy010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhwy010POSTAsync(trhwi);
            return (List<Trhwy010Dto>)await api.Trhwy010AllAsync();

        }

        public async Task<List<Trhwy010Dto>> PutTrhwy010Dtos(Trhwy010Dto trhwi)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhwy010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhwy010PUTAsync(trhwi.IdentityColumn, trhwi);
            return (List<Trhwy010Dto>)await api.Trhwy010AllAsync();

        }
        public async Task<List<Trhwy010Dto>> DelTrhwy010Dtos(int id)
        {

            //return await _eikonDataRepository.psGetAll(GetToken());
            var api = new Trhwy010Client(_apiBaseUrl, _httpClient);
            var ret = api.Trhwy010DELETEAsync(id);
            return (List<Trhwy010Dto>)await api.Trhwy010AllAsync();

        }

        public async Task<Trhps000Dto> GetByIdTrhps000Dtos(int id)
        {
             //**revisar controlador metodo ById
            return await _eikonDataRepository.psGetById(id);
            //var api = new Trhps000Client(_apiBaseUrl, _httpClient);
            //return (Trhps000Dto)await api.??(id);
        }
        

        public async Task<bool> InsertTrhps000Dto(Trhps000Dto trhps)
        {
            //return await _eikonDataRepository.psInsert(trhps);
            var api = new Trhps000Client(_apiBaseUrl, _httpClient);
            try
            {
                await api.Trhps000POSTAsync(trhps);
                return true;
            }
            catch (Exception)
            {
                return false;
            }   
        }
        public async Task<bool> UpdateTrhps000Dto(Trhps000Dto trhps)
        {
            //return await _eikonDataRepository.psUpdate(trhps);
            var api = new Trhps000Client(_apiBaseUrl, _httpClient);
            try
            {
                await api.Trhps000PUTAsync(trhps.IdentityColumn, trhps);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> DeleteTrhps000Dto(int id)
        {
            //return await _eikonDataRepository.psDeleteById(id);
            var api = new Trhps000Client(_apiBaseUrl, _httpClient);
            try
            {
                await api.Trhps000DELETEAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                // Loguear el error 
                return false; // fallo el delete;
            }
        }
        // Acceso a datos trhps000 Fin


        // Acceso a datos Frhue000 Inicio
        public async Task<List<Frhue000Dto>> GetFrhue000Dtos()
        {
            //return await _eikonDataRepository.ueGetAll();
            var api = new Frhue000Client(_apiBaseUrl, _httpClient);
            return (List<Frhue000Dto>)await api.Frhue000AllAsync();

        }
        public async Task<List<Frhue000Dto>> GetFrhue000DtosUserId(string UserId)
        {
            //return await _eikonDataRepository.ueGetAll();
            var api = new Frhue000Client(_apiBaseUrl, _httpClient);
            return (List<Frhue000Dto>)await api.Frhue000sByUserIdAsync(UserId);

        }
        public async Task<Frhue000Dto> GetByIdFrhue000Dtos(int id)
        {
            //return await _eikonDataRepository.ueGetById(id);
            var api = new Frhue000Client(_apiBaseUrl, _httpClient);
            return (Frhue000Dto)await api.Frhue000GETAsync(id);

        }
        public async Task<List<Frhue000Dto>> GetFrhue000DtosByPasUserId(string ueCodigo)
        {
            //return await _eikonDataRepository.ueGetFrhue000sByUserId(ueCodigo);
            var api = new Frhue000Client(_apiBaseUrl, _httpClient);
            return (List<Frhue000Dto>)await api.Frhue000sByUserIdAsync(ueCodigo);
        }

        public async Task<bool> InsertFrhue000Dto(Frhue000Dto Frhue)
        {
            // return await _eikonDataRepository.ueInsert(Frhue);
            var api = new Frhue000Client(_apiBaseUrl, _httpClient);
            try
            {
                await api.Frhue000POSTAsync(Frhue);
                return true;
            }
            catch (Exception)
            {
                return false;
            }   

        }
        public async Task<bool> UpdateFrhue000Dto(Frhue000Dto Frhue)
        {
            // return await _eikonDataRepository.ueUpdate(Frhue);
            var api = new Frhue000Client(_apiBaseUrl, _httpClient);
            try
            {
                await api.Frhue000PUTAsync(Frhue.IdentityColumn, Frhue);
                return true;
            }
            catch (Exception ex)
            {
                // Loguear el error 
                return false; // fallo el delete;
            }
        }
        public async Task<bool> DeleteFrhue000Dto(int id)
        {
            return await _eikonDataRepository.ueDeleteById(id);
        }
        // Acceso a datos Frhue000 Fin

        // Acceso a datos Trhfm000 Inicio
        public async Task<List<Trhfm000Dto>> GetTrhfm000Dtos()
        {
            return await _eikonDataRepository.fmGetAll();
        }
        public async Task<Trhfm000Dto> GetByIdTrhfm000Dtos(int id)
        {
            return await _eikonDataRepository.fmGetById(id);
        }
        public async Task<Trhfm000Dto> GetTrhfm000DtosByPaIs(string IsCodigo, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var api = new Trhfm000Client(_apiBaseUrl, _httpClient);
            return (Trhfm000Dto)await api.PorIsCodigoAsync(IsCodigo);
            // return await _eikonDataRepository.fmGetTrhfm000sByPaIs(IsCodigo);
        }
        public async Task<List<Trhfm000Dto>> GetTrhfm000DtosByPaIsCodigo(string IsCodigo, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var api = new Trhfm000Client(_apiBaseUrl, _httpClient);
            var fm = (List<Trhfm000Dto>)await api.Trhfm000AllAsync();
            return fm.Where(f => f.IsCodigo.Trim() == IsCodigo).ToList();
            // return await _eikonDataRepository.fmGetTrhfm000sByPaIs(IsCodigo);
        }
        public async Task<bool> InsertTrhfm000Dto(Trhfm000Dto Trhfm)
        {
            //return await _eikonDataRepository.fmInsert(Trhfm);
            var api = new Trhfm000Client(_apiBaseUrl, _httpClient);
            try
            {
                await api.Trhfm000POSTAsync(Trhfm);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
            
        }
        public async Task<bool> UpdateTrhfm000Dto(Trhfm000Dto Trhfm)
        {
            var api = new Trhfm000Client(_apiBaseUrl, _httpClient);
            try
            {
                await api.Trhfm000PUTAsync(Trhfm.IdentityColumn,Trhfm);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        public async Task<bool> UpdateTrhmp000Dto(Trhmp000Dto Trhmp)
        {
            var api = new Trhmp000Client(_apiBaseUrl, _httpClient);
            try
            {
                await api.Trhmp000PUTAsync(Trhmp.IdentityColumn, Trhmp);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        public async Task<bool> InsertTrhmp000Dto(Trhmp000Dto Trhmp)
        {
            var api = new Trhmp000Client(_apiBaseUrl, _httpClient);
            try
            {
                await api.Trhmp000POSTAsync(Trhmp);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        public async Task<bool> DeleteTrhfm000Dto(int id)
        {
            //return await _eikonDataRepository.fmDeleteById(id);
            var api = new Trhfm000Client(_apiBaseUrl, _httpClient);
            try
            {
                await api.Trhfm000DELETEAsync(id);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        // Acceso a datos Trhfm000 Fin

        // Acceso a datos Frhso000 Inicio
        public async Task<List<Frhso000Dto>> GetFrhso000Dtos()
        {   var api = new Frhso000Client(_apiBaseUrl, _httpClient);
            return (List<Frhso000Dto>)await api.Frhso000AllAsync();
            //return await _eikonDataRepository.fmGetAll();
        }
        public async Task<Frhso000Dto> GetByIdFrhso000Dtos(int id)
        {
            var api = new Frhso000Client(_apiBaseUrl, _httpClient);
            return (Frhso000Dto)await api.Frhso000GETAsync(id);
            //return await _eikonDataRepository.fmGetById(id);
        }
        

        public async Task<bool> InsertFrhso000Dto(Frhso000Dto Frhso)
        {
            var api = new Frhso000Client(_apiBaseUrl, _httpClient);
            try
            {
               await api.Frhso000POSTAsync(Frhso);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> UpdateFrhso000Dto(Frhso000Dto Frhso)
        {
            var api = new Frhso000Client(_apiBaseUrl, _httpClient);
            try
            {
                // await api.;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> DeleteFrhso000Dto(int id)
        {
            var api = new Frhso000Client(_apiBaseUrl, _httpClient);
            try
            {
                // await api.;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        // Acceso a datos Frhso000 Fin

        //experiencia laboral Inicio
        public async Task<List<Frhtr000Dto>> GetFrhtr000Dtos()
        {
            var api = new Frhtr000Client(_apiBaseUrl, _httpClient);
            return (List<Frhtr000Dto>)await api.Frhtr000AllAsync();
        }
        public async Task<List<Frhtr000Dto>> GetFrhtr000DtosBySolnume(string solnume)
        {
            var api = new Frhtr000Client(_apiBaseUrl, _httpClient);
            return (List<Frhtr000Dto>)await api.Frhtr000BySolnumeAsync(solnume);
        }

        public async Task<List<Frhsf000Dto>> GetFrhsf000Dtos()
        {
            var api = new Frhsf000Client(_apiBaseUrl, _httpClient);
            return (List<Frhsf000Dto>)await api.Frhsf000AllAsync();
        }
        public async Task<List<Frhsf000Dto>> GetFrhsf000DtosBySolnume(string solnume)
        {
            var api = new Frhsf000Client(_apiBaseUrl, _httpClient);
            return (List<Frhsf000Dto>)await api.Frhsf000BySolnumeAsync(solnume);
        }

        public async Task<List<Frhse000Dto>> GetFrhse000Dtos()
        {
            var api = new Frhse000Client(_apiBaseUrl, _httpClient);
            return (List<Frhse000Dto>)await api.Frhse000AllAsync();
        }
        public async Task<List<Frhse000Dto>> GetFrhse000DtosBySolnume(string solnume)
        {
            var api = new Frhse000Client(_apiBaseUrl, _httpClient);
            return (List<Frhse000Dto>)await api.Frhse000BySolnumeAsync(solnume);
        }

        // Enviar correo inicio
        public async Task<SendEmailRequest> EnviarCorreo(SendEmailRequest sendEmailRequest)
        {
            var api = new AuthenticationClient (_apiBaseUrl, _httpClient);
            await api.SendMailAsync(sendEmailRequest);
            return sendEmailRequest;
            //return await _eikonDataRepository.sendEmailRequest(sendEmailRequest);
        }
        // Enviar correo fin

        // Acceso a datos Frhpu010 Inicio
        /**
        Task<IEnumerable<Frhpu010Dto>> puGetAllAsync(string token);
        Task<Frhpu010Dto> puGetByReqnumeAsync(string reqnume, string token);
        Task<Frhpu010Dto> puGetByIdAsync(int id, string token);
        Task<Frhpu010Dto> puGetByJoNumberAsync(string jonumber, string token);
        Task<Frhpu010Dto> puGetByPuenumeAsync(string puenume, string token);
        Task<IEnumerable<SelectListItem>> puGetSelectListAsync(string token);
        Task<bool> puAddAsync(Frhpu010Dto dto, string token);
        Task<bool> puUpdateAsync(int id, Frhpu010Dto dto, string token);
        Task<bool> puDeleteAsync(int id, string token);
        
         **/
        public async Task<List<Frhpu010Dto>> GetFrhpu010Dtos()
        {

            var api = new Frhpu010Client(_apiBaseUrl, _httpClient);
            return (List<Frhpu010Dto>)await api.Frhpu010AllAsync();
        }
        public async Task<Frhpu010Dto> GetByIdFrhpu010Dtos(int id)
        {
            //return await _eikonDataRepository.puGetByIdAsync(id, GetToken());
            var api = new Frhpu010Client(_apiBaseUrl, _httpClient);
            return (Frhpu010Dto)await api.Frhpu010ByIdAsync(id);
        }

        public async Task<Frhpu010Dto> puGetByReqnumeFrhpu010Dtos(string reqnumber)
        {
            //return await _eikonDataRepository.puGetByReqnumeAsync(reqnumber, GetToken());
            var api = new Frhpu010Client(_apiBaseUrl, _httpClient);
            return (Frhpu010Dto)await api.Frhpu010ByReqnumeAsync(reqnumber);
        }

        public async Task<Frhpu010Dto> puGetByPuenumeFrhpu010Dtos(string puenume)
        {
            //return await _eikonDataRepository.puGetByPuenumeAsync(puenume, GetToken());
            var api = new Frhpu010Client(_apiBaseUrl, _httpClient);
            return (Frhpu010Dto)await api.Frhpu010ByPuenumeAsync(puenume);
        }

        public async Task<Frhpu010Dto> puGetByJoNumberFrhpu010Dtos(string jonumber)
        {     //**revisar controlador, deberia devolver una lista de objetos
            //return await _eikonDataRepository.puGetByJoNumberAsync(jonumber, GetToken());
            var api = new Frhpu010Client(_apiBaseUrl, _httpClient);
            return (Frhpu010Dto)await api.Frhpu010ByJoNumberAsync(jonumber);
        }

        public async Task<bool> InsertFrhpu010Dto(Frhpu010Dto trhps)
        {
            var api = new Frhpu010Client(_apiBaseUrl, _httpClient);
            // return await _eikonDataRepository.puAddAsync(trhps,  GetToken());
            await api.Frhpu010POSTAsync(trhps);
            return true;    
        }
        public async Task<bool> UpdateFrhpu010Dto(Frhpu010Dto trhps)
        {
            // return await _eikonDataRepository.puUpdateAsync(trhps.IdentityColumn, trhps, GetToken());
            var api = new Frhpu010Client(_apiBaseUrl, _httpClient);
            await api.Frhpu010PUTAsync(trhps.IdentityColumn, trhps);
            return true;
        }
        public async Task<bool> puDeleteAsync(int id, string token)
        {
            var api = new Frhpu010Client(_apiBaseUrl, _httpClient);
		await api.Frhpu010DELETEAsync(id);
		return true;
        }
        // Acceso a datos Frhpu010 Fin



        // Acceso a datos Frhjo010Dto Inicio
        /**
        Task<List<Frhjo010DtoDto>> joGetAllAsync(string token);
        Task<Frhjo010Dto> joGetByCodigoAsync(string codigo, string token);
        Task<List<Frhjo010Dto>> joGetByDescriAsync(string descri, string token);
        Task<bool> joAddAsync(Frhjo010Dto entity, string token);
        Task<bool> joUpdateAsync(Frhjo010Dto entity, string token);
        Task<bool> joDeleteAsync(int id, string token);
        Task<bool> joExistsAsync(string codigo, string token);
        
         **/
        public async Task<List<Frhjo010Dto>> joGetAllAsync(string token)
        {
            
            var api = new Frhjo010Client(_apiBaseUrl, _httpClient);
            return (List<Frhjo010Dto>)await api.Frhjo010AllAsync();
        }
        public async Task<Frhjo010Dto> joGetByCodigoAsync(string codigo, string token)
        {
            var api = new Frhjo010Client(_apiBaseUrl, _httpClient);
            //return await _eikonDataRepository.joGetByCodigoAsync(codigo, token);
            return (Frhjo010Dto)await api.Frhjo010PorCodigoAsync(codigo);
        }

        public async Task<List<Frhjo010Dto>> joGetByDescriAsync(string descri, string token)
        {
            // return await _eikonDataRepository.joGetByDescriAsync(descri, token);
            var api = new Frhjo010Client(_apiBaseUrl, _httpClient);
            return (List<Frhjo010Dto>)await api.Frhjo010PorDescriAsync(descri);
        }
   
       public   async Task<bool> joAddAsync(Frhjo010Dto entity, string token)
        {
            //return await _eikonDataRepository.joAddAsync(entity,token);
            var api = new Frhjo010Client(_apiBaseUrl, _httpClient);
            await api.Frhjo010POSTAsync(entity);
            return true;
        }
        public async Task<bool> joUpdateAsync(Frhjo010Dto entity, string token)
        {
            //return await _eikonDataRepository.joUpdateAsync( entity, token);
            var api = new Frhjo010Client(_apiBaseUrl, _httpClient);
            await api.Frhjo010PUTAsync(entity.IdentityColumn, entity);
            return true;
        }
        public async Task<bool> joDeleteAsync(int id, string token)
        {
            var api = new Frhjo010Client(_apiBaseUrl, _httpClient);
		await api.Frhjo010DELETEAsync(id);
		return true;
        }
        // Acceso a datos Frhjo010Dto Fin

        // Acceso a datos Trhof010 Inicio
        //Trhof010 inicio
        /**
        Task<List<Trhof010Dto>> ofGetAllAsync(string token);
        Task<Trhof010Dto> ofGetByCodigoAsync(string codigo, string token);
        Task<List<Trhof010Dto>> ofGetByDescriAsync(string emcodigo, string token);
        Task<bool> ofAddAsync(Trhof010 entity, string token);
        Task<bool> ofUpdateAsync(Trhof010Dto entity, string token);
        Task<bool> ofDeleteAsync(int id, string token);
        **/
        public async Task<List<Trhem000Dto>> emGetAllAsync(string token)
        {

            var api = new Trhem000Client(_apiBaseUrl, _httpClient);
            return (List<Trhem000Dto>)await api.Trhem000AllAsync();// .Trhof010AllAsync();
        }
        public async Task<List<Trhof010Dto>> ofGetAllAsync(string token)
        {

            var api = new Trhof010Client(_apiBaseUrl, _httpClient);
            return (List<Trhof010Dto>)await api.Trhof010AllAsync();
        }
        public async Task<Trhof010Dto> ofGetByCodigoAsync(string codigo, string token)
        {
            var api = new Trhof010Client(_apiBaseUrl, _httpClient);
            return (Trhof010Dto)await api.Trhof010PorCodigoAsync(codigo);
        }

        public async Task<List<Trhof010Dto>> ofGetByEmCodigo(string emcodigo, string token)
        {   var api = new Trhof010Client(_apiBaseUrl, _httpClient);
            //return await _eikonDataRepository.ofGetByDescriAsync(emcodigo, token);
            return (List<Trhof010Dto>)await api.Trhof010AllAsync();
        }

        public async Task<List<Trhan000Dto>> anGetByEmCodigo(string emcodigo, string token)
        {
            var api = new Trhan000Client(_apiBaseUrl, _httpClient);
            //return await _eikonDataRepository.ofGetByDescriAsync(emcodigo, token);
            return (List<Trhan000Dto>)await api.Trhan000AllAsync();
        }

        public async Task<bool> ofAddAsync(Trhof010Dto entity, string token)
        {
            var api = new Trhof010Client(_apiBaseUrl, _httpClient);
             await api.Trhof010POSTAsync(entity);
            return true;
        }
        public async Task<bool> ofUpdateAsync(Trhof010Dto entity, string token)
        {
            var api = new Trhof010Client(_apiBaseUrl, _httpClient);
		    await api.Trhof010PUTAsync(entity.IdentityColumn,entity);
		    return true;
        }
        public async Task<bool> ofDeleteAsync(int id, string token)
        {
            var api = new Trhof010Client(_apiBaseUrl, _httpClient);
		await api.Trhof010DELETEAsync(id);
		return true;
        }

        //Trhof010 fin

        public async Task<bool> grAddAsync(Trhgr010Dto entity, string token)
        {
            var api = new Trhgr010Client(_apiBaseUrl, _httpClient);
            await api.Trhgr010POSTAsync(entity);
            return true;
        }

        public async Task<bool> grUpdateAsync(Trhgr010Dto entity, string token)
        {
            var api = new Trhgr010Client(_apiBaseUrl, _httpClient);
            await api.Trhgr010PUTAsync(entity.IdentityColumn, entity);
            return true;
        }

        public async Task<bool> grDeleteAsync(int id, string token)
        {
            var api = new Trhgr010Client(_apiBaseUrl, _httpClient);
            await api.Trhgr010DELETEAsync(id);
            return true;
        }

        //Trhek000
        public async Task<bool> ekAddAsync(Trhek000Dto entity, string token)
        {
            var api = new Trhek000Client(_apiBaseUrl, _httpClient);
            await api.Trhek000POSTAsync(entity);
            return true;
        }

        public async Task<bool> ekUpdateAsync(Trhek000Dto entity, string token)
        {
            var api = new Trhek000Client(_apiBaseUrl, _httpClient);
            await api.Trhek000PUTAsync(entity.EkId, entity);
            return true;
        }

        public async Task<bool> ekDeleteAsync(int id, string token)
        {
            var api = new Trhek000Client(_apiBaseUrl, _httpClient);
            await api.Trhek000DELETEAsync(id);
            return true;
        }


        //TRHem000
        public async Task<bool> emUpdateAsync(Trhem000Dto entity, string token)
        {
            var api = new Trhem000Client(_apiBaseUrl, _httpClient);
            await api.Trhem000PUTAsync(entity.IdentityColumn, entity);
            return true;
        }
        //TRHUB010 acceso a datos Inicio

        public async Task<List<Trhub010Dto>> ubGetAllAsync(string token)
        {

            var api = new Trhub010Client(_apiBaseUrl, _httpClient);
            return (List<Trhub010Dto>)await api.Trhub010AllAsync();
        }
        public async Task<Trhub010Dto> ubGetByCodigoAsync(string codigo, string token)
        {
            var api = new Trhub010Client(_apiBaseUrl, _httpClient);
            return (Trhub010Dto)await api.Trhub010PorCodigoAsync(codigo);
        }

        public async Task<List<Trhub010Dto>> ubGetByEmCodigo(string emcodigo, string token)
        {
            //** //** var api0 = new MockService<Trhub010Dto>();
            //** return await api0.GetAllAsync(9);
            //return await _eikonDataRepository.ubGetByDescriAsync(emcodigo, token);
            var api = new Trhub010Client(_apiBaseUrl, _httpClient);
            return (List<Trhub010Dto>)await api.ByEmCodigo5Async(emcodigo);
        }

        public async Task<bool> ubAddAsync(Trhub010Dto entity, string token)
        {
            var api = new Trhub010Client(_apiBaseUrl, _httpClient);
             await api.Trhub010POSTAsync(entity);
            return true;
        }
        public async Task<bool> ubUpdateAsync(Trhub010Dto entity, string token)
        {
            var api = new Trhub010Client(_apiBaseUrl, _httpClient);
		await api.Trhub010PUTAsync(entity.IdentityColumn,entity);
		return true;
        }
        public async Task<bool> ubDeleteAsync(int id, string token)
        {
            var api = new Trhub010Client(_apiBaseUrl, _httpClient);
		await api.Trhub010DELETEAsync(id);
		return true;
        }

        //Trhub010 fin

        //TRHNP010 acceso a datos Inicio

        public async Task<List<Trhnp010Dto>> npGetAllAsync(string token)
        {

            var api = new Trhnp010Client(_apiBaseUrl, _httpClient);
            return (List<Trhnp010Dto>)await api.Trhnp010AllAsync();
        }
        public async Task<Trhnp010Dto> npGetByCodigoAsync(string codigo, string token)
        {
            var api = new Trhnp010Client(_apiBaseUrl, _httpClient);
            return (Trhnp010Dto)await api.Trhnp010PorCodigoAsync(codigo);
        }

        public async Task<List<Trhnp010Dto>> npGetByEmCodigo(string emcodigo, string token)
        {
            //return await _eikonDataRepository.npGetByEmCodigoAsync(emcodigo, token);
            var api = new Trhnp010Client(_apiBaseUrl, _httpClient);
            return (List<Trhnp010Dto>)await api.ByEmCodigo3Async(emcodigo);
        }

       public async Task<bool> npAddAsync(Trhnp010Dto entity, string token)
        {
            var api = new Trhnp010Client(_apiBaseUrl, _httpClient);
             await api.Trhnp010POSTAsync(entity);
            return true;    
        }
       public async Task<bool> npUpdateAsync(Trhnp010Dto entity, string token)
        {
            var api = new Trhnp010Client(_apiBaseUrl, _httpClient);
		await api.Trhnp010PUTAsync(entity.IdentityColumn,entity);
		return true;
        }
       public async Task<bool> npDeleteAsync(int id, string token)
        {
            var api = new Trhnp010Client(_apiBaseUrl, _httpClient);
		await api.Trhnp010DELETEAsync(id);
		return true;
        }

        //Trhnp010 fin

        //TRHCT010 acceso a datos Inicio

        public async Task<List<Trhct010Dto>> ctGetAllAsync(string token)
        {

            var api = new Trhct010Client(_apiBaseUrl, _httpClient);
            return (List<Trhct010Dto>)await api.Trhct010AllAsync();
        }
        public async Task<Trhct010Dto> ctGetByCodigoAsync(string codigo, string token)
        {
            var api = new Trhct010Client(_apiBaseUrl, _httpClient);
        return (Trhct010Dto)await api.Trhct010PorCodigoAsync(codigo);
        }

        public async Task<List<Trhct010Dto>> ctGetByEmCodigo(string emcodigo, string token)
        {
            //return await _eikonDataRepository.ctGetByEmCodigoAsync(emcodigo, token);
            var api = new Trhct010Client(_apiBaseUrl, _httpClient);
            return (List<Trhct010Dto>)await api.ByEmCodigoAsync(emcodigo);
        }

        public async Task<bool> ctAddAsync(Trhct010Dto entity, string token)
        {
            var api = new Trhct010Client(_apiBaseUrl, _httpClient);
             await api.Trhct010POSTAsync(entity);
            return true;
        }
      public  async Task<bool> ctUpdateAsync(Trhct010Dto entity, string token)
        {
            var api = new Trhct010Client(_apiBaseUrl, _httpClient);
		await api.Trhct010PUTAsync(entity.IdentityColumn,entity);
		return true;
        }
       public async Task<bool> ctDeleteAsync(int id, string token)
        {
            var api = new Trhct010Client(_apiBaseUrl, _httpClient);
		await api.Trhct010DELETEAsync(id);
		return true;
        }

        //Trhct010 fin

        //TRHXX010 ini

        //TRHXX010 fin
        // Trhpt000 inicio
        public async Task<List<Trhpt000Dto>> ptGetAllAsync(string token)
        {
            var api = new Trhpt000Client(_apiBaseUrl, _httpClient);
            return (List<Trhpt000Dto>)await api.Trhpt000AllAsync();
            //return ret;
            //var api = new Trhccx010Client(_apiBaseUrl, _httpClient);
            //return (List<Trhccx010Dto>)await api.Trhccx010AllAsync();
        }
        // Trhpt000 fin

        // Trhpf000 inicio
        public async Task<List<Trhpf000Dto>> pfGetAllAsync(string token)
        {
            var api = new Trhpf000Client(_apiBaseUrl, _httpClient);
            return (List<Trhpf000Dto>)await api.Trhpf000AllAsync();
            //return ret;
            //var api = new Trhccx010Client(_apiBaseUrl, _httpClient);
            //return (List<Trhccx010Dto>)await api.Trhccx010AllAsync();
        }
        // Trhpf000 fin

        // Trhdo000 inicio
        public async Task<List<Trhdo000Dto>> doGetAllAsync(string token)
        {
            var api = new Trhdo000Client(_apiBaseUrl, _httpClient);
            return (List<Trhdo000Dto>)await api.Trhdo000AllAsync();
            //return ret;
            //var api = new Trhccx010Client(_apiBaseUrl, _httpClient);
            //return (List<Trhccx010Dto>)await api.Trhccx010AllAsync();
        }
        // Trhpf000 fin
        // Trhgn000 inicio
        public async Task<List<Trhgn000Dto>> gnGetAllAsync(string token)
        {
            var api = new Trhgn000Client(_apiBaseUrl, _httpClient);
            return (List<Trhgn000Dto>)await api.Trhgn000AllAsync();
            //return ret;
            //var api = new Trhccx010Client(_apiBaseUrl, _httpClient);
            //return (List<Trhccx010Dto>)await api.Trhccx010AllAsync();
        }
        // Trhgn000 fin

        //TRHCC010 acceso a datos Inicio

        public async Task<List<Trhcc010Dto>> ccGetAllAsync(string token)
        {
            var api = new Trhcc010Client(_apiBaseUrl, _httpClient);
             return (List<Trhcc010Dto>) await api.Trhcc010AllAsync();
            //return ret;
            //var api = new Trhccx010Client(_apiBaseUrl, _httpClient);
            //return (List<Trhccx010Dto>)await api.Trhccx010AllAsync();
        }
        public async Task<Trhcc010Dto> ccGetByCodigoAsync(int id, string token)
        {
            var api = new Trhcc010Client(_apiBaseUrl, _httpClient);
            return (Trhcc010Dto)await api.Trhcc010ByIdAsync(id); //Trhcc010PorCodigoAsync(codigo);
            //return await _eikonDataRepository.ccGetByCodigoAsync(codigo, token);
        }

        public async Task<List<Trhcc010Dto>> ccGetByEmCodigo(string emcodigo, string token)
        {
            var api = new Trhcc010Client(_apiBaseUrl, _httpClient);
            var datalist= (List<Trhcc010Dto>)await api.Trhcc010AllAsync();
            return datalist.Where(x => x.EmEmpresa == emcodigo).ToList();   
            // return await _eikonDataRepository.ccGetByEmCodigoAsync(emcodigo, token);
        }

        
        public async Task<List<Trhmt000Dto>> mtGetByEmCodigo(string emcodigo, string token)
        {
            var api = new Trhmt000Client(_apiBaseUrl, _httpClient);
            var datalist = (List<Trhmt000Dto>)await api.Trhmt000AllAsync();
            return datalist.ToList();
            // return await _eikonDataRepository.ccGetByEmCodigoAsync(emcodigo, token);
        }

        public async Task<bool> ccAddAsync(Trhcc010Dto entity, string token)
        {   var api = new Trhcc010Client(_apiBaseUrl, _httpClient);
            //Trhcc010Dto trhcc010Dto = new Trhcc010Dto
            //{
            //    CcCodigo = entity.CcCodigo,
            //    CcDescri = entity.CcDescri,
            //    CcEstatus = entity.CcEstatus,
            //    CcDesde = entity.CcDesde,
            //    CcHasta = entity.CcHasta,
            //    CcContabl = entity.CcContabl,
            //    CcCosto = entity.CcCosto,
            //    EmEmpresa = entity.EmEmpresa,
            //    ChangedBy = entity.ChangedBy,
            //    CreatedBy = entity.CreatedBy,
            //    CreatedOn = entity.CreatedOn,
            //    ChangedOn = entity.ChangedOn,

            //};
            await api.Trhcc010POSTAsync(entity);
            return true;
            // var api = new Trhccx000Client(_apiBaseUrl, _httpClient);
                //return await api.Postccx010Async(entity);
        }
        public async Task<bool> ccUpdateAsync(Trhcc010Dto entity, string token)
        {
            var api = new Trhcc010Client(_apiBaseUrl, _httpClient);
            //Trhcc010Dto trhcc010Dto = new Trhcc010Dto
            //{
            //    CcCodigo = entity.CcCodigo,
            //    CcDescri = entity.CcDescri,
            //    CcEstatus = entity.CcEstatus,
            //    CcDesde = entity.CcDesde,
            //    CcHasta = entity.CcHasta,
            //    CcContabl = entity.CcContabl,
            //    CcCosto = entity.CcCosto,
            //    EmEmpresa = entity.EmEmpresa,
            //    ChangedBy = entity.ChangedBy,
            //    CreatedBy = entity.CreatedBy,
            //    CreatedOn = entity.CreatedOn,
            //    ChangedOn = entity.ChangedOn,
            //};
            entity.CcEstatus = false;
            await api.Trhcc010PUTAsync(entity.IdentityColumn,entity);
            return true;
            //return await _eikonDataRepository.ccUpdateAsync(entity, token);
        }
        public async Task<bool> ccDeleteAsync(int id, string token)
        {
            var api = new Trhcc010Client(_apiBaseUrl, _httpClient);
            //bool ret = false;
             await api.Trhcc010DELETEAsync(id);
            return true;    
            //return await _eikonDataRepository.puDeleteAsync(id, token);
        }

        //Trhcc010 fin

        //TRHRW000 acceso a datos Inicio

        public async Task<List<Trhrw000Dto>> rwGetAllAsync(string token)
        {
            var api = new Trhrw000Client(_apiBaseUrl, _httpClient);
            return (List<Trhrw000Dto>)await api.Trhrw000AllAsync();
        }
        public async Task<Trhrw000Dto> rwGetByCodigoAsync(string codigo, string token)
        {
            var api = new Trhrw000Client(_apiBaseUrl, _httpClient);
            return (Trhrw000Dto) await api.Trhrw000PorCodigoAsync(codigo);
        }
        //verificar este metodo en WebApi
        public async Task<List<Trhrw000Dto>> rwGetByEmCodigo(string emcodigo, string token)
        {
            var api = new Trhrw000Client(_apiBaseUrl, _httpClient);
            return (List<Trhrw000Dto>) await api.Trhrw000SelectListAsync(emcodigo);
        }

        public async Task<bool> rwAddAsync(Trhrw000Dto entity, string token)
        {
            var api = new Trhrw000Client(_apiBaseUrl, _httpClient);
            //var api = new Trhrwx000Client(_apiBaseUrl, _httpClient);
                 await api.Trhrw000POSTAsync(entity);
            return true;
        }
        public async Task<bool> rwUpdateAsync(Trhrw000Dto entity, string token)
        {
            var api = new Trhrw000Client(_apiBaseUrl, _httpClient);
            //var api = new Trhrw000Client(_apiBaseUrl, _httpClient);
		        await api.Trhrw000PUTAsync(entity.IdentityColumn,entity);
		    return true;
        }
        public async Task<bool> rwDeleteAsync(int id, string token)
        {
            var api = new Trhrw000Client(_apiBaseUrl, _httpClient);
            //var api = new Trhpux010Client(_apiBaseUrl, _httpClient);
		    await api.Trhrw000DELETEAsync(id);
		return true;
        }

        //Trhrw000 fin
        //TRHpr000 acceso a datos Inicio

        public async Task<List<Trhpr000Dto>> prGetAllAsync(string token)
        {

            var api = new Trhpr000Client(_apiBaseUrl, _httpClient);
            return (List<Trhpr000Dto>)await api.Trhpr000AllAsync();
        }
        public async Task<Trhpr000Dto> prGetByCodigoAsync(string codigo, string token)
        {
            var api = new Trhpr000Client(_apiBaseUrl, _httpClient);
            return (Trhpr000Dto)await api.Trhpr000PorCodigoAsync(codigo);
        }
        //revisar webapi

        public async Task<bool> prAddAsync(Trhpr000Dto entity, string token)
        {
            var api = new Trhpr000Client(_apiBaseUrl, _httpClient);
                await api.Trhpr000POSTAsync(entity);
            return true;
        }
        public async Task<bool> prUpdateAsync(Trhpr000Dto entity, string token)
        {
            var api = new Trhpr000Client(_apiBaseUrl, _httpClient);
		await api.Trhpr000PUTAsync(entity.IdentityColumn,entity);
		return true;
        }
        public async Task<bool> prDeleteAsync(int id, string token)
        {
            var api = new Trhpr000Client(_apiBaseUrl, _httpClient);
		await api.Trhpr000DELETEAsync(id);
		return true;
        }

        //Trhpr000 fin

        // * Get trhxx000 all inicio
        public async Task<List<Trhaa000Dto>> Trhaa000DtoGetAll(string token)
        {
            //** //** var api0 = new MockService<Trhaa000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhaa000Client(_apiBaseUrl, _httpClient);
            return (List<Trhaa000Dto>)await api.Trhaa000AllAsync(); 
        }

        public async Task<List<Trhac000Dto>> Trhac000DtoGetAll(string token)
        {
            //** //** var api0 = new MockService<Trhac000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhac000Client(_apiBaseUrl, _httpClient);
            return (List<Trhac000Dto>)await api.Trhac000AllAsync(); 
        }

        public async Task<List<Trhaf000Dto>> Trhaf000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhaf000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhaf000Client(_apiBaseUrl, _httpClient);
            return (List<Trhaf000Dto>)await api.Trhaf000AllAsync(); 
        }

        public async Task<List<Trhal000Dto>> Trhal000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhal000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhal000Client(_apiBaseUrl, _httpClient);
            return (List<Trhal000Dto>)await api.Trhal000AllAsync(); 
        }

        public async Task<List<Trham000Dto>> Trham000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trham000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trham000Client(_apiBaseUrl, _httpClient);
            return (List<Trham000Dto>)await api.Trham000AllAsync(); 
        }

        public async Task<List<Trhan000Dto>> Trhan000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhan000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhan000Client(_apiBaseUrl, _httpClient);
            return (List<Trhan000Dto>)await api.Trhan000AllAsync(); 
        }

        public async Task<List<Trhap000Dto>> Trhap000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhap000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhap000Client(_apiBaseUrl, _httpClient);
            return (List<Trhap000Dto>)await api.Trhap000AllAsync(); 
        }

        public async Task<List<Trhas000Dto>> Trhas000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhas000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhas000Client(_apiBaseUrl, _httpClient);
            return (List<Trhas000Dto>)await api.Trhas000AllAsync(); 
        }

        public async Task<List<Trhat000Dto>> Trhat000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhat000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhat000Client(_apiBaseUrl, _httpClient);
            return (List<Trhat000Dto>)await api.Trhat000AllAsync(); 
        }

        public async Task<List<Trhau000Dto>> Trhau000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhau000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhau000Client(_apiBaseUrl, _httpClient);
            return (List<Trhau000Dto>)await api.Trhau000AllAsync(); 
        }

        public async Task<List<Trhaw000Dto>> Trhaw000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhaw000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhaw000Client(_apiBaseUrl, _httpClient);
            return (List<Trhaw000Dto>)await api.Trhaw000AllAsync(); 
        }

        public async Task<List<Trhax000Dto>> Trhax000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhax000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhax000Client(_apiBaseUrl, _httpClient);
            return (List<Trhax000Dto>)await api.Trhax000AllAsync(); 
        }

        public async Task<List<Trhba000Dto>> Trhba000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhba000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhba000Client(_apiBaseUrl, _httpClient);
            return (List<Trhba000Dto>)await api.Trhba000AllAsync(); 
        }

        public async Task<List<Trhbc000Dto>> Trhbc000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhbc000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhbc000Client(_apiBaseUrl, _httpClient);
            return (List<Trhbc000Dto>)await api.Trhbc000AllAsync(); 
        }

        public async Task<List<Trhbe000Dto>> Trhbe000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhbe000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhbe000Client(_apiBaseUrl, _httpClient);
            return (List<Trhbe000Dto>)await api.Trhbe000AllAsync(); 
        }

        public async Task<List<Trhbf000Dto>> Trhbf000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhbf000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhbf000Client(_apiBaseUrl, _httpClient);
            return (List<Trhbf000Dto>)await api.Trhbf000AllAsync(); 
        }

        public async Task<List<Trhbg000Dto>> Trhbg000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhbg000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhbg000Client(_apiBaseUrl, _httpClient);
            return (List<Trhbg000Dto>)await api.Trhbg000AllAsync(); 
        }

        public async Task<List<Trhbk000Dto>> Trhbk000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhbk000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhbk000Client(_apiBaseUrl, _httpClient);
            return (List<Trhbk000Dto>)await api.Trhbk000AllAsync(); 
        }

        public async Task<List<Trhbl000Dto>> Trhbl000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhbl000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhbl000Client(_apiBaseUrl, _httpClient);
            return (List<Trhbl000Dto>)await api.Trhbl000AllAsync(); 
        }

        public async Task<List<Trhbq000Dto>> Trhbq000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhbq000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhbq000Client(_apiBaseUrl, _httpClient);
            return (List<Trhbq000Dto>)await api.Trhbq000AllAsync(); 
        }

        public async Task<List<Trhca000Dto>> Trhca000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhca000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhca000Client(_apiBaseUrl, _httpClient);
            return (List<Trhca000Dto>)await api.Trhca000AllAsync(); 
        }

        public async Task<List<Trhce000Dto>> Trhce000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhce000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhce000Client(_apiBaseUrl, _httpClient);
            return (List<Trhce000Dto>)await api.Trhce000AllAsync(); 
        }

        public async Task<List<Trhcf000Dto>> Trhcf000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhcf000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhcf000Client(_apiBaseUrl, _httpClient);
            return (List<Trhcf000Dto>)await api.Trhcf000AllAsync(); 
        }

        public async Task<List<Trhcg000Dto>> Trhcg000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhcg000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhcg000Client(_apiBaseUrl, _httpClient);
            return (List<Trhcg000Dto>)await api.Trhcg000AllAsync(); 
        }

        public async Task<List<Trhch000Dto>> Trhch000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhch000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhch000Client(_apiBaseUrl, _httpClient);
            return (List<Trhch000Dto>)await api.Trhch000AllAsync(); 
        }

        public async Task<List<Trhci000Dto>> Trhci000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhci000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhci000Client(_apiBaseUrl, _httpClient);
            return (List<Trhci000Dto>)await api.Trhci000AllAsync(); 
        }

        public async Task<List<Trhcj000Dto>> Trhcj000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhcj000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhcj000Client(_apiBaseUrl, _httpClient);
            return (List<Trhcj000Dto>)await api.Trhcj000AllAsync(); 
        }

        public async Task<List<Trhck000Dto>> Trhck000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhck000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhck000Client(_apiBaseUrl, _httpClient);
            return (List<Trhck000Dto>)await api.Trhck000AllAsync(); 
        }

        public async Task<List<Trhcl000Dto>> Trhcl000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhcl000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhcl000Client(_apiBaseUrl, _httpClient);
            return (List<Trhcl000Dto>)await api.Trhcl000AllAsync(); 
        }
        public async Task<List<Trhco000Dto>> Trhco000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhco000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhco000Client(_apiBaseUrl, _httpClient);
            return (List<Trhco000Dto>)await api.Trhco000AllAsync(); 
        }

        public async Task<List<Trhcp000Dto>> Trhcp000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhcp000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhcp000Client(_apiBaseUrl, _httpClient);
            return (List<Trhcp000Dto>)await api.Trhcp000AllAsync(); 
        }

        public async Task<List<Trhcq000Dto>> Trhcq000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhcq000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhcq000Client(_apiBaseUrl, _httpClient);
            return (List<Trhcq000Dto>)await api.Trhcq000AllAsync(); 
        }

        public async Task<List<Trhcr000Dto>> Trhcr000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhcr000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhcr000Client(_apiBaseUrl, _httpClient);
            return (List<Trhcr000Dto>)await api.Trhcr000AllAsync(); 
        }

        public async Task<List<Trhcs000Dto>> Trhcs000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhcs000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhcs000Client(_apiBaseUrl, _httpClient);
            return (List<Trhcs000Dto>)await api.Trhcs000AllAsync(); 
        }

        public async Task<List<Trhcu000Dto>> Trhcu000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhcu000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhcu000Client(_apiBaseUrl, _httpClient);
            return (List<Trhcu000Dto>)await api.Trhcu000AllAsync(); 
        }

        public async Task<List<Trhcy000Dto>> Trhcy000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhcy000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhcy000Client(_apiBaseUrl, _httpClient);
            return (List<Trhcy000Dto>)await api.Trhcy000AllAsync(); 
        }

        public async Task<List<Trhcz000Dto>> Trhcz000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhcz000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhcz000Client(_apiBaseUrl, _httpClient);
            return (List<Trhcz000Dto>)await api.Trhcz000AllAsync(); 
        }

        public async Task<List<Trhda000Dto>> Trhda000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhda000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhda000Client(_apiBaseUrl, _httpClient);
            return (List<Trhda000Dto>)await api.Trhda000AllAsync(); 
        }

        public async Task<List<Trhdb000Dto>> Trhdb000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhdb000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhdb000Client(_apiBaseUrl, _httpClient);
            return (List<Trhdb000Dto>)await api.Trhdb000AllAsync(); 
        }

        public async Task<List<Trhdc000Dto>> Trhdc000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhdc000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhdc000Client(_apiBaseUrl, _httpClient);
            return (List<Trhdc000Dto>)await api.Trhdc000AllAsync(); 
        }

        public async Task<List<Trhdf000Dto>> Trhdf000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhdf000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhdf000Client(_apiBaseUrl, _httpClient);
            return (List<Trhdf000Dto>)await api.Trhdf000AllAsync(); 
        }

        public async Task<List<Trhdg000Dto>> Trhdg000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhdg000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhdg000Client(_apiBaseUrl, _httpClient);
            return (List<Trhdg000Dto>)await api.Trhdg000AllAsync(); 
        }

        public async Task<List<Trhdi000Dto>> Trhdi000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhdi000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhdi000Client(_apiBaseUrl, _httpClient);
            return (List<Trhdi000Dto>)await api.Trhdi000AllAsync(); 
        }

        public async Task<List<Trhdk000Dto>> Trhdk000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhdk000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhdk000Client(_apiBaseUrl, _httpClient);
            return (List<Trhdk000Dto>)await api.Trhdk000AllAsync(); 
        }

        public async Task<List<Trhdl000Dto>> Trhdl000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhdl000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhdl000Client(_apiBaseUrl, _httpClient);
            return (List<Trhdl000Dto>)await api.Trhdl000AllAsync(); 
        }

        public async Task<List<Trhdm000Dto>> Trhdm000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhdm000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhdm000Client(_apiBaseUrl, _httpClient);
            return (List<Trhdm000Dto>)await api.Trhdm000AllAsync(); 
        }

        public async Task<List<Trhdo000Dto>> Trhdo000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhdo000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhdo000Client(_apiBaseUrl, _httpClient);
            return (List<Trhdo000Dto>)await api.Trhdo000AllAsync(); 
        }

        public async Task<List<Trhdp000Dto>> Trhdp000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhdp000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhdp000Client(_apiBaseUrl, _httpClient);
            return (List<Trhdp000Dto>)await api.Trhdp000AllAsync(); 
        }

        public async Task<List<Trhdr000Dto>> Trhdr000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhdr000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhdr000Client(_apiBaseUrl, _httpClient);
            return (List<Trhdr000Dto>)await api.Trhdr000AllAsync(); 
        }

        public async Task<List<Trhdt000Dto>> Trhdt000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhdt000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhdt000Client(_apiBaseUrl, _httpClient);
            return (List<Trhdt000Dto>)await api.Trhdt000AllAsync(); 
        }

        public async Task<List<Trhdv000Dto>> Trhdv000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhdv000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhdv000Client(_apiBaseUrl, _httpClient);
            return (List<Trhdv000Dto>)await api.Trhdv000AllAsync(); 
        }

        public async Task<List<Trhed000Dto>> Trhed000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhed000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhed000Client(_apiBaseUrl, _httpClient);
            return (List<Trhed000Dto>)await api.Trhed000AllAsync(); 
        }

        public async Task<List<Trhee000Dto>> Trhee000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhee000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhee000Client(_apiBaseUrl, _httpClient);
            return (List<Trhee000Dto>)await api.Trhee000AllAsync(); 
        }

        public async Task<List<Trhef000Dto>> Trhef000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhef000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhef000Client(_apiBaseUrl, _httpClient);
            return (List<Trhef000Dto>)await api.Trhef000AllAsync(); 
        }

        public async Task<List<Trheh000Dto>> Trheh000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trheh000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trheh000Client(_apiBaseUrl, _httpClient);
            return (List<Trheh000Dto>)await api.Trheh000AllAsync(); 
        }

        public async Task<List<Trhem000Dto>> Trhem000DtoGetAll(string token)
        {
            ////** var api0 = new MockService<Trhem000Dto>();
            ////** return await api0.GetAllAsync(9);
            var api = new Trhem000Client(_apiBaseUrl, _httpClient);
            return (List<Trhem000Dto>)await api.Trhem000AllAsync(); 
        }

        public async Task<List<Trher000Dto>> Trher000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trher000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trher000Client(_apiBaseUrl, _httpClient);
            return (List<Trher000Dto>)await api.Trher000AllAsync(); 
        }

        public async Task<List<Trhtp000Dto>> Trhtp000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhtp000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhem000Client(_apiBaseUrl, _httpClient);
            return (List<Trhtp000Dto>)await api.Trhem000AllAsync();
        }
        public async Task<List<Trhes000Dto>> Trhes000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhes000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhes000Client(_apiBaseUrl, _httpClient);
            return (List<Trhes000Dto>)await api.Trhes000AllAsync(); 
        }
        public async Task<List<Trhew000Dto>> Trhew000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhew000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhew000Client(_apiBaseUrl, _httpClient);
            return (List<Trhew000Dto>)await api.Trhew000AllAsync(); 
        }

        public async Task<List<Trhex000Dto>> Trhex000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhex000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhex000Client(_apiBaseUrl, _httpClient);
            return (List<Trhex000Dto>)await api.Trhex000AllAsync(); 
        }

        public async Task<List<Trhfb000Dto>> Trhfb000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhfb000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhfb000Client(_apiBaseUrl, _httpClient);
            return (List<Trhfb000Dto>)await api.Trhfb000AllAsync(); 
        }

        public async Task<List<Trhfc000Dto>> Trhfc000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhfc000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhfc000Client(_apiBaseUrl, _httpClient);
            return (List<Trhfc000Dto>)await api.Trhfc000AllAsync(); 
        }

        public async Task<List<Trhfd000Dto>> Trhfd000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhfd000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhfd000Client(_apiBaseUrl, _httpClient);
            return (List<Trhfd000Dto>)await api.Trhfd000AllAsync(); 
        }

        public async Task<List<Trhfe000Dto>> Trhfe000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhfe000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhfe000Client(_apiBaseUrl, _httpClient);
            return (List<Trhfe000Dto>)await api.Trhfe000AllAsync(); 
        }

        public async Task<List<Trhfi000Dto>> Trhfi000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhfi000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhfi000Client(_apiBaseUrl, _httpClient);
            return (List<Trhfi000Dto>)await api.Trhfi000AllAsync(); 
        }

        public async Task<List<Trhfl000Dto>> Trhfl000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhfl000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhfl000Client(_apiBaseUrl, _httpClient);
            return (List<Trhfl000Dto>)await api.Trhfl000AllAsync(); 
        }

        public async Task<List<Trhfm000Dto>> Trhfm000DtoGetAll(string token)
        {
            ////** var api0 = new MockService<Trhfm000Dto>();
            ////** return await api0.GetAllAsync(9);
            var api = new Trhfm000Client(_apiBaseUrl, _httpClient);
            return (List<Trhfm000Dto>)await api.Trhfm000AllAsync(); 
        }
        public async Task<List<Trhfm000Dto>> Trhfm000DtoGetPorPaCodigo(string token,string paCodigo)
        {
            ////** var api0 = new MockService<Trhfm000Dto>();
            ////** return await api0.GetAllAsync(9);
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var api = new Trhfm000Client(_apiBaseUrl, _httpClient);
            return (List<Trhfm000Dto>)await api.Trhfm000PorPerfilAsync(paCodigo);
           // return lista.Where(pa=>pa.PaCodigo==paCodigo).ToList();
        }

        public async Task<List<Trhmp000Dto>> Trhmp000DtoGetPorPaCodigo(string token, string paCodigo)
        {
            ////** var api0 = new MockService<Trhmp000Dto>();
            ////** return await api0.GetAllAsync(9);
            var api = new Trhmp000Client(_apiBaseUrl, _httpClient);
            return (List<Trhmp000Dto>)await api.Trhmp000PorPaCodigoAsync(paCodigo);
            // return lista.Where(pa=>pa.PaCodigo==paCodigo).ToList();
        }


        public async Task<List<Trhfu000Dto>> Trhfu000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhfu000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhfu000Client(_apiBaseUrl, _httpClient);
            return (List<Trhfu000Dto>)await api.Trhfu000AllAsync(); 
        }

        public async Task<List<Trhfx000Dto>> Trhfx000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhfx000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhfx000Client(_apiBaseUrl, _httpClient);
            return (List<Trhfx000Dto>)await api.Trhfx000AllAsync(); 
        }

        public async Task<List<Trhgc000Dto>> Trhgc000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhgc000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhgc000Client(_apiBaseUrl, _httpClient);
            return (List<Trhgc000Dto>)await api.Trhgc000AllAsync(); 
        }

        public async Task<List<Trhge000Dto>> Trhge000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhge000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhge000Client(_apiBaseUrl, _httpClient);
            return (List<Trhge000Dto>)await api.Trhge000AllAsync(); 
        }

        public async Task<List<Trhgf000Dto>> Trhgf000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhgf000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhgf000Client(_apiBaseUrl, _httpClient);
            return (List<Trhgf000Dto>)await api.Trhgf000AllAsync(); 
        }

        public async Task<List<Trhgl000Dto>> Trhgl000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhgl000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhgl000Client(_apiBaseUrl, _httpClient);
            return (List<Trhgl000Dto>)await api.Trhgl000AllAsync(); 
        }

        public async Task<List<Trhgn000Dto>> Trhgn000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhgn000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhgn000Client(_apiBaseUrl, _httpClient);
            return (List<Trhgn000Dto>)await api.Trhgn000AllAsync(); 
        }

        public async Task<List<Trhgu000Dto>> Trhgu000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhgu000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhgu000Client(_apiBaseUrl, _httpClient);
            return (List<Trhgu000Dto>)await api.Trhgu000AllAsync(); 
        }

        public async Task<List<Trhgx000Dto>> Trhgx000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhgx000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhgx000Client(_apiBaseUrl, _httpClient);
            return (List<Trhgx000Dto>)await api.Trhgx000AllAsync(); 
        }

        public async Task<List<Trhhc000Dto>> Trhhc000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhhc000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhhc000Client(_apiBaseUrl, _httpClient);
            return (List<Trhhc000Dto>)await api.Trhhc000AllAsync(); 
        }

        public async Task<List<Trhhe000Dto>> Trhhe000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhhe000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhhe000Client(_apiBaseUrl, _httpClient);
            return (List<Trhhe000Dto>)await api.Trhhe000AllAsync(); 
        }

        public async Task<List<Trhhg000Dto>> Trhhg000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhhg000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhhg000Client(_apiBaseUrl, _httpClient);
            return (List<Trhhg000Dto>)await api.Trhhg000AllAsync(); 
        }

        public async Task<List<Trhhj000Dto>> Trhhj000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhhj000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhhj000Client(_apiBaseUrl, _httpClient);
            return (List<Trhhj000Dto>)await api.Trhhj000AllAsync(); 
        }

        public async Task<List<Trhhm000Dto>> Trhhm000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhhm000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhhm000Client(_apiBaseUrl, _httpClient);
            return (List<Trhhm000Dto>)await api.Trhhm000AllAsync(); 
        }

        public async Task<List<Trhho000Dto>> Trhho000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhho000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhho000Client(_apiBaseUrl, _httpClient);
            return (List<Trhho000Dto>)await api.Trhho000AllAsync(); 
        }

        public async Task<List<Trhhp000Dto>> Trhhp000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhhp000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhhp000Client(_apiBaseUrl, _httpClient);
            return (List<Trhhp000Dto>)await api.Trhhp000AllAsync(); 
        }

        public async Task<List<Trhhz000Dto>> Trhhz000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhhz000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhhz000Client(_apiBaseUrl, _httpClient);
            return (List<Trhhz000Dto>)await api.Trhhz000AllAsync(); 
        }

        public async Task<List<Trhia000Dto>> Trhia000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhia000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhia000Client(_apiBaseUrl, _httpClient);
            return (List<Trhia000Dto>)await api.Trhia000AllAsync(); 
        }

        public async Task<List<Trhic000Dto>> Trhic000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhic000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhic000Client(_apiBaseUrl, _httpClient);
            return (List<Trhic000Dto>)await api.Trhic000AllAsync(); 
        }

        public async Task<List<Trhid000Dto>> Trhid000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhid000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhid000Client(_apiBaseUrl, _httpClient);
            return (List<Trhid000Dto>)await api.Trhid000AllAsync(); 
        }
        public async Task<List<Trhih000Dto>> Trhih000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhih000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhih000Client(_apiBaseUrl, _httpClient);
            return (List<Trhih000Dto>)await api.Trhih000AllAsync(); 
        }

        public async Task<List<Trhij000Dto>> Trhij000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhij000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhij000Client(_apiBaseUrl, _httpClient);
            return (List<Trhij000Dto>)await api.Trhij000AllAsync(); 
        }

        public async Task<List<Trhim000Dto>> Trhim000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhim000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhim000Client(_apiBaseUrl, _httpClient);
            return (List<Trhim000Dto>)await api.Trhim000AllAsync(); 
        }

        public async Task<List<Trhin000Dto>> Trhin000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhin000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhin000Client(_apiBaseUrl, _httpClient);
            return (List<Trhin000Dto>)await api.Trhin000AllAsync(); 
        }

        public async Task<List<Trhip000Dto>> Trhip000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhip000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhip000Client(_apiBaseUrl, _httpClient);
            return (List<Trhip000Dto>)await api.Trhip000AllAsync(); 
        }

        public async Task<List<Trhis000Dto>> Trhis000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhis000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhis000Client(_apiBaseUrl, _httpClient);
            return (List<Trhis000Dto>)await api.Trhis000AllAsync(); 
        }

        public async Task<List<Trhit000Dto>> Trhit000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhit000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhit000Client(_apiBaseUrl, _httpClient);
            return (List<Trhit000Dto>)await api.Trhit000AllAsync(); 
        }

        public async Task<List<Trhja000Dto>> Trhja000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhja000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhja000Client(_apiBaseUrl, _httpClient);
            return (List<Trhja000Dto>)await api.Trhja000AllAsync(); 
        }

        public async Task<List<Trhjo000Dto>> Trhjo000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhjo000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhjo000Client(_apiBaseUrl, _httpClient);
            return (List<Trhjo000Dto>)await api.Trhjo000AllAsync(); 
        }

        public async Task<List<Trhke000Dto>> Trhke000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhke000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhke000Client(_apiBaseUrl, _httpClient);
            return (List<Trhke000Dto>)await api.Trhke000AllAsync(); 
        }

        public async Task<List<Trhkr000Dto>> Trhkr000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhkr000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhkr000Client(_apiBaseUrl, _httpClient);
            return (List<Trhkr000Dto>)await api.Trhkr000AllAsync(); 
        }

        public async Task<List<Trhku000Dto>> Trhku000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhku000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhku000Client(_apiBaseUrl, _httpClient);
            return (List<Trhku000Dto>)await api.Trhku000AllAsync(); 
        }

        public async Task<List<Trhlc000Dto>> Trhlc000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhlc000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhlc000Client(_apiBaseUrl, _httpClient);
            return (List<Trhlc000Dto>)await api.Trhlc000AllAsync(); 
        }

        public async Task<List<Trhlg000Dto>> Trhlg000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhlg000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhlg000Client(_apiBaseUrl, _httpClient);
            return (List<Trhlg000Dto>)await api.Trhlg000AllAsync(); 
        }

        public async Task<List<Trhli000Dto>> Trhli000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhli000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhli000Client(_apiBaseUrl, _httpClient);
            return (List<Trhli000Dto>)await api.Trhli000AllAsync(); 
        }

        public async Task<List<Trhlj000Dto>> Trhlj000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhlj000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhlj000Client(_apiBaseUrl, _httpClient);
            return (List<Trhlj000Dto>)await api.Trhlj000AllAsync(); 
        }

        public async Task<List<Trhlk000Dto>> Trhlk000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhlk000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhlk000Client(_apiBaseUrl, _httpClient);
            return (List<Trhlk000Dto>)await api.Trhlk000AllAsync(); 
        }

        public async Task<List<Trhlu000Dto>> Trhlu000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhlu000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhlu000Client(_apiBaseUrl, _httpClient);
            return (List<Trhlu000Dto>)await api.Trhlu000AllAsync(); 
        }

        public async Task<List<Trhma000Dto>> Trhma000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhma000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhma000Client(_apiBaseUrl, _httpClient);
            return (List<Trhma000Dto>)await api.Trhma000AllAsync(); 
        }

        public async Task<List<Trhmc000Dto>> Trhmc000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhmc000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhmc000Client(_apiBaseUrl, _httpClient);
            return (List<Trhmc000Dto>)await api.Trhmc000AllAsync(); 
        }

        public async Task<List<Trhmd000Dto>> Trhmd000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhmd000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhmd000Client(_apiBaseUrl, _httpClient);
            return (List<Trhmd000Dto>)await api.Trhmd000AllAsync(); 
        }

        public async Task<List<Trhme000Dto>> Trhme000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhme000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhme000Client(_apiBaseUrl, _httpClient);
            return (List<Trhme000Dto>)await api.Trhme000AllAsync(); 
        }

        public async Task<List<Trhmi000Dto>> Trhmi000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhmi000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhmi000Client(_apiBaseUrl, _httpClient);
            return (List<Trhmi000Dto>)await api.Trhmi000AllAsync(); 
        }

        public async Task<List<Trhml000Dto>> Trhml000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhml000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhml000Client(_apiBaseUrl, _httpClient);
            return (List<Trhml000Dto>)await api.Trhml000AllAsync(); 
        }

        public async Task<List<Trhmn000Dto>> Trhmn000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhmn000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhmn000Client(_apiBaseUrl, _httpClient);
            return (List<Trhmn000Dto>)await api.Trhmn000AllAsync(); 
        }

        public async Task<List<Trhms000Dto>> Trhms000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhms000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhms000Client(_apiBaseUrl, _httpClient);
            return (List<Trhms000Dto>)await api.Trhms000AllAsync(); 
        }

        public async Task<List<Trhmt000Dto>> Trhmt000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhmt000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhmt000Client(_apiBaseUrl, _httpClient);
            return (List<Trhmt000Dto>)await api.Trhmt000AllAsync(); 
        }

        public async Task<List<Trhmv000Dto>> Trhmv000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhmv000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhmv000Client(_apiBaseUrl, _httpClient);
            return (List<Trhmv000Dto>)await api.Trhmv000AllAsync(); 
        }

        public async Task<List<Trhnc000Dto>> Trhnc000DtoGetAll(string token)
        {
           // //** var api0 = new MockService<Trhnc000Dto>();
           // //** return await api0.GetAllAsync(9);
            var api = new Trhnc000Client(_apiBaseUrl, _httpClient);
            return (List<Trhnc000Dto>)await api.Trhnc000AllAsync(); 
        }

        public async Task<List<Trhne000Dto>> Trhne000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhne000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhne000Client(_apiBaseUrl, _httpClient);
            return (List<Trhne000Dto>)await api.Trhne000AllAsync(); 
        }

        public async Task<List<Trhni000Dto>> Trhni000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhni000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhni000Client(_apiBaseUrl, _httpClient);
            return (List<Trhni000Dto>)await api.Trhni000AllAsync(); 
        }

        public async Task<List<Trhnl000Dto>> Trhnl000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhnl000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhnl000Client(_apiBaseUrl, _httpClient);
            return (List<Trhnl000Dto>)await api.Trhnl000AllAsync(); 
        }

        public async Task<List<Trhnt000Dto>> Trhnt000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhnt000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhnt000Client(_apiBaseUrl, _httpClient);
            return (List<Trhnt000Dto>)await api.Trhnt000AllAsync(); 
        }

        public async Task<List<Trhnz000Dto>> Trhnz000DtoGetAll(string token)
        {
            ////** var api0 = new MockService<Trhnz000Dto>();
            ////** return await api0.GetAllAsync(9);
            var api = new Trhnz000Client(_apiBaseUrl, _httpClient);
            return (List<Trhnz000Dto>)await api.Trhnz000AllAsync(); 
        }

        public async Task<List<Trhob000Dto>> Trhob000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhob000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhob000Client(_apiBaseUrl, _httpClient);
            return (List<Trhob000Dto>)await api.Trhob000AllAsync(); 
        }

        public async Task<List<Trhoi000Dto>> Trhoi000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhoi000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhoi000Client(_apiBaseUrl, _httpClient);
            return (List<Trhoi000Dto>)await api.Trhoi000AllAsync(); 
        }

        public async Task<List<Trhol000Dto>> Trhol000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhol000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhol000Client(_apiBaseUrl, _httpClient);
            return (List<Trhol000Dto>)await api.Trhol000AllAsync(); 
        }

        public async Task<List<Trhos000Dto>> Trhos000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhos000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhos000Client(_apiBaseUrl, _httpClient);
            return (List<Trhos000Dto>)await api.Trhos000AllAsync(); 
        }

        public async Task<List<Trhpa000Dto>> Trhpa000DtoGetAll(string token)
        {
            ////** var api0 = new MockService<Trhpa000Dto>();
            ////** return await api0.GetAllAsync(9);
            var api = new Trhpa000Client(_apiBaseUrl, _httpClient);
            return (List<Trhpa000Dto>)await api.Trhpa000AllAsync(); 
        }

        public async Task<List<Trhpc000Dto>> Trhpc000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhpc000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhpc000Client(_apiBaseUrl, _httpClient);
            return (List<Trhpc000Dto>)await api.Trhpc000AllAsync(); 
        }

        public async Task<List<Trhpf000Dto>> Trhpf000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhpf000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhpf000Client(_apiBaseUrl, _httpClient);
            return (List<Trhpf000Dto>)await api.Trhpf000AllAsync(); 
        }

        public async Task<List<Trhpr000Dto>> Trhpr000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhpr000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhpr000Client(_apiBaseUrl, _httpClient);
            return (List<Trhpr000Dto>)await api.Trhpr000AllAsync(); 
        }

        public async Task<List<Trhps000Dto>> Trhps000DtoGetAll(string token)
        {
            ////** var api0 = new MockService<Trhps000Dto>();
            ////** return await api0.GetAllAsync(9);
            var api = new Trhps000Client(_apiBaseUrl, _httpClient);
            return (List<Trhps000Dto>)await api.Trhps000AllAsync(); 
        }

        public async Task<List<Trhpt000Dto>> Trhpt000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhpt000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhpt000Client(_apiBaseUrl, _httpClient);
            return (List<Trhpt000Dto>)await api.Trhpt000AllAsync(); 
        }

        public async Task<List<Trhpv000Dto>> Trhpv000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhpv000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhpv000Client(_apiBaseUrl, _httpClient);
            return (List<Trhpv000Dto>)await api.Trhpv000AllAsync(); 
        }

        public async Task<List<Trhrg000Dto>> Trhrg000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhrg000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhrg000Client(_apiBaseUrl, _httpClient);
            return (List<Trhrg000Dto>)await api.Trhrg000AllAsync(); 
        }

        public async Task<List<Trhrt000Dto>> Trhrt000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhrt000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhrt000Client(_apiBaseUrl, _httpClient);
            return (List<Trhrt000Dto>)await api.Trhrt000AllAsync(); 
        }

        public async Task<List<Trhrw000Dto>> Trhrw000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhrw000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhrw000Client(_apiBaseUrl, _httpClient);
            return (List<Trhrw000Dto>)await api.Trhrw000AllAsync(); 
        }

        public async Task<List<Trhsr000Dto>> Trhsr000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhsr000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhsr000Client(_apiBaseUrl, _httpClient);
            return (List<Trhsr000Dto>)await api.Trhsr000AllAsync(); 
        }

        public async Task<List<Trhst000Dto>> Trhst000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhst000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhst000Client(_apiBaseUrl, _httpClient);
            return (List<Trhst000Dto>)await api.Trhst000AllAsync(); 
        }

        public async Task<List<Trhsu000Dto>> Trhsu000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhsu000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhsu000Client(_apiBaseUrl, _httpClient);
            return (List<Trhsu000Dto>)await api.Trhsu000AllAsync(); 
        }

        public async Task<List<Trhsw000Dto>> Trhsw000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhsw000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhsw000Client(_apiBaseUrl, _httpClient);
            return (List<Trhsw000Dto>)await api.Trhsw000AllAsync(); 
        }

        public async Task<List<Trhti000Dto>> Trhti000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhti000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhti000Client(_apiBaseUrl, _httpClient);
            return (List<Trhti000Dto>)await api.Trhti000AllAsync(); 
        }

        public async Task<List<Trhto000Dto>> Trhto000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhto000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhto000Client(_apiBaseUrl, _httpClient);
            return (List<Trhto000Dto>)await api.Trhto000AllAsync(); 
        }

        public async Task<List<Trhts000Dto>> Trhts000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhts000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhts000Client(_apiBaseUrl, _httpClient);
            return (List<Trhts000Dto>)await api.Trhts000AllAsync(); 
        }

        public async Task<List<Trhtu000Dto>> Trhtu000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhtu000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhtu000Client(_apiBaseUrl, _httpClient);
            return (List<Trhtu000Dto>)await api.Trhtu000AllAsync(); 
        }

        public async Task<List<Trhuo000Dto>> Trhuo000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhuo000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhuo000Client(_apiBaseUrl, _httpClient);
            return (List<Trhuo000Dto>)await api.Trhuo000AllAsync(); 
        }
        public async Task<List<Trhhq000Dto>> Trhhq000DtoGetAll(string token)
        {
            var api = new Trhhq000Client(_apiBaseUrl, _httpClient);
            return (List<Trhhq000Dto>)await api.Trhhq000AllAsync();
        }
        public async Task<List<Trhqe000Dto>> Trhqe000DtoGetAll(string token)
        {
            var api = new Trhqe000Client(_apiBaseUrl, _httpClient);
            return (List<Trhqe000Dto>)await api.Trhqe000AllAsync();
        }
        public async Task<List<Trhud000Dto>> Trhud000DtoGetAll(string token)
        {
            var api = new Trhud000Client(_apiBaseUrl, _httpClient);
            return (List<Trhud000Dto>)await api.Trhud000AllAsync();
        }
        // * Get trhxx000 all fin

        // * Get trhxx010 all inicio
        public async Task<List<Trhcc010Dto>> Trhcc010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhcc010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhcc010Client(_apiBaseUrl, _httpClient);
            return (List<Trhcc010Dto>)await api.Trhcc010AllAsync(); 
        }

        public async Task<List<Trhcp010Dto>> Trhcp010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhcp010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhcp010Client(_apiBaseUrl, _httpClient);
            return (List<Trhcp010Dto>)await api.Trhcp010AllAsync(); 
        }

        public async Task<List<Trhct010Dto>> Trhct010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhct010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhct010Client(_apiBaseUrl, _httpClient);
            return (List<Trhct010Dto>)await api.Trhct010AllAsync(); 
        }
        public async Task<List<Trhtt000Dto>> Trhtt000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhct010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhtt000Client(_apiBaseUrl, _httpClient);
            return (List<Trhtt000Dto>)await api.Trhtt000AllAsync();
        }
        public async Task<List<Trhcv010Dto>> Trhcv010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhcv010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhcv010Client(_apiBaseUrl, _httpClient);
            return (List<Trhcv010Dto>)await api.Trhcv010AllAsync(); 
        }

        public async Task<List<Trhfs010Dto>> Trhfs010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhfs010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhfs010Client(_apiBaseUrl, _httpClient);
            return (List<Trhfs010Dto>)await api.Trhfs010AllAsync(); 
        }

        public async Task<List<Trhgr010Dto>> Trhgr010DtoGetAll(string token)
        {
            ////** var api0 = new MockService<Trhgr010Dto>();
            ////** return await api0.GetAllAsync(9);
            var api = new Trhgr010Client(_apiBaseUrl, _httpClient);
            return (List<Trhgr010Dto>)await api.Trhgr010AllAsync(); 
        }

        public async Task<List<Trhjs010Dto>> Trhjs010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhjs010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhjs010Client(_apiBaseUrl, _httpClient);
            return (List<Trhjs010Dto>)await api.Trhjs010AllAsync(); 
        }

        public async Task<List<Trhnp010Dto>> Trhnp010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhnp010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhnp010Client(_apiBaseUrl, _httpClient);
            return (List<Trhnp010Dto>)await api.Trhnp010AllAsync(); 
        }

        public async Task<List<Trhof010Dto>> Trhof010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhof010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhof010Client(_apiBaseUrl, _httpClient);
            return (List<Trhof010Dto>)await api.Trhof010AllAsync(); 
        }

        public async Task<List<Trhub010Dto>> Trhub010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhub010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhub010Client(_apiBaseUrl, _httpClient);
            return (List<Trhub010Dto>)await api.Trhub010AllAsync(); 
        }

        public async Task<List<Trhwb010Dto>> Trhwb010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhwb010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhwb010Client(_apiBaseUrl, _httpClient);
            return (List<Trhwb010Dto>)await api.Trhwb010AllAsync(); 
        }

        public async Task<List<Trhwc010Dto>> Trhwc010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhwc010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhwc010Client(_apiBaseUrl, _httpClient);
            return (List<Trhwc010Dto>)await api.Trhwc010AllAsync(); 
        }
        public async Task<List<Trhwc010Dto>> Trhwc010DtoPost(Trhwc010Dto wc)
        {
            //** var api0 = new MockService<Trhwc010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhwc010Client(_apiBaseUrl, _httpClient);
            await api.Trhwc010POSTAsync(wc);
            return (List<Trhwc010Dto>)await api.Trhwc010AllAsync();
        }
        public async Task<List<Trhwc010Dto>> Trhwc010DtoPut(Trhwc010Dto wc)
        {
            //** var api0 = new MockService<Trhwc010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhwc010Client(_apiBaseUrl, _httpClient);
            await api.Trhwc010PUTAsync(wc.IdentityColumn,wc.EmEmpresa,wc);
            return (List<Trhwc010Dto>)await api.Trhwc010AllAsync();
        }

        public async Task<List<Trhwg010Dto>> Trhwg010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Trhwg010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Trhwg010Client(_apiBaseUrl, _httpClient);
            return (List<Trhwg010Dto>)await api.Trhwg010Async(); 
        }

        public async Task<List<Frhad000Dto>> Frhad000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhad000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhad000Client(_apiBaseUrl, _httpClient);
            return (List<Frhad000Dto>)await api.Frhad000AllAsync(); 
        }

        public async Task<List<Frhbe000Dto>> Frhbe000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhbe000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhbe000Client(_apiBaseUrl, _httpClient);
            return (List<Frhbe000Dto>)await api.Frhbe000AllAsync(); 
        }

        public async Task<List<Frhde000Dto>> Frhde000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhde000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhde000Client(_apiBaseUrl, _httpClient);
            return (List<Frhde000Dto>)await api.Frhde000AllAsync(); 
        }

        public async Task<List<Frhdp000Dto>> Frhdp000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhdp000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhdp000Client(_apiBaseUrl, _httpClient);
            return (List<Frhdp000Dto>)await api.Frhdp000AllAsync(); 
        }

        public async Task<List<Frhef000Dto>> Frhef000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhef000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhef000Client(_apiBaseUrl, _httpClient);
            return (List<Frhef000Dto>)await api.Frhef000AllAsync(); 
        }

        public async Task<List<Frhim000Dto>> Frhim000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhim000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhim000Client(_apiBaseUrl, _httpClient);
            return (List<Frhim000Dto>)await api.Frhim000AllAsync(); 
        }

        public async Task<List<Frhse000Dto>> Frhse000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhse000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhse000Client(_apiBaseUrl, _httpClient);
            return (List<Frhse000Dto>)await api.Frhse000AllAsync(); 
        }

        public async Task<List<Frhsf000Dto>> Frhsf000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhsf000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhsf000Client(_apiBaseUrl, _httpClient);
            return (List<Frhsf000Dto>)await api.Frhsf000AllAsync(); 
        }

        public async Task<List<Frhso000Dto>> Frhso000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhso000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhso000Client(_apiBaseUrl, _httpClient);
            return (List<Frhso000Dto>)await api.Frhso000AllAsync(); 
        }

        public async Task<List<Frhsp000Dto>> Frhsp000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhsp000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhsp000Client(_apiBaseUrl, _httpClient);
            return (List<Frhsp000Dto>)await api.Frhsp000AllAsync(); 
        }

        public async Task<List<Frhsw000Dto>> Frhsw000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhsw000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhsw000Client(_apiBaseUrl, _httpClient);
            return (List<Frhsw000Dto>)await api.Frhsw000AllAsync(); 
        }

        public async Task<List<Frhtl000Dto>> Frhtl000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhtl000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhtl000Client(_apiBaseUrl, _httpClient);
            return (List<Frhtl000Dto>)await api.Frhtl000AllAsync(); 
        }

        public async Task<List<Frhtr000Dto>> Frhtr000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhtr000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhtr000Client(_apiBaseUrl, _httpClient);
            return (List<Frhtr000Dto>)await api.Frhtr000AllAsync(); 
        }

        public async Task<List<Frhtz000Dto>> Frhtz000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhtz000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhtz000Client(_apiBaseUrl, _httpClient);
            return (List<Frhtz000Dto>)await api.Frhtz000AllAsync(); 
        }

        public async Task<List<Frhue000Dto>> Frhue000DtoGetAll(string token)
        {
            ////** var api0 = new MockService<Frhue000Dto>();
            ////** return await api0.GetAllAsync(9);
            var api = new Frhue000Client(_apiBaseUrl, _httpClient);
            return (List<Frhue000Dto>)await api.Frhue000AllAsync(); 
        }
        public async Task<List<SeguridadDBDto>> Frhue000DtoGetDashBoard(string emCodigo)
        {
            ////** var api0 = new MockService<Frhue000Dto>();
            ////** return await api0.GetAllAsync(9);
            var api = new Frhue000Client(_apiBaseUrl, _httpClient);
            return (List<SeguridadDBDto>)await api.Frhue000RolesDBAsync("01");
        }

        public async Task<List<Frhvi000Dto>> Frhvi000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhvi000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhvi000Client(_apiBaseUrl, _httpClient);
            return (List<Frhvi000Dto>)await api.Frhvi000AllAsync(); 
        }

        public async Task<List<Frhpd000Dto>> Frhpd000DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhpd000Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhpd000Client(_apiBaseUrl, _httpClient);
            return (List<Frhpd000Dto>)await api.Frhpd000AllAsync(); 
        }
        // * Get trhxx010 all fin

        // * Get trhxx010 byEmpresa inicio
        public async Task<List<Trhcc010Dto>> Trhcc010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Trhcc010Dto>();
            //** return await api0.GetAllAsync(9);

             var api = new Trhcc010Client(_apiBaseUrl, _httpClient);
             var data = (List<Trhcc010Dto>)await api.Trhcc010AllAsync(); 
             return data.Where(em=>em.EmEmpresa==emEmpresa).ToList(); 
        }

        public async Task<List<Trhcp010Dto>> Trhcp010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Trhcp010Dto>();
            //** return await api0.GetAllAsync(9);

             var api = new Trhcp010Client(_apiBaseUrl, _httpClient);
             var data = (List<Trhcp010Dto>)await api.Trhcp010AllAsync(); 
             return data.Where(em=>em.EmEmpresa==emEmpresa).ToList(); 
        }

        public async Task<List<Trhct010Dto>> Trhct010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Trhct010Dto>();
            //** return await api0.GetAllAsync(9);

             var api = new Trhct010Client(_apiBaseUrl, _httpClient);
             var data = (List<Trhct010Dto>)await api.Trhct010AllAsync(); 
             return data.Where(em=>em.EmEmpresa==emEmpresa).ToList(); 
        }

        public async Task<List<Trhcv010Dto>> Trhcv010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Trhcv010Dto>();
            //** return await api0.GetAllAsync(9);

             var api = new Trhcv010Client(_apiBaseUrl, _httpClient);
             var data = (List<Trhcv010Dto>)await api.Trhcv010AllAsync(); 
             return data.Where(em=>em.EmEmpresa==emEmpresa).ToList(); 
        }

        public async Task<List<Trhfs010Dto>> Trhfs010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Trhfs010Dto>();
            //** return await api0.GetAllAsync(9);

             var api = new Trhfs010Client(_apiBaseUrl, _httpClient);
             var data = (List<Trhfs010Dto>)await api.Trhfs010AllAsync(); 
             return data.Where(em=>em.EmEmpresa==emEmpresa).ToList(); 
        }

        public async Task<List<Trhgr010Dto>> Trhgr010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Trhgr010Dto>();
            //** return await api0.GetAllAsync(9);

             var api = new Trhgr010Client(_apiBaseUrl, _httpClient);
             var data = (List<Trhgr010Dto>)await api.Trhgr010AllAsync(); 
             return data.Where(em=>em.EmEmpresa==emEmpresa).ToList(); 
        }

        public async Task<List<Trhjs010Dto>> Trhjs010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Trhjs010Dto>();
            //** return await api0.GetAllAsync(9);

             var api = new Trhjs010Client(_apiBaseUrl, _httpClient);
             var data = (List<Trhjs010Dto>)await api.Trhjs010AllAsync(); 
             return data.Where(em=>em.EmEmpresa==emEmpresa).ToList(); 
        }

        public async Task<List<Trhnp010Dto>> Trhnp010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Trhnp010Dto>();
            //** return await api0.GetAllAsync(9);

             var api = new Trhnp010Client(_apiBaseUrl, _httpClient);
             var data = (List<Trhnp010Dto>)await api.Trhnp010AllAsync(); 
             return data.Where(em=>em.EmEmpresa==emEmpresa).ToList(); 
        }

        public async Task<List<Trhof010Dto>> Trhof010DtoGetByEmpresa(string emEmpresa, string token)
        {
            ////** var api0 = new MockService<Trhof010Dto>();
            ////** return await api0.GetAllAsync(9);

             var api = new Trhof010Client(_apiBaseUrl, _httpClient);
             var data = (List<Trhof010Dto>)await api.Trhof010AllAsync(); 
             return data.Where(em=>em.EmEmpresa==emEmpresa).ToList(); 
        }

        public async Task<List<Trhub010Dto>> Trhub010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Trhub010Dto>();
            //** return await api0.GetAllAsync(9);

             var api = new Trhub010Client(_apiBaseUrl, _httpClient);
             var data = (List<Trhub010Dto>)await api.Trhub010AllAsync(); 
             return data.Where(em=>em.EmEmpresa==emEmpresa).ToList(); 
        }

        public async Task<List<Trhwb010Dto>> Trhwb010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Trhwb010Dto>();
            //** return await api0.GetAllAsync(9);

             var api = new Trhwb010Client(_apiBaseUrl, _httpClient);
             var data = (List<Trhwb010Dto>)await api.Trhwb010AllAsync(); 
             return data.Where(em=>em.EmEmpresa==emEmpresa).ToList(); 
        }

        public async Task<List<Trhwc010Dto>> Trhwc010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Trhwc010Dto>();
            //** return await api0.GetAllAsync(9);

             var api = new Trhwc010Client(_apiBaseUrl, _httpClient);
             var data = (List<Trhwc010Dto>)await api.Trhwc010AllAsync(); 
             return data.Where(em=>em.EmEmpresa==emEmpresa).ToList(); 
        }

        public async Task<List<Trhwg010Dto>> Trhwg010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Trhwg010Dto>();
            //** return await api0.GetAllAsync(9);

             var api = new Trhwg010Client(_apiBaseUrl, _httpClient);
             var data = (List<Trhwg010Dto>)await api.Trhwg010Async(); 
             return data.Where(em=>em.EmEmpresa==emEmpresa).ToList(); 
        }

        public async Task<List<Frhma010Dto>> Frhma010DtoGetAll(string token)
        {
            // //** var api0 = new MockService<Frhma010Dto>();
            // //** return await api0.GetAllAsync(9);
            var api = new Frhma010Client(_apiBaseUrl, _httpClient);
            return (List<Frhma010Dto>)await api.Frhma010Async();
        }

        public async Task<Vfrhma000ActivoDto> Frhma010DtoGetByMaenume(string token, string maenume)
        {
            // //** var api0 = new MockService<Frhma010Dto>();
            // //** return await api0.GetAllAsync(9);
            var api = new Frhma010Client(_apiBaseUrl, _httpClient);
           // return (Vfrhma000ActivoDto)await api.VFrhma000ActByMaenomiAsync;
            return (Vfrhma000ActivoDto)await api.VFrhma000ActByMaenumeAsync(maenume);
        }
        public async Task<List<Vfrhma000Activo>> Vfrhma000ActivoGetAll(string token)
        {
            
            var api = new  Vfrhma000ActivoClient (_apiBaseUrl, _httpClient);
            return (List<Vfrhma000Activo>)await api.Vfrhma000ActivoAsync();
        }



        public async Task<List<Frhbj010Dto>> Frhbj010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhbj010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhbj010Client(_apiBaseUrl, _httpClient);
            return (List<Frhbj010Dto>)await api.Frhbj010AllAsync(); 
        }

        public async Task<List<Frhdc010Dto>> Frhdc010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhdc010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhdc010Client(_apiBaseUrl, _httpClient);
            return (List<Frhdc010Dto>)await api.Frhdc010AllAsync(); 
        }

        public async Task<List<Frhdj010Dto>> Frhdj010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhdj010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhdj010Client(_apiBaseUrl, _httpClient);
            return (List<Frhdj010Dto>)await api.Frhdj010AllGETAsync(); 
        }

        public async Task<List<Frhef010Dto>> Frhef010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhef010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhef010Client(_apiBaseUrl, _httpClient);
            return (List<Frhef010Dto>)await api.Frhef010AllAsync(); 
        }

        public async Task<List<Frhjc010Dto>> Frhjc010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhjc010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhjc010Client(_apiBaseUrl, _httpClient);
            return (List<Frhjc010Dto>)await api.Frhjc010AllAsync(); 
        }

        public async Task<List<Frhjl010Dto>> Frhjl010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhjl010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhjl010Client(_apiBaseUrl, _httpClient);
            return (List<Frhjl010Dto>)await api.Frhjl010AllGETAsync(); 
        }

        public async Task<List<Frhjo010Dto>> Frhjo010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhjo010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhjo010Client(_apiBaseUrl, _httpClient);
            return (List<Frhjo010Dto>)await api.Frhjo010AllAsync(); 
        }

        public async Task<List<Frhjs010Dto>> Frhjs010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhjs010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhjs010Client(_apiBaseUrl, _httpClient);
            return (List<Frhjs010Dto>)await api.Frhjs010AllAsync(); 
        }

        public async Task<List<Frhlc010Dto>> Frhlc010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhlc010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhlc010Client(_apiBaseUrl, _httpClient);
            return (List<Frhlc010Dto>)await api.Frhlc010AllAsync(); 
        }

        public async Task<List<Frhlz010Dto>> Frhlz010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhlz010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhlz010Client(_apiBaseUrl, _httpClient);
            return (List<Frhlz010Dto>)await api.Frhlz010AllAsync(); 
        }
        //public async Task<List<Frhma010Dto>> Frhma010DtoGetAll(string token)
        //{
        //    //** var api0 = new MockService<Frhma010Dto>();
        //    //** return await api0.GetAllAsync(9);
        //    //**var api = new Frhma010Client(_apiBaseUrl, _httpClient);
        //    //**return (List<Frhma010Dto>)await api.Frhma010AllAsync(); 
        //}

        public async Task<List<Frhpu010Dto>> Frhpu010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhpu010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhpu010Client(_apiBaseUrl, _httpClient);
            return (List<Frhpu010Dto>)await api.Frhpu010AllAsync(); 
        }

        public async Task<List<Frhrq010Dto>> Frhrq010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhrq010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhrq010Client(_apiBaseUrl, _httpClient);
            return (List<Frhrq010Dto>)await api.Frhrq010AllAsync(); 
        }
        public async Task<bool> Frhrq010DtoDelete (string token, int id)
        {
            //** var api0 = new MockService<Frhrq010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhrq010Client(_apiBaseUrl, _httpClient);
            try
            {
                await api.Frhrq010DELETEAsync(id);
                return true;
            }
            catch(ApiException ae)
            {
                Console.WriteLine($"ApiException: {ae.Message}");
                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Failed to delete General Exception: {ex.Message}");
                return false;
            }
            
        }

        public async Task<List<Frhsf010Dto>> Frhsf010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhsf010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhsf010Client(_apiBaseUrl, _httpClient);
            return (List<Frhsf010Dto>)await api.Frhsf010AllAsync(); 
        }

        public async Task<List<Frhvi010Dto>> Frhvi010DtoGetAll(string token)
        {
            //** var api0 = new MockService<Frhvi010Dto>();
            //** return await api0.GetAllAsync(9);
            var api = new Frhvi010Client(_apiBaseUrl, _httpClient);
            return (List<Frhvi010Dto>)await api.Frhvi010AllAsync(); 
        }
        // * Get trhxx010 all fin
        // * Get Frhxx010 byEmpresa inicio
        public async Task<List<Frhrj010Dto>> Frhrj010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Frhrj010Dto>();
            //** return await api0.GetAllAsync(9);
            //** Revisar
            var api = new Frhrj010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhrj010Dto>)await api.Frhrj010All2Async();
            return data.Where(em => em.EmEmpresa == emEmpresa).ToList();
        }

        public async Task<List<Frhma010Dto>> Frhma010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Frhma010Dto>();
            //** return await api0.GetAllAsync(9);

            var api = new Frhma010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhma010Dto>)await api.Frhma010PorEmpresaAsync(emEmpresa);
            return data.Where(em => em.EmEmpresa == emEmpresa).ToList();
        }

        public async Task<List<Frhbj010Dto>> Frhbj010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Frhbj010Dto>();
            //** return await api0.GetAllAsync(9);

            var api = new Frhbj010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhbj010Dto>)await api.Frhbj010PorEmpresaAsync(emEmpresa);
            return data.Where(em => em.EmEmpresa == emEmpresa).ToList();
        }

        public async Task<List<Frhdc010Dto>> Frhdc010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Frhdc010Dto>();
            //** return await api0.GetAllAsync(9);
            //** Revisar
            var api = new Frhdc010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhdc010Dto>)await api.Frhdc010AllAsync();
            return data.Where(em => em.EmEmpresa == emEmpresa).ToList();
        }

        public async Task<List<Frhdj010Dto>> Frhdj010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Frhdj010Dto>();
            //** return await api0.GetAllAsync(9);

            var api = new Frhdj010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhdj010Dto>)await api.Frhdj010PorEmpresaAsync(emEmpresa);
            return data.Where(em => em.EmEmpresa == emEmpresa).ToList();
        }

        public async Task<List<Frhef010Dto>> Frhef010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Frhef010Dto>();
            //** return await api0.GetAllAsync(9);

            var api = new Frhef010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhef010Dto>)await api.Frhef010PorEmpresaAsync(emEmpresa);
            return data.Where(em => em.EmEmpresa == emEmpresa).ToList();
        }

        public async Task<List<Frhjc010Dto>> Frhjc010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Frhjc010Dto>();
            //** return await api0.GetAllAsync(9);

            var api = new Frhjc010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhjc010Dto>)await api.Frhjc010PorEmpresaAsync(emEmpresa);
            return data.Where(em => em.EmEmpresa == emEmpresa).ToList();
        }

        public async Task<List<Frhjl010Dto>> Frhjl010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Frhjl010Dto>();
            //** return await api0.GetAllAsync(9);

            var api = new Frhjl010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhjl010Dto>)await api.Frhjl010PorEmpresaAsync(emEmpresa);
            return data.Where(em => em.EmEmpresa == emEmpresa).ToList();
        }

        public async Task<List<Frhjo010Dto>> Frhjo010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Frhjo010Dto>();
            //** return await api0.GetAllAsync(9);

            var api = new Frhjo010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhjo010Dto>)await api.Frhjo010PorEmpresaAsync(emEmpresa);
            return data.Where(em => em.EmEmpresa == emEmpresa).ToList();
        }

        public async Task<List<Frhjs010Dto>> Frhjs010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Frhjs010Dto>();
            //** return await api0.GetAllAsync(9);

            var api = new Frhjs010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhjs010Dto>)await api.Frhjs010PorEmpresaAsync(emEmpresa);
            return data.Where(em => em.EmEmpresa == emEmpresa).ToList();
        }

        public async Task<List<Frhlc010Dto>> Frhlc010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Frhlc010Dto>();
            //** return await api0.GetAllAsync(9);

            var api = new Frhlc010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhlc010Dto>)await api.Frhlc010PorEmpresaAsync(emEmpresa);
            return data.Where(em => em.EmEmpresa == emEmpresa).ToList();
        }

        public async Task<List<Frhlz010Dto>> Frhlz010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Frhlz010Dto>();
            //** return await api0.GetAllAsync(9);

            var api = new Frhlz010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhlz010Dto>)await api.Frhlz010PorEmpresaAsync(emEmpresa);
            return data.Where(em => em.EmEmpresa == emEmpresa).ToList();
        }

        public async Task<List<Frhpu010Dto>> Frhpu010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Frhpu010Dto>();
            //** return await api0.GetAllAsync(9);

            var api = new Frhpu010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhpu010Dto>)await api.Frhpu010PorEmpresaAsync(emEmpresa);
            return data.Where(em => em.EmEmpresa == emEmpresa).ToList();
        }

        public async Task<List<Frhrq010Dto>> Frhrq010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Frhrq010Dto>();
            //** return await api0.GetAllAsync(9);

            var api = new Frhrq010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhrq010Dto>)await api.Frhrq010PorEmpresaAsync(emEmpresa);
            return data.Where(em => em.EmEmpresa == emEmpresa).ToList();
        }

        public async Task<List<Frhsf010Dto>> Frhsf010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Frhsf010Dto>();
            //** return await api0.GetAllAsync(9);

            var api = new Frhsf010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhsf010Dto>)await api.Frhsf010PorEmpresaAsync(emEmpresa);
            return data.Where(em => em.EmEmpresa == emEmpresa).ToList();
        }

        public async Task<List<Frhvi010Dto>> Frhvi010DtoGetByEmpresa(string emEmpresa, string token)
        {
            //** var api0 = new MockService<Frhvi010Dto>();
            //** return await api0.GetAllAsync(9);

            var api = new Frhvi010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhvi010Dto>)await api.Frhvi010PorEmpresaAsync(emEmpresa);
            return data.Where(em => em.EmEmpresa == emEmpresa).ToList();
        }
        public async Task<List<Frhwd010Dto>> Frhwd010DtoGetByEmpresa(string emEmpresa, string token)
        {
            ////** var api0 = new MockService<Frhvi010Dto>();
            ////** return await api0.GetAllAsync(9);

            var api = new Frhwd010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhwd010Dto>)await api.Frhwd010PorEmpresaAsync(emEmpresa);//Frhvi010PorEmpresaAsync(emEmpresa);
            return data.Where(em => em.EmEmpresa == emEmpresa).ToList();
        }
        public async Task<List<Frhwd010Dto>> Frhwd010DtoGetByEmpresaNomina(string emEmpresa,string wbCodigo, string token)
        {
            ////** var api0 = new MockService<Frhvi010Dto>();
            ////** return await api0.GetAllAsync(9);

            var api = new Frhwd010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhwd010Dto>)await api.Frhwd010PorEmpresaAsync(emEmpresa);//Frhvi010PorEmpresaAsync(emEmpresa);
            return data.Where(em => em.EmEmpresa == emEmpresa && em.WbCodigo==wbCodigo).ToList();
        }
        public async Task<List<Frhwe010Dto>> Frhwe010DtoGetByEmpresaNomina(string emEmpresa, string wbCodigo, string token)
        {
            ////** var api0 = new MockService<Frhvi010Dto>();
            ////** return await api0.GetAllAsync(9);

            var api = new Frhwe010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhwe010Dto>)await api.Frhwe010PorEmpresaAsync(emEmpresa);//Frhvi010PorEmpresaAsync(emEmpresa);
            return data.Where(em => em.EmEmpresa == emEmpresa && em.WbCodigo == wbCodigo).ToList();
        }
        public async Task<List<Frhwd010Dto>> Frhwd010DtoGetByEmpresaMaenume(string emEmpresa,string maenume, string wbCodigo, int wgAnoRef, int wgSecuen, string token)
        {
            ////** var api0 = new MockService<Frhvi010Dto>();
            ////** return await api0.GetAllAsync(9);

            var api = new Frhwd010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhwd010Dto>)await api.Frhwd010PorNominaMaenumeAsync( emEmpresa,wgAnoRef,wgSecuen,wbCodigo,maenume);//Frhvi010PorEmpresaAsync(emEmpresa);
            return data.ToList();
        }
        public async Task<List<Frhwe010Dto>> Frhwe010DtoGetByEmpresaMaenume(string emEmpresa, string maenume, string wbCodigo, int wgAnoRef, int wgSecuen, string token)
        {
            ////** var api0 = new MockService<Frhvi010Dto>();
            ////** return await api0.GetAllAsync(9);

            var api = new Frhwe010Client(_apiBaseUrl, _httpClient);
            var data = (List<Frhwe010Dto>)await api.Frhwe010PorNominaMaenumeAsync(emEmpresa, wgAnoRef, wgSecuen, wbCodigo, maenume);//Frhvi010PorEmpresaAsync(emEmpresa);
            return data.ToList();
        }
        public async Task<List<StatusNominaDto>> StatusNominaDtoPorNominaAsync(string emEmpresa, string wbCodigo, int wgAnoRef, int wgSecuen, string token)
        {
            ////** var api0 = new MockService<Frhvi010Dto>();
            ////** return await api0.GetAllAsync(9);

            var api = new StatusNominaClient(_apiBaseUrl, _httpClient);
            var data = (List<StatusNominaDto>)await api.StatusNominasPorNominaAsync(emEmpresa, wgAnoRef, wgSecuen, wbCodigo);//Frhvi010PorEmpresaAsync(emEmpresa);
            return data.ToList();
        }
        //* Get Frhxx010 byEmpresa fin
        //public async Task<List<Frhwd010>> ProcesarNominaAsync(List<CsrPreGenDto> transacciones)
        public async Task<List<Frhwd010>> PayrollProcessAsync(
        IEnumerable<CsrPreGenDto> transacciones,
        string emCodigo = "00",
        string ofCodigo = "000",
        string ubCodigo = "00000",
        string tipoEmpl = "F",
        string wbCodigo = "00001",
        int wgAnoRef = 0,
        int wgSecuen = 0,
        string maenomi="00000000",
        string token = "")
        {
            // Aquí puedes aplicar lógica de negocio si necesitas
            return await _eikonDataRepository.PayrollProcessAsync(transacciones,emCodigo,ofCodigo,ubCodigo,tipoEmpl,wbCodigo,wgAnoRef,wgSecuen,maenomi,token);
        }
    }
}