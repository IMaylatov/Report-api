using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using SofTrust.Report.Api.Auth;
using SofTrust.Report.Core.Generator.DataReader;
using SofTrust.Report.Core.Generator.Report;
using SofTrust.Report.Core.Generator.Report.ClosedXml;
using SofTrust.Report.Core.Generator.Report.Malibu;
using SofTrust.Report.Core.Generator.Source;
using SofTrust.Report.Infrastructure;
using SofTrust.Report.Infrastructure.Repository;
using SofTrust.Report.Trs;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace SofTrust.Report.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // Показывать ошибки IdentityServer
            IdentityModelEventSource.ShowPII = true;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false).AddNewtonsoftJson();

            services.AddAuthentication()
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5001";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                })
                .AddScheme<StHostAuthOptions, StHostAuthorizationHandler>("StHost", "StHost Auth", o => { });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(
                        JwtBearerDefaults.AuthenticationScheme,
                        "StHost")
                    .Build();
            });

            services.AddEntityFrameworkNpgsql().AddDbContext<ReportContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("ReportConnection"))
                    .UseSnakeCaseNamingConvention());

            services.AddDbContext<TrsContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("TrsConnection")));

            services.AddScoped<ReportGeneratorFactory, ReportGeneratorFactory>();

            services.AddScoped<ClosedXmlReportGenerator, ClosedXmlReportGenerator>();
            services.AddScoped<MalibuReportGenerator, MalibuReportGenerator>();

            services.AddScoped<SourceFactory, SourceFactory>();
            services.AddScoped<DataReaderFactory, DataReaderFactory>();

            services.AddScoped<ReportRepository, ReportRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
