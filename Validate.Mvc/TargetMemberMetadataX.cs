using System;
using System.Text.RegularExpressions;

namespace Validate.Mvc
{
    public static class TargetMemberMetadataX
    {
        public static string GetRootRelativePath(this TargetMemberMetadata metadata, Type modelType)
        {   
            var rootTypeName = new Regex("^" +modelType.Name + "\\.");
            return rootTypeName.Replace((metadata.Path ?? string.Empty).Replace("this", string.Empty), string.Empty, 1);
        }
    }
}