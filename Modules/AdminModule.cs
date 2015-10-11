namespace DotNancyTemplate.Modules
{
    using Models.Admin;
    using Nancy.Security;

    public class AdminModule : BaseModule
    {
        public AdminModule() : base("/admin")
        {
            this.RequiresAuthentication();
            Get["/"] = x =>
            {
                Model.Index = new IndexModel();
                Model.Index.UserName = Context.CurrentUser.UserName;
                return View["admin/index", Model];
            };
        }
    }
}
