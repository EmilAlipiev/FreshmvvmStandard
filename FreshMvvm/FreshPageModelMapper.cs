using System;

namespace FreshMvvm
{
    public class FreshPageModelMapper : IFreshPageModelMapper
    {
        readonly string pageNamespace;
        readonly string pageAssemblyName; 

        public FreshPageModelMapper(string pageNamespace = null, string pageAssemblyName = null)
        {
            this.pageNamespace = pageNamespace;
            this.pageAssemblyName = pageAssemblyName;
        }

        public string GetPageTypeName(Type pageModelType)
        {
            var assemblyQualifiedName = pageModelType.AssemblyQualifiedName;

            // Replace Namespace
            if (pageNamespace != null)
                assemblyQualifiedName = assemblyQualifiedName.Replace(pageModelType.Namespace, pageNamespace);

            // Replace Assembly
            if (pageAssemblyName != null)
                assemblyQualifiedName = assemblyQualifiedName.Replace(pageModelType.Assembly.ToString(), pageAssemblyName);

            // Replace "Model"
            assemblyQualifiedName = assemblyQualifiedName
                .Replace("PageModel", "Page")
                .Replace("ViewModel", "Page");

            return assemblyQualifiedName;
        }

    }
}

