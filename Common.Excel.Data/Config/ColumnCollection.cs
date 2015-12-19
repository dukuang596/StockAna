using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Ganji.ExcelManagement.Config
{
    public sealed class ColumnCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ColumnElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ColumnElement)element).ColumnName;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return ConfigConstants.Column_Element; }
        }

        public ColumnElement this[int index]
        {
            get { return (ColumnElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        new public ColumnElement this[string columnName]
        {
            get { return (ColumnElement)BaseGet(columnName); }
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

        public ColumnElement FindMatchedColumn(string columnName)
        {
            ColumnElement col = null;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Match(columnName))
                {
                    // assume the first one is the best one
                    col = this[i];
                    break;
                }
            }

            return col;
        }
    }
}
