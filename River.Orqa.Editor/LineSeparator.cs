namespace River.Orqa.Editor
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;

    public class LineSeparator : ILineSeparator
    {
        // Methods
        public LineSeparator()
        {
            this.highlightBackColor = EditConsts.DefaultLineSeparatorColor;
            this.highlightForeColor = Color.Empty;
            this.lineColor = EditConsts.DefaultLineSeparatorLineColor;
            this.tempLine = -1;
        }

        public LineSeparator(ISyntaxEdit Owner) : this()
        {
            this.owner = Owner;
        }

        public void Assign(ILineSeparator Source)
        {
            this.BeginUpdate();
            try
            {
                this.Options = Source.Options;
                this.HighlightForeColor = Source.HighlightForeColor;
                this.HighlightBackColor = Source.HighlightBackColor;
                this.LineColor = Source.LineColor;
            }
            finally
            {
                this.EndUpdate();
            }
        }

        protected internal void BeginUpdate()
        {
            this.updateCount++;
        }

        protected internal void EndUpdate()
        {
            this.updateCount--;
            if (this.updateCount == 0)
            {
                this.Update();
            }
        }

        private void InvalidateLine()
        {
            if ((this.owner != null) && (this.tempLine >= 0))
            {
                Region region1 = ((SyntaxEdit) this.owner).GetRectRegion(new Rectangle(0, this.tempLine, 0x7fffffff, this.tempLine));
                if (region1 != null)
                {
                    ((SyntaxEdit) this.owner).Invalidate(region1, false);
                    region1.Dispose();
                }
            }
        }

        public bool NeedHide()
        {
            if ((this.options & SeparatorOptions.HighlightCurrentLine) != SeparatorOptions.None)
            {
                return ((this.options & SeparatorOptions.HideHighlighting) != SeparatorOptions.None);
            }
            return false;
        }

        public bool NeedHighlight()
        {
            if ((this.owner == null) || ((this.options & SeparatorOptions.HighlightCurrentLine) == SeparatorOptions.None))
            {
                return false;
            }
            if ((this.options & SeparatorOptions.HideHighlighting) != SeparatorOptions.None)
            {
                return this.owner.Focused;
            }
            return true;
        }

        protected internal bool NeedHighlightLine(int Index, bool ADisplay)
        {
            if ((this.owner == null) || ((this.options & SeparatorOptions.HighlightCurrentLine) == SeparatorOptions.None))
            {
                goto Label_006C;
            }
            if (ADisplay)
            {
                Point point1 = this.owner.DisplayLines.PointToDisplayPoint(this.owner.Position);
                if (point1.Y == Index)
                {
                    goto Label_0054;
                }
            }
            if (ADisplay || (this.owner.Position.Y != Index))
            {
                goto Label_006C;
            }
        Label_0054:
            if ((this.options & SeparatorOptions.HideHighlighting) != SeparatorOptions.None)
            {
                return this.owner.Focused;
            }
            return true;
        Label_006C:
            return false;
        }

        public virtual void ResetHighlightBackColor()
        {
            this.HighlightBackColor = EditConsts.DefaultLineSeparatorColor;
        }

        public virtual void ResetHighlightForeColor()
        {
            this.HighlightForeColor = Color.Empty;
        }

        public virtual void ResetLineColor()
        {
            this.LineColor = EditConsts.DefaultLineSeparatorLineColor;
        }

        public virtual void ResetOptions()
        {
            this.Options = SeparatorOptions.None;
        }

        public bool ShouldSerializeHighlightBackColor()
        {
            return (this.highlightBackColor != EditConsts.DefaultLineSeparatorColor);
        }

        public bool ShouldSerializeHighlightForeColor()
        {
            return (this.highlightForeColor != Color.Empty);
        }

        public bool ShouldSerializeLineColor()
        {
            return (this.lineColor != EditConsts.DefaultLineSeparatorLineColor);
        }

        public void TempHightlightLine(int Index)
        {
            this.TempLine = Index;
        }

        public void TempUnhightlightLine()
        {
            this.TempLine = -1;
        }

        protected internal void Update()
        {
            if ((this.updateCount == 0) && (this.owner != null))
            {
                this.owner.Invalidate();
            }
        }


        // Properties
        public Color HighlightBackColor
        {
            get
            {
                return this.highlightBackColor;
            }
            set
            {
                if (this.highlightBackColor != value)
                {
                    this.highlightBackColor = value;
                    this.Update();
                }
            }
        }

        public Color HighlightForeColor
        {
            get
            {
                return this.highlightForeColor;
            }
            set
            {
                if (this.highlightForeColor != value)
                {
                    this.highlightForeColor = value;
                    this.Update();
                }
            }
        }

        public Color LineColor
        {
            get
            {
                return this.lineColor;
            }
            set
            {
                if (this.lineColor != value)
                {
                    this.lineColor = value;
                    this.Update();
                }
            }
        }

        [DefaultValue(0), Editor("QWhale.Design.FlagEnumerationEditor, River.Orqa.Editor", typeof(UITypeEditor))]
        public SeparatorOptions Options
        {
            get
            {
                return this.options;
            }
            set
            {
                if (this.options != value)
                {
                    this.options = value;
                    this.Update();
                }
            }
        }

        protected internal int TempLine
        {
            get
            {
                return this.tempLine;
            }
            set
            {
                if (this.tempLine != value)
                {
                    this.InvalidateLine();
                    this.tempLine = value;
                    this.InvalidateLine();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object XmlInfo
        {
            get
            {
                return new XmlLineSeparatorInfo(this);
            }
            set
            {
                ((XmlLineSeparatorInfo) value).FixupReferences(this);
            }
        }


        // Fields
        private Color highlightBackColor;
        private Color highlightForeColor;
        private Color lineColor;
        private SeparatorOptions options;
        private ISyntaxEdit owner;
        private int tempLine;
        private int updateCount;
    }
}

