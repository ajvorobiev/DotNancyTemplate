namespace DotNancyTemplate.Modules
{
    using Models.Admin;
    using Nancy.Security;

    public class AdminModule : BaseModule
    {
        public AdminModule() : base("/admin")
        {
            this.RequiresAuthentication();
            this.RequiresClaims(new[] { "AdministrationPanel" });

            this.Get["/"] = x =>
            {
                this.Model.Index = new IndexModel();
                this.Model.Index.UserName = this.Context.CurrentUser.UserName;
                return this.View["admin/index", this.Model];
            };
        }
    }
}
