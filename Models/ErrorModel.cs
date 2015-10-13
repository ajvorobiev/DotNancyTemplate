namespace DotNancyTemplate.Models
{
    public class ErrorModel
    {
        /// <summary>
        /// The error code
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// The message to be displayed to user.
        /// </summary>
        public string Message { get; set; }
    }
}
