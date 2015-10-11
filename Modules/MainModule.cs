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
            this.Get["/"] = x =>
            {
                this.Model.index = new IndexModel();
                this.Model.index.HelloWorld = "HelloWorld!";
                return this.View["Index", this.Model];
            };

            this.Get["/login"] = x =>
            {
                this.Model.login = new LoginModel() { Error = this.Request.Query.error.HasValue, ReturnUrl = this.Request.Url };
                return this.View["Login", this.Model];
            };

            this.Post["/login"] = x =>
            {
                var userGuid = UserMapper.ValidateUser((string)this.Request.Form.Username, (string)this.Request.Form.Password);

                if (userGuid == null)
                {
                    return this.Context.GetRedirect("~/login?error=true&username=" + (string)this.Request.Form.Username);
                }

                DateTime? expiry = null;
                if (this.Request.Form.RememberMe.HasValue)
                {
                    expiry = DateTime.Now.AddDays(7);
                }

                return this.LoginAndRedirect(userGuid.Value, expiry);
            };

            this.Post["/logout"] = x =>
            {
                return this.LogoutAndRedirect("/");
            };

        }
    }
}
