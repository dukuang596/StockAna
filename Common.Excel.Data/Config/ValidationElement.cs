using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Ganji.ExcelManagement.Config
{
    public sealed class ValidationElement : ConfigurationElement
    {
        const string DefaultAlertStyle = "xlValidAlertStop";

        private static ConfigurationPropertyCollection properties;

        private static ConfigurationProperty typeProp;
        private static ConfigurationProperty alertStyleProp;
        private static ConfigurationProperty operatorProp;
        private static ConfigurationProperty formula1Prop;
        private static ConfigurationProperty formula1zhCNProp;
        private static ConfigurationProperty formula2Prop;
        private static ConfigurationProperty inputTitleProp;
        private static ConfigurationProperty inputMsgProp;
        private static ConfigurationProperty errorTitleProp;
        private static ConfigurationProperty errorMsgProp;
        //private static ConfigurationProperty ignoreBlankProp;
        private static ConfigurationProperty showInput;
        private static ConfigurationProperty showError;

        /*
         * <validation type="xlValidateList" alertStyle="xlValidAlertStop" operator="xlBetween" formula1="" formula2=""
                        inputTitle="" inputMessage="" errorTitle="" errorMessage=""/>
         */
        static ValidationElement()
        {
            // Predefine properties here
            typeProp = new ConfigurationProperty(
                ConfigConstants.Type_Property,
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            alertStyleProp = new ConfigurationProperty(
                ConfigConstants.AlertStyle_Property,
                typeof(string),
                DefaultAlertStyle,
                ConfigurationPropertyOptions.None
            );

            operatorProp = new ConfigurationProperty(
                ConfigConstants.Operator_Property,
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            formula1Prop = new ConfigurationProperty(
                ConfigConstants.Formula1_Property,
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            formula1zhCNProp = new ConfigurationProperty(
               ConfigConstants.Formula1_zhCNProperty,
               typeof(string),
               null,
               ConfigurationPropertyOptions.None
            );

            formula2Prop = new ConfigurationProperty(
                ConfigConstants.Formula2_Property,
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            inputTitleProp = new ConfigurationProperty(
                ConfigConstants.InputTitle_Property,
                typeof(string),
                string.Empty,
                ConfigurationPropertyOptions.None
            );

            inputMsgProp = new ConfigurationProperty(
                ConfigConstants.InputMessage_Property,
                typeof(string),
                string.Empty,
                ConfigurationPropertyOptions.None
            );

            errorTitleProp = new ConfigurationProperty(
                ConfigConstants.ErrorTitle_Property,
                typeof(string),
                string.Empty,
                ConfigurationPropertyOptions.None
            );

            errorMsgProp = new ConfigurationProperty(
                ConfigConstants.ErrorMessage_Property,
                typeof(string),
                string.Empty,
                ConfigurationPropertyOptions.None
            );

            showInput = new ConfigurationProperty(
                ConfigConstants.ShowInput_Property,
                typeof(bool),
                true,
                ConfigurationPropertyOptions.None
            );

            showError = new ConfigurationProperty(
                ConfigConstants.ShowError_Property,
                typeof(bool),
                true,
                ConfigurationPropertyOptions.None
            );

            properties = new ConfigurationPropertyCollection();

            properties.Add(typeProp);
            properties.Add(alertStyleProp);
            properties.Add(operatorProp);
            properties.Add(formula1Prop);
            properties.Add(formula1zhCNProp);
            properties.Add(formula2Prop);
            properties.Add(inputTitleProp);
            properties.Add(inputMsgProp);
            properties.Add(errorTitleProp);
            properties.Add(errorMsgProp);
            properties.Add(showInput);
            properties.Add(showError);
        }

        public ValidationElement()
        {
        }

        [ConfigurationProperty(ConfigConstants.Type_Property, IsRequired = true)]
        public string ValidationType
        {
            get { return (string)this[ConfigConstants.Type_Property]; }
            set { this[ConfigConstants.Type_Property] = value; }
        }

        [ConfigurationProperty(ConfigConstants.AlertStyle_Property)]
        public string AlertStyle
        {
            get { return (string)this[ConfigConstants.AlertStyle_Property]; }
            set { this[ConfigConstants.AlertStyle_Property] = value; }
        }

        [ConfigurationProperty(ConfigConstants.Operator_Property)]
        public string Operator
        {
            get { return (string)this[ConfigConstants.Operator_Property]; }
            set { this[ConfigConstants.Operator_Property] = value; }
        }

        [ConfigurationProperty(ConfigConstants.Formula1_Property)]
        public string Formula1
        {
            get { return (string)this[ConfigConstants.Formula1_Property]; }
            set { this[ConfigConstants.Formula1_Property] = value; }
        }

        [ConfigurationProperty(ConfigConstants.Formula1_zhCNProperty)]
        public string Formula1zhCN
        {
            get { return (string)this[ConfigConstants.Formula1_zhCNProperty]; }
            set { this[ConfigConstants.Formula1_zhCNProperty] = value; }
        }

        [ConfigurationProperty(ConfigConstants.Formula2_Property)]
        public string Formula2
        {
            get { return (string)this[ConfigConstants.Formula2_Property]; }
            set { this[ConfigConstants.Formula2_Property] = value; }
        }

        [ConfigurationProperty(ConfigConstants.InputTitle_Property)]
        public string InputTitle
        {
            get { return (string)this[ConfigConstants.InputTitle_Property]; }
            set { this[ConfigConstants.InputTitle_Property] = value; }
        }

        [ConfigurationProperty(ConfigConstants.InputMessage_Property)]
        public string InputMessage
        {
            get { return (string)this[ConfigConstants.InputMessage_Property]; }
            set { this[ConfigConstants.InputMessage_Property] = value; }
        }

        [ConfigurationProperty(ConfigConstants.ErrorTitle_Property)]
        public string ErrorTitle
        {
            get { return (string)this[ConfigConstants.ErrorTitle_Property]; }
            set { this[ConfigConstants.ErrorTitle_Property] = value; }
        }

        [ConfigurationProperty(ConfigConstants.ErrorMessage_Property)]
        public string ErrorMessage
        {
            get { return (string)this[ConfigConstants.ErrorMessage_Property]; }
            set { this[ConfigConstants.ErrorMessage_Property] = value; }
        }

        [ConfigurationProperty(ConfigConstants.ShowInput_Property)]
        public bool ShowInput
        {
            get { return (bool)this[ConfigConstants.ShowInput_Property]; }
            set { this[ConfigConstants.ShowInput_Property] = value; }
        }

        [ConfigurationProperty(ConfigConstants.ShowError_Property)]
        public bool ShowError
        {
            get { return (bool)this[ConfigConstants.ShowError_Property]; }
            set { this[ConfigConstants.ShowError_Property] = value; }
        }

        /// <summary>
        /// Override the Properties collection and return our custom one.
        /// </summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get { return properties; }
        }

        public string GetColumnFormula1Style(string languageCode)
        {
            switch (languageCode.ToLower())
            {
                case "zh-cn":
                    return string.IsNullOrEmpty(this.Formula1zhCN) ? this.Formula1 : this.Formula1zhCN;
                default:
                    return this.Formula1;
            }
        }
    }
}
