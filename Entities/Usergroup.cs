namespace DotNancyTemplate.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using DotNORM.Model;

    /// <summary>
    /// The usergroup contains the list of claims that this group has assigned
    /// </summary>
    public class Usergroup : NORMObject<Usergroup>
    {
        /// <summary>
        /// The unique Id of the <see cref="Usergroup"/>
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The name of this <see cref="Usergroup"/>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The list of <see cref="Claim"/> ids 
        /// </summary>
        public List<Guid> Claims { get; set; }

        /// <summary>
        /// Gets the string representation of the list of <see cref="Claim"/> of this usergroup.
        /// </summary>
        [IgnoreDataMember]
        public string ClaimsList
        {
            get { return this.GetClaimsList(); }
        }

        public Usergroup()
        {
            this.Claims = new List<Guid>();
        }

        /// <summary>
        /// Gets the <see cref="Claim"/> list in string form.
        /// </summary>
        /// <returns>The string with all the <see cref="Claim"/>s.</returns>
        public string GetClaimsList()
        {
            // get all the claims
            var allClaims = Claim.All();

            var listOfClaims = this.Claims.Select(claimId => allClaims.First(c => c.Id.Equals(claimId)).Name);

            return string.Join(", ", listOfClaims);
        }
    }
}
