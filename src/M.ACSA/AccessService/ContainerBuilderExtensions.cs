using Autofac;
using FD.Simple.Utils.Agent;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace M.ACSA.AccessService
{
    /// <summary>
    /// 注册dll到DI容器，后续此功能增加到apiloader
    /// </summary>
    /// <param name="containerBuilder"></param>
    /// <param name="dllName"></param>
    //public static class ContainerBuilderExtensions
    //{
    //    public static void RegisterBllModule(this ContainerBuilder containerBuilder, string dllName)
    //    {
    //        var basePath = AppContext.BaseDirectory;
    //        Assembly moduleAssembly = null;
    //        moduleAssembly = Assembly.LoadFrom(Path.Combine(basePath, dllName));

    //        //autowired  class实现
    //        var propertySelector = new AutowiredPropertySelector();

    //        var typeAutowired = moduleAssembly.GetTypes().Where(t => t.GetCustomAttribute<AutowiredAttribute>() != null && !t.GetTypeInfo().IsAbstract);
    //        foreach (var t in typeAutowired)
    //        {
    //            if (t.GetInterfaces().Count() == 0)
    //            {
    //                containerBuilder.RegisterType(t).PropertiesAutowired(propertySelector, false).SingleInstance();
    //            }
    //            else
    //            {
    //                containerBuilder.RegisterType(t).AsImplementedInterfaces().PropertiesAutowired(propertySelector).SingleInstance();
    //            }
    //        }
    //    }
    //}
}
