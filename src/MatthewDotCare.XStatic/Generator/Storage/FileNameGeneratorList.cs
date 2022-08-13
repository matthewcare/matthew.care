namespace MatthewDotCare.XStatic.Generator.Storage
{
    public class FileNameGeneratorList
    {
        public List<Type> FileNameGenerators { get; }

        public FileNameGeneratorList()
        {
            FileNameGenerators = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(IFileNameGenerator).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .ToList();
        }
    }
}