using System;

namespace Sintering_of_ceramics.Helpers
{
    public static class ClassHelper
    {
        public static T? GetAttributeOfType<T>(Type classVal, string propName) where T : System.Attribute
        {
            var memInfo = classVal.GetMember(propName);
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }
}
