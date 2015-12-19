using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Ganji.ExcelManagement.Config
{
    public sealed class TableElement : ConfigurationElement
    {
        private static ConfigurationPropertyCollection properties;

        private static ConfigurationProperty styleProp;
        private static ConfigurationProperty orderByProp;
        private static ConfigurationProperty fcondProp;
        private static ConfigurationProperty autoFitProp;
        private static ConfigurationProperty columnsProp;

        static TableElement()
        {
            // Predefine properties here
            styleProp = new ConfigurationProperty(
                ConfigConstants.Style_Property,
                typeof(string),
                string.Empty,
                ConfigurationPropertyOptions.IsRequired
            );

            orderByProp = new ConfigurationProperty(
                ConfigConstants.OrderBy_Property,
                typeof(string),
                string.Empty,
                ConfigurationPropertyOptions.None
            );

            fcondProp = new ConfigurationProperty(
                ConfigConstants.FormatCondition_Element,
                typeof(bool),
                true,
                ConfigurationPropertyOptions.None
            );

            autoFitProp = new ConfigurationProperty(
                ConfigConstants.AutoFit_Property,
                typeof(bool),
                true,
                ConfigurationPropertyOptions.None
            );

            columnsProp = new ConfigurationProperty(
                null,
                typeof(ColumnCollection),
                null,
                ConfigurationPropertyOptions.IsDefaultCollection
            );

            properties = new ConfigurationPropertyCollection();

            properties.Add(styleProp);
            properties.Add(orderByProp);
            properties.Add(fcondProp);
            properties.Add(autoFitProp);
            properties.Add(columnsProp);
        }

        public TableElement()
        {
        }

        [ConfigurationProperty(ConfigConstants.Style_Property, IsRequired = true)]
        public string TableStyle
        {
            get { return (string)this[styleProp]; }
            set { this[styleProp] = value; }
        }

        [ConfigurationProperty(ConfigConstants.OrderBy_Property)]
        public string OrderBy
        {
            get { return (string)this[orderByProp]; }
            set { this[orderByProp] = value; }
        }

        [ConfigurationProperty(ConfigConstants.FormatCondition_Element, DefaultValue = true)]
        public bool SupportFormatCondition
        {
            get { return (bool)this[fcondProp]; }
            set { this[fcondProp] = value; }
        }

        [ConfigurationProperty(ConfigConstants.AutoFit_Property, DefaultValue = true)]
        public bool SupportColumnAutoFit
        {
            get { return (bool)this[autoFitProp]; }
            set { this[autoFitProp] = value; }
        }

        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public ColumnCollection Columns
        {
            get
            {
                return (ColumnCollection)base[columnsProp];
            }
        }
    }
}
