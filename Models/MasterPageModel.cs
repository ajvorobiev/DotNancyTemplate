namespace DotNancyTemplate.Models
{
    /// <summary>
    /// The view model for the master page
    /// </summary>
    public class MasterPageModel
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
    }
}