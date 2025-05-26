using System;


namespace NexaShopify.Core.Shop.Enums
{
    public static class ExtensionsClass
    {
        public static string GetDescription(this Enum value)
        {
            // adda 04/01/2025 --> reflection mechanism --> review that after may that's not neccessary
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                System.Reflection.FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    System.ComponentModel.DescriptionAttribute attr =
                            Attribute.GetCustomAttribute(field,
                                typeof(System.ComponentModel.DescriptionAttribute)) as System.ComponentModel.DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }
    }
}
