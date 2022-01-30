﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Guldan.Common;

namespace Guldan.DynamicWebApi
{
    public class Convention : IApplicationModelConvention
    {
        private readonly ISelectController _selectController;
        private readonly IActionRouteFactory _actionRouteFactory;

        public Convention(ISelectController selectController, IActionRouteFactory actionRouteFactory)
        {
            _selectController = selectController;
            _actionRouteFactory = actionRouteFactory;
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var type = controller.ControllerType.AsType();
                var dynamicWebApiAttr = ReflectionHelper.GetSingleAttributeOrDefaultByFullSearch<DynamicWebApiAttribute>(type.GetTypeInfo());

                if (!(_selectController is DefaultSelectController) && _selectController.IsController(type))
                {
                    controller.ControllerName = controller.ControllerName.RemovePostFix(DynamicApiConsts.ControllerPostfixes.ToArray());
                    ConfigureDynamicWebApi(controller, dynamicWebApiAttr);
                }
                else
                {
                    if (typeof(IDynamicWebApi).GetTypeInfo().IsAssignableFrom(type))
                    {
                        controller.ControllerName = controller.ControllerName.RemovePostFix(DynamicApiConsts.ControllerPostfixes.ToArray());
                        ConfigureArea(controller, dynamicWebApiAttr);
                        ConfigureDynamicWebApi(controller, dynamicWebApiAttr);
                    }
                    else
                    {
                        if (dynamicWebApiAttr != null)
                        {
                            ConfigureArea(controller, dynamicWebApiAttr);
                            ConfigureDynamicWebApi(controller, dynamicWebApiAttr);
                        }
                    }
                }
            }
        }

        private void ConfigureArea(ControllerModel controller, DynamicWebApiAttribute attr)
        {
            if (!controller.RouteValues.ContainsKey("area"))
            {
                if (attr == null)
                {
                    throw new ArgumentException(nameof(attr));
                }

                if (!string.IsNullOrEmpty(attr.Module))
                {
                    controller.RouteValues["area"] = attr.Module;
                }
                else if (!string.IsNullOrEmpty(DynamicApiConsts.DefaultAreaName))
                {
                    controller.RouteValues["area"] = DynamicApiConsts.DefaultAreaName;
                }
            }

        }

        private void ConfigureDynamicWebApi(ControllerModel controller, DynamicWebApiAttribute controllerAttr)
        {
            ConfigureApiExplorer(controller);
            ConfigureSelector(controller, controllerAttr);
            ConfigureParameters(controller);
        }


        private void ConfigureParameters(ControllerModel controller)
        {
            foreach (var action in controller.Actions)
            {
                if (!CheckNoMapMethod(action))
                    foreach (var para in action.Parameters)
                    {
                        if (para.BindingInfo != null)
                        {
                            continue;
                        }

                        if (!TypeExtension.IsPrimitiveExtendedIncludingNullable(para.ParameterInfo.ParameterType))
                        {
                            if (CanUseFormBodyBinding(action, para))
                            {
                                para.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromBodyAttribute() });
                            }
                        }
                    }
            }
        }


        private bool CanUseFormBodyBinding(ActionModel action, ParameterModel parameter)
        {
            if (DynamicApiConsts.FormBodyBindingIgnoredTypes.Any(t => t.IsAssignableFrom(parameter.ParameterInfo.ParameterType)))
            {
                return false;
            }

            foreach (var selector in action.Selectors)
            {
                if (selector.ActionConstraints == null)
                {
                    continue;
                }

                foreach (var actionConstraint in selector.ActionConstraints)
                {

                    var httpMethodActionConstraint = actionConstraint as HttpMethodActionConstraint;
                    if (httpMethodActionConstraint == null)
                    {
                        continue;
                    }

                    if (httpMethodActionConstraint.HttpMethods.All(hm => hm.IsIn("GET", "DELETE", "TRACE", "HEAD")))
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        #region ConfigureApiExplorer

        private void ConfigureApiExplorer(ControllerModel controller)
        {
            if (controller.ApiExplorer.GroupName.IsNull())
            {
                controller.ApiExplorer.GroupName = controller.ControllerName;
            }

            if (controller.ApiExplorer.IsVisible == null)
            {
                controller.ApiExplorer.IsVisible = true;
            }

            foreach (var action in controller.Actions)
            {
                if (!CheckNoMapMethod(action))
                    ConfigureApiExplorer(action);
            }
        }

        private void ConfigureApiExplorer(ActionModel action)
        {
            if (action.ApiExplorer.IsVisible == null)
            {
                action.ApiExplorer.IsVisible = true;
            }
        }

        #endregion


        /// <summary>
        /// //不映射指定的方法
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private bool CheckNoMapMethod(ActionModel action)
        {
            bool isExist = false;
            var noMapMethod = ReflectionHelper.GetSingleAttributeOrDefault<NonDynamicMethodAttribute>(action.ActionMethod);

            if (noMapMethod != null)
            {
                action.ApiExplorer.IsVisible = false;//对应的Api不映射
                isExist = true;
            }

            return isExist;
        }
        private void ConfigureSelector(ControllerModel controller, DynamicWebApiAttribute controllerAttr)
        {

            if (controller.Selectors.Any(selector => selector.AttributeRouteModel != null))
            {
                return;
            }

            var areaName = string.Empty;

            if (controllerAttr != null)
            {
                areaName = controllerAttr.Module;
            }

            foreach (var action in controller.Actions)
            {
                if (!CheckNoMapMethod(action))
                    ConfigureSelector(areaName, controller.ControllerName, action);
            }
        }

        private void ConfigureSelector(string areaName, string controllerName, ActionModel action)
        {

            var nonAttr = ReflectionHelper.GetSingleAttributeOrDefault<NonDynamicWebApiAttribute>(action.ActionMethod);

            if (nonAttr != null)
            {
                return;
            }

            if (action.Selectors.IsNullOrEmpty() || action.Selectors.Any(a => a.ActionConstraints.IsNullOrEmpty()))
            {
                if (!CheckNoMapMethod(action))
                    AddAppServiceSelector(areaName, controllerName, action);
            }
            else
            {
                NormalizeSelectorRoutes(areaName, controllerName, action);
            }
        }

        private void AddAppServiceSelector(string areaName, string controllerName, ActionModel action)
        {

            var verb = GetHttpVerb(action);

            action.ActionName = GetRestFulActionName(action.ActionName);

            var appServiceSelectorModel = action.Selectors[0];

            if (appServiceSelectorModel.AttributeRouteModel == null)
            {
                appServiceSelectorModel.AttributeRouteModel = CreateActionRouteModel(areaName, controllerName, action);
            }

            if (!appServiceSelectorModel.ActionConstraints.Any())
            {
                appServiceSelectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { verb }));
                switch (verb)
                {
                    case "GET":
                        appServiceSelectorModel.EndpointMetadata.Add(new HttpGetAttribute());
                        break;
                    case "POST":
                        appServiceSelectorModel.EndpointMetadata.Add(new HttpPostAttribute());
                        break;
                    case "PUT":
                        appServiceSelectorModel.EndpointMetadata.Add(new HttpPutAttribute());
                        break;
                    case "DELETE":
                        appServiceSelectorModel.EndpointMetadata.Add(new HttpDeleteAttribute());
                        break;
                    default:
                        throw new Exception($"Unsupported http verb: {verb}.");
                }
            }


        }



        /// <summary>
        /// Processing action name
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        private static string GetRestFulActionName(string actionName)
        {
            // custom process action name
            var DynamicApiConstsActionName = DynamicApiConsts.GetRestFulActionName?.Invoke(actionName);
            if (DynamicApiConstsActionName != null)
            {
                return DynamicApiConstsActionName;
            }

            // default process action name.

            // Remove Postfix
            actionName = actionName.RemovePostFix(DynamicApiConsts.ActionPostfixes.ToArray());

            // Remove Prefix
            var verbKey = actionName.GetPascalOrCamelCaseFirstWord().ToLower();
            if (DynamicApiConsts.HttpVerbs.ContainsKey(verbKey))
            {
                if (actionName.Length == verbKey.Length)
                {
                    return "";
                }
                else
                {
                    return actionName.Substring(verbKey.Length);
                }
            }
            else
            {
                return actionName;
            }
        }

        private void NormalizeSelectorRoutes(string areaName, string controllerName, ActionModel action)
        {
            action.ActionName = GetRestFulActionName(action.ActionName);
            foreach (var selector in action.Selectors)
            {
                selector.AttributeRouteModel = selector.AttributeRouteModel == null ?
                     CreateActionRouteModel(areaName, controllerName, action) :
                     AttributeRouteModel.CombineAttributeRouteModel(CreateActionRouteModel(areaName, controllerName, action), selector.AttributeRouteModel);
            }
        }

        private static string GetHttpVerb(ActionModel action)
        {
            var getValueSuccess = DynamicApiConsts.AssemblyDynamicWebApiOptions
                .TryGetValue(action.Controller.ControllerType.Assembly, out WebApiOptions assemblyDynamicWebApiOptions);
            if (getValueSuccess && !string.IsNullOrWhiteSpace(assemblyDynamicWebApiOptions?.HttpVerb))
            {
                return assemblyDynamicWebApiOptions.HttpVerb;
            }


            var verbKey = action.ActionName.GetPascalOrCamelCaseFirstWord().ToLower();

            var verb = DynamicApiConsts.HttpVerbs.ContainsKey(verbKey) ? DynamicApiConsts.HttpVerbs[verbKey] : DynamicApiConsts.DefaultHttpVerb;
            return verb;
        }

        private AttributeRouteModel CreateActionRouteModel(string areaName, string controllerName, ActionModel action)
        {
            var route = _actionRouteFactory.CreateActionRouteModel(areaName, controllerName, action);

            return new AttributeRouteModel(new RouteAttribute(route));
        }
    }
}