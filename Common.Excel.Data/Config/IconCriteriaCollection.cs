using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using GanJi.SEM.UICommon.Config;

namespace Ganji.ExcelManagement.Config
{
    public sealed class IconCriteriaCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new IconCriterionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IconCriterionElement)element).Name;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return ConfigConstants.IconCriterion_Element; }
        }

        public IconCriterionElement this[int index]
        {
            get { return (IconCriterionElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public ParameterElement this[string paramName]
        {
            get { return (ParameterElement)BaseGet(paramName); }
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
