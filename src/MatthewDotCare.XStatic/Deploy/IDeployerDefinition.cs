namespace MatthewDotCare.XStatic.Deploy
{
    public interface IDeployerDefinition
    {
        string Id { get; }

        string Name { get; }

        string Help { get; }

        IEnumerable<string> Fields { get; }
    }
}