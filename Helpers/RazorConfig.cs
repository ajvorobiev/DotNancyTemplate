namespace DotNancyTemplate.Helpers
{
    using System.Collections.Generic;
    using Nancy.ViewEngines.Razor;

    public class RazorConfig //: IRazorConfiguration
    {
        public IEnumerable<string> GetAssemblyNames()
        {
            yield return "System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
            yield return "Common";

            yield return "HtmlTags";

        }

        public IEnumerable<string> GetDefaultNamespaces()
        {
            //contains html extensions
            yield return "Web._Auxilia.Html";

        }

        public bool AutoIncludeModelNamespace
        {
            get { return true; }
        }
    }
}
