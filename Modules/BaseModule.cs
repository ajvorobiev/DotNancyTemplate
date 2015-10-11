﻿namespace DotNancyTemplate.Modules
{
    using System;
    using System.Dynamic;
    using Models;
    using Nancy;

    public abstract class BaseModule : NancyModule
    {
        public dynamic Model = new ExpandoObject();

        public BaseModule()
        {
            SetupModelDefaults();
        }

        public BaseModule(string modulepath)
            : base(modulepath)
        {
            SetupModelDefaults();
        }

        private void SetupModelDefaults()
        {
            Before.AddItemToEndOfPipeline(ctx =>
            {
                Model.MasterPage = new MasterPageModel();
                Model.MasterPage.Title = "MyNancy - Hello World!";
                if (Request.Cookies.ContainsKey("lastvisit"))
                {
                    Model.MasterPage.LastVisit = Uri.UnescapeDataString(Request.Cookies["lastvisit"]);
                }
                else
                {
                    Model.MasterPage.LastVisit = "No cookie value yet";
                }
                Model.MasterPage.IsAuthenticated = (ctx.CurrentUser == null);
                return null;
            });
            After.AddItemToEndOfPipeline(ctx =>
            {
                var now = DateTime.Now;
                ctx.Response.WithCookie("lastvisit", now.ToShortDateString() + " " + now.ToShortTimeString(), now.AddYears(1));
            });
        }
    }
}
