namespace MatthewDotCare.XStatic.Actions
{
    public class ConfiguredPostGenerationAction
    {
        public ConfiguredPostGenerationAction(IPostGenerationAction action, Dictionary<string, string> config)
        {
            Action = action;
            Config = config;
        }

        public IPostGenerationAction Action { get; }

        public Dictionary<string, string> Config { get; }
    }
}