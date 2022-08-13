﻿namespace MatthewDotCare.XStatic.Models
{
    public class TypeModel
    {
        public TypeModel()
        {
        }

        public TypeModel(Type type)
        {
            Id = type?.AssemblyQualifiedName;
            Name = type?.Name;
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class ConfigurableTypeModel : TypeModel
    {
        public ConfigurableTypeModel(Type type)
        {
            Id = type?.AssemblyQualifiedName;
            Name = type?.Name;

            var attrs = type?.GetCustomAttributes(typeof(XStaticEditableFieldAttribute), false)?.Cast<XStaticEditableFieldAttribute>()?.Select(a => a.FieldName);

            if(attrs?.Any() == true)
            {
                Fields = attrs.ToDictionary(a => a, a => "");
            }
        }

        public ConfigurableTypeModel(Type type, Dictionary<string, string> config)
        {
            Id = type?.AssemblyQualifiedName;
            Name = type?.Name;
            Fields = config;
        }

        public Dictionary<string, string> Fields { get; set; }
    }
}
