namespace River.Orqa.Editor
{
    using System;
    using System.ComponentModel;

    [ToolboxItem(false)]
    public class Resources : Component
    {
        // Methods
        public Resources()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public Resources(IContainer container)
        {
            this.components = null;
            container.Add(this);
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }


        // Fields
        private Container components;
    }
}

