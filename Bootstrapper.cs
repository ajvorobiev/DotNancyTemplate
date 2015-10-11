namespace DotNancyTemplate
{
    using System.IO;
    using Helpers;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Bootstrapper;
    using Nancy.TinyIoc;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        byte[] favicon;

        //protected override byte[] DefaultFavIcon
        //{
        //    get
        //    {
        //        if(favicon == null)
        //        {
        //            using(MemoryStream ms = new MemoryStream())
        //            {
        //                Resources.FavIcon.favicon.Save(ms);
        //                favicon = ms.ToArray();
        //            }
        //        }
        //        return favicon;
        //    }
        //}

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            container.Register<IUserMapper, UserMapper>();
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration()
                {
                    RedirectUrl = "~/login",
                    UserMapper = container.Resolve<IUserMapper>()
                };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }
    }
}
