using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Ganji.ExcelManagement.Config
{
    public sealed class ExcelSettingsSection : ConfigurationSection
    {
        private static ConfigurationPropertyCollection properties;
        private static ConfigurationProperty viewsProp;

        static ExcelSettingsSection()
        {
            viewsProp = new ConfigurationProperty(
                ConfigConstants.Views_Element,
                typeof(ViewsElement),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            properties = new ConfigurationPropertyCollection();

            properties.Add(viewsProp);
        }

        public ExcelSettingsSection()
        {

        }

        [ConfigurationProperty(ConfigConstants.Views_Element)]
        public ViewsElement Views
        {
            get { return (ViewsElement)base[ConfigConstants.Views_Element]; }
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
