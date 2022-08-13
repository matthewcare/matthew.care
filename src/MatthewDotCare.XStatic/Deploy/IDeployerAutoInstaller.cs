namespace MatthewDotCare.XStatic.Deploy
{
    public interface IDeployerAutoInstaller
    {
        public IDeployerDefinition Definition { get; }

        public Func<Dictionary<string, string>, IDeployer> Constructor { get; }
    }
}