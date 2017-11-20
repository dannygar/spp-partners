using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace AdminPortal
{
    public class Startup
    {
        public static string clientId = "19157fc6-f0b5-452f-908f-fdc619d9226b";
        public static string clientSecret = "8isdrvm3hqRyHkAQvGRmJZa";
        private static string authority = "https://login.microsoftonline.com/common/v2.0";
        public static string redirectUri = "https://localhost:44399/";


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Authentication scheme properties.
            services.AddAuthentication(options => {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            });


            //OpenID Connect (OIDC) Authentication
            services.AddAuthentication(options => {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie()
                .AddOpenIdConnect(options => {
                    options.ClientId = clientId;
                    options.Authority = authority;
                    options.SignedOutRedirectUri = redirectUri;
                    options.ResponseType = OpenIdConnectResponseType.IdToken;
                    options.ResponseMode = OpenIdConnectResponseMode.FormPost;
                    options.Events = new OpenIdConnectEvents
                    {
                        OnAuthenticationFailed = OnAuthenticationFailed,
                        OnRemoteFailure = OnRemoteFailure,
                        OnTokenValidated = OnTokenValidated
                    };
                    // ValidateIssuer set to false to allow personal and work accounts from any organization to sign in to your application
                    // To only allow users from a single organizations, set ValidateIssuer to true and 'tenant' setting in appsettings.json to the tenant name
                    // To allow users from only a list of specific organizations, set ValidateIssuer to true and use ValidIssuers parameter 
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Instead of using the default validation (validating against
                        // a single issuer value, as we do in line of business apps), 
                        // we inject our own multitenant validation logic
                        ValidateIssuer = false,

                        // In this app, both API and client apps
                        // are represented using the same Application Id - we use
                        // the Application Id to represent the audience, or the
                        // intended recipient of tokens.
                        //ValidAudience = clientId,
                        NameClaimType = "name"
                    };
                });



            services.AddMvc();

            // Inject an implementation of ISwaggerProvider with defaulted settings applied
            services.AddSwaggerGen(c =>
            {
                //Add the detail information for the API.
                c.SwaggerDoc("v1", new Info
                {
                    Version = "1.0",
                    Title = "Sample Web API with AD v2",
                    Description = "Sample Web API with AD v2",
                    TermsOfService = "Contract",
                    Contact = new Contact { Name = "Copyright (c) Microsoft Corporation. All rights reserved.", Email = "", Url = "" },
                    License = new License { Name = "Licensed under the MIT license", Url = "http://License.txt" },
                    Extensions = { }
                });
                c.IncludeXmlComments(GetXmlCommentsPath());
            });

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCors(builder => builder
                .AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/swagger/{documentName}/swagger.json";
            });

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Web API ADv2");
                c.RoutePrefix = "api/swagger";
            });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }



        private Task OnAuthenticationFailed(AuthenticationFailedContext notification)
        {
            notification.HandleResponse();
            notification.Response.Redirect("/Home/Error");
            return Task.FromResult(0);
        }



        private Task OnTokenValidated(TokenValidatedContext context)
        {
            // Example of how to validate for Microsoft Account + specific Azure AD tenant       
            // Replace this with your logic to validate the issuer/tenant
            // Retriever caller data from the incoming principal
            string issuer = context.SecurityToken.Issuer;
            string subject = context.SecurityToken.Subject;
            string tenantID = context.Principal.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

            // Build a dictionary of approved tenants
            IEnumerable<string> approvedTenantIds = new List<string>
            {
                //"<Your tenantID>",
                "9188040d-6c67-4c5b-b112-36a304b66dad" // MSA Tenant
            };

            if (!approvedTenantIds.Contains(tenantID))
                throw new SecurityTokenValidationException();



            return Task.FromResult(0);
        }

        // Handle sign-in errors differently than generic errors.
        private Task OnRemoteFailure(RemoteFailureContext remoteFailureContext)
        {
            remoteFailureContext.HandleResponse();
            remoteFailureContext.Response.Redirect("/Home/Error?message=" + remoteFailureContext.Failure.Message);
            return Task.FromResult(0);
        }



        private string GetXmlCommentsPath()
        {
            return System.IO.Path.Combine("wwwroot", @"api.xml");
        }

    }
}
