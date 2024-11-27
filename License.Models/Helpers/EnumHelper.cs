using System.ComponentModel;
using System.Reflection;

namespace License.Models.Helpers
{
    public class EnumHelper
    {
        public static List<EnumItem> GetEnumList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                       .Cast<T>()
                       .Select(e => new EnumItem
                       {
                           Name = GetEnumDescription(e),
                           Value = Convert.ToInt32(e)
                       })
                       .ToList();
        }

        private static string GetEnumDescription<T>(T enumValue) where T : Enum
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var attribute = field!.GetCustomAttribute<DescriptionAttribute>();
            return attribute != null ? attribute.Description : enumValue.ToString();
        }
        public class EnumItem
        {
            public string Name { get; set; } = string.Empty;
            public int Value { get; set; }
        }
    }
}
