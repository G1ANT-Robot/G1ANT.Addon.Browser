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
        [STAThread]
        static void Main(string[] args)
        {
            var container = BuildContainer();
            container.Resolve<ChromeService>().Run(args);
        }

        static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ChromeEventsClient>();
            builder.RegisterType<ChromeService>();
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
