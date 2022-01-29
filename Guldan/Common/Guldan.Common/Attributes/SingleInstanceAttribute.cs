using System;

namespace Guldan.Common
{
    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class SingleInstanceAttribute : Attribute
    {
    }
}