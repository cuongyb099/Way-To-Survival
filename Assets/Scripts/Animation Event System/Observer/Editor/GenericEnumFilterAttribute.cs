using System;
using System.Diagnostics;
using UnityEngine;

namespace KatLib.Observer.Editor
{
    [Conditional("UNITY_EDITOR")]
    public class GenericEnumFilterAttribute : PropertyAttribute
    {
        public string[] IncludeAssembly;
        public string[] IncludeNamespace;
        public Type[] IncludeEnumType;
        public string[] ExcludeNamespace;
        public Type[] ExcludeEnumType;
        
        public GenericEnumFilterAttribute(string[] includeAssembly = null, 
            string[] includeNamespace = null
            , Type[] includeEnumType = null
            , string[] excludeNamespace = null,
            Type[] excludeEnumType = null)
        {
            this.IncludeAssembly = includeAssembly;
            this.IncludeNamespace = includeNamespace;
            this.IncludeEnumType = includeEnumType;
            this.ExcludeNamespace = excludeNamespace;
            this.ExcludeEnumType = excludeEnumType;
        }
    }
}