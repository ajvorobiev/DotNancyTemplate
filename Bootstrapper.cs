namespace DotNancyTemplate
{
    using System.Net.Http;
    using DotNORM.Database;
    using DotNORM.MigrationEngine;
    using Helpers;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Bootstrapper;
    using Nancy.TinyIoc;

    /// <summary>
    /// The bootsrapper initializes all containers and authentication.
    /// </summary>
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        /// <summary>
        /// Configure the request container
        /// </summary>
        /// <param name="container">Request container instance</param><param name="context"/>
        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            container.Register<IUserMapper, UserMapper>();
        }

        /// <summary>
        /// Initialise the bootstrapper - can be used for adding pre/post hooks and
        ///             any other initialisation tasks that aren't specifically container setup
        ///             related
        /// </summary>
        /// <param name="container">Container instance for resolving types if required.</param>
        /// <param name="pipelines">The pipelines used in this application.</param>
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            this.SetupDatabaseConnection();

            base.ApplicationStartup(container, pipelines);
            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration()
                {
                    RedirectUrl = "~/login",
                    UserMapper = container.Resolve<IUserMapper>()
                };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }

        /// <summary>
        /// Sets up the database connection and migration engine
        /// </summary>
        private void SetupDatabaseConnection()
        {
            // read all settings from the db
            var settings = AppSettingsManager.ReadAllSettings();

            try
            {
                // Create the connection.
                DatabaseSession.Instance.CreateConnector(settings["dbhost"], settings["dbport"], settings["dbname"], settings["dbuser"], settings["dbpassword"], ConnectorType.Postgresql);
            }
            catch (System.Exception ex)
            {
                throw new HttpRequestException(string.Format("The connection to database could not be made: {0}", ex.Message));
            }

#if DEBUG
            // when debugging reset the full database
            MigrationEngine.Reset();
#endif
            // perform the migration
            MigrationEngine.Migrate();
        }
    }
}
