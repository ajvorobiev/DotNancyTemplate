namespace DotNancyTemplate.Modules
{
    using System;
    using System.Dynamic;
    using Helpers;
    using Models;
    using Nancy;

    public abstract class BaseModule : NancyModule
    {
        public dynamic Model = new ExpandoObject();

        public BaseModule()
        {
            this.SetupModelDefaults();
        }

        public BaseModule(string modulepath)
            : base(modulepath)
        {
            this.SetupModelDefaults();
        }

        private void SetupModelDefaults()
        {
            this.Before.AddItemToEndOfPipeline(ctx =>
            {
                this.Model.MasterPage = new MasterPageModel();

                var appTitle = AppSettingsManager.ReadSetting("appname");

                this.Model.MasterPage.Title = appTitle;
                this.Model.MasterPage.AppTitle = appTitle;

                if (this.Request.Cookies.ContainsKey("lastvisit"))
                {
                    this.Model.MasterPage.LastVisit = Uri.UnescapeDataString(this.Request.Cookies["lastvisit"]);
                }
                else
                {
                    this.Model.MasterPage.LastVisit = "No cookie value yet";
                }

                this.Model.MasterPage.IsAuthenticated = (ctx.CurrentUser == null);
                return null;
            });

            this.After.AddItemToEndOfPipeline(ctx =>
            {
                var now = DateTime.Now;
                ctx.Response.WithCookie("lastvisit", now.ToShortDateString() + " " + now.ToShortTimeString(), now.AddYears(1));
            });
        }
    }
}
