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
                this.Model.MasterModel = new MasterModel();

                var appTitle = AppSettingsManager.ReadSetting("appname");

                this.Model.MasterModel.Title = appTitle;
                this.Model.MasterModel.AppTitle = appTitle;

                if (this.Request.Cookies.ContainsKey("lastvisit"))
                {
                    this.Model.MasterModel.LastVisit = Uri.UnescapeDataString(this.Request.Cookies["lastvisit"]);
                }
                else
                {
                    this.Model.MasterModel.LastVisit = "No cookie value yet";
                }

                this.Model.MasterModel.IsAuthenticated = (ctx.CurrentUser == null);
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
