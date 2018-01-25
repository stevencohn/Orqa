namespace River.Orqa.Editor.Common
{
    using System;
    using System.Windows.Forms;

    public class ErrorHandler
    {
        // Methods
        static ErrorHandler()
        {
            ErrorHandler.errorBehaviour = River.Orqa.Editor.Common.ErrorBehaviour.Message;
        }

        public ErrorHandler()
        {
        }

        public static void Error(Exception e)
        {
            switch (ErrorHandler.errorBehaviour)
            {
                case River.Orqa.Editor.Common.ErrorBehaviour.Message:
                {
                    MessageBox.Show(e.Message, Consts.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                case River.Orqa.Editor.Common.ErrorBehaviour.Exception:
                {
                    throw e;
                }
            }
        }


        // Properties
        public static River.Orqa.Editor.Common.ErrorBehaviour ErrorBehaviour
        {
            get
            {
                return ErrorHandler.errorBehaviour;
            }
            set
            {
                ErrorHandler.errorBehaviour = value;
            }
        }


        // Fields
        private static River.Orqa.Editor.Common.ErrorBehaviour errorBehaviour;
    }
}

