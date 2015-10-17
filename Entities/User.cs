namespace DotNancyTemplate.Entities
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using DotNORM.Model;
    using Helpers;
    using Nancy.Security;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The <see cref="User"/> class is responsible in handling the user entities 
    /// </summary>
    public class User : NORMObject<User>, IUserIdentity
    {
        /// <summary>
        /// The id of the user
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The username of the user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The users email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The users password
        /// </summary>
        [IgnoreDataMember]
        public string Password { get; set; }

        /// <summary>
        /// The encoded password string
        /// </summary>
        public string EncodedPassword { get; set; }

        /// <summary>
        /// The list of claim the user has.
        /// </summary>
        [IgnoreDataMember]
        public IEnumerable<string> Claims
        {
            get { return this.GetClaims(); }
        }

        /// <summary>
        /// The ids of the <see cref="Claim"/>s that apply to this <see cref="User"/>
        /// </summary>
        public Guid UsergroupId { get; set; }

        /// <summary>
        /// Gets the <see cref="Usergroup"/> that this person has assigned.
        /// </summary>
        [IgnoreDataMember]
        public Usergroup Usergroup { get { return this.GetUsergroup(); } }

        /// <summary>
        /// The date that this user registered
        /// </summary>
        public DateTime DateRegistered { get; set; }

        /// <summary>
        /// Authenticates the user based on his or her password.
        /// </summary>
        /// <returns>True if the user authenticates correctly</returns>
        public bool Authenticate()
        {
            if(this.Password == null || this.EncodedPassword == null)
            {
                throw new HttpRequestException("The authentication can only be performed when both the encoded and the unencoded passwords are known.");
            }

            return Encoder.AuthenticateUser(this);
        }

        /// <summary>
        /// Sets the current <see cref="User"/>s <see cref="EncodedPassword"/> property based on the literal password.
        /// </summary>
        public void EncodePassword()
        {
            this.EncodedPassword = Encoder.EncodeUserPassword(this);
        }

        /// <summary>
        /// Gets the claims from the database and provides the appropriate array for authentication.
        /// </summary>
        /// <returns>The list of <see cref="Claim"/> names that this User posesses.</returns>
        private IEnumerable<string> GetClaims()
        {
            // get all the claims
            var allClaims = Claim.All();
            var usergroup = this.GetUsergroup();

            return usergroup.Claims.Select(claimId => allClaims.First(c => c.Id.Equals(claimId)).Name);
        }

        /// <summary>
        /// Gets the <see cref="Usergroup"/> assigned to this user.
        /// </summary>
        /// <returns>The <see cref="Usergroup"/> assigned.</returns>
        private Usergroup GetUsergroup()
        {
            return Usergroup.Find(this.UsergroupId);
        }
    }
}
