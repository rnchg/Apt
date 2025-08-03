namespace APT.App.Models
{
    public class AppSettings
    {
        public App App { get; set; } = null!;
    }
    public class App
    {
        public string Pack { get; set; } = null!;
    }
}
