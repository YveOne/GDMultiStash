using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GDMultiStash.Global
{
    using Resources;
    namespace Resources
    {

    }

    internal partial class ResourcesManager : Base.Manager
    {

        public ResourcesManager() : base()
        {
        }

        public string WriteReadExternalResource(string resourceText, string resourceFile, char separator)
        {
            if (File.Exists(resourceFile))
            {
                var externalResourcesDict = Utils.Funcs.ReadDictionaryFromFile(resourceFile, separator);
                var internalResourcesDict = Utils.Funcs.ReadDictionaryFromText(resourceText, separator);
                var missingResourcesDict = new Dictionary<string, string>();
                foreach(var kvp in internalResourcesDict)
                {
                    if (!externalResourcesDict.ContainsKey(kvp.Key))
                    {
                        missingResourcesDict.Add(kvp.Key, kvp.Value);
                    }
                }
                if (missingResourcesDict.Count > 0)
                {
                    resourceText = File.ReadAllText(resourceFile);
                    resourceText += Environment.NewLine + "";
                    resourceText += Environment.NewLine + $"# New data ({DateTime.Now})";
                    foreach (var kvp in missingResourcesDict)
                    {
                        resourceText += Environment.NewLine + $"{kvp.Key} {separator} {kvp.Value}";
                    }
                    resourceText += Environment.NewLine + "";
                }
            }
            File.WriteAllText(resourceFile, resourceText);
            return resourceText;
        }

    }
}
