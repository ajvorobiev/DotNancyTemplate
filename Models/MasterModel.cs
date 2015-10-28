namespace DotNancyTemplate.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// The view model for the master page
    /// </summary>
    public class MasterModel
    {
        /// <summary>
        /// Gets or sets the title of the page
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the last date visited
        /// </summary>
        public string LastVisit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is authorised or not.
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// Gets or sets the app title.
        /// </summary>
        public string AppTitle { get; set; }

        /// <summary>
        /// Gets or sets whether the model is errored
        /// </summary>
        public bool Errored { get; set; }

        /// <summary>
        /// Gets or sets the list of errors.
        /// </summary>
        public List<string> ErrorsList { get; set; }

        public MasterModel()
        {
            this.ErrorsList = new List<string>();
        }
    }
}