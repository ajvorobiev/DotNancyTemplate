namespace DotNancyTemplate.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Security;

    /// <summary>
    /// The user mapper handles user authentication
    /// </summary>
    public class UserMapper : IUserMapper
    {
        /// <summary>
        /// Get the real username from an identifier
        /// </summary>
        /// <param name="identifier">User identifier</param><param name="context">The current NancyFx context</param>
        /// <returns>
        /// Matching populated IUserIdentity object, or empty
        /// </returns>
        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var userRecord = User.Find(identifier);

            return userRecord ?? null;

        }

        /// <summary>
        /// Validates the user and if correctly validated returns the user's Id
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static Guid? ValidateUser(string username, string password)
        {
            var userRecord = User.All().FirstOrDefault(u => u.UserName == username || u.Email == username);

            if(userRecord != null)
            {
                userRecord.Password = password;
                if(!userRecord.Authenticate())
                {
                    return null;
                }
            }

            if (userRecord == null) return null;

            return userRecord.Id;
        }
    }
}
