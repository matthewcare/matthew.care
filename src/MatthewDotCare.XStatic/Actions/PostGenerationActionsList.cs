namespace MatthewDotCare.XStatic.Actions
{
    public class PostGenerationActionsList
    {
        public List<Type> PostActions { get; }

        public PostGenerationActionsList()
        {
            PostActions = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(IPostGenerationAction).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .ToList();
        }
    }
}