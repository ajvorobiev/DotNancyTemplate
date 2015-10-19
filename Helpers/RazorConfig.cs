namespace DotNancyTemplate.Helpers
{
    using System.Collections.Generic;
    using Nancy.ViewEngines.Razor;

    public class RazorConfig : IRazorConfiguration
    {
        public IEnumerable<string> GetAssemblyNames()
        {
            yield return "DotNancyTemplate.Entities";
            yield return "DotNancyTemplate.Models";
        }

        public IEnumerable<string> GetDefaultNamespaces()
        {
            yield return "DotNancyTemplate.Entities";
            yield return "DotNancyTemplate.Models";
        }

        public bool AutoIncludeModelNamespace
        {
            get { return true; }
        }
    }
}
