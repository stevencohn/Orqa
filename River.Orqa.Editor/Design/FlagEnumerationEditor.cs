namespace QWhale.Design
{
    using QWhale.Design.Dialogs;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms.Design;

    public class FlagEnumerationEditor : UITypeEditor
    {
        // Methods
        public FlagEnumerationEditor()
        {
            this.service = null;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (((context != null) && (context.Instance != null)) && (provider != null))
            {
                this.service = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
                if (this.service != null)
                {
                    DlgFlagEnumeration enumeration1 = new DlgFlagEnumeration(this);
                    enumeration1.EditValue = value;
                    this.service.DropDownControl(enumeration1.ListBox);
                    value = enumeration1.EditValue;
                    enumeration1.Dispose();
                    this.service = null;
                }
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if ((context != null) && (context.Instance != null))
            {
                return UITypeEditorEditStyle.DropDown;
            }
            return base.GetEditStyle(context);
        }


        // Properties
        public IWindowsFormsEditorService Service
        {
            get
            {
                return this.service;
            }
        }


        // Fields
        private IWindowsFormsEditorService service;
    }
}

