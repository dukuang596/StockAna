using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Ganji.ExcelManagement.Config
{
    public sealed class ViewCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ViewElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ViewElement)element).ViewName;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return ConfigConstants.View_Element; }
        }

        public ViewElement this[int index]
        {
            get { return (ViewElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        new public ViewElement this[string viewName]
        {
            get { return (ViewElement)BaseGet(viewName); }
        }

        public bool ContainsKey(string key)
        {
            bool result = false;

            object[] keys = BaseGetAllKeys();
            foreach (object obj in keys)
            {
                if ((string)obj == key)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}
