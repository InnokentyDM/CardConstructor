using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security;
using Microsoft.Owin.Logging;

namespace WebApplication9
{
    public partial class Startup
    {

        //public void Configure(IAppBuilder app, ILoggerFactory loggerFactory)
        //{
        //    loggerFactory.AddConsole();
        //    app.Run(async (context) =>
        //    {
        //        // создаем объект логгера
        //        var logger = loggerFactory.CreateLogger("RequestInfoLogger");
        //        // пишем на консоль информацию
        //        logger.LogInformation("Processing request {0}", context.Request.Path);

        //        await context.Response.WriteAsync("Hello World!");
        //    });
        //}

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            // app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");
            app.UseVkontakteAuthentication(new Duke.Owin.VkontakteMiddleware.VkAuthenticationOptions()
            {
                AppId = "5539354",
                AppSecret = "BGyEyIukWrwycrE1IlhY"
            });

            var fdesc = new AuthenticationDescription();
            fdesc.Caption = "Google";
            fdesc.AuthenticationType = "Google";
            fdesc.Properties["Img"] = "<img>";
        


            app.UseFacebookAuthentication(
               appId: "299636750382790",
               appSecret: "6b680d40392f580f4415f115de0172f9");

            var gdesc = new AuthenticationDescription();
            gdesc.Caption = "Google";
            gdesc.AuthenticationType = "Google";
            gdesc.Properties["Img"] = "<img>";
            var googleOauth2Authentication = new GoogleOAuth2AuthenticationOptions()
            {
                Description = gdesc,
                ClientId = "590776490678-orlc0fur5hgdouhgd4rodf00qt9e6kau.apps.googleusercontent.com",
                ClientSecret = "aQvm8YcQ3BRxuyzMRYhnU4X9",
                Provider = new GoogleOAuth2AuthenticationProvider
                {
                    OnAuthenticated = async context =>
                    {
                        // Retrieve the OAuth access token to store for subsequent API calls
                        string accessToken = context.AccessToken;

                        // Retrieve the name of the user in Google
                        string googleName = context.Name;

                        // Retrieve the user's email address
                        string googleEmailAddress = context.Email;
                    }
                }
            };
            googleOauth2Authentication.Scope.Add("email");
            app.UseGoogleAuthentication(googleOauth2Authentication);

        }
    }
}