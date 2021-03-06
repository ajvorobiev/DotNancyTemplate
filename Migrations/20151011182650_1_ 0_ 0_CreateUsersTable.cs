namespace DotNancyTemplate.Migrations
{
    using System;
    using DotNORM.Database;
    using DotNORM.MigrationEngine;
    using Entities;

    /// <summary>
    /// The purpose of the <see cref="CreateUsersTable"/> migration is to ....
    /// </summary>
    internal class CreateUsersTable : MigrationBase
    {
        /// <summary>
        /// Gets the unique <see cref="Guid"/> of the <see cref="IMigration"/>. This must be generated pre-compile time.
        /// </summary>
        public override Guid Id 
        {
            get 
            { 
                return Guid.Parse("988b6f23-8dc1-4821-a539-3c60e073205e"); 
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
                return string.Format("{0}_{1}", "20151011182650", this.Name);
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public override string Description
        {
            get 
            { 
                return "Creates a table of users, usergroups and claims."; 
            }
        }

        /// <summary>
        /// Gets the version this migration belongs to
        /// </summary>
        public override Version Version
        {
            get
            {
                return new Version(1, 0, 0);
            }
        }

        /// <summary>
        /// The method that executes to apply a migration.
        /// </summary>
        public override void Migrate()
        {
            var claim = new Claim();
            var usergroup = new Usergroup();
            var user = new User();

            var transaction = DatabaseSession.Instance.CreateTransaction();

            DatabaseSession.Instance.Connector.CreateTableWithColumns(claim, transaction);
            DatabaseSession.Instance.Connector.CreatePrimaryKeyConstraint(claim, transaction);
            DatabaseSession.Instance.Connector.CreateTableWithColumns(usergroup, transaction);
            DatabaseSession.Instance.Connector.CreatePrimaryKeyConstraint(usergroup, transaction);
            DatabaseSession.Instance.Connector.CreateTableWithColumns(user, transaction);
            DatabaseSession.Instance.Connector.CreatePrimaryKeyConstraint(user, transaction);
            DatabaseSession.Instance.Connector.CreateUniquenessConstraint(new []{ user.GetType().GetProperty("UserName") }, user, transaction);
            DatabaseSession.Instance.Connector.CreateUniquenessConstraint(new[] { user.GetType().GetProperty("Email") }, user, transaction);

            DatabaseSession.Instance.Connector.CreateForeignKeyConstraint(user.GetType().GetProperty("UsergroupId"),user, usergroup.GetType().GetProperty("Id"), usergroup, transaction);

            DatabaseSession.Instance.CommitTransaction(transaction);
        }

        /// <summary>
        /// The method that executes if a migration needs to be rolled back.
        /// </summary>
        public override void Reverse()
        {
            var transaction = DatabaseSession.Instance.CreateTransaction();

            DatabaseSession.Instance.Connector.DeleteTable(new User(), transaction);
            DatabaseSession.Instance.Connector.DeleteTable(new Usergroup(), transaction);
            DatabaseSession.Instance.Connector.DeleteTable(new Claim(), transaction);

            DatabaseSession.Instance.CommitTransaction(transaction);
        }

        /// <summary>
        /// Seeds the needed claims, usergroups and some users.
        /// </summary>
        public override void Seed()
        {
            var usergroupClaim = new Claim
            {
                Name = "Usergroups",
                Id = Guid.NewGuid(),
                Description = "Allows for viewing and editing of Usergroups."
            };

            usergroupClaim.Save();

            var userClaim = new Claim
            {
                Name = "Users",
                Id = Guid.NewGuid(),
                Description = "Allows for viewing and editing the Users."
            };

            userClaim.Save();

            var adminSectionClaim = new Claim
            {
                Name = "AdminOverview",
                Id = Guid.NewGuid(),
                Description = "Grants access to to the Administration section of the application."
            };

            adminSectionClaim.Save();

            var logClaim = new Claim
            {
                Name = "Log",
                Id = Guid.NewGuid(),
                Description = "Allows for viewing the Log."
            };

            logClaim.Save();

            var adminUsergroup = new Usergroup()
            {
                Name = "Administrator",
                Id = Guid.NewGuid()
            };

            adminUsergroup.Claims.Add(adminSectionClaim.Id);
            adminUsergroup.Claims.Add(usergroupClaim.Id);
            adminUsergroup.Claims.Add(userClaim.Id);
            adminUsergroup.Claims.Add(logClaim.Id);
            adminUsergroup.Save();

            var userUsergroup = new Usergroup()
            {
                Name = "User",
                Id = Guid.NewGuid()
            };

            userUsergroup.Save();

            var adminUser = new User
            {
                DateRegistered = DateTime.Now,
                Email = "admin@mycompany.com",
                Password = "pass",
                Id = Guid.NewGuid(),
                UserName = "admin",
                UsergroupId = adminUsergroup.Id
            };

            adminUser.EncodePassword();

            adminUser.Save();

#if DEBUG
            var normalUser = new User
            {
                DateRegistered = DateTime.Now,
                Email = "user@mycompany.com",
                Password = "user",
                Id = Guid.NewGuid(),
                UserName = "user",
                UsergroupId = userUsergroup.Id
            };

            normalUser.EncodePassword();
            normalUser.Save();
#endif

        }
    }
}
