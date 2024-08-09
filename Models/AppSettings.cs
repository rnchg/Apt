namespace General.Apt.App.Models
{
    public class AppSettings
    {
        public App App { get; set; }
    }
    public class App
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Pack { get; set; }
    }
}
