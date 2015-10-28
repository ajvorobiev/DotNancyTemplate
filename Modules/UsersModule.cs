namespace DotNancyTemplate.Modules
{
    using System.Net.Http;
    using System.Runtime.InteropServices;
    using Entities;
    using Models.Admin;
    using Nancy;
    using Nancy.Security;
    using System;
    using System.Linq;
    using Models;
    using Nancy.ModelBinding;

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

                var master = (MasterModel) this.Model.MasterModel;
                master.Errored = false;
                master.ErrorsList.Clear();
                
                var newUser = new User()
                {
                    Id = Guid.NewGuid(),
                    UserName = username,
                    Email = email,
                    Password = password,
                    UsergroupId = usergroup
                };

                var allUsers = User.All();

                if (string.IsNullOrWhiteSpace(newUser.UserName))
                {
                    master.ErrorsList.Add("The username must not be empty.");
                }

                if (allUsers.Any(u => u.UserName.Equals(newUser.UserName)))
                {
                    master.ErrorsList.Add("The provided username is already taken.");
                }

                if (string.IsNullOrWhiteSpace(newUser.Email))
                {
                    master.ErrorsList.Add("The email must not be empty.");
                }

                if (allUsers.Any(u => u.Email.Equals(newUser.Email)))
                {
                    master.ErrorsList.Add("The provided email is already taken.");
                }
                
                if (string.IsNullOrWhiteSpace(newUser.Password))
                {
                    master.ErrorsList.Add("The password must not be empty.");
                }

                if (!newUser.Password.Equals(passwordValid))
                {
                    master.ErrorsList.Add("The passwords do not match.");
                }

                // save
                
                if (master.ErrorsList.Any())
                {
                    master.Errored = true;
                    this.Model.User = newUser;
                    this.Model.Usergroups = Usergroup.All();
                    var u = this.BindTo(newUser, "Password");
                    return this.View["admin/UserEdit", this.Model];
                }
          
                newUser.Save();


                // redirect to the list
                return this.Response.AsRedirect("/admin/users");
            };

            this.Post["/users/{id:guid}/update"] = x =>
            {
                throw new HttpRequestException("Not implemented yet.");
            };

            this.Post["/users/{id:guid}/remove"] = x =>
            {
                var user = User.Find((Guid)x.id);

                if(user == null)
                {
                    return HttpStatusCode.NotFound;
                }

                // remove the user
                user.Delete();

                return this.Response.AsRedirect("/admin/users");
            };
        }
    }
}
