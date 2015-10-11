namespace DotNancyTemplate.Entities
{
    using System;
    using System.Collections.Generic;
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

        public Usergroup()
        {
            this.Claims = new List<Guid>();
        }
    }
}
