using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
//using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

//using System.IdentityModel.Claims
namespace EIKON.UI.Services
{
    public class AuthenticationService
    {
        private readonly IJSRuntime _jsRuntime;
        public string userId;
        public string role;
        public string tipo;
        public AuthenticationService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<string> GetTokenAsync()
        {
            // Si estás usando cookies, puedes obtener el token con JSInterop
            tkn = await _jsRuntime.InvokeAsync<string>("cookieInterop.getCookie", "AuthToken");
            //if (tkn == "")
            //{
            //    tkn = await _jsRuntime.InvokeAsync<string>("cookieInterop.getCookie", "AuthToken");
            //}
            // Alternativamente, si usas localStorage, puedes usar:
            // var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            SetToken(tkn);
            return tkn;
        }
        public string tkn="";
        private string? _token;
        private Dictionary<string, string>? _claims;

        public bool IsAuthenticated => _claims != null;
        public string Username => _claims?.GetValueOrDefault(ClaimTypes.NameIdentifier) ?? "Desconocido";
        public string Role => _claims?.GetValueOrDefault(ClaimTypes.Role) ?? "Sin Rol";
        public string emCodigo => _claims?.GetValueOrDefault(ClaimTypes.UserData) ?? "00";
        public void SetToken(string token)
        {
            _token = token;
            _claims = DecodeJwt(token);
        }
        public string GetNombreEmpresa()
        {
            //EikonDataServices eds = new EikonDataServices();

            return "";
        }
        private Dictionary<string, string> DecodeJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var dict = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
            //userId = dict.ContainsKey("nameid").ToString();
            dict.TryGetValue("nameid", out userId);
            dict.TryGetValue("role", out role);
            dict.TryGetValue("given_name", out tipo);
            //var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            //var  tipo = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;

            return dict; // jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
        }
    }

}

