namespace DotNancyTemplate.Helpers
{
    using System.Linq.Expressions;
    using System.Reflection;
    using Nancy.ViewEngines.Razor;
    using System;

    /// <summary>
    /// Contains extension methods for the <see cref="Expression"/> type.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Retrieves the member that an expression is defined for.
        /// </summary>
        /// <param name="expression">The expression to retreive the member from.</param>
        /// <returns>A <see cref="MemberInfo"/> instance if the member could be found; otherwise <see langword="null"/>.</returns>
        public static MemberInfo GetTargetMemberInfo(this Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Convert:
                    return GetTargetMemberInfo(((UnaryExpression)expression).Operand);
                case ExpressionType.Lambda:
                    return GetTargetMemberInfo(((LambdaExpression)expression).Body);
                case ExpressionType.Call:
                    return ((MethodCallExpression)expression).Method;
                case ExpressionType.MemberAccess:
                    return ((MemberExpression)expression).Member;
                default:
                    return null;
            }
        }
    }

    /// <summary>
    /// Contains the extension methods on HtmlHelper<T>
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Renders a textbox for the given property on the model
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="helpers">The object that the extension was called on</param>
        /// <param name="expression">The expression that is used to extract the member name from</param>
        /// <returns>Markup that will not be encoded by the viewengine</returns>
        public static IHtmlString TextBoxFor<T>(string name)
        {
            var markup =
                string.Concat("<input type='textbox' name='", name, "' />");

            return new NonEncodedHtmlString(markup);
        }
    }
}
