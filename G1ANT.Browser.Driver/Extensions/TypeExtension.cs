using G1ANT.Browser.Driver.Interfaces;
using System;
using System.ServiceModel;

namespace G1ANT.Browser.Driver.Extensions
{
    public static class TypeExtension
    {
        public static string  ServiceContract_Name(this Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(ServiceContractAttribute), true);
            return attributes?.Length == 1 ? ((ServiceContractAttribute)attributes[0]).Name : null;
        }

        public static bool ServiceContract_HasCallbackContract(this Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(ServiceContractAttribute), true);
            return attributes?.Length == 1 ? ((ServiceContractAttribute)attributes[0]).CallbackContract != null : false;
        }
    }
}
