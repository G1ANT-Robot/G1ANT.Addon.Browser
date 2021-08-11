using Autofac;
using G1ANT.Browser.Driver.Interfaces;
using G1ANT.Browser.Driver.Services;
using G1ANT.Chrome.Driver;
using G1ANT.Chromium.Host;
using System;

namespace G1ANT.Chrome.Host
{
    class Program
    {
        static Program()
        {
            SetAssemblyResolver();
        }

        [STAThread]
        static void Main(string[] args)
        {
            var container = BuildContainer();
            container.Resolve<ChromeHostWorker>().Run(args);
        }

        static void SetAssemblyResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResourceAssemblyResolver.Resolve;
        }

        static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterAssemblyModules(
                typeof(ChromeClient).Assembly,
                typeof(ChromiumHost).Assembly);
            
            builder.RegisterType<ChromeEventsClient>();
            builder.RegisterType<ChromeHostWorker>();
            builder.RegisterType<ChromeServer<ChromeActionService>>()
                .SingleInstance();
            builder.RegisterType<ChromeActionService>()
                .AsSelf()
                .As<ChromiumActionService>()
                .As<BrowserActionService>()
                .SingleInstance();
            builder.RegisterType<ChromeHost>()
                .AsSelf()
                .As<ChromiumHost>()
                .As<ChromeHost>()
                .SingleInstance();
            return builder.Build();
        }
    }
}
