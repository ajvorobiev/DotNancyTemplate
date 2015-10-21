namespace DotNancyTemplate.Modules
{
    using System;
    using Entities;
    using Models.Admin;
    using Nancy;
    using Nancy.Security;

    public class UsergroupsModule : BaseModule
    {
        public UsergroupsModule() : base("/admin")
        {
            this.RequiresAuthentication();
            this.RequiresClaims(new[] {"Users"});

            this.Get["/usergroups"] = x =>
            {
                this.Model.Usergroups = new UsergroupsModel();
                return this.View["admin/Usergroups", this.Model];
            };

            this.Get["/usergroups/{id:guid}"] = x =>
            {
                var usergroup = Usergroup.Find(Guid.Parse(x.id));

                if (usergroup == null)
                {
                    return HttpStatusCode.NotFound;
                }

                this.Model.Usergroup = usergroup;

                return this.View["admin/usergroup", this.Model];
            };
        }
    }
}
