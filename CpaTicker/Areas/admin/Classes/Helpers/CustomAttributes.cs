using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CpaTicker.Areas.admin.Classes.Helpers
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RemoteWithServerSideAttribute : RemoteAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string controllerName = this.RouteData["controller"].ToString();
            string actionName = this.RouteData["action"].ToString();
            string[] additionalFields = this.AdditionalFields.Split(',');

            List<object> propValues = new List<object>();
            propValues.Add(value);
            foreach (string additionalField in additionalFields)
            {
                PropertyInfo prop = validationContext.ObjectType.GetProperty(additionalField);
                if (prop != null)
                {
                    object propValue = prop.GetValue(validationContext.ObjectInstance, null);
                    propValues.Add(propValue);
                }
            }

            Type controllerType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name.ToLower() == (controllerName + "Controller").ToLower());
            if (controllerType != null)
            {
                object instance = Activator.CreateInstance(controllerType);

                MethodInfo method = controllerType.GetMethod(actionName);

                if (method != null)
                {
                    ActionResult response = (ActionResult)method.Invoke(instance, propValues.ToArray());

                    //ValidationResult output = null;

                    if (response is JsonResult)
                    {
                        bool isAvailable = false;
                        JsonResult json = (JsonResult)response;
                        string jsonData = json.Data.ToString();

                        bool.TryParse(jsonData, out isAvailable);

                        if (!isAvailable)
                        {
                            //if (value != null)
                            //{
                            //    return new ValidationResult(this.FormatErrorMessage(value.ToString()));
                            //}
                            return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));

                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }

            return null;
        }

        public RemoteWithServerSideAttribute(string routeName)
            : base()
        {
        }

        public RemoteWithServerSideAttribute(string action, string controller)
            : base(action, controller)
        {
        }

        public RemoteWithServerSideAttribute(string action, string controller, string areaName)
            : base(action, controller, areaName)
        {
        }

    }

    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        // Custom property
        //public ViewPermission RequiredPermission { get; set; }
        public long RequiredPermission { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            if (httpContext.User.Identity.IsAuthenticated)
            {
                var repo = new EFCpatickerRepository();
                var p = repo.GetCurrentUser().Permissions;
                var result = !((p & RequiredPermission) == RequiredPermission);
                return result;
            }

            return true;
        }

        // this works!!
        //protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        //{
        //    filterContext.Result = new RedirectToRouteResult(
        //            new RouteValueDictionary(
        //                new
        //                {
        //                    controller = "Home",
        //                    action = "AuthorizationError"
        //                })
        //            );

        //    //filterContext.Result = new RedirectResult(urlHelper.Action("Index", "Error"));
        //}

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("~/Account/Login?ReturnUrl=%2fadmin");
                return;
            }

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = "Error",
                            action = "index"
                        })
                    );
            }
        }
    }

    public class AuthorizeUserAttribute1 : AuthorizeAttribute
    {
        public long RequiredPermission { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            if (WebMatrix.WebData.WebSecurity.IsAuthenticated)
            {
                var repo = new EFCpatickerRepository();
                var p = repo.GetCurrentUser().Permissions1;
                var result = !((p & RequiredPermission) == RequiredPermission);
                return result;
            }
            return true;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("~/Account/Login?ReturnUrl=%2fadmin");
                return;
            }

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = "Error",
                            action = "index"
                        })
                    );
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class RequireHttpsIf : ValidationAttribute, IClientValidatable
    {
        string otherPropertyName;

        public RequireHttpsIf(string otherPropertyName, string errorMessage)
            : base(errorMessage)
        {
            this.otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            ValidationResult validationResult = ValidationResult.Success;

            if (value == null)
            {
                validationResult = new ValidationResult("Pixel is required.");
                return validationResult;
            }

            try
            {
                // Using reflection we can get a reference to the other date property, in this example the project start date
                var otherPropertyInfo = validationContext.ObjectType.GetProperty(this.otherPropertyName);
                // Let's check that otherProperty is of type TrackingType as we expect it to be
                if (otherPropertyInfo.PropertyType.Equals(typeof(TrackingType)))
                {
                    TrackingType tt = (TrackingType)otherPropertyInfo.GetValue(validationContext.ObjectInstance);

                    if (tt == TrackingType.HttpsiFrame || tt == TrackingType.HttpsImage)
                    {
                        //if (!Regex.IsMatch(value.ToString(), "src=(\"|'|(&quot;)|(&#39;))https"))
                        if (!Regex.IsMatch(value.ToString(), "https", RegexOptions.IgnoreCase))
                        {
                            validationResult = new ValidationResult(ErrorMessageString);
                        }
                    }

                    return validationResult;
                }
                else
                {
                    validationResult = new ValidationResult("An error occurred while validating the property. OtherProperty is not of type TrackingType");
                }
            }
            catch (Exception ex)
            {
                // Do stuff, i.e. log the exception
                // Let it go through the upper levels, something bad happened
                throw ex;
            }

            return validationResult;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            //string errorMessage = this.FormatErrorMessage(metadata.DisplayName);
            string errorMessage = ErrorMessageString;

            // The value we set here are needed by the jQuery adapter
            ModelClientValidationRule requireHttpsIfRule = new ModelClientValidationRule();
            requireHttpsIfRule.ErrorMessage = errorMessage;
            requireHttpsIfRule.ValidationType = "requirehttpsif"; // This is the name the jQuery adapter will use
            //"otherpropertyname" is the name of the jQuery parameter for the adapter, must be LOWERCASE!
            requireHttpsIfRule.ValidationParameters.Add("otherpropertyname", otherPropertyName);

            yield return requireHttpsIfRule;
        }
    }

    public class NeverValidAttribute : ValidationAttribute, IClientValidatable
    {
        public override bool IsValid(object value)
        {
            return false;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return new ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessage,
                ValidationType = "nevervalid"
            };
        }
    }

    public class CustomActionInvoker : ControllerActionInvoker
    {
        protected override FilterInfo GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor);

            string _action = actionDescriptor.ActionName.ToLower();

            // put the first char in upper case
            // _action = _action.First().ToString().ToUpper() + String.Join("", _action.Skip(1));
            string _controllername = actionDescriptor.ControllerDescriptor.ControllerName;

            Type enumtype = CPAHelper.GetEnumType();


            //if (_action == "customreport" || _action == "managereport" || _action == "deletemanagereport")
            //{ enumtype = CPAHelper.GetEnumType1(); }
            //for find name from assemblytype1 
            //  List of reports from GetEnumType1 which has parent controller is basecontroller
            //  customreport
            //  managereport
            //  deletemanagereport
            //


            var enumitemscheck = enumtype.GetField(_action + _controllername);

            try
            {
                if (enumitemscheck == null)
                {
                    enumtype = CPAHelper.GetEnumType1();
                    //ViewPermission vp = (ViewPermission)Enum.Parse(typeof(ViewPermission), sb.ToString());
                    System.Reflection.FieldInfo enumItem = enumtype.GetField(_action + _controllername);
                    //if (enumItem != null)
                    //{
                    long value = (long)enumItem.GetRawConstantValue();

                    AuthorizeUserAttribute1 _afilter = new AuthorizeUserAttribute1()
                    {
                        RequiredPermission = value
                    };

                    filters.AuthorizationFilters.Add(_afilter);
                    //  }
                }
                else
                {
                    //ViewPermission vp = (ViewPermission)Enum.Parse(typeof(ViewPermission), sb.ToString());
                    System.Reflection.FieldInfo enumItem = enumtype.GetField(_action + _controllername);
                    //if (enumItem != null)
                    //{
                    long value = (long)enumItem.GetRawConstantValue();

                    AuthorizeUserAttribute _afilter = new AuthorizeUserAttribute()
                    {
                        RequiredPermission = value
                    };

                    filters.AuthorizationFilters.Add(_afilter);
                }
            }
            catch//(Exception ex)
            //catch (System.ArgumentException)
            {
                // if we are here is because there is no associate permitions for the action
                // either because is not defined by forget or intentionally
                // do nothing...


                throw new Exception(_action);
            }


            return filters;
        }
    }

    public class CustomActionInvoker1 : ControllerActionInvoker
    {
        protected override FilterInfo GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor);

            string _action = actionDescriptor.ActionName.ToLower();

            // put the first char in upper case
            // _action = _action.First().ToString().ToUpper() + String.Join("", _action.Skip(1));
            string _controllername = actionDescriptor.ControllerDescriptor.ControllerName;

            Type enumtype = CPAHelper.GetEnumType1();

            try
            {
                System.Reflection.FieldInfo enumItem = enumtype.GetField(_action + _controllername);
                //if (enumItem != null)
                //{
                //ViewPermission vp = (ViewPermission)Enum.Parse(typeof(ViewPermission), sb.ToString());
                // System.Reflection.FieldInfo enumItem = enumtype.GetField(_action + _controllername);
                long value = (long)enumItem.GetRawConstantValue();


                //var repo = new EFCpatickerRepository();
                AuthorizeUserAttribute1 _afilter = new AuthorizeUserAttribute1()
                {
                    RequiredPermission = value,
                };

                filters.AuthorizationFilters.Add(_afilter);
                //}
            }
            catch//(Exception ex)
            //catch (System.ArgumentException)
            {
                // if we are here is because there is no associate permitions for the action
                // either because is not defined by forget or intentionally
                // do nothing...
                throw new Exception(_action);
            }


            return filters;
        }
    }

    public class ValidateFileAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            if (file == null)
            {
                return false;
            }

            //if (file.ContentLength > 1 * 1024 * 1024)
            //{
            //    return false;
            //}

            //try
            //{
            //    using (var img = Image.FromStream(file.InputStream))
            //    {
            //        return img.RawFormat.Equals(ImageFormat.Png);
            //    }
            //}
            //catch { }
            //return false;

            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class CacheControlAttribute : ActionFilterAttribute
    {
        public CacheControlAttribute(HttpCacheability cacheability)
        {
            this._cacheability = cacheability;
        }

        public HttpCacheability Cacheability { get { return this._cacheability; } }

        private HttpCacheability _cacheability;

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
            cache.SetCacheability(_cacheability);
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredArray : RequiredAttribute, IClientValidatable
    {
        public RequiredArray() : base() { }

        public override bool IsValid(object value)
        {
            var list = value as System.Collections.IList;
            if (list != null)
            {
                return list.Count > 0;
            }
            return false;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            string errorMessage = this.ErrorMessage;

            // Get the specific error message if set, otherwise the default
            if (string.IsNullOrEmpty(errorMessage) && metadata != null)
            {
                errorMessage = FormatErrorMessage(metadata.GetDisplayName());
            }

            var clientValidationRule = new ModelClientValidationRule()
            {
                ErrorMessage = errorMessage,
                ValidationType = "requiredarray"
            };

            //return new[] { clientValidationRule };
            yield return clientValidationRule;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class RequiredIfTypeAttribute : ValidationAttribute, IClientValidatable
    {
        public string PropertyName { get; set; }

        public int ExpectedValue { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var evalue = GetValue<int>(validationContext.ObjectInstance, PropertyName);
            //if (GetValue(validationContext.ObjectInstance, PropertyName).Equals(ExpectedValue))
            if ((evalue & ExpectedValue) == ExpectedValue)
            {
                return new RequiredAttribute().IsValid(value) ?
                    ValidationResult.Success :
                    new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }

        private static T GetValue<T>(object objectInstance, string propertyName)
        {
            if (objectInstance == null) throw new ArgumentNullException("objectInstance");
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException("propertyName");

            var propertyInfo = objectInstance.GetType().GetProperty(propertyName);
            return (T)propertyInfo.GetValue(objectInstance);
        }

        private static Type GetPropertyType(object objectInstance, string propertyName)
        {
            if (objectInstance == null) throw new ArgumentNullException("objectInstance");
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException("propertyName");

            var propertyInfo = objectInstance.GetType().GetProperty(propertyName);
            return propertyInfo.PropertyType;
        }

        public static object GetValue(object objectInstance, string propertyName)
        {
            if (objectInstance == null) throw new ArgumentNullException("objectInstance");
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException("propertyName");

            var propertyInfo = objectInstance.GetType().GetProperty(propertyName);
            return propertyInfo.GetValue(objectInstance);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var modelClientValidationRule = new ModelClientValidationRule
            {
                ValidationType = "requirediftype",
                ErrorMessage = FormatErrorMessage(metadata.DisplayName)
            };
            modelClientValidationRule.ValidationParameters.Add("property", PropertyName);
            modelClientValidationRule.ValidationParameters.Add("expected", ExpectedValue);
            yield return modelClientValidationRule;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class RequiredIfNotAttribute : ValidationAttribute, IClientValidatable
    {
        public string PropertyName { get; set; }

        public object ExpectedValue { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!GetValue(validationContext.ObjectInstance, PropertyName).Equals(ExpectedValue))
            {
                return new RequiredAttribute().IsValid(value) ?
                    ValidationResult.Success :
                    new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }

        public static object GetValue(object objectInstance, string propertyName)
        {
            if (objectInstance == null) throw new ArgumentNullException("objectInstance");
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException("propertyName");

            var propertyInfo = objectInstance.GetType().GetProperty(propertyName);
            return propertyInfo.GetValue(objectInstance);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var modelClientValidationRule = new ModelClientValidationRule
            {
                ValidationType = "requiredifnot",
                ErrorMessage = FormatErrorMessage(metadata.DisplayName)
            };
            modelClientValidationRule.ValidationParameters.Add("property", PropertyName);
            modelClientValidationRule.ValidationParameters.Add("expected", ExpectedValue);
            yield return modelClientValidationRule;
        }
    }

}