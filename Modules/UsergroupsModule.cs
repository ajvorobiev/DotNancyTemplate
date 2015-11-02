namespace DotNancyTemplate.Modules
{
    using System;
    using System.IO;
    using System.Linq;
    using Entities;
    using Models;
    using Models.Admin;
    using Nancy;
    using Nancy.ModelBinding;
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

            this.Get["/usergroups/create"] = x =>
            {
                this.Model.Usergroup = new Usergroup();
                this.Model.Claims = Claim.All();
                return this.View["admin/UsergroupEdit", this.Model];
            };

            this.Post["/usergroups/create"] = x =>
            {
                // do the save
                var name = (string)this.Request.Form.Name;
                var claims = (string)this.Request.Form.Claims;
                
                var master = (MasterModel)this.Model.MasterModel;
                master.Errored = false;
                master.ErrorsList.Clear();

                var newUsergroup = new Usergroup()
                {
                    Id = Guid.NewGuid(),
                    Name = name
                };

                var allUsergroups = Usergroup.All();

                if (string.IsNullOrWhiteSpace(newUsergroup.Name))
                {
                    master.ErrorsList.Add("The name must not be empty.");
                }

                if (allUsergroups.Any(u => u.Name.Equals(newUsergroup.Name)))
                {
                    master.ErrorsList.Add("The provided name is already taken.");
                }

                // set the claims
                newUsergroup.Claims = claims.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList().Select(Guid.Parse).ToList();
                
                // save

                if (master.ErrorsList.Any())
                {
                    master.Errored = true;
                    this.Model.Usergroup = newUsergroup;
                    this.Model.Claims = Claim.All();
                    var u = this.BindTo(newUsergroup);
                    return this.View["admin/UsergroupEdit", this.Model];
                }

                newUsergroup.Save();

                // redirect to the list
                return this.Response.AsRedirect("/admin/usergroups");
            };

            this.Get["/usergroups/{id:guid}/edit"] = x =>
            {
                var usergroup = Usergroup.Find(Guid.Parse(x.id));

                if (usergroup == null)
                {
                    return HttpStatusCode.NotFound;
                }

                this.Model.Usergroup = usergroup;
                this.Model.Claims = Claim.All();

                return this.View["admin/UsergroupEdit", this.Model];
            };

            this.Post["/usergroups/{id:guid}/update"] = x =>
            {
                // do the save
                var name = (string)this.Request.Form.Name;
                var claims = (string)this.Request.Form.Claims;

                var master = (MasterModel)this.Model.MasterModel;
                master.Errored = false;
                master.ErrorsList.Clear();

                var oldUsergroup = Usergroup.Find((Guid)x.Id);

                var allUsergroups = Usergroup.All();


                if (string.IsNullOrWhiteSpace(name))
                {
                    master.ErrorsList.Add("The name must not be empty.");
                }

                if (allUsergroups.Any(u => u.Name.Equals(name) && !u.Name.Equals(oldUsergroup.Name)))
                {
                    master.ErrorsList.Add("The provided name is already taken.");
                }

                oldUsergroup.Name = name;

                oldUsergroup.Claims = claims.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().Select(Guid.Parse).ToList();
                
                // save
                if (master.ErrorsList.Any())
                {
                    master.Errored = true;
                    this.Model.Usergroup = oldUsergroup;
                    this.Model.Usergroups = Claim.All();
                    var u = this.BindTo(oldUsergroup);
                    return this.View["admin/UsergroupEdit", this.Model];
                }

                oldUsergroup.Save();

                // redirect to the list
                return this.Response.AsRedirect("/admin/usergroups");
            };

            this.Post["/usergroups/{id:guid}/remove"] = x =>
            {
                var usergroup = Usergroup.Find((Guid)x.id);

                if (usergroup == null)
                {
                    return HttpStatusCode.NotFound;
                }

                // remove the user

                try
                {
                    usergroup.Delete();
                }
                catch (InvalidDataException)
                {
                    var master = (MasterModel)this.Model.MasterModel;
                    master.Errored = true;
                    master.ErrorsList.Add("You cannot delete a usergroup that has members assigned.");

                    this.Model.Usergroups = new UsergroupsModel();
                    return this.View["admin/Usergroups", this.Model];
                }

                return this.Response.AsRedirect("/admin/usergroups");
            };
        }
    }
}
