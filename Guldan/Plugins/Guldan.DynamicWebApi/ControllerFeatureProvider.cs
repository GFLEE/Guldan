using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers; 

namespace Guldan.DynamicWebApi
{
    public class ControllerFeatureProvider: Microsoft.AspNetCore.Mvc.Controllers.ControllerFeatureProvider
    {
        private ISelectController _selectController;

        public ControllerFeatureProvider(ISelectController selectController)
        {
            _selectController = selectController;
        }

        protected override bool IsController(TypeInfo typeInfo)
        {
            return _selectController.IsController(typeInfo);
        }
    }
}