namespace DotNancyTemplate.Entities
{
    using System;
    using DotNORM.Model;

    /// <summary>
    /// A <see cref="Claim"/> represents a single permission a <see cref="User"/> may have
    /// </summary>
    public class Claim : NORMObject<Claim>
    {
        /// <summary>
        /// The unique <see cref="Guid"/> of the claim
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the claim.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Describes the point of the <see cref="Claim"/> and what it allows to do.
        /// </summary>
        public string Description { get; set; }
    }
}
