using System.IO;
using System.Reflection;
using Host.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RigoFunc.Account;
using Swashbuckle.Swagger.Model;

namespace Host {
    public class Startup {
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.UseDefaultAccountService<ApplicationUser>();

            services.AddMvcCore().AddApiExplorer();

            var xmlDocPath = GetXmlDocPath(typeof(AccountController).GetTypeInfo());
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info {
                    Version = "v1",
                    Title = "Account Api",
                    Description = "Account Api documentation",
                    TermsOfService = "Account Api"
                });
                options.IncludeXmlComments(xmlDocPath);
                options.DescribeAllEnumsAsStrings();
                //options.OperationFilter(new Swashbuckle.SwaggerGen.XmlComments.ApplyXmlActionComments(pathToDoc));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseStaticFiles();

            //swagger
            app.UseSwagger();
            app.UseSwaggerUi();
        }

        private static string GetXmlDocPath(TypeInfo typeInfo) {
            var assembly = typeInfo.Assembly;
            var assemblyName = assembly.GetName();
            var assemblyPath = Path.GetDirectoryName(assembly.Location);
            var xmlDocPath = Path.Combine(assemblyPath, assemblyName.Name + ".xml");

            return xmlDocPath;
        }
    }
}
