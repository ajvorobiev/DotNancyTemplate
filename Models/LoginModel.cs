namespace DotNancyTemplate.Models
{
    using Nancy;

    public class LoginModel
    {
        public bool Errored { get; set; }
        public Url ReturnUrl { get; set; }
    }
}
