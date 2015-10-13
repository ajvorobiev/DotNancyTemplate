namespace DotNancyTemplate.Models.Admin
{
    using Entities;
    using System.Collections.Generic;
    using System.Linq;

    public class UsersModel
    {
        /// <summary>
        /// Gets the list of all <see cref="User"/> entities
        /// </summary>
        public List<User>  AllUsers { get; private set; }

        /// <summary>
        /// Gets the count of <see cref="AllUsers"/>. 
        /// </summary>
        public int UserCount
        {
            get
            {
                return this.AllUsers.Count;
            }
        }

        public UsersModel()
        {
            this.AllUsers = User.All().ToList();
        }
    }
}
