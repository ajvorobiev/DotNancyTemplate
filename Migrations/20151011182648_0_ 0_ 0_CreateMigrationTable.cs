namespace DotNancyTemplate.Migrations
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;
    using DotNORM.Database;
    using DotNORM.MigrationEngine;

    /// <summary>
    /// Creates migration table.
    /// </summary>
    internal class CreateMigrationTable : MigrationBase
    {
        /// <summary>
        /// Gets the unique <see cref="Guid"/> of the <see cref="IMigration"/>. This must be generated pre-compile time.
        /// </summary>
        public override Guid Id 
        {
            get 
            { 
                return Guid.Parse("ab059bda-2dab-43d5-b4b2-a0998a8a22c5"); 
            }
        }

        /// <summary>
        /// Gets the name of the <see cref="IMigration"/>.
        /// </summary>
        public override string Name
        {
            get 
            { 
                return this.GetType().Name; 
            }
        }

        /// <summary>
        /// Gets the full name of the migration. The full name of a migration is the <see cref="IMigration.Name"/> property
        /// prepended with a long date. This is done for sorting purposes.
        /// </summary>
        public override string FullName
        {
            get
            {
                return string.Format("{0}_{1}", "20151011182648", this.Name);
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public override string Description 
        {
            get 
            { 
                return "Creates the Migration table so that the application can write migration patch logs."; 
            }
        }

        /// <summary>
        /// Gets the version this migration belongs to
        /// </summary>
        public override Version Version
        {
            get
            {
                return new Version(0, 0, 0);
            }
        }

        /// <summary>
        /// The method that executes to apply a migration.
        /// </summary>
        public override void Migrate()
        {
            var migrationTableTemplate = new Migration();
            DatabaseSession.Instance.Connector.CreateTable(migrationTableTemplate);

            foreach (var property in migrationTableTemplate.GetType().GetProperties().Where(p => !Attribute.IsDefined(p, typeof(IgnoreDataMemberAttribute))).ToList())
            {
                DatabaseSession.Instance.Connector.CreateColumn(property, migrationTableTemplate);
            }

            DatabaseSession.Instance.Connector.CreatePrimaryKeyConstraint(migrationTableTemplate);
        }

        /// <summary>
        /// The method that executes if a migration needs to be rolled back.
        /// </summary>
        public override void Reverse()
        {
            var migrationTableTemplate = new Migration();
            DatabaseSession.Instance.Connector.DeleteTable(migrationTableTemplate);
        }

        /// <summary>
        /// Deletes the record from the migration table.
        /// </summary>
        public override void Delete()
        {
            // nothing needed
        }

        /// <summary>
        /// Checks whether the migration should execute.
        /// </summary>
        /// <returns>True if the migration should run.</returns>
        public override bool ShouldMigrate()
        {
            var migrationTableTemplate = new Migration();
            return !DatabaseSession.Instance.Connector.CheckTableExists(migrationTableTemplate);
        }
    }
}
