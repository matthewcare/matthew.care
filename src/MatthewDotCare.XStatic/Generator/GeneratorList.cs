﻿namespace MatthewDotCare.XStatic.Generator
{
    public class GeneratorList
    {
        public List<Type> Generators { get; }

        public GeneratorList()
        {
            Generators = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(IGenerator).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .ToList();
        }
    }
}