using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using amazonCloneWebAPI.Models;
using System.Security.Claims;

namespace amazonCloneWebAPI.Handler
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        private readonly AmazonCloneContext _DBContext;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> option, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock, AmazonCloneContext dBContext) : base(option, logger, encoder, clock)
        {
            this._DBContext = dBContext;
        }
        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return (AuthenticateResult.Fail("No Header found"));
            var _headerValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var bytes = Convert.FromBase64String(_headerValue.Parameter);
            string credentials = Encoding.UTF8.GetString(bytes);
            if (!string.IsNullOrEmpty(credentials))
            {
                string[] array = credentials.Split(":");
                string username = array[0];
                string password = array[1];
                var user = this._DBContext.Users.FirstOrDefault(item => item.UsrUserName == username && item.UsrPassword == password);
                if(user == null)
                    return AuthenticateResult.Fail("UnAuthorized");

                var claim = new[]{new Claim(ClaimTypes.Name, username)};
                var identity= new ClaimsIdentity(claim, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
            }
            else{
                return AuthenticateResult.Fail("UnAuthorized");
            }
        }
    }
}