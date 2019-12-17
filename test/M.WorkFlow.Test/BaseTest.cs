using Autofac;
using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Serialize;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit.Abstractions;

namespace M.WorkFlow.Test
{
    public class BaseTest
    {
        protected IContainer _container;
        protected ITestOutputHelper _testOutputHelper;
        private string bllDllName = string.Empty;
        public BaseTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder.AddJsonFile("appsettings.json", true).AddJsonFile("M.WorkFlow.Test.json", true).Build();
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<JsonConverter>().As<IJsonConverter>();

            foreach (var load in GetComponets())
            {
                load.LoadComponent(builder, configuration);
            }
            if (string.IsNullOrWhiteSpace(bllDllName))
            {
                builder.RegisterBllModule(bllDllName);
            }
            _container = builder.Build();
        }
        List<IComponent> GetComponets()
        {
            bllDllName = "M.WFDesigner.dll";

            List<IComponent> list = new List<IComponent>();
            list.Add(new FD.Component.DB.LoadDB());
            return list;
        }
    }

    /// <summary>
    /// 注册dll到DI容器，后续此功能增加到apiloader
    /// </summary>
    /// <param name="containerBuilder"></param>
    /// <param name="dllName"></param>
    public static class ContainerBuilderExtensions
    {
        public static void RegisterBllModule(this ContainerBuilder containerBuilder, string dllName)
        {
            var basePath = AppContext.BaseDirectory;
            Assembly moduleAssembly = null;
            moduleAssembly = Assembly.LoadFrom(Path.Combine(basePath, dllName));

            //autowired  class实现
            var propertySelector = new AutowiredPropertySelector();

            var typeAutowired = moduleAssembly.GetTypes().Where(t => t.GetCustomAttribute<AutowiredAttribute>() != null && !t.GetTypeInfo().IsAbstract);
            foreach (var t in typeAutowired)
            {
                if (t.GetInterfaces().Count() == 0)
                {
                    containerBuilder.RegisterType(t).PropertiesAutowired(propertySelector, false).SingleInstance();
                }
                else
                {
                    containerBuilder.RegisterType(t).AsImplementedInterfaces().PropertiesAutowired(propertySelector).SingleInstance();
                }
            }
        }
    }

}
