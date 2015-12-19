using System;

namespace Stock.Common
{
    public class EnumDescriptionAttribute : Attribute
    {
        private readonly string _strDescription;
        public EnumDescriptionAttribute(string strEnumDescription)
        {
            _strDescription = strEnumDescription;
        }

        public string Description { get { return _strDescription; } }

        public static string GetEnumDescription(Enum enumObj)
        {
            System.Reflection.FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());
            object[] attribArray = fieldInfo.GetCustomAttributes(false);
            if (attribArray.Length == 0)
                return String.Empty;
            else
            {
                EnumDescriptionAttribute attrib = attribArray[0] as EnumDescriptionAttribute;

                return attrib.Description;
            }
        }
        public static T GetAttribute<T>(Enum enumObj)
            where T : EnumDescriptionAttribute
        {
            System.Reflection.FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());
            object[] attribArray = fieldInfo.GetCustomAttributes(false);
            if (attribArray.Length == 0)
                return default(T);
            else
            {
                T attrib = (T)attribArray[0];
                if (attrib != null)
                    return attrib;
                else
                    return default(T);
            }
        }
    }
}