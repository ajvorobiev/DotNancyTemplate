namespace DotNancyTemplate.Modules
{
    using System.Net.Http;
    using System.Runtime.InteropServices;
    using Entities;
    using Models.Admin;
    using Nancy;
    using Nancy.Security;
    using System;

    public class UsersModule : BaseModule
    {
        public UsersModule() : base("/admin")
        {
            this.RequiresAuthentication();
            this.RequiresClaims(new[] { "Users" });

            this.Get["/users"] = x =>
            {
                this.Model.Users = new UsersModel();
                return this.View["admin/Users", this.Model];
            };

            this.Get["/users/{id:guid}"] = x =>
            {
                var user = User.Find(Guid.Parse(x.id));

                if(user == null)
                {
                    return HttpStatusCode.NotFound;
                }
                
                this.Model.User = user;

                return this.View["admin/user", this.Model];
            };

            this.Post["/users/create"] = x =>
            {
                throw new HttpRequestException("Not implemented yet.");
            };

            this.Post["/users/{id:guid}/update"] = x =>
            {
                throw new HttpRequestException("Not implemented yet.");
            };

            this.Post["/users/{id:guid}/delete"] = x =>
            {
                throw new HttpRequestException("Not implemented yet.");
            };
        }
    }
}
