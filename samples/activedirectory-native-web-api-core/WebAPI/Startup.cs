using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using TokenValidatedContext = Microsoft.AspNetCore.Authentication.OpenIdConnect.TokenValidatedContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AuthenticationFailedContext = Microsoft.AspNetCore.Authentication.JwtBearer.AuthenticationFailedContext;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // Add Authentication scheme properties.
            services.AddAuthentication(options => {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            });


            // The Client ID is used by the application to uniquely identify itself to Azure AD.
            string clientId = Configuration["AzureAd:ClientId"];

            // RedirectUri is the URL where the user will be redirected to after they sign in.
            string redirectUri = Configuration["AzureAd:RedirectUri"];

            // Tenant is the tenant ID (e.g. contoso.onmicrosoft.com, or 'common' for multi-tenant)
            string tenant = Configuration["AzureAd:Tenant"];

            // Authority is the URL for authority, composed by Azure Active Directory v2 endpoint and the tenant name (e.g. https://login.microsoftonline.com/contoso.onmicrosoft.com/v2.0)
            string authority = String.Format(System.Globalization.CultureInfo.InvariantCulture,
                            Configuration["AzureAd:AadInstance"], tenant);

            //OpenID Connect (OIDC) Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(option =>
            {
                option.Authority = authority;
                option.Audience = clientId;
                option.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = OnAuthenticationFailed,
                    OnTokenValidated = OnTokenValidated
                };
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    // Instead of using the default validation (validating against
                    // a single issuer value, as we do in line of business apps), 
                    // we inject our own multitenant validation logic
                    ValidateIssuer = true,
                    ValidIssuers = new List<string>
                    {
                        "https://login.microsoftonline.com/0802107c-ed17-4b41-9f91-96250f0b54d4/v2.0"
                    },

                    // In this app, both API and client apps
                    // are represented using the same Application Id - we use
                    // the Application Id to represent the audience, or the
                    // intended recipient of tokens.
                    ValidAudience = clientId,
                    NameClaimType = "name"
                };
            });
            //.AddCookie()
            //.AddOpenIdConnect(options => {
            //    options.ClientId = clientId;
            //    options.Authority = authority;
            //    options.SignedOutRedirectUri = redirectUri;
            //    options.ResponseType = OpenIdConnectResponseType.IdToken;
            //    options.ResponseMode = OpenIdConnectResponseMode.Query;
            //    options.Events = new OpenIdConnectEvents
            //    {
            //        OnRemoteFailure = OnRemoteFailure,
            //        OnTokenValidated = OnTokenValidated
            //    };
            //    // ValidateIssuer set to false to allow personal and work accounts from any organization to sign in to your application
            //    // To only allow users from a single organizations, set ValidateIssuer to true and 'tenant' setting in appsettings.json to the tenant name
            //    // To allow users from only a list of specific organizations, set ValidateIssuer to true and use ValidIssuers parameter 
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        // Instead of using the default validation (validating against
            //        // a single issuer value, as we do in line of business apps), 
            //        // we inject our own multitenant validation logic
            //        ValidateIssuer = false,

            //        // In this app, both API and client apps
            //        // are represented using the same Application Id - we use
            //        // the Application Id to represent the audience, or the
            //        // intended recipient of tokens.
            //        ValidAudience = clientId,
            //        NameClaimType = "name"
            //    };
            //});


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
                //c.IncludeXmlComments(GetXmlCommentsPath());
            });

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Configure error handling middleware.
            app.UseExceptionHandler("/Home/Error");

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseCors(builder => builder
                .AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            // Configure MVC routes
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/v1/{controller}/{id?}");
            });


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

        }




        private Task OnAuthenticationFailed(AuthenticationFailedContext notification)
        {
            notification.Response.Redirect("/Home/Error");
            return Task.FromResult(0);
        }




        private Task OnTokenValidated(Microsoft.AspNetCore.Authentication.JwtBearer.TokenValidatedContext context)
        {
            // Example of how to validate for Microsoft Account + specific Azure AD tenant       
            // Replace this with your logic to validate the issuer/tenant
            // Retriever caller data from the incoming principal
            string issuer = context.SecurityToken.Issuer;
            string tenantID = context.Principal.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

            // Build a dictionary of approved tenants
            IEnumerable<string> approvedTenantIds = new List<string>
            {
                "0802107c-ed17-4b41-9f91-96250f0b54d4", //tppusers.onmicrosoft.com
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
