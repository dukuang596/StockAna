using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using GanJi.SEM.UICommon.Config;

namespace Ganji.ExcelManagement.Config
{
    public sealed class FormatConditionElement : ConfigurationElement
    {
        private static ConfigurationPropertyCollection properties;

        private static ConfigurationProperty typeProp;
        private static ConfigurationProperty paramsProp;
        private static ConfigurationProperty iconCriteriaProp;

        static FormatConditionElement()
        {
            // Predefine properties here
            typeProp = new ConfigurationProperty(
                ConfigConstants.Type_Property,
                typeof(FormatTypeEnum),
                FormatTypeEnum.None,
                ConfigurationPropertyOptions.None
            );

            paramsProp = new ConfigurationProperty(
                null,
                typeof(ParameterCollection),
                null,
                ConfigurationPropertyOptions.IsDefaultCollection
            );

            iconCriteriaProp = new ConfigurationProperty(
                ConfigConstants.IconCriteria_Element,
                typeof(IconCriteriaElement),
                null,
                ConfigurationPropertyOptions.None
            );

            properties = new ConfigurationPropertyCollection();

            properties.Add(typeProp);
            properties.Add(paramsProp);
            properties.Add(iconCriteriaProp);
        }

        public FormatConditionElement()
        {
        }

        [ConfigurationProperty(ConfigConstants.Type_Property, DefaultValue = FormatTypeEnum.None)]
        public FormatTypeEnum TypeName
        {
            get { return (FormatTypeEnum)base[typeProp]; }
            set { base[typeProp] = value; }
        }

        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public ParameterCollection Parameters
        {
            get
            {
                return (ParameterCollection)base[paramsProp];
            }
        }

        [ConfigurationProperty(ConfigConstants.IconCriteria_Element)]
        public IconCriteriaElement IconCriteria
        {
            get { return (IconCriteriaElement)this[iconCriteriaProp]; }
        }

        /// <summary>
        /// Override the Properties collection and return our custom one.
        /// </summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get { return properties; }
        }
    }

    public enum FormatTypeEnum
    {
        None, // default type
        ColorScale,
        DataBar,
        IconSet,
        DupeUnique,
        ColorFont
    }
}
