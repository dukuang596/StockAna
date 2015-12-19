using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Ganji.ExcelManagement.Config
{
    public sealed class ViewsElement : ConfigurationElement
    {
        private static ConfigurationPropertyCollection properties;

        private static ConfigurationProperty viewsProp;

        static ViewsElement()
        {
            // Predefine properties here
            viewsProp = new ConfigurationProperty(
                null,
                typeof(ViewCollection),
                null,
                ConfigurationPropertyOptions.IsDefaultCollection
            );

            properties = new ConfigurationPropertyCollection();

            properties.Add(viewsProp);
        }

        public ViewsElement()
        {
        }

        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public ViewCollection All
        {
            get
            {
                return (ViewCollection)base[viewsProp];
            }
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
