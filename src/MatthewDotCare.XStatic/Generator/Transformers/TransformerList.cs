﻿namespace MatthewDotCare.XStatic.Generator.Transformers
{
    public class TransformerList
    {
        public List<Type> TransformerListFactories { get; }

        public TransformerList()
        {
            TransformerListFactories = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(ITransformerListFactory).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .ToList();
        }
    }
}