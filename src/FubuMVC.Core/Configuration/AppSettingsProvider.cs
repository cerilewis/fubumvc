using System;
using FubuMVC.Core.Models;
using FubuMVC.Core.Runtime;
using Microsoft.Practices.ServiceLocation;

namespace FubuMVC.Core.Configuration
{
    public class AppSettingsProvider : ISettingsProvider
    {
        private readonly IServiceLocator _locator;
        private readonly IObjectResolver _resolver;

        public AppSettingsProvider(IObjectResolver resolver, IServiceLocator locator)
        {
            _resolver = resolver;
            _locator = locator;
        }

        public T SettingsFor<T>() where T : class, new()
        {
            Type settingsType = typeof (T);

            object value = SettingsFor(settingsType);

            return (T) value;
        }

        public object SettingsFor(Type settingsType)
        {
            IBindingContext context = new BindingContext(new AppSettingsRequestData(), _locator)
                .PrefixWith(settingsType.Name + ".");

            BindResult result = _resolver.BindModel(settingsType, context);

            result.AssertProblems(settingsType);

            return result.Value;
        }
    }
}