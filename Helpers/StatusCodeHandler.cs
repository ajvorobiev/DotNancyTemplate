namespace DotNancyTemplate.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Nancy;
    using Nancy.ErrorHandling;
    using Nancy.ViewEngines;

    /// <summary>
    /// Handles HTTP status codes to display a custom error page.
    /// </summary>
    /// <remarks>https://blog.tommyparnell.com/custom-error-pages-in-nancy/</remarks>
    public class StatusCodeHandler : IStatusCodeHandler
    {
        /// <summary>
        /// The collection of tracked status codes
        /// </summary>
        private static IEnumerable<int> _checks = new List<int>();

        /// <summary>
        /// Nancy view renderer
        /// </summary>
        private IViewRenderer viewRenderer;

        /// <summary>
        /// Creates an instance of the <see cref="StatusCodeHandler"/> class.
        /// </summary>
        /// <param name="viewRenderer">The view renderer.</param>
        public StatusCodeHandler(IViewRenderer viewRenderer)
        {
            this.viewRenderer = viewRenderer;
        }

        /// <summary>
        /// Returns tru if the status code is monitored.
        /// </summary>
        /// <param name="statusCode">The status code to check.</param>
        /// <param name="context">The <see cref="NancyContext"/> that comes in.</param>
        /// <returns>True if the status code is tracked.</returns>
        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            if(bool.Parse(AppSettingsManager.ReadSetting("debugmode"))) return false;

            return (_checks.Any(x => x == (int)statusCode));
        }

        /// <summary>
        /// Add code to tracking
        /// </summary>
        /// <param name="code">The code to add.</param>
        public static void AddCode(int code)
        {
            AddCode(new List<int>() { code });
        }

        /// <summary>
        /// Add code to tracking
        /// </summary>
        /// <param name="code">The code to add.</param>
        public static void AddCode(IEnumerable<int> code)
        {
            _checks = _checks.Union(code);
        }

        /// <summary>
        /// Remove code from tracking.
        /// </summary>
        /// <param name="code">The code to remove.</param>
        public static void RemoveCode(int code)
        {
            RemoveCode(new List<int>() { code });
        }

        /// <summary>
        /// Remove code from tracking.
        /// </summary>
        /// <param name="code">The code to remove.</param>
        public static void RemoveCode(IEnumerable<int> code)
        {
            _checks = _checks.Except(code);
        }

        /// <summary>
        /// Refresh the list.
        /// </summary>
        public static void Disable()
        {
            _checks = new List<int>();
        }

        /// <summary>
        /// Handle the code that comes in and display the correct page.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="context">The <see cref="NancyContext"/> that the code rides on.</param>
        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            try
            {
                var model = new ErrorModel
                {
                    ErrorCode = (int)statusCode,
                    Message = context.ResolvedRoute.ToString()

                };

                var response = viewRenderer.RenderView(context, "/Codes/Error.html", model);
                response.StatusCode = statusCode;
                context.Response = response;
            }
            catch (Exception)
            {

                RemoveCode((int)statusCode);
                context.Response.StatusCode = statusCode;
            }
        }
    }
}
