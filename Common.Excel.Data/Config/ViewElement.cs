using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Ganji.ExcelManagement.Config
{
    public sealed class ViewElement : ConfigurationElement
    {
        private static ConfigurationPropertyCollection properties;

        private static ConfigurationProperty nameProp;
        private static ConfigurationProperty exportProp;
        private static ConfigurationProperty tableProp;

        static ViewElement()
        {
            // Predefine properties here
            nameProp = new ConfigurationProperty(
                ConfigConstants.Name_Property,
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            exportProp = new ConfigurationProperty(
                ConfigConstants.ExportToNew_Property,
                typeof(bool),
                false,
                ConfigurationPropertyOptions.None
            );

            tableProp = new ConfigurationProperty(
                ConfigConstants.Table_Element,
                typeof(TableElement),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            properties = new ConfigurationPropertyCollection();

            properties.Add(nameProp);
            properties.Add(exportProp);
            properties.Add(tableProp);
        }

        public ViewElement()
        {
        }

        [ConfigurationProperty(ConfigConstants.Name_Property, IsKey = true, IsRequired = true)]
        public string ViewName
        {
            get { return (string)base[nameProp]; }
            set { base[nameProp] = value; }
        }

        [ConfigurationProperty(ConfigConstants.ExportToNew_Property, DefaultValue = false)]
        public bool IsExportToNew
        {
            get { return (bool)this[ConfigConstants.ExportToNew_Property]; }
            set { this[ConfigConstants.ExportToNew_Property] = value; }
        }

        [ConfigurationProperty(ConfigConstants.Table_Element, IsRequired = true)]
        public TableElement Table
        {
            get { return (TableElement)this[ConfigConstants.Table_Element]; }
        }
    }
}
