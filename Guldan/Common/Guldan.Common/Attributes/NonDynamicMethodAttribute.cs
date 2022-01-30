using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldan.Common
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class NonDynamicMethodAttribute : Attribute
    {

    }
}
