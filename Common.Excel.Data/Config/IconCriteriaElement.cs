using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Ganji.ExcelManagement.Config
{
    public sealed class IconCriteriaElement : ConfigurationElement
    {
        private static ConfigurationPropertyCollection properties;

        private static ConfigurationProperty iconSetProp;
        private static ConfigurationProperty reverseProp;
        private static ConfigurationProperty showIconOnlyProp;
        private static ConfigurationProperty iconCriteriaProp;

        static IconCriteriaElement()
        {
            // Predefine properties here
            iconSetProp = new ConfigurationProperty(
                ConfigConstants.IconSet_Property,
                typeof(string),
                string.Empty,
                ConfigurationPropertyOptions.IsRequired
            );

            reverseProp = new ConfigurationProperty(
                ConfigConstants.ReverseOrder_Property,
                typeof(bool),
                false,
                ConfigurationPropertyOptions.None
            );


            showIconOnlyProp = new ConfigurationProperty(
                ConfigConstants.ShowIconOnly_Property,
                typeof(bool),
                false,
                ConfigurationPropertyOptions.None
            );

            iconCriteriaProp = new ConfigurationProperty(
                null,
                typeof(IconCriteriaCollection),
                null,
                ConfigurationPropertyOptions.IsDefaultCollection
            );

            properties = new ConfigurationPropertyCollection();

            properties.Add(iconSetProp);
            properties.Add(reverseProp);
            properties.Add(showIconOnlyProp);
            properties.Add(iconCriteriaProp);
        }

        public IconCriteriaElement()
        {
        }

        [ConfigurationProperty(ConfigConstants.IconSet_Property, IsRequired = true)]
        public string IconSet
        {
            get { return (string)this[iconSetProp]; }
            set { this[iconSetProp] = value; }
        }

        [ConfigurationProperty(ConfigConstants.ShowIconOnly_Property, DefaultValue = false)]
        public bool ShowIconOnly
        {
            get { return (bool)this[showIconOnlyProp]; }
            set { this[showIconOnlyProp] = value; }
        }

        [ConfigurationProperty(ConfigConstants.ReverseOrder_Property, DefaultValue = false)]
        public bool ReverseOrder
        {
            get { return (bool)this[reverseProp]; }
            set { this[reverseProp] = value; }
        }

        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public IconCriteriaCollection All
        {
            get
            {
                return (IconCriteriaCollection)base[iconCriteriaProp];
            }
        }
    }
}
