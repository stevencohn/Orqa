namespace River.Orqa.Editor.Design
{
    using River.Orqa.Editor.Design.Dialogs;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class SyntaxBuilderEditor : UITypeEditor
    {
        // Methods
        public SyntaxBuilderEditor()
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
                    DlgSyntaxBuilder builder1 = new DlgSyntaxBuilder(this);
                    try
                    {
                        builder1.Scheme = (LexScheme) value;
                    }
                    catch
                    {
                        MessageBox.Show("Not implemented: Error loading scheme");
                        builder1.Dispose();
                        this.service = null;
                        return value;
                    }
                    DialogResult result1 = this.service.ShowDialog(builder1);
                    if (result1 == DialogResult.OK)
                    {
                        value = builder1.Scheme;
                    }
                    builder1.Dispose();
                    this.service = null;
                }
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if ((context != null) && (context.Instance != null))
            {
                return UITypeEditorEditStyle.Modal;
            }
            return base.GetEditStyle(context);
        }


        // Fields
        private IWindowsFormsEditorService service;
    }
}

