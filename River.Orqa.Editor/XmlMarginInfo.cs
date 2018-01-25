namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;

    public class XmlMarginInfo
    {
        // Methods
        public XmlMarginInfo()
        {
            this.position = EditConsts.DefaultMarginPosition;
            this.penColor = EditConsts.DefaultMarginForeColor.Name;
            this.penWidth = 1f;
            this.visible = true;
        }

        public XmlMarginInfo(Margin Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(Margin Owner)
        {
            this.owner = Owner;
            this.Position = this.position;
            this.PenColor = this.penColor;
            this.PenWidth = this.penWidth;
            this.Visible = this.visible;
            this.AllowDrag = this.allowDrag;
            this.ShowHints = this.showHints;
        }


        // Properties
        public bool AllowDrag
        {
            get
            {
                if (this.owner == null)
                {
                    return this.allowDrag;
                }
                return this.owner.AllowDrag;
            }
            set
            {
                this.allowDrag = value;
                if (this.owner != null)
                {
                    this.owner.AllowDrag = value;
                }
            }
        }

        public string PenColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.penColor;
                }
                return XmlHelper.SerializeColor(this.owner.Pen.Color);
            }
            set
            {
                this.penColor = value;
                if (this.owner != null)
                {
                    this.owner.Pen.Color = XmlHelper.DeserializeColor(value);
                }
            }
        }

        public float PenWidth
        {
            get
            {
                if (this.owner == null)
                {
                    return this.penWidth;
                }
                return this.owner.Pen.Width;
            }
            set
            {
                this.penWidth = value;
                if (this.owner != null)
                {
                    this.owner.Pen.Width = value;
                }
            }
        }

        public int Position
        {
            get
            {
                if (this.owner == null)
                {
                    return this.position;
                }
                return this.owner.Position;
            }
            set
            {
                this.position = value;
                if (this.owner != null)
                {
                    this.owner.Position = value;
                }
            }
        }

        public bool ShowHints
        {
            get
            {
                if (this.owner == null)
                {
                    return this.showHints;
                }
                return this.owner.ShowHints;
            }
            set
            {
                this.showHints = value;
                if (this.owner != null)
                {
                    this.owner.ShowHints = value;
                }
            }
        }

        public bool Visible
        {
            get
            {
                if (this.owner == null)
                {
                    return this.visible;
                }
                return this.owner.Visible;
            }
            set
            {
                this.visible = value;
                if (this.owner != null)
                {
                    this.owner.Visible = value;
                }
            }
        }


        // Fields
        private bool allowDrag;
        private Margin owner;
        private string penColor;
        private float penWidth;
        private int position;
        private bool showHints;
        private bool visible;
    }
}

