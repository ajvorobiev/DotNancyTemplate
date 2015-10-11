namespace DotNancyTemplate.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Security;

    class UserMapper : IUserMapper
    {
        private static List<Tuple<string, string, Guid>> users = new List<Tuple<string, string, Guid>>();

        static UserMapper()
        {
            users.Add(new Tuple<string, string, Guid>("admin", "password", new Guid("55E1E49E-B7E8-4EEA-8459-7A906AC4D4C0")));
            users.Add(new Tuple<string, string, Guid>("user", "password", new Guid("56E1E49E-B7E8-4EEA-8459-7A906AC4D4C1")));
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var userRecord = users.FirstOrDefault(u => u.Item3 == identifier);

            return userRecord == null ? null : new User { UserName = userRecord.Item1 };

        }

        public static Guid? ValidateUser(string username, string password)
        {
            var userRecord = users.Where(u => u.Item1 == username && u.Item2 == password).FirstOrDefault();

            if (userRecord == null) return null;

            return userRecord.Item3;
        }
    }
}
