﻿using Umbraco.Cms.Core.Web;

namespace MatthewDotCare.XStatic.Generator.Transformers
{
    public interface ITransformer
    {
        string Transform(string input, IUmbracoContext context);
    }
}