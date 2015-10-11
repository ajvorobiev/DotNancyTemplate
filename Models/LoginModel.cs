namespace DotNancyTemplate.Models
{
    using Nancy;

    public class LoginModel
    {
        public bool Error { get; set; }
        public Url ReturnUrl { get; set; }
    }
}
