namespace General.Apt.App.Models
{
    public class AppSettings
    {
        public App App { get; set; }
        public Chat Chat { get; set; }
    }
    public class App
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Pack { get; set; }
    }
    public class Chat
    {
        public Gpt Gpt { get; set; }
    }
    public class Gpt
    {
        public string PromptSystem { get; set; }
        public int PromptMaxLength { get; set; }
    }
}
