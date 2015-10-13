namespace DotNancyTemplate.Modules
{
    using Models.Admin;
    using Nancy.Security;

    public class UsersModule : BaseModule
    {
        public UsersModule() : base("/admin")
        {
            this.RequiresAuthentication();
            this.RequiresClaims(new[] { "Users" });

            this.Get["/users"] = x =>
            {
                this.Model.Users = new UsersModel();
                return this.View["admin/users", this.Model];
            };
        }
    }
}
