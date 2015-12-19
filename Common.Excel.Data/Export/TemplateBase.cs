using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestExcelAddin.Export
{
    public abstract class TemplateBase
    {
        private ExportParameters exportParameters;
        public TemplateBase()
        { }

        public TemplateBase(ExportParameters exParams)
            : base()
        {
            this.exportParameters = exParams;
        }

        public ExportParameters ExportParameters
        {
            get
            {
                return this.exportParameters;
            }
        }

        public abstract List<string> TemplateProperty
        {
            get;
        }
        public int PropertyCount
        {
            get
            {
                if (TemplateProperty != null)
                {
                    return TemplateProperty.Count;
                }
                else
                {
                    return 0;
                }
            }
        }
        public int EditablePropertyCount
        {
            get
            {
                if (EditableTemplateProperty != null)
                {
                    return EditableTemplateProperty.Count;
                }
                else
                {
                    return 0;
                }
            }
        }
        public abstract Dictionary<string, PropertyTag> EditableTemplateProperty
        {
            get;
        }
        public abstract int ObjectPerfStartColumn
        {
            get;
        }
        public void AddProperty(string propertyName, Editable editable, int propertyDigit, List<string> templateProperty, Dictionary<string, PropertyTag> editableProperty)
        {
            templateProperty.Add(propertyName);
            if (editable != Editable.IsFalse)
            {
                editableProperty.Add(propertyName, new PropertyTag() { EditStatus = editable, DigitNumber = propertyDigit });
            }
        }
    }

    public enum Editable
    {
        IsTag,
        IsTracking,
        IsProperty,
        IsImmutable,
        IsParent,
        IsFalse
    }

    public class PropertyTag
    {
        public Editable EditStatus;
        public int DigitNumber;
    }
}
