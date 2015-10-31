using System.ComponentModel;
using System.Reflection;

namespace FineBot.API.Extensions
{
    public static class EnumerationExtensions
    {
        public static string GetDescription(this object enumValue) {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            if (null != fi) {
                object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return "";
        }
    }
}