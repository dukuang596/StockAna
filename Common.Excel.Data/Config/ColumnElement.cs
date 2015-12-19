using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ganji.ExcelManagement.Config
{
    public sealed class ColumnElement : ConfigurationElement
    {
        ///FIXME: use a weak hash map here
        private static readonly IDictionary<string, Regex> res
            = new Dictionary<string, Regex>();
        private static ConfigurationPropertyCollection properties;

        private static ConfigurationProperty nameProp;
        private static ConfigurationProperty exactMatchProp;
        private static ConfigurationProperty dataFormatProp;
        //
        private static ConfigurationProperty DataFormat_CNYProp;
        //
        private static ConfigurationProperty formulaProp;

        //private static ConfigurationProperty indexProp;
        private static ConfigurationProperty validationProp;
        private static ConfigurationProperty formatProp;

        private static ConfigurationProperty autoFitProp;
        private static ConfigurationProperty bgColorProp;
        private static ConfigurationProperty linkProp;
        private static ConfigurationProperty fontColorProp;
        private static ConfigurationProperty columnWidthProp;

        private static ConfigurationProperty necessaryProp;
        private static ConfigurationProperty reportShow;

        private static ConfigurationProperty isLocked;
        private static ConfigurationProperty gradientColorsProp;

        static ColumnElement()
        {
            // Predefine properties here
            nameProp = new ConfigurationProperty(
                ConfigConstants.Name_Property,
                typeof(string),
                string.Empty,
                ConfigurationPropertyOptions.IsRequired
            );
            exactMatchProp = new ConfigurationProperty(
                ConfigConstants.ExactMatch_Property,
                typeof(bool),
                true,
                ConfigurationPropertyOptions.None
            );
            dataFormatProp = new ConfigurationProperty(
                ConfigConstants.DataFormat_Property,
                typeof(string),
                string.Empty,
                ConfigurationPropertyOptions.None
            );
            formulaProp = new ConfigurationProperty(
                ConfigConstants.Formula_Property,
                typeof(string),
                string.Empty,
                ConfigurationPropertyOptions.None
            );

            DataFormat_CNYProp = new ConfigurationProperty(
               ConfigConstants.DataFormat_CNY_Property,
               typeof(string),
               string.Empty,
               ConfigurationPropertyOptions.None
           );

            //indexProp = new ConfigurationProperty(
            //    "index",
            //    typeof(int),
            //    0,
            //    ConfigurationPropertyOptions.None
            //);

            validationProp = new ConfigurationProperty(
                ConfigConstants.Validation_Element,
                typeof(ValidationElement),
                null,
                ConfigurationPropertyOptions.None
            );

            formatProp = new ConfigurationProperty(
                ConfigConstants.FormatCondition_Element,
                typeof(FormatConditionElement),
                null,
                ConfigurationPropertyOptions.None
            );

            autoFitProp = new ConfigurationProperty(
                ConfigConstants.AutoFit_Property,
                typeof(bool),
                true,
                ConfigurationPropertyOptions.None
            );

            bgColorProp = new ConfigurationProperty(
                ConfigConstants.BgColor_Property,
                typeof(string),
                null,
                ConfigurationPropertyOptions.None
            );

            linkProp = new ConfigurationProperty(
                ConfigConstants.Link_Property,
                typeof(string),
                null,
                ConfigurationPropertyOptions.None
            );

            fontColorProp = new ConfigurationProperty(
                ConfigConstants.FontColor_Property,
                typeof(string),
                "0,0,0",
                ConfigurationPropertyOptions.None
                );

            columnWidthProp = new ConfigurationProperty(
                ConfigConstants.ColumnWidth_Property,
                typeof(string),
                null,
                ConfigurationPropertyOptions.None
                );
            necessaryProp = new ConfigurationProperty(
                ConfigConstants.Necessary_Property,
                typeof(bool),
                null,
                ConfigurationPropertyOptions.None
            );

            reportShow = new ConfigurationProperty(
                ConfigConstants.ReportShow_Property,
                typeof(bool),
                null,
                ConfigurationPropertyOptions.None
            );

            isLocked = new ConfigurationProperty(
                ConfigConstants.IsLocked_Property,
                typeof(bool),
                null,
                ConfigurationPropertyOptions.None
            );
            gradientColorsProp = new ConfigurationProperty(
                ConfigConstants.GradientColors_Property,
                typeof(string),
                null,
                ConfigurationPropertyOptions.None
            );

            properties = new ConfigurationPropertyCollection();

            properties.Add(nameProp);
            properties.Add(exactMatchProp);
            properties.Add(dataFormatProp);
            properties.Add(DataFormat_CNYProp);

            properties.Add(formulaProp);
            //properties.Add(indexProp);
            properties.Add(validationProp);
            properties.Add(formatProp);
            properties.Add(autoFitProp);
            properties.Add(bgColorProp);
            properties.Add(linkProp);
            properties.Add(fontColorProp);
            properties.Add(columnWidthProp);
            properties.Add(reportShow);
            properties.Add(necessaryProp);
            properties.Add(isLocked);
            properties.Add(gradientColorsProp);
        }

        public ColumnElement()
        {
        }

        //[ConfigurationProperty("index")]
        //public int ColumnIndex
        //{
        //    get { return (int)base[indexProp]; }
        //    set { base[indexProp] = value; }
        //}

        [ConfigurationProperty(ConfigConstants.Necessary_Property, DefaultValue = false)]
        public bool Necessary
        {
            get { return (bool)base[necessaryProp]; }
            set { base[necessaryProp] = value; }
        }

        [ConfigurationProperty(ConfigConstants.ReportShow_Property, DefaultValue = false)]
        public bool ReportShow
        {
            get { return (bool)base[reportShow]; }
            set { base[reportShow] = value; }
        }

        [ConfigurationProperty(ConfigConstants.IsLocked_Property, DefaultValue = false)]
        public bool IsLocked
        {
            get { return (bool)base[isLocked]; }
            set { base[isLocked] = value; }
        }
        [ConfigurationProperty(ConfigConstants.GradientColors_Property, IsRequired = false)]
        public string GradientColors
        {
            get { return (string)base[gradientColorsProp]; }
            set { base[gradientColorsProp] = value; }
        }

        [ConfigurationProperty(ConfigConstants.Link_Property)]
        public string Link
        {
            get { return (string)base[linkProp]; }
            set { base[linkProp] = value; }
        }

        [ConfigurationProperty(ConfigConstants.Name_Property, IsKey = true, IsRequired = true)]
        public string ColumnName
        {
            get { return (string)base[nameProp]; }
            set { base[nameProp] = value; }
        }

        [ConfigurationProperty(ConfigConstants.ExactMatch_Property, DefaultValue = true)]
        public bool IsExactMatch
        {
            get { return (bool)base[exactMatchProp]; }
            set { base[exactMatchProp] = value; }
        }

        [ConfigurationProperty(ConfigConstants.DataFormat_Property, IsRequired = false)]
        public string DataFormat
        {
            get { return (string)base[dataFormatProp]; }
            set { base[dataFormatProp] = value; }
        }

        [ConfigurationProperty(ConfigConstants.DataFormat_CNY_Property, IsRequired = false)]
        public string DataFormat_CNY
        {
            get { return (string)base[DataFormat_CNYProp]; }
            set { base[DataFormat_CNYProp] = value; }
        }



        [ConfigurationProperty(ConfigConstants.Formula_Property, IsRequired = false)]
        public string Formula
        {
            get { return (string)base[formulaProp]; }
            set { base[formulaProp] = value; }
        }

        [ConfigurationProperty(ConfigConstants.FontColor_Property, IsRequired = false)]
        public string FontColor
        {
            get { return (string)base[fontColorProp]; }
            set { base[fontColorProp] = value; }
        }

        [ConfigurationProperty(ConfigConstants.ColumnWidth_Property, IsRequired = false)]
        public string ColumnWidth
        {
            get { return (string)base[columnWidthProp]; }
            set { base[columnWidthProp] = value; }
        }


        [ConfigurationProperty(ConfigConstants.Validation_Element)]
        public ValidationElement Validation
        {
            get
            {
                return (ValidationElement)base[validationProp];
            }
        }

        [ConfigurationProperty(ConfigConstants.FormatCondition_Element)]
        public FormatConditionElement FormatCondition
        {
            get
            {
                return (FormatConditionElement)base[formatProp];
            }
        }

        [ConfigurationProperty(ConfigConstants.AutoFit_Property, DefaultValue = true)]
        public bool AutoFit
        {
            get { return (bool)base[autoFitProp]; }
            set { base[autoFitProp] = value; }
        }

        [ConfigurationProperty(ConfigConstants.BgColor_Property)]
        public string BackgroundColor
        {
            get { return (string)base[bgColorProp]; }
            set { base[bgColorProp] = value; }
        }

        /// <summary>
        /// Override the Properties collection and return our custom one.
        /// </summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get { return properties; }
        }

        public bool Match(string columnName)
        {
            List<string> names = new List<string>(this.ColumnName.Split('|'));
            if (this.IsExactMatch)
            {
                // return this.ColumnName == columnName;
                return names.Any(col => string.Compare(col, columnName, true) == 0);
            }
            foreach (var name in names)
            {
                string regKey = this.ColumnName;
                if (!res.ContainsKey(regKey))
                {
                    lock (res)
                    {
                        if (!res.ContainsKey(regKey))
                        {
                            res.Add(regKey, new Regex(regKey, RegexOptions.Compiled));
                        }
                    }
                }
                Regex re = res[regKey];
                if (re.IsMatch(name)) return true;

            }

            return false;


        }
        public string GetColumnFormatStyle(string CurrencyCode)
        {
            switch (CurrencyCode)
            {
                case "CNY":
                    return string.IsNullOrEmpty(this.DataFormat_CNY) ? this.DataFormat : this.DataFormat_CNY;
                default:
                    return this.DataFormat;
            }
        }
    }
}
