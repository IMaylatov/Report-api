namespace SofTrust.Report.Api.Auth
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.EntityFrameworkCore.Internal;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    public class StHostAuthorizationHandler : AuthenticationHandler<StHostAuthOptions>
    {
        private readonly IServiceProvider serviceProvider;

        public StHostAuthorizationHandler(IOptionsMonitor<StHostAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
            IServiceProvider serviceProvider)
            : base(options, logger, encoder, clock)
        {
            this.serviceProvider = serviceProvider;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Context.Request.Headers.ContainsKey("Authorization"))
            {
                var authScheme = Context.Request.Headers["Authorization"].FirstOrDefault();
                if (authScheme.StartsWith("StHost"))
                {
                    var host = authScheme.Substring("StHost ".Length);

                    var claims = new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, "1") };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var authenticationTicket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(authenticationTicket);
                }
            }
            return AuthenticateResult.Fail("Authenticate host fail");
        }
    }
}
