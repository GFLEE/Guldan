using System;

namespace Guldan.Common
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class NonDynamicWebApiAttribute:Attribute
    {
        
    }
}