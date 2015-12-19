using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Ganji.ExcelManagement.Config
{
    public sealed class IconCriterionElement : ConfigurationElement
    {
        private static ConfigurationPropertyCollection properties;

        private static ConfigurationProperty nameProp;
        private static ConfigurationProperty indexProp;
        private static ConfigurationProperty operatorProp;
        private static ConfigurationProperty valueProp;
        private static ConfigurationProperty typeProp;

        static IconCriterionElement()
        {
            // Predefine properties here
            nameProp = new ConfigurationProperty(
                ConfigConstants.Name_Property,
                typeof(string),
                string.Empty,
                ConfigurationPropertyOptions.IsRequired
            );

            indexProp = new ConfigurationProperty(
                ConfigConstants.Index_Property,
                typeof(int),
                0,
                ConfigurationPropertyOptions.None
            );

            operatorProp = new ConfigurationProperty(
                ConfigConstants.Operator_Property,
                typeof(string),
                "xlGreaterEqual",
                ConfigurationPropertyOptions.None
            );

            valueProp = new ConfigurationProperty(
                ConfigConstants.Value_Property,
                typeof(double),
                0.0D,
                ConfigurationPropertyOptions.IsRequired
            );

            typeProp = new ConfigurationProperty(
                ConfigConstants.Type_Property,
                typeof(string),
                "xlConditionValuePercent",
                ConfigurationPropertyOptions.None
            );

            properties = new ConfigurationPropertyCollection();

            properties.Add(nameProp);
            properties.Add(indexProp);
            properties.Add(operatorProp);
            properties.Add(valueProp);
            properties.Add(typeProp);
        }

        public IconCriterionElement()
        {
        }

        [ConfigurationProperty(ConfigConstants.Name_Property, IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this[ConfigConstants.Name_Property]; }
            set { this[ConfigConstants.Name_Property] = value; }
        }

        [ConfigurationProperty(ConfigConstants.Index_Property)]
        public int Index
        {
            get { return (int)this[ConfigConstants.Index_Property]; }
            set { this[ConfigConstants.Index_Property] = value; }
        }

        [ConfigurationProperty(ConfigConstants.Operator_Property, IsKey = true)]
        public string Operator
        {
            get { return (string)this[ConfigConstants.Operator_Property]; }
            set { this[ConfigConstants.Operator_Property] = value; }
        }

        [ConfigurationProperty(ConfigConstants.Value_Property, IsRequired = true)]
        public double Value
        {
            get { return (double)this[ConfigConstants.Value_Property]; }
            set { this[ConfigConstants.Value_Property] = value; }
        }

        [ConfigurationProperty(ConfigConstants.Type_Property)]
        public string ValueType
        {
            get { return (string)this[ConfigConstants.Type_Property]; }
            set { this[ConfigConstants.Type_Property] = value; }
        }

        /// <summary>
        /// Override the Properties collection and return our custom one.
        /// </summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get { return properties; }
        }
    }
}
