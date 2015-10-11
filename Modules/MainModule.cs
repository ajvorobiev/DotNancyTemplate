namespace DotNancyTemplate.Modules
{
    using System;
    using Helpers;
    using Models;
    using Nancy.Authentication.Forms;
    using Nancy.Extensions;

    public class MainModule : BaseModule
    {
        public MainModule()
        {
            Get["/"] = x =>
            {
                Model.index = new IndexModel();
                Model.index.HelloWorld = "HelloWorld!";
                return View["Index", Model];
            };

            Get["/login"] = x =>
            {
                Model.login = new LoginModel() { Error = this.Request.Query.error.HasValue, ReturnUrl = this.Request.Url };
                return View["Login", Model];
            };

            Post["/login"] = x =>
            {
                var userGuid = UserMapper.ValidateUser((string)this.Request.Form.Username, (string)this.Request.Form.Password);

                if (userGuid == null)
                {
                    return Context.GetRedirect("~/login?error=true&username=" + (string)this.Request.Form.Username);
                }

                DateTime? expiry = null;
                if (this.Request.Form.RememberMe.HasValue)
                {
                    expiry = DateTime.Now.AddDays(7);
                }

                return this.LoginAndRedirect(userGuid.Value, expiry);
            };
            Post["/logout"] = x =>
            {
                return this.LogoutAndRedirect("/");
            };

        }
    }
}
