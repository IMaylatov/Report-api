using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SofTrust.Report.Business;
using SofTrust.Report.Business.Service.DataSet;
using SofTrust.Report.Business.Service.DataSource;
using SofTrust.Report.Business.Service.Report;
using SofTrust.Report.Business.Service.Report.ClosedXml;
using SofTrust.Report.Business.Service.Report.Malibu;

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

            services.AddScoped<DataSourceFactory, DataSourceFactory>();
            services.AddScoped<DataSetFactory, DataSetFactory>();

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
