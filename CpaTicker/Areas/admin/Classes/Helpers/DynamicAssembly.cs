using CpaTicker.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Web;
using System.Web.Mvc;

namespace CpaTicker.Areas.admin.Classes.Helpers
{
    public class DynamicAssembly
    {
        //public static void Create()
        //{
        //    // Get the current application domain for the current thread.
        //    AppDomain currentDomain = AppDomain.CurrentDomain;

        //    // Get the path for the assembly
        //    string path = HttpRuntime.AppDomainAppPath + "\\bin";

        //    // Create a dynamic assembly in the current application domain,  
        //    // and allow it to be executed and saved to disk.
        //    AssemblyName aName = new AssemblyName("EnumAssembly");
        //    AssemblyBuilder ab = currentDomain.DefineDynamicAssembly(
        //        aName, AssemblyBuilderAccess.RunAndSave, path);

        //    // Define a dynamic module in "TempAssembly" assembly. For a single-
        //    // module assembly, the module has the same name as the assembly.
        //    ModuleBuilder mb = ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

        //    // Define a public enumeration with the name "Elevation" and an 
        //    // underlying type of Integer.
        //    EnumBuilder eb = mb.DefineEnum("ViewPermissiond", TypeAttributes.Public, typeof(long));

        //    // Set the flag attribute
        //    eb.SetCustomAttribute(new CustomAttributeBuilder(typeof(FlagsAttribute).GetConstructor(Type.EmptyTypes), new object[] { }));
        //    List<string> obj = new List<string>();
        //    int pot = 0;
        //    foreach (var controllerType in GetSubClasses<CpaTicker.Areas.admin.Controllers.BaseController>())
        //    {
        //        foreach (var action in ActionNames(controllerType))
        //        {
        //            obj.Add(action + controllerType.Name);
        //            if (action + controllerType.Name != "indexHomeController")
        //            {
        //                if (pot < 63)
        //                {
        //                    eb.DefineLiteral(String.Format("{0}{1}", action,
        //                           controllerType.Name.Substring(0, controllerType.Name.Length - 10)),  // 10 = "Controller".Length
        //                           Convert.ToInt64(Math.Pow(2, pot++)));
        //                }
        //            }   
        //        }
        //    }

        //    // Create the type and save the assembly. 
        //    Type finished = eb.CreateType();

        //    /////////////////////////////////////////////////////////////////////////////////////////////////////////

        //    // Define a public enumeration with the name "Elevation" and an 
        //    // underlying type of Integer.
        //    eb = mb.DefineEnum("ViewPermission", TypeAttributes.Public, typeof(long));

        //    // Set the flag attribute
        //    eb.SetCustomAttribute(new CustomAttributeBuilder(typeof(FlagsAttribute).GetConstructor(Type.EmptyTypes), new object[] { }));


        //    pot = 0;
        //    foreach (var controllerType in GetSubClasses<CpaTicker.Areas.admin.Controllers.BaseController1>())
        //    {

        //        foreach (var action in ActionNames(controllerType))
        //        {

        //            if (action + controllerType.Name != "indexHomeController")
        //            {

        //                if (pot < 63)
        //                {
        //                    eb.DefineLiteral(String.Format("{0}{1}", action,
        //                       controllerType.Name.Substring(0, controllerType.Name.Length - 10)),  // 10 = "Controller".Length
        //                       Convert.ToInt64(Math.Pow(2, pot++)));
        //                }
        //            }


        //        }
        //    }
        //    // Create the type and save the assembly. 
        //    finished = eb.CreateType();
        //    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //    /////

        //    ab.Save(aName.Name + ".dll");
        //}












        /*Modified by Dharmesh to recreate assembly using already assigned bits*/

        public static void Create()
        {
            // Get the current application domain for the current thread.
            AppDomain currentDomain = AppDomain.CurrentDomain;

            // Get the path for the assembly
            string path = HttpRuntime.AppDomainAppPath + "\\bin";

            // Create a dynamic assembly in the current application domain,  
            // and allow it to be executed and saved to disk.
            AssemblyName aName = new AssemblyName("EnumAssembly");
            AssemblyBuilder ab = currentDomain.DefineDynamicAssembly(
                aName, AssemblyBuilderAccess.RunAndSave, path);

            // Define a dynamic module in "TempAssembly" assembly. For a single-
            // module assembly, the module has the same name as the assembly.
            ModuleBuilder mb = ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

            // Define a public enumeration with the name "Elevation" and an 
            // underlying type of Integer.
            EnumBuilder eb = mb.DefineEnum("ViewPermissiond", TypeAttributes.Public, typeof(long));

            // Set the flag attribute
            eb.SetCustomAttribute(new CustomAttributeBuilder(typeof(FlagsAttribute).GetConstructor(Type.EmptyTypes), new object[] { }));
            List<string> obj = new List<string>();

            SignalRNotifyController cntrl = new SignalRNotifyController();
            int pot = 0;
            var result = cntrl.GetBitsAssembly();


            for (int a = 0; a < result.Count; a++)
            {
                eb.DefineLiteral(result[a].name, result[a].key);
            }

            // Create the type and save the assembly. 
            Type finished = eb.CreateType();

            /////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Define a public enumeration with the name "Elevation" and an 
            // underlying type of Integer.
            eb = mb.DefineEnum("ViewPermission", TypeAttributes.Public, typeof(long));

            // Set the flag attribute
            eb.SetCustomAttribute(new CustomAttributeBuilder(typeof(FlagsAttribute).GetConstructor(Type.EmptyTypes), new object[] { }));


            pot = 0;
            foreach (var controllerType in GetSubClasses<CpaTicker.Areas.admin.Controllers.BaseController1>())
            {

                foreach (var action in ActionNames(controllerType))
                {

                    if (action + controllerType.Name != "indexHomeController")
                    {

                        if (pot < 63)
                        {
                            eb.DefineLiteral(String.Format("{0}{1}", action,
                               controllerType.Name.Substring(0, controllerType.Name.Length - 10)),  // 10 = "Controller".Length
                               Convert.ToInt64(Math.Pow(2, pot++)));
                        }
                    }


                }
            }
            eb.DefineLiteral(String.Format("{0}", "customreportReports"), Convert.ToInt64(Math.Pow(2, pot++)));
            eb.DefineLiteral(String.Format("{0}", "managereportReports"), Convert.ToInt64(Math.Pow(2, pot++)));
            eb.DefineLiteral(String.Format("{0}", "deletemanagereportReports"), Convert.ToInt64(Math.Pow(2, pot++)));
            eb.DefineLiteral(String.Format("{0}", "clicksdetailslogReports"), Convert.ToInt64(Math.Pow(2, pot++)));

            

            eb.DefineLiteral(String.Format("{0}", "affiliatepayoutCampaign"), Convert.ToInt64(Math.Pow(2, pot++)));
            eb.DefineLiteral(String.Format("{0}", "createaffiliatepayoutCampaign"), Convert.ToInt64(Math.Pow(2, pot++)));
            eb.DefineLiteral(String.Format("{0}", "editaffiliatepayoutCampaign"), Convert.ToInt64(Math.Pow(2, pot++)));
            eb.DefineLiteral(String.Format("{0}", "addaffiliateoverrideCampaign"), Convert.ToInt64(Math.Pow(2, pot++)));
            eb.DefineLiteral(String.Format("{0}", "updateaffiliateoverrideCampaign"), Convert.ToInt64(Math.Pow(2, pot++)));

            eb.DefineLiteral(String.Format("{0}", "admin_master_loginSettings"), Convert.ToInt64(Math.Pow(2, pot++)));
            eb.DefineLiteral(String.Format("{0}", "addcustomtimezoneSettings"), Convert.ToInt64(Math.Pow(2, pot++)));
            eb.DefineLiteral(String.Format("{0}", "editcustomtimezoneSettings"), Convert.ToInt64(Math.Pow(2, pot++)));


            eb.DefineLiteral(String.Format("{0}", "indexPAGE"), Convert.ToInt64(Math.Pow(2, pot++)));
            eb.DefineLiteral(String.Format("{0}", "createPAGE"), Convert.ToInt64(Math.Pow(2, pot++)));
            eb.DefineLiteral(String.Format("{0}", "editPAGE"), Convert.ToInt64(Math.Pow(2, pot++)));
            eb.DefineLiteral(String.Format("{0}", "getpagecategoriesPAGE"), Convert.ToInt64(Math.Pow(2, pot++)));
            eb.DefineLiteral(String.Format("{0}", "savepagecategoryPAGE"), Convert.ToInt64(Math.Pow(2, pot++)));
            eb.DefineLiteral(String.Format("{0}", "removepagecategoryPAGE"), Convert.ToInt64(Math.Pow(2, pot++)));

            eb.DefineLiteral(String.Format("{0}", "overridepayoutAffiliate"), Convert.ToInt64(Math.Pow(2, pot++)));
            eb.DefineLiteral(String.Format("{0}", "createoverridepayoutAffiliate"), Convert.ToInt64(Math.Pow(2, pot++)));
            eb.DefineLiteral(String.Format("{0}", "editoverridepayoutAffiliate"), Convert.ToInt64(Math.Pow(2, pot++)));
            eb.DefineLiteral(String.Format("{0}", "addaffiliateoverrideAffiliate"), Convert.ToInt64(Math.Pow(2, pot++)));
            eb.DefineLiteral(String.Format("{0}", "updateaffiliateoverrideAffiliate"), Convert.ToInt64(Math.Pow(2, pot++)));
            // Create the type and save the assembly. 
            finished = eb.CreateType();
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////

            /////

            ab.Save(aName.Name + ".dll");
        }



        private static List<Type> GetSubClasses<T>()
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(
                type => type.IsSubclassOf(typeof(T))).ToList();
        }

        private static List<string> ActionNames(Type controllerType)
        {
            //return new ReflectedControllerDescriptor(controllerType).GetCanonicalActions().Select(x => x.ActionName).ToList();

            //return new ReflectedControllerDescriptor(controllerType).GetCanonicalActions().
            //    Where(
            //    x => !x.IsDefined(typeof(HttpPostAttribute), false) &&
            //    (((ReflectedActionDescriptor)x).MethodInfo.ReturnType == typeof(ActionResult) || ((ReflectedActionDescriptor)x).MethodInfo.ReturnType.IsSubclassOf(typeof(ActionResult)))
            //    ).Select(x => x.ActionName.ToLower()).ToList();

            ActionDescriptor[] adlist = new ReflectedControllerDescriptor(controllerType).GetCanonicalActions();
            List<string> rlist = new List<string>();
            foreach (var action in adlist)
            {

                // this is for skip actions with the httppostattribute
                if (action.IsDefined(typeof(HttpPostAttribute), false))
                    continue;
                ReflectedActionDescriptor rad = action as ReflectedActionDescriptor;
                if (rad != null)
                {
                    if (rad.MethodInfo.ReturnType == typeof(ActionResult) || rad.MethodInfo.ReturnType.IsSubclassOf(typeof(ActionResult)))
                        rlist.Add(rad.ActionName.ToLower());
                }

            }

            return rlist;

        }

        private static List<string> GetControllerNames()
        {
            List<string> controllerNames = new List<string>();
            GetSubClasses<Controller>().ForEach(
                type => controllerNames.Add(type.Name));
            return controllerNames;
        }
    }




    public static class DynamicType
    {
        public static TypeBuilder CreateTypeBuilder(string assemblyName, string moduleName, string typeName)
        {
            TypeBuilder typeBuilder = AppDomain
                .CurrentDomain
                .DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.Run)
                .DefineDynamicModule(moduleName)
                .DefineType(typeName, TypeAttributes.Public);
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
            return typeBuilder;
        }

        public static void CreateAutoImplementedProperty(
            TypeBuilder builder, string propertyName, Type propertyType)
        {
            const string PrivateFieldPrefix = "m_";
            const string GetterPrefix = "get_";
            const string SetterPrefix = "set_";

            // Generate the field.
            FieldBuilder fieldBuilder = builder.DefineField(
                string.Concat(PrivateFieldPrefix, propertyName), propertyType, FieldAttributes.Private);

            // Generate the property
            PropertyBuilder propertyBuilder = builder.DefineProperty(
                propertyName, PropertyAttributes.HasDefault, propertyType, null);

            // Property getter and setter attributes.
            MethodAttributes propertyMethodAttributes =
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            // Define the getter method.
            MethodBuilder getterMethod = builder.DefineMethod(
                string.Concat(GetterPrefix, propertyName),
                propertyMethodAttributes, propertyType, Type.EmptyTypes);

            // Emit the IL code.
            // ldarg.0
            // ldfld,_field
            // ret
            ILGenerator getterILCode = getterMethod.GetILGenerator();
            getterILCode.Emit(OpCodes.Ldarg_0);
            getterILCode.Emit(OpCodes.Ldfld, fieldBuilder);
            getterILCode.Emit(OpCodes.Ret);

            // Define the setter method.
            MethodBuilder setterMethod = builder.DefineMethod(
                string.Concat(SetterPrefix, propertyName),
                propertyMethodAttributes, null, new Type[] { propertyType });

            // Emit the IL code.
            // ldarg.0
            // ldarg.1
            // stfld,_field
            // ret
            ILGenerator setterILCode = setterMethod.GetILGenerator();
            setterILCode.Emit(OpCodes.Ldarg_0);
            setterILCode.Emit(OpCodes.Ldarg_1);
            setterILCode.Emit(OpCodes.Stfld, fieldBuilder);
            setterILCode.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getterMethod);
            propertyBuilder.SetSetMethod(setterMethod);
        }
    }
}