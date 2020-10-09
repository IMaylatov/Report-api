using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SofTrust.Report.Core.Generator.DataReader;
using SofTrust.Report.Core.Generator.Report;
using SofTrust.Report.Core.Generator.Report.ClosedXml;
using SofTrust.Report.Core.Generator.Report.Malibu;
using SofTrust.Report.Core.Generator.Source;
using SofTrust.Report.Infrastructure;
using SofTrust.Report.Infrastructure.Repository;

namespace SofTrust.Report.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false).AddNewtonsoftJson();

            services.AddEntityFrameworkNpgsql().AddDbContext<ReportContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("ReportConnection"))
                    .UseSnakeCaseNamingConvention());

            services.AddScoped<ReportGeneratorFactory, ReportGeneratorFactory>();

            services.AddScoped<ClosedXmlReportGenerator, ClosedXmlReportGenerator>();
            services.AddScoped<MalibuReportGenerator, MalibuReportGenerator>();

            services.AddScoped<SourceFactory, SourceFactory>();
            services.AddScoped<DataReaderFactory, DataReaderFactory>();

            services.AddScoped<ReportRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
