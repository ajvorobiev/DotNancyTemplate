using DotNancyTemplate.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DotNancyTemplate.Models.Admin
{
    public class UsergroupsModel
    {
        /// <summary>
        /// Gets the list of all <see cref="Usergroup"/> entities
        /// </summary>
        public List<Usergroup> AllGroups { get; private set; }

        /// <summary>
        /// Gets the list of all <see cref="Claim"/> entities
        /// </summary>
        public List<Claim> AllClaims { get; private set; }

        public UsergroupsModel()
        {
            this.AllGroups = Usergroup.All().ToList();
            this.AllClaims = Claim.All().ToList();
        }
    }
}
