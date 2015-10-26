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

            this.Get["/users/create"] = x =>
            {
                this.Model.User = new User();
                this.Model.Usergroups = Usergroup.All();
                return this.View["admin/UserEdit", this.Model];
            };

            this.Post["/users/create"] = x =>
            {
                // do the save
                var username = (string) this.Request.Form.UserName;
                var email = (string) this.Request.Form.Email;
                var password = (string) this.Request.Form.Password;
                var passwordValid = (string) this.Request.Form.PasswordValidation;
                var usergroup = Guid.Parse((string) this.Request.Form.Usergroup);

                // TODO: Do validation and redirect if faulty

                // save
                var newUser = new User()
                {
                    Id = Guid.NewGuid(),
                    UserName = username,
                    Email = email,
                    Password = password,
                    UsergroupId = usergroup
                };

                newUser.Save();

                // redirect to the list
                return this.Response.AsRedirect("/admin/users");
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
